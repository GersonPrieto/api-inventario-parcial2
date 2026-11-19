Imports System.Collections.Generic
Imports ApiInventario.Models

Namespace Repositories
    Public Interface ICategoriaRepository
        Function ObtenerTodas() As List(Of Categoria)
        Function ObtenerPorId(id As Integer, Optional incluirProductos As Boolean = False) As Categoria
        Function ExisteNombre(nombre As String, Optional excluirId As Integer = 0) As Boolean
        Function TieneProductos(id As Integer) As Boolean
        Sub Agregar(categoria As Categoria)
        Sub Eliminar(categoria As Categoria)
        Sub Guardar()
    End Interface
End Namespace
