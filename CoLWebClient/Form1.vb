Public Class Form1

    Public Sub DoStuff()
        Dim svc As New CoL.CASWebServiceService
        TextBox1.Text += "Version : " + svc.satisfyType0Request() + Environment.NewLine
        TextBox1.Text += "Search : " + svc.satisfyType1Request("amanita muscaria", "", Nothing, "", True).speciesNames.ToString

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DoStuff()
    End Sub
End Class
