Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Migrations
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.OpenApi
Imports ApiInventario.Data
Imports ApiInventario.DTOs
Imports ApiInventario.Middleware
Imports ApiInventario.Repositories
Imports ApiInventario.Services

Public Class Program
    Public Shared Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)
        Dim cadena = builder.Configuration.GetConnectionString("InventarioDB")
        If String.IsNullOrWhiteSpace(cadena) Then
            Throw New InvalidOperationException("Falta la cadena de conexión InventarioDB.")
        End If

        builder.Services.AddDbContext(Of AppDbContext)(Sub(opciones) opciones.UseSqlServer(cadena))
        builder.Services.AddScoped(Of ICategoriaRepository, CategoriaRepository)()
        builder.Services.AddScoped(Of IProductoRepository, ProductoRepository)()
        builder.Services.AddScoped(Of ICategoriaService, CategoriaService)()
        builder.Services.AddScoped(Of IProductoService, ProductoService)()

        builder.Services.AddControllers().ConfigureApiBehaviorOptions(
            Sub(opciones)
                opciones.InvalidModelStateResponseFactory =
                    Function(contexto)
                        Dim errores As New Dictionary(Of String, String())()
                        For Each campo In contexto.ModelState
                            If campo.Value.Errors.Count > 0 Then
                                errores(campo.Key) = campo.Value.Errors.Select(
                                    Function(e) If(e.Exception IsNot Nothing OrElse String.IsNullOrWhiteSpace(e.ErrorMessage),
                                                  "El valor enviado no es válido.", e.ErrorMessage)).ToArray()
                            End If
                        Next
                        Return New BadRequestObjectResult(New ErrorRespuestaDto With {
                            .Codigo = 400, .Mensaje = "Revise los datos enviados.",
                            .TraceId = contexto.HttpContext.TraceIdentifier, .Errores = errores
                        })
                    End Function
            End Sub)

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen(
            Sub(opciones)
                opciones.SwaggerDoc("v1", New OpenApiInfo With {
                    .Title = "API de Inventario", .Version = "v1",
                    .Description = "Administración de categorías y productos. Segundo parcial - Visual Basic .NET."
                })
                Dim xml = Path.Combine(AppContext.BaseDirectory, "ApiInventario.xml")
                If File.Exists(xml) Then opciones.IncludeXmlComments(xml)
            End Sub)

        Dim app = builder.Build()

        ' Estas opciones son comandos locales, no endpoints accesibles por HTTP.
        If args.Contains("--migrar") OrElse args.Contains("--verificar-modelo") OrElse args.Contains("--generar-sql") Then
            Using scope = app.Services.CreateScope()
                Dim contexto = scope.ServiceProvider.GetRequiredService(Of AppDbContext)()
                If contexto.Database.HasPendingModelChanges() Then
                    Throw New InvalidOperationException("El modelo no coincide con la migración. Revise los archivos antes de continuar.")
                End If
                If args.Contains("--verificar-modelo") Then
                    Console.WriteLine("Modelo y snapshot coinciden. Migraciones: " & String.Join(", ", contexto.Database.GetMigrations()))
                ElseIf args.Contains("--generar-sql") Then
                    Dim migrador = contexto.GetService(Of IMigrator)()
                    Dim sql = migrador.GenerateScript(options:=MigrationsSqlGenerationOptions.Idempotent)
                    Dim directorio = Path.Combine(Directory.GetCurrentDirectory(), "sql")
                    Directory.CreateDirectory(directorio)
                    Dim archivo = Path.Combine(directorio, "InventarioDB.sql")
                    Dim encabezado = "IF DB_ID(N'InventarioDB') IS NULL" & Environment.NewLine &
                        "    EXEC(N'CREATE DATABASE [InventarioDB]');" & Environment.NewLine &
                        "GO" & Environment.NewLine & "USE [InventarioDB];" & Environment.NewLine & "GO" & Environment.NewLine
                    File.WriteAllText(archivo, encabezado & sql)
                    Console.WriteLine("Script generado: " & archivo)
                Else
                    contexto.Database.Migrate()
                    Console.WriteLine("Base de datos InventarioDB preparada correctamente.")
                End If
            End Using
            Return
        End If

        app.UseMiddleware(Of ExceptionMiddleware)()
        app.UseSwagger()
        app.UseSwaggerUI()
        Dim inicio As Func(Of String) = Function() "API de inventario en funcionamiento. Abrí /swagger para realizar las pruebas."
        app.MapGet("/", inicio)
        app.MapControllers()
        app.Run("http://localhost:5082")
    End Sub
End Class
