Imports System.Xml

Public Class ResolveControl

    Private Sub ResolveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResolveButton.Click
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim rdf As XmlDocument

            If LSIDRadio.Checked Then
                If RDFClasses.Utility.IsLSID(lsidText.Text) Then
                    rdf = RDFClasses.Query.QueryController.ResolveLSID(lsidText.Text)
                Else
                    rdf = RDFClasses.Query.QueryController.ResolveUri(lsidText.Text)
                End If
            Else
                Dim row As DataRowView = ModelCombo.SelectedItem
                Dim m As New RDFClasses.RDFModel(row("ModelId").ToString, row("Name").ToString, row("Url").ToString, row("Type").ToString, row("QueryTemplateUrl").ToString, row("GUIDElement").ToString, row("HarvestTemplateUrl").ToString)
                rdf = RDFClasses.Query.QueryController.ResolveID(m, lsidText.Text)
            End If

            Windows.Forms.Cursor.Current = Cursors.Default

            DetailsControl1.RdfDocument = rdf

        Catch ex As Exception
            MsgBox("Failed to resolve Id : " + ex.Message + " : " + ex.StackTrace)
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

    Private Sub ResolveControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not DesignMode Then
            Try
                Dim models As DataSet = DataAccess.RDFModel.ListRDFModels()
                ModelCombo.DataSource = models.Tables(0)
                ModelCombo.ValueMember = "ModelId"
                ModelCombo.DisplayMember = "Name"
                ModelCombo.SelectedIndex = 0

            Catch ex As Exception
                MsgBox("Failed to load models : " + ex.Message + " : " + ex.StackTrace)
                RDFClasses.TDWGRDFException.LogError(ex)

            End Try
        End If
    End Sub

    Private Sub LSIDRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LSIDRadio.CheckedChanged
        ModelCombo.Enabled = ModelRadio.Checked
    End Sub

    Private Sub ModelRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModelRadio.CheckedChanged
        ModelCombo.Enabled = ModelRadio.Checked
    End Sub
End Class
