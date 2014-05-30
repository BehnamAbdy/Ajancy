using System;
using System.Linq;
using System.Data.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Management_User : System.Web.UI.Page
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
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس های شهرستان", "9"));
                    this.drpCity.Enabled = true;
                    this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                    break;

                case (short)Public.Role.CityManager:
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                    this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                    break;

                case (short)Public.Role.Admin:
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                    this.drpRoles.Items.Add(new ListItem("مدیر سیستم", "1"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس های شهرستان", "9"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آژانس های استان", "10"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آموزشگاه های شهرستان", "13"));
                    this.drpRoles.Items.Add(new ListItem("مدیر آموزشگاه های استان", "14"));
                    this.drpRoles.Items.Add(new ListItem("بازدید کننده", "16"));
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

    protected void txtNationalCode_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var person = from p in db.Persons
                         join u in db.Users on p.PersonID equals u.PersonID
                         where p.NationalCode == this.txtNationalCode.Text
                         select new { p.PersonID, u.ProvinceID, u.CityID };

            foreach (var item in person)
            {
                switch (Public.ActiveUserRole.RoleID)
                {
                    case (short)Public.Role.ProvinceManager:
                        if (item.ProvinceID != Public.ActiveUserRole.User.ProvinceID)
                        {
                            DisposeContext();
                            Response.Redirect("~/Message.aspx?mode=24");
                        }
                        break;

                    case (short)Public.Role.CityManager:
                        if (item.CityID != Public.ActiveUserRole.User.CityID)
                        {
                            DisposeContext();
                            Response.Redirect("~/Message.aspx?mode=23");
                        }
                        break;
                }
                DisposeContext();
                Response.Redirect(string.Format("~/Management/UserRoles.aspx?id={0}", TamperProofString.QueryStringEncode(item.PersonID.ToString())));
            }
        }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Ajancy.Person person = new Ajancy.Person();
            person.NationalCode = this.txtNationalCode.Text;
            person.FirstName = this.txtFirstName.Text;
            person.LastName = this.txtLastName.Text;
            person.Mobile = this.txtMobile.Text.Trim();
            person.SubmitDate = DateTime.Now;
            Ajancy.User user = new Ajancy.User();
            user.UserName = this.txtNationalCode.Text;
            user.PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtPassword.Text, "SHA1");
            user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
            user.CityID = Public.ToShort(this.drpCity.SelectedValue);
            user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = Public.ToShort(this.drpRoles.SelectedValue), MembershipDate = DateTime.Now });
            person.User = user;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

            switch ((Public.Role)byte.Parse(this.drpRoles.SelectedValue))
            {
                case Public.Role.AjancyManager:
                case Public.Role.AjancySupervisor:
                case Public.Role.AjancySecretary:
                case Public.Role.AjancyPartner:
                    user.UsersInRoles[0].AjancyPartners.Add(new Ajancy.AjancyPartner { SubmitDate = DateTime.Now });
                    break;

                case Public.Role.ProvinceManager:
                    var prvManager = from u in db.Users
                                     join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                     where ur.RoleID == (short)Public.Role.ProvinceManager &&
                                                u.ProvinceID == user.ProvinceID
                                     select new { u.UserID };
                    foreach (var item in prvManager)
                    {
                        this.lblMessage.Text = string.Format("برای استان {0} کاربری با سمت مدیر استان موجود میباشد", this.drpProvince.SelectedItem.Text);
                        return;
                    }
                    break;

                case Public.Role.CityManager:
                    var ctyManager = from u in db.Users
                                     join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                     where ur.RoleID == (short)Public.Role.CityManager &&
                                                u.CityID == user.CityID
                                     select new { u.UserID };
                    foreach (var item in ctyManager)
                    {
                        this.lblMessage.Text = string.Format("برای شهرستان {0} کاربری با سمت مدیر شهرستان موجود میباشد", this.drpCity.SelectedItem.Text);
                        return;
                    }
                    break;

                case Public.Role.AcademyProvince:
                    var acdPrv = from u in db.Users
                                 join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                 where ur.RoleID == (short)Public.Role.AcademyProvince &&
                                                u.ProvinceID == user.ProvinceID
                                 select new { u.UserID };
                    foreach (var item in acdPrv)
                    {
                        this.lblMessage.Text = string.Format("برای استان {0} کاربری با سمت مدیر استان موجود میباشد", this.drpProvince.SelectedItem.Text);
                        return;
                    }
                    break;

                case Public.Role.AcademyCity:
                    var acdCty = from u in db.Users
                                 join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                 where ur.RoleID == (short)Public.Role.AcademyCity &&
                                                u.CityID == user.CityID
                                 select new { u.UserID };
                    foreach (var item in acdCty)
                    {
                        this.lblMessage.Text = string.Format("برای شهرستان {0} کاربری با سمت مدیر شهرستان موجود میباشد", this.drpCity.SelectedItem.Text);
                        return;
                    }
                    break;
            }

            try
            {
                db.Persons.InsertOnSubmit(person);
                db.SubmitChanges();
                this.lblMessage.Text = "ثبت کاربر انجام شد";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_Persons"))
                {
                    this.lblMessage.Text = "کد ملی تکراری میباشد!";
                }
                else if (ex.Message.Contains("IX_Persons_2"))
                {
                    this.lblMessage.Text = "نام کاربری تکراری میباشد!";
                }
            }

            this.txtNationalCode.Text = null;
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            this.txtMobile.Text = null;
            this.txtPassword.Text = null;
            this.txtRePassword.Text = null;
            this.drpRoles.SelectedIndex = 0;
            this.drpProvince.SelectedIndex = 0;
            this.drpCity.SelectedIndex = 0;
        }
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
