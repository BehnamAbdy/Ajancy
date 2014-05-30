using System;
using System.Linq;
using System.Web.UI;
using System.Data.Linq;

public partial class Zone_FCReplacement : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            switch ((Public.Role)Public.ActiveUserRole.RoleID)
            {
                case Public.Role.CityManager:
                    this.drpCity.Enabled = false;
                    this.drpCity_2.Enabled = false;
                    break;

                case Public.Role.Admin:
                    this.drpProvince.Enabled = true;
                    this.drpProvince_2.Enabled = true;
                    break;
            }

            if (Public.ActiveUserRole.RoleID != (short)Public.Role.Admin)
            {
                Ajancy.User user = Public.ActiveUserRole.User;
                this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                this.drpProvince_2.SelectedIndex = this.drpProvince.SelectedIndex;
                this.drpProvince_SelectedIndexChanged(sender, e);
                this.drpCity.SelectedValue = user.CityID.ToString();
                this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;

                this.drpCarPlateNumberProvince.SelectedValue = user.ProvinceID.ToString();
                this.drpCarPlateNumberProvince_SelectedIndexChanged(sender, e);
                this.drpCarPlateNumberCity.SelectedValue = user.CityID.ToString();
                this.drpCarPlateNumberProvince_2.SelectedValue = user.ProvinceID.ToString();
                this.drpCarPlateNumberProvince_2_SelectedIndexChanged(sender, e);
                this.drpCarPlateNumberCity_2.SelectedValue = user.CityID.ToString();
            }
            else
            {
                this.drpProvince.SelectedIndex = 0;
                this.drpProvince_2.SelectedIndex = 0;
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            }

            System.Collections.Generic.List<Ajancy.CarType> carTypes = db.CarTypes.OrderBy(ct => ct.TypeName).ToList<Ajancy.CarType>();
            this.drpCarType.DataSource = carTypes;
            this.drpCarType.DataBind();
            this.drpCarType_2.DataSource = carTypes;
            this.drpCarType_2.DataBind();
            this.drpCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
            this.drpCarType_2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));

            if (Request.QueryString["nc1"] != null && Request.QueryString["nc2"] != null) // Edit mode
            {
                this.txtNationalCode.ReadOnly = true;
                this.txtNationalCode_2.ReadOnly = true;
                this.txtNationalCode.Text = TamperProofString.QueryStringDecode(Request.QueryString["nc1"]);
                this.txtNationalCode_2.Text = TamperProofString.QueryStringDecode(Request.QueryString["nc2"]);
                this.ViewState["Mode"] = "EDT";
                this.txtNationalCode_TextChanged(sender, e);
                this.txtNationalCode_2_TextChanged(sender, e);
            }
            else
            {
                //this.drpCity.SelectedValue = user.CityID.ToString();
                //this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
                LoadAjancies(true);
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (this.txtCarVIN.Text.ToUpper().Trim().Equals(this.txtCarVIN_2.Text.ToUpper().Trim()))
            {
                this.lblMessage.Text = "VIN خودرو بخش آژانسی با VIN خودرو بخش  شخصی نباید یکسان باشد";
                return;
            }

            if (this.txtFuelCardPAN.Text.ToUpper().Trim().Equals(this.txtFuelCardPAN_2.Text.ToUpper().Trim()))
            {
                this.lblMessage.Text = "PAN کارت سوخت بخش آژانسی با PAN کارت سوخت بخش  شخصی نباید یکسان باشد";
                return;
            }

            if (this.txtNationalCode.Text.Trim() == this.txtNationalCode_2.Text.Trim() && this.ViewState["Mode"] == null)
            {
                SelfReplacement_Save();
            }

            if (this.txtCarPlateNumber_5.Text.Trim().Equals(this.txtCarPlateNumber_5_2.Text.Trim()) &&
                this.drpCarPlateNumberCity.SelectedValue.Equals(this.drpCarPlateNumberCity_2.SelectedValue))
            {
                this.lblMessage.Text = "پلاک خودرو بخش آژانسی با پلاک خودرو بخش شخصی نباید یکسان باشد";
                return;
            }

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
            Ajancy.ZonePlateNumber plateNumber = new Ajancy.ZonePlateNumber();
            Ajancy.Car car = new Ajancy.Car();
            Ajancy.FuelCard fuelCard = new Ajancy.FuelCard();

            if (this.ViewState["PersonID"] == null) // New Person  
            {
                if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim()))
                {
                    this.lblMessage.Text = "خودرو بخش کارت سوخت آژانسی با این شماره VIN متعلق به شخص دیگری میباشد";
                    return;
                }

                if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text))
                {
                    this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت آژانسی قبلا برای خودرو دیگری ثبت شده";
                    return;
                }

                Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                                                                                                              pl.Number == this.txtCarPlateNumber_5.Text.Trim());
                if (pln != null)
                {
                    this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت آژانسی متعلق به شخص دیگری میباشد";
                    return;
                }

                Ajancy.User user = new Ajancy.User();
                user.UserName = this.txtNationalCode.Text.Trim();
                user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                user.CityID = Public.ToShort(this.drpCity.SelectedValue);
                user.SubmitDate = DateTime.Now;
                user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                person.User = user;
                person.NationalCode = this.txtNationalCode.Text.Trim();
                person.SubmitDate = DateTime.Now;

                driverCertification = new Ajancy.DriverCertification
                {
                    CertificationType = (byte)Public.AjancyType.TaxiAjancy,
                    SubmitDate = DateTime.Now
                };
                carPlateNumber = new Ajancy.CarPlateNumber { Car = car, ZonePlateNumber = plateNumber };
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
                    plateNumber = carPlateNumber.ZonePlateNumber;
                    car = carPlateNumber.Car;
                    fuelCard = car.FuelCards.Last<Ajancy.FuelCard>();
                    fuelCard.DiscardDate = null;

                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim() && c.CarID != car.CarID))
                    {
                        this.lblMessage.Text = "خودرو بخش کارت سوخت آژانسی با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text && fc.FuelCardID != fuelCard.FuelCardID))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت آژانسی قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                                                                                                               pl.Number == this.txtCarPlateNumber_5.Text.Trim() &&
                                                                                                               pl.ZonePlateNumberID != carPlateNumber.ZonePlateNumberID);
                    if (pln != null)
                    {
                        this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت آژانسی متعلق به شخص دیگری میباشد";
                        return;
                    }

                    Ajancy.AjancyDriver ajancyDriver = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null);
                    int ajancyId = Public.ToInt(this.drpAjancies.SelectedValue);
                    if (ajancyDriver.AjancyID != ajancyId) // Driver has moved to one other ajancy
                    {
                        ajancyDriver.LockOutDate = DateTime.Now;
                        driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = ajancyId, MembershipDate = DateTime.Now });
                    }

                    //if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
                    //{
                    //    person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    //}
                }
                else // Person is not driver
                {
                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim()))
                    {
                        this.lblMessage.Text = "خودرو بخش کارت سوخت آژانسی با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت آژانسی قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                                                                                                                 pl.Number == this.txtCarPlateNumber_5.Text.Trim());
                    if (pln != null)
                    {
                        this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت آژانسی متعلق به شخص دیگری میباشد";
                        return;
                    }

                    person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                    driverCertification = new Ajancy.DriverCertification
                    {
                        CertificationType = (byte)Public.AjancyType.TaxiAjancy,
                        SubmitDate = DateTime.Now
                    };
                    carPlateNumber = new Ajancy.CarPlateNumber { Car = car, ZonePlateNumber = plateNumber };
                    driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };
                    driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), DriverCertificationCar = driverCertificationCar, MembershipDate = DateTime.Now });
                    driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                    person.DriverCertifications.Add(driverCertification);
                    fuelCard.SubmitDate = DateTime.Now;
                    car.FuelCards.Add(fuelCard);
                }

                switch ((Public.Role)Public.ActiveUserRole.RoleID)
                {
                    case Public.Role.ProvinceManager:
                        person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                        break;

                    case Public.Role.Admin:
                        person.User.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                        person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                        break;
                }
            }

            // --------------- setting values           

            person.FirstName = this.txtFirstName.Text.Trim();
            person.LastName = this.txtLastName.Text.Trim();

            plateNumber.CityID = Public.ToShort(this.drpCarPlateNumberCity.SelectedValue);
            plateNumber.Number = this.txtCarPlateNumber_5.Text.Trim();

            car.CarTypeID = Public.ToShort(this.drpCarType.SelectedValue);
            car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);
            car.Model = this.txtCarModel.Text;

            fuelCard.CardType = (byte)Public.FuelCardType.Ajancy;
            fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();

            car.VIN = this.txtCarVIN.Text.Trim().ToUpper();

            #endregion

            #region PersonalType

            Ajancy.Person person_2 = new Ajancy.Person();
            Ajancy.DriverCertification driverCertification_2 = new Ajancy.DriverCertification();
            Ajancy.DriverCertificationCar driverCertificationCar_2 = new Ajancy.DriverCertificationCar();
            Ajancy.CarPlateNumber carPlateNumber_2 = new Ajancy.CarPlateNumber();
            Ajancy.ZonePlateNumber plateNumber_2 = new Ajancy.ZonePlateNumber();
            Ajancy.Car car_2 = new Ajancy.Car();
            Ajancy.FuelCard fuelCard_2 = new Ajancy.FuelCard();

            if (this.ViewState["PersonID_2"] == null) // New Person  
            {
                if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN_2.Text.Trim()))
                {
                    this.lblMessage.Text = "خودرو بخش کارت سوخت شخصی با این شماره VIN متعلق به شخص دیگری میباشد";
                    return;
                }

                if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN_2.Text))
                {
                    this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت شخصی قبلا برای خودرو دیگری ثبت شده";
                    return;
                }

                Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity_2.SelectedValue) &&
                                                                                                              pl.Number == this.txtCarPlateNumber_5_2.Text.Trim());
                if (pln != null)
                {
                    this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت شخصی متعلق به شخص دیگری میباشد";
                    return;
                }

                Ajancy.User user = new Ajancy.User();
                user.UserName = this.txtNationalCode_2.Text.Trim();
                user.ProvinceID = Public.ToByte(this.drpProvince_2.SelectedValue);
                user.CityID = Public.ToShort(this.drpCity_2.SelectedValue);
                user.SubmitDate = DateTime.Now;
                user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                person_2.User = user;
                person_2.NationalCode = this.txtNationalCode_2.Text.Trim();
                person_2.SubmitDate = DateTime.Now;

                driverCertification_2 = new Ajancy.DriverCertification
                {
                    CertificationType = (byte)Public.AjancyType.TaxiAjancy,
                    SubmitDate = DateTime.Now
                };
                carPlateNumber_2 = new Ajancy.CarPlateNumber { Car = car_2, ZonePlateNumber = plateNumber_2 };
                driverCertificationCar_2 = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber_2, SubmitDate = DateTime.Now };
                driverCertificationCar_2.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies_2.SelectedValue), MembershipDate = DateTime.Now });
                driverCertification_2.DriverCertificationCars.Add(driverCertificationCar_2);
                person_2.DriverCertifications.Add(driverCertification_2);
                fuelCard_2.SubmitDate = DateTime.Now;
                car_2.FuelCards.Add(fuelCard_2);
                db.Persons.InsertOnSubmit(person_2);
            }
            else // Person Exists
            {
                person_2 = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID_2"]));
                if (person_2.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.TaxiDriver))  // Person is a driver
                {
                    driverCertification_2 = person_2.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                    driverCertificationCar_2 = driverCertification_2.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
                    carPlateNumber_2 = driverCertificationCar_2.CarPlateNumber;
                    plateNumber_2 = carPlateNumber_2.ZonePlateNumber;
                    car_2 = carPlateNumber_2.Car;
                    fuelCard_2 = car_2.FuelCards.Last<Ajancy.FuelCard>();
                    fuelCard_2.DiscardDate = null;

                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN_2.Text.Trim() && c.CarID != car_2.CarID))
                    {
                        this.lblMessage.Text = "خودرو بخش کارت سوخت شخصی با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN_2.Text && fc.FuelCardID != fuelCard_2.FuelCardID))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت شخصی قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity_2.SelectedValue) &&
                                                                                                                  pl.Number == this.txtCarPlateNumber_5_2.Text.Trim() &&
                                                                                                                  pl.ZonePlateNumberID != carPlateNumber_2.ZonePlateNumberID);
                    if (pln != null)
                    {
                        this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت شخصی متعلق به شخص دیگری میباشد";
                        return;
                    }

                    Ajancy.AjancyDriver ajancyDriver_2 = driverCertificationCar_2.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null);
                    int ajancyId_2 = Public.ToInt(this.drpAjancies_2.SelectedValue);
                    if (ajancyDriver_2.AjancyID != ajancyId_2) // Driver has moved to one other ajancy
                    {
                        ajancyDriver_2.LockOutDate = DateTime.Now;
                        driverCertificationCar_2.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = ajancyId_2, MembershipDate = DateTime.Now });
                    }

                    //if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
                    //{
                    //    person_2.User.CityID = Public.ToShort(this.drpCity_2.SelectedValue);
                    //}
                }
                else // Person is not driver
                {
                    if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN_2.Text.Trim()))
                    {
                        this.lblMessage.Text = "خودرو بخش کارت سوخت شخصی با این شماره VIN متعلق به شخص دیگری میباشد";
                        return;
                    }

                    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN_2.Text))
                    {
                        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت شخصی قبلا برای خودرو دیگری ثبت شده";
                        return;
                    }

                    Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity_2.SelectedValue) &&
                                                                                                                  pl.Number == this.txtCarPlateNumber_5_2.Text.Trim());
                    if (pln != null)
                    {
                        this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت شخصی متعلق به شخص دیگری میباشد";
                        return;
                    }

                    person_2.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                    driverCertification_2 = new Ajancy.DriverCertification
                    {
                        CertificationType = (byte)Public.AjancyType.TaxiAjancy,
                        SubmitDate = DateTime.Now
                    };
                    carPlateNumber_2 = new Ajancy.CarPlateNumber { Car = car_2, ZonePlateNumber = plateNumber_2 };
                    driverCertificationCar_2 = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber_2, SubmitDate = DateTime.Now };
                    driverCertificationCar_2.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies_2.SelectedValue), DriverCertificationCar = driverCertificationCar_2, MembershipDate = DateTime.Now });
                    driverCertification_2.DriverCertificationCars.Add(driverCertificationCar_2);
                    person_2.DriverCertifications.Add(driverCertification_2);
                    fuelCard_2.SubmitDate = DateTime.Now;
                    car_2.FuelCards.Add(fuelCard_2);
                }

                switch ((Public.Role)Public.ActiveUserRole.RoleID)
                {
                    case Public.Role.ProvinceManager:
                        person_2.User.CityID = Public.ToShort(this.drpCity_2.SelectedValue);
                        break;

                    case Public.Role.Admin:
                        person_2.User.ProvinceID = Public.ToByte(this.drpProvince_2.SelectedValue);
                        person_2.User.CityID = Public.ToShort(this.drpCity_2.SelectedValue);
                        break;
                }
            }

            // --------------- setting values           

            person_2.FirstName = this.txtFirstName_2.Text.Trim();
            person_2.LastName = this.txtLastName_2.Text.Trim();

            plateNumber_2.CityID = Public.ToShort(this.drpCarPlateNumberCity_2.SelectedValue);
            plateNumber_2.Number = this.txtCarPlateNumber_5_2.Text.Trim();

            car_2.CarTypeID = Public.ToShort(this.drpCarType_2.SelectedValue);
            car_2.FuelType = Public.ToByte(this.drpFuelType_2.SelectedValue);
            car_2.Model = this.txtCarModel_2.Text;

            fuelCard_2.CardType = (byte)Public.FuelCardType.Ajancy;
            fuelCard_2.PAN = this.txtFuelCardPAN_2.Text.Trim();

            car_2.VIN = this.txtCarVIN_2.Text.Trim().ToUpper();

            #endregion

            #region Owners

            bool ajancyTypeOwner = false;
            bool personalTypeOwner = false;
            Ajancy.Person ownerPer = null;
            Ajancy.Person ownerPer_2 = null;

            if (!string.IsNullOrEmpty(this.txtOwnerName.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerFamily.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerNationalCode.Text.Trim()) && !this.txtOwnerNationalCode.Text.Trim().Equals(person.NationalCode))
            {
                ajancyTypeOwner = true;
            }
            if (!string.IsNullOrEmpty(this.txtOwnerName_2.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerFamily_2.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerNationalCode_2.Text.Trim()) && !this.txtOwnerNationalCode_2.Text.Trim().Equals(person_2.NationalCode))
            {
                personalTypeOwner = true;
            }

            if (ajancyTypeOwner && personalTypeOwner && this.txtOwnerNationalCode_2.Text == this.txtOwnerNationalCode.Text) // Both owners are the same person
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
                ownerPer.CarPlateNumbers.Add(carPlateNumber);
                ownerPer.CarPlateNumbers.Add(carPlateNumber_2);
            }
            else
            {
                if (personalTypeOwner && this.txtNationalCode.Text == this.txtOwnerNationalCode_2.Text) // Cross /
                {
                    if (!person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                    {
                        person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                    }
                    person.CarPlateNumbers.Add(carPlateNumber_2);
                }
                else
                {
                    if (personalTypeOwner)
                    {
                        ownerPer_2 = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode_2.Text.Trim());
                        if (ownerPer_2 == null)
                        {
                            ownerPer_2 = new Ajancy.Person { NationalCode = this.txtOwnerNationalCode_2.Text.Trim(), SubmitDate = DateTime.Now };
                            Ajancy.User ownerUser = new Ajancy.User
                            {
                                UserName = this.txtOwnerNationalCode_2.Text.Trim()
                                ,
                                ProvinceID = person_2.User.ProvinceID
                                ,
                                CityID = person_2.User.CityID
                                ,
                                SubmitDate = DateTime.Now
                            };
                            ownerUser.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                            ownerPer_2.User = ownerUser;
                            db.Persons.InsertOnSubmit(ownerPer_2);
                        }
                        else if (!ownerPer_2.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                        {
                            ownerPer_2.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                        }

                        ownerPer_2.FirstName = this.txtOwnerName_2.Text.Trim();
                        ownerPer_2.LastName = this.txtOwnerFamily_2.Text.Trim();
                        ownerPer_2.CarPlateNumbers.Add(carPlateNumber_2);
                    }
                    else
                    {
                        person_2.CarPlateNumbers.Add(carPlateNumber_2);
                        if (carPlateNumber_2.OwnerPersonID > 0 && carPlateNumber_2.OwnerPersonID != person_2.PersonID) // Set driver as owner again
                        {
                            carPlateNumber_2.Person = person_2;
                        }
                    }
                }

                if (ajancyTypeOwner && this.txtNationalCode_2.Text == this.txtOwnerNationalCode.Text) // Cross \
                {
                    if (!person_2.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                    {
                        person_2.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                    }
                    person_2.CarPlateNumbers.Add(carPlateNumber);
                }
                else
                {
                    if (ajancyTypeOwner)
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
                        ownerPer.CarPlateNumbers.Add(carPlateNumber);
                    }
                    else
                    {
                        person.CarPlateNumbers.Add(carPlateNumber);
                        if (carPlateNumber.OwnerPersonID > 0 && carPlateNumber.OwnerPersonID != person.PersonID) // Set driver as owner again
                        {
                            carPlateNumber.Person = person;
                        }
                    }
                }
            }

            #endregion

            if (this.ViewState["Mode"] == null) // Add mode
            {
                Ajancy.FuelCardSubstitution formerReq = null;
                if (fuelCard.FuelCardID > 0)
                {
                    formerReq = db.FuelCardSubstitutions.FirstOrDefault<Ajancy.FuelCardSubstitution>(fcs => (fcs.AjancyTypeFuelCardID == fuelCard.FuelCardID || fcs.PersonalTypeFuelCardID.GetValueOrDefault() == fuelCard.FuelCardID));
                    if (formerReq != null)
                    {
                        this.lblMessage.Text = "برای کارت سوخت بخش آژانسی قبلا درخواست ابطال یا جایگزین ثبت شده است";
                        return;
                    }
                }
                if (fuelCard_2.FuelCardID > 0)
                {
                    formerReq = db.FuelCardSubstitutions.FirstOrDefault<Ajancy.FuelCardSubstitution>(fcs => (fcs.AjancyTypeFuelCardID == fuelCard_2.FuelCardID || fcs.PersonalTypeFuelCardID.GetValueOrDefault() == fuelCard_2.FuelCardID));
                    if (formerReq != null)
                    {
                        this.lblMessage.Text = "برای کارت سوخت بخش شخصی قبلا درخواست ابطال یا جایگزین ثبت شده است";
                        return;
                    }
                }

                Ajancy.FuelCardSubstitution fcsReq = new Ajancy.FuelCardSubstitution
                {
                    FuelCard = fuelCard,
                    UserInRoleID = Public.ActiveUserRole.UserRoleID,
                    SubmitDate = DateTime.Now
                };

                db.FuelCardSubstitutions.InsertOnSubmit(fcsReq);
                db.SubmitChanges();
                fcsReq.PersonalTypeFuelCardID = fuelCard_2.FuelCardID;
            }
            else // Edit mode
            {
                db.FuelCardSubstitutions.FirstOrDefault<Ajancy.FuelCardSubstitution>(fcs => (fcs.AjancyTypeFuelCardID == fuelCard.FuelCardID && fcs.PersonalTypeFuelCardID.GetValueOrDefault() == fuelCard_2.FuelCardID)).SubmitDate = DateTime.Now;
            }

            db.SubmitChanges();
            DisposeContext();
            Response.Redirect("~/Message.aspx?mode=11");
        }
    }

    protected void drpProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpProvince_2.SelectedIndex = this.drpProvince.SelectedIndex;
        if (this.drpProvince.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var cities = db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue)).Select(c => new { c.CityID, c.Name }).ToList();
            this.drpCity.DataSource = cities;
            this.drpCity.DataBind();
            this.drpCity_2.DataSource = cities;
            this.drpCity_2.DataBind();
            LoadAjancies(true);
        }
        else
        {
            this.drpCity.Items.Clear();
            this.drpCity.Items.Insert(0, "- انتخاب کنید -");
            this.drpCity_2.Items.Clear();
            this.drpCity_2.Items.Insert(0, "- انتخاب کنید -");
            LoadAjancies(false);
        }
        this.drpCity_SelectedIndexChanged(sender, e);
    }

    protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
        if (this.drpProvince.SelectedIndex > 0)
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
            dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.Person>(p => p.User);
            dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
            dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
            dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.PlateNumber);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
            dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;

            Ajancy.Person person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtNationalCode.Text.Trim());
            if (person != null && Public.ActiveUserRole.RoleID == (short)Public.Role.Admin)
            {
                this.drpProvince.SelectedValue = person.User.ProvinceID.ToString();
                this.drpProvince_SelectedIndexChanged(sender, e);
                this.drpCity.SelectedValue = person.User.CityID.ToString();
                this.drpCity.Enabled = true;
                if (Request.QueryString["nc1"] == null && Request.QueryString["nc2"] == null)
                {
                    LoadAjancies(true);
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

    protected void drpCarPlateNumberProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCarPlateNumberProvince.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCarPlateNumberCity.DataSource = db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpCarPlateNumberProvince.SelectedValue)).Select(c => new { c.CityID, c.Name });
            this.drpCarPlateNumberCity.DataBind();
        }
        else
        {
            this.drpCarPlateNumberCity.Items.Clear();
            this.drpCarPlateNumberCity.Items.Insert(0, "- انتخاب کنید -");
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person == null)
        {
            this.ViewState["PersonID"] = null;
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            SetCarOwner(null);
            SetCar(null);
            SetFuelCard(null);
        }
        else
        {
            this.ViewState["PersonID"] = person.PersonID;
            this.txtFirstName.Text = person.FirstName;
            this.txtLastName.Text = person.LastName;
            this.txtNationalCode.Text = person.NationalCode;
            if (person.DriverCertifications.Count > 0)
            {
                Ajancy.DriverCertification driverCertification = person.DriverCertifications.SingleOrDefault<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                if (driverCertification != null)
                {
                    if (this.txtNationalCode.Text == this.txtNationalCode_2.Text) // Self replacement for a person who is driver
                    {
                        DisposeContext();
                        Response.Redirect(string.Format("~/Zone/SelfReplacement.aspx?id={0}", TamperProofString.QueryStringEncode(person.PersonID.ToString())));
                    }

                    if (Request.QueryString["nc1"] == null && Request.QueryString["nc2"] == null) // Add mode
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
                                this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
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
                        //if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager) // Is ProvinceManager
                        {
                            this.drpCity.SelectedValue = person.User.CityID.Value.ToString();
                            this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
                        }
                        LoadAjancies(true);
                    }

                    Ajancy.DriverCertificationCar driverCertificationCar = null;
                    try
                    {
                        driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
                    }
                    catch (InvalidOperationException)
                    {
                        DisposeContext();
                        Response.Redirect("~/Message.aspx?mode=28"); // This driver has no active car
                    }

                    try
                    {
                        this.drpAjancies.SelectedValue = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).AjancyID.ToString();
                    }
                    catch { }

                    if (driverCertificationCar.CarPlateNumber.PlateNumberID.HasValue)
                    {
                        DisposeContext();
                        Response.Redirect("~/Message.aspx?mode=30"); // This driver's current platenumber is of type Iran
                    }

                    SetCarOwner(driverCertificationCar.CarPlateNumber.Person);
                    SetCar(driverCertificationCar.CarPlateNumber.Car);
                    SetFuelCard(driverCertificationCar.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());
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
            this.txtCarModel.Text = null;
            this.drpCarType.SelectedIndex = 0;
            this.drpFuelType.SelectedIndex = 0;
            SetPlateNumber(null);
        }
        else
        {
            this.txtCarVIN.Text = car.VIN;
            this.txtCarModel.Text = car.Model;
            this.drpCarType.SelectedValue = car.CarTypeID.ToString();
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
        }
        else
        {
            this.txtFuelCardPAN.Text = fuelCard.PAN;
        }
    }


    protected void drpCity_2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpCity.SelectedIndex = this.drpCity_2.SelectedIndex;
        if (this.drpProvince_2.SelectedIndex > 0)
        {
            LoadAjancies(true);
        }
        else
        {
            LoadAjancies(false);
        }
    }

    protected void txtNationalCode_2_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtNationalCode_2.Text))
        {
            dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.Person>(p => p.User);
            dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
            dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
            dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.PlateNumber);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
            dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;

            Ajancy.Person person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtNationalCode_2.Text.Trim());
            if (person != null && Public.ActiveUserRole.RoleID == (short)Public.Role.Admin)
            {
                this.drpProvince_2.SelectedValue = person.User.ProvinceID.ToString();
                //this.drpProvince_SelectedIndexChanged(sender, e);
                this.drpCity_2.SelectedValue = person.User.CityID.ToString();
                this.drpCity_2.Enabled = true;
                if (Request.QueryString["nc1"] == null && Request.QueryString["nc2"] == null)
                {
                    LoadAjancies(true);
                }
            }
            SetPerson_2(person);
        }
        this.txtFirstName_2.Focus();
    }

    protected void txtOwnerNationalCode_2_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtOwnerNationalCode_2.Text))
        {
            this.txtOwnerName_2.Text = null;
            this.txtOwnerFamily_2.Text = null;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var person = db.Persons.Where<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode_2.Text).Select(p => new { p.FirstName, p.LastName });
            foreach (var item in person)
            {
                this.txtOwnerName_2.Text = item.FirstName;
                this.txtOwnerFamily_2.Text = item.LastName;
            }
        }
        this.txtOwnerName_2.Focus();
    }

    protected void drpCarPlateNumberProvince_2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCarPlateNumberProvince.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCarPlateNumberCity_2.DataSource = db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpCarPlateNumberProvince_2.SelectedValue)).Select(c => new { c.CityID, c.Name });
            this.drpCarPlateNumberCity_2.DataBind();
        }
        else
        {
            this.drpCarPlateNumberCity_2.Items.Clear();
            this.drpCarPlateNumberCity_2.Items.Insert(0, "- انتخاب کنید -");
        }
    }

    private void SetPerson_2(Ajancy.Person person)
    {
        if (person == null)
        {
            this.ViewState["PersonID_2"] = null;
            this.txtFirstName_2.Text = null;
            this.txtLastName_2.Text = null;
            SetCarOwner_2(null);
            SetCar_2(null);
            SetFuelCard_2(null);
        }
        else
        {
            this.ViewState["PersonID_2"] = person.PersonID;
            this.txtFirstName_2.Text = person.FirstName;
            this.txtLastName_2.Text = person.LastName;
            this.txtNationalCode_2.Text = person.NationalCode;
            if (person.DriverCertifications.Count > 0)
            {
                Ajancy.DriverCertification driverCertification = person.DriverCertifications.SingleOrDefault<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                if (driverCertification != null)
                {
                    if (this.txtNationalCode.Text == this.txtNationalCode_2.Text) // Self replacement for a person who is driver
                    {
                        DisposeContext();
                        Response.Redirect(string.Format("~/Zone/SelfReplacement.aspx?id={0}", TamperProofString.QueryStringEncode(person.PersonID.ToString())));
                    }

                    if (Request.QueryString["nc1"] == null && Request.QueryString["nc2"] == null) // Add mode
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
                                this.drpCity_2.SelectedValue = person.User.CityID.Value.ToString();
                                this.drpCity.SelectedIndex = this.drpCity_2.SelectedIndex;
                                LoadAjancies(true);
                            }
                        }
                        else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager && person.User.CityID != Public.ActiveUserRole.User.CityID)
                        {
                            DisposeContext();
                            Response.Redirect("~/Message.aspx?mode=13"); // This is a driver of an other city
                        }
                    }

                    Ajancy.DriverCertificationCar driverCertificationCar = null;
                    try
                    {
                        driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
                    }
                    catch (InvalidOperationException)
                    {
                        DisposeContext();
                        Response.Redirect("~/Message.aspx?mode=28"); // This driver has no active car
                    }

                    try
                    {
                        this.drpAjancies_2.SelectedValue = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).AjancyID.ToString();
                    }
                    catch { }

                    if (driverCertificationCar.CarPlateNumber.PlateNumberID.HasValue)
                    {
                        DisposeContext();
                        Response.Redirect("~/Message.aspx?mode=30"); // This driver's current platenumber is of type Iran
                    }

                    SetCarOwner_2(driverCertificationCar.CarPlateNumber.Person);
                    SetCar_2(driverCertificationCar.CarPlateNumber.Car);
                    SetFuelCard_2(driverCertificationCar.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());
                }
            }
        }
    }

    private void SetCarOwner_2(Ajancy.Person carOwner)
    {
        if (carOwner == null)
        {
            this.txtOwnerNationalCode_2.Text = null;
            this.txtOwnerName_2.Text = null;
            this.txtOwnerFamily_2.Text = null;
        }
        else
        {
            this.txtOwnerNationalCode_2.Text = carOwner.NationalCode;
            this.txtOwnerName_2.Text = carOwner.FirstName;
            this.txtOwnerFamily_2.Text = carOwner.LastName;
        }
    }

    private void SetCar_2(Ajancy.Car car)
    {
        if (car == null)
        {
            this.txtCarVIN_2.Text = null;
            this.txtCarModel_2.Text = null;
            this.drpCarType_2.SelectedIndex = 0;
            this.drpFuelType_2.SelectedIndex = 0;
            SetPlateNumber_2(null);
        }
        else
        {
            this.txtCarVIN_2.Text = car.VIN;
            this.txtCarModel_2.Text = car.Model;
            this.drpCarType_2.SelectedValue = car.CarTypeID.ToString();
            this.drpFuelType_2.SelectedValue = car.FuelType.ToString();
            SetPlateNumber_2(car.CarPlateNumbers.Last<Ajancy.CarPlateNumber>().ZonePlateNumber);
        }
    }

    private void SetPlateNumber_2(Ajancy.ZonePlateNumber plateNumber)
    {
        if (plateNumber != null)
        {
            this.txtCarPlateNumber_5_2.Text = plateNumber.Number;
            this.drpCarPlateNumberProvince_2.SelectedValue = plateNumber.City.ProvinceID.ToString();
            this.drpCarPlateNumberProvince_2_SelectedIndexChanged(this, new EventArgs());
            this.drpCarPlateNumberCity_2.SelectedValue = plateNumber.CityID.ToString();
        }
        else
        {
            this.txtCarPlateNumber_5_2.Text = null;
            this.drpCarPlateNumberProvince_2.SelectedIndex = 0;
            this.drpCarPlateNumberCity_2.SelectedIndex = 0;
        }
    }

    private void SetFuelCard_2(Ajancy.FuelCard fuelCard)
    {
        if (fuelCard == null)
        {
            this.txtFuelCardPAN_2.Text = null;
        }
        else
        {
            this.txtFuelCardPAN_2.Text = fuelCard.PAN;
        }
    }

    private void LoadAjancies(bool loadItems)
    {
        this.drpAjancies.Items.Clear();
        this.drpAjancies_2.Items.Clear();

        if (loadItems)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var ajancyList = (from j in db.Ajancies
                              join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                              join ct in db.Cities on j.CityID equals ct.CityID
                              where j.AjancyType == (byte)Public.AjancyType.TaxiAjancy &&
                                         ct.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue) &&
                                         ct.CityID == Public.ToShort(this.drpCity.SelectedValue) &&
                                         bl.LockOutDate == null
                              orderby j.AjancyName
                              select new { j.AjancyID, j.AjancyName }).ToList();

            this.drpAjancies.DataSource = ajancyList;
            this.drpAjancies.DataBind();
            this.drpAjancies_2.DataSource = ajancyList;
            this.drpAjancies_2.DataBind();
        }
        this.drpAjancies.Items.Insert(0, "- انتخاب کنید -");
        this.drpAjancies_2.Items.Insert(0, "- انتخاب کنید -");
    }

    private void SelfReplacement_Save()
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
        Ajancy.ZonePlateNumber plateNumber = new Ajancy.ZonePlateNumber();
        Ajancy.Car car = new Ajancy.Car();
        Ajancy.FuelCard fuelCard = new Ajancy.FuelCard();

        if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim()))
        {
            this.lblMessage.Text = "خودرو بخش کارت سوخت آژانسی با این شماره VIN متعلق به شخص دیگری میباشد";
            return;
        }

        if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text))
        {
            this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت آژانسی قبلا برای خودرو دیگری ثبت شده";
            return;
        }

        Ajancy.ZonePlateNumber pln = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity.SelectedValue) &&
                                                                                                      pl.Number == this.txtCarPlateNumber_5.Text.Trim());
        if (pln != null)
        {
            this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت آژانسی متعلق به شخص دیگری میباشد";
            return;
        }

        if (this.ViewState["PersonID"] == null && this.ViewState["PersonID_2"] == null) // New Person  
        {
            Ajancy.User user = new Ajancy.User();
            user.UserName = this.txtNationalCode.Text.Trim();
            user.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
            user.CityID = Public.ToShort(this.drpCity.SelectedValue);
            user.SubmitDate = DateTime.Now;
            user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
            person.User = user;
            person.NationalCode = this.txtNationalCode.Text.Trim();
            person.SubmitDate = DateTime.Now;

            driverCertification = new Ajancy.DriverCertification
            {
                CertificationType = (byte)Public.AjancyType.TaxiAjancy,
                SubmitDate = DateTime.Now
            };
            carPlateNumber = new Ajancy.CarPlateNumber { Car = car, ZonePlateNumber = plateNumber };
            driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now, LockOutDate = DateTime.Now };
            driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
            driverCertification.DriverCertificationCars.Add(driverCertificationCar);
            person.DriverCertifications.Add(driverCertification);
            fuelCard.SubmitDate = DateTime.Now;
            fuelCard.DiscardDate = DateTime.Now;
            car.FuelCards.Add(fuelCard);
            db.Persons.InsertOnSubmit(person);
        }
        else // Person Exists
        {
            if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
            {
                person.User.CityID = Public.ToShort(this.drpCity_2.SelectedValue);
            }

            person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));
            person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
            driverCertification = new Ajancy.DriverCertification
            {
                CertificationType = (byte)Public.AjancyType.TaxiAjancy,
                SubmitDate = DateTime.Now
            };
            carPlateNumber = new Ajancy.CarPlateNumber { Car = car, ZonePlateNumber = plateNumber };
            driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now, LockOutDate = DateTime.Now };
            driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), DriverCertificationCar = driverCertificationCar, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
            driverCertification.DriverCertificationCars.Add(driverCertificationCar);
            person.DriverCertifications.Add(driverCertification);
            fuelCard.SubmitDate = DateTime.Now;
            fuelCard.DiscardDate = DateTime.Now;
            car.FuelCards.Add(fuelCard);

            switch ((Public.Role)Public.ActiveUserRole.RoleID)
            {
                case Public.Role.ProvinceManager:
                    person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    break;

                case Public.Role.Admin:
                    person.User.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                    person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                    break;
            }
        }

        // --------------- setting values           

        person.FirstName = this.txtFirstName.Text.Trim();
        person.LastName = this.txtLastName.Text.Trim();

        plateNumber.CityID = Public.ToShort(this.drpCarPlateNumberCity.SelectedValue);
        plateNumber.Number = this.txtCarPlateNumber_5.Text.Trim();

        car.CarTypeID = Public.ToShort(this.drpCarType.SelectedValue);
        car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);
        car.Model = this.txtCarModel.Text;

        fuelCard.CardType = (byte)Public.FuelCardType.Ajancy;
        fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();

        car.VIN = this.txtCarVIN.Text.Trim().ToUpper();

        #endregion

        #region PersonalType

        Ajancy.DriverCertificationCar driverCertificationCar_2 = new Ajancy.DriverCertificationCar { SubmitDate = DateTime.Now };
        Ajancy.CarPlateNumber carPlateNumber_2 = new Ajancy.CarPlateNumber();
        Ajancy.ZonePlateNumber plateNumber_2 = new Ajancy.ZonePlateNumber();
        Ajancy.Car car_2 = new Ajancy.Car();
        Ajancy.FuelCard fuelCard_2 = new Ajancy.FuelCard();

        if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN_2.Text.Trim()))
        {
            this.lblMessage.Text = "خودرو بخش کارت سوخت شخصی با این شماره VIN متعلق به شخص دیگری میباشد";
            return;
        }

        if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN_2.Text))
        {
            this.lblMessage.Text = "شماره PAN کارت سوخت خودرو بخش کارت سوخت شخصی قبلا برای خودرو دیگری ثبت شده";
            return;
        }

        Ajancy.ZonePlateNumber pnl = db.ZonePlateNumbers.FirstOrDefault<Ajancy.ZonePlateNumber>(pl => pl.CityID == Public.ToShort(this.drpCarPlateNumberCity_2.SelectedValue) &&
                                                                                                      pl.Number == this.txtCarPlateNumber_5_2.Text.Trim());
        if (pnl != null)
        {
            this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت شخصی متعلق به شخص دیگری میباشد";
            return;
        }

        if (this.ViewState["PersonID"] == null && this.ViewState["PersonID_2"] == null) // New Person  
        {
            carPlateNumber_2 = new Ajancy.CarPlateNumber { Car = car_2, ZonePlateNumber = plateNumber_2 };
            driverCertificationCar_2 = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber_2, SubmitDate = DateTime.Now };
            driverCertificationCar_2.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies_2.SelectedValue), MembershipDate = DateTime.Now });
            driverCertification.DriverCertificationCars.Add(driverCertificationCar_2);
            fuelCard_2.SubmitDate = DateTime.Now;
            car_2.FuelCards.Add(fuelCard_2);
        }
        else // Person Exists
        {
            carPlateNumber_2 = new Ajancy.CarPlateNumber { Car = car_2, ZonePlateNumber = plateNumber_2 };
            driverCertificationCar_2 = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber_2, SubmitDate = DateTime.Now };
            driverCertificationCar_2.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies_2.SelectedValue), MembershipDate = DateTime.Now });
            driverCertification.DriverCertificationCars.Add(driverCertificationCar_2);
            fuelCard_2.SubmitDate = DateTime.Now;
            car_2.FuelCards.Add(fuelCard_2);
        }

        // --------------- setting values        

        if (this.txtCarPlateNumber_5.Text.Trim().Equals(this.txtCarPlateNumber_5_2.Text.Trim()) &&
            this.drpCarPlateNumberCity.SelectedValue.Equals(this.drpCarPlateNumberCity_2.SelectedValue))
        {
            carPlateNumber_2.ZonePlateNumber = plateNumber;
        }
        else
        {
            plateNumber_2.Number = this.txtCarPlateNumber_5_2.Text.Trim();
            plateNumber_2.CityID = Public.ToShort(this.drpCarPlateNumberCity_2.SelectedValue);
        }

        car_2.CarTypeID = Public.ToShort(this.drpCarType_2.SelectedValue);
        car_2.FuelType = Public.ToByte(this.drpFuelType_2.SelectedValue);
        car_2.Model = this.txtCarModel_2.Text;

        fuelCard_2.CardType = (byte)Public.FuelCardType.Ajancy;
        fuelCard_2.PAN = this.txtFuelCardPAN_2.Text.Trim();

        car_2.VIN = this.txtCarVIN_2.Text.Trim().ToUpper();

        #endregion

        #region Owners

        bool ajancyTypeOwner = false;
        bool personalTypeOwner = false;
        Ajancy.Person ownerPer = null;
        Ajancy.Person ownerPer_2 = null;

        if (!string.IsNullOrEmpty(this.txtOwnerName.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerFamily.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerNationalCode.Text.Trim()) && !this.txtOwnerNationalCode.Text.Trim().Equals(person.NationalCode))
        {
            ajancyTypeOwner = true;
        }
        if (!string.IsNullOrEmpty(this.txtOwnerName_2.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerFamily_2.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerNationalCode_2.Text.Trim()) && !this.txtOwnerNationalCode_2.Text.Trim().Equals(person.NationalCode))
        {
            personalTypeOwner = true;
        }

        if (ajancyTypeOwner && personalTypeOwner && this.txtOwnerNationalCode_2.Text == this.txtOwnerNationalCode.Text) // Both owners are the same person
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
            ownerPer.CarPlateNumbers.Add(carPlateNumber);
            ownerPer.CarPlateNumbers.Add(carPlateNumber_2);
        }
        else
        {
            if (personalTypeOwner && this.txtNationalCode.Text == this.txtOwnerNationalCode_2.Text) // Cross /
            {
                if (!person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                {
                    person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                }
                person.CarPlateNumbers.Add(carPlateNumber_2);
            }
            else
            {
                if (personalTypeOwner)
                {
                    ownerPer_2 = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode_2.Text.Trim());
                    if (ownerPer_2 == null)
                    {
                        ownerPer_2 = new Ajancy.Person { NationalCode = this.txtOwnerNationalCode_2.Text.Trim(), SubmitDate = DateTime.Now };
                        Ajancy.User ownerUser = new Ajancy.User
                        {
                            UserName = this.txtOwnerNationalCode_2.Text.Trim()
                            ,
                            ProvinceID = person.User.ProvinceID
                            ,
                            CityID = person.User.CityID
                            ,
                            SubmitDate = DateTime.Now
                        };
                        ownerUser.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                        ownerPer_2.User = ownerUser;
                        db.Persons.InsertOnSubmit(ownerPer_2);
                    }
                    else if (!ownerPer_2.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                    {
                        ownerPer_2.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                    }

                    ownerPer_2.FirstName = this.txtOwnerName_2.Text.Trim();
                    ownerPer_2.LastName = this.txtOwnerFamily_2.Text.Trim();
                    ownerPer_2.CarPlateNumbers.Add(carPlateNumber_2);
                }
                else
                {
                    person.CarPlateNumbers.Add(carPlateNumber_2);
                    if (carPlateNumber_2.OwnerPersonID > 0 && carPlateNumber_2.OwnerPersonID != person.PersonID) // Set driver as owner again
                    {
                        carPlateNumber_2.Person = person;
                    }
                }
            }

            if (ajancyTypeOwner && this.txtNationalCode_2.Text == this.txtOwnerNationalCode.Text) // Cross \
            {
                if (!person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
                {
                    person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
                }
                person.CarPlateNumbers.Add(carPlateNumber);
            }
            else
            {
                if (ajancyTypeOwner)
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
                    ownerPer.CarPlateNumbers.Add(carPlateNumber);
                }
                else
                {
                    person.CarPlateNumbers.Add(carPlateNumber);
                    if (carPlateNumber.OwnerPersonID > 0 && carPlateNumber.OwnerPersonID != person.PersonID) // Set driver as owner again
                    {
                        carPlateNumber.Person = person;
                    }
                }
            }
        }

        #endregion

        Ajancy.FuelCardSubstitution fcsReq = new Ajancy.FuelCardSubstitution
        {
            FuelCard = fuelCard,
            UserInRoleID = Public.ActiveUserRole.UserRoleID,
            SubmitDate = DateTime.Now
        };

        db.FuelCardSubstitutions.InsertOnSubmit(fcsReq);
        db.SubmitChanges();
        fcsReq.PersonalTypeFuelCardID = fuelCard_2.FuelCardID;
        db.SubmitChanges();
        DisposeContext();
        Response.Redirect("~/Message.aspx?mode=11");
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
