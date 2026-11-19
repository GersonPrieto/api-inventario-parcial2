Imports System.Collections.Generic
Imports Microsoft.AspNetCore.Mvc
Imports ApiInventario.DTOs
Imports ApiInventario.Services

Namespace Controllers
    <ApiController>
    <Route("api/categorias")>
    <Produces("application/json")>
    <ProducesResponseType(GetType(ErrorRespuestaDto), 500)>
    Public Class CategoriasController
        Inherits ControllerBase

        Private ReadOnly _servicio As ICategoriaService

        Public Sub New(servicio As ICategoriaService)
            _servicio = servicio
        End Sub

        ''' <summary>Consultar categorias.</summary>
        <HttpGet>
        <ProducesResponseType(GetType(List(Of CategoriaRespuestaDto)), 200)>
        Public Function ObtenerTodas() As IActionResult
            Return Ok(_servicio.ObtenerTodas())
        End Function

        ''' <summary>Consultar una categoría incluyendo sus productos.</summary>
        <HttpGet("{id:int}")>
        <ProducesResponseType(GetType(CategoriaDetalleDto), 200)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        Public Function ObtenerPorId(id As Integer) As IActionResult
            Return Ok(_servicio.ObtenerPorId(id))
        End Function

        ''' <summary>Registrar una categoría. El identificador se genera automáticamente.</summary>
        <HttpPost>
        <ProducesResponseType(GetType(CategoriaRespuestaDto), 201)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 409)>
        Public Function Crear(<FromBody> dto As CategoriaCrearDto) As IActionResult
            Dim creado = _servicio.Crear(dto)
            Return CreatedAtAction(NameOf(ObtenerPorId), New With {.id = creado.Id}, creado)
        End Function

        ''' <summary>Modificar una categoría y devolver los datos actualizados.</summary>
        <HttpPut("{id:int}")>
        <ProducesResponseType(GetType(CategoriaRespuestaDto), 200)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 409)>
        Public Function Modificar(id As Integer, <FromBody> dto As CategoriaModificarDto) As IActionResult
            Return Ok(_servicio.Modificar(id, dto))
        End Function

        ''' <summary>Eliminar una categoría solamente si no tiene productos asociados.</summary>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(204)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 400)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 404)>
        <ProducesResponseType(GetType(ErrorRespuestaDto), 409)>
        Public Function Eliminar(id As Integer) As IActionResult
            _servicio.Eliminar(id)
            Return NoContent()
        End Function
    End Class
End Namespace
