Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Helper
    Module MetroHelper
        <Extension> _
        Public Function IsNullColor(ByVal asColor As Color) As Boolean
            If asColor = Color.Empty Or asColor = Color.Transparent Then
                Return True
            End If
            Return False
        End Function
    End Module
End Namespace
