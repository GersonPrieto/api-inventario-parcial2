Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports ApiInventario.DTOs
Imports ApiInventario.Exceptions
Imports ApiInventario.Models
Imports ApiInventario.Repositories

Namespace Services
    Public Class ProductoService
        Implements IProductoService

        Private ReadOnly _productos As IProductoRepository
        Private ReadOnly _categorias As ICategoriaRepository

        Public Sub New(productos As IProductoRepository, categorias As ICategoriaRepository)
            _productos = productos
            _categorias = categorias
        End Sub

        Public Function ObtenerTodos() As List(Of ProductoRespuestaDto) Implements IProductoService.ObtenerTodos
            Return _productos.ObtenerTodos().Select(Function(p) MapeoDtos.ProductoCompleto(p)).ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As ProductoRespuestaDto Implements IProductoService.ObtenerPorId
            Validaciones.Identificador(id)
            Return MapeoDtos.ProductoCompleto(BuscarProducto(id))
        End Function

        Public Function ObtenerPorCategoria(categoriaId As Integer) As List(Of ProductoRespuestaDto) _
            Implements IProductoService.ObtenerPorCategoria
            Validaciones.Identificador(categoriaId)
            BuscarCategoria(categoriaId)
            Return _productos.ObtenerTodos(categoriaId).Select(Function(p) MapeoDtos.ProductoCompleto(p)).ToList()
        End Function

        Public Function Crear(dto As ProductoCrearDto) As ProductoRespuestaDto Implements IProductoService.Crear
            Validaciones.Entrada(dto)
            ValidarStock(dto.Stock.Value)
            Dim categoria = BuscarCategoria(dto.CategoriaId.Value)
            Dim nombre = dto.Nombre.Trim()
            If _productos.ExisteNombre(nombre) Then
                Throw New ApiException(409, "Ya existe un producto con ese nombre.")
            End If
            Dim entidad As New Producto With {
                .Nombre = nombre, .Descripcion = Validaciones.Descripcion(dto.Descripcion),
                .Precio = dto.Precio.Value, .Stock = dto.Stock.Value,
                .FechaRegistro = DateTime.UtcNow,
                .CategoriaId = categoria.Id, .Categoria = categoria
            }
            _productos.Agregar(entidad)
            Return MapeoDtos.ProductoCompleto(entidad)
        End Function

        Public Function Modificar(id As Integer, dto As ProductoModificarDto) As ProductoRespuestaDto _
            Implements IProductoService.Modificar
            Validaciones.Identificador(id)
            Validaciones.Entrada(dto)
            ValidarStock(dto.Stock.Value)
            Dim entidad = BuscarProducto(id)
            Dim categoria = BuscarCategoria(dto.CategoriaId.Value)
            Dim nombre = dto.Nombre.Trim()
            If _productos.ExisteNombre(nombre, excluirId:=id) Then
                Throw New ApiException(409, "Ya existe otro producto con ese nombre.")
            End If
            entidad.Nombre = nombre
            entidad.Descripcion = Validaciones.Descripcion(dto.Descripcion)
            entidad.Precio = dto.Precio.Value
            entidad.Stock = dto.Stock.Value
            entidad.CategoriaId = categoria.Id
            entidad.Categoria = categoria
            _productos.Guardar()
            Return MapeoDtos.ProductoCompleto(entidad)
        End Function

        Public Sub Eliminar(id As Integer) Implements IProductoService.Eliminar
            Validaciones.Identificador(id)
            _productos.Eliminar(BuscarProducto(id))
        End Sub

        Private Function BuscarProducto(id As Integer) As Producto
            Dim entidad = _productos.ObtenerPorId(id)
            If entidad Is Nothing Then Throw New ApiException(404, "No se encontró el producto.")
            Return entidad
        End Function

        Private Function BuscarCategoria(id As Integer) As Categoria
            Dim entidad = _categorias.ObtenerPorId(id)
            If entidad Is Nothing Then Throw New ApiException(404, "La categoría indicada no existe.")
            Return entidad
        End Function

        Private Shared Sub ValidarStock(stock As Integer)
            If stock < 0 Then Throw New ApiException(400, "El stock no puede ser negativo.")
        End Sub
    End Class
End Namespace
