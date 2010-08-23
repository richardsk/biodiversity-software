Imports System.Xml

Public Class RDFSourceForm

    Public RDFDocument As XmlDocument

    Private Sub RDFSourceForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim ms As New IO.MemoryStream()
            Dim wr As New XmlTextWriter(New IO.StreamWriter(ms))
            wr.Formatting = Formatting.Indented
            wr.Indentation = 4

            RDFDocument.WriteTo(wr)
            wr.Flush()

            ms.Position = 0
            RDFSourceText.Text = New IO.StreamReader(ms).ReadToEnd()
            RDFSourceText.Select(0, -1)

        Catch ex As Exception
            MsgBox("Error loading RDF : " + ex.Message)
        End Try
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

End Class