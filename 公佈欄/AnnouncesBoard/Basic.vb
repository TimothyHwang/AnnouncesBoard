Imports Oracle.DataAccess.Client

' 2011/07/01 移除外包的DB Conn dll (DDTek.Oracle) by NaNa
Public Class Basic    
    'Public Conn As New OracleConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
    Dim oraclePath As String = Environment.CurrentDirectory + "\Oracle\"
    Public connStr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Dim envStr As String = Environment.GetEnvironmentVariable("PATH")

    Public Function ODataAdapter(ByVal SqlCommandText As String) As DataTable
        'Conn.Open()
        Dim dt As New DataTable
        'Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(SqlCommandText, Conn)
        Dim da As System.Data.OracleClient.OracleDataAdapter

        Try

            '' 此处被注释的代码为ORACLE的环境设置。下文说明作用。
            'nvironment.SetEnvironmentVariable("PATH", oraclePath, EnvironmentVariableTarget.Process)
            'nvironment.SetEnvironmentVariable("ORACLE_HOME", oraclePath, EnvironmentVariableTarget.Process)
            'nvironment.SetEnvironmentVariable("LD_LIBRARY_PATH", oraclePath, EnvironmentVariableTarget.Process)
            'nvironment.SetEnvironmentVariable("NLS_LANG", "SIMPLIFIED CHINESE_CHINA.ZHS16GBK", EnvironmentVariableTarget.Process)
            'nvironment.SetEnvironmentVariable("TNS_ADMIN", oraclePath, EnvironmentVariableTarget.Process)

            Using Conn As New System.Data.OracleClient.OracleConnection(connStr)
                Conn.Open()
                da = New System.Data.OracleClient.OracleDataAdapter(SqlCommandText, Conn)
                da.Fill(dt)
                da.Dispose()
                Conn.Dispose()
            End Using

        Catch

        End Try

        Return dt
    End Function

    Public Sub ODataExecute(ByVal SqlCommandText As String)
        Using Conn As New System.Data.OracleClient.OracleConnection(connStr)
            Conn.Open()
            Dim Com As New System.Data.OracleClient.OracleCommand(SqlCommandText, Conn)
            Com.ExecuteNonQuery()
            Conn.Close()
        End Using

        'Conn.Open()
        'Dim Com As New OracleCommand(SqlCommandText, Conn)
        'Com.ExecuteNonQuery()
        'Conn.Close()
    End Sub

    Public Shared Function HtmlEncode(ByVal Text As String) As String
        Return System.Web.HttpUtility.HtmlEncode(Text).Replace("'", "").Replace(" ", "")
    End Function

End Class
