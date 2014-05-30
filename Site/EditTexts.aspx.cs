using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

public partial class Site_EditTexts : System.Web.UI.Page
{
    XDocument doc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            doc = XDocument.Load(Server.MapPath("~/App_Data/Announcements.xml"));
            this.lstAnnouncements.DataSource = doc.Element("Announcements").Elements("Announcement").Select(an => new { Id = an.Attribute("id").Value, Title = an.Element("Title").Value });
            this.lstAnnouncements.DataBind();
        }
    }

    protected void lstAnnouncements_SelectedIndexChanged(object sender, EventArgs e)
    {
        doc = XDocument.Load(Server.MapPath("~/App_Data/Announcements.xml"));
        var text = doc.Element("Announcements").Elements("Announcement").Where(an => an.Attribute("id").Value == this.lstAnnouncements.SelectedValue).Select(an => new { Text = an.Element("Text").Value });
        foreach (var item in text)
        {
            this.textEditor.Text = item.Text;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.lstAnnouncements.SelectedIndex > -1)
        {
            doc = XDocument.Load(Server.MapPath("~/App_Data/Announcements.xml"));
            IEnumerable<XElement> elements = doc.Element("Announcements").Elements("Announcement").Where(an => an.Attribute("id").Value == this.lstAnnouncements.SelectedValue).Elements("Text");
            foreach (XElement item in elements)
            {
                item.Value = this.textEditor.Text;
            }
            doc.Save(Server.MapPath("~/App_Data/Announcements.xml"));
            this.lblMessage.Text = Public.SAVEMESSAGE;
        }
        else
        {
            this.lblMessage.Text = "گزینه متن مورد نظرتان راانتخاب کنید";
        }
    }
}
