Public Class RDFModelsControl

    Private _models As DataSet

    Private Sub RDFModelsControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ModelsGrid.AutoGenerateColumns = False

        TypeCol.Items.Add("XSD")
        TypeCol.Items.Add("OWL")
        TypeCol.Items.Add("CNS")

        If Not DesignMode Then
            LoadModels()
        End If
    End Sub

    Private Sub LoadModels()
        Try
            _models = DataAccess.RDFModel.ListRDFModels

            ModelsGrid.DataSource = _models.Tables(0)
        Catch ex As Exception
            MsgBox("Error loading mnodels")
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

    Public Sub Save()
        Dim dt As DataTable = ModelsGrid.DataSource
        If dt IsNot Nothing Then
            Dim ch As DataTable = dt.GetChanges
            If ch IsNot Nothing Then
                For Each row As DataRow In ch.Rows
                    If row.RowState = DataRowState.Deleted Then
                        row.RejectChanges()
                        Dim id As Integer = row("ModelId")
                        DataAccess.RDFModel.DeleteModel(id)
                    Else
                        DataAccess.RDFModel.UpdateModel(row)
                    End If
                Next
            End If
        End If

        LoadModels()
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
        Save()
    End Sub
End Class
