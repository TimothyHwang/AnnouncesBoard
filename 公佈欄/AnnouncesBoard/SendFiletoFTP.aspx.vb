Imports System.IO
Imports System.Net

Public Class SendFiletoFTP
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        CopyFileToFtpPath("D:\Work\Prj\AnnouncesBoard\snd\SI039901-0990100001.di", True) 'true是附件
    End Sub
    Private Shared Sub CopyFileToFtpPath(ByVal fullFilenm As String, ByVal fileType As Boolean)

        Dim requestStream As Stream = Nothing
        Dim fileStream As FileStream = Nothing
        Dim uploadResponse As FtpWebResponse = Nothing
        Dim myInfo As FileInfo
        Dim strFileName As String

        Try
            myInfo = New FileInfo(fullFilenm)
            strFileName = myInfo.Name

            'Dim swS As New StreamWriter(fullFilenm, True, System.Text.Encoding.Default)
            'swS.WriteLine("加字")
            'swS.Flush()
            'swS.Close()

            '取得WebConfig裡的設定
            Dim Address As String = ConfigurationManager.AppSettings("ftpAddress") & strFileName
            Dim UserName As String = ConfigurationManager.AppSettings("ftpUserName")
            Dim Password As String = ConfigurationManager.AppSettings("ftpPassword")

            If fileType Then
                Address = ConfigurationManager.AppSettings("ftpAddress") & "attch/" & strFileName
            End If
            '要給檔案路徑
            Dim uploadRequest As FtpWebRequest = DirectCast(WebRequest.Create(Address), FtpWebRequest)

            '設定Method上傳檔案
            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile

            'Dim myProxy As WebProxy = New WebProxy("999.99.999.99", 99999)
            'myProxy.Credentials = New NetworkCredential("UID", "PWD", "Domain")

            uploadRequest.Proxy = Nothing

            If UserName.Length > 0 Then
                '如果需要帳號登入
                Dim nc As New NetworkCredential(UserName, Password)
                '設定帳號
                uploadRequest.Credentials = nc

            End If

            requestStream = uploadRequest.GetRequestStream()
            fileStream = File.Open(fullFilenm, FileMode.Open)

            Dim buffer As Byte() = New Byte(1024) {}
            Dim bytesRead As Integer

            While True
                '開始上傳資料流
                bytesRead = fileStream.Read(buffer, 0, buffer.Length)


                If bytesRead = 0 Then
                    Exit While
                End If


                requestStream.Write(buffer, 0, bytesRead)


            End While

            requestStream.Close()
            uploadResponse = DirectCast(uploadRequest.GetResponse(), FtpWebResponse)

            fileStream.Flush()
            fileStream.Close()

            'File.Delete(fullFilenm) 保留檔案

        Catch ex As Exception
            'lblMessage.Text = ex.Message
            Throw ex

        Finally
            If uploadResponse IsNot Nothing Then
                uploadResponse.Close()

            End If

            If fileStream IsNot Nothing Then
                fileStream.Close()

            End If

            If requestStream IsNot Nothing Then
                requestStream.Close()

            End If

        End Try
    End Sub

End Class