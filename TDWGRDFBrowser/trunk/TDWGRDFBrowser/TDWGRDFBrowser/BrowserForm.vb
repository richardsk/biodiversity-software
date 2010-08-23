Imports System.Configuration

Public Class BrowserForm

    Public Sub New()

        SetCnnStrings()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    Private Sub BrowserForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SetCnnStrings()
        Dim cnn As String = ConfigurationManager.AppSettings.Get("ConnectionString")

        DataAccess.Config.ConncetionString = cnn
        DataAccess.DataSource.ConncetionString = cnn
        DataAccess.RDFModel.ConncetionString = cnn
        DataAccess.OptionData.ConncetionString = cnn
    End Sub

    Private Sub NavigationBar1_SelectedPaneChanging(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles NavigationBar1.SelectedPaneChanging
        If Not DesignMode AndAlso NavigationBar1.SelectedPane IsNot Nothing Then
            Try
                If NavigationBar1.SelectedPane.Name = "OptionsPane" Then
                    Dim ctrl As OptionsControl = NavigationContainer1.Controls("OptionsHeaderControl").Controls(0)
                    ctrl.Save()
                ElseIf NavigationBar1.SelectedPane.Name = "DataSourcePane" Then
                    Dim ctrl As DataSourceControl = NavigationContainer1.Controls("DSControl").Controls(0)
                    ctrl.SaveDataSources()
                ElseIf NavigationBar1.SelectedPane.Name = "RDFModelsPane" Then
                    Dim ctrl As RDFModelsControl = NavigationContainer1.Controls("RDFModelsHeaderControl").Controls(0)
                    ctrl.Save()
                End If

            Catch ex As Exception
                MsgBox("Failed to save : " + ex.Message)
                RDFClasses.TDWGRDFException.LogError(ex)
            End Try
        End If
    End Sub

    Private Sub NavigationPane2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HttpPane.Click

    End Sub
End Class
