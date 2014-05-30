using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web.Script.Serialization;

public partial class Site_Links : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            XDocument doc = null;
            string result = null;
            if (Request.QueryString["mode"] != null)
            {
                JavaScriptSerializer serializer = null;
                doc = XDocument.Load(Server.MapPath("~/App_Data/Links.xml"));
                switch (Request.QueryString["mode"])
                {
                    case "1": // Load list
                        serializer = new JavaScriptSerializer();
                        var list = doc.Element("Links").Elements("Link").Select(lk => new { Id = lk.Attribute("id").Value, Title = lk.Element("Title").Value, Target = lk.Element("Target").Value });
                        result = serializer.Serialize(list);
                        break;

                    case "2": // Load a link
                        serializer = new JavaScriptSerializer();
                        var test = doc.Element("Links").Elements("Link").Where(lk => lk.Attribute("id").Value == Request.QueryString["id"]).Select(lk => new { Title = lk.Element("Title").Value, Target = lk.Element("Target").Value });
                        result = serializer.Serialize(test);
                        break;
                }

                Response.Clear();
                Response.Write(result);
                Response.End();
            }
            else if (Request.QueryString["id"] != null)
            {
                doc = XDocument.Load(Server.MapPath("~/App_Data/Links.xml"));
                if (Request.QueryString["id"] == "0") // Add mode
                {
                    string maxId = doc.Element("Links").Elements("Link").Max(tst => tst.Attribute("id").Value);

                    doc.Element("Links").Add(new XElement("Link", new XAttribute("id", maxId == null ? "1" : (short.Parse(maxId) + 1).ToString()),
                                                       new XElement("Title", Request.QueryString["tle"]),
                                                       new XElement("Target", Request.QueryString["trg"])));
                    doc.Save(Server.MapPath("~/App_Data/Links.xml"));
                    result = "1";
                }
                else // Edit mode
                {
                    IEnumerable<XElement> element = doc.Element("Links").Elements("Link").Where(an => an.Attribute("id").Value == Request.QueryString["id"]);
                    foreach (XElement item in element)
                    {
                        item.Element("Title").Value = Request.QueryString["tle"];
                        item.Element("Target").Value = Request.QueryString["trg"];
                        doc.Save(Server.MapPath("~/App_Data/Links.xml"));
                        result = "1";
                    }
                }

                Response.Clear();
                Response.Write(result);
                Response.End();
            }
        }
    }
}