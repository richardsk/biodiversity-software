Public Class HarvestControl

    Private Sub HarvestControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not DesignMode Then
                ResultsGrid.AutoGenerateColumns = False

                Dim ds As DataSet = DataAccess.DataSource.ListDataSources()
                For Each row As DataRow In ds.Tables(0).Rows
                    If row("QuerierClass") = "RDFClasses.Query.TAPIRQuerier" Then row("QuerierClass") = "TAPIR"
                    If row("QuerierClass") = "RDFClasses.Query.LSIDQuerier" Then row("QuerierClass") = "LSID"
                    If row("QuerierClass") = "RDFClasses.Query.OAI_PMHQuerier" Then row("QuerierClass") = "OAI-PMH"
                Next

                ds.AcceptChanges()

                DataSourceCombo.DisplayMember = "Name"
                DataSourceCombo.ValueMember = "DataSourceId"
                DataSourceCombo.DataSource = ds.Tables(0)
            End If
        Catch ex As Exception
            MsgBox("Failed to load data sources")
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

    Private Sub DataSourceCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataSourceCombo.SelectedIndexChanged
        Try
            If DataSourceCombo.SelectedIndex <> -1 Then
                Dim ds As DataSet = DataAccess.DataSource.ListDataSourceModels(DataSourceCombo.SelectedValue)
                RDFModelCombo.DataSource = ds.Tables(0)
                RDFModelCombo.DisplayMember = "Name"
                RDFModelCombo.ValueMember = "ModelId"
            End If
        Catch ex As Exception
            MsgBox("Failed to load models for data source")
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

    Private Sub GetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetButton.Click
        If RDFModelCombo.SelectedIndex <> -1 AndAlso DataSourceCombo.SelectedIndex <> -1 Then
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Try
                Dim drv As DataRowView = RDFModelCombo.SelectedItem
                Dim model As New RDFClasses.RDFModel
                model.Load(drv.Row()("ModelId"))
                Dim ids As List(Of String) = RDFClasses.Query.QueryController.ListIds(DataSourceCombo.SelectedValue, model)

                Dim dt As New DataTable
                dt.Columns.Add("Id")
                dt.Columns.Add("Show")

                Dim col As New DataGridViewTextBoxColumn
                col.Name = "IdCol"
                col.HeaderText = "Id"
                col.DataPropertyName = "Id"
                col.Width = 250
                ResultsGrid.Columns.Add(col)

                Dim buttonCol As New DataGridViewButtonColumn
                buttonCol.Name = "ShowCol"
                buttonCol.DataPropertyName = "Show"
                ResultsGrid.Columns.Add(buttonCol)

                For Each id As String In ids
                    dt.Rows.Add(New Object() {id, "Show"})
                Next

                ResultsGrid.DataSource = dt

            Catch ex As Exception
                MsgBox("Failed to harvest Ids")
                RDFClasses.TDWGRDFException.LogError(ex)
            End Try
            Windows.Forms.Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub ResultsGrid_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ResultsGrid.CellContentClick
        If e.ColumnIndex = 1 Then
            Try
                'get rdf
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                Dim rdf As Xml.XmlDocument = Nothing
                Dim val As String = ResultsGrid.CurrentRow.Cells("IdCol").Value.ToString
                If RDFClasses.Utility.IsLSID(val) Then
                    rdf = RDFClasses.Query.QueryController.ResolveLSID(val)
                Else
                    rdf = RDFClasses.Query.QueryController.ResolveUri(val)
                End If

                Windows.Forms.Cursor.Current = Cursors.Default
                If rdf.DocumentElement IsNot Nothing Then
                    Dim det As New DetailsForm
                    det.DetailsControl1.RdfDocument = rdf

                    det.ShowDialog()
                Else
                    MsgBox("Failed to resolve data.")
                End If

            Catch ex As Exception
                MsgBox("Failed to resolve data.")
                RDFClasses.TDWGRDFException.LogError(ex)
            End Try
        End If
    End Sub
End Class
