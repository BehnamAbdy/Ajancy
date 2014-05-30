using System;
using System.Web;

public partial class SiteWide : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Header.DataBind();
        }
    }

    protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Abandon();
        Response.Redirect("~/Default.aspx");
    }
}
