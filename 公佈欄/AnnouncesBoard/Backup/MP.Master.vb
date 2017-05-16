Public Class MP
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Login.Click
        Response.Redirect("BBVeriSign.aspx")
    End Sub
End Class