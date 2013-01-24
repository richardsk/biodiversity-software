Imports System.Xml

Partial Class UploadForm
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'upload
        Try
            Dim preg As New RegistryWS.ProviderRegistry
            Dim doc As New XmlDocument
            doc.Load(regitryFileUrl.Text)

            preg.AddProvider("", doc)

            resultLabel.Text = "Success"
        Catch ex As Exception
            Response.Write("Error : " + ex.Message)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
