<%@ WebHandler Language="C#" Class="DBFileStream" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

/// <summary>
/// 開發人員: 千機科技 黃威傑 (Mark)
/// 2011/07/01 移除外包的DB Conn dll by NaNa
/// </summary>

public class DBFileStream : IHttpHandler, IReadOnlySessionState
{
    byte[] DataBuffer;
    string filename;
    string fileExtension;
    public string strConn = (string)ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    public void ProcessRequest (HttpContext context) {
        String SEQNO = context.Request.QueryString["SEQNO"];
        String DOWN = context.Request.QueryString["DOWN"];
        
        if (!string.IsNullOrEmpty(SEQNO))
        {
            if (String.IsNullOrEmpty(DOWN))
                GetDBpdf(SEQNO);
            else
                GetDBAttach(SEQNO);
        }
        else
        {
            return;
        }

        string contentType = string.Empty;

        switch (fileExtension)
        {
            case ".pdf":
                contentType = "application/pdf";
                break;
            case ".doc":
                contentType = "application/msword";
                break;
            case ".xls":
                contentType = "application/vnd.ms-excel";
                break;
            case ".ppt":
                contentType = "application/vnd.ms-powerpoint";
                break;
            case ".txt":
                contentType = "text/plain";
                break;
            case ".tif":
                contentType = "image/tiff";
                break;
            case ".tiff":
                contentType = "image/tiff";
                break;
            case ".jpg":
                contentType = "image/jpeg";
                break;
            case ".jpeg":
                contentType = "image/jpeg";
                break;
            default :
                contentType = "application/octet-stream";
                break;
        }

        //有要直接存檔時取消註記
        if (!string.IsNullOrEmpty(DOWN) && !string.IsNullOrEmpty(filename))
            context.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", filename));
        
        context.ClearError();
        context.Response.Expires = 0;
        context.Response.Buffer = true;
        context.Response.ContentType = contentType;
        context.Response.Clear();

        MemoryStream memStream = new MemoryStream((byte[])DataBuffer);
        memStream.WriteTo(context.Response.OutputStream);
        memStream.Close();
    }

    private void GetDBpdf(string AT_SEQNO)
    {
        
        try
        {
            using (OracleConnection Conn = new OracleConnection(strConn))
            {
                String SqlCmd = "select at_pdfdata from attach where at_seqno = :at_seqno";
                OracleCommand cmd = new OracleCommand(SqlCmd, Conn);
                OracleDataReader dr;
                cmd.Parameters.Add(":at_seqno", AT_SEQNO);
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    DataBuffer = (byte[])dr["at_pdfdata"];
                    fileExtension = ".pdf";
                }
                dr.Close();
                cmd.Connection.Close();
            }
        }
        catch
        { }
    }

    private void GetDBAttach(string AT_SEQNO)
    {
        try
        {
            using (OracleConnection Conn = new OracleConnection(strConn))
            {
                String SqlCmd = "select at_filenm, at_data from attach where at_seqno = :at_seqno";
                OracleCommand cmd = new OracleCommand(SqlCmd, Conn);
                OracleDataReader dr;
                cmd.Parameters.Add(":at_seqno", AT_SEQNO);
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    DataBuffer = (byte[])dr["at_data"];
                    filename = dr["at_filenm"].ToString();
                    fileExtension = new System.IO.FileInfo(dr["at_filenm"].ToString()).Extension;
                }
                dr.Close();
                cmd.Connection.Close();
            }
        }
        catch
        { }
    }
   
    public bool IsReusable {
        get {
            return false;
        }
    }
}