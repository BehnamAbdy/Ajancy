<%@ WebHandler Language="C#" Class="Document" %>

using System;
using System.Linq;
using System.Web;

public class Document : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.AddHeader("Pragma", "no-cache");
        context.Response.AddHeader("Cache-Control", "private, no-cache");
        int id = 0;
        Ajancy.Doument doc = null;
        Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

        if (int.TryParse(context.Request.QueryString["id"], out id))
        {
            doc = db.Douments.First<Ajancy.Doument>(d => d.DoumentID == id);
        }
        else if (int.TryParse(context.Request.QueryString["pId"], out id))
        {
            doc = db.Douments.FirstOrDefault<Ajancy.Doument>(d => d.PersonID == id && d.DocumentType == 1);
        }
        
        if (doc != null)
        {
            System.IO.Stream strm = new System.IO.MemoryStream(doc.Picture.ToArray());
            byte[] buffer = new byte[strm.Length];
            int byteSeq = strm.Read(buffer, 0, (int)strm.Length);
            while (byteSeq > 0)
            {
                context.Response.OutputStream.Write(buffer, 0, byteSeq);
                byteSeq = strm.Read(buffer, 0, (int)strm.Length);
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}