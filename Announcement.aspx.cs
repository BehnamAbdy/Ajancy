using System;
using System.Linq;
using System.Xml.Linq;

public partial class Announcement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Announcements.xml"));
                var text = doc.Element("Announcements").Elements("Announcement").Where(an => an.Attribute("id").Value == Request.QueryString["id"]).Select(an => new { Title = an.Element("Title").Value, Text = an.Element("Text").Value });
                foreach (var item in text)
                {
                    this.dvtitle.InnerHtml = item.Title;
                    this.dvText.InnerHtml = item.Text;
                }
            }
            else if (Request.QueryString["lid"] != null)
            {
                XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Letters.xml"));
                var text = doc.Element("Letters").Elements("Letter").Where(an => an.Attribute("id").Value == Request.QueryString["lid"]).Select(an => new { Title = an.Element("Title").Value, Text = an.Element("Text").Value, Attachment = an.Element("Attachment").Value });
                foreach (var item in text)
                {
                    this.dvtitle.InnerHtml = item.Title;
                    this.dvText.InnerHtml = item.Text;
                    if (!string.IsNullOrEmpty(item.Attachment))
                    {
                        this.dvAttachment.InnerHtml = string.Format("<a style='float: left;margin: 10px 0 0 15px;' href='./Attachments/{0}' title='دانلود فایل پیوستی' alt='دانلود فایل پیوستی'><img src='App_Themes/Default/images/attachment.png' /></a>", item.Attachment);
                    }
                }
            }
            else if (Request.QueryString["iid"] != null)
            {
                XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Images.xml"));
                var text = doc.Element("Images").Elements("Image").Where(an => an.Attribute("id").Value == Request.QueryString["iid"]).Select(an => new { Title = an.Element("Title").Value });
                foreach (var item in text)
                {
                    this.dvtitle.InnerHtml = item.Title;
                    this.dvText.InnerHtml = string.Format("<img src='LettImg/{0}.jpg' style='margin: 5px auto;' />", Request.QueryString["iid"]); ;
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}
