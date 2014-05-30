using System;
using System.Linq;
using System.Web;
using System.Web.Security;

public partial class Members_EditPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtUserName.Text = HttpContext.Current.User.Identity.Name;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            Ajancy.User user = db.Users.FirstOrDefault<Ajancy.User>(u => u.UserName == HttpContext.Current.User.Identity.Name && u.PassWord == FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtOldPassword.Text, "SHA1"));
            if (user != null)
            {
                user.PassWord = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtNewPassword.Text, "SHA1");
                db.SubmitChanges();
                db.Dispose();
                this.lblMessage.Text = "ویرایش گذرواژه انجام گردید";
            }
            else
            {
                this.lblMessage.Text = "گذرواژه نادرست میباشد";
            }
        }
        this.txtOldPassword.Text = null;
        this.txtNewPassword.Text = null;
        this.txtRePassword.Text = null;
    }
}