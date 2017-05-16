<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MP.Master"
    CodeBehind="BBVeriSign.aspx.vb" Inherits="AnnouncesBoard.BBVeriSign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <object id="APMCer" codebase="APMCer.cab#version=1,0,0,1" classid="CLSID:77DF8C7B-F505-433e-B768-22DD05B4AC1C">
    </object>
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_pnlUnitCheck
        {
            margin: 50px auto 0 auto;
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
    <script type="text/javascript">
        var data;
        $(function () {
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

            try {
                $("#ctl00_ContentPlaceHolder1_lblMessage").text("讀取單位卡憑證中，請稍後 ...");
                data = APMCer.Read();
                data = '1,1,1,1'; //測試資料
                if (data == -1) {
                    $("#ctl00_ContentPlaceHolder1_lblMessage").text("讀取卡片失敗 ...(請確認讀卡機驅動只能安裝一個!)");
                    $("#ctl00_ContentPlaceHolder1_btnReCheck").attr("disabled", false);
                    $("#ctl00_ContentPlaceHolder1_btnreturnLogin").attr("disabled", false);
                }
                else {
                    PageMethods.JVVar2Server(data, chkCRL, null);
                    $("#ctl00_ContentPlaceHolder1_lblMessage").text("讀卡成功，驗證撤憑證中 ...");
                }
            }
            catch (e) {
                $("#ctl00_ContentPlaceHolder1_lblMessage").text("ActiveX無回應 ...(請確認是否安裝APMCer驅動程式!)");
                $("#ctl00_ContentPlaceHolder1_btnReCheck").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_btnreturnLogin").attr("disabled", false);
            }
        });
        function chkCRL(result, context, CheckCRL) {
            result = 0;//測試資料
            if (result == 0) {
                $("#ctl00_ContentPlaceHolder1_lblMessage").text("此憑證未被撤銷，準備轉址中 ...");
                //setTimeout("location.href = 'BBContent.aspx?action=login'", 3000);
                setTimeout("location.href = 'SaveData.aspx?carddata=' + data", 3000);
            }
            else if (result == -1) {
                $("#ctl00_ContentPlaceHolder1_lblMessage").text("系統驗證CRL憑證失敗，請聯絡系統管理員 ...");
                $("#ctl00_ContentPlaceHolder1_btnReCheck").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_btnreturnLogin").attr("disabled", false);
            }
            else {
                $("#ctl00_ContentPlaceHolder1_lblMessage").text("此憑證已被撤銷，禁止登入 ...");
                $("#ctl00_ContentPlaceHolder1_btnReCheck").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_btnreturnLogin").attr("disabled", false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <center>
        <asp:Panel ID="pnlUnitCheck" runat="server" GroupingText="單位卡驗證" Width="700px" ForeColor="#3366FF">
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="系統處理訊息顯示區域。"></asp:Label>
            <br />
            <br />
            <div class="Button_Up">
                <asp:Button ID="btnReCheck" runat="server" Text="重新讀取" Enabled="false" />
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <div class="Button_Up">
                <asp:Button ID="btnreturnLogin" runat="server" Text="取消登入" Enabled="False" />
            </div>
            <br />
        </asp:Panel>
    </center>
</asp:Content>
