using System;
using System.Web.UI;
using System.Web;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Header.DataBind();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                this.pnlLogIn.Visible = false;
                this.pnlLogedIn.Visible = true;
                this.lblUser.InnerHtml = string.Format("{0} {1}", Public.ActiveUserRole.User.Person.FirstName, Public.ActiveUserRole.User.Person.LastName);

                switch ((Public.Role)Public.ActiveUserRole.RoleID)
                {
                    case Public.Role.AjancyManager:
                        this.lblRole.InnerHtml = string.Format("{0} {1}", Public.GetRoleName(Public.ActiveUserRole.RoleID), Public.ActiveAjancy.AjancyName);
                        break;

                    case Public.Role.ProvinceManager:
                    case Public.Role.AcademyProvince:
                        this.lblRole.InnerHtml = string.Format("{0} {1}", Public.GetRoleName(Public.ActiveUserRole.RoleID), Public.ActiveUserRole.User.Province.Name);
                        break;

                    case Public.Role.CityManager:
                    case Public.Role.AcademyCity:
                        this.lblRole.InnerHtml = string.Format("{0} {1}", Public.GetRoleName(Public.ActiveUserRole.RoleID), Public.ActiveUserRole.User.City.Name);
                        break;

                    default:
                        this.lblRole.InnerHtml = Public.GetRoleName(Public.ActiveUserRole.RoleID);
                        break;
                }
            }
        }
    }

    protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Abandon();
        Response.Redirect("~/Default.aspx");
    }

    protected void lnkLogOut_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Abandon();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx");
    }
}
