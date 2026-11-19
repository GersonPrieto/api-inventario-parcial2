Imports System.ComponentModel.DataAnnotations

Namespace DTOs
    Public MustInherit Class CategoriaEntradaDto
        <Required(ErrorMessage:="El nombre de la categoría es obligatorio.")>
        <StringLength(50, ErrorMessage:="El nombre de la categoría admite hasta 50 caracteres.")>
        Public Property Nombre As String

        <StringLength(200, ErrorMessage:="La descripción admite hasta 200 caracteres.")>
        Public Property Descripcion As String
    End Class
End Namespace
