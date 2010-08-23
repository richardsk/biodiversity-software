Imports System.Xml

Public Class GenericHTTPControl

    Private Sub ResolveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResolveButton.Click
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim q As New RDFClasses.Query.GenericQuerier

            Dim res As String = q.ResolveGenericHTTP(uriText.Text, contentTypeCombo.Text)

            resultsText.Text = res

            Windows.Forms.Cursor.Current = Cursors.Default

        Catch ex As Exception
            MsgBox("Failed to resolve Id : " + ex.Message + " : " + ex.StackTrace)
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

    Private Sub GenericHTTPControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        contentTypeCombo.Items.Add("application/json")
        contentTypeCombo.Items.Add("application/csv")
        contentTypeCombo.Items.Add("application/rdf+xml")

        contentTypeCombo.SelectedIndex = 0
    End Sub
End Class
