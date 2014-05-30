using System;
using System.Linq;
using System.Data.Linq;

public partial class Academy_SelfReplacement : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int personId = 0;
            if (Request.QueryString["nc"] == null && Request.QueryString["id"] == null) // Invalid Reqest
            {
                DisposeContext();
                Response.Redirect("~/Academy/FCReplacement.aspx");
            }

            System.Collections.Generic.List<Ajancy.CarType> carTypes = db.CarTypes.OrderBy(ct => ct.TypeName).ToList<Ajancy.CarType>();
            this.drpCarType.DataSource = carTypes;
            this.drpCarType.DataBind();
            this.drpCarType_2.DataSource = carTypes;
            this.drpCarType_2.DataBind();
            this.drpCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
            this.drpCarType_2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));

            Ajancy.User user = Public.ActiveUserRole.User;
            if (Request.QueryString["nc"] == null && Request.QueryString["id"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId)) // Add mode
            {
                switch ((Public.Role)Public.ActiveUserRole.RoleID)
                {
                    case Public.Role.AcademyCity:
                        this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                        this.drpProvince_2.SelectedIndex = this.drpProvince.SelectedIndex;
                        this.drpProvince_SelectedIndexChanged(sender, e);
                        this.drpCity.SelectedValue = user.CityID.ToString();
                        this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;

                        this.drpCity.Enabled = false;
                        this.drpCity_2.Enabled = false;
                        break;

                    case Public.Role.AcademyProvince:
                        this.drpProvince.SelectedValue = user.ProvinceID.ToString();
                        this.drpProvince_2.SelectedIndex = this.drpProvince.SelectedIndex;
                        this.drpProvince_SelectedIndexChanged(sender, e);
                        this.drpCity.SelectedValue = user.CityID.ToString();
                        this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;

                        this.drpCity.Enabled = true;
                        this.drpCity_2.Enabled = true;
                        break;

                    case Public.Role.Admin:
                        this.drpProvince.Enabled = true;
                        this.drpProvince_2.Enabled = true;
                        break;
                }
                //LoadAjancies(true);
                SetPerson(db.Persons.First<Ajancy.Person>(p => p.PersonID == personId));
            }
            else if (Request.QueryString["nc"] != null && Request.QueryString["id"] == null) // Edit mode
            {
                switch ((Public.Role)Public.ActiveUserRole.RoleID)
                {
                    case Public.Role.AcademyCity:
                        this.drpCity.Enabled = false;
                        this.drpCity_2.Enabled = false;
                        break;

                    case Public.Role.AcademyProvince:
                        this.drpCity.Enabled = true;
                        this.drpCity_2.Enabled = true;
                        break;

                    case Public.Role.Admin:
                        this.drpProvince.Enabled = true;
                        this.drpProvince_2.Enabled = true;
                        break;
                }
                SelfReplacement_Restore(TamperProofString.QueryStringDecode(Request.QueryString["nc"]));
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

            if (Request.QueryString["nc"] == null && Request.QueryString["id"] != null) // Add mode
            {
                SelfReplacement_Save();
            }
            else if (Request.QueryString["nc"] != null && Request.QueryString["id"] == null) // Edit mode
            {
                SelfReplacement_Edit();
            }
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
    }

    protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
        if (this.drpCity.SelectedIndex > 0)
        {
            LoadAjancies(true);
        }
        else
        {
            LoadAjancies(false);
        }
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
            this.txtFirstName_2.Text = person.FirstName;
            this.txtLastName_2.Text = person.LastName;
            this.txtNationalCode_2.Text = person.NationalCode;

            if (person.DriverCertifications.Count > 0)
            {
                Ajancy.DriverCertification driverCertification = person.DriverCertifications.SingleOrDefault<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.Academy);
                if (driverCertification != null)
                {
                    if (Request.QueryString["nc1"] == null && Request.QueryString["nc2"] == null) // Add mode
                    {
                        if (Public.ActiveUserRole.RoleID == (short)Public.Role.AcademyProvince) // Is ProvinceManager
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
                        if (Public.ActiveUserRole.RoleID == (short)Public.Role.AcademyProvince) // Is ProvinceManager
                        {
                            this.drpCity.SelectedValue = person.User.CityID.Value.ToString();
                            this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
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
        }
        else
        {
            this.txtFuelCardPAN.Text = fuelCard.PAN;
        }
    }


    protected void drpCity_2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpCity.SelectedIndex = this.drpCity_2.SelectedIndex;
        if (this.drpCity_2.SelectedIndex > 0)
        {
            LoadAjancies(true);
        }
        else
        {
            LoadAjancies(false);
        }
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
            SetPlateNumber_2(car.CarPlateNumbers.Last<Ajancy.CarPlateNumber>().PlateNumber);
        }
    }

    private void SetPlateNumber_2(Ajancy.PlateNumber plateNumber)
    {
        if (plateNumber != null)
        {
            this.txtCarPlateNumber_1_2.Text = plateNumber.TwoDigits;
            this.drpCarPlateNumber_2.SelectedValue = plateNumber.Alphabet;
            this.txtCarPlateNumber_2_2.Text = plateNumber.ThreeDigits;
            this.txtCarPlateNumber_3_2.Text = plateNumber.RegionIdentifier;
        }
        else
        {
            this.txtCarPlateNumber_1_2.Text = null;
            this.drpCarPlateNumber_2.SelectedIndex = 0;
            this.txtCarPlateNumber_2_2.Text = null;
            this.txtCarPlateNumber_3_2.Text = null;
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
                              where j.AjancyType == (byte)Public.AjancyType.Academy &&
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
        dlo = new DataLoadOptions();
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

        Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));
        Ajancy.DriverCertification driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.Academy);
        Ajancy.DriverCertificationCar driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
        Ajancy.CarPlateNumber carPlateNumber = driverCertificationCar.CarPlateNumber;
        Ajancy.PlateNumber plateNumber = carPlateNumber.PlateNumber;
        Ajancy.Car car = carPlateNumber.Car;
        Ajancy.FuelCard fuelCard = car.FuelCards.Last<Ajancy.FuelCard>();

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

        Ajancy.PlateNumber pln = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                            pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                            pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                            pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim() &&
                                                                                                                            pl.PlateNumberID != carPlateNumber.PlateNumber.PlateNumberID);
        if (pln != null)
        {
            this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت آژانسی متعلق به شخص دیگری میباشد";
            return;
        }

        // --------------- setting values           

        driverCertificationCar.LockOutDate = DateTime.Now;
        driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).LockOutDate = DateTime.Now;

        person.FirstName = this.txtFirstName.Text.Trim();
        person.LastName = this.txtLastName.Text.Trim();

        plateNumber.TwoDigits = this.txtCarPlateNumber_1.Text.Trim();
        plateNumber.Alphabet = this.drpCarPlateNumber.SelectedValue;
        plateNumber.ThreeDigits = this.txtCarPlateNumber_2.Text.Trim();
        plateNumber.RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim();

        car.CarTypeID = Public.ToShort(this.drpCarType.SelectedValue);
        car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);
        car.Model = this.txtCarModel.Text;

        fuelCard.CardType = (byte)Public.FuelCardType.Ajancy;
        fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();
        fuelCard.DiscardDate = DateTime.Now;

        car.VIN = this.txtCarVIN.Text.Trim().ToUpper();

        #endregion

        #region PersonalType

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

        Ajancy.PlateNumber pnl = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1_2.Text.Trim() &&
                                                                                                                           pl.Alphabet == this.drpCarPlateNumber_2.SelectedValue &&
                                                                                                                           pl.ThreeDigits == this.txtCarPlateNumber_2_2.Text.Trim() &&
                                                                                                                           pl.RegionIdentifier == this.txtCarPlateNumber_3_2.Text.Trim());
        if (pnl != null)
        {
            this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت شخصی متعلق به شخص دیگری میباشد";
            return;
        }

        Ajancy.DriverCertificationCar driverCertificationCar_2 = new Ajancy.DriverCertificationCar { SubmitDate = DateTime.Now };
        Ajancy.CarPlateNumber carPlateNumber_2 = new Ajancy.CarPlateNumber();
        Ajancy.PlateNumber plateNumber_2 = new Ajancy.PlateNumber();
        Ajancy.Car car_2 = new Ajancy.Car();
        Ajancy.FuelCard fuelCard_2 = new Ajancy.FuelCard { SubmitDate = DateTime.Now };
        driverCertificationCar_2.CarPlateNumber = carPlateNumber_2;

        carPlateNumber_2 = new Ajancy.CarPlateNumber { Car = car_2, PlateNumber = plateNumber_2 };
        driverCertificationCar_2 = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber_2, SubmitDate = DateTime.Now };
        driverCertificationCar_2.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies_2.SelectedValue), MembershipDate = DateTime.Now });
        driverCertification.DriverCertificationCars.Add(driverCertificationCar_2);
        car_2.FuelCards.Add(fuelCard_2);

        // --------------- setting values                   

        if (this.txtCarPlateNumber_1.Text.Trim().Equals(this.txtCarPlateNumber_1_2.Text.Trim()) &&
           this.drpCarPlateNumber.SelectedValue.Equals(this.drpCarPlateNumber_2.SelectedValue) &&
           this.txtCarPlateNumber_2.Text.Trim().Equals(this.txtCarPlateNumber_2_2.Text.Trim()) &&
           this.txtCarPlateNumber_3.Text.Trim().Equals(this.txtCarPlateNumber_3_2.Text.Trim()))
        {
            carPlateNumber_2.PlateNumber = plateNumber;
        }
        else
        {
            plateNumber_2.TwoDigits = this.txtCarPlateNumber_1_2.Text.Trim();
            plateNumber_2.Alphabet = this.drpCarPlateNumber_2.SelectedValue;
            plateNumber_2.ThreeDigits = this.txtCarPlateNumber_2_2.Text.Trim();
            plateNumber_2.RegionIdentifier = this.txtCarPlateNumber_3_2.Text.Trim();
        }

        plateNumber_2.TwoDigits = this.txtCarPlateNumber_1_2.Text.Trim();
        plateNumber_2.Alphabet = this.drpCarPlateNumber_2.SelectedValue;
        plateNumber_2.ThreeDigits = this.txtCarPlateNumber_2_2.Text.Trim();
        plateNumber_2.RegionIdentifier = this.txtCarPlateNumber_3_2.Text.Trim();

        car_2.CarTypeID = Public.ToShort(this.drpCarType_2.SelectedValue);
        car_2.FuelType = Public.ToByte(this.drpFuelType_2.SelectedValue);
        car_2.Model = this.txtCarModel_2.Text;

        fuelCard_2.CardType = (byte)Public.FuelCardType.Ajancy;
        fuelCard_2.PAN = this.txtFuelCardPAN_2.Text.Trim();

        car_2.VIN = this.txtCarVIN_2.Text.Trim().ToUpper();

        #endregion

        switch ((Public.Role)Public.ActiveUserRole.RoleID)
        {
            case Public.Role.AcademyProvince:
                person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                break;

            case Public.Role.Admin:
                person.User.ProvinceID = Public.ToByte(this.drpProvince.SelectedValue);
                person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
                break;
        }

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


        if (Public.ActiveUserRole.RoleID == (short)Public.Role.AcademyProvince)
        {
            person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
        }

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

        formerReq = new Ajancy.FuelCardSubstitution
        {
            FuelCard = fuelCard,
            UserInRoleID = Public.ActiveUserRole.UserRoleID,
            SubmitDate = DateTime.Now
        };

        db.FuelCardSubstitutions.InsertOnSubmit(formerReq);
        db.SubmitChanges();
        formerReq.PersonalTypeFuelCardID = fuelCard_2.FuelCardID;
        db.SubmitChanges();
        DisposeContext();
        Response.Redirect("~/Message.aspx?mode=11");
    }

    private void SelfReplacement_Edit()
    {
        dlo = new DataLoadOptions();
        dlo.LoadWith<Ajancy.Person>(p => p.User);
        dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
        dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
        dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
        dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
        dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
        dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        db.LoadOptions = dlo;

        Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));
        Ajancy.DriverCertification driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.Academy);
        Ajancy.DriverCertificationCar driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate != null);
        Ajancy.CarPlateNumber carPlateNumber = driverCertificationCar.CarPlateNumber;
        Ajancy.PlateNumber plateNumber = carPlateNumber.PlateNumber;
        Ajancy.Car car = carPlateNumber.Car;
        Ajancy.FuelCard fuelCard = car.FuelCards.Last<Ajancy.FuelCard>(fc => fc.DiscardDate != null);

        Ajancy.DriverCertificationCar driverCertificationCar_2 = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
        Ajancy.CarPlateNumber carPlateNumber_2 = driverCertificationCar_2.CarPlateNumber;
        Ajancy.PlateNumber plateNumber_2 = carPlateNumber_2.PlateNumber;
        Ajancy.Car car_2 = carPlateNumber_2.Car;
        Ajancy.FuelCard fuelCard_2 = car_2.FuelCards.Last<Ajancy.FuelCard>(fc => fc.DiscardDate == null);

        #region AjancyType

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

        Ajancy.PlateNumber pln = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                            pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                            pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                            pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim() &&
                                                                                                                            pl.PlateNumberID != carPlateNumber.PlateNumber.PlateNumberID);
        if (pln != null)
        {
            this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت آژانسی متعلق به شخص دیگری میباشد";
            return;
        }

        // --------------- setting values           

        person.FirstName = this.txtFirstName.Text.Trim();
        person.LastName = this.txtLastName.Text.Trim();

        plateNumber.TwoDigits = this.txtCarPlateNumber_1.Text.Trim();
        plateNumber.Alphabet = this.drpCarPlateNumber.SelectedValue;
        plateNumber.ThreeDigits = this.txtCarPlateNumber_2.Text.Trim();
        plateNumber.RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim();

        car.CarTypeID = Public.ToShort(this.drpCarType.SelectedValue);
        car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);
        car.Model = this.txtCarModel.Text;

        fuelCard.CardType = (byte)Public.FuelCardType.Ajancy;
        fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();

        car.VIN = this.txtCarVIN.Text.Trim().ToUpper();
        driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate != null).AjancyID = Public.ToInt(this.drpAjancies.SelectedValue);

        if (Public.ActiveUserRole.RoleID == (short)Public.Role.AcademyProvince)
        {
            person.User.CityID = Public.ToShort(this.drpCity.SelectedValue);
        }

        #endregion

        #region PersonalType

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

        Ajancy.PlateNumber pnl = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1_2.Text.Trim() &&
                                                                                                                           pl.Alphabet == this.drpCarPlateNumber_2.SelectedValue &&
                                                                                                                           pl.ThreeDigits == this.txtCarPlateNumber_2_2.Text.Trim() &&
                                                                                                                           pl.RegionIdentifier == this.txtCarPlateNumber_3_2.Text.Trim() &&
                                                                                                                           pl.PlateNumberID != carPlateNumber_2.PlateNumber.PlateNumberID);
        if (pnl != null)
        {
            this.lblMessage.Text = "شماره پلاک وارد شده در  بخش کارت سوخت شخصی متعلق به شخص دیگری میباشد";
            return;
        }

        // --------------- setting values           

        if (this.txtCarPlateNumber_1.Text.Trim().Equals(this.txtCarPlateNumber_1_2.Text.Trim()) &&
              this.drpCarPlateNumber.SelectedValue.Equals(this.drpCarPlateNumber_2.SelectedValue) &&
              this.txtCarPlateNumber_2.Text.Trim().Equals(this.txtCarPlateNumber_2_2.Text.Trim()) &&
              this.txtCarPlateNumber_3.Text.Trim().Equals(this.txtCarPlateNumber_3_2.Text.Trim()))
        {
            carPlateNumber_2.PlateNumber = plateNumber;
        }
        else
        {
            plateNumber_2.TwoDigits = this.txtCarPlateNumber_1_2.Text.Trim();
            plateNumber_2.Alphabet = this.drpCarPlateNumber_2.SelectedValue;
            plateNumber_2.ThreeDigits = this.txtCarPlateNumber_2_2.Text.Trim();
            plateNumber_2.RegionIdentifier = this.txtCarPlateNumber_3_2.Text.Trim();
        }

        car_2.CarTypeID = Public.ToShort(this.drpCarType_2.SelectedValue);
        car_2.FuelType = Public.ToByte(this.drpFuelType_2.SelectedValue);
        car_2.Model = this.txtCarModel_2.Text;

        fuelCard_2.CardType = (byte)Public.FuelCardType.Ajancy;
        fuelCard_2.PAN = this.txtFuelCardPAN_2.Text.Trim();

        car_2.VIN = this.txtCarVIN_2.Text.Trim().ToUpper();
        driverCertificationCar_2.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).AjancyID = Public.ToInt(this.drpAjancies_2.SelectedValue);

        #endregion

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

        db.FuelCardSubstitutions.FirstOrDefault<Ajancy.FuelCardSubstitution>(fcs => (fcs.AjancyTypeFuelCardID == fuelCard.FuelCardID && fcs.PersonalTypeFuelCardID.GetValueOrDefault() == fuelCard_2.FuelCardID)).SubmitDate = DateTime.Now;
        db.SubmitChanges();
        DisposeContext();
        Response.Redirect("~/Message.aspx?mode=11");
    }

    private void SelfReplacement_Restore(string nationalCode)
    {
        dlo = new DataLoadOptions();
        dlo.LoadWith<Ajancy.Person>(p => p.User);
        dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
        dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
        dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
        dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
        dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
        dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        db.LoadOptions = dlo;

        Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.NationalCode == nationalCode);
        Ajancy.DriverCertification driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.Academy);
        Ajancy.DriverCertificationCar driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate != null);
        Ajancy.CarPlateNumber carPlateNumber = driverCertificationCar.CarPlateNumber;
        Ajancy.PlateNumber plateNumber = carPlateNumber.PlateNumber;
        Ajancy.Car car = carPlateNumber.Car;
        Ajancy.FuelCard fuelCard = car.FuelCards.Last<Ajancy.FuelCard>(fc => fc.DiscardDate != null);

        this.drpProvince.SelectedValue = person.User.ProvinceID.ToString();
        this.drpProvince_2.SelectedIndex = this.drpProvince.SelectedIndex;
        this.drpProvince_SelectedIndexChanged(this, new EventArgs());
        this.drpCity.SelectedValue = person.User.CityID.ToString();
        this.drpCity_2.SelectedIndex = this.drpCity.SelectedIndex;
        LoadAjancies(true);

        this.ViewState["PersonID"] = person.PersonID;
        this.txtFirstName.Text = person.FirstName;
        this.txtLastName.Text = person.LastName;
        this.txtNationalCode.Text = person.NationalCode;

        this.drpAjancies.SelectedValue = driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate != null).AjancyID.ToString();
        SetCarOwner(carPlateNumber.Person);
        SetCar(car);
        SetFuelCard(fuelCard);

        Ajancy.DriverCertificationCar driverCertificationCar_2 = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null);
        Ajancy.CarPlateNumber carPlateNumber_2 = driverCertificationCar_2.CarPlateNumber;
        Ajancy.PlateNumber plateNumber_2 = carPlateNumber_2.PlateNumber;
        Ajancy.Car car_2 = carPlateNumber_2.Car;
        Ajancy.FuelCard fuelCard_2 = car_2.FuelCards.Last<Ajancy.FuelCard>(fc => fc.DiscardDate == null);

        this.txtFirstName_2.Text = person.FirstName;
        this.txtLastName_2.Text = person.LastName;
        this.txtNationalCode_2.Text = person.NationalCode;
        this.drpAjancies_2.SelectedValue = driverCertificationCar_2.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate == null).AjancyID.ToString();
        SetCarOwner_2(carPlateNumber_2.Person);
        SetCar_2(car_2);
        SetFuelCard_2(fuelCard_2);
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}