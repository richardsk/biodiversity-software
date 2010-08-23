Public Class RDFConcept
    Public Id As String
    Public Name As String
    Public NS As String
    Public FullPath As String


    Public Sub New(ByVal id As String, ByVal name As String, ByVal ns As String, ByVal fullPath As String)
        Me.Id = id
        Me.Name = name
        Me.NS = ns
        Me.FullPath = fullPath
    End Sub

    Public Overrides Function ToString() As String
        Return NS + Name
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is DBNull.Value Then Return False

        Dim c2 As RDFConcept = obj
        Return (c2.Id = Id And c2.Name = Name And c2.NS = NS And c2.FullPath = FullPath)
    End Function

End Class
