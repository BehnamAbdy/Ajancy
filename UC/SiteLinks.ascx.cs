using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public partial class UC_SiteLinks : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/Letters.xml"));
            var letters = doc.Element("Letters").Elements("Letter").Select(let => new
            {
                Title = let.Element("Title").Value,
                Date = let.Element("Date").Value,
                Text = string.IsNullOrEmpty(let.Element("Text").Value) ? null : (let.Element("Text").Value.Length < 320 ? let.Element("Text").Value : let.Element("Text").Value.Substring(0, 320)),
                Id = let.Attribute("id").Value,
                Scope = let.Attribute("scope").Value
            });
            bool isUserAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            StringBuilder html = new StringBuilder("<marquee onmouseover='this.scrollAmount=0' onmouseout='this.scrollAmount=2' scrolldelay='1' scrollamount='1' direction='down' loop='true' height='450px'><ul id='letters'>");
            foreach (var item in letters)
            {
                switch (item.Scope)
                {
                    case "1": // for authenticated users only
                        if (isUserAuthenticated)
                        {
                            html.AppendFormat(@"<li>
                                    <img src='./LettImg/{0}.jpg' />
                                    <div>&nbsp;
                                        <a target='_blank' href='Announcement.aspx?lid={0}'>{1}</a>
                                        <p class='lett-date'>{2}</p>
                                        <p>{3} ...</p>
                                    </div></li>", System.IO.File.Exists(string.Format("{0}/{1}.jpg", Server.MapPath("~/LettImg"), item.Id)) ? item.Id : "0", item.Title, item.Date, item.Text);
                        }
                        break;

                    case "2": // item is inactive
                        continue;
                        break;

                    default:
                        html.AppendFormat(@"<li>
                                    <img src='./LettImg/{0}.jpg' />
                                    <div>&nbsp;
                                        <a target='_blank' href='Announcement.aspx?lid={0}'>{1}</a>
                                        <p class='lett-date'>{2}</p>
                                        <p>{3} ...</p>
                                    </div></li>", System.IO.File.Exists(string.Format("{0}/{1}.jpg", Server.MapPath("~/LettImg"), item.Id)) ? item.Id : "0", item.Title, item.Date, item.Text);
                        break;
                }
            }

            html.Append("</ul></marquee><div class='clear'></div>");
            this.dvList.InnerHtml = null;
            this.dvList.InnerHtml = html.ToString();
        }
    }
}