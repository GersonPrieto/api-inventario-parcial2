Imports System.Collections.Generic
Imports ApiInventario.DTOs

Namespace Services
    Public Interface ICategoriaService
        Function ObtenerTodas() As List(Of CategoriaRespuestaDto)
        Function ObtenerPorId(id As Integer) As CategoriaDetalleDto
        Function Crear(dto As CategoriaCrearDto) As CategoriaRespuestaDto
        Function Modificar(id As Integer, dto As CategoriaModificarDto) As CategoriaRespuestaDto
        Sub Eliminar(id As Integer)
    End Interface
End Namespace
