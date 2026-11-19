Imports System.Collections.Generic

Namespace DTOs
    Public Class CategoriaDetalleDto
        Inherits CategoriaRespuestaDto
        Public Property Productos As List(Of ProductoResumenDto) = New List(Of ProductoResumenDto)()
    End Class
End Namespace
