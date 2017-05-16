<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PortalContent.aspx.vb" Inherits="AnnouncesBoard.PortalContent" %>

<%@ Register assembly="MarkGridView" namespace="MarkGridView.WebControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        #SearchTable
        {
            margin: 0 auto 0 auto;
        }
                
        *
        {
            margin: 0px;
            padding: 0px;
        }
        td
        {
            font-size: 12pt;
            line-height: 1.2em;
        }
        .style1
        {
            text-align: left;
            font-size: 11pt;
            margin: 0 auto 0 auto;
        }
        .style1
        {
            text-align: left;
            font-size: 11pt;
            height: 25px;
            margin: 0 auto 0 auto;
        }
        
        .Area
        {
            margin-left: 1px;
        }
        #btnList
        {
            margin: 0 auto 0 auto;
        }
        
        #btnList td
        {
            padding: 0 10px 0 10px;
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
                
        .style3
        {
            text-align: left;
            font-size: 8pt;
            margin: 0 auto 0 auto;
            color: #800000;
            vertical-align: middle;
        }
        .style2
        {
            text-align: center;
            font-size: 11pt;
            margin: 0 auto 0 auto;
        }
            
        .style2
        {
            text-align: center;
            font-size: 11pt;
            margin: 0 auto 0 auto;
            border-color: Gray;
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
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <table id="SearchTable" width="95%">
        <tr>
            <td>
                &nbsp;
                <table class="style1">
                    <tr>
                        <td>
                            
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
                            <div class="Button_Up" id="divDownBtn" runat="server" style="display:none;float: right">
                                <asp:Button runat="server" Text="批次下載" ID="btnDownloadAll" 
                                    OnClientClick="downloadAllCheck();" Visible="False">
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
                                    <asp:TemplateField HeaderText="公告日期" SortExpression="CO_POSTDT" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPostDT" runat="server" Text='<%# WestYearToChineseYear(String.Format(Eval("CO_POSTDT"), "{0:yyyy/MM/dd}"), "/") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" Font-Size="11pt" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="發文字號" ControlStyle-Font-Size="11pt">
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="主旨">
                                        <ItemTemplate>
                                            
                                            <asp:LinkButton ID="lbtnSubj" runat="server" Text='<%# Eval("CO_SUBJ") %>'></asp:LinkButton>
                                            <asp:HiddenField ID="hidCO_PATH" runat="server" 
                                                Value='<%# Eval("CO_PATH") %>' />
                                            
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt" />
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="380px" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>


                                    <asp:BoundField DataField="or_orgname" HeaderText="發文機關">
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="100px" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="11pt" Width="100px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="公告人員" ControlStyle-Font-Size="11pt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerial" runat="server" 
                                                Text='<%# Eval("CO_WORD") + "字第" + Eval("CO_SNO") + "號" %>' />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="11pt"></ControlStyle>
                                        <HeaderStyle BackColor="#4F74AA" BorderColor="Gray" BorderWidth="1px" ForeColor="White"
                                            Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="11pt" Width="100px" />
                                    </asp:TemplateField>


                                </Columns>
                                <PagerStyle HorizontalAlign="Center" />
                            </cc1:MarkGridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
