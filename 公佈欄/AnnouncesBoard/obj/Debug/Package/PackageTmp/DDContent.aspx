<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MP.Master"
    CodeBehind="DDContent.aspx.vb" Inherits="AnnouncesBoard.DDContent" %>

<%@ Register Assembly="MarkGridView" Namespace="MarkGridView.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <object id='APMCer' classid='CLSID:77DF8C7B-F505-433e-B768-22DD05B4AC1C'>
    </object>
    <style type="text/css">
        #SearchTable
        {
            margin: 0 auto 0 auto;
        }
        #btnList
        {
            margin: 0 auto 0 auto;
        }
        
        #btnList td
        {
            padding: 0 10px 0 10px;
        }
        #UnitDetail td
        {
            padding: 0 10px 0 10px;
        }
        .Download
        {
            width: 30px;
            height: 30px;
            background-color: Transparent;
            background-image: url("./Image/download.png");
            background-position: center center;
            background-repeat: no-repeat;
        }
        .Button_Up
        {
            width: 80px;
            height: 25px;
            font-size: 12pt;
            background: url("image/btn2.png") no-repeat top;
        }
        .Button_Up input
        {
            width: 80px;
            height: 25px;
            border: 0;
            background: url("image/btn2.png") no-repeat top;
        }
        .Button_Down
        {
            width: 80px;
            height: 25px;
            font-size: 12pt;
            background: url("image/btn1.png") no-repeat top;
        }
        .Button_Down input
        {
            width: 80px;
            height: 25px;
            border: 0;
            background: url("image/btn1.png") no-repeat top;
        }
    </style>
    <script src="Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function Clear() {
            $("#ctl00_ContentPlaceHolder1_txtOr_orgname").val('');
            $("#ctl00_ContentPlaceHolder1_txtStart_Co_IssuDT").val('');
            $("#ctl00_ContentPlaceHolder1_txtCo_Subj").val('');
            $("#ctl00_ContentPlaceHolder1_txtCo_Word").val('');
            $("#ctl00_ContentPlaceHolder1_txtCo_SNO").val('');
            $("#ctl00_ContentPlaceHolder1_txtStart_Co_IssuDT").val('');
            $("#ctl00_ContentPlaceHolder1_txtEnd_Co_IssuDT").val('');
            $("#ctl00_ContentPlaceHolder1_txtStart_Co_PostDT").val('');
            $("#ctl00_ContentPlaceHolder1_txtEnd_Co_PostDT").val('');
        }
    </script>
    <script type="text/javascript">
        //轉民國年
        function StartToLocalDate() {
            var year = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_txtStart_Co_IssuDT").value.substr(0, 4)) - 1911;
            document.getElementById("ctl00_ContentPlaceHolder1_txtStart_Co_IssuDT").value = year + document.getElementById("ctl00_ContentPlaceHolder1_txtStart_Co_IssuDT").value.substr(4, 6);
        }
        //轉民國年
        function EndToLocalDate() {
            var year = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_txtEnd_Co_IssuDT").value.substr(0, 4)) - 1911;
            document.getElementById("ctl00_ContentPlaceHolder1_txtEnd_Co_IssuDT").value = year + document.getElementById("ctl00_ContentPlaceHolder1_txtEnd_Co_IssuDT").value.substr(4, 6);
        }

        function SStartToLocalDate() {
            var year = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_txtStart_Co_PostDT").value.substr(0, 4)) - 1911;
            document.getElementById("ctl00_ContentPlaceHolder1_txtStart_Co_PostDT").value = year + document.getElementById("ctl00_ContentPlaceHolder1_txtStart_Co_PostDT").value.substr(4, 6);
        }
        function EEndToLocalDate() {
            var year = parseInt(document.getElementById("ctl00_ContentPlaceHolder1_txtEnd_Co_PostDT").value.substr(0, 4)) - 1911;
            document.getElementById("ctl00_ContentPlaceHolder1_txtEnd_Co_PostDT").value = year + document.getElementById("ctl00_ContentPlaceHolder1_txtEnd_Co_PostDT").value.substr(4, 6);
        }
        function downloadCheck(x) {
            if (x.parentNode.parentNode.children[6].innerText.trim() == '已下載') {
                if (!confirm("已下載過，是否還要下載 ?")) {
                    window.event.returnValue = false;
                }
            }
        }

        function downloadAllCheck() {
            var flag = false;
            var $cb = $(":checkbox");
            for (var i = 0; i < $cb.length; i++) {
                if (i != 0) {
                    var $status = $cb[i].parentNode.parentNode.parentNode.children[6].innerText.trim();
                }
                if ($cb[i].checked && $status == '已下載') {
                    flag = true;
                    break;
                }
            }
            if (flag) {
                if (!confirm("部份項目已下載過，是否還要下載 ?")) {
                    window.event.returnValue = false;
                }
            }
        }

        $("form").ready(function () {
            $("#ctl00_ContentPlaceHolder1_gvList_ctl02_cbAllSelect").click(
            function () {
                if ($("#ctl00_ContentPlaceHolder1_gvList_ctl02_cbAllSelect").attr("checked")) {
                    $(":checkbox").attr("checked", true);
                } else {
                    $(":checkbox").attr("checked", false);
                }
            });

            $("#ctl00_ContentPlaceHolder1_btnDownloadAll").attr("disabled", true);
            $(":checkbox").click(function () {
                var flag = false;
                var $cb = $(":checkbox");
                for (var i = 0; i < $cb.length; i++) {
                    if ($cb[i].checked) {
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    $("#ctl00_ContentPlaceHolder1_btnDownloadAll").attr("disabled", false);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_btnDownloadAll").attr("disabled", true);
                }
            });

            $("div .Button_Up").mouseenter(
            function () {
                $(this).removeClass("Button_Up");
                $(this).addClass("Button_Down");
            });
            $("div .Button_Up").mouseout(
            function () {
                $(this).removeClass("Button_Down");
                $(this).addClass("Button_Up");
            });
        });
    </script>
    <script type="text/javascript">
        function CheckCard() {
            try {
                if (APMCer.CheckCard('<%= Readname %>') == false) { location.href = 'BBContent.aspx?action=logout'; }
            } catch (e) {

            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            text-align: left;
            font-size: 11pt;
            height: 25px;
            margin: 0 auto 0 auto;
        }
        
        .style2
        {
            text-align: center;
            font-size: 11pt;
            margin: 0 auto 0 auto;
            border-color: Gray;
        }
        
        .style3
        {
            text-align: left;
            font-size: 8pt;
            margin: 0 auto 0 auto;
            color: #800000;
            vertical-align: middle;
        }
    </style>
    <style type="text/css">
        .Area
        {
            margin-left: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="SearchTable" width="95%">
        <tr>
            <td>
                &nbsp;
                <table class="style1">
                    <tr>
                        <td>
                            <table style="margin: 0px auto 0px auto">
                                <tr>
                                    <td width="80px" class="style1">
                                        發文機關
                                    </td>
                                    <td width="5px" class="style1">
                                        ：
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtOr_orgname" runat="server" Width="303px" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td width="20px" class="style1">
                                        &nbsp;
                                    </td>
                                    <td width="70px" class="style1">
                                        發文日期
                                    </td>
                                    <td width="5px" class="style1">
                                        ：
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtStart_Co_IssuDT" runat="server" CssClass="Area" AutoCompleteType="Disabled"
                                            Width="120px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtStart_Co_IssuDT_CalendarExtender" runat="server" DaysModeTitleFormat="yyyy/MM/"
                                            Enabled="True" Format="yyyy/MM/dd" TargetControlID="txtStart_Co_IssuDT" TodaysDateFormat="yyyy/MM/dd"
                                            OnClientDateSelectionChanged="StartToLocalDate" PopupButtonID="calendar1">
                                        </asp:CalendarExtender>
                                        &nbsp;<asp:Image ID="calendar1" runat="server" ImageUrl="~/Image/calendar.gif" BehaviorID="calendar1" />
                                    </td>
                                    <td class="style1">
                                        &nbsp;～
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtEnd_Co_IssuDT" runat="server" CssClass="Area" AutoCompleteType="Disabled"
                                            Width="120px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEnd_Co_IssuDT_CalendarExtender" runat="server" DaysModeTitleFormat="yyyy/MM/"
                                            Enabled="True" Format="yyyy/MM/dd" TargetControlID="txtEnd_Co_IssuDT" TodaysDateFormat="yyyy/MM/dd"
                                            OnClientDateSelectionChanged="EndToLocalDate" PopupButtonID="calendar2">
                                        </asp:CalendarExtender>
                                        &nbsp;<asp:Image ID="calendar2" runat="server" ImageUrl="~/Image/calendar.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="80px" class="style1">
                                        主旨關鍵字
                                    </td>
                                    <td width="5px" class="style1">
                                        ：
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtCo_Subj" runat="server" CssClass="Area" Width="300px" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td width="20px" class="style1">
                                        &nbsp;
                                    </td>
                                    <td width="70px" class="style1">
                                        公告日期
                                    </td>
                                    <td width="5px" class="style1">
                                        ：
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtStart_Co_PostDT" runat="server" CssClass="Area" AutoCompleteType="Disabled"
                                            Width="120px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtStart_Co_PostDT_CalendarExtender" runat="server" DaysModeTitleFormat="yyyy/MM/"
                                            Enabled="True" Format="yyyy/MM/dd" TargetControlID="txtStart_Co_PostDT" TodaysDateFormat="yyyy/MM/dd"
                                            OnClientDateSelectionChanged="SStartToLocalDate" PopupButtonID="calendar3">
                                        </asp:CalendarExtender>
                                        &nbsp;<asp:Image ID="calendar3" runat="server" ImageUrl="~/Image/calendar.gif" BehaviorID="calendar3" />
                                    </td>
                                    <td class="style1">
                                        &nbsp;&nbsp;～
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtEnd_Co_PostDT" runat="server" CssClass="Area" AutoCompleteType="Disabled"
                                            Width="120px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEnd_Co_PostDT_CalendarExtender" runat="server" DaysModeTitleFormat="yyyy/MM/"
                                            Enabled="True" Format="yyyy/MM/dd" TargetControlID="txtEnd_Co_PostDT" TodaysDateFormat="yyyy/MM/dd"
                                            OnClientDateSelectionChanged="EEndToLocalDate" PopupButtonID="calendar4">
                                        </asp:CalendarExtender>
                                        &nbsp;<asp:Image ID="calendar4" runat="server" ImageUrl="~/Image/calendar.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="80px" class="style1">
                                        發文字號
                                    </td>
                                    <td width="5px" class="style1">
                                        ：
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtCo_Word" runat="server" CssClass="Area" AutoCompleteType="Disabled"
                                            Width="140px"></asp:TextBox>
                                    </td>
                                    <td class="style1">
                                        字
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtCo_SNO" runat="server" CssClass="Area" AutoCompleteType="Disabled"
                                            Width="140px"></asp:TextBox>
                                    </td>
                                    <td width="20px" class="style1">
                                        號
                                    </td>
                                    <td width="70px" class="style1">
                                        &nbsp;
                                    </td>
                                    <td width="5px" class="style1">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <table id="btnList">
                                    <tr>
                                        <td>
                                            <div class="Button_Up">
                                                <asp:Button runat="server" Text="三天公告" ID="btnSearchIn3Day" OnClientClick="Clear();">
                                                </asp:Button></div>
                                        </td>
                                        <td>
                                            <div class="Button_Up">
                                                <asp:Button runat="server" Text="一個月內" ID="btnSearchIn1Month" OnClientClick="Clear();">
                                                </asp:Button></div>
                                        </td>
                                        <td>
                                            <div class="Button_Up">
                                                <asp:Button runat="server" Text="條件查尋" ID="btnSearch"></asp:Button></div>
                                        </td>
                                        <td>
                                            <div class="Button_Up">
                                                <asp:Button runat="server" Text="清    除" ID="btnClear" OnClientClick="Clear();return false;">
                                                </asp:Button></div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </td>
                    </tr>
                    <tr class="style3">
                        <td class="style3">
                            人員模式：不需插上單位憑證卡即可進入系統，查詢瀏覽國防部電子公布欄公文。<asp:DropDownList ID="ddlOr_orgname" runat="server"
                                AppendDataBoundItems="True" CssClass="Area" DataTextField="or_orgname" DataValueField="or_orgno"
                                Width="303px" Visible="false">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="style3">
                        <td class="style3">
                            單位模式：請先插上單位憑證卡，經過驗證合法後即進入系統，查詢預覽國防部電子公布欄公文，可下載傳送。
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="float: left; width: 50%; font-size: 10pt; color: rgb(79, 116, 170)">
                                目前線上人數：<asp:Label ID="onlineUserLab" runat="server" Text=""></asp:Label>
                                &nbsp;&nbsp; 總筆數：<asp:Label ID="rowCountLab" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="Button_Up" id="divDownBtn" runat="server" style="float: right">
                                <asp:Button runat="server" Text="批次下載" ID="btnDownloadAll" OnClientClick="downloadAllCheck();">
                                </asp:Button></div>
                        </td>
                    </tr>
                    <tr class="style2">
                        <td class="style2">
                            <cc1:MarkGridView runat="server" ID="gvList" class="style2" AutoGenerateColumns="False"
                                Width="100%" AllowPaging="True" PageSize="20" BorderWidth="1px" AllowSorting="True"
                                CellPadding="1" EmptyDataText="無資料." EmptyShowHeader="True">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
                                <Columns>
                                    <asp:BoundField DataField="or_orgname" HeaderText="發文機關">
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="100px" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="11pt" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="co_dtype" HeaderText="文別" ControlStyle-Font-Size="11pt">
                                        <ControlStyle Width="40px" />
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" Font-Size="11pt" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="發文字號" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerial" runat="server" Text='<%# Eval("CO_WORD") + "字第" + Eval("CO_SNO") + "號" %>'></asp:Label>
                                            <asp:HiddenField ID="hid_CO_WORD" runat="server" Value='<%# Eval("CO_WORD")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hid_CO_SNO" Value='<%# Eval("CO_SNO") %>' runat="server" />
                                            <asp:HiddenField ID="hid_OR_ORGNO" Value='<%# Eval("OR_ORGNO") %>' runat="server" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="100px" />
                                        <ItemStyle HorizontalAlign="left" Width="100px" Font-Size="11pt" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="主旨" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnSubj" runat="server" Text='<%# Eval("CO_SUBJ") %>'></asp:LinkButton>
                                            <asp:HiddenField ID="hidCO_PATH" runat="server" Value='<%# Eval("CO_PATH") %>' />
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("CO_PATH") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="380px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="發文日期" SortExpression="CO_ISSUDT" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsuuDT" runat="server" Text='<%# WestYearToChineseYear(String.Format(Eval("CO_ISSUDT"), "{0:yyyy/MM/dd}"), "/") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" Font-Size="11pt" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="公告日期" SortExpression="CO_POSTDT" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPostDT" runat="server" Text='<%# WestYearToChineseYear(String.Format(Eval("CO_POSTDT"), "{0:yyyy/MM/dd}"), "/") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" Font-Size="11pt" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="狀態" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsDownload" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidCO_SEQNO" runat="server" Value='<%# Eval("CO_SEQNO") %>' />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="65px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="附件">
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="120px" />
                                        <ItemStyle HorizontalAlign="left" Width="120px" Font-Size="11pt" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="顯示" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Button ID="btnShowDetail" runat="server" Text="明細" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="55px" />
                                        <ItemStyle HorizontalAlign="Center" Width="55px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="下載" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDownload" runat="server" Text="" CommandName="download" CssClass="Download"
                                                OnClientClick="downloadCheck(this);" />
                                            <asp:HiddenField ID="hidCO_SNO" Value='<%# Eval("CO_SNO") %>' runat="server" />
                                            <asp:HiddenField ID="hidOR_ORGNO" Value='<%# Eval("OR_ORGNO") %>' runat="server" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="55px" />
                                        <ItemStyle HorizontalAlign="Center" Width="55px" Font-Size="11pt" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="全選" ControlStyle-Font-Size="11pt">
                                        <HeaderTemplate>
                                            全選
                                            <asp:CheckBox ID="cbAllSelect" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelect" runat="server" />
                                        </ItemTemplate>
                                        <ControlStyle />
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Button"  CancelText="" EditText="" ShowDeleteButton="True" />
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" />
                            </cc1:MarkGridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
