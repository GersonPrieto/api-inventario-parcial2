Imports System.Collections.Generic
Imports Microsoft.AspNetCore.Mvc
Imports ApiInventario.DTOs
Imports ApiInventario.Services

Namespace Controllers
    <ApiController>
    <Route("api/productos")>
    <Produces("application/json")>
    <ProducesResponseType(GetType(ErrorRespuestaDto), 500)>
    Public Class ProductosController
        Inherits ControllerBase

        Private ReadOnly _servicio As IProductoService

        Public Sub New(servicio As IProductoService)
            _servicio = servicio
        End Sub

        ''' <summary>Consultar productos con los datos de su categoría.</summary>
        <HttpGet>
        <ProducesResponseType(GetType(List(Of ProductoRespuestaDto)), 200)>
        Public Function ObtenerTodos() As IActionResult
            Return Ok(_servicio.ObtenerTodos())
        End Function

        ''' <summary>Consultar un producto por identificador, incluyendo su categoría.</summary>
        <HttpGet("{id:int}")>
        <ProducesResponseType(GetType(ProductoRespuestaDto), 200)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        Public Function ObtenerPorId(id As Integer) As IActionResult
            Return Ok(_servicio.ObtenerPorId(id))
        End Function

        ''' <summary>Consultar productos de una categoría. Devuelve una lista vacía si la categoría existe y no tiene productos.</summary>
        <HttpGet("categoria/{categoriaId:int}")>
        <ProducesResponseType(GetType(List(Of ProductoRespuestaDto)), 200)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        Public Function ObtenerPorCategoria(categoriaId As Integer) As IActionResult
            Return Ok(_servicio.ObtenerPorCategoria(categoriaId))
        End Function

        ''' <summary>Registrar un producto. El identificador se genera automáticamente.</summary>
        <HttpPost>
        <ProducesResponseType(GetType(ProductoRespuestaDto), 201)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 409)>
        Public Function Crear(<FromBody> dto As ProductoCrearDto) As IActionResult
            Dim creado = _servicio.Crear(dto)
            Return CreatedAtAction(NameOf(ObtenerPorId), New With {.id = creado.Id}, creado)
        End Function

        ''' <summary>Modificar un producto y devolver los datos actualizados.</summary>
        <HttpPut("{id:int}")>
        <ProducesResponseType(GetType(ProductoRespuestaDto), 200)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 409)>
        Public Function Modificar(id As Integer, <FromBody> dto As ProductoModificarDto) As IActionResult
            Return Ok(_servicio.Modificar(id, dto))
        End Function

        ''' <summary>Eliminar un producto.</summary>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(204)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        Public Function Eliminar(id As Integer) As IActionResult
            _servicio.Eliminar(id)
            Return NoContent()
        End Function
    End Class
End Namespace
