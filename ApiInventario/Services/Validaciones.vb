Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports ApiInventario.Exceptions

Namespace Services
    Friend Module Validaciones
        Public Sub Identificador(id As Integer)
            If id <= 0 Then Throw New ApiException(400, "El identificador debe ser mayor que cero.")
        End Sub

        Public Sub Entrada(dto As Object)
            If dto Is Nothing Then Throw New ApiException(400, "Debe enviar los datos de la operación.")
            Dim errores As New List(Of ValidationResult)()
            If Not Validator.TryValidateObject(dto, New ValidationContext(dto), errores, True) Then
                Throw New ApiException(400, String.Join(" ", errores.Select(Function(e) e.ErrorMessage)))
            End If
        End Sub

        Public Function Descripcion(valor As String) As String
            Return If(String.IsNullOrWhiteSpace(valor), Nothing, valor.Trim())
        End Function
    End Module
End Namespace
