Imports DDTek.Oracle

Public Class BBContent
    Inherits System.Web.UI.Page

    Public CardData As String
    Public Readname As String
    Public pdt As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("carddata")) Then
            If Request.QueryString("action") = "login" And Not Session("carddata") Is Nothing Then
                CardData = Session("carddata")
                Session("LoginMode") = 2
            Else
                Session("carddata") = Nothing
                Session("LoginMode") = 1
            End If
        Else
            Session("carddata") = Nothing
            Session("LoginMode") = 1
        End If

        Session("carddata") = "大同公司,2015/1/1,2020/12/31,cradreader"
        CardData = Session("carddata")
        Session("LoginMode") = 2

        If Not IsPostBack Then
            Dim lblLoginMode As Label = CType(Master.FindControl("lblLoginMode"), Label)
            Dim lblUnitName As Label = CType(Master.FindControl("lblUnitName"), Label)
            Dim div_unitname As HtmlGenericControl = CType(Master.FindControl("div_unitname"), HtmlGenericControl)
            Dim divLogin As HtmlGenericControl = Master.FindControl("div_Login")
            Dim divDrive As HtmlGenericControl = Master.FindControl("div_Drive")
            '2為單位模式,1為人員模式
            If Session("LoginMode") = 2 And Not String.IsNullOrEmpty(CardData) Then
                div_unitname.Visible = True
                lblUnitName.Text = CardData.Split(",")(0)
                divDownBtn.Visible = True
                divLogin.Visible = False
                divDrive.Visible = False
                gvList.Columns(6).Visible = True
                gvList.Columns(9).Visible = True
                gvList.Columns(10).Visible = True
            ElseIf Session("LoginMode") = 1 Then
                div_unitname.Visible = False
                divDownBtn.Visible = False
                divLogin.Visible = True
                divDrive.Visible = True
                gvList.Columns(6).Visible = False
                gvList.Columns(9).Visible = False
                gvList.Columns(10).Visible = False
            End If

            'If Session("LoginMode") = 2 And Not String.IsNullOrEmpty(CardData) Then
            '    div_unitname.Visible = True
            '    lblUnitName.Text = CardData.Split(",")(0)
            '    divDownBtn.Visible = True
            '    divLogin.Visible = False
            '    divDrive.Visible = False
            '    gvList.Columns(6).Visible = True
            '    gvList.Columns(9).Visible = True
            '    gvList.Columns(10).Visible = True
            'ElseIf Session("LoginMode") = 1 Then
            '    div_unitname.Visible = False
            '    divDownBtn.Visible = False
            '    divLogin.Visible = True
            '    divDrive.Visible = True
            '    gvList.Columns(6).Visible = False
            '    gvList.Columns(9).Visible = False
            '    gvList.Columns(10).Visible = False
            'End If
            pdt = PublicFunc.Search(3)
            Session("DV") = pdt
            gvList.DataSource = pdt
            gvList.DataBind()
            rowCountLab.Text = pdt.Rows.Count.ToString()


            ddlOrganBind(ddlOr_orgname)
            onlineUserLab.Text = Application("online")
        End If

        If Session("LoginMode") = 2 Then
            '將讀卡機名稱丟至前端
            Readname = CardData.Split(",")(3)
            '持續檢查卡片狀態
            Dim Script As String = "setInterval('CheckCard()',100000);"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", Script, True)
        End If

    End Sub

    'Bind DropDownList Data
    Private Sub ddlOrganBind(ByRef ddlOrgan As DropDownList)
        ddlOrgan.DataSource = PublicFunc.GetOrganData()
        ddlOrgan.DataBind()
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

    Private Sub gvList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowCreated
        ''隱藏批次下載
        gvList.Columns(10).Visible = False
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hidCO_PATH As HiddenField = CType(e.Row.FindControl("hidCO_PATH"), HiddenField)

            '加附加檔案
            Dim dt As DataTable = PublicFunc.GetAttach(hidCO_PATH.Value)
            Dim i As Integer
            i = 1
            'For Each row As DataRow In dt.Rows
            '    If Not row("at_filenm") = "_" Then
            '        Dim seqno As String = row("at_seqno")
            '        Dim filenm As String = row("at_filenm")
            '        Dim filetype As String = filenm.Substring(filenm.LastIndexOf(".") + 1)
            '        'filenm = filenm.Substring(filenm.LastIndexOf("-") + 1)
            '        filenm = i.ToString & "." & PublicFunc.GetAT_FDESC(seqno) & "(" & filetype & ")"
            '        Dim hplk As HyperLink = New HyperLink() With {.Text = filenm, .NavigateUrl = String.Format("DBFileStream.ashx?seqno={0}&down=1", seqno)}
            '        e.Row.Cells(7).Controls.Add(hplk)
            '        e.Row.Cells(7).Controls.Add(New LiteralControl("<br />"))
            '        i = i + 1
            '    End If
            'Next

            For Each row As DataRow In dt.Rows
                If Not row("at_filenm") = "_" Then
                    Dim seqno As String = row("at_seqno")
                    Dim filenm As String = row("at_filenm")
                    Dim filetype As String = filenm.Substring(filenm.LastIndexOf(".") + 1)
                    Dim lbAttach2 As Label = New Label() With {.Text = PublicFunc.GetAT_FDESC(seqno) & "(" & filetype & ")"}
                    e.Row.Cells(7).Controls.Add(lbAttach2)
                    e.Row.Cells(7).Controls.Add(New LiteralControl("<br />"))
                    i = i + 1
                End If
            Next
            i = 1
            For Each row As DataRow In dt.Rows
                If Not row("at_filenm") = "_" Then
                    Dim seqno As String = row("at_seqno")
                    Dim filenm As String = row("at_filenm")
                    Dim filetype As String = filenm.Substring(filenm.LastIndexOf(".") + 1)
                    Dim hplk As HyperLink = New HyperLink() With {.Text = i.ToString(), .NavigateUrl = String.Format("DBFileStream.ashx?seqno={0}&down=1", seqno)}
                    e.Row.Cells(7).Controls.Add(hplk)
                    e.Row.Cells(7).Controls.Add(New LiteralControl("&nbsp;"))
                    i = i + 1
                End If
            Next

            '加主檔開pdf
            Dim lbtnSubj As LinkButton = CType(e.Row.FindControl("lbtnSubj"), LinkButton)
            lbtnSubj.OnClientClick = String.Format("var show=window.open('DBFileStream.ashx?seqno={0}#toolbar=0', '', 'menubar=no,toolbar=no,location=no,status=no,resizable=yes');show.resizeTo(screen.availWidth,screen.availHeight);show.focus();return false;", PublicFunc.GetAT_SEQNO(hidCO_PATH.Value))

            '加下載明細
            Dim btnDetail As Button = CType(e.Row.FindControl("btnShowDetail"), Button)
            btnDetail.OnClientClick = String.Format("var show = window.open('BBDetail.aspx?path={0}&linktype=2&linkmode={1}', '', 'menubar=no,toolbar=no,location=no,status=no,scrollbars=yes,resizable=yes');show.resizeTo(screen.availWidth,screen.availHeight);show.focus();return false;", hidCO_PATH.Value, Session("LoginMode"))

            '判斷是否下載
            If Session("LoginMode") = 2 Then
                Dim lblIsDown As Label = CType(e.Row.FindControl("lblIsDownload"), Label)
                Dim OU As String = CardData.Split(",")(0)
                Dim CO_SEQNO As HiddenField = CType(e.Row.FindControl("hidCO_SEQNO"), HiddenField)
                If PublicFunc.IsDownLoad(PublicFunc.GetOrgno(OU), CO_SEQNO.Value) Then
                    lblIsDown.Text = "已下載"
                Else
                    lblIsDown.Text = "未下載"
                End If
            End If
        End If
    End Sub

    Protected Sub btnSearchIn3Day_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchIn3Day.Click
        Dim dt As DataTable = PublicFunc.Search(3)
        If dt.Rows.Count > 0 Then
            Session("DV") = dt
            gvList.DataSource = dt
            gvList.DataBind()
        Else
            gvList.DataBind()
        End If
        rowCountLab.Text = dt.Rows.Count.ToString()
    End Sub

    Protected Sub btnSearchIn1Month_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchIn1Month.Click
        Dim dt As DataTable = PublicFunc.Search(30)
        If dt.Rows.Count > 0 Then
            Session("DV") = dt
            gvList.DataSource = dt
            gvList.DataBind()
        Else
            gvList.DataBind()
        End If
        rowCountLab.Text = dt.Rows.Count.ToString()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Dim strDate As String = txtStart_Co_IssuDT.Text
        Dim endDate As String = txtEnd_Co_IssuDT.Text
        Dim strDate1 As String = txtStart_Co_PostDT.Text
        Dim endDate1 As String = txtEnd_Co_PostDT.Text

        If Not String.IsNullOrEmpty(strDate) And Not String.IsNullOrEmpty(endDate) Then
            strDate = ChineseYearToWestYear(strDate, "/")
            endDate = ChineseYearToWestYear(endDate, "/")
        End If

        If Not String.IsNullOrEmpty(strDate1) And Not String.IsNullOrEmpty(endDate1) Then
            strDate1 = ChineseYearToWestYear(strDate1, "/")
            endDate1 = ChineseYearToWestYear(endDate1, "/")
        End If

        Dim dt As DataTable = PublicFunc.Search(txtOr_orgname.Text, txtCo_Subj.Text, txtCo_Word.Text, txtCo_SNO.Text, strDate, endDate, strDate1, endDate1, ckbGuideFlag.Checked)

        If dt.Rows.Count > CInt(ConfigurationManager.AppSettings.Item("SearchMaxLength")) Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "downloadBefore", "alert('警告，查詢的資料量過大，\r\n請縮小查詢範圍!!');", True)
            Return
        End If

        If dt.Rows.Count > 0 Then
            Session("DV") = dt
            gvList.DataSource = dt
            gvList.DataBind()
        Else
            gvList.DataBind()
        End If
        rowCountLab.Text = dt.Rows.Count.ToString()
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        Select Case e.CommandName
            Case "download"
                'DI or 公文內容
                Dim RowIndex As Integer = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow).RowIndex
                Dim CO_PATH As String = CType(gvList.Rows(RowIndex).FindControl("hidCO_PATH"), HiddenField).Value
                Dim CO_SNO As String = CType(gvList.Rows(RowIndex).FindControl("hidCO_SNO"), HiddenField).Value
                Dim OR_ORGNO As String = CType(gvList.Rows(RowIndex).FindControl("hidOR_ORGNO"), HiddenField).Value
                Dim lblSerial As String = CType(gvList.Rows(RowIndex).FindControl("lblSerial"), Label).Text
                Dim lblIsDownload As String = CType(gvList.Rows(RowIndex).FindControl("lblIsDownload"), Label).Text

                'CardData = "憲兵指揮部苗栗憲兵隊,2014/1/1,2020/1/1,cardreadername"

                If Not String.IsNullOrEmpty(CardData) Then
                    Dim OU = CardData.Split(",")(0).Trim()
                    'Dim OU = "國防部情報參謀次長室" '測試
                    Dim StrDate = CardData.Split(",")(1).Trim()
                    Dim EndDate = CardData.Split(",")(2).Trim()
                    Dim OutputOR_ORGNO As String = PublicFunc.GetOrgno(OU)
                    Dim OutputOR_ORGNAME As String = PublicFunc.GetOrgname(OutputOR_ORGNO)

                    '判斷OR_ORGNO
                    If String.IsNullOrEmpty(OutputOR_ORGNO) Then
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "date_range_err", "alert('系統顯示該單位名稱不存於系統資料庫，請通知系統管理員。系統停止下載。');", True)
                        Return
                    End If

                    '判斷日期
                    If Not IsDate(StrDate) And Not IsDate(EndDate) Then
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "date_err", "alert('卡片日期格式不正確。');", True)
                        Return
                    End If

                    '判斷有效期限
                    If Not DateTime.Parse(StrDate) <= Now And Not DateTime.Parse(EndDate) >= Now Then
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "date_range_err", "alert('已超過有效使用期限。');", True)
                        Return
                    End If

                    Try
                        Console.WriteLine(OR_ORGNO + " " + OutputOR_ORGNO + " " + OutputOR_ORGNAME + " " + CO_SNO + " " + CO_PATH)
                        'PublicFunc.Download(OR_ORGNO, OutputOR_ORGNO, OutputOR_ORGNAME, CO_SNO, CO_PATH)
                        gvList.DataSource = Session("DV")
                        gvList.DataBind()
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "download_finish", String.Format("alert('{0} - 下載成功!!');", lblSerial), True)
                    Catch ex As Exception
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "download_finish", String.Format("alert('{0} - 下載失敗!!');", lblSerial), True)
                    End Try
                End If
            Case Else
        End Select
    End Sub

    Protected Sub btnDownloadAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDownloadAll.Click
        If Not String.IsNullOrEmpty(CardData) Then
            Dim OU = CardData.Split(",")(0).Trim()
            Dim StrDate = CardData.Split(",")(1).Trim()
            Dim EndDate = CardData.Split(",")(2).Trim()
            Dim OutputOR_ORGNO As String = PublicFunc.GetOrgno(OU)
            Dim OutputOR_ORGNAME As String = PublicFunc.GetOrgname(OutputOR_ORGNO)

            '判斷OR_ORGNAME,OR_ORGNO
            If String.IsNullOrEmpty(OutputOR_ORGNO) Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "date_range_err", "alert('系統顯示該單位名稱不存於系統資料庫，請通知系統管理員。系統停止下載。');", True)
                Return
            End If

            '判斷日期
            If Not IsDate(StrDate) And Not IsDate(EndDate) Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "date_err", "alert('卡片日期格式不正確。');", True)
                Return
            End If

            '判斷有效期限
            If Not DateTime.Parse(StrDate) <= Now And Not DateTime.Parse(EndDate) >= Now Then
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "date_range_err", "alert('已超過有效使用期限。');", True)
                Return
            End If

            Try
                For Each gvRow As GridViewRow In gvList.Rows
                    Dim cbSelect As CheckBox = CType(gvRow.FindControl("cbSelect"), CheckBox)
                    Dim CO_PATH As String = CType(gvRow.FindControl("hidCO_PATH"), HiddenField).Value
                    Dim CO_SNO As String = CType(gvRow.FindControl("hidCO_SNO"), HiddenField).Value
                    Dim OR_ORGNO As String = CType(gvRow.FindControl("hidOR_ORGNO"), HiddenField).Value
                    Dim lblIsDownload As String = CType(gvRow.FindControl("lblIsDownload"), Label).Text

                    If cbSelect.Checked Then
                        'PublicFunc.Download(OR_ORGNO, OutputOR_ORGNO, OutputOR_ORGNAME, CO_SNO, CO_PATH)
                    End If
                    gvList.DataSource = Session("DV")
                    gvList.DataBind()
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "download_finish", "alert('批次下載成功!!');", True)
                Next

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "download_finish", "alert('批次下載失敗!!');", True)
            End Try
        End If
    End Sub

    Protected Sub gvList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvList.PageIndexChanging
        If IsNothing(Session("DV")) Then
            Session.Abandon()
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SessionTimeOut", "alert('頁面停滯過長。系統重新登入。');", True)
            Response.Redirect("Default.aspx", True)
        End If
        gvList.PageIndex = e.NewPageIndex()
        gvList.DataSource = Session("DV")
        gvList.DataBind()
        'pdt = Session("DV")
        'rowCountLab.Text = pdt.Rows.Count.ToString()
    End Sub

    Protected Sub gvList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvList.Sorting
        If Session("Sort") Is Nothing Then
            Session("Sort") = String.Format("{0} ASC", e.SortExpression)
        ElseIf Session("Sort").ToString().StartsWith(e.SortExpression) And Session("Sort").ToString().EndsWith("ASC") Then
            Session("Sort") = String.Format("{0} DESC", e.SortExpression)
        Else
            Session("Sort") = String.Format("{0} ASC", e.SortExpression)
        End If

        Dim dv As DataView = CType(Session("DV"), DataTable).DefaultView
        dv.Sort = Session("Sort")
        gvList.DataSource = dv
        gvList.DataBind()
    End Sub
End Class
