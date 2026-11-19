Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports ApiInventario.Data
Imports ApiInventario.Models

Namespace Repositories
    Public Class CategoriaRepository
        Implements ICategoriaRepository

        Private ReadOnly _contexto As AppDbContext

        Public Sub New(contexto As AppDbContext)
            _contexto = contexto
        End Sub

        Public Function ObtenerTodas() As List(Of Categoria) Implements ICategoriaRepository.ObtenerTodas
            Return _contexto.Categorias.AsNoTracking().OrderBy(Function(c) c.Id).ToList()
        End Function

        Public Function ObtenerPorId(id As Integer, Optional incluirProductos As Boolean = False) As Categoria _
            Implements ICategoriaRepository.ObtenerPorId
            Dim consulta As IQueryable(Of Categoria) = _contexto.Categorias
            If incluirProductos Then consulta = consulta.Include(Function(c) c.Productos)
            Return consulta.SingleOrDefault(Function(c) c.Id = id)
        End Function

        Public Function ExisteNombre(nombre As String, Optional excluirId As Integer = 0) As Boolean _
            Implements ICategoriaRepository.ExisteNombre
            Return _contexto.Categorias.Any(Function(c) c.Id <> excluirId AndAlso c.Nombre.Equals(nombre))
        End Function

        Public Function TieneProductos(id As Integer) As Boolean Implements ICategoriaRepository.TieneProductos
            Return _contexto.Productos.Any(Function(p) p.CategoriaId = id)
        End Function

        Public Sub Agregar(categoria As Categoria) Implements ICategoriaRepository.Agregar
            _contexto.Categorias.Add(categoria)
            Guardar()
        End Sub

        Public Sub Eliminar(categoria As Categoria) Implements ICategoriaRepository.Eliminar
            _contexto.Categorias.Remove(categoria)
            Guardar()
        End Sub

        Public Sub Guardar() Implements ICategoriaRepository.Guardar
            _contexto.SaveChanges()
        End Sub
    End Class
End Namespace
