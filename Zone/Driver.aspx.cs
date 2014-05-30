using System;
using System.Linq;
using System.Data.Linq;

public partial class Zone_Driver : System.Web.UI.Page
{
    private Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
    private DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int personId = 0;
            if (Request.QueryString["id"] != null &&
                int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId))
            {
                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.DrivingLicenses);
                dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
                dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
                dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
                dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.PlateNumber);
                dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
                dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
                dlo.LoadWith<Ajancy.Car>(c => c.CarType);
                db.LoadOptions = dlo;
                SetPerson(db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == personId));
                this.txtNationalCode.ReadOnly = true;
            }

            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();
            this.drpCity.Enabled = Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager;
            this.drpCarType.DataSource = db.CarTypes.OrderBy(ct => ct.TypeName);
            this.drpCarType.DataBind();
            this.drpCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
            LoadAjancies(true);
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
            this.drpCity.DataSource =
                db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue))
                    .Select(c => new { c.CityID, c.Name });
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
            dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
            db.LoadOptions = dlo;
            Ajancy.Person person =
                db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtNationalCode.Text.Trim());
            if (person != null)
            {
                if (
                    person.DriverCertifications.Any<Ajancy.DriverCertification>(
                        dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy))
                {
                    // This person already has DriverCertification
                    DisposeContext();
                    Response.Redirect(
                        string.Format("../Union/Driver.aspx?id={0}",
                            TamperProofString.QueryStringEncode(person.PersonID.ToString())), true);
                }
            }
            SetPerson(person);
        }
        this.txtFirstName.Focus();
    }

    protected void txtOwnerNationalCode_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtOwnerNationalCode.Text))
        {
            this.txtOwnerName.Text = null;
            this.txtOwnerFamily.Text = null;

            var person =
                db.Persons.Where<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode.Text)
                    .Select(p => new { p.FirstName, p.LastName });
            foreach (var item in person)
            {
                this.txtOwnerName.Text = item.FirstName;
                this.txtOwnerFamily.Text = item.LastName;
            }
        }
        this.txtOwnerName.Focus();
    }

    protected void drpCarPlateNumberProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCarPlateNumberProvince.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCarPlateNumberCity.DataSource =
                db.Cities.Where<Ajancy.City>(
                    c => c.ProvinceID == Public.ToByte(this.drpCarPlateNumberProvince.SelectedValue))
                    .Select(c => new { c.CityID, c.Name });
            this.drpCarPlateNumberCity.DataBind();
        }
        else
        {
            this.drpCarPlateNumberCity.Items.Clear();
            this.drpCarPlateNumberCity.Items.Insert(0, "- انتخاب کنید -");
        }
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
            db.LoadOptions = dlo;

            #region AjancyType

            Ajancy.Person person = new Ajancy.Person();
            Ajancy.DriverCertification driverCertification = new Ajancy.DriverCertification();
            Ajancy.DriverCertificationCar driverCertificationCar = new Ajancy.DriverCertificationCar();
            Ajancy.CarPlateNumber carPlateNumber = new Ajancy.CarPlateNumber();
            Ajancy.ZonePlateNumber plateNumber = new Ajancy.ZonePlateNumber();
            Ajancy.Car car = new Ajancy.Car();
            Ajancy.FuelCard fuelCard = new Ajancy.FuelCard();
            Ajancy.Car car_db = null;

            if (this.ViewState["PersonID"] == null) // New Person  
            {
                car_db = db.Cars.FirstOrDefault<Ajancy.Car>(c => c.EngineNo == this.txtCarEngineNo.Text.Trim() &&
                                                                 c.ChassisNo == this.txtCarChassisNo.Text.Trim());
                if (car_db != null)
                {
                    foreach (var cpn in car_db.CarPlateNumbers)
                    {
                        foreach (var dcc in cpn.DriverCertificationCars)
                        {
                            if (dcc.LockOutDate == null)
                            {
                                this.lblMessage.Text =
                                    "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
                                return;
                            }
                        }
                    }
                    car = car_db;
                }
                else
                {
                    db.Cars.InsertOnSubmit(car);
                }

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

                Ajancy.ZonePlateNumber pln =
                    db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(
                        pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                              pl.Number == this.txtCarPlateNumber_5.Text.Trim());
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
                user.CityID = Public.ActiveUserRole.User.CityID;
                user.SubmitDate = DateTime.Now;
                user.UsersInRoles.Add(new Ajancy.UsersInRole
                {
                    RoleID = (short)Public.Role.TaxiDriver,
                    MembershipDate = DateTime.Now
                });
                person.User = user;
                person.NationalCode = this.txtNationalCode.Text.Trim();
                person.SubmitDate = DateTime.Now;

                driverCertification.CertificationType = (byte)Public.AjancyType.TaxiAjancy;
                driverCertification.SubmitDate = DateTime.Now;

                carPlateNumber = new Ajancy.CarPlateNumber { Car = car, ZonePlateNumber = plateNumber, Person = person };
                driverCertificationCar = new Ajancy.DriverCertificationCar
                {
                    CarPlateNumber = carPlateNumber,
                    SubmitDate = DateTime.Now
                };
                driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                person.DriverCertifications.Add(driverCertification);
                fuelCard.SubmitDate = DateTime.Now;
                car.FuelCards.Add(fuelCard);
                db.CarPlateNumbers.InsertOnSubmit(carPlateNumber);
                db.AjancyDrivers.InsertOnSubmit(new Ajancy.AjancyDriver
                {
                    AjancyID = Public.ToInt(this.drpAjancies.SelectedValue),
                    DriverCertificationCar = driverCertificationCar,
                    MembershipDate = DateTime.Now
                });
                db.Persons.InsertOnSubmit(person);
            }
            else // Person Exists
            {
                person =
                    db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));

                if (person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.TaxiDriver))
                // Person is a driver
                {
                    driverCertification =
                        person.DriverCertifications.First<Ajancy.DriverCertification>(
                            dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                    driverCertificationCar =
                        driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>();
                    carPlateNumber = driverCertificationCar.CarPlateNumber;
                    plateNumber = carPlateNumber.ZonePlateNumber;
                    car = carPlateNumber.Car;
                    fuelCard = car.FuelCards.Last<Ajancy.FuelCard>();
                    //fuelCard = car.FuelCards.First<Ajancy.FuelCard>(fc => fc.DiscardDate == null);

                    car_db = db.Cars.FirstOrDefault<Ajancy.Car>(c => c.EngineNo == this.txtCarEngineNo.Text.Trim() &&
                                                                     c.ChassisNo == this.txtCarChassisNo.Text.Trim() &&
                                                                     c.CarID != carPlateNumber.Car.CarID);
                    if (car_db != null)
                    {
                        foreach (var cpn in car_db.CarPlateNumbers)
                        {
                            foreach (var dcc in cpn.DriverCertificationCars)
                            {
                                if (dcc.LockOutDate == null)
                                {
                                    this.lblMessage.Text =
                                        "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
                                    return;
                                }
                            }
                        }
                        carPlateNumber.Car = car_db;
                    }

                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim() && c.CarID != car.CarID))
                    {
                        this.lblMessage.Text = "خودرو مورد نظر با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (
                        db.FuelCards.Any<Ajancy.FuelCard>(
                            fc => fc.PAN == this.txtFuelCardPAN.Text && fc.FuelCardID != fuelCard.FuelCardID))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.ZonePlateNumber pln =
                        db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(
                            pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                                  pl.Number == this.txtCarPlateNumber_5.Text.Trim() &&
                                  pl.ZonePlateNumberID != carPlateNumber.ZonePlateNumberID);
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

                    //Ajancy.AjancyDriver ajancyDriver = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null);
                    Ajancy.AjancyDriver ajancyDriver = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>();
                    int ajancyId = Public.ToInt(this.drpAjancies.SelectedValue);
                    if (ajancyDriver.AjancyID != ajancyId) // Driver has moved to one other ajancy
                    {
                        ajancyDriver.LockOutDate = DateTime.Now;
                        driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver
                        {
                            AjancyID = ajancyId,
                            MembershipDate = DateTime.Now
                        });
                    }

                    if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
                    {
                        person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    }
                }
                else // Person is not driver
                {
                    car_db = db.Cars.FirstOrDefault<Ajancy.Car>(c => c.EngineNo == this.txtCarEngineNo.Text.Trim() &&
                                                                     c.ChassisNo == this.txtCarChassisNo.Text.Trim());
                    if (car_db != null)
                    {
                        foreach (var cpn in car_db.CarPlateNumbers)
                        {
                            foreach (var dcc in cpn.DriverCertificationCars)
                            {
                                if (dcc.LockOutDate == null)
                                {
                                    this.lblMessage.Text =
                                        "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
                                    return;
                                }
                            }
                        }
                        car = car_db;
                    }
                    else
                    {
                        db.Cars.InsertOnSubmit(car);
                    }

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

                    Ajancy.ZonePlateNumber pln =
                        db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(
                            pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                                  pl.Number == this.txtCarPlateNumber_5.Text.Trim());
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

                    person.User.UsersInRoles.Add(new Ajancy.UsersInRole
                    {
                        RoleID = (short)Public.Role.TaxiDriver,
                        MembershipDate = DateTime.Now
                    });
                    driverCertification.CertificationType = (byte)Public.AjancyType.TaxiAjancy;
                    driverCertification.SubmitDate = DateTime.Now;
                    carPlateNumber = new Ajancy.CarPlateNumber
                    {
                        Car = car,
                        ZonePlateNumber = plateNumber,
                        Person = person
                    };
                    driverCertificationCar = new Ajancy.DriverCertificationCar
                    {
                        CarPlateNumber = carPlateNumber,
                        SubmitDate = DateTime.Now
                    };
                    driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                    person.DriverCertifications.Add(driverCertification);
                    fuelCard.SubmitDate = DateTime.Now;
                    car.FuelCards.Add(fuelCard);
                    db.CarPlateNumbers.InsertOnSubmit(carPlateNumber);
                    db.AjancyDrivers.InsertOnSubmit(new Ajancy.AjancyDriver
                    {
                        AjancyID = Public.ToInt(this.drpAjancies.SelectedValue),
                        DriverCertificationCar = driverCertificationCar,
                        MembershipDate = DateTime.Now
                    });
                }
            }

            // --------- setting other values  

            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();
            person.Father = this.txtFather.Text.Trim();
            person.BirthCertificateNo = this.txtBirthCertificateNo.Text.Trim();
            person.Gender = Public.ToByte(this.drpGender.SelectedValue);
            person.Marriage = Public.ToByte(this.drpMarriage.SelectedValue);
            person.BirthPlace = this.txtBirthPlace.Text.Trim();
            person.Phone = this.txtPhone.Text.Trim();
            person.Mobile = this.txtMobile.Text.Trim();
            person.PostalCode = this.txtPostalCode.Text.Trim();
            person.Address = this.txtAddress.Text.Trim();
            plateNumber.CityID = Public.ToShort(this.drpCarPlateNumberCity.SelectedValue);
            plateNumber.Number = this.txtCarPlateNumber_5.Text.Trim();

            if (!string.IsNullOrEmpty(this.txtDriverCertificationNo.Text))
            {
                if (person.PersonID == 0)
                {
                    if (
                        db.DriverCertifications.Any<Ajancy.DriverCertification>(
                            dc => dc.DriverCertificationNo == this.txtDriverCertificationNo.Text))
                    {
                        this.lblMessage.Text = "شماره دفترچه صلاحیت تکراری میباشد";
                        return;
                    }
                }
                else
                {
                    if (
                        db.DriverCertifications.Any<Ajancy.DriverCertification>(
                            dc =>
                                dc.PersonID != person.PersonID &&
                                dc.DriverCertificationNo == this.txtDriverCertificationNo.Text))
                    {
                        this.lblMessage.Text = "شماره دفترچه صلاحیت تکراری میباشد";
                        return;
                    }
                }
            }
            driverCertification.DriverCertificationNo = string.IsNullOrEmpty(this.txtDriverCertificationNo.Text)
                ? null
                : this.txtDriverCertificationNo.Text.Trim();

            car.CarTypeID = Public.ToByte(this.drpCarType.SelectedValue);
            car.Model = this.txtCarModel.Text.Trim();
            car.EngineNo = this.txtCarEngineNo.Text.Trim();
            car.ChassisNo = this.txtCarChassisNo.Text.Trim();
            car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);

            fuelCard.CardType = Public.ToByte(this.drpFuelCardType.SelectedValue);
            fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();

            car.VIN = this.txtCarVIN.Text.Trim().ToUpper();

            // Sets the owner of the car 
            Ajancy.Person ownerPer = null;
            if (!string.IsNullOrEmpty(this.txtOwnerName.Text.Trim()) &&
                !string.IsNullOrEmpty(this.txtOwnerFamily.Text.Trim()) &&
                !string.IsNullOrEmpty(this.txtOwnerNationalCode.Text.Trim()) &&
                !this.txtOwnerNationalCode.Text.Trim().Equals(person.NationalCode))
            {
                ownerPer =
                    db.Persons.FirstOrDefault<Ajancy.Person>(
                        p => p.NationalCode == this.txtOwnerNationalCode.Text.Trim());
                if (ownerPer == null)
                {
                    ownerPer = new Ajancy.Person
                    {
                        NationalCode = this.txtOwnerNationalCode.Text.Trim(),
                        SubmitDate = DateTime.Now
                    };
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
                    ownerUser.UsersInRoles.Add(new Ajancy.UsersInRole
                    {
                        RoleID = (short)Public.Role.CarOwner,
                        MembershipDate = DateTime.Now,
                        LockOutDate = DateTime.Now
                    });
                    ownerPer.User = ownerUser;
                    db.Persons.InsertOnSubmit(ownerPer);
                }
                else if (
                    !ownerPer.User.UsersInRoles.Any<Ajancy.UsersInRole>(
                        ur => ur.RoleID == (short)Public.Role.CarOwner))
                {
                    ownerPer.User.UsersInRoles.Add(new Ajancy.UsersInRole
                    {
                        RoleID = (short)Public.Role.CarOwner,
                        MembershipDate = DateTime.Now,
                        LockOutDate = DateTime.Now
                    });
                }

                ownerPer.FirstName = this.txtOwnerName.Text.Trim();
                ownerPer.LastName = this.txtOwnerFamily.Text.Trim();
                person.CarPlateNumbers.Remove(carPlateNumber);
                ownerPer.CarPlateNumbers.Add(carPlateNumber);
            }
            else if (carPlateNumber.OwnerPersonID > 0 && carPlateNumber.OwnerPersonID != person.PersonID)
            // Set the driver as owner again
            {
                carPlateNumber.Person = person;
            }

            #endregion

            db.SubmitChanges();
            DisposeContext();
            Response.Redirect("~/Message.aspx?mode=12");
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
            this.txtBirthPlace.Text = null;
            this.drpMarriage.SelectedIndex = 0;
            this.txtPhone.Text = null;
            this.txtMobile.Text = null;
            this.txtPostalCode.Text = null;
            this.txtAddress.Text = null;
            SetCarOwner(null);
            SetCar(null);
            SetFuelCard(null);
            //SetFormerCar(null);
        }
        else
        {
            if (person.User.ProvinceID != Public.ActiveUserRole.User.ProvinceID) // This is driver of another province
            {
                DisposeContext();
                Response.Redirect("~/Message.aspx?mode=15");
            }
            this.ViewState["PersonID"] = person.PersonID;
            this.txtFirstName.Text = person.FirstName;
            this.txtLastName.Text = person.LastName;
            this.txtFather.Text = person.Father;
            this.txtNationalCode.Text = person.NationalCode;
            this.txtBirthCertificateNo.Text = person.BirthCertificateNo;
            this.drpGender.SelectedValue = person.Gender.GetValueOrDefault().ToString();
            this.txtBirthPlace.Text = person.BirthPlace;
            this.drpMarriage.SelectedValue = person.Marriage.GetValueOrDefault().ToString();
            this.txtPhone.Text = person.Phone;
            this.txtMobile.Text = person.Mobile;
            this.txtPostalCode.Text = person.PostalCode;
            this.txtAddress.Text = person.Address;
            if (person.DriverCertifications.Count > 0)
            {
                Ajancy.DriverCertification driverCertification =
                    person.DriverCertifications.SingleOrDefault<Ajancy.DriverCertification>(
                        dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                Ajancy.DriverCertificationCar driverCertificationCar =
                    driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>();
                SetCarOwner(driverCertificationCar.CarPlateNumber.Person);
                SetCar(driverCertificationCar.CarPlateNumber.Car);
                SetFuelCard(driverCertificationCar.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());

                string ajancyId = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>().AjancyID.ToString();
                if (this.drpAjancies.Items.FindByValue(ajancyId) != null)
                {
                    this.drpAjancies.SelectedValue = ajancyId;
                }
                //this.drpAjancies.SelectedValue = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).AjancyID.ToString();
                this.txtDriverCertificationNo.Text = driverCertification.DriverCertificationNo;
                this.drpDriverCertificationNo.SelectedIndex =
                    string.IsNullOrEmpty(driverCertification.DriverCertificationNo) ? 1 : 0;
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
            this.drpCarType.SelectedIndex = 0;
            this.txtCarVIN.Text = null;
            this.txtCarModel.Text = null;
            this.txtCarEngineNo.Text = null;
            this.txtCarChassisNo.Text = null;
            this.drpFuelType.SelectedIndex = 0;
            SetPlateNumber(null);
        }
        else
        {
            this.drpCarType.SelectedValue = car.CarTypeID.ToString();
            this.txtCarVIN.Text = car.VIN;
            this.txtCarModel.Text = car.Model;
            this.txtCarEngineNo.Text = car.EngineNo;
            this.txtCarChassisNo.Text = car.ChassisNo;
            this.drpFuelType.SelectedValue = car.FuelType.ToString();
            SetPlateNumber(car.CarPlateNumbers.Last<Ajancy.CarPlateNumber>().ZonePlateNumber);
        }
    }

    private void SetPlateNumber(Ajancy.ZonePlateNumber plateNumber)
    {
        if (plateNumber != null)
        {
            this.txtCarPlateNumber_5.Text = plateNumber.Number;
            this.drpCarPlateNumberProvince.SelectedValue = plateNumber.City.ProvinceID.ToString();
            this.drpCarPlateNumberProvince_SelectedIndexChanged(this, new EventArgs());
            this.drpCarPlateNumberCity.SelectedValue = plateNumber.CityID.ToString();
        }
        else
        {
            this.txtCarPlateNumber_5.Text = null;
            this.drpCarPlateNumberProvince.SelectedIndex = 0;
            this.drpCarPlateNumberCity.SelectedIndex = 0;
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

    private void LoadAjancies(bool loadItems)
    {
        this.drpAjancies.Items.Clear();

        if (loadItems)
        {

            var ajancyList = from j in db.Ajancies
                             join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == (byte)Public.AjancyType.TaxiAjancy &&
                                   ct.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue) &&
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
