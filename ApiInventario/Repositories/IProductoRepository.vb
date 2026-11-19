Imports System.Collections.Generic
Imports ApiInventario.Models

Namespace Repositories
    Public Interface IProductoRepository
        Function ObtenerTodos(Optional categoriaId As Integer? = Nothing) As List(Of Producto)
        Function ObtenerPorId(id As Integer) As Producto
        Function ExisteNombre(nombre As String, Optional excluirId As Integer = 0) As Boolean
        Sub Agregar(producto As Producto)
        Sub Eliminar(producto As Producto)
        Sub Guardar()
    End Interface
End Namespace
