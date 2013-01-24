Imports System.Data

Partial Class ProviderForm
    Inherits System.Web.UI.Page

    Protected ProviderId As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("provid") IsNot Nothing Then
            ProviderId = CInt(Request.QueryString("provid"))

            If Request.QueryString("getschema") IsNot Nothing Then
                'return schema
                NominaDataAccess.RegistryData.ConnectionString = ConfigurationManager.ConnectionStrings("NominaConnectionString1").ConnectionString
                Dim res As String = NominaDataAccess.RegistryData.GetProviderMetadata(ProviderId)
                Response.ContentType = "xml"
                Response.Write(res)
                Response.End()
                Exit Sub
            End If
        Else
            'new prov
            DetailsView1.DefaultMode = DetailsViewMode.Insert
            DetailsView1.AutoGenerateInsertButton = True
        End If

        If GridView2.Rows.Count = 0 Then

        End If
    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand

        If e.CommandName = "Update" AndAlso ProviderId = 0 Then
            Response.Redirect("default.aspx")
        End If

    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Response.Redirect("providerform.aspx?provid=" + ProviderId.ToString + "&getschema=1")
    End Sub
End Class
