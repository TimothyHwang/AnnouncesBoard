Imports System.IO
Imports System.Xml
Imports System.Web
Imports System
Imports System.Diagnostics
Imports System.Xml.XPath
'Imports SevenZip.Sdk
'Imports SevenZip
Imports Microsoft.Win32
Imports Oracle.DataAccess.Client

Public Class PublicFunc
    ''' <summary>
    ''' 搜尋日期(天)內的資料
    ''' </summary>
    ''' <param name="ContainsDays">Integer</param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function Search(ByVal ContainsDays As Integer) As DataTable
        Dim SqlCmd As String = String.Empty
        If IsNothing(ContainsDays) Then
            ContainsDays = 0
        End If
        SqlCmd = String.Format("select rownum, t.* from (select distinct a.or_orgno, a.or_orgname, b.co_word, b.co_dtype, b.co_sno, b.co_subj, b.co_issudt, b.co_postdt, b.co_path, b.co_seqno from organ a, content b where a.or_orgno = b.co_orgno and co_postdt between to_date('{0}','yyyy/mm/dd') and to_date('{1}','yyyy/mm/dd') order by co_postdt desc, co_issudt desc, co_word, co_sno, or_orgname)t", Now.AddDays(ContainsDays * -1).ToString("yyyy/MM/dd"), Now.ToString("yyyy/MM/dd"))
        Return New Basic().ODataAdapter(SqlCmd)
    End Function

    ''' <summary>
    ''' 搜尋一個月內，或歷史查詢
    ''' </summary>
    ''' <param name="co_orgname">查詢的CO_ORGNO</param>
    ''' <param name="co_subj">查詢的CO_SUBJ</param>
    ''' <param name="co_word">查詢的CO_WORD</param>
    ''' <param name="co_sno">查詢的CO_SNO</param>
    ''' <param name="startIssudt">發布起日期</param>
    ''' <param name="endIssudt">發布迄日期</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Search(ByVal co_orgname As String, ByVal co_subj As String, ByVal co_word As String, ByVal co_sno As String, ByVal startIssudt As String, ByVal endIssudt As String, ByVal startPostdt As String, ByVal endPostdt As String, ByVal GuideFlag As Boolean) As DataTable
        '防Sql injection
        co_orgname = Basic.HtmlEncode(co_orgname)
        co_subj = Basic.HtmlEncode(co_subj)
        co_word = Basic.HtmlEncode(co_word)
        co_sno = Basic.HtmlEncode(co_sno)
        startIssudt = Basic.HtmlEncode(startIssudt)
        endIssudt = Basic.HtmlEncode(endIssudt)

        Dim SqlCmd As String = "select rownum, t.* from (select distinct a.or_orgno, a.or_orgname, b.co_word, b.co_dtype, b.co_sno, b.co_subj, b.co_issudt, b.co_postdt, b.co_path, b.co_seqno from organ a, content b where a.or_orgno = b.co_orgno"

        ''機關代碼
        'If Not String.IsNullOrEmpty(co_orgno) Then
        '    SqlCmd += String.Format(" and co_orgno = '{0}'", co_orgno)
        'End If

        '機關名稱
        If Not String.IsNullOrEmpty(co_orgname) Then
            SqlCmd += String.Format(" and or_orgname like '%{0}%'", co_orgname)
        End If

        '主旨
        If Not String.IsNullOrEmpty(co_subj) Then
            SqlCmd += String.Format(" and co_subj like '%{0}%'", co_subj)
        End If
        '發文字
        If Not String.IsNullOrEmpty(co_word) Then
            SqlCmd += String.Format(" and co_word = '{0}'", co_word)
        End If
        '發文號
        If Not String.IsNullOrEmpty(co_sno) Then
            SqlCmd += String.Format(" and co_sno = '{0}'", co_sno)
        End If

        If Not String.IsNullOrEmpty(startIssudt) And Not String.IsNullOrEmpty(endIssudt) Then
            SqlCmd += String.Format(" and co_issudt between to_date('{0}','yyyy/mm/dd') and to_date('{1}','yyyy/mm/dd')", startIssudt, endIssudt)
        End If

        If Not String.IsNullOrEmpty(startPostdt) And Not String.IsNullOrEmpty(endPostdt) Then
            SqlCmd += String.Format(" and co_postdt between to_date('{0}','yyyy/mm/dd') and to_date('{1}','yyyy/mm/dd')", startPostdt, endPostdt)
        End If

        If GuideFlag Then
            SqlCmd += String.Format(" AND CO_GUIDEFLAG={0}", 1)
        End If

        SqlCmd += " order by co_postdt desc, co_issudt desc, co_word, co_sno, or_orgname)t"
        Return New Basic().ODataAdapter(SqlCmd)
    End Function

    ''' <summary>
    ''' 取得發布單位
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrganData() As DataTable
        Return New Basic().ODataAdapter("select distinct * from organ where or_orgno in(select distinct co_orgno from content)")
    End Function

    ''' <summary>
    ''' 取得單位代碼
    ''' </summary>
    ''' <param name="UnitName">卡片單位名稱</param>
    ''' <returns>有資料回傳單位代碼，無資料為空</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgno(ByVal UnitName As String) As String
        '防Sql injection
        UnitName = Basic.HtmlEncode(UnitName)
        'UnitName = UnitName.Replace("'", "").Replace(" ", "")

        'Dim SqlCmd As String = String.Format("select or_orgno from organ where or_unitname = '{0}'", UnitName)
        'Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
        'If dt.Rows.Count = 0 Then
        '    Return ""
        'Else
        '    Return dt.Rows(0)("or_orgno")
        'End If        
        Using Conn As New System.Data.OracleClient.OracleConnection(New Basic().connStr)

            Dim SqlCmd As String = String.Format("select or_orgno from organ where or_unitname = '{0}'", UnitName)
            Dim cmd As System.Data.OracleClient.OracleCommand = New System.Data.OracleClient.OracleCommand(SqlCmd, Conn)
            Dim dr As System.Data.OracleClient.OracleDataReader

            cmd.Connection.Open()
            dr = cmd.ExecuteReader()
            If (dr.Read()) Then
                GetOrgno = dr("or_orgno")
            Else
                GetOrgno = ""
            End If
            dr.Close()
            cmd.Connection.Close()
        End Using
    End Function

    ''' <summary>
    ''' 取得單位名稱
    ''' </summary>
    ''' <param name="UnitCode">單位代碼</param>
    ''' <returns>有資料回傳單位名稱，無資料為空</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgname(ByVal UnitCode As String) As String
        '防Sql injection
        UnitCode = Basic.HtmlEncode(UnitCode)

        Dim SqlCmd As String = String.Format("select distinct or_orgname from organ where or_orgno = '{0}'", UnitCode)
        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
        If dt.Rows.Count = 0 Then
            Return ""
        Else
            Return dt.Rows(0)("or_orgname")
        End If
    End Function

    ''' <summary>
    ''' 取得單位名稱
    ''' </summary>
    ''' <param name="Unitname">單位代碼</param>
    ''' <returns>有資料回傳單位名稱，無資料為空</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrg_Unitname(ByVal Unitname As String) As String
        '防Sql injection
        Unitname = Basic.HtmlEncode(Unitname)

        Dim SqlCmd As String = String.Format("select distinct or_unitname from organ where or_orgname = '{0}'", Unitname)
        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
        If dt.Rows.Count = 0 Then
            Return ""
        Else
            Return dt.Rows(0)("or_unitname")
        End If
    End Function

    ''' <summary>
    ''' 取得AT_SEQNO
    ''' </summary>
    ''' <param name="Path">AT_PATH 或 CO_PATH</param>
    ''' <returns>AT_SEQNO 或 空值</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAT_SEQNO(ByVal Path As String) As String
        '防Sql injection
        Path = Basic.HtmlEncode(Path)

        Dim SqlCmd As String = String.Format("select at_seqno from attach where at_ftype=1 and at_path = '{0}'", Path)

        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
        If dt.Rows.Count = 0 Then
            Return ""
        Else
            Return dt.Rows(0)("at_seqno")
        End If
    End Function

    ''' <summary>
    ''' 取得AT_FDESC
    ''' </summary>
    ''' <param name="Seqno">seqno</param>
    ''' <returns>AT_FDESC 或 空值</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAT_FDESC(ByVal Seqno As String) As String
        '防Sql injection
        Seqno = Basic.HtmlEncode(Seqno)

        Dim SqlCmd As String = String.Format("select at_fdesc from attach where at_seqno = '{0}'", Seqno)

        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
        If dt.Rows.Count = 0 Then
            Return ""
        Else
            If IsDBNull(dt.Rows(0)("at_fdesc")) Then
                Return ""
            Else
                Return dt.Rows(0)("at_fdesc")
            End If
        End If
    End Function


    ''' <summary>
    ''' 下載SW
    ''' </summary>
    ''' <param name="FileName">完整路徑檔名 如C:\ODEDI3\SND\Attch\5874748926-1030000001.sw</param>
    ''' <param name="UnitName">下載單位名稱 (OU)</param>
    ''' <param name="UnitCode">下載單位代碼</param>
    ''' <remarks></remarks>
    Private Shared Sub WriteSW(ByVal FileName As String, ByVal UnitName As String, ByVal UnitCode As String)
        Dim writer As XmlTextWriter = New XmlTextWriter(FileName, Encoding.GetEncoding("BIG5"))
        'Use indenting for readability.
        writer.Formatting = Formatting.Indented

        'Write the XML delcaration. 
        writer.WriteStartDocument()

        writer.WriteDocType("交換表單", Nothing, "99_roster_big5.dtd", Nothing)

        writer.WriteStartElement("交換表單")
        writer.WriteElementString("全銜", UnitName)
        writer.WriteElementString("機關代碼", UnitCode)
        writer.WriteEndElement()

        writer.WriteEndDocument()

        writer.Flush()
        writer.Close()
    End Sub

    ''' <summary>
    ''' 下載DI
    ''' </summary>
    ''' <param name="OutPutFilePath">存檔路徑 如C:\TEMP</param>
    ''' <param name="AT_PATH">附件AT_PATH</param>
    ''' <remarks></remarks>
    Private Shared Sub WriteDIdata(ByVal OutPutFilePath As String, ByVal AT_PATH As String, ByVal OutputUnitCode As String)
        ' ''壓縮檔案供下載 2014/5/21 Paul
        'Dim SqlCmd As String = String.Format("select at_data from attach where at_ftype = 1 and at_path = '{0}' ", AT_PATH)
        'Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)

        'SqlCmd = String.Format("select at_filenm from attach where at_ftype = 2 and at_path = '{0}' ", AT_PATH)
        'Dim dt1 As DataTable = New Basic().ODataAdapter(SqlCmd)

        'For Each dr As DataRow In dt.Rows
        '    Dim byteBuffer As Byte() = CType(dr("at_data"), Byte())
        '    Dim filename = OutPutFilePath
        '    Dim dirs As String() = filename.Split("\")
        '    ''C:\RCV\ATTACH
        '    Dim TempFile = dirs(0) + "\" + dirs(1) + "\" + dirs(dirs.Length - 1)
        '    Dim fs As FileStream = New FileStream(TempFile, FileMode.Create)
        '    fs.Write(byteBuffer, 0, byteBuffer.Length)
        '    fs.Close()
        '    Dim filenm As String = String.Empty
        '    If dt1.Rows.Count > 0 Then
        '        'If Not IsDBNull(dt1.Rows(0)("at_filenm")) Then
        '        filenm = dt1.Rows(0)("at_filenm").ToString().Split("-")(0)
        '    End If
        '    DIRework(TempFile, filenm, OutputUnitCode, filename)
        'Next

        ''不再直接把檔案放入固定資料夾，由下載使用者自行下載壓縮檔

        Dim SqlCmd As String = String.Format("select at_data from attach where at_ftype = 1 and at_path = '{0}' ", AT_PATH)

        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)

        SqlCmd = String.Format("select at_filenm from attach where at_ftype = 2 and at_path = '{0}' ", AT_PATH)
        Dim dt1 As DataTable = New Basic().ODataAdapter(SqlCmd)

        For Each dr As DataRow In dt.Rows
            Dim byteBuffer As Byte() = CType(dr("at_data"), Byte())
            Dim filename = OutPutFilePath
            Dim dirs As String() = filename.Split("\")
            Dim TempFile = dirs(0) + "\" + dirs(1) + "\" + dirs(dirs.Length - 1)
            Dim fs As FileStream = New FileStream(TempFile, FileMode.Create)
            fs.Write(byteBuffer, 0, byteBuffer.Length)
            fs.Close()
            Dim filenm As String = String.Empty
            If dt1.Rows.Count > 0 Then
                'If Not IsDBNull(dt1.Rows(0)("at_filenm")) Then
                'filenm = dt1.Rows(0)("at_filenm").ToString().Split("-")(0)
            End If
            DIRework(TempFile, filenm, OutputUnitCode, filename)
        Next
    End Sub

    ''' <summary>
    ''' 下載附件
    ''' </summary>
    ''' <param name="FilePath">存檔路徑 如C:\TEMP</param>
    ''' <param name="AT_PATH">附件AT_PATH</param>
    ''' <remarks></remarks>
    Private Shared Sub WriteAttch(ByVal FilePath As String, ByVal AT_PATH As String)
        Try
            Dim SqlCmd As String = String.Format("select at_filenm, at_data from attach where at_ftype = 2 and at_path = '{0}'", AT_PATH)
            Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)

            For Each dr As DataRow In dt.Rows
                Dim byteBuffer As Byte() = CType(dr("at_data"), Byte())
                Dim filename = FilePath 'String.Format("{0}\{1}", FilePath, dr("at_filenm"))
                Dim fileDotname = dr("at_filenm").ToString().Remove(0, dr("at_filenm").ToString().LastIndexOf("-", StringComparison.Ordinal))
                Dim fs As FileStream = New FileStream(filename + fileDotname, FileMode.Create)
                fs.Write(byteBuffer, 0, byteBuffer.Length)
                fs.Close()
                'File.WriteAllText(FilePath + fileDotname, fs.ToString)

                'SFTPsnd("sndattch", "sndattch", filename + fileDotname)
            Next
        Catch ex As Exception
            HttpContext.Current.Response.Write("<script>alert('" & ex.Message & "');</script>")
        End Try

    End Sub

    ''' <summary>
    ''' 下載本文PDF
    ''' </summary>
    ''' <param name="FilePath">存檔路徑 如C:\TEMP</param>
    ''' <param name="CO_ORGNO">附件AT_PATH</param>    
    ''' <remarks></remarks>
    Private Shared Sub WriteCO_DATA(ByVal FilePath As String, ByVal AT_PATH As String, ByVal TitleFileName As String, ByVal CO_ORGNO As String)
        Try
            Dim SqlCmd As String = String.Format("SELECT A.AT_PDFDATA,B.CO_PATH,B.CO_SNO FROM ATTACH A,CONTENT B WHERE A.AT_PATH=B.CO_PATH AND A.AT_FTYPE='1' AND A.AT_PATH='{0}'", AT_PATH)
            Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)

            If dt.Rows.Count > 0 Then
                Dim byteBuffer As Byte() = CType(dt.Rows(0)("AT_PDFDATA"), Byte())
                Dim filename = FilePath 'String.Format("{0}\{1}", FilePath, dr("at_filenm"))                
                Dim fs As FileStream = New FileStream(filename + CO_ORGNO + "-" + dt.Rows(0)("CO_SNO") + "\RCV\ATTCH\" + TitleFileName + "_di.pdf", FileMode.Create)
                fs.Write(byteBuffer, 0, byteBuffer.Length)
                fs.Close()

            End If
        Catch ex As Exception
            HttpContext.Current.Response.Write("<script>alert('" & ex.Message & "');</script>")
        End Try

    End Sub

    ''' <summary>
    ''' 下載資訊Log
    ''' </summary>
    ''' <param name="CO_SEQNO">文件序號</param>
    ''' <param name="CO_WORD">發文字</param>
    ''' <param name="CO_SNO">發文號</param>
    ''' <param name="UnitCode">下載單位代碼</param>
    ''' <remarks></remarks>
    Private Shared Sub InsertDownloadLog(ByVal CO_SEQNO As String, ByVal CO_WORD As String, ByVal CO_SNO As String, ByVal UnitCode As String)
        Dim SqlCmd As String = String.Format("insert into bull_dl_det(DD_SEQNO, DD_DPID, DD_COSEQNO, DD_DATE) values(bull_dl_det_sequence.nextval, '{0}', '{1}', to_date('{2}', 'yyyy/mm/dd'))", UnitCode, CO_SEQNO, Now.ToString("yyyy/MM/dd"))
        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
    End Sub

    ''' <summary>
    ''' 下載
    ''' </summary>
    ''' <param name="Or_orgno">發文單位代碼</param>
    ''' <param name="OutputUnitCode">下載單位代碼</param>
    ''' <param name="UnitName">下載單位</param>
    ''' <param name="CO_SNO">下載號</param>
    ''' <param name="CO_PATH">CO_PATH</param>
    ''' <remarks></remarks>
    Public Shared Sub Download(ByVal Or_orgno As String, ByVal OutputUnitCode As String, ByVal UnitName As String, ByVal CO_SNO As String, ByVal CO_PATH As String)
        Dim FilePath As String = String.Empty
        Dim RndFileTitle As String = DateTime.Now.Ticks.ToString().Substring(8)
        Try

            ''saveFileName = FILENAME + ".zip" ''.Substring(intStart, FILENAME.Length - intStart)
            Dim saveFileName As String = String.Format("{0}", ConfigurationManager.AppSettings.Item("FilePath")) + Or_orgno + "-" + CO_SNO + ".zip"
            'Dim intStart As Integer = FILENAME.LastIndexOf("\", StringComparison.Ordinal) + 1
            Dim fi As FileInfo = New FileInfo(saveFileName)

            Dim D As New Detail()
            D.LoadPathDetail(CO_PATH)
            PublicFunc.InsertDownloadLog(D.Co_seqno, D.Co_word, D.Co_sno, OutputUnitCode)

            If fi.Exists Then
                ''直接下載檔案
                Dim fileextname As String = fi.Extension
                Dim DEFAULT_CONTENT_TYPE As String = "application/octet-stream"
                Dim regkey, fileextkey As RegistryKey

                Dim filecontenttype As String
                Try
                    regkey = Registry.ClassesRoot
                    fileextkey = regkey.OpenSubKey(fileextname)
                    filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString()
                Catch
                    filecontenttype = DEFAULT_CONTENT_TYPE
                End Try

                Using Stream As New MemoryStream
                    Using File As New FileStream(fi.FullName, FileMode.Open, FileAccess.Read)
                        Dim obytes As Byte() = New Byte(File.Length) {}
                        File.Read(obytes, 0, CType(File.Length, Integer))
                        Stream.Write(obytes, 0, CType(File.Length, Integer))
                    End Using
                    Dim content As Byte() = Stream.ToArray()
                    HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + Or_orgno + "-" + CO_PATH + ".zip")
                    HttpContext.Current.Response.ContentType = "application/octet-stream"
                    HttpContext.Current.Response.BinaryWrite(content)
                    HttpContext.Current.Response.End()
                End Using

                'HttpContext.Current.Response.ClearContent()
                'HttpContext.Current.Response.Charset = "utf-8"
                'HttpContext.Current.Response.Buffer = True

                'HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8

                'HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Or_orgno + "-" + CO_SNO + ".zip")
                'HttpContext.Current.Response.ContentType = filecontenttype

                'HttpContext.Current.Response.TransmitFile(fi.FullName)
                'HttpContext.Current.Response.Flush()
                'HttpContext.Current.Response.Close()

            Else
                '檢查SW路徑
                FilePath = String.Format("{0}{1}-{2}\RCV\Attch\", ConfigurationManager.AppSettings.Item("FilePath"), Or_orgno, CO_SNO) ''C:\ODEDI3\SND\Attch\
                If Not Directory.Exists(FilePath) Then
                    Directory.CreateDirectory(FilePath)
                End If
                '產SW檔
                Dim FILENAME As String = String.Format("{0}-{1}", RndFileTitle, CO_SNO) ''檔案名稱：random+文號
                PublicFunc.WriteSW(String.Format("{0}{1}.sw", FilePath, FILENAME), UnitName, OutputUnitCode)
                'SFTPsnd("sndattch", "sndattch", String.Format("{0}{1}.sw", FilePath, FILENAME))

                '檢查附件路徑
                FilePath = String.Format("{0}{1}-{2}\RCV\Attch\", ConfigurationManager.AppSettings.Item("FilePath"), Or_orgno, CO_SNO)
                If Not Directory.Exists(FilePath) Then
                    Directory.CreateDirectory(FilePath)
                End If
                '產附件
                PublicFunc.WriteAttch(String.Format("{0}{1}", FilePath, FILENAME), CO_PATH)

                '檢查DI路徑
                FilePath = String.Format("{0}{1}-{2}\RCV\", ConfigurationManager.AppSettings.Item("FilePath"), Or_orgno, CO_SNO) ''C:\ODEDI3\SND\
                If Not Directory.Exists(FilePath) Then
                    Directory.CreateDirectory(FilePath)
                End If
                '產DI檔
                PublicFunc.WriteDIdata(String.Format("{0}{1}.di", FilePath, FILENAME), CO_PATH, RndFileTitle)
                'SFTPsnd("snddi", "snddi", String.Format("{0}{1}.di", FilePath, FILENAME))

                '產文頭PDF檔
                PublicFunc.WriteCO_DATA(ConfigurationManager.AppSettings.Item("FilePath"), CO_PATH, FILENAME, Or_orgno)

                SevenZip(String.Format("{0}", ConfigurationManager.AppSettings.Item("FilePath")) + Or_orgno + "-" + CO_SNO + ".zip", String.Format("{0}{1}-{2}\RCV\", ConfigurationManager.AppSettings.Item("FilePath"), Or_orgno, CO_PATH), "")

                ''下載檔案
                Dim fileextname As String = fi.Extension
                Dim DEFAULT_CONTENT_TYPE As String = "application/octet-stream"
                Dim regkey, fileextkey As RegistryKey

                Dim filecontenttype As String
                Try
                    regkey = Registry.ClassesRoot
                    fileextkey = regkey.OpenSubKey(fileextname)
                    filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString()
                Catch
                    filecontenttype = DEFAULT_CONTENT_TYPE
                End Try

                Using Stream As New MemoryStream
                    Using File As New FileStream(fi.FullName, FileMode.Open, FileAccess.Read)
                        Dim obytes As Byte() = New Byte(File.Length) {}
                        File.Read(obytes, 0, CType(File.Length, Integer))
                        Stream.Write(obytes, 0, CType(File.Length, Integer))
                    End Using
                    Dim content As Byte() = Stream.ToArray()
                    HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + Or_orgno + "-" + CO_PATH + ".zip")
                    HttpContext.Current.Response.ContentType = "application/octet-stream"
                    Console.WriteLine(content)
                    HttpContext.Current.Response.BinaryWrite(content)
                    HttpContext.Current.Response.End()
                End Using

                'Dim D As New Detail()
                'D.LoadPathDetail(CO_PATH)
                ''PublicFunc.DownloadPackage(CO_PATH, FILENAME, FilePath)
                'PublicFunc.InsertDownloadLog(D.Co_seqno, D.Co_word, D.Co_sno, OutputUnitCode)

                ''壓縮檔案            

                'HttpContext.Current.Response.ClearContent()
                'HttpContext.Current.Response.Charset = "utf-8"
                'HttpContext.Current.Response.Buffer = True

                'HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8

                'HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Or_orgno + "-" + CO_SNO + ".zip")
                'HttpContext.Current.Response.ContentType = filecontenttype

                'HttpContext.Current.Response.TransmitFile(fi.FullName)
                'HttpContext.Current.Response.Flush()
                'HttpContext.Current.Response.Close()

                ''刪除zip檔
                'File.Delete(saveFileName)
            End If
        Catch ex As Exception
            HttpContext.Current.Response.Write("<script>alert('" & ex.Message & "');</script>")
        End Try

    End Sub

    ''' <summary>
    ''' 壓縮目錄內檔案
    ''' </summary>
    ''' <param name="ZipFileName">要壓縮的檔名</param>
    ''' <param name="ZipSourcePath">要壓縮的檔案</param>
    ''' <param name="ZipPassWord">壓縮檔密碼</param>
    ''' <remarks></remarks>
    Public Shared Sub SevenZip(ByVal ZipFileName, ByVal ZipSourcePath, ByVal ZipPassWord)
        Dim ZipPs As New Diagnostics.Process
        Try
            Dim strExec As String = HttpContext.Current.Server.MapPath("7z/7za.exe")
            Dim strAgr As String

            '需要設定密碼
            If ZipPassWord <> "" Then
                strAgr = "a " & ZipFileName & " " & ZipSourcePath & " -p" & ZipPassWord
            Else
                '不需要密碼
                strAgr = " a -tzip " & ZipFileName & " " & ZipFileName & " -mx=9"
            End If

            ZipPs.StartInfo.FileName = strExec
            ZipPs.StartInfo.Arguments = strAgr
            ZipPs.StartInfo.WindowStyle = Diagnostics.ProcessWindowStyle.Normal
            ZipPs.Start()
            ZipPs.WaitForExit()
            ZipPs.Close()

            If Dir(ZipFileName) <> "" Then
                HttpContext.Current.Response.Write("Zip Complete.") 'check zip file complete
            End If
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' 判斷是否有下載記錄Log
    ''' </summary>
    ''' <param name="UnitCode">下載單位</param>
    ''' <param name="CO_SEQNO">下載公文序號</param>
    ''' <returns>True / False</returns>
    ''' <remarks></remarks>
    Public Shared Function IsDownLoad(ByVal UnitCode As String, ByVal CO_SEQNO As String) As Boolean
        Dim SqlCmd As String = String.Format("select DD_SEQNO from bull_dl_det t where DD_DPID = '{0}' and DD_COSEQNO = '{1}'", UnitCode, CO_SEQNO)
        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)
        If dt.Rows.Count = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' 取出下載資料
    ''' </summary>
    ''' <param name="CO_SEQNO">下載公文序號</param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDownloadLog(ByVal CO_SEQNO As String) As DataTable
        Dim SqlCmd As String = String.Format("select * from bull_dl_det t where DD_COSEQNO = '{0}' order by dd_seqno desc", CO_SEQNO)
        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)

        Return dt
    End Function

    Public Shared Function GetAttach(ByVal CO_PATH As String) As DataTable
        Dim SqlCmd As String = String.Format("select at_seqno, at_filenm from attach where at_ftype = 2 and at_path = '{0}' order by at_ln", CO_PATH)
        Dim dt As DataTable = New Basic().ODataAdapter(SqlCmd)

        Return dt
    End Function

    Public Shared Sub DIRework(ByVal DIpath As String, ByVal Or_orgno As String, ByVal OutputUnitCode As String, ByVal OutputDI As String)
        Dim diDtd As String() = File.ReadAllLines(DIpath, Encoding.GetEncoding("Big5"))
        Dim Dtd As String = String.Empty
        Dim arrChangSW As String()
        Dim changesw As String
        Dim strSWFileName As String
        strSWFileName = DIpath.Split("\")(2).Replace("di", "sw")
        changesw = ""

        For i = 1 To diDtd.Length - 1
            If Not String.IsNullOrEmpty(Or_orgno) And Not String.IsNullOrEmpty(OutputUnitCode) Then '20110502 peter edit Or=> and 修改下載時為有附件無法下載
                diDtd(i) = diDtd(i).Replace(Or_orgno, OutputUnitCode)
            End If
            If diDtd(i).IndexOf(".sw") > -1 Then
                arrChangSW = diDtd(i).Split(" ")
                arrChangSW(3) = """" + strSWFileName + """"
                For j = 0 To arrChangSW.Length - 1
                    changesw += arrChangSW(j) + " "
                Next
                diDtd(i) = changesw
            End If

            Dtd += [String].Format("{0}{1}", diDtd(i), Environment.NewLine)
            If diDtd(i).Substring(0, 2) = "]>" Then
                Exit For
            End If
        Next

        Dim xdoc As New XmlDocument()
        Dim xrset As New XmlReaderSettings()
        xrset.DtdProcessing = DtdProcessing.Ignore
        Dim xread As XmlReader = XmlReader.Create(DIpath, xrset)

        '讀取xml
        Try
            xdoc.Load(xread)
        Catch ex As Exception
            xread.Close()
            File.Delete(DIpath)
            Throw ex
        End Try

        '' 變更收發文單位
        '' 20140507_Paul 將原先收發為單位對調機制修改回發文者不變，收文者固定為國防部公文電子公布欄
        Dim xnode As XmlNode = xdoc.SelectSingleNode("//受文者")
        'xnode.RemoveAll()
        'xnode.InnerText = "國防部公文電子公布欄"

        Dim xelement As XmlElement = xdoc.CreateElement("交換表")
        xelement.InnerText = "如交換表"
        xelement.SetAttribute("交換表單", "表單")
        xnode.AppendChild(xelement)

        Dim xnode1 As XmlNode = xdoc.SelectSingleNode("//全銜") '20110510 peter修改 為與二代公文銜接，發文機關須變更如下
        xnode1.InnerText = "國防部公文電子公布欄"
        xnode1 = xdoc.SelectSingleNode("//機關代碼")
        'xnode1.InnerText = "305000000CU022000"
        xnode1.InnerText = "A05000000CU002021"


        Dim dirs As String() = DIpath.Split("\")
        Dim TempFilename As String = String.Format("{0}\{1}\{2}{3}", dirs(0), dirs(1), Or_orgno, DateTime.Now.Ticks)
        xdoc.Save(TempFilename)
        xread.Close()

        Dim TempDI As String() = File.ReadAllLines(TempFilename, Encoding.GetEncoding("Big5"))
        Dim NewDI As New StringBuilder()
        For i As Integer = 0 To TempDI.Length - 1
            If i = 1 Then
                NewDI.Append(Dtd)
            End If
            NewDI.Append([String].Format("{0}{1}", TempDI(i), Environment.NewLine))
        Next
        File.WriteAllText(OutputDI, NewDI.ToString(), Encoding.GetEncoding("Big5"))
        File.Delete(DIpath)
        File.Delete(TempFilename)
    End Sub

    Public Shared Sub SFTPsnd(ByVal user As String, ByVal psw As String, ByVal filename As String)
        Dim winscp As Process = New Process()
        Dim count As Integer = 0
        Dim logname As String = "winscplog\" + DateTime.Now.Ticks.ToString().Substring(8) + "_log.xml"
        Try
            winscp.StartInfo.FileName = "C:\Program Files\WinSCP\winscp.com"
            winscp.StartInfo.Arguments = "/log=" + logname
            winscp.StartInfo.UseShellExecute = False
            winscp.StartInfo.RedirectStandardInput = True
            winscp.StartInfo.RedirectStandardOutput = True
            winscp.StartInfo.CreateNoWindow = True
            winscp.Start()

            winscp.StandardInput.WriteLine("open 10.18.10.100")
            winscp.StandardInput.WriteLine(user)
            winscp.StandardInput.WriteLine(psw)
            winscp.StandardInput.WriteLine("put " + filename)

            winscp.StandardInput.Close()
            For i = 0 To 10000
                count = count + 1
            Next
            ' Collect all output (not used in this example)
            Dim output As String = winscp.StandardOutput.ReadToEnd()


        Catch ex As Exception
            HttpContext.Current.Response.Write("<script>alert('" & ex.Message & "');</script>")
        End Try

        ' Wait until WinSCP finishes
        winscp.WaitForExit()
    End Sub
End Class
