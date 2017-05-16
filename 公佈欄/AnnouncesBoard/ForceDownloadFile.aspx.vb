Public Class ForceDownloadFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SavePath As String = "C:\odedi3\rcv\"
        Dim FileName As String = Request.QueryString("filename").ToString
        Dim FilePath As String = SavePath + FileName

        Dim SaveAttrachPath As String = "C:\odedi3\rcv\"
        Dim AttachFileName As String() = Request.QueryString("attfilename").ToString.Split("|")
        Dim AttachFilePath As String = "" 'SavePath + FileName

        '設定表頭並存檔
        '附件檔
        For Each s In AttachFileName
            AttachFilePath = SavePath + s
            If System.IO.File.Exists(AttachFilePath) Then
                With Response
                    .ContentType = "application/save-as"
                    .AddHeader("content-disposition", "attachment; filename=" & FileName)
                    .ContentEncoding = Encoding.UTF8
                    .WriteFile(AttachFilePath)
                End With
            Else
                Exit Sub
            End If
        Next
        'di
        If System.IO.File.Exists(FilePath) Then
            With Response
                .ContentType = "application/save-as"
                .AddHeader("content-disposition", "attachment; filename=" & FileName)
                .ContentEncoding = Encoding.UTF8
                .WriteFile(FilePath)
            End With
        Else
            Exit Sub
        End If
        

    End Sub

End Class