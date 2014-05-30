using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Management_UsersList : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ajancy.User user = Public.ActiveUserRole.User;
            switch (Public.ActiveUserRole.RoleID)
            {
                case (short)Public.Role.ProvinceManager:
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                    this.drpRoles.Items.Add(new ListItem("مالک خودرو", "8"));
                    this.drpRoles.Items.Add(new ListItem("راننده تاکسی تلفنی", "6"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس های شهرستان", "9"));
                    this.drpCity.Enabled = true;
                    this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                    break;

                case (short)Public.Role.CityManager:
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                    this.drpRoles.Items.Add(new ListItem("مالک خودرو", "8"));
                    this.drpRoles.Items.Add(new ListItem("راننده تاکسی تلفنی", "6"));
                    this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                    break;

                case (short)Public.Role.Admin:
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس های شهرستان", "9"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس های استان", "10"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آموزشگاه های شهرستان", "13"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آموزشگاه های استان", "14"));
                    this.drpRoles.Items.Add(new ListItem("مباشر آژانس", "3"));
                    this.drpRoles.Items.Add(new ListItem("شریک آژانس", "4"));
                    this.drpRoles.Items.Add(new ListItem("کارپرداز آژانس", "5"));
                    this.drpRoles.Items.Add(new ListItem("مالک خودرو", "8"));
                    this.drpRoles.Items.Add(new ListItem("راننده تاکسی تلفنی", "6"));
                    this.drpRoles.Items.Add(new ListItem("راننده پیک موتوری", "7"));
                    this.drpRoles.Items.Add(new ListItem("بازدید کننده", "16"));
                    this.drpRoles.Items.Add(new ListItem("مدیر سیستم", "1"));
                    this.drpProvince.Enabled = true;
                    this.drpCity.Enabled = true;
                    break;
            }

            this.drpRoles.Items.Insert(0, new ListItem("- انتخاب کنید -", "0"));
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void drpProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpProvince.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCity.DataSource = db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue)).Select(c => new { c.CityID, c.Name });
            this.drpCity.DataBind();
        }
        else
        {
            this.drpCity.Items.Clear();
            this.drpCity.Items.Insert(0, "- انتخاب کنید -");
        }
    }

    protected void lnkPassword_Click(object sender, EventArgs e)
    {
        this.btnBack_Click(sender, e);
        string[] values = ((LinkButton)sender).CommandArgument.Split('|');
        this.ViewState["UserID"] = values[1];
        this.tdPasswordPanelTitle.InnerHtml = values[0];
        this.pnlPassword.Visible = true;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ObjectDataSource1.Select();
        this.lstUsers.DataBind();
        this.btnBack_Click(sender, e);
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.Users.First<Ajancy.User>(u => u.UserID == Public.ToInt(this.ViewState["UserID"])).PassWord = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtPassword.Text, "SHA1");
            db.SubmitChanges();
            this.btnBack_Click(sender, e);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.ViewState["PersonID"] = null;
        this.ViewState["UserID"] = null;
        this.txtPassword.Text = null;
        this.txtRePassword.Text = null;
        this.pnlPassword.Visible = false;
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["roleId"] = Public.ToShort(this.drpRoles.SelectedValue);
        e.InputParameters["nationalCode"] = this.txtNationalCode.Text;
        e.InputParameters["firstName"] = this.txtFirstName.Text;
        e.InputParameters["lastName"] = this.txtLastName.Text;
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}