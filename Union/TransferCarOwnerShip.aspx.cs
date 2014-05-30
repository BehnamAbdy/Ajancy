using System;
using System.Linq;
using System.Web.UI;
using System.Data.Linq;

public partial class Union_TransferCarOwnerShip : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();
            this.drpCity.Enabled = Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager;
            LoadAjancies(true);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtCarVIN.Text))
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var driver = (from p in db.Persons
                          join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                          join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                          join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                          join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                          join c in db.Cars on cpn.CarID equals c.CarID
                          join ct in db.CarTypes on c.CarTypeID equals ct.CarTypeID
                          join fc in db.FuelCards on c.CarID equals fc.CarID
                          join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                          join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                          join u in db.Users on owp.PersonID equals u.PersonID
                          where c.VIN == this.txtCarVIN.Text.Trim()
                          orderby dcc.DriverCertificationCarID descending
                          select
                          new
                          {
                              OwnerNationalCode = owp.NationalCode,
                              OwnerName = string.Format("{0} {1}", owp.FirstName, owp.LastName),
                              DriverNationalCode = p.NationalCode,
                              DriverName = string.Format("{0} {1}", p.FirstName, p.LastName),
                              ct.TypeName,
                              pn.TwoDigits,
                              pn.ThreeDigits,
                              pn.Alphabet,
                              pn.RegionIdentifier,
                              fc.PAN,
                              c.CarID,
                              c.VIN,
                              u.ProvinceID,
                              u.CityID,
                              dcc.DriverCertificationCarID,
                              jd.AjancyDriverID
                          }).FirstOrDefault();

            if (driver != null)
            {
                if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager && driver.ProvinceID != Public.ActiveUserRole.User.ProvinceID) // Is ProvinceManager
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=27"); // This is a driver of an other province
                }
                else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager && driver.CityID != Public.ActiveUserRole.User.CityID)
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=26"); // This is a driver of an other city
                }

                this.lblTypeName.Text = driver.TypeName;
                this.lblPlateNumber.Text = Public.PlateNumberRenderToHTML(driver.TwoDigits, driver.Alphabet, driver.ThreeDigits, driver.RegionIdentifier);
                this.lblPAN.Text = driver.PAN;
                this.lblDriverNationalCode.Text = driver.DriverNationalCode;
                this.lblDriverName.Text = driver.DriverName;
                this.lblOwnerNationalCode.Text = driver.OwnerNationalCode;
                this.lblOwnerName.Text = driver.OwnerName;
                this.hdnValues.Value = string.Concat(driver.CarID, "|", driver.DriverCertificationCarID, "|", driver.AjancyDriverID);
                this.btnSave.Enabled = true;
            }
            else
            {
                this.lblTypeName.Text = null;
                this.lblPlateNumber.Text = null;
                this.lblPAN.Text = null;
                this.lblDriverNationalCode.Text = null;
                this.lblDriverName.Text = null;
                this.lblOwnerNationalCode.Text = null;
                this.lblOwnerName.Text = null;
                this.btnSave.Enabled = false;
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

    protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCity.SelectedIndex > 0)
        {
            LoadAjancies(true);
        }
        else
        {
            LoadAjancies(false);
        }
    }

    protected void txtNationalCode_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
        {
            this.ViewState["PersonID"] = null;
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var driver = from p in db.Persons
                         join u in db.Users on p.PersonID equals u.PersonID
                         where p.NationalCode == this.txtNationalCode.Text.Trim()
                         select
                         new
                         {
                             p.PersonID,
                             p.FirstName,
                             p.LastName,
                             u.ProvinceID,
                             u.CityID
                         };

            foreach (var item in driver)
            {
                if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager && item.ProvinceID != Public.ActiveUserRole.User.ProvinceID) // Is ProvinceManager
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=15"); // This is a driver of an other province
                }
                else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager && item.CityID != Public.ActiveUserRole.User.CityID)
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=13"); // This is a driver of an other city
                }

                this.ViewState["PersonID"] = item.PersonID;
                this.txtFirstName.Text = item.FirstName;
                this.txtLastName.Text = item.LastName;
            }
        }
        this.txtFirstName.Focus();
    }

    protected void txtOwnerNationalCode_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtOwnerNationalCode.Text))
        {
            this.txtOwnerName.Text = null;
            this.txtOwnerFamily.Text = null;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var person = db.Persons.Where<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode.Text).Select(p => new { p.FirstName, p.LastName });
            foreach (var item in person)
            {
                this.txtOwnerName.Text = item.FirstName;
                this.txtOwnerFamily.Text = item.LastName;
            }
        }
        this.txtOwnerName.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.Person>(p => p.User);
            dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
            dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
            dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
            dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;

            #region AjancyType

            Ajancy.Person person = new Ajancy.Person();
            Ajancy.DriverCertification driverCertification = new Ajancy.DriverCertification();
            Ajancy.DriverCertificationCar driverCertificationCar = new Ajancy.DriverCertificationCar();
            Ajancy.CarPlateNumber carPlateNumber = new Ajancy.CarPlateNumber();
            Ajancy.PlateNumber plateNumber = new Ajancy.PlateNumber();

            plateNumber = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim());
            if (plateNumber != null)
            {
                foreach (Ajancy.CarPlateNumber cpn in plateNumber.CarPlateNumbers)
                {
                    foreach (var dcc in cpn.DriverCertificationCars)
                    {
                        if (dcc.LockOutDate == null)
                        {
                            this.lblMessage.Text = "شماره پلاک وارد شده متعلق به شخص دیگری میباشد";
                            return;
                        }
                    }
                }
            }
            else
            {
                plateNumber = new Ajancy.PlateNumber();
                plateNumber.TwoDigits = this.txtCarPlateNumber_1.Text.Trim();
                plateNumber.Alphabet = this.drpCarPlateNumber.SelectedValue;
                plateNumber.ThreeDigits = this.txtCarPlateNumber_2.Text.Trim();
                plateNumber.RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim();
            }

            if (this.ViewState["PersonID"] == null) // New Person  
            {
                Ajancy.User user = new Ajancy.User();
                user.UserName = this.txtNationalCode.Text.Trim();
                user.ProvinceID = Public.ActiveUserRole.User.ProvinceID;
                user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                user.SubmitDate = DateTime.Now;
                user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                person.User = user;
                person.NationalCode = this.txtNationalCode.Text.Trim();
                person.SubmitDate = DateTime.Now;

                driverCertification = new Ajancy.DriverCertification { CertificationType = (byte)Public.AjancyType.TaxiAjancy, SubmitDate = DateTime.Now };
                person.DriverCertifications.Add(driverCertification);
                db.Persons.InsertOnSubmit(person);
            }
            else // Person Exists
            {
                person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));

                if (person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.TaxiDriver))  // Person is a driver
                {
                    driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                    Ajancy.DriverCertificationCar current_dcc = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>();
                    current_dcc.LockOutDate = DateTime.Now;
                    current_dcc.AjancyDrivers.Last<Ajancy.AjancyDriver>().LockOutDate = DateTime.Now;
                }
                else // Person is not driver
                {
                    person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                    driverCertification = new Ajancy.DriverCertification { CertificationType = (byte)Public.AjancyType.TaxiAjancy, SubmitDate = DateTime.Now };
                    person.DriverCertifications.Add(driverCertification);
                }
            }

            // --------------- setting values        

            string[] values = this.hdnValues.Value.Split('|');
            db.DriverCertificationCars.First<Ajancy.DriverCertificationCar>(dcc => dcc.DriverCertificationCarID == Public.ToInt(values[1])).LockOutDate = DateTime.Now;
            db.AjancyDrivers.First<Ajancy.AjancyDriver>(jd => jd.AjancyDriverID == Public.ToInt(values[2])).LockOutDate = DateTime.Now;

            carPlateNumber = new Ajancy.CarPlateNumber { CarID = Public.ToInt(values[0]), PlateNumber = plateNumber, Person = person };
            driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };
            driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), MembershipDate = DateTime.Now });
            driverCertification.DriverCertificationCars.Add(driverCertificationCar);

            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();

            // Sets the owner of the car 
            Ajancy.Person ownerPer = null;
            if (!string.IsNullOrEmpty(this.txtOwnerName.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerFamily.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerNationalCode.Text.Trim()) && !this.txtOwnerNationalCode.Text.Trim().Equals(person.NationalCode))
            {
                ownerPer = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode.Text.Trim());
                if (ownerPer == null)
                {
                    ownerPer = new Ajancy.Person { NationalCode = this.txtOwnerNationalCode.Text.Trim(), SubmitDate = DateTime.Now };
                    Ajancy.User ownerUser = new Ajancy.User
                    {
                        UserName = this.txtOwnerNationalCode.Text.Trim()
                        ,
                        ProvinceID = person.User.ProvinceID
                        ,
                        CityID = person.User.CityID
                        ,
                        SubmitDate = DateTime.Now
                    };
                    ownerUser.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                    ownerPer.User = ownerUser;
                    db.Persons.InsertOnSubmit(ownerPer);
                }
                else if (!ownerPer.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                {
                    ownerPer.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                }

                ownerPer.FirstName = this.txtOwnerName.Text.Trim();
                ownerPer.LastName = this.txtOwnerFamily.Text.Trim();
                person.CarPlateNumbers.Remove(carPlateNumber);
                ownerPer.CarPlateNumbers.Add(carPlateNumber);
            }
            else if (carPlateNumber.OwnerPersonID > 0 && carPlateNumber.OwnerPersonID != person.PersonID) // Set the driver as owner again
            {
                carPlateNumber.Person = person;
            }

            #endregion

            db.SubmitChanges();
            DisposeContext();
            Response.Redirect("~/Message.aspx?mode=25");
        }
    }

    private void LoadAjancies(bool loadItems)
    {
        this.drpAjancies.Items.Clear();

        if (loadItems)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var ajancyList = from j in db.Ajancies
                             join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == (byte)Public.AjancyType.TaxiAjancy &&
                                        ct.ProvinceID == Public.ActiveUserRole.User.ProvinceID &&
                                        ct.CityID == Public.ToShort(this.drpCity.SelectedValue) &&
                                        bl.LockOutDate == null
                             orderby j.AjancyName
                             select new { j.AjancyID, j.AjancyName };

            this.drpAjancies.DataSource = ajancyList;
            this.drpAjancies.DataBind();
        }
        this.drpAjancies.Items.Insert(0, "- انتخاب کنید -");
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}