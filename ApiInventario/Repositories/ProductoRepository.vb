Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports ApiInventario.Data
Imports ApiInventario.Models

Namespace Repositories
    Public Class ProductoRepository
        Implements IProductoRepository

        Private ReadOnly _contexto As AppDbContext

        Public Sub New(contexto As AppDbContext)
            _contexto = contexto
        End Sub

        Public Function ObtenerTodos(Optional categoriaId As Integer? = Nothing) As List(Of Producto) _
            Implements IProductoRepository.ObtenerTodos
            Dim consulta = _contexto.Productos.AsNoTracking().Include(Function(p) p.Categoria).AsQueryable()
            If categoriaId.HasValue Then
                Dim id = categoriaId.Value
                consulta = consulta.Where(Function(p) p.CategoriaId = id)
            End If
            Return consulta.OrderBy(Function(p) p.Id).ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As Producto Implements IProductoRepository.ObtenerPorId
            Return _contexto.Productos.Include(Function(p) p.Categoria).SingleOrDefault(Function(p) p.Id = id)
        End Function

        Public Function ExisteNombre(nombre As String, Optional excluirId As Integer = 0) As Boolean _
            Implements IProductoRepository.ExisteNombre
            Return _contexto.Productos.Any(Function(p) p.Id <> excluirId AndAlso p.Nombre.Equals(nombre))
        End Function

        Public Sub Agregar(producto As Producto) Implements IProductoRepository.Agregar
            _contexto.Productos.Add(producto)
            Guardar()
        End Sub

        Public Sub Eliminar(producto As Producto) Implements IProductoRepository.Eliminar
            _contexto.Productos.Remove(producto)
            Guardar()
        End Sub

        Public Sub Guardar() Implements IProductoRepository.Guardar
            _contexto.SaveChanges()
        End Sub
    End Class
End Namespace
