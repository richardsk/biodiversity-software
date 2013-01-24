
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub GridView1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            If GridView1.SelectedIndex >= GridView1.Rows.Count Then
                GridView1.SelectedIndex = -1
            End If
        End If
    End Sub

End Class
