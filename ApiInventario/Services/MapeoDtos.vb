Imports System
Imports System.Linq
Imports ApiInventario.DTOs
Imports ApiInventario.Models

Namespace Services
    Friend Module MapeoDtos
        Public Function CategoriaSimple(entidad As Categoria) As CategoriaRespuestaDto
            Return New CategoriaRespuestaDto With {
                .Id = entidad.Id, .Nombre = entidad.Nombre, .Descripcion = entidad.Descripcion
            }
        End Function

        Public Function CategoriaCompleta(entidad As Categoria) As CategoriaDetalleDto
            Return New CategoriaDetalleDto With {
                .Id = entidad.Id, .Nombre = entidad.Nombre, .Descripcion = entidad.Descripcion,
                .Productos = entidad.Productos.OrderBy(Function(p) p.Id).
                    Select(Function(p) ProductoSimple(p)).ToList()
            }
        End Function

        Public Function ProductoSimple(entidad As Producto) As ProductoResumenDto
            Return New ProductoResumenDto With {
                .Id = entidad.Id, .Nombre = entidad.Nombre, .Descripcion = entidad.Descripcion,
                .Precio = entidad.Precio, .Stock = entidad.Stock,
                .FechaRegistro = DateTime.SpecifyKind(entidad.FechaRegistro, DateTimeKind.Utc), .CategoriaId = entidad.CategoriaId
            }
        End Function

        Public Function ProductoCompleto(entidad As Producto) As ProductoRespuestaDto
            Return New ProductoRespuestaDto With {
                .Id = entidad.Id, .Nombre = entidad.Nombre, .Descripcion = entidad.Descripcion,
                .Precio = entidad.Precio, .Stock = entidad.Stock,
                .FechaRegistro = DateTime.SpecifyKind(entidad.FechaRegistro, DateTimeKind.Utc), .CategoriaId = entidad.CategoriaId,
                .Categoria = CategoriaSimple(entidad.Categoria)
            }
        End Function
    End Module
End Namespace
