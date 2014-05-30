using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Requests_BusinessLicense : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = "240";
            int ajancyId = 0;
            if (Request.QueryString["j"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["j"]), out ajancyId))
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyUtilities);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyPartners);
                dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.UsersInRole);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.User);
                dlo.LoadWith<Ajancy.User>(u => u.Person);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;
                Ajancy.Ajancy ajancy = db.Ajancies.FirstOrDefault<Ajancy.Ajancy>(j => j.AjancyID == ajancyId);
                SetPerson(ajancy.AjancyPartners.Single<Ajancy.AjancyPartner>(jp => jp.UsersInRole.RoleID == (short)Public.Role.AjancyManager && jp.LockOutDate != null).UsersInRole.User.Person);
                SetAjancy(ajancy);
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
            Ajancy.Person person = null;
            Ajancy.Ajancy ajancy = new Ajancy.Ajancy();
            Ajancy.FormerBusiness formerBusiness = new Ajancy.FormerBusiness();
            Ajancy.UsersInRole usersInRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyManager, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now };
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

            if (this.ViewState["AjancyID"] == null) // Add mode
            {
                ajancy.SubmitDate = DateTime.Now;
                db.Ajancies.InsertOnSubmit(ajancy);

                if (this.ViewState["PersonID"] == null) // Add mode
                {
                    person = new Ajancy.Person();
                    person.NationalCode = this.txtNationalCode.Text;
                    person.SubmitDate = DateTime.Now;
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtNationalCode.Text;
                    user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                    user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    user.SubmitDate = DateTime.Now;
                    usersInRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    user.UsersInRoles.Add(usersInRole);
                    person.User = user;
                    db.Persons.InsertOnSubmit(person);
                }
                else // Edit mode
                {
                    dlo = new DataLoadOptions();
                    dlo.LoadWith<Ajancy.Person>(p => p.User);
                    dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
                    dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.AjancyPartners);
                    db.LoadOptions = dlo;
                    person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));
                    usersInRole = person.User.UsersInRoles.SingleOrDefault<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyManager);
                    if (usersInRole == null)
                    {
                        usersInRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyManager, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now };
                        person.User.UsersInRoles.Add(usersInRole);
                    }
                    usersInRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now, LockOutDate = DateTime.Now });
                }
            }
            else // Edit mode
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyUtilities);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.BusinessLicenses);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyPartners);
                dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.UsersInRole);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.User);
                dlo.LoadWith<Ajancy.User>(u => u.Person);
                db.LoadOptions = dlo;
                ajancy = db.Ajancies.FirstOrDefault<Ajancy.Ajancy>(j => j.AjancyID == Public.ToInt(this.ViewState["AjancyID"]));
                person = ajancy.AjancyPartners.Single<Ajancy.AjancyPartner>(jp => jp.UsersInRole.RoleID == (short)Public.Role.AjancyManager && jp.LockOutDate != null).UsersInRole.User.Person;
            }

            if (ajancy.FormerBusiness == null && !string.IsNullOrEmpty(this.txtFormerBusinessLicenseNo.Text))
            {
                formerBusiness.BusinessLicenseNo = this.txtFormerBusinessLicenseNo.Text;
                formerBusiness.BusinessLicenseDate = this.txtFormerBusinessLicenseDate.GeorgianDate != null ? this.txtFormerBusinessLicenseDate.GeorgianDate : null;
                formerBusiness.BusinessStartDate = this.txtFormerBusinessStartDate.GeorgianDate != null ? this.txtFormerBusinessStartDate.GeorgianDate : null;
                formerBusiness.Board = this.txtFormerBusinessBoard.Text;
                ajancy.FormerBusiness = formerBusiness;
            }
            else if (!string.IsNullOrEmpty(this.txtFormerBusinessLicenseNo.Text))
            {
                ajancy.FormerBusiness.BusinessLicenseNo = this.txtFormerBusinessLicenseNo.Text;
                ajancy.FormerBusiness.BusinessLicenseDate = this.txtFormerBusinessLicenseDate.GeorgianDate != null ? this.txtFormerBusinessLicenseDate.GeorgianDate : null;
                ajancy.FormerBusiness.BusinessStartDate = this.txtFormerBusinessStartDate.GeorgianDate != null ? this.txtFormerBusinessStartDate.GeorgianDate : null;
                ajancy.FormerBusiness.Board = this.txtFormerBusinessBoard.Text;
            }

            person.FirstName = this.txtFirstName.Text;
            person.LastName = this.txtLastName.Text;
            person.Father = this.txtFather.Text;
            person.BirthCertificateNo = this.txtBirthCertificateNo.Text;
            person.BirthCertificateSerial = this.txtBirthCertificateSerial.Text;
            person.BirthCertificateSerie = this.txtBirthCertificateSerie.Text;
            person.BirthCertificateAlfa = this.drpBirthCertificateAlfa.SelectedValue;
            person.Gender = Public.ToByte(this.drpGender.SelectedValue);
            person.Marriage = Public.ToByte(this.drpMarriage.SelectedValue);
            person.BirthDate = this.txtBirthDate.GeorgianDate.Value;
            person.BirthPlace = this.txtBirthPlace.Text;
            person.BirthCertificatePlace = this.txtBirthCertificatePlace.Text;
            person.FamilyMembersCount = this.txtFamilyMembersCount.Text;
            person.Education = Public.ToByte(this.drpEducation.SelectedValue);
            person.MilitaryService = Public.ToByte(this.drpMilitaryService.SelectedValue);
            person.Religion = Public.ToByte(this.drpReligion.SelectedValue);
            person.Subreligion = this.txtSubreligion.Text;
            person.JobStatus = Public.ToByte(this.drpJobStatus.SelectedValue);
            person.Phone = this.txtPhone.Text;
            person.Mobile = this.txtMobile.Text;
            person.PostalCode = this.txtPostalCode.Text;
            person.Address = this.txtAddress.Text;

            ajancy.AjancyType = Public.ToByte(this.drpAjancyType.SelectedValue);
            ajancy.CityID = Public.ToShort(this.drpCity.SelectedValue);
            ajancy.BusinessLicenseType = Public.ToByte(this.drpBusinessLicenseType.SelectedValue);
            ajancy.OfficePosition = Public.ToByte(this.drpOfficePosition.SelectedValue);
            ajancy.OfficeLevel = this.txtOfficeLevel.Text;
            ajancy.OfficeSpace = this.txtOfficeSpace.Text;
            ajancy.BalconySpace = this.txtBalconySpace.Text;
            ajancy.BalconyHeight = this.txtBalconyHeight.Text;
            ajancy.ParkingSpace = this.txtParkingSpace.Text;
            ajancy.ParkingState = this.drpParkingState.SelectedIndex == 0 ? true : false;
            ajancy.BusinessScope = this.txtBusinessScope.Text;
            ajancy.PoliceStation = this.txtPoliceStation.Text;
            ajancy.Mayor = Public.ToByte(this.drpMayor.SelectedValue);
            ajancy.Address = this.txtPlaceAddress.Text;
            ajancy.Phone = this.txtBusinessPlacePhone.Text;
            ajancy.RegisteredPelak = this.txtRegisteredPelak.Text;
            ajancy.BluePelak = this.txtBluePelak.Text;
            ajancy.PostalCode = this.txtPlacePostalCode.Text;
            ajancy.EstateType = Public.ToByte(this.drpEstateType.SelectedValue);
            ajancy.DocumentType = Public.ToByte(this.drpDocumentType.SelectedValue);
            ajancy.PlaceOwner = this.txtPlaceOwner.Text;
            ajancy.WaterBillSerial = this.txtWaterBillSerial.Text;
            ajancy.ElectricityBillSerial = this.txtElectricityBillSerial.Text;
            ajancy.GasBillSerial = this.txtGasBillSerial.Text;

            Ajancy.AjancyUtility utility = null;
            foreach (ListItem item in this.lstUtility.Items)
            {
                utility = ajancy.AjancyUtilities.FirstOrDefault<Ajancy.AjancyUtility>(u => u.Utility == Public.ToByte(item.Value));
                if (!item.Selected && utility != null)
                {
                    db.AjancyUtilities.DeleteOnSubmit(utility);
                }
                else if (item.Selected && utility == null)
                {
                    ajancy.AjancyUtilities.Add(new Ajancy.AjancyUtility { Utility = Public.ToByte(item.Value) });
                }
            }

            #region SetPartners

            if (!string.IsNullOrEmpty(this.txtPartnerName1.Text) && !string.IsNullOrEmpty(this.txtPartnerFamily1.Text) && !string.IsNullOrEmpty(this.txtPartnerNationalCode1.Text))
            {
                Ajancy.Person prsn = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtPartnerNationalCode1.Text);
                Ajancy.UsersInRole userRole = null;
                if (prsn == null)
                {
                    prsn = new Ajancy.Person { NationalCode = this.txtPartnerNationalCode1.Text, FirstName = this.txtPartnerName1.Text, LastName = this.txtPartnerFamily1.Text, SubmitDate = DateTime.Now };
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtPartnerNationalCode1.Text;
                    user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                    user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    user.SubmitDate = DateTime.Now;
                    userRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyPartner, MembershipDate = DateTime.Now };
                    userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    user.UsersInRoles.Add(userRole);
                    prsn.User = user;
                    db.Persons.InsertOnSubmit(prsn);
                }
                else
                {
                    userRole = prsn.User.UsersInRoles.SingleOrDefault<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyPartner);
                    if (userRole == null)
                    {
                        userRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyPartner, MembershipDate = DateTime.Now };
                        userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                        prsn.User.UsersInRoles.Add(userRole);
                    }
                    else if (!userRole.AjancyPartners.Any<Ajancy.AjancyPartner>(jp => jp.AjancyID == ajancy.AjancyID))
                    {
                        userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.txtPartnerName2.Text) && !string.IsNullOrEmpty(this.txtPartnerFamily2.Text) && !string.IsNullOrEmpty(this.txtPartnerNationalCode2.Text))
            {
                Ajancy.Person prsn = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtPartnerNationalCode2.Text);
                Ajancy.UsersInRole userRole = null;
                if (prsn == null)
                {
                    prsn = new Ajancy.Person { NationalCode = this.txtPartnerNationalCode2.Text, FirstName = this.txtPartnerName2.Text, LastName = this.txtPartnerFamily2.Text, SubmitDate = DateTime.Now };
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtPartnerNationalCode2.Text;
                    user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                    user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    user.SubmitDate = DateTime.Now;
                    userRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyPartner, MembershipDate = DateTime.Now };
                    userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    user.UsersInRoles.Add(userRole);
                    prsn.User = user;
                    db.Persons.InsertOnSubmit(prsn);
                }
                else
                {
                    userRole = prsn.User.UsersInRoles.SingleOrDefault<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyPartner);
                    if (userRole == null)
                    {
                        userRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyPartner, MembershipDate = DateTime.Now };
                        userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                        prsn.User.UsersInRoles.Add(userRole);
                    }
                    else if (!userRole.AjancyPartners.Any<Ajancy.AjancyPartner>(jp => jp.AjancyID == ajancy.AjancyID))
                    {
                        userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.txtPartnerName3.Text) && !string.IsNullOrEmpty(this.txtPartnerFamily3.Text) && !string.IsNullOrEmpty(this.txtPartnerNationalCode3.Text))
            {
                Ajancy.Person prsn = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtPartnerNationalCode3.Text);
                Ajancy.UsersInRole userRole = null;
                if (prsn == null)
                {
                    prsn = new Ajancy.Person { NationalCode = this.txtPartnerNationalCode3.Text, FirstName = this.txtPartnerName3.Text, LastName = this.txtPartnerFamily3.Text, SubmitDate = DateTime.Now };
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtPartnerNationalCode3.Text;
                    user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                    user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    user.SubmitDate = DateTime.Now;
                    userRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyPartner, MembershipDate = DateTime.Now };
                    userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    user.UsersInRoles.Add(userRole);
                    prsn.User = user;
                    db.Persons.InsertOnSubmit(prsn);
                }
                else
                {
                    userRole = prsn.User.UsersInRoles.SingleOrDefault<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyPartner);
                    if (userRole == null)
                    {
                        userRole = new Ajancy.UsersInRole { RoleID = (short)Public.Role.AjancyPartner, MembershipDate = DateTime.Now };
                        userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                        prsn.User.UsersInRoles.Add(userRole);
                    }
                    else if (!userRole.AjancyPartners.Any<Ajancy.AjancyPartner>(jp => jp.AjancyID == ajancy.AjancyID))
                    {
                        userRole.AjancyPartners.Add(new Ajancy.AjancyPartner { Ajancy = ajancy, SubmitDate = DateTime.Now });
                    }
                }
            }

            #endregion

            string url = null;
            try
            {
                db.SubmitChanges();
                url = string.Format("~/Message.aspx?mode=1&c={0}", null);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_Ajancies")) // Duplicate BusinessLicense Request for one place
                {
                    url = "~/Message.aspx?mode=3";
                }
                else if (ex.Message.Contains("Native"))
                {
                    url = "~/Message.aspx?mode=1";
                }
            }
            DisposeContext();
            Response.Redirect(url);
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person != null)
        {
            this.ViewState["PersonID"] = person.PersonID.ToString();
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
            this.ViewState["PersonID"] = null;
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

    private void SetAjancy(Ajancy.Ajancy ajancy)
    {
        if (ajancy == null)
        {
            this.ViewState["AjancyID"] = null;
            this.drpAjancyType.SelectedIndex = 0;
            this.drpProvince.SelectedIndex = 0;
            this.drpCity.SelectedIndex = 0;
            this.drpBusinessLicenseType.SelectedIndex = 0;
            this.drpOfficePosition.SelectedIndex = 0;
            this.txtOfficeLevel.Text = null;
            this.txtOfficeSpace.Text = null;
            this.txtBalconySpace.Text = null;
            this.txtBalconyHeight.Text = null;
            this.txtParkingSpace.Text = null;
            this.drpParkingState.SelectedIndex = 0;
            this.txtBusinessScope.Text = null;
            this.txtPoliceStation.Text = null;
            this.drpMayor.SelectedIndex = 0;
            this.drpBusinessLicenseType.SelectedIndex = 0;
            this.txtBusinessPlacePhone.Text = null;
            this.txtRegisteredPelak.Text = null;
            this.txtBluePelak.Text = null;
            this.drpEstateType.SelectedIndex = 0;
            this.drpDocumentType.SelectedIndex = 0;
            this.txtPlaceOwner.Text = null;
            this.txtPlaceAddress.Text = null;
            this.txtPlacePostalCode.Text = null;
            this.lstUtility.ClearSelection();
            SetFormerBusiness(null);
            SetPartners(null);
        }
        else
        {
            this.ViewState["AjancyID"] = ajancy.AjancyID;
            this.drpAjancyType.SelectedValue = ajancy.AjancyType.ToString();
            //this.drpProvince.SelectedValue = ajancy.City.ProvinceID.ToString();
            this.drpCity.SelectedValue = ajancy.CityID.ToString();
            this.drpBusinessLicenseType.SelectedValue = ajancy.BusinessLicenseType.ToString();
            this.drpOfficePosition.SelectedValue = ajancy.OfficePosition.ToString();
            this.txtOfficeLevel.Text = ajancy.OfficeLevel;
            this.txtOfficeSpace.Text = ajancy.OfficeSpace;
            this.txtBalconySpace.Text = ajancy.BalconySpace;
            this.txtBalconyHeight.Text = ajancy.BalconyHeight;
            this.txtParkingSpace.Text = ajancy.ParkingSpace;
            this.drpParkingState.SelectedIndex = ajancy.ParkingState.GetValueOrDefault() ? 0 : 1;
            this.txtBusinessScope.Text = ajancy.BusinessScope;
            this.txtPoliceStation.Text = ajancy.PoliceStation;
            this.drpMayor.SelectedValue = ajancy.Mayor.ToString();
            this.txtBusinessPlacePhone.Text = ajancy.Phone;
            this.txtRegisteredPelak.Text = ajancy.RegisteredPelak;
            this.txtBluePelak.Text = ajancy.BluePelak;
            this.txtPlacePostalCode.Text = ajancy.PostalCode;
            this.txtPlaceAddress.Text = ajancy.Address;
            this.txtWaterBillSerial.Text = ajancy.WaterBillSerial;
            this.txtElectricityBillSerial.Text = ajancy.ElectricityBillSerial;
            this.txtGasBillSerial.Text = ajancy.GasBillSerial;
            this.drpEstateType.SelectedValue = ajancy.EstateType.ToString();
            this.drpDocumentType.SelectedValue = ajancy.DocumentType.ToString();
            this.txtPlaceOwner.Text = ajancy.PlaceOwner;
            SetFormerBusiness(ajancy.FormerBusiness);
            SetPartners(ajancy.AjancyPartners.Where<Ajancy.AjancyPartner>(jp => jp.UsersInRole.RoleID == (short)Public.Role.AjancyPartner).ToList<Ajancy.AjancyPartner>());
            foreach (ListItem item in this.lstUtility.Items)
            {
                item.Selected = ajancy.AjancyUtilities.Any<Ajancy.AjancyUtility>(u => u.Utility == Public.ToByte(item.Value));
            }
        }
    }

    private void SetPartners(List<Ajancy.AjancyPartner> partners)
    {
        if (partners != null && partners.Count > 0)
        {
            Ajancy.Person prs = null;
            for (int i = 0; i < partners.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        prs = partners[i].UsersInRole.User.Person;
                        this.txtPartnerNationalCode1.Text = prs.NationalCode;
                        this.txtPartnerName1.Text = prs.FirstName;
                        this.txtPartnerFamily1.Text = prs.LastName;
                        break;

                    case 1:
                        prs = partners[i].UsersInRole.User.Person;
                        this.txtPartnerNationalCode2.Text = prs.NationalCode;
                        this.txtPartnerName2.Text = prs.FirstName;
                        this.txtPartnerFamily2.Text = prs.LastName;
                        break;

                    case 2:
                        prs = partners[i].UsersInRole.User.Person;
                        this.txtPartnerNationalCode3.Text = prs.NationalCode;
                        this.txtPartnerName3.Text = prs.FirstName;
                        this.txtPartnerFamily3.Text = prs.LastName;
                        break;
                }
            }
        }
        else
        {
            this.txtPartnerNationalCode1.Text = null;
            this.txtPartnerNationalCode2.Text = null;
            this.txtPartnerNationalCode3.Text = null;
            this.txtPartnerName1.Text = null;
            this.txtPartnerName2.Text = null;
            this.txtPartnerName3.Text = null;
            this.txtPartnerFamily1.Text = null;
            this.txtPartnerFamily2.Text = null;
            this.txtPartnerFamily3.Text = null;
        }
    }

    private void SetFormerBusiness(Ajancy.FormerBusiness formerBusiness)
    {
        if (formerBusiness != null)
        {
            this.txtFormerBusinessLicenseNo.Text = formerBusiness.BusinessLicenseNo;
            this.txtFormerBusinessLicenseDate.SetDate(formerBusiness.BusinessLicenseDate.GetValueOrDefault());
            this.txtFormerBusinessStartDate.SetDate(formerBusiness.BusinessStartDate.GetValueOrDefault());
            this.txtFormerBusinessBoard.Text = formerBusiness.Board;
        }
        else
        {
            this.txtFormerBusinessLicenseNo.Text = null;
            this.txtFormerBusinessLicenseDate.Text = null;
            this.txtFormerBusinessStartDate.Text = null;
            this.txtFormerBusinessBoard.Text = null;
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