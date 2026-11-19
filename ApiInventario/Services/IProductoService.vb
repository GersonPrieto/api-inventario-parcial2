Imports System.Collections.Generic
Imports ApiInventario.DTOs

Namespace Services
    Public Interface IProductoService
        Function ObtenerTodos() As List(Of ProductoRespuestaDto)
        Function ObtenerPorId(id As Integer) As ProductoRespuestaDto
        Function ObtenerPorCategoria(categoriaId As Integer) As List(Of ProductoRespuestaDto)
        Function Crear(dto As ProductoCrearDto) As ProductoRespuestaDto
        Function Modificar(id As Integer, dto As ProductoModificarDto) As ProductoRespuestaDto
        Sub Eliminar(id As Integer)
    End Interface
End Namespace
