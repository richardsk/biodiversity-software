Public Class NullableBoolean

    Private _val As Integer = -1

    Public Property Value() As Boolean
        Get
            Return (_val = 1)
        End Get
        Set(ByVal val As Boolean)
            If val Then
                _val = 1
            Else
                _val = 0
            End If
        End Set
    End Property

    Public Function IsNull() As Boolean
        Return (_val = -1)
    End Function

    Public Sub SetNull()
        _val = -1
    End Sub

    Public Overrides Function ToString() As String
        If _val = -1 Then Return ""
        If _val = 1 Then Return "true"
        Return "false"
    End Function

End Class
