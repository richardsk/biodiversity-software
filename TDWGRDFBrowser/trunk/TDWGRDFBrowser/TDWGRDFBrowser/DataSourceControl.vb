Public Class DataSourceControl

    Private _dataSources As DataSet
    Private _models As DataSet


    Private Sub DataSourceControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DataSourcesGrid.AutoGenerateColumns = False

        Type.Items.Add("TAPIR")
        Type.Items.Add("LSID")
        Type.Items.Add("OAI-PMH")

        If Not DesignMode Then
            LoadDS()
        End If
    End Sub

    Private Sub DataSourcesGrid_RowsAdded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataSourcesGrid.RowsAdded
        If e.RowIndex > 1 Then
            DataSourcesGrid.Rows(e.RowIndex - 1).Cells(0).Value = True
        End If
    End Sub

    Public Sub LoadDS()

        If Not DesignMode Then
            Try
                _dataSources = DataAccess.DataSource.ListDataSources()
                For Each row As DataRow In _dataSources.Tables(0).Rows
                    If row("QuerierClass") = "RDFClasses.Query.TAPIRQuerier" Then row("QuerierClass") = "TAPIR"
                    If row("QuerierClass") = "RDFClasses.Query.LSIDQuerier" Then row("QuerierClass") = "LSID"
                    If row("QuerierClass") = "RDFClasses.Query.OAI_PMHQuerier" Then row("QuerierClass") = "OAI-PMH"
                Next

                _dataSources.AcceptChanges()

                DataSourcesGrid.DataSource = _dataSources.Tables(0)

                UpdateModels(DataSourcesGrid.CurrentRow.Index)

            Catch ex As Exception
                MsgBox("Failed to load data sources : " + ex.Message)
                RDFClasses.TDWGRDFException.LogError(ex)
            End Try
        End If
    End Sub

    Public Sub SaveDataSources()
        Dim dt As DataTable = DataSourcesGrid.DataSource
        If dt IsNot Nothing Then
            Dim ch As DataTable = dt.GetChanges
            If ch IsNot Nothing Then
                For Each row As DataRow In ch.Rows
                    If row.RowState = DataRowState.Deleted Then
                        row.RejectChanges()
                        Dim id As Integer = row("DataSourceId")
                        DataAccess.DataSource.DeleteDataSource(id)
                    Else
                        If row("QuerierClass") = "TAPIR" Then row("QuerierClass") = "RDFClasses.Query.TAPIRQuerier"
                        If row("QuerierClass") = "LSID" Then row("QuerierClass") = "RDFClasses.Query.LSIDQuerier"
                        If row("QuerierClass") = "OAI-PMH" Then row("QuerierClass") = "RDFClasses.Query.OAI_PMHQuerier"

                        DataAccess.DataSource.UpdateDataSource(row)
                    End If
                Next
            End If

            LoadDS()
        End If
    End Sub

    Private Sub DataSourcesGrid_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataSourcesGrid.RowEnter
        UpdateModels(e.RowIndex)
    End Sub

    Private Sub UpdateModels(ByVal row As Integer)
        ModelsList.Items.Clear()

        If row <> -1 And DataSourcesGrid.Rows(row).Cells("Id").Value IsNot DBNull.Value Then
            Dim id As Integer = DataSourcesGrid.Rows(row).Cells("Id").Value

            _models = DataAccess.DataSource.ListDataSourceModels(id)

            For Each r As DataRow In _models.Tables(0).Rows
                ModelsList.Items.Add(r("Name").ToString)
            Next
        End If

    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        If DataSourcesGrid.CurrentRow.Index <> -1 Then
            Dim pos As Integer = DataSourcesGrid.CurrentRow.Index
            If DataSourcesGrid.Rows(DataSourcesGrid.CurrentRow.Index).Cells("Id").Value Is DBNull.Value Then
                If MsgBox("You must save first", MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then Return
                SaveDataSources()
                DataSourcesGrid.ClearSelection()
                DataSourcesGrid.Rows(DataSourcesGrid.Rows.Count - 2).Selected = True
            End If

            Dim id As Integer = DataSourcesGrid.Rows(pos).Cells("Id").Value
            Dim rForm As New RDFModelsForm
            rForm.SelectedModels = DataAccess.DataSource.ListDataSourceModels(id)
            rForm.ShowDialog()

            ModelsList.Items.Clear()
            _models = rForm.SelectedModels
            For Each row As DataRow In _models.Tables(0).Rows
                ModelsList.Items.Add(row("Name").ToString)
            Next

            Try
                DataAccess.DataSource.UpdateDataSourceModels(id, _models)
            Catch ex As Exception
                MsgBox("Error saving data sources")
                RDFClasses.TDWGRDFException.LogError(ex)
            End Try
        End If
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
        SaveDataSources()
    End Sub

End Class
