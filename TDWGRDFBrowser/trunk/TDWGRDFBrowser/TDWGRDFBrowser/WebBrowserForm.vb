Public Class WebBrowserForm

    Public Property Html() As String
        Get
            Return BrowserControl1.WebBrowser1.DocumentText
        End Get
        Set(ByVal value As String)
            BrowserControl1.WebBrowser1.DocumentText = value
        End Set
    End Property

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub
End Class