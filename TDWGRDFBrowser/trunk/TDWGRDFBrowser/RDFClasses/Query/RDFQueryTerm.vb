
Namespace Query

    Public Enum QueryAndOrEnum
        _And
        _Or
        _None
    End Enum

    Public Class RDFQueryTerm
        Public Field As String 'full namespaced property name
        Public Value As String 'search value
        Public Exact As Boolean = False
        Public AndOr As QueryAndOrEnum = QueryAndOrEnum._None

    End Class

End Namespace
