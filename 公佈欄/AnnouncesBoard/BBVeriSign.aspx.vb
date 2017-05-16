Public Class BBVeriSign
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim pnlLogMode As Panel = CType(Master.FindControl("pnlLogMode"), Panel)
            pnlLogMode.Visible = False
            If String.IsNullOrEmpty(Session("LoginMode")) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "alert('非法登入.');location.href='BBContent.aspx';", True)
                Return
            End If
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function JVVar2Server(ByVal Data As String)
        Try
            Dim CRLBytes As Byte() = System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings.Item("CRLFilePath"))
            Return APMCer.Cer.CertInCRL_Status(Data.Split(",")(4), CRLBytes)
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Protected Sub btnreturnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnreturnLogin.Click
        Response.Redirect("BBContent.aspx?action=logout")
    End Sub
End Class