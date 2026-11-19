Imports System
Imports System.Threading.Tasks
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging
Imports ApiInventario.DTOs
Imports ApiInventario.Exceptions

Namespace Middleware
    Public Class ExceptionMiddleware
        Private ReadOnly _siguiente As RequestDelegate
        Private ReadOnly _logger As ILogger(Of ExceptionMiddleware)

        Public Sub New(siguiente As RequestDelegate, logger As ILogger(Of ExceptionMiddleware))
            _siguiente = siguiente
            _logger = logger
        End Sub

        Public Async Function InvokeAsync(contexto As HttpContext) As Task
            Dim respuesta As ErrorRespuestaDto = Nothing
            Try
                Await _siguiente(contexto)
            Catch ex As Exception
                If contexto.Response.HasStarted Then Throw

                Dim codigo = StatusCodes.Status500InternalServerError
                Dim mensaje = "Ocurrió un error inesperado. Intente nuevamente."
                Dim errorApi = TryCast(ex, ApiException)
                Dim errorSql = TryCast(ex.GetBaseException(), SqlException)

                If errorApi IsNot Nothing Then
                    codigo = errorApi.Codigo
                    mensaje = errorApi.Message
                ElseIf TypeOf ex Is DbUpdateException AndAlso errorSql IsNot Nothing Then
                    Select Case errorSql.Number
                        Case 2601, 2627
                            codigo = StatusCodes.Status409Conflict
                            mensaje = "Ya existe un registro con ese nombre."
                        Case 547
                            codigo = StatusCodes.Status409Conflict
                            mensaje = "La operación entra en conflicto con los datos relacionados o sus restricciones."
                    End Select
                ElseIf TypeOf ex Is BadHttpRequestException Then
                    codigo = StatusCodes.Status400BadRequest
                    mensaje = "La solicitud enviada no es válida."
                End If

                If codigo = 500 Then
                    _logger.LogError(ex, "Error inesperado. TraceId: {TraceId}", contexto.TraceIdentifier)
                End If
                respuesta = New ErrorRespuestaDto With {
                    .Codigo = codigo, .Mensaje = mensaje, .TraceId = contexto.TraceIdentifier
                }
            End Try

            ' En VB, Await debe estar fuera del bloque Catch.
            If respuesta IsNot Nothing Then
                contexto.Response.Clear()
                contexto.Response.StatusCode = respuesta.Codigo
                Await contexto.Response.WriteAsJsonAsync(respuesta)
            End If
        End Function
    End Class
End Namespace
