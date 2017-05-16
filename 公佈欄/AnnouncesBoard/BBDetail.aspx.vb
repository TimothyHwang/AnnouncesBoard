Imports System.Data
Imports DDTek.Oracle

Public Class BBDetail
    Inherits System.Web.UI.Page

    Private seqno As String
    Private type As String
    Private mode As String
    Private co_seqno As String
    Private co_path As String
    Private co_sno As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request("path")) And Not String.IsNullOrEmpty(Request("linktype")) And Not String.IsNullOrEmpty(Request("linkmode")) Then
            co_path = Request("path")
            type = Request("linktype")
            mode = Request("linkmode")
        ElseIf Not String.IsNullOrEmpty(Request("seqno")) And Not String.IsNullOrEmpty(Request("linktype")) And Not String.IsNullOrEmpty(Request("linkmode")) Then
            seqno = Request("seqno")
            type = Request("linktype")
            mode = Request("linkmode")
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "alert('非法登入.');history.back();", True)
            Return
        End If

        If Not IsPostBack Then
            Dim lblLoginMode As Label = CType(Master.FindControl("lblLoginMode"), Label)
            Dim lblUnitName As Label = CType(Master.FindControl("lblUnitName"), Label)
            Dim div_unitname As HtmlGenericControl = CType(Master.FindControl("div_unitname"), HtmlGenericControl)
            If mode = 1 Then
                div_unitname.Visible = False
            ElseIf mode = 2 And Not Session("carddata") Is Nothing Then
                lblUnitName.Text = Session("carddata").ToString().Split(",")(0)
                div_unitname.Visible = True
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "alert('非法登入.');history.back();", True)
                Return
            End If

            Dim detail As New Detail()
            If type = 2 Then
                detail.LoadPathDetail(co_path)
                lblOr_orgname.Text = detail.Or_orgname
                lblCo_issudt.Text = WestYearToChineseYear(detail.Co_issudt, "/")
                lblCo_dtype.Text = detail.Co_dtype
                lblCo_postdt.Text = WestYearToChineseYear(detail.Co_postdt, "/")
                lblCo_postnm.Text = detail.Co_postnm
                lblCo_subj.Text = detail.Co_subj
                lblCo_word_sno.Text = String.Format("{0}字第{1}號", detail.Co_word, detail.Co_sno)
                co_seqno = detail.Co_seqno
                ViewState("co_seqno") = detail.Co_seqno
                co_sno = detail.Co_sno
            ElseIf detail.LoadSeqnoDetail(seqno) Then
                lblOr_orgname.Text = detail.Or_orgname
                lblCo_issudt.Text = WestYearToChineseYear(detail.Co_issudt, "/")
                lblCo_dtype.Text = detail.Co_dtype
                lblCo_postdt.Text = WestYearToChineseYear(detail.Co_postdt, "/")
                lblCo_postnm.Text = detail.Co_postnm
                lblCo_subj.Text = detail.Co_subj
                lblCo_word_sno.Text = String.Format("{0} - {1}", detail.Co_word, detail.Co_sno)
                co_seqno = detail.Co_seqno
                ViewState("co_seqno") = detail.Co_seqno
                co_sno = detail.Co_sno
                co_path = detail.Co_path
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "alert('無資料.');history.back();", True)
            End If
        End If

        BindAttachList(co_path)
        BindDownloadList()
    End Sub

    Public Shared Function ChineseYearToWestYear(ByVal DateString As String, ByVal Split As Char)
        Try
            Return String.Format("{0}{1}{2}", Integer.Parse(DateString.Substring(0, DateString.IndexOf(Split))) + 1911, Split, DateString.Substring(DateString.IndexOf(Split) + 1))
        Catch
            Return String.Empty
        End Try
    End Function

    Public Shared Function WestYearToChineseYear(ByVal DateString As String, ByVal Split As Char)
        Try
            Return String.Format("{0}{1}{2}", Integer.Parse(DateString.Substring(0, DateString.IndexOf(Split))) - 1911, Split, DateString.Substring(DateString.IndexOf(Split) + 1))
        Catch
            Return String.Empty
        End Try
    End Function

    Private Sub BindAttachList(ByVal AT_PATH As String)
        '加附加檔案
        AT_PATH = Basic.HtmlEncode(AT_PATH)
        Dim cmd As String = String.Format("select rownum, at_ftype, at_fdesc, at_seqno, at_filenm from attach where at_path = '{0}' order by at_ftype,at_ln", AT_PATH)
        Dim dt As DataTable = New Basic().ODataAdapter(cmd)
        Dim tb As New DataTable()
        tb.Columns.Add("Rownum")
        tb.Columns.Add("at_ftype")
        tb.Columns.Add("at_fdesc")
        tb.Columns.Add("showPDF")

        For Each Row As DataRow In dt.Rows
            If Not Row("at_filenm") = "_" Then
                Dim filenm As String = Row("at_filenm")

                Dim dr As DataRow = tb.NewRow()
                dr.Item(0) = Row("Rownum")
                dr.Item(1) = FTypeCode2Name(Row("At_ftype"))
                dr.Item(2) = Row("At_fdesc")
                dr.Item(3) = String.Format("var show=window.open('DBFileStream.ashx?seqno={0}#toolbar=0', '', 'menubar=no,toolbar=no,location=no,status=no,resizable=yes');show.resizeTo(screen.availWidth,screen.availHeight);show.focus();return false;", Row("at_seqno"))
                tb.Rows.Add(dr)
            End If
        Next

        gvDetailList.DataSource = tb
        gvDetailList.DataBind()
    End Sub

    Private Sub BindDownloadList()
        Dim dt As DataTable = PublicFunc.GetDownloadLog(co_seqno)
        Dim tb As New DataTable()
        tb.Columns.Add("UnitName")
        tb.Columns.Add("DownloadDate")

        For Each Row As DataRow In dt.Rows
            Dim dr As DataRow = tb.NewRow()
            dr.Item(0) = PublicFunc.GetOrgname(Row("DD_DPID"))
            dr.Item(1) = Date.Parse(Row("DD_DATE")).ToString("yyyy/MM/dd")
            tb.Rows.Add(dr)
        Next

        gvDownRecordList.DataSource = tb
        gvDownRecordList.DataBind()
    End Sub

    Public Function FTypeCode2Name(ByVal ftypeCode As String) As String
        Select Case ftypeCode
            Case "1"
                Return lblCo_dtype.Text
            Case "2"
                Return "附件"
            Case Else
                Return ""
        End Select
    End Function

    Protected Sub btnDownLog_Click(sender As Object, e As EventArgs) Handles btnDownLog.Click

        Dim dt As DataTable = PublicFunc.GetDownloadLog(ViewState("co_seqno"))
        Dim tb As New DataTable()
        tb.Columns.Add("單位名稱")
        tb.Columns.Add("下載日期")

        For Each Row As DataRow In dt.Rows
            Dim dr As DataRow = tb.NewRow()
            dr.Item(0) = PublicFunc.GetOrgname(Row("DD_DPID"))
            dr.Item(1) = Date.Parse(Row("DD_DATE")).ToString("yyyy/MM/dd")
            tb.Rows.Add(dr)
        Next

        Dim gvDeatil = New GridView()
        gvDeatil.DataSource = tb
        gvDeatil.DataBind()

        Dim strExportFilename As String = Now.ToString("yyyyMMddHHmmss") & ".xls"

        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=" + strExportFilename)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"
        Response.Charset = "big5"

        Dim stringWrite = New IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)
        gvDeatil.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString().Replace("<div>", "").Replace("</div>", ""))
        Response.End()
    End Sub
End Class