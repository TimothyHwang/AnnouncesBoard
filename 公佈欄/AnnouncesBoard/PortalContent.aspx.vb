Public Class PortalContent
    Inherits System.Web.UI.Page

    Public Shared Function WestYearToChineseYear(ByVal DateString As String, ByVal Split As Char)
        Try
            Return String.Format("{0}{1}{2}", Integer.Parse(DateString.Substring(0, DateString.IndexOf(Split))) - 1911, Split, DateString.Substring(DateString.IndexOf(Split) + 1))
        Catch
            Return String.Empty
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SqlCmd As String = String.Empty        
        SqlCmd = String.Format("select rownum, t.* from (select distinct a.or_orgno, a.or_orgname, b.co_word, b.co_dtype, b.co_sno, b.co_subj, b.co_issudt, b.co_postdt, b.co_path, b.co_seqno from organ a, content b where a.or_orgno = b.co_orgno and co_postdt between to_date('{0}','yyyy/mm/dd') and to_date('{1}','yyyy/mm/dd') order by co_postdt desc, co_issudt desc, co_word, co_sno, or_orgname)t", Now.AddDays(3 * -1).ToString("yyyy/MM/dd"), Now.ToString("yyyy/MM/dd"))        

        Dim pdt As DataTable = New Basic().ODataAdapter(SqlCmd)        
        gvList.DataSource = pdt
        gvList.DataBind()
    End Sub

    Protected Sub btnSearchIn3Day_Click(sender As Object, e As EventArgs) Handles btnSearchIn3Day.Click

    End Sub
End Class