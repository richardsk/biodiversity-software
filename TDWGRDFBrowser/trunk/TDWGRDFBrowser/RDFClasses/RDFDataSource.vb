Public Class RDFDataSource

    Public DataSourceId As Integer = -1
    Public Name As String = ""
    Public Url As String = ""
    Public Include As Boolean = False
    Public QuerierClass As String = ""

    Sub New()
    End Sub

    Sub New(ByVal id As Integer, ByVal name As String, ByVal url As String, ByVal include As Boolean, ByVal querierClass As String)
        Me.DataSourceId = id
        Me.Name = name
        Me.Url = url
        Me.Include = include
        Me.QuerierClass = querierClass
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Sub Load(ByVal id As Integer)
        Dim ds As DataSet = DataAccess.DataSource.GetDataSource(id)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            Dim row As DataRow = ds.Tables(0).Rows(0)
            Me.DataSourceId = id
            Me.Name = row("Name").ToString
            Me.Url = row("Url").ToString
            Me.Include = row("Include")
            Me.QuerierClass = row("QuerierClass").ToString
        End If
    End Sub

End Class
