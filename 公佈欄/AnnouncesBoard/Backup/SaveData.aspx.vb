Public Class SaveData
    Inherits System.Web.UI.Page

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("carddata") = Request.QueryString("carddata")
        Response.Redirect("BBContent.aspx?action=login")
    End Sub

End Class