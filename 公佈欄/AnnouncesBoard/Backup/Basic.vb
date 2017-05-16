Imports System.Data.OracleClient
' 2011/07/01 移除外包的DB Conn dll (DDTek.Oracle) by NaNa
Public Class Basic
    Public Conn As New OracleConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
    Public Function ODataAdapter(ByVal SqlCommandText As String) As DataTable
        Conn.Open()
        Dim dt As New DataTable
        Dim da As New OracleDataAdapter(SqlCommandText, Conn)

        Try
            da.Fill(dt)
        Catch
        End Try
        da.Dispose()
        Conn.Close()
        Return dt
    End Function

    Public Sub ODataExecute(ByVal SqlCommandText As String)
        Conn.Open()
        Dim Com As New OracleCommand(SqlCommandText, Conn)
        Com.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Public Shared Function HtmlEncode(ByVal Text As String) As String
        Return System.Web.HttpUtility.HtmlEncode(Text).Replace("'", "").Replace(" ", "")
    End Function
End Class
