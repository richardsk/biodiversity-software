Public Class ComboItem
    Private _text As String = ""
    Private _value As Object = DBNull.Value

    Sub New(ByVal val As Object, ByVal text As String)
        _value = val
        _text = text
    End Sub

    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Public Property Value() As Object
        Get
            Return _value
        End Get
        Set(ByVal value As Object)
            _value = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return _text
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If _value Is DBNull.Value And obj Is DBNull.Value Then Return True
        If obj Is DBNull.Value Then Return False

        Return _value.Equals(obj)
    End Function
End Class
