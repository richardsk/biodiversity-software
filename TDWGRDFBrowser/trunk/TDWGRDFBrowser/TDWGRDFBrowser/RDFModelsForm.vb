Public Class RDFModelsForm

    Private Class ListItem
        Public Id As Integer = 0
        Public Name As String = ""

        Sub New(ByVal id As Integer, ByVal name As String)
            Me.Id = id
            Me.Name = name
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    Private _models As DataSet

    Public SelectedModels As DataSet

    Private Sub RDFModelsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not DesignMode Then
            _models = DataAccess.RDFModel.ListRDFModels()
            For Each row As DataRow In _models.Tables(0).Rows
                Dim pos As Integer = ModelsList.Items.Add(New ListItem(row("ModelId"), row("Name")))

                If SelectedModels IsNot Nothing AndAlso SelectedModels.Tables(0).Select("ModelId = " + row("ModelId").ToString).Length > 0 Then
                    ModelsList.SetItemChecked(pos, True)
                End If
            Next
        End If
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        If SelectedModels IsNot Nothing Then
            SelectedModels.Tables(0).Rows.Clear()

            Dim pos As Integer = 0
            For Each li As ListItem In ModelsList.Items
                If ModelsList.GetItemChecked(pos) Then
                    SelectedModels.Tables(0).ImportRow(_models.Tables(0).Select("ModelId = " + li.Id.ToString)(0))
                End If
                pos += 1
            Next
        End If

        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class