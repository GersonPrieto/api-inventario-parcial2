Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations

Namespace DTOs
    Public MustInherit Class ProductoEntradaDto
        Implements IValidatableObject

        <Required(ErrorMessage:="El nombre del producto es obligatorio.")>
        <StringLength(50, ErrorMessage:="El nombre del producto admite hasta 50 caracteres.")>
        Public Property Nombre As String

        <StringLength(200, ErrorMessage:="La descripción admite hasta 200 caracteres.")>
        Public Property Descripcion As String

        <Required(ErrorMessage:="El precio es obligatorio.")>
        <Range(GetType(Decimal), "0.01", "9999999999999999.99",
               ParseLimitsInInvariantCulture:=True,
               ErrorMessage:="El precio debe ser mayor que cero y no superar 9999999999999999.99.")>
        Public Property Precio As Decimal?

        <Required(ErrorMessage:="El stock es obligatorio.")>
        <Range(0, Integer.MaxValue, ErrorMessage:="El stock no puede ser negativo.")>
        Public Property Stock As Integer?

        <Required(ErrorMessage:="La categoría es obligatoria.")>
        <Range(1, Integer.MaxValue, ErrorMessage:="El identificador de categoría debe ser mayor que cero.")>
        Public Property CategoriaId As Integer?

        Public Iterator Function Validate(context As ValidationContext) As IEnumerable(Of ValidationResult) _
            Implements IValidatableObject.Validate

            If Precio.HasValue AndAlso Decimal.Round(Precio.Value, 2) <> Precio.Value Then
                Yield New ValidationResult("El precio admite hasta dos decimales.", {NameOf(Precio)})
            End If
        End Function
    End Class
End Namespace
