using System;
using System.Linq;
using System.Web;
using System.Data.Linq;

public partial class Union_BusinessLicense : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);

            int ajancyId = 0;
            if (Request.QueryString["j"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["j"]), out ajancyId)) // Edit mode
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                var ajancy = from j in db.Ajancies
                             join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                             join ur in db.UsersInRoles on jp.UserRoleID equals ur.UserRoleID
                             join u in db.Users on ur.UserID equals u.UserID
                             join p in db.Persons on u.PersonID equals p.PersonID
                             join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                             where j.AjancyID == ajancyId && ur.RoleID == (short)Public.Role.AjancyManager
                             select
                         new
                         {
                             j.AjancyID,
                             j.AjancyType,
                             j.AjancyName,
                             j.BusinessLicenseType,
                             j.Phone,
                             j.PostalCode,
                             j.Address,
                             j.CityID,
                             bl.BusinessLicenseNo,
                             p.PersonID,
                             p.NationalCode,
                             p.FirstName,
                             p.LastName,
                             p.Gender,
                             p.Mobile
                         };

                foreach (var item in ajancy)
                {
                    this.ViewState["AjancyID"] = item.AjancyID;
                    this.ViewState["PersonID"] = item.PersonID;
                    this.txtNationalCode.ReadOnly = true;
                    this.txtNationalCode.Text = item.NationalCode;
                    this.txtFirstName.Text = item.FirstName;
                    this.txtLastName.Text = item.LastName;
                    this.txtMobile.Text = item.Mobile;
                    this.drpGender.SelectedValue = item.Gender.GetValueOrDefault().ToString();
                    this.drpCity.SelectedValue = item.CityID.ToString();
                    this.drpAjancyType.SelectedValue = item.AjancyType.ToString();
                    this.txtAjancyName.Text = item.AjancyName;
                    this.drpBusinessLicenseType.SelectedValue = item.BusinessLicenseType.ToString();
                    this.txtPlaceAddress.Text = item.Address;
                    this.txtBusinessPlacePhone.Text = item.Phone;
                    this.txtBusinessLicenseNo.Text = item.BusinessLicenseNo;
                    this.txtPlacePostalCode.Text = item.PostalCode;
                }
            }
            else
            {
                this.drpCity.SelectedValue = user.CityID.ToString();
            }

            this.drpProvince.Enabled = Public.ActiveUserRole.RoleID == (short)Public.Role.Admin;
            if (Public.ActiveUserRole.RoleID == (short)Public.Role.Admin || Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
            {
                this.drpCity.Enabled = true;
            }
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
            SetPerson(db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtNationalCode.Text));
        }
        this.txtFirstName.Focus();
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
        if (this.Page.IsValid)
        {
            int personId = 0;
            int ajancyId = 0;
            Ajancy.Person person = new Ajancy.Person();
            Ajancy.UsersInRole usersInRole = null;
            Ajancy.Ajancy ajancy = new Ajancy.Ajancy();
            Ajancy.BusinessLicense businessLicense = new Ajancy.BusinessLicense();
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

            if (this.ViewState["PersonID"] == null) // Add mode
            {
                Ajancy.User user = new Ajancy.User();
                user.UserName = this.txtNationalCode.Text;
                user.PassWord = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtNationalCode.Text.Trim(), "SHA1");
                user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                user.SubmitDate = DateTime.Now;
                usersInRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyManager, MembershipDate = DateTime.Now };
                usersInRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                user.UsersInRoles.Add(usersInRole);
                person.SubmitDate = DateTime.Now;
                person.User = user;
                db.Persons.InsertOnSubmit(person);
            }
            else if (int.TryParse(this.ViewState["PersonID"].ToString(), out personId)) // Edit person mode
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.User);
                dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
                db.LoadOptions = dlo;

                person = db.Persons.First<Ajancy.Person>(p => p.PersonID == personId);
                usersInRole = person.User.UsersInRoles.SingleOrDefault<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyManager);
                if (usersInRole == null)
                {
                    usersInRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyManager, MembershipDate = DateTime.Now };
                    person.User.UsersInRoles.Add(usersInRole);
                }

                if (this.ViewState["AjancyID"] == null)
                {
                    usersInRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                }
            }

            if (this.ViewState["AjancyID"] == null)
            {
                if (db.Ajancies.Any<Ajancy.Ajancy>(j => j.CityID == short.Parse(this.drpCity.SelectedValue) &&
                                                                                j.AjancyType != (byte)Public.AjancyType.Academy &&
                                                                                j.AjancyName.Equals(this.txtAjancyName.Text.Trim())))
                {
                    this.lblMessage.Text = string.Format("آژانسی با نام <b>{0}</b> در شهرستان <b>{1}</b> موجود میباشد", ajancy.AjancyName, this.drpCity.SelectedItem.Text);
                    return;
                }

                var buslic = from j in db.Ajancies
                             join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                             where bl.BusinessLicenseNo == this.txtBusinessLicenseNo.Text.Trim() &&
                                        j.CityID == Public.ToShort(this.drpCity.SelectedValue)
                             select new { j.AjancyID };
                foreach (var item in buslic)
                {
                    this.lblMessage.Text = string.Format("آژانسی با شماره پروانه کسب <b>{0}</b> در شهرستان <b>{1}</b> موجود میباشد", this.txtBusinessLicenseNo.Text, this.drpCity.SelectedItem.Text);
                    return;
                }

                businessLicense.SubmitDate = DateTime.Now;
                ajancy.BusinessLicenses.Add(businessLicense);
            }
            else if (int.TryParse(this.ViewState["AjancyID"].ToString(), out ajancyId)) // Edit ajancy mode
            {
                if (db.Ajancies.Any<Ajancy.Ajancy>(j => j.CityID == int.Parse(this.drpCity.SelectedValue) && j.AjancyID != ajancyId && j.AjancyName.Equals(this.txtAjancyName.Text.Trim())))
                {
                    this.lblMessage.Text = string.Format("آژانسی با نام <b>{0}</b> در شهرستان <b>{1}</b> موجود میباشد", this.txtAjancyName.Text, this.drpCity.SelectedItem.Text);
                    return;
                }

                var buslic = from j in db.Ajancies
                             join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                             where bl.BusinessLicenseNo == this.txtBusinessLicenseNo.Text.Trim() &&
                                        j.CityID == Public.ToShort(this.drpCity.SelectedValue) &&
                                        j.AjancyID != ajancyId
                             select new { j.AjancyID };
                foreach (var item in buslic)
                {
                    this.lblMessage.Text = string.Format("آژانسی با شماره پروانه کسب <b>{0}</b> در شهرستان <b>{1}</b> موجود میباشد", this.txtBusinessLicenseNo.Text, this.drpCity.SelectedItem.Text);
                    return;
                }

                ajancy = db.Ajancies.FirstOrDefault<Ajancy.Ajancy>(j => j.AjancyID == ajancyId);
                businessLicense = ajancy.BusinessLicenses.First<Ajancy.BusinessLicense>();
            }

            person.NationalCode = this.txtNationalCode.Text.Trim();
            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();
            person.Mobile = this.txtMobile.Text.Trim();
            person.Gender = Public.ToByte(this.drpGender.SelectedValue);

            ajancy.SubmitDate = DateTime.Now;
            ajancy.AjancyType = Public.ToByte(this.drpAjancyType.SelectedValue);
            ajancy.AjancyName = this.txtAjancyName.Text.Trim();
            ajancy.CityID = Public.ToShort(this.drpCity.SelectedValue);
            ajancy.BusinessLicenseType = Public.ToByte(this.drpBusinessLicenseType.SelectedValue);
            ajancy.PostalCode = this.txtPlacePostalCode.Text.Trim();
            ajancy.Address = this.txtPlaceAddress.Text.Trim();
            ajancy.Phone = this.txtBusinessPlacePhone.Text.Trim();
            businessLicense.BusinessLicenseNo = this.txtBusinessLicenseNo.Text.Trim();

            db.SubmitChanges();
            DisposeContext();
            Response.Redirect("~/Message.aspx?mode=4");
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person != null)
        {
            this.ViewState["PersonID"] = person.PersonID;
            this.txtFirstName.Text = person.FirstName;
            this.txtLastName.Text = person.LastName;
            this.txtNationalCode.Text = person.NationalCode;
            this.drpGender.SelectedValue = person.Gender.GetValueOrDefault().ToString();
            this.txtMobile.Text = person.Mobile;
        }
        else
        {
            this.ViewState["PersonID"] = null;
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            this.drpGender.SelectedIndex = 0;
            this.txtMobile.Text = null;
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