using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;

public partial class Ajancy_BusinessLicense : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.User.IsInRole(Public.Role.AjancyManager.ToString()) && Public.ActiveAjancy.AjancyID > 0)
            {
                DisposeContext();
                Response.Redirect("~/Message.aspx?mode=9");
            }

            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();
            this.drpProvince.Enabled = user.PersonID == (short)Public.Role.Admin ? true : false;
            this.drpCity.Enabled = user.PersonID == (short)Public.Role.Admin ? true : false;
            int ajancyId = 0;
            if (HttpContext.Current.User.IsInRole(Public.Role.AjancyManager.ToString()))
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.User);
                dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.AjancyPartners);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;

                Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.NationalCode == HttpContext.Current.User.Identity.Name);
                if (person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyManager &&
                                                                                                                  ur.LockOutDate == null &&
                                                                                                                  ur.AjancyPartners.Any<Ajancy.AjancyPartner>(jp => jp.AjancyID == null)))
                {
                    SetPerson(person);
                }
                else
                {
                    DisposeContext();
                    Response.Redirect("~/Default.aspx");
                }
            }
            else if (!HttpContext.Current.User.IsInRole(Public.Role.AjancyManager.ToString()) && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["j"]), out ajancyId)) // Edit mode
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyUtilities);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.BusinessLicenses);
                dlo.LoadWith<Ajancy.BusinessLicense>(bl => bl.BusinessLicenseRevivals);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyPartners);
                dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.UsersInRole);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.User);
                dlo.LoadWith<Ajancy.User>(u => u.Person);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;
                Ajancy.Ajancy ajancy = db.Ajancies.FirstOrDefault<Ajancy.Ajancy>(j => j.AjancyID == ajancyId);
                SetPerson(ajancy.AjancyPartners.Single<Ajancy.AjancyPartner>(jp => jp.UsersInRole.RoleID == (short)Public.Role.AjancyManager && jp.LockOutDate == null).UsersInRole.User.Person);
                SetAjancy(ajancy);
            }
            else
            {
                DisposeContext();
                Response.Redirect("~/Default.aspx");
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
        if (this.Page.IsValid)
        {
            int ajancyId = 0;
            Ajancy.Person person = null;
            Ajancy.Ajancy ajancy = new Ajancy.Ajancy();
            Ajancy.BusinessLicense businessLicense = new Ajancy.BusinessLicense();
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

            if (HttpContext.Current.User.IsInRole(Public.Role.AjancyManager.ToString())) // Add mode
            {
                if (db.Ajancies.Any<Ajancy.Ajancy>(j => j.PostalCode == this.txtPlacePostalCode.Text.Trim()))
                {
                    this.lblMessage.Text = "کد پستی واحد تجاری تکراری میباشد";// Duplicate PostalCode 
                    return;
                }
                else if (db.Ajancies.Any<Ajancy.Ajancy>(j => j.CityID == int.Parse(this.drpCity.SelectedValue) &&
                                                                                       j.AjancyType != (byte)Public.AjancyType.Academy &&
                                                                                       j.AjancyName.Equals(this.txtAjancyName.Text.Trim())))
                {
                    this.lblMessage.Text = string.Format("آژانسی با نام <b>{0}</b> در شهرستان <b>{1}</b> موجود میباشد", ajancy.AjancyName, this.drpCity.SelectedItem.Text);
                    return;
                }

                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.User);
                dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.AjancyPartners);
                db.LoadOptions = dlo;

                person = db.Persons.First<Ajancy.Person>(p => p.NationalCode == HttpContext.Current.User.Identity.Name);
                Ajancy.AjancyPartner partner = person.User.UsersInRoles.Single<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.AjancyManager && ur.LockOutDate == null).AjancyPartners.Single<Ajancy.AjancyPartner>(jp => jp.AjancyID == null);
                ajancy.AjancyPartners.Add(partner);
                ajancy.BusinessLicenses.Add(businessLicense);
                businessLicense.BusinessLicenseRevivals.Add(new Ajancy.BusinessLicenseRevival { StartDate = this.txtStartDate.GeorgianDate.Value, EndDate = this.txtEndDate.GeorgianDate.Value, SubmitDate = DateTime.Now });
                ajancy.SubmitDate = DateTime.Now;
                db.Ajancies.InsertOnSubmit(ajancy);
            }
            else if (!HttpContext.Current.User.IsInRole(Public.Role.AjancyManager.ToString()) && int.TryParse(this.ViewState["AjancyID"].ToString(), out ajancyId)) // Edit mode
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyUtilities);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.BusinessLicenses);
                dlo.LoadWith<Ajancy.BusinessLicense>(bl => bl.BusinessLicenseRevivals);
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyPartners);
                dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.UsersInRole);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.User);
                dlo.LoadWith<Ajancy.User>(u => u.Person);
                db.LoadOptions = dlo;

                ajancy = db.Ajancies.FirstOrDefault<Ajancy.Ajancy>(j => j.AjancyID == ajancyId);
                businessLicense = ajancy.BusinessLicenses.First<Ajancy.BusinessLicense>();
                person = ajancy.AjancyPartners.Single<Ajancy.AjancyPartner>(jp => jp.UsersInRole.RoleID == (short)Public.Role.AjancyManager && jp.LockOutDate == null).UsersInRole.User.Person;
            }

            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();
            person.Father = this.txtFather.Text.Trim();
            person.BirthCertificateNo = this.txtBirthCertificateNo.Text.Trim();
            person.BirthCertificateSerial = this.txtBirthCertificateSerial.Text.Trim();
            person.BirthCertificateSerie = this.txtBirthCertificateSerie.Text.Trim();
            person.BirthCertificateAlfa = this.drpBirthCertificateAlfa.SelectedValue;
            person.Gender = Public.ToByte(this.drpGender.SelectedValue);
            person.Marriage = Public.ToByte(this.drpMarriage.SelectedValue);
            person.BirthDate = this.txtBirthDate.GeorgianDate.Value;
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

            ajancy.AjancyType = Public.ToByte(this.drpAjancyType.SelectedValue);
            ajancy.AjancyName = this.txtAjancyName.Text.Trim();
            ajancy.CityID = Public.ToShort(this.drpCity.SelectedValue);
            ajancy.BusinessLicenseType = Public.ToByte(this.drpBusinessLicenseType.SelectedValue);
            ajancy.OfficePosition = Public.ToByte(this.drpOfficePosition.SelectedValue);
            ajancy.OfficeLevel = this.txtOfficeLevel.Text.Trim();
            ajancy.OfficeSpace = this.txtOfficeSpace.Text.Trim();
            ajancy.BalconySpace = this.txtBalconySpace.Text.Trim();
            ajancy.BalconyHeight = this.txtBalconyHeight.Text.Trim();
            ajancy.ParkingSpace = this.txtParkingSpace.Text.Trim();
            ajancy.ParkingState = this.drpParkingState.SelectedIndex == 0 ? true : false;
            ajancy.BusinessScope = this.txtBusinessScope.Text.Trim();
            ajancy.PoliceStation = this.txtPoliceStation.Text.Trim();
            ajancy.Mayor = (byte)this.drpMayor.SelectedIndex;
            ajancy.Address = this.txtPlaceAddress.Text.Trim();
            ajancy.Phone = this.txtBusinessPlacePhone.Text.Trim();
            ajancy.RegisteredPelak = this.txtRegisteredPelak.Text.Trim();
            ajancy.BluePelak = this.txtBluePelak.Text.Trim();
            ajancy.PostalCode = this.txtPlacePostalCode.Text.Trim();
            ajancy.EstateType = Public.ToByte(this.drpEstateType.SelectedValue);
            ajancy.DocumentType = Public.ToByte(this.drpDocumentType.SelectedValue);
            ajancy.PlaceOwner = this.txtPlaceOwner.Text.Trim();

            businessLicense.BusinessLicenseNo = this.txtBusinessLicenseNo.Text.Trim();
            businessLicense.MemberShipCode = this.txtMemberShipCode.Text.Trim();
            businessLicense.NationalCardBarCode = this.txtNationalCardBarCode.Text.Trim();
            businessLicense.SerialNo = this.txtSerialNo.Text.Trim();
            businessLicense.CategoryCode = this.txtCategoryCode.Text.Trim();
            businessLicense.ISIC = this.txtISIC.Text.Trim();
            businessLicense.SubmitDate = DateTime.Now;

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

            if (!string.IsNullOrEmpty(this.txtPartnerName1.Text.Trim()) && !string.IsNullOrEmpty(this.txtPartnerFamily1.Text.Trim()) && !string.IsNullOrEmpty(this.txtPartnerNationalCode1.Text.Trim()))
            {
                Ajancy.Person prsn = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtPartnerNationalCode1.Text.Trim());
                Ajancy.UsersInRole userRole = null;
                if (prsn == null)
                {
                    prsn = new Ajancy.Person { NationalCode = this.txtPartnerNationalCode1.Text.Trim(), FirstName = this.txtPartnerName1.Text.Trim(), LastName = this.txtPartnerFamily1.Text.Trim(), SubmitDate = DateTime.Now };
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtPartnerNationalCode1.Text.Trim();
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
            if (!string.IsNullOrEmpty(this.txtPartnerName2.Text.Trim()) && !string.IsNullOrEmpty(this.txtPartnerFamily2.Text.Trim()) && !string.IsNullOrEmpty(this.txtPartnerNationalCode2.Text.Trim()))
            {
                Ajancy.Person prsn = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtPartnerNationalCode2.Text.Trim());
                Ajancy.UsersInRole userRole = null;
                if (prsn == null)
                {
                    prsn = new Ajancy.Person { NationalCode = this.txtPartnerNationalCode2.Text.Trim(), FirstName = this.txtPartnerName2.Text.Trim(), LastName = this.txtPartnerFamily2.Text.Trim(), SubmitDate = DateTime.Now };
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtPartnerNationalCode2.Text.Trim();
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
            if (!string.IsNullOrEmpty(this.txtPartnerName3.Text.Trim()) && !string.IsNullOrEmpty(this.txtPartnerFamily3.Text.Trim()) && !string.IsNullOrEmpty(this.txtPartnerNationalCode3.Text.Trim()))
            {
                Ajancy.Person prsn = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtPartnerNationalCode3.Text.Trim());
                Ajancy.UsersInRole userRole = null;
                if (prsn == null)
                {
                    prsn = new Ajancy.Person { NationalCode = this.txtPartnerNationalCode3.Text.Trim(), FirstName = this.txtPartnerName3.Text.Trim(), LastName = this.txtPartnerFamily3.Text.Trim(), SubmitDate = DateTime.Now };
                    Ajancy.User user = new Ajancy.User();
                    user.UserName = this.txtPartnerNationalCode3.Text.Trim();
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

            try
            {
                db.SubmitChanges();
                DisposeContext();
                this.Session["Ajancy"] = new Ajancy.Ajancy { AjancyID = ajancy.AjancyID, AjancyType = ajancy.AjancyType, AjancyName = ajancy.AjancyName };
                Response.Redirect("~/Message.aspx?mode=4");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_BusinessLicenses"))
                {
                    this.lblMessage.Text = "شماره پروانه کسب تکراری میباشد";
                }
                else
                {
                    throw ex;
                }
            }
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person != null)
        {
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
            this.txtAjancyName.Text = null;
            this.txtBusinessLicenseNo.Text = null;
            this.txtMemberShipCode.Text = null;
            this.txtNationalCardBarCode.Text = null;
            this.txtSerialNo.Text = null;
            this.txtCategoryCode.Text = null;
            this.txtISIC.Text = null;
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
            SetPartners(null);
        }
        else
        {
            this.ViewState["AjancyID"] = ajancy.AjancyID;
            this.drpAjancyType.SelectedValue = ajancy.AjancyType.ToString();
            //this.drpProvince.SelectedValue = ajancy.City.ProvinceID.ToString();
            this.drpCity.SelectedValue = ajancy.CityID.ToString();
            this.drpBusinessLicenseType.SelectedValue = ajancy.BusinessLicenseType.ToString();
            this.txtAjancyName.Text = ajancy.AjancyName;
            this.drpOfficePosition.SelectedValue = ajancy.OfficePosition.ToString();
            this.txtOfficeLevel.Text = ajancy.OfficeLevel;
            this.txtOfficeSpace.Text = ajancy.OfficeSpace;
            this.txtBalconySpace.Text = ajancy.BalconySpace;
            this.txtBalconyHeight.Text = ajancy.BalconyHeight;
            this.txtParkingSpace.Text = ajancy.ParkingSpace;
            this.drpParkingState.SelectedIndex = ajancy.ParkingState.GetValueOrDefault() ? 0 : 1;
            this.txtBusinessScope.Text = ajancy.BusinessScope;
            this.txtPoliceStation.Text = ajancy.PoliceStation;
            this.drpMayor.SelectedIndex = ajancy.Mayor.GetValueOrDefault() == 0 ? 0 : ajancy.Mayor.GetValueOrDefault();
            this.txtBusinessPlacePhone.Text = ajancy.Phone;
            this.txtRegisteredPelak.Text = ajancy.RegisteredPelak;
            this.txtBluePelak.Text = ajancy.BluePelak;
            this.txtPlacePostalCode.Text = ajancy.PostalCode;
            this.txtPlaceAddress.Text = ajancy.Address;
            this.drpEstateType.SelectedValue = ajancy.EstateType.ToString();
            this.drpDocumentType.SelectedValue = ajancy.DocumentType.ToString();
            this.txtPlaceOwner.Text = ajancy.PlaceOwner;

            Ajancy.BusinessLicense businessLicense = ajancy.BusinessLicenses.LastOrDefault<Ajancy.BusinessLicense>();
            if (businessLicense != null)
            {
                this.txtBusinessLicenseNo.Text = businessLicense.BusinessLicenseNo;
                this.txtMemberShipCode.Text = businessLicense.MemberShipCode;
                this.txtNationalCardBarCode.Text = businessLicense.NationalCardBarCode;
                this.txtSerialNo.Text = businessLicense.SerialNo;
                this.txtCategoryCode.Text = businessLicense.CategoryCode;
                this.txtISIC.Text = businessLicense.ISIC;
                Ajancy.BusinessLicenseRevival businessLicenseRevival = businessLicense.BusinessLicenseRevivals.LastOrDefault<Ajancy.BusinessLicenseRevival>();
                if (businessLicenseRevival != null)
                {
                    this.txtStartDate.SetDate(businessLicenseRevival.StartDate);
                    this.txtEndDate.SetDate(businessLicenseRevival.EndDate);
                }
            }
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

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}