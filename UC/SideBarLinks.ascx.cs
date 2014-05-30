using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public partial class UC_SideBarLinks : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Announcements.xml"));
            var announces = doc.Element("Announcements").Elements("Announcement").Select(an => new { Id = an.Attribute("id").Value, Title = an.Element("Title").Value });
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<h2 style='margin: 20px 0px 2px 0px;background-color: #d7d7eb;text-align:center;color:#666666;'>آیین نامه ها</h2><ul id='links'>");
            foreach (var item in announces)
            {
                html.AppendFormat("<li><a href='{0}?id={1}'>{2}</a></li>", ResolveUrl("~/Announcement.aspx"), item.Id, item.Title);
            }
            html.AppendFormat("<li><a href='{0}'>درخواست پروانه کسب</a></li>", ResolveUrl("~/Requests/BusinessLicense.aspx"));
            html.AppendFormat("<li><a href='{0}' style='font-weight: bold;'>شکایات مردمی</a></li></ul>", ResolveUrl("~/Comment.aspx"));
            this.dvList.InnerHtml = null;
            this.dvList.InnerHtml = html.ToString();
        }
    }
}