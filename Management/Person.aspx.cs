using System;
using System.Linq;
using System.Data.Linq;

public partial class Management_Person : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int personId = 0;
            if (int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId)) // Edit mode
            {
                switch (Public.ActiveUserRole.RoleID)
                {
                    case (short)Public.Role.Admin:
                        this.drpProvince.Enabled = true;
                        this.drpCity.Enabled = true;
                        break;

                    case (short)Public.Role.ProvinceManager:
                        this.drpCity.Enabled = true;
                        break;
                }

                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.User);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;
                SetPerson(db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == personId));
            }
            else
            {
                DisposeContext();
                Response.Redirect("~/Management/UsersList.aspx");
            }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int personId = 0;
        if (this.Page.IsValid && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId))
        {
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.Person>(p => p.User);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;
            Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.PersonID == personId);
            if (person.NationalCode != this.txtNationalCode.Text.Trim()) // Nationalcode is changed
            {
                person.NationalCode = this.txtNationalCode.Text;
                person.User.UserName = this.txtNationalCode.Text;
            }

            person.User.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
            person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();
            person.Father = this.txtFather.Text.Trim();
            person.BirthCertificateNo = this.txtBirthCertificateNo.Text.Trim();
            person.BirthCertificateSerial = this.txtBirthCertificateSerial.Text.Trim();
            person.BirthCertificateSerie = this.txtBirthCertificateSerie.Text.Trim();
            person.BirthCertificateAlfa = this.drpBirthCertificateAlfa.SelectedValue;
            person.Gender = Public.ToByte(this.drpGender.SelectedValue);
            person.Marriage = Public.ToByte(this.drpMarriage.SelectedValue);
            person.BirthDate = this.txtBirthDate.GeorgianDate;
            person.BirthPlace = this.txtBirthPlace.Text.Trim();
            person.BirthCertificatePlace = this.txtBirthCertificatePlace.Text.Trim();
            person.FamilyMembersCount = this.txtFamilyMembersCount.Text.Trim();
            person.Education = Public.ToByte(this.drpEducation.SelectedValue);
            person.MilitaryService = Public.ToByte(this.drpMilitaryService.SelectedValue);
            person.Religion = Public.ToByte(this.drpReligion.SelectedValue);
            person.Subreligion = this.txtSubreligion.Text.Trim();
            person.JobStatus = Public.ToByte(this.drpJobStatus.SelectedValue);
            person.Phone = this.txtPhone.Text.Trim();
            person.Mobile = this.txtMobile.Text.Trim();
            person.PostalCode = this.txtPostalCode.Text.Trim();
            person.Address = this.txtAddress.Text.Trim();

            try
            {
                db.SubmitChanges();
                DisposeContext();
                Response.Redirect("~/Message.aspx?mode=17");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate key"))
                {
                    this.lblMessage.Text = "کد ملی تکراری میباشد";
                }
            }
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person != null)
        {
            this.drpProvince.SelectedValue = person.User.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(this, new EventArgs());
            this.drpCity.SelectedValue = person.User.CityID.ToString();
            this.txtFirstName.Text = person.FirstName;
            this.txtLastName.Text = person.LastName;
            this.txtFather.Text = person.Father;
            this.txtNationalCode.Text = person.NationalCode;
            this.txtBirthCertificateNo.Text = person.BirthCertificateNo;
            this.txtBirthCertificateSerial.Text = person.BirthCertificateSerial;
            this.txtBirthCertificateSerie.Text = person.BirthCertificateSerie;
            this.drpBirthCertificateAlfa.SelectedValue = person.BirthCertificateAlfa;
            this.drpGender.SelectedValue = person.Gender.GetValueOrDefault().ToString();
            this.txtBirthDate.SetDate(person.BirthDate);
            this.txtBirthPlace.Text = person.BirthPlace;
            this.txtBirthCertificatePlace.Text = person.BirthCertificatePlace;
            this.drpMarriage.SelectedValue = person.Marriage.GetValueOrDefault().ToString();
            this.txtFamilyMembersCount.Text = person.FamilyMembersCount;
            this.drpEducation.SelectedValue = person.Education.GetValueOrDefault().ToString();
            this.drpMilitaryService.SelectedValue = person.MilitaryService.GetValueOrDefault().ToString();
            this.drpReligion.SelectedValue = person.Religion.GetValueOrDefault().ToString();
            this.txtSubreligion.Text = person.Subreligion;
            this.drpJobStatus.SelectedValue = person.JobStatus.GetValueOrDefault().ToString();
            this.txtPhone.Text = person.Phone;
            this.txtMobile.Text = person.Mobile;
            this.txtPostalCode.Text = person.PostalCode;
            this.txtAddress.Text = person.Address;
        }
        else
        {
            this.drpProvince.SelectedIndex = 0;
            this.drpCity.SelectedIndex = 0;
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            this.txtFather.Text = null;
            this.txtBirthCertificateNo.Text = null;
            this.txtBirthCertificateSerial.Text = null;
            this.txtBirthCertificateSerie.Text = null;
            this.drpBirthCertificateAlfa.SelectedIndex = 0;
            this.drpGender.SelectedIndex = 0;
            this.txtBirthDate.Text = null;
            this.txtBirthPlace.Text = null;
            this.txtBirthCertificatePlace.Text = null;
            this.drpMarriage.SelectedIndex = 0;
            this.txtFamilyMembersCount.Text = null;
            this.drpEducation.SelectedIndex = 0;
            this.drpMilitaryService.SelectedIndex = 0;
            this.drpReligion.SelectedIndex = 0;
            this.txtSubreligion.Text = null;
            this.drpJobStatus.SelectedIndex = 0;
            this.txtPhone.Text = null;
            this.txtMobile.Text = null;
            this.txtPostalCode.Text = null;
            this.txtAddress.Text = null;
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