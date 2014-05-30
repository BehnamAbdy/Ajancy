using System;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnLogin_Click(object sender, System.EventArgs e)
    {
        if (Page.IsValid)
        {
            DataLoadOptions dlo = new DataLoadOptions();

            if (this.trMembership.Visible && this.drpMemberships.SelectedItem != null) // login with selected membership
            {
                string[] roleParts = this.drpMemberships.SelectedValue.Split('|');
                dlo.LoadWith<Ajancy.UsersInRole>(u => u.User);
                dlo.LoadWith<Ajancy.User>(u => u.Person);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;
                Ajancy.UsersInRole userInRole = db.UsersInRoles.First<Ajancy.UsersInRole>(ur => ur.UserRoleID == int.Parse(roleParts[0]));
                userInRole.LastLoginDate = DateTime.Now;
                db.SubmitChanges();
                this.LogIn(userInRole, roleParts);
            }

            dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
            dlo.LoadWith<Ajancy.User>(u => u.Person);
            dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.AjancyPartners);
            dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.Ajancy);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;
            Ajancy.User user = db.Users.FirstOrDefault<Ajancy.User>(u => u.UserName == this.txtUserName.Text && u.PassWord == FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtPassword.Text, "SHA1") && u.LockOutDate == null);

            if (user != null) // Credentials are valid
            {
                foreach (Ajancy.UsersInRole ur in user.UsersInRoles)
                {
                    if (ur.LockOutDate == null)
                    {
                        if (ur.AjancyPartners.Count == 0)
                        {
                            this.drpMemberships.Items.Add(new ListItem(Public.GetRoleName(ur.RoleID), string.Concat(ur.UserRoleID, "|0")));
                        }
                        else
                        {
                            foreach (Ajancy.AjancyPartner partner in ur.AjancyPartners)
                            {
                                if (partner.LockOutDate == null)
                                {
                                    if (partner.Ajancy == null)
                                    {
                                        this.drpMemberships.Items.Add(new ListItem(string.Concat(Public.GetRoleName(ur.RoleID), " --- ", partner.Ajancy == null ? null : partner.Ajancy.AjancyName), string.Concat(ur.UserRoleID, "|0|0|---")));
                                    }
                                    else
                                    {
                                        this.drpMemberships.Items.Add(new ListItem(string.Concat(Public.GetRoleName(ur.RoleID), " --- ", partner.Ajancy == null ? null : partner.Ajancy.AjancyName), string.Concat(ur.UserRoleID, "|", partner.AjancyID, "|", partner.Ajancy.AjancyType, "|", partner.Ajancy.AjancyName)));
                                    }
                                }
                            }
                        }
                    }
                }

                if (this.drpMemberships.Items.Count == 1) // login immediately
                {
                    user.UsersInRoles[0].LastLoginDate = DateTime.Now;
                    db.SubmitChanges();
                    string[] roleParts = this.drpMemberships.Items[0].Value.Split('|');
                    this.LogIn(user.UsersInRoles.First<Ajancy.UsersInRole>(ur => ur.UserRoleID.ToString() == roleParts[0]), roleParts);
                }
                else if (this.drpMemberships.Items.Count > 1) // several active roles are found
                {
                    this.DisposeContext();
                    this.trMembership.Visible = true;
                }
                else // no active role found
                {
                    this.lblMessage.Text = "برای این کاربر پست فعالی یافت نشد";
                }
            }
            else
            {
                this.lblMessage.Text = "نام کاربری یا گذرواژه نادرست میباشد";
            }
        }
    }

    private void LogIn(Ajancy.UsersInRole userInRole, string[] roleParts)
    {
        Ajancy.User user = new Ajancy.User
        {
            UserID = userInRole.UserID,
            CityID = userInRole.User.Person.User.CityID.GetValueOrDefault(),
            ProvinceID = userInRole.User.Person.User.ProvinceID,
            Person = new Ajancy.Person { PersonID = userInRole.User.PersonID, FirstName = userInRole.User.Person.FirstName, LastName = userInRole.User.Person.LastName },
            City = new Ajancy.City { CityID = userInRole.User.Person.User.CityID.GetValueOrDefault(), ProvinceID = userInRole.User.Person.User.ProvinceID, Name = userInRole.User.Person.User.City.Name },
            Province = new Ajancy.Province { ProvinceID = userInRole.User.Person.User.ProvinceID, Name = userInRole.User.Person.User.Province.Name }
        };

        this.DisposeContext();
        this.Session["UserRole"] = new Ajancy.UsersInRole { UserRoleID = userInRole.UserRoleID, RoleID = userInRole.RoleID, User = user };
        if (userInRole.RoleID == (short)Public.Role.AjancyManager)
        {
            this.Session["Ajancy"] = new Ajancy.Ajancy { AjancyID = int.Parse(roleParts[1]), AjancyType = byte.Parse(roleParts[2]), AjancyName = roleParts[3] };
        }
        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, this.txtUserName.Text, DateTime.Now, DateTime.Now.AddMinutes(30), true, userInRole.RoleID.ToString());
        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket)));
        Response.Redirect(FormsAuthentication.GetRedirectUrl(this.txtUserName.Text, true));
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
