Imports System.Data.OracleClient


' 2011/07/01 移除外包的DB Conn dll (DDTek.Oracle) by NaNa
Public Class Detail
    Private Conn As New System.Data.OracleClient.OracleConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
    Public Function LoadSeqnoDetail(ByVal Seqno As String)
        Conn.Open()
        Dim dr As System.Data.OracleClient.OracleDataReader
        Dim cmd As New System.Data.OracleClient.OracleCommand("select o.or_orgname, c.co_dtype, c.co_issudt, c.co_postdt, c.co_postnm, c.co_seqno, c.co_subj, c.co_word, c.co_sno, co_path from organ o,content c where o.or_orgno = c.co_orgno and c.co_seqno =  :co_seqno", Conn)
        cmd.Parameters.AddWithValue(":co_seqno", Seqno)
        dr = cmd.ExecuteReader()

        If dr.HasRows Then
            dr.Read()
            _or_orgname = dr("or_orgname")
            _co_dtype = dr("co_dtype")
            _co_issudt = dr("co_issudt")
            _co_postdt = dr("co_postdt")
            _co_postnm = dr("co_postnm")
            _co_seqno = dr("co_seqno")
            _co_subj = dr("co_subj")
            _co_word = dr("co_word")
            _co_sno = dr("co_sno")
            _co_path = dr("co_path")
            Return True
        Else
            Return False
        End If
        dr.Close()
        cmd.Dispose()
        Conn.Close()
    End Function

    Public Function LoadPathDetail(ByVal Path As String)
        Conn.Open()
        Dim dr As System.Data.OracleClient.OracleDataReader
        Dim cmd As New System.Data.OracleClient.OracleCommand("select o.or_orgname, c.co_dtype, c.co_issudt, c.co_postdt, c.co_postnm, c.co_seqno, c.co_subj, c.co_word, c.co_sno, co_path from organ o,content c where o.or_orgno = c.co_orgno and c.co_path = :co_path", Conn)
        cmd.Parameters.AddWithValue(":co_path", Path)
        dr = cmd.ExecuteReader()

        If dr.HasRows Then
            dr.Read()
            _or_orgname = dr("or_orgname")
            _co_dtype = dr("co_dtype")
            _co_issudt = dr("co_issudt")
            _co_postdt = dr("co_postdt")
            _co_postnm = dr("co_postnm")
            _co_seqno = dr("co_seqno")
            _co_subj = dr("co_subj")
            _co_word = dr("co_word")
            _co_sno = dr("co_sno")
            _co_path = dr("co_path")
            Return True
        Else
            Return False
        End If
        dr.Close()
        cmd.Dispose()
        Conn.Close()
    End Function

    Private _co_seqno As String
    Public ReadOnly Property Co_seqno As String
        Get
            Return _co_seqno
        End Get
    End Property

    Private _or_orgname As String
    Public ReadOnly Property Or_orgname As String
        Get
            Return _or_orgname
        End Get
    End Property

    Private _co_postnm As String
    Public ReadOnly Property Co_postnm As String
        Get
            Return _co_postnm
        End Get
    End Property

    Private _co_dtype As String
    Public ReadOnly Property Co_dtype As String
        Get
            Return _co_dtype
        End Get
    End Property

    Private _co_word As String
    Public ReadOnly Property Co_word As String
        Get
            Return _co_word
        End Get
    End Property

    Private _co_sno As String
    Public ReadOnly Property Co_sno As String
        Get
            Return _co_sno
        End Get
    End Property

    Private _co_subj As String
    Public ReadOnly Property Co_subj As String
        Get
            Return _co_subj
        End Get
    End Property

    Private _co_issudt As String
    Public ReadOnly Property Co_issudt As String
        Get
            Return _co_issudt
        End Get
    End Property

    Private _co_postdt As String
    Public ReadOnly Property Co_postdt As String
        Get
            Return _co_postdt
        End Get
    End Property

    Private _co_path As String
    Public ReadOnly Property Co_path As String
        Get
            Return _co_path
        End Get
    End Property
End Class
