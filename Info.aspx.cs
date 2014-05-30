using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public partial class Info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Questions.xml"));
            var question = doc.Element("Questions").Elements("Question").Select(q => new { q.Value });
            foreach (var item in question)
            {
                this.text.InnerHtml = item.Value;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Questions.xml"));
        IEnumerable<XElement> selement = doc.Element("Questions").Elements("Question");
        foreach (XElement item in selement)
        {
            item.Value = Request.Params["txt"];
            doc.Save(Server.MapPath("~/App_Data/Questions.xml"));
        }
    }
}