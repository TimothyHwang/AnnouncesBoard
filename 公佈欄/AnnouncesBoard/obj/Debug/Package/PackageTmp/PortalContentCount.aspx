<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PortalContentCount.aspx.vb" Inherits="AnnouncesBoard.PortalContentCount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-image: url(../image/bg_mid03.gif);
        }
        table, th, td
        {
            border: 0px solid black;
            border-right: #0000ff;
            border-left: #0000ff;
            white-space: nowrap;
            margin: 0 0 0 0;
            padding: 0 0 0 0;
            color: #666666;
            font-size: small;
        }
        table
        {
            border-spacing: 0;
            border-width: 0px;
        }
        .whitetext
        {
            color: #ffffff;
        }
        .litebluetext
        {
            color: #00bfff;
        }
    </style>
    <script src="../script/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OnLoadEvent() {
            if (parent.document.getElementById('noticeFrame').src.indexOf('NoticeBoard.aspx?flag=1') > -1) {
                document.body.style.backgroundImage = "url(../image/bg_left03.gif)";
                parent.document.getElementById('noticeFrameMinistryAnnCount').contentWindow.document.body.style.backgroundImage = "url(../image/bg_mid03.gif)";
                parent.document.getElementById('noticeFrameMinistryDepCount').contentWindow.document.body.style.backgroundImage = "url(../image/bg_mid03.gif)";
                parent.document.getElementById('noticeFrameMinistrySysCount').contentWindow.document.body.style.backgroundImage = "url(../image/bg_mid03.gif)";
            } else {
                document.body.style.backgroundImage = "url(../image/bg_mid03.gif)";
            }
        }
        function lnkNumber_Click() {
            parent.document.getElementById('noticeFrame').src = 'http://10.22.8.159/moa/Portal/NoticeBoard.aspx?flag=1';
        }
    </script>
</head>
<body onload="OnLoadEvent();">
    <form id="form1" runat="server">
    <div style="position:absolute;top:9px;left:100px">
        <span class="whitetext" style="font-size:small">
        
            國防部公告(<asp:LinkButton ID="lnkNumber" runat="server" OnClientClick="javascript:lnkNumber_Click();" CssClass="litebluetext" Font-Size="Small">0</asp:LinkButton>
            )
            </span>
    </div>
    </form>
</body>
</html>
