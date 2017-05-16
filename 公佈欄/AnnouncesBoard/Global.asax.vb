Imports System.Web.SessionState
Imports System.Data
Imports System.Data.SqlClient


Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 在應用程式啟動時引發

        '建立全域變數並初始化0
        Application("online") = 0
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 在工作階段啟動時引發

        'Session存活時間，若需Login則不適用
        'Session.Timeout = 1
        'Application鎖定後，只有此session能夠對話
        Application.Lock()
        Application("online") = CInt(Application("online")) + 1
        'UpdateCount(1)
        '對話完畢後，Application解鎖
        Application.UnLock()
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 在每個要求開頭引發
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 在一開始嘗試驗證使用時引發
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' 在錯誤發生時引發
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 在工作階段結束時引發

        Application.Lock()
        Application("online") = CInt(Application("online")) - 1
        Application.UnLock()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 在應用程式結束時引發

        Application("online") = 0
        'UpdateCount(2)
    End Sub

    Public Sub UpdateCount(ByVal state As String)
        '更新資料庫語法
    End Sub


End Class