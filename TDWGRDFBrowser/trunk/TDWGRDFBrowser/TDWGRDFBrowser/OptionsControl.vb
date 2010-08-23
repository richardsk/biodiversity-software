Public Class OptionsControl

    Private Sub OptionsControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim opt As DataAccess.Options = DataAccess.OptionData.GetOptions()
            CacheResultsCheck.Checked = opt.CacheQueryResults
            AutoResolveText.Text = opt.AutoResolveToLevel.ToString
        Catch ex As Exception
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

    Public Sub Save()
        Try
            Dim opt As New DataAccess.Options
            opt.CacheQueryResults = CacheResultsCheck.Checked
            Try
                opt.AutoResolveToLevel = Integer.Parse(AutoResolveText.Text)
            Catch ex As Exception
            End Try

            DataAccess.OptionData.SaveOptions(opt)
        Catch ex As Exception
            RDFClasses.TDWGRDFException.LogError(ex)
        End Try
    End Sub

End Class
