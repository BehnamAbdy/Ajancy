using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public partial class Admin_Questions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["mode"] != null)
            {
                XDocument doc = null;
                string result = null;
                switch (Request.Params["mode"])
                {
                    case "r": // read mode
                        doc = XDocument.Load(Server.MapPath("~/App_Data/Questions.xml"));
                        var question = doc.Element("Questions").Elements("Question").Select(q => new { q.Value });
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        result = serializer.Serialize(question);
                        break;

                    case "w": // write mode
                        doc = XDocument.Load(Server.MapPath("~/App_Data/Questions.xml"));
                        IEnumerable<XElement> selement = doc.Element("Questions").Elements("Question");
                        foreach (XElement item in selement)
                        {
                            item.Value = Request.Params["txt"];
                            doc.Save(Server.MapPath("~/App_Data/Questions.xml"));
                        }
                        result = "1";
                        break;
                }
                Response.Clear();
                Response.Write(result);
                Response.End();
            }
        }
    }
}