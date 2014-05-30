using System;
using System.Linq;
using System.Web.UI;
using System.Data.Linq;

public partial class Union_Insurance : System.Web.UI.Page
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
            this.drpCarType.DataSource = db.CarTypes.OrderBy(ct => ct.TypeName);
            this.drpCarType.DataBind();
            this.drpCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));

            if (Request.QueryString["nc"] != null) // Edit mode
            {
                this.txtNationalCode.ReadOnly = true;
                this.txtNationalCode.Text = TamperProofString.QueryStringDecode(Request.QueryString["nc"]);
                this.txtNationalCode_TextChanged(sender, e);
            }
            else
            {
                LoadAjancies(true);
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
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.Person>(p => p.User);
            dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
            dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
            dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.PlateNumber);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
            dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;
            SetPerson(db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtNationalCode.Text.Trim()));
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
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
            dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;

            #region AjancyType

            Ajancy.Person person = new Ajancy.Person();
            Ajancy.DriverCertification driverCertification = new Ajancy.DriverCertification();
            Ajancy.DriverCertificationCar driverCertificationCar = new Ajancy.DriverCertificationCar();
            Ajancy.CarPlateNumber carPlateNumber = new Ajancy.CarPlateNumber();
            Ajancy.PlateNumber plateNumber = new Ajancy.PlateNumber();
            Ajancy.Car car = new Ajancy.Car();
            Ajancy.FuelCard fuelCard = new Ajancy.FuelCard();

            if (this.ViewState["PersonID"] == null) // New Person  
            {
                if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim()))
                {
                    this.lblMessage.Text = "خودرو مورد نظر با این شماره VIN متعلق به شخص دیگری میباشد";
                    return;
                }

                if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text))
                {
                    this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلا برای خودرو دیگری ثبت شده";
                    return;
                }

                Ajancy.PlateNumber pln = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                                                          pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                                                          pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                                                          pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim());
                if (pln != null)
                {
                    foreach (Ajancy.CarPlateNumber cpn in pln.CarPlateNumbers)
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
                carPlateNumber = new Ajancy.CarPlateNumber { Car = car, PlateNumber = plateNumber, Person = person };
                driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };

                driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), MembershipDate = DateTime.Now });
                driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                person.DriverCertifications.Add(driverCertification);
                fuelCard.SubmitDate = DateTime.Now;
                car.FuelCards.Add(fuelCard);
                db.Persons.InsertOnSubmit(person);
            }
            else // Person Exists
            {
                person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));

                if (person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.TaxiDriver))  // Person is a driver
                {
                    driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                    driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
                    carPlateNumber = driverCertificationCar.CarPlateNumber;
                    plateNumber = carPlateNumber.PlateNumber;
                    car = carPlateNumber.Car;
                    fuelCard = car.FuelCards.First<Ajancy.FuelCard>(fc => fc.DiscardDate == null);

                    Ajancy.AjancyDriver ajancyDriver = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null);
                    int ajancyId = Public.ToInt(this.drpAjancies.SelectedValue);
                    if (ajancyDriver.AjancyID != ajancyId) // Driver has moved to one other ajancy
                    {
                        ajancyDriver.LockOutDate = DateTime.Now;
                        driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = ajancyId, MembershipDate = DateTime.Now });
                    }

                    if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
                    {
                        person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    }

                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim() && c.CarID != car.CarID))
                    {
                        this.lblMessage.Text = "خودرو مورد نظر با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text && fc.FuelCardID != fuelCard.FuelCardID))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.PlateNumber pln = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                                                              pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                                                              pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                                                              pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim() &&
                                                                                                                                                              pl.PlateNumberID != carPlateNumber.PlateNumber.PlateNumberID);
                    if (pln != null)
                    {
                        foreach (Ajancy.CarPlateNumber cpn in pln.CarPlateNumbers)
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
                }
                else // Person is not driver
                {
                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim()))
                    {
                        this.lblMessage.Text = "خودرو مورد نظر با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.PlateNumber pln = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                                                              pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                                                              pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                                                              pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim());
                    if (pln != null)
                    {
                        foreach (Ajancy.CarPlateNumber cpn in pln.CarPlateNumbers)
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

                    person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                    driverCertification = new Ajancy.DriverCertification { CertificationType = (byte)Public.AjancyType.TaxiAjancy, SubmitDate = DateTime.Now };
                    carPlateNumber = new Ajancy.CarPlateNumber { Car = car, PlateNumber = plateNumber, Person = person };
                    driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };

                    driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), DriverCertificationCar = driverCertificationCar, MembershipDate = DateTime.Now });
                    driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                    person.DriverCertifications.Add(driverCertification);
                    fuelCard.SubmitDate = DateTime.Now;
                    car.FuelCards.Add(fuelCard);
                }
            }

            // --------------- setting values           

            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();
            person.Father = this.txtFather.Text.Trim();
            person.BirthCertificateNo = this.txtBirthCertificateNo.Text.Trim();
            person.Gender = Public.ToByte(this.drpGender.SelectedValue);
            person.BirthDate = this.txtBirthDate.GeorgianDate.Value;

            plateNumber.TwoDigits = this.txtCarPlateNumber_1.Text.Trim();
            plateNumber.Alphabet = this.drpCarPlateNumber.SelectedValue;
            plateNumber.ThreeDigits = this.txtCarPlateNumber_2.Text.Trim();
            plateNumber.RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim();

            car.CarTypeID = Public.ToShort(this.drpCarType.SelectedValue);
            car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);

            fuelCard.CardType = Public.ToByte(this.drpFuelCardType.SelectedValue);
            fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();

            car.VIN = this.txtCarVIN.Text.Trim().ToUpper();

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

            if (driverCertification.Insurance == null) // Add mode
            {
                driverCertification.Insurance = new Ajancy.Insurance
                {
                    InsuranceNo = string.IsNullOrEmpty(this.txtInsuranceNo.Text) ? null : this.txtInsuranceNo.Text.Trim()
                                   ,
                    BranchNo = string.IsNullOrEmpty(this.txtBranchNo.Text) ? null : this.txtBranchNo.Text.Trim()
                                   ,
                    CancelInsurance = Request.QueryString["m"].Equals("1")
                                   ,
                    SubmitDate = DateTime.Now
                };
            }
            else // Edit mode
            {
                driverCertification.Insurance.InsuranceNo = string.IsNullOrEmpty(this.txtInsuranceNo.Text) ? null : this.txtInsuranceNo.Text.Trim();
                driverCertification.Insurance.BranchNo = string.IsNullOrEmpty(this.txtBranchNo.Text) ? null : this.txtBranchNo.Text.Trim();
            }

            try
            {
                db.SubmitChanges();
                DisposeContext();
                Response.Redirect(string.Concat("~/Message.aspx?mode=2", Request.QueryString["m"]));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("InsuranceNo"))
                {
                    this.lblMessage.Text = "شماره بیمه وارد شده متعلق به شخص دیگری میباشد";
                }
            }
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person == null)
        {
            this.ViewState["PersonID"] = null;
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            this.txtFather.Text = null;
            this.txtBirthCertificateNo.Text = null;
            this.drpGender.SelectedIndex = 0;
            this.txtBirthDate.Text = null;
            SetCarOwner(null);
            SetCar(null);
            SetFuelCard(null);
            SetInsurance(null);
        }
        else
        {
            this.ViewState["PersonID"] = person.PersonID;
            this.txtFirstName.Text = person.FirstName;
            this.txtLastName.Text = person.LastName;
            this.txtNationalCode.Text = person.NationalCode;
            this.txtFather.Text = person.Father;
            this.txtBirthCertificateNo.Text = person.BirthCertificateNo;
            this.drpGender.SelectedValue = person.Gender.GetValueOrDefault().ToString();
            this.txtBirthDate.SetDate(person.BirthDate);

            if (person.DriverCertifications.Count > 0)
            {
                Ajancy.DriverCertification driverCertification = person.DriverCertifications.SingleOrDefault<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                if (driverCertification != null)
                {
                    if (Request.QueryString["nc"] == null) // Add mode
                    {
                        if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager) // Is ProvinceManager
                        {
                            if (person.User.ProvinceID != Public.ActiveUserRole.User.ProvinceID)
                            {
                                DisposeContext();
                                Response.Redirect("~/Message.aspx?mode=15"); // This is a driver of an other province
                            }
                            else if (person.User.CityID.Value.ToString() != this.drpCity.SelectedValue)
                            {
                                this.drpCity.SelectedValue = person.User.CityID.Value.ToString();
                                LoadAjancies(true);
                            }
                        }
                        else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager && person.User.CityID != Public.ActiveUserRole.User.CityID)
                        {
                            DisposeContext();
                            Response.Redirect("~/Message.aspx?mode=13"); // This is a driver of an other city
                        }
                    }
                    else // Edit mode
                    {
                        if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager) // Is ProvinceManager
                        {
                            this.drpCity.SelectedValue = person.User.CityID.Value.ToString();
                        }
                        LoadAjancies(true);
                    }

                    Ajancy.DriverCertificationCar driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
                    try
                    {
                        this.drpAjancies.SelectedValue = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).AjancyID.ToString();
                    }
                    catch { }
                    SetCarOwner(driverCertificationCar.CarPlateNumber.Person);
                    SetCar(driverCertificationCar.CarPlateNumber.Car);
                    SetFuelCard(driverCertificationCar.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());
                    SetInsurance(driverCertification.Insurance);
                }
            }
        }
    }

    private void SetCarOwner(Ajancy.Person carOwner)
    {
        if (carOwner == null)
        {
            this.txtOwnerNationalCode.Text = null;
            this.txtOwnerName.Text = null;
            this.txtOwnerFamily.Text = null;
        }
        else
        {
            this.txtOwnerNationalCode.Text = carOwner.NationalCode;
            this.txtOwnerName.Text = carOwner.FirstName;
            this.txtOwnerFamily.Text = carOwner.LastName;
        }
    }

    private void SetCar(Ajancy.Car car)
    {
        if (car == null)
        {
            this.txtCarVIN.Text = null;
            this.drpCarType.SelectedIndex = 0;
            this.drpFuelType.SelectedIndex = 0;
            SetPlateNumber(null);
        }
        else
        {
            this.txtCarVIN.Text = car.VIN;
            this.drpCarType.SelectedValue = car.CarTypeID.ToString();
            this.drpFuelType.SelectedValue = car.FuelType.ToString();
            SetPlateNumber(car.CarPlateNumbers.Last<Ajancy.CarPlateNumber>().PlateNumber);
        }
    }

    private void SetPlateNumber(Ajancy.PlateNumber plateNumber)
    {
        if (plateNumber != null)
        {
            this.txtCarPlateNumber_1.Text = plateNumber.TwoDigits;
            this.drpCarPlateNumber.SelectedValue = plateNumber.Alphabet;
            this.txtCarPlateNumber_2.Text = plateNumber.ThreeDigits;
            this.txtCarPlateNumber_3.Text = plateNumber.RegionIdentifier;
        }
        else
        {
            this.txtCarPlateNumber_1.Text = null;
            this.drpCarPlateNumber.SelectedIndex = 0;
            this.txtCarPlateNumber_2.Text = null;
            this.txtCarPlateNumber_3.Text = null;
        }
    }

    private void SetFuelCard(Ajancy.FuelCard fuelCard)
    {
        if (fuelCard == null)
        {
            this.txtFuelCardPAN.Text = null;
            this.drpFuelCardType.SelectedIndex = 0;
        }
        else
        {
            this.txtFuelCardPAN.Text = fuelCard.PAN;
            this.drpFuelCardType.SelectedValue = fuelCard.CardType.ToString();
        }
    }

    private void SetInsurance(Ajancy.Insurance Insurance)
    {
        if (Insurance == null)
        {
            this.txtInsuranceNo.Text = null;
            this.txtBranchNo.Text = null;
        }
        else
        {
            this.txtInsuranceNo.Text = Insurance.InsuranceNo;
            this.txtBranchNo.Text = Insurance.BranchNo;
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