Imports System.Collections.Generic
Imports System.Linq
Imports ApiInventario.DTOs
Imports ApiInventario.Exceptions
Imports ApiInventario.Models
Imports ApiInventario.Repositories

Namespace Services
    Public Class CategoriaService
        Implements ICategoriaService

        Private ReadOnly _categorias As ICategoriaRepository

        Public Sub New(categorias As ICategoriaRepository)
            _categorias = categorias
        End Sub

        Public Function ObtenerTodas() As List(Of CategoriaRespuestaDto) Implements ICategoriaService.ObtenerTodas
            Return _categorias.ObtenerTodas().Select(Function(c) MapeoDtos.CategoriaSimple(c)).ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As CategoriaDetalleDto Implements ICategoriaService.ObtenerPorId
            Validaciones.Identificador(id)
            Dim entidad = _categorias.ObtenerPorId(id, incluirProductos:=True)
            If entidad Is Nothing Then Throw New ApiException(404, "No se encontró la categoría.")
            Return MapeoDtos.CategoriaCompleta(entidad)
        End Function

        Public Function Crear(dto As CategoriaCrearDto) As CategoriaRespuestaDto Implements ICategoriaService.Crear
            Validaciones.Entrada(dto)
            Dim nombre = dto.Nombre.Trim()
            If _categorias.ExisteNombre(nombre) Then
                Throw New ApiException(409, "Ya existe una categoría con ese nombre.")
            End If
            Dim entidad As New Categoria With {.Nombre = nombre, .Descripcion = Validaciones.Descripcion(dto.Descripcion)}
            _categorias.Agregar(entidad)
            Return MapeoDtos.CategoriaSimple(entidad)
        End Function

        Public Function Modificar(id As Integer, dto As CategoriaModificarDto) As CategoriaRespuestaDto _
            Implements ICategoriaService.Modificar
            Validaciones.Identificador(id)
            Validaciones.Entrada(dto)
            Dim entidad = _categorias.ObtenerPorId(id)
            If entidad Is Nothing Then Throw New ApiException(404, "No se encontró la categoría.")
            Dim nombre = dto.Nombre.Trim()
            If _categorias.ExisteNombre(nombre, excluirId:=id) Then
                Throw New ApiException(409, "Ya existe otra categoría con ese nombre.")
            End If
            entidad.Nombre = nombre
            entidad.Descripcion = Validaciones.Descripcion(dto.Descripcion)
            _categorias.Guardar()
            Return MapeoDtos.CategoriaSimple(entidad)
        End Function

        Public Sub Eliminar(id As Integer) Implements ICategoriaService.Eliminar
            Validaciones.Identificador(id)
            Dim entidad = _categorias.ObtenerPorId(id)
            If entidad Is Nothing Then Throw New ApiException(404, "No se encontró la categoría.")
            If _categorias.TieneProductos(id) Then
                Throw New ApiException(409, "No se puede eliminar una categoría que tiene productos asociados.")
            End If
            _categorias.Eliminar(entidad)
        End Sub
    End Class
End Namespace
