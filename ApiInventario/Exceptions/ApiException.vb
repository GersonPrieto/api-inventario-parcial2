Imports System

Namespace Exceptions
    Public Class ApiException
        Inherits Exception

        Public ReadOnly Property Codigo As Integer

        Public Sub New(codigo As Integer, mensaje As String)
            MyBase.New(mensaje)
            Me.Codigo = codigo
        End Sub
    End Class
End Namespace
