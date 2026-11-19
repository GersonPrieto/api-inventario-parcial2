Imports System.Collections.Generic

Namespace DTOs
    Public Class ErrorRespuestaDto
        Public Property Codigo As Integer
        Public Property Mensaje As String
        Public Property TraceId As String
        Public Property Errores As Dictionary(Of String, String())
    End Class
End Namespace
