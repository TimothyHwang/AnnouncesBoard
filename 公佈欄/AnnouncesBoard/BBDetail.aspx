<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MP.Master"
    CodeBehind="BBDetail.aspx.vb" Inherits="AnnouncesBoard.BBDetail" %>

<%@ Register Assembly="MarkGridView" Namespace="MarkGridView.WebControls" TagPrefix="cc1" %>

<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #Detail
        {
            margin: 30px auto 0 auto;
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
        $("form").ready(function () {
            $("#DownLog").attr("style", "display:none;");
//            $("#ctl00_ContentPlaceHolder1_btnDownLog").toggle(function () {
//                $("#DownLog").attr("style", "");
//            }, function () {
//                $("#DownLog").attr("style", "display:none;");
//            });

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
    <script type="text/javascript" language="javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Detail">
        <tr>
            <td valign="top" width="834px">
                <table width="85%">
                    <tr>
                        <td width="968px">
                            <table style="width: 832px">
                                <tr>
                                    <td width="184px">
                                        發文機關：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOr_orgname" runat="server" Width="200px" BorderWidth="1px"></asp:Label>
                                    </td>
                                    <td width="116px" align="right">
                                        承辦人：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCo_postnm" runat="server" Width="100px" BorderWidth="1px"></asp:Label>
                                    </td>
                                    <td width="143px">
                                        發文日期：
                                    </td>
                                    <td width="176px" align="left">
                                        <asp:Label ID="lblCo_issudt" runat="server" Width="100px" BorderWidth="1px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="184px">
                                        文別：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCo_dtype" runat="server" Width="100px" BorderWidth="1px"></asp:Label>
                                    </td>
                                    <td width="116px" align="right">
                                        發文字號：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCo_word_sno" runat="server" Width="200px" BorderWidth="1px"></asp:Label>
                                    </td>
                                    <td width="143px" align="left">
                                        公告日期：
                                    </td>
                                    <td width="176px">
                                        <asp:Label ID="lblCo_postdt" runat="server" Width="100px" BorderWidth="1px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="184px">
                                        主旨：
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblCo_subj" runat="server" Height="100px" Width="692px" BorderWidth="1px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="184px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="116px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="143px">
                                        &nbsp;
                                    </td>
                                    <td width="176px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="184px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="116px">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td width="143px">
                                        &nbsp;
                                    </td>
                                    <td width="176px">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="968px" valign="top" align="center">
                            <cc1:MarkGridView ID="gvDetailList" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                Width="84%" PageSize="5" AllowPaging="True" ShowFooter="True">
                                <Columns>
                                    <asp:BoundField HeaderText="項次" DataField="Rownum">
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="#CEF3FF" BorderWidth="2px" ForeColor="White"
                                            Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="文件種類" DataField="At_ftype">
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="#CEF3FF" BorderWidth="2px" ForeColor="White"
                                            Width="150px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="說明" DataField="At_fdesc">
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="#CEF3FF" BorderWidth="2px" ForeColor="White"
                                            Width="300px" />
                                    </asp:BoundField>

                                </Columns>
                            </cc1:MarkGridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div class="Button_Up">
                                <asp:Button runat="server" Text="下載單位資訊" ID="btnDownLog"></asp:Button></div>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center">
                            <div id="DownLog">
                                <cc1:MarkGridView ID="gvDownRecordList" runat="server" 
                                    AutoGenerateColumns="False" EnableModelValidation="True"
                                    Width="500px" EmptyShowHeader="True" AllowPaging="True">
                                    <Columns>
                                        <asp:BoundField HeaderText="下載單位" DataField="UnitName">
                                            <HeaderStyle BackColor="#4F74AA" BorderColor="#CEF3FF" BorderWidth="2px" ForeColor="White" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="下載日期">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# WestYearToChineseYear(String.Format(Eval("DownloadDate"), "{0:yyyy/MM/dd}"), "/") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#4F74AA" BorderColor="#CEF3FF" BorderWidth="2px" 
                                                ForeColor="White" HorizontalAlign="Center" Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </cc1:MarkGridView>
                                </div>
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

