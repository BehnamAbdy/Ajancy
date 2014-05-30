using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;

public partial class Ajancy_Driver : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Public.ActiveUserRole.RoleID == (short)Public.Role.AjancyManager && Public.ActiveAjancy.AjancyID == 0)
            {
                DisposeContext();
                Response.Redirect("~/Message.aspx?mode=8");
            }
            else if ((Public.ActiveUserRole.RoleID == (short)Public.Role.Admin ||
                      Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager ||
                      Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager) && Request.QueryString["id"] == null)
            {
                DisposeContext();
                Response.Redirect("~/Default.aspx");
            }

            int personId = 0;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCarType.DataSource = db.CarTypes.OrderBy(ct => ct.TypeName);
            this.drpCarType.DataBind();
            this.drpCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
            if (Request.QueryString["id"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId))
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
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;
                SetPerson(db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == personId));
                this.txtNationalCode.ReadOnly = true;
            }

            //this.drpFormerCarType.DataSource = cars;
            //this.drpFormerCarType.DataBind();
            //this.drpFormerCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
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
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
            db.LoadOptions = dlo;
            Ajancy.Person person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtNationalCode.Text.Trim());
            if (person != null)
            {
                if (person.DriverCertifications.Any<Ajancy.DriverCertification>(dc => dc.CertificationType == Public.ActiveAjancy.AjancyType))
                {
                    // This person already has DriverCertification
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=5", true);
                }
            }
            SetPerson(person);
        }
        this.txtFirstName.Focus();
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
            Ajancy.DrivingLicense drivingLicense = new Ajancy.DrivingLicense();
            Ajancy.DriverCertification driverCertification = new Ajancy.DriverCertification();
            Ajancy.DriverCertificationCar driverCertificationCar = new Ajancy.DriverCertificationCar();
            Ajancy.CarPlateNumber carPlateNumber = new Ajancy.CarPlateNumber();
            Ajancy.PlateNumber plateNumber = new Ajancy.PlateNumber();
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
                                this.lblMessage.Text = "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
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
                user.CityID = Public.ActiveUserRole.User.CityID;
                user.SubmitDate = DateTime.Now;
                user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
                person.User = user;
                person.NationalCode = this.txtNationalCode.Text.Trim();
                person.SubmitDate = DateTime.Now;

                driverCertification.CertificationType = (byte)Public.AjancyType.TaxiAjancy;
                driverCertification.SubmitDate = DateTime.Now;

                carPlateNumber = new Ajancy.CarPlateNumber { Car = car, PlateNumber = plateNumber, Person = person };
                driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };
                driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                person.DrivingLicenses.Add(drivingLicense);
                person.DriverCertifications.Add(driverCertification);
                fuelCard.SubmitDate = DateTime.Now;
                car.FuelCards.Add(fuelCard);
                db.CarPlateNumbers.InsertOnSubmit(carPlateNumber);
                db.AjancyDrivers.InsertOnSubmit(new Ajancy.AjancyDriver { AjancyID = Public.ActiveAjancy.AjancyID, DriverCertificationCar = driverCertificationCar, MembershipDate = DateTime.Now });
                db.Persons.InsertOnSubmit(person);
            }
            else // Person Exists
            {
                person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));

                if (person.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.TaxiDriver))  // Person is a driver
                {
                    driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                    driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>();
                    carPlateNumber = driverCertificationCar.CarPlateNumber;
                    plateNumber = carPlateNumber.PlateNumber;
                    car = carPlateNumber.Car;
                    //fuelCard = car.FuelCards.First<Ajancy.FuelCard>(fc => fc.DiscardDate == null);
                    fuelCard = car.FuelCards.Last<Ajancy.FuelCard>();
                    drivingLicense = person.DrivingLicenses.LastOrDefault<Ajancy.DrivingLicense>();
                    if (drivingLicense == null)
                    {
                        drivingLicense = new Ajancy.DrivingLicense();
                        person.DrivingLicenses.Add(drivingLicense);
                    }

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
                                    this.lblMessage.Text = "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
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
                                    this.lblMessage.Text = "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
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
                    driverCertification.CertificationType = (byte)Public.AjancyType.TaxiAjancy;
                    driverCertification.SubmitDate = DateTime.Now;
                    carPlateNumber = new Ajancy.CarPlateNumber { Car = car, PlateNumber = plateNumber, Person = person };
                    driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };
                    driverCertification.DriverCertificationCars.Add(driverCertificationCar);
                    person.DriverCertifications.Add(driverCertification);
                    person.DrivingLicenses.Add(drivingLicense);
                    fuelCard.SubmitDate = DateTime.Now;
                    car.FuelCards.Add(fuelCard);
                    db.CarPlateNumbers.InsertOnSubmit(carPlateNumber);
                    db.AjancyDrivers.InsertOnSubmit(new Ajancy.AjancyDriver { AjancyID = Public.ActiveAjancy.AjancyID, DriverCertificationCar = driverCertificationCar, MembershipDate = DateTime.Now });
                }
            }

            // --------- setting other values  

            person.SubmitDate = DateTime.Now;
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
            //person.FormerJob = this.txtFormerJob.Text.Trim();
            //person.FormerJobType = Public.ToByte(this.drpFormerJobType.SelectedValue);
            plateNumber.TwoDigits = this.txtCarPlateNumber_1.Text.Trim();
            plateNumber.Alphabet = this.drpCarPlateNumber.SelectedValue;
            plateNumber.ThreeDigits = this.txtCarPlateNumber_2.Text.Trim();
            plateNumber.RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim();

            drivingLicense.DrivingLicenseNo = this.txtDrivingLicenseNo.Text.Trim();
            drivingLicense.ExportDate = this.txtDrivingLicenseDate.GeorgianDate.Value;
            drivingLicense.ExportPlace = this.txtDrivingLicensePlace.Text.Trim();
            drivingLicense.Type = Public.ToByte(this.drpDrivingLicenseType.SelectedValue);

            //if (!string.IsNullOrEmpty(this.txtDriverCertificationNo.Text))
            //{
            //    if (db.DriverCertifications.Any<Ajancy.DriverCertification>(dc => dc.DriverCertificationNo == this.txtDriverCertificationNo.Text))
            //    {
            //        this.lblMessage.Text = "شماره دفترچه صلاحیت تکراری میباشد";
            //        return;
            //    }
            //}
            driverCertification.DriverCertificationNo = string.IsNullOrEmpty(this.txtDriverCertificationNo.Text) ? null : this.txtDriverCertificationNo.Text.Trim();

            car.CarTypeID = Public.ToByte(this.drpCarType.SelectedValue);
            car.Model = this.txtCarModel.Text.Trim();
            car.Color = this.txtCarColor.Text.Trim();
            car.EngineNo = this.txtCarEngineNo.Text.Trim();
            car.ChassisNo = this.txtCarChassisNo.Text.Trim();
            car.GASProvider = Public.ToByte(this.drpGASProvider.SelectedValue);
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

            try
            {
                db.SubmitChanges();

                if (Request.QueryString["id"] == null)
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=12");
                }
                else // edit mode
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=7");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("DrivingLicenseNo"))
                {
                    this.lblMessage.Text = "شماره گواهینامه رانندگی تکراری میباشد";
                }
                else
                {
                    throw ex;
                }
            }
        }
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    if (this.Page.IsValid)
    //    {
    //        db = new Ajancy.Kimia_Ajancy(Public.CONNECTIONSTRING);
    //        dlo = new DataLoadOptions();
    //        dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
    //        dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
    //        dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
    //        dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
    //        db.LoadOptions = dlo;

    //        Ajancy.Car car = db.Cars.FirstOrDefault<Ajancy.Car>(c => c.CarTypeID == Public.ToByte(this.drpCarType.SelectedValue) &&
    //                                                                                                     c.EngineNo == this.txtCarEngineNo.Text.Trim() &&
    //                                                                                                     c.ChassisNo == this.txtCarChassisNo.Text.Trim());
    //        if (car != null)
    //        {
    //            foreach (var cpn in car.CarPlateNumbers)
    //            {
    //                foreach (var dcc in cpn.DriverCertificationCars)
    //                {
    //                    if (dcc.LockOutDate == null)
    //                    {
    //                        this.lblMessage.Text = "خودرو مورد نظر با این شماره موتور و شماره شاسی متعلق به شخص دیگری میباشد";
    //                        return;
    //                    }
    //                }
    //            }
    //        }

    //        if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text.Trim()))
    //        {
    //            this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلا برای خودرو دیگری ثبت شده";
    //            return;
    //        }

    //        Ajancy.PlateNumber plateNumber = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
    //                                                                                                                                                                 pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
    //                                                                                                                                                                 pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
    //                                                                                                                                                                 pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim());
    //        if (plateNumber != null)
    //        {
    //            foreach (Ajancy.CarPlateNumber cpn in plateNumber.CarPlateNumbers)
    //            {
    //                foreach (var dcc in cpn.DriverCertificationCars)
    //                {
    //                    if (dcc.LockOutDate == null)
    //                    {
    //                        this.lblMessage.Text = "شماره پلاک وارد شده متعلق به شخص دیگری میباشد";
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            plateNumber = new Ajancy.PlateNumber
    //            {
    //                TwoDigits = this.txtCarPlateNumber_1.Text.Trim(),
    //                Alphabet = this.drpCarPlateNumber.SelectedValue,
    //                ThreeDigits = this.txtCarPlateNumber_2.Text.Trim(),
    //                RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim(),
    //            };
    //        }

    //        Ajancy.Person person = new Ajancy.Person();
    //        if (this.ViewState["PersonID"] == null)
    //        {
    //            Ajancy.User user = new Ajancy.User();
    //            user.UserName = this.txtNationalCode.Text.Trim();
    //            user.ProvinceID = Public.ActiveUserRole.User.ProvinceID;
    //            user.CityID = Public.ActiveUserRole.User.CityID;
    //            user.SubmitDate = DateTime.Now;
    //            user.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
    //            person.User = user;
    //            person.NationalCode = this.txtNationalCode.Text.Trim();
    //            person.SubmitDate = DateTime.Now;
    //            db.Persons.InsertOnSubmit(person);
    //        }
    //        else
    //        {
    //            person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));
    //            person.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.TaxiDriver, MembershipDate = DateTime.Now });
    //        }

    //        #region FormerCar
    //        //Ajancy.Car formerCar = new Ajancy.Car();
    //        //Ajancy.FuelCard formerFuelCard = new Ajancy.FuelCard();
    //        //if (HasFormerCar())
    //        //{
    //        //    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFormerCarPAN.Text.Trim()))
    //        //    {
    //        //        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلی مربوط به دفترچه صلاحیت برای خودرو دیگری ثبت شده";
    //        //        return;
    //        //    }

    //        //    formerCar = db.Cars.FirstOrDefault<Ajancy.Car>(c => c.CarTypeID == Public.ToByte(this.drpFormerCarType.SelectedValue) &&
    //        //                                                                            c.EngineNo == this.txtFormerCarEngineNo.Text.Trim() &&
    //        //                                                                            c.ChassisNo == this.txtFormerCarChassisNo.Text.Trim());
    //        //    if (formerCar == null)
    //        //    {
    //        //        formerCar = new Ajancy.Car();
    //        //        formerCar.CarTypeID = Public.ToByte(this.drpFormerCarType.SelectedValue);
    //        //        formerCar.Model = this.txtFormerCarModel.Text.Trim();
    //        //        formerCar.EngineNo = this.txtFormerCarEngineNo.Text.Trim();
    //        //        formerCar.ChassisNo = this.txtFormerCarChassisNo.Text.Trim();
    //        //        formerCar.VIN = this.txtFormerCarVIN.Text.Trim();
    //        //        formerCar.FuelType = Public.ToByte(this.drpFormerCarFuelType.SelectedValue);
    //        //        db.Cars.InsertOnSubmit(formerCar);
    //        //    }

    //        //    formerFuelCard.CardType = Public.ToByte(this.drpFormerCarFuelCardType.SelectedValue);
    //        //    formerFuelCard.PAN = this.txtFormerCarPAN.Text.Trim();
    //        //    formerFuelCard.SubmitDate = DateTime.Now;
    //        //    formerFuelCard.DiscardDate = DateTime.Now;
    //        //    formerCar.FuelCards.Add(formerFuelCard);
    //        //}
    //        #endregion

    //        person.FirstName = this.txtFirstName.Text.Trim();
    //        person.LastName = this.txtLastName.Text.Trim();
    //        person.Father = this.txtFather.Text.Trim();
    //        person.BirthCertificateNo = this.txtBirthCertificateNo.Text.Trim();
    //        person.BirthCertificateSerial = this.txtBirthCertificateSerial.Text.Trim();
    //        person.BirthCertificateSerie = this.txtBirthCertificateSerie.Text.Trim();
    //        person.BirthCertificateAlfa = this.drpBirthCertificateAlfa.SelectedValue;
    //        person.Gender = Public.ToByte(this.drpGender.SelectedValue);
    //        person.Marriage = Public.ToByte(this.drpMarriage.SelectedValue);
    //        person.BirthDate = this.txtBirthDate.GeorgianDate.Value;
    //        person.BirthPlace = this.txtBirthPlace.Text.Trim();
    //        person.BirthCertificatePlace = this.txtBirthCertificatePlace.Text.Trim();
    //        person.FamilyMembersCount = this.txtFamilyMembersCount.Text.Trim();
    //        person.Education = Public.ToByte(this.drpEducation.SelectedValue);
    //        person.MilitaryService = Public.ToByte(this.drpMilitaryService.SelectedValue);
    //        person.Religion = Public.ToByte(this.drpReligion.SelectedValue);
    //        person.Subreligion = this.txtSubreligion.Text.Trim();
    //        person.JobStatus = Public.ToByte(this.drpJobStatus.SelectedValue);
    //        person.Phone = this.txtPhone.Text.Trim();
    //        person.Mobile = this.txtMobile.Text.Trim();
    //        person.PostalCode = this.txtPostalCode.Text.Trim();
    //        person.Address = this.txtAddress.Text.Trim();
    //        person.TraceCode = Public.GetTraceKey();
    //        person.FormerJob = this.txtFormerJob.Text.Trim();
    //        person.FormerJobType = Public.ToByte(this.drpFormerJobType.SelectedValue);

    //        Ajancy.DrivingLicense drivingLicense = new Ajancy.DrivingLicense
    //        {
    //            DrivingLicenseNo = this.txtDrivingLicenseNo.Text.Trim(),
    //            ExportDate = this.txtDrivingLicenseDate.GeorgianDate.Value,
    //            ExportPlace = this.txtDrivingLicensePlace.Text.Trim(),
    //            Type = Public.ToByte(this.drpDrivingLicenseType.SelectedValue)
    //        };
    //        person.DrivingLicenses.Add(drivingLicense);

    //        car = new Ajancy.Car
    //            {
    //                CarTypeID = Public.ToByte(this.drpCarType.SelectedValue),
    //                Model = this.txtCarModel.Text.Trim(),
    //                Color = this.txtCarColor.Text.Trim(),
    //                EngineNo = this.txtCarEngineNo.Text.Trim(),
    //                ChassisNo = this.txtCarChassisNo.Text.Trim(),
    //                GASProvider = Public.ToByte(this.drpGASProvider.SelectedValue),
    //                FuelType = Public.ToByte(this.drpFuelType.SelectedValue)
    //            };

    //        Ajancy.FuelCard fuelCard = new Ajancy.FuelCard
    //        {
    //            CardType = Public.ToByte(this.drpFuelCardType.SelectedValue),
    //            PAN = this.txtFuelCardPAN.Text.Trim(),
    //            SubmitDate = DateTime.Now
    //        };
    //        car.FuelCards.Add(fuelCard);
    //        db.Cars.InsertOnSubmit(car);

    //        Ajancy.DriverCertification driverCertification = new Ajancy.DriverCertification
    //        {
    //            DriverCertificationNo = string.IsNullOrEmpty(this.txtDriverCertificationNo.Text.Trim()) ? null : this.txtDriverCertificationNo.Text.Trim(),
    //            CertificationType = Public.ActiveAjancy.AjancyType,
    //            SubmitDate = DateTime.Now
    //        };

    //        //if (HasFormerCar())
    //        //{
    //        //    driverCertification.DriverCertificationCars.Add(new Ajancy.DriverCertificationCar { CarPlateNumber = new Ajancy.CarPlateNumber { Car = formerCar, Person = person }, SubmitDate = DateTime.Now, LockOutDate = DateTime.Now });
    //        //}

    //        Ajancy.CarPlateNumber carPlateNumber = new Ajancy.CarPlateNumber { Car = car, PlateNumber = plateNumber, VIN = this.txtCarVIN.Text.Trim(), Person = person };
    //        Ajancy.DriverCertificationCar driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };
    //        driverCertification.DriverCertificationCars.Add(driverCertificationCar);
    //        db.AjancyDrivers.InsertOnSubmit(new Ajancy.AjancyDriver { AjancyID = Public.ActiveAjancy.AjancyID, DriverCertificationCar = driverCertificationCar, MembershipDate = DateTime.Now });
    //        person.DriverCertifications.Add(driverCertification);

    //        // Sets the owner of the car 
    //        Ajancy.Person carOwner = null;
    //        if (!string.IsNullOrEmpty(this.txtOwnerName.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerFamily.Text.Trim()) && !string.IsNullOrEmpty(this.txtOwnerNationalCode.Text.Trim()) && !this.txtOwnerNationalCode.Text.Trim().Equals(person.NationalCode))
    //        {
    //            carOwner = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode.Text.Trim());
    //            if (carOwner == null)
    //            {
    //                carOwner = new Ajancy.Person { NationalCode = this.txtOwnerNationalCode.Text.Trim(), FirstName = this.txtOwnerName.Text.Trim(), LastName = this.txtOwnerFamily.Text.Trim(), SubmitDate = DateTime.Now, TraceCode = Public.GetTraceKey() };
    //                Ajancy.User userOwner = new Ajancy.User();
    //                userOwner.UserName = this.txtOwnerNationalCode.Text.Trim();
    //                if ((short)Public.ActiveUserRole.RoleID == (short)Public.Role.AjancyManager)
    //                {
    //                    userOwner.ProvinceID = Public.ActiveUserRole.User.ProvinceID;
    //                    userOwner.CityID = Public.ActiveUserRole.User.CityID;
    //                }
    //                else
    //                {
    //                    userOwner.ProvinceID = person.User.ProvinceID;
    //                    userOwner.CityID = person.User.CityID;
    //                }
    //                userOwner.SubmitDate = DateTime.Now;
    //                userOwner.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
    //                carOwner.User = userOwner;
    //                db.Persons.InsertOnSubmit(carOwner);

    //            }
    //            else
    //            {
    //                if (!carOwner.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
    //                {
    //                    carOwner.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
    //                }
    //            }
    //            person.CarPlateNumbers.Remove(carPlateNumber);
    //            carOwner.CarPlateNumbers.Add(carPlateNumber);
    //        }

    //        try
    //        {
    //            db.SubmitChanges();
    //            Response.Redirect("~/Message.aspx?mode=12");
    //        }
    //        catch (Exception ex)
    //        {
    //            if (ex.Message.Contains("DrivingLicenseNo"))
    //            {
    //                this.lblMessage.Text = "شماره گواهینامه رانندگی تکراری میباشد";
    //            }
    //            else
    //            {
    //                throw ex;
    //            }
    //        }
    //    }
    //}

    //protected void btnEdit_Click(object sender, EventArgs e)
    //{
    //    if (this.Page.IsValid)
    //    {
    //        db = new Ajancy.Kimia_Ajancy(Public.CONNECTIONSTRING);
    //        dlo = new DataLoadOptions();
    //        dlo.LoadWith<Ajancy.Person>(p => p.DrivingLicenses);
    //        dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
    //        dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
    //        dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
    //        dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.PlateNumber);
    //        dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
    //        dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
    //        db.LoadOptions = dlo;

    //        Ajancy.Person person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == Public.ToInt(this.ViewState["PersonID"]));
    //        Ajancy.DrivingLicense drivingLicense = person.DrivingLicenses.FirstOrDefault<Ajancy.DrivingLicense>();
    //        if (drivingLicense == null)
    //        {
    //            drivingLicense = new Ajancy.DrivingLicense();
    //        }
    //        drivingLicense.DrivingLicenseNo = this.txtDrivingLicenseNo.Text.Trim();
    //        drivingLicense.ExportDate = this.txtDrivingLicenseDate.GeorgianDate.Value;
    //        drivingLicense.ExportPlace = this.txtDrivingLicensePlace.Text.Trim();
    //        drivingLicense.Type = Public.ToByte(this.drpDrivingLicenseType.SelectedValue);

    //        Ajancy.DriverCertification driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
    //        driverCertification.DriverCertificationNo = string.IsNullOrEmpty(this.txtDriverCertificationNo.Text) ? null : this.txtDriverCertificationNo.Text.Trim();

    //        Ajancy.Car car = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null).CarPlateNumber.Car;
    //        car.CarTypeID = Public.ToShort(this.drpCarType.SelectedValue);
    //        car.Model = this.txtCarModel.Text.Trim();
    //        car.Color = this.txtCarColor.Text.Trim();
    //        car.EngineNo = this.txtCarEngineNo.Text.Trim();
    //        car.ChassisNo = this.txtCarChassisNo.Text.Trim();
    //        car.GASProvider = Public.ToByte(this.drpGASProvider.SelectedValue);
    //        car.FuelType = Public.ToByte(this.drpFuelType.SelectedValue);

    //        Ajancy.FuelCard fuelCard = car.FuelCards.Last<Ajancy.FuelCard>();
    //        fuelCard.CardType = Public.ToByte(this.drpFuelCardType.SelectedValue);
    //        fuelCard.PAN = this.txtFuelCardPAN.Text.Trim();

    //        Ajancy.CarPlateNumber carPlateNumber = car.CarPlateNumbers.Last<Ajancy.CarPlateNumber>();
    //        carPlateNumber.VIN = this.txtCarVIN.Text.Trim();
    //        carPlateNumber.OwnerPersonID = person.PersonID;
    //        Ajancy.PlateNumber plateNumber = carPlateNumber.PlateNumber;
    //        plateNumber.TwoDigits = this.txtCarPlateNumber_1.Text.Trim();
    //        plateNumber.Alphabet = this.drpCarPlateNumber.SelectedValue;
    //        plateNumber.ThreeDigits = this.txtCarPlateNumber_2.Text.Trim();
    //        plateNumber.RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim();

    //        #region FormerCar
    //        //Ajancy.DriverCertificationCar formerDcc = driverCertification.DriverCertificationCars.SingleOrDefault<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate != null && dcc.CarPlateNumber.PlateNumberID == null);
    //        //if (formerDcc != null)
    //        //{
    //        //    if (HasFormerCar()) // Edit
    //        //    {
    //        //        Ajancy.Car formerCar = formerDcc.CarPlateNumber.Car;
    //        //        Ajancy.FuelCard formerFuelCard = formerCar.FuelCards.Last<Ajancy.FuelCard>();
    //        //        if (formerCar != null)// PAN is modified
    //        //        {
    //        //            if (formerFuelCard.PAN != this.txtFormerCarPAN.Text && db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFormerCarPAN.Text))
    //        //            {
    //        //                this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلی مربوط به دفترچه صلاحیت برای خودرو دیگری ثبت شده";
    //        //                return;
    //        //            }

    //        //            formerCar.CarTypeID = Public.ToByte(this.drpFormerCarType.SelectedValue);
    //        //            formerCar.Model = this.txtFormerCarModel.Text.Trim();
    //        //            formerCar.EngineNo = this.txtFormerCarEngineNo.Text.Trim();
    //        //            formerCar.ChassisNo = this.txtFormerCarChassisNo.Text.Trim();
    //        //            formerCar.VIN = this.txtFormerCarVIN.Text.Trim();
    //        //            formerCar.FuelType = Public.ToByte(this.drpFormerCarFuelType.SelectedValue);
    //        //            formerFuelCard.CardType = Public.ToByte(this.drpFormerCarFuelCardType.SelectedValue);
    //        //            formerFuelCard.PAN = this.txtFormerCarPAN.Text.Trim();
    //        //        }
    //        //    }
    //        //    else // Delete
    //        //    {
    //        //        db.DriverCertificationCars.DeleteOnSubmit(formerDcc);
    //        //        db.CarPlateNumbers.DeleteOnSubmit(formerDcc.CarPlateNumber);
    //        //        db.FuelCards.DeleteOnSubmit(formerDcc.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());
    //        //        db.Cars.DeleteOnSubmit(formerDcc.CarPlateNumber.Car);
    //        //    }
    //        //}
    //        //else if (HasFormerCar()) // Add
    //        //{
    //        //    if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFormerCarPAN.Text))
    //        //    {
    //        //        this.lblMessage.Text = "شماره PAN کارت سوخت خودرو قبلی مربوط به دفترچه صلاحیت برای خودرو دیگری ثبت شده";
    //        //        return;
    //        //    }

    //        //    Ajancy.Car formerCar = db.Cars.FirstOrDefault<Ajancy.Car>(c => c.CarTypeID == Public.ToByte(this.drpFormerCarType.SelectedValue) &&
    //        //                                                                                            c.EngineNo == this.txtFormerCarEngineNo.Text.Trim() &&
    //        //                                                                                            c.ChassisNo == this.txtFormerCarChassisNo.Text.Trim());
    //        //    if (formerCar == null)
    //        //    {
    //        //        formerCar = new Ajancy.Car();
    //        //        formerCar.CarTypeID = Public.ToByte(this.drpFormerCarType.SelectedValue);
    //        //        formerCar.Model = this.txtFormerCarModel.Text.Trim();
    //        //        formerCar.EngineNo = this.txtFormerCarEngineNo.Text.Trim();
    //        //        formerCar.ChassisNo = this.txtFormerCarChassisNo.Text.Trim();
    //        //        formerCar.VIN = this.txtFormerCarVIN.Text.Trim();
    //        //        formerCar.FuelType = Public.ToByte(this.drpFormerCarFuelType.SelectedValue);
    //        //        db.Cars.InsertOnSubmit(formerCar);
    //        //    }

    //        //    Ajancy.FuelCard formerFuelCard = new Ajancy.FuelCard();
    //        //    formerFuelCard.CardType = Public.ToByte(this.drpFormerCarFuelCardType.SelectedValue);
    //        //    formerFuelCard.PAN = this.txtFormerCarPAN.Text.Trim();
    //        //    formerFuelCard.SubmitDate = DateTime.Now;
    //        //    formerFuelCard.DiscardDate = DateTime.Now;
    //        //    formerCar.FuelCards.Add(formerFuelCard);
    //        //    driverCertification.DriverCertificationCars.Add(new Ajancy.DriverCertificationCar { CarPlateNumber = new Ajancy.CarPlateNumber { Car = formerCar, Person = person }, LockOutDate = DateTime.Now, SubmitDate = DateTime.Now });
    //        //}
    //        #endregion

    //        person.FirstName = this.txtFirstName.Text.Trim();
    //        person.LastName = this.txtLastName.Text.Trim();
    //        person.Father = this.txtFather.Text.Trim();
    //        person.BirthCertificateNo = this.txtBirthCertificateNo.Text.Trim();
    //        person.BirthCertificateSerial = this.txtBirthCertificateSerial.Text.Trim();
    //        person.BirthCertificateSerie = this.txtBirthCertificateSerie.Text.Trim();
    //        person.BirthCertificateAlfa = this.drpBirthCertificateAlfa.SelectedValue;
    //        person.Gender = Public.ToByte(this.drpGender.SelectedValue);
    //        person.Marriage = Public.ToByte(this.drpMarriage.SelectedValue);
    //        person.BirthDate = this.txtBirthDate.GeorgianDate.Value;
    //        person.BirthPlace = this.txtBirthPlace.Text.Trim();
    //        person.BirthCertificatePlace = this.txtBirthCertificatePlace.Text.Trim();
    //        person.FamilyMembersCount = this.txtFamilyMembersCount.Text.Trim();
    //        person.Education = Public.ToByte(this.drpEducation.SelectedValue);
    //        person.MilitaryService = Public.ToByte(this.drpMilitaryService.SelectedValue);
    //        person.Religion = Public.ToByte(this.drpReligion.SelectedValue);
    //        person.Subreligion = this.txtSubreligion.Text.Trim();
    //        person.JobStatus = Public.ToByte(this.drpJobStatus.SelectedValue);
    //        person.Phone = this.txtPhone.Text.Trim();
    //        person.Mobile = this.txtMobile.Text.Trim();
    //        person.PostalCode = this.txtPostalCode.Text.Trim();
    //        person.Address = this.txtAddress.Text.Trim();
    //        person.FormerJob = this.txtFormerJob.Text.Trim();
    //        person.FormerJobType = Public.ToByte(this.drpFormerJobType.SelectedValue);

    //        // Sets the owner of the car 
    //        Ajancy.Person carOwner = null;
    //        if (!string.IsNullOrEmpty(this.txtOwnerName.Text) && !string.IsNullOrEmpty(this.txtOwnerFamily.Text) && !string.IsNullOrEmpty(this.txtOwnerNationalCode.Text) && !this.txtOwnerNationalCode.Text.Equals(person.NationalCode))
    //        {
    //            carOwner = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.NationalCode == this.txtOwnerNationalCode.Text.Trim());
    //            if (carOwner == null)
    //            {
    //                carOwner = new Ajancy.Person { NationalCode = this.txtOwnerNationalCode.Text.Trim(), FirstName = this.txtOwnerName.Text.Trim(), LastName = this.txtOwnerFamily.Text.Trim(), SubmitDate = DateTime.Now, TraceCode = Public.GetTraceKey() };
    //                Ajancy.User userOwner = new Ajancy.User();
    //                userOwner.UserName = this.txtOwnerNationalCode.Text.Trim();
    //                userOwner.ProvinceID = person.User.ProvinceID;
    //                userOwner.CityID = person.User.CityID;
    //                userOwner.SubmitDate = DateTime.Now;
    //                userOwner.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
    //                carOwner.User = userOwner;
    //                db.Persons.InsertOnSubmit(carOwner);
    //            }
    //            else
    //            {
    //                if (!carOwner.User.UsersInRoles.Any<Ajancy.UsersInRole>(ur => ur.RoleID == (short)Public.Role.CarOwner))
    //                {
    //                    carOwner.User.UsersInRoles.Add(new Ajancy.UsersInRole { RoleID = (short)Public.Role.CarOwner, MembershipDate = DateTime.Now, LockOutDate = DateTime.Now });
    //                }
    //            }
    //            carOwner.CarPlateNumbers.Add(carPlateNumber);
    //            person.CarPlateNumbers.Remove(carPlateNumber);
    //        }

    //        try
    //        {
    //            db.SubmitChanges();
    //            Response.Redirect("~/Message.aspx?mode=7");
    //        }
    //        catch (Exception ex)
    //        {
    //            if (ex.Message.Contains("DrivingLicenseNo"))
    //            {
    //                this.lblMessage.Text = "شماره گواهینامه رانندگی تکراری میباشد";
    //            }
    //            else
    //            {
    //                throw ex;
    //            }
    //        }
    //    }
    //}

    private void SetPerson(Ajancy.Person person)
    {
        if (person == null)
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
            SetDrivingLicense(null);
            SetCarOwner(null);
            SetCar(null);
            SetFuelCard(null);
            //SetFormerCar(null);
        }
        else
        {
            this.ViewState["PersonID"] = person.PersonID;
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
            //this.drpFormerJobType.SelectedValue = person.FormerJobType.GetValueOrDefault().ToString();
            //this.txtFormerJob.Text = person.FormerJob;
            if (person.DriverCertifications.Count > 0)
            {
                Ajancy.DriverCertification driverCertification = person.DriverCertifications.SingleOrDefault<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy);
                Ajancy.DriverCertificationCar driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>();
                SetDrivingLicense(person.DrivingLicenses.LastOrDefault<Ajancy.DrivingLicense>());
                SetCarOwner(driverCertificationCar.CarPlateNumber.Person);
                SetCar(driverCertificationCar.CarPlateNumber.Car);
                SetFuelCard(driverCertificationCar.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());
                this.txtDriverCertificationNo.Text = driverCertification.DriverCertificationNo;
                this.drpDriverCertificationNo.SelectedIndex = string.IsNullOrEmpty(driverCertification.DriverCertificationNo) ? 1 : 0;
                //Ajancy.DriverCertificationCar formerDcc = driverCertification.DriverCertificationCars.SingleOrDefault<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate != null && dcc.CarPlateNumber.PlateNumberID == null);
                //if (formerDcc != null)
                //{
                //    SetFormerCar(formerDcc.CarPlateNumber.Car);
                //}
            }
        }
    }

    private void SetDrivingLicense(Ajancy.DrivingLicense drivingLicense)
    {
        if (drivingLicense == null)
        {
            this.txtDrivingLicenseNo.Text = null;
            this.txtDrivingLicenseDate.Text = null;
            this.txtDrivingLicensePlace.Text = null;
            this.drpDrivingLicenseType.SelectedIndex = 0;
        }
        else
        {
            this.txtDrivingLicenseNo.Text = drivingLicense.DrivingLicenseNo;
            this.txtDrivingLicenseDate.SetDate(drivingLicense.ExportDate);
            this.txtDrivingLicensePlace.Text = drivingLicense.ExportPlace;
            this.drpDrivingLicenseType.SelectedValue = drivingLicense.Type.ToString();
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
            this.txtCarColor.Text = null;
            this.txtCarEngineNo.Text = null;
            this.txtCarChassisNo.Text = null;
            this.drpFuelType.SelectedIndex = 0;
            this.drpGASProvider.SelectedIndex = 0;
            SetPlateNumber(null);
        }
        else
        {
            this.drpCarType.SelectedValue = car.CarTypeID.ToString();
            this.txtCarVIN.Text = car.VIN;
            this.txtCarModel.Text = car.Model;
            this.txtCarColor.Text = car.Color;
            this.txtCarEngineNo.Text = car.EngineNo;
            this.txtCarChassisNo.Text = car.ChassisNo;
            this.drpFuelType.SelectedValue = car.FuelType.ToString();
            this.drpGASProvider.SelectedValue = car.GASProvider.GetValueOrDefault().ToString();
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

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }

    #region FormerCar
    //private void SetFormerCar(Ajancy.Car formerCar)
    //{
    //    if (formerCar == null)
    //    {
    //        this.drpFormerCarType.SelectedIndex = 0;
    //        this.txtFormerCarModel.Text = null;
    //        this.txtFormerCarEngineNo.Text = null;
    //        this.txtFormerCarChassisNo.Text = null;
    //        this.txtFormerCarVIN.Text = null;
    //        this.drpFormerCarFuelType.SelectedIndex = 0;
    //        SetFormerCarFuelCard(null);
    //    }
    //    else
    //    {
    //        this.drpFormerCarType.SelectedValue = formerCar.CarTypeID.ToString();
    //        this.txtFormerCarModel.Text = formerCar.Model;
    //        this.txtFormerCarEngineNo.Text = formerCar.EngineNo;
    //        this.txtFormerCarChassisNo.Text = formerCar.ChassisNo;
    //        this.txtFormerCarVIN.Text = formerCar.VIN;
    //        this.drpFormerCarFuelType.SelectedValue = formerCar.FuelType.ToString();
    //        SetFormerCarFuelCard(formerCar.FuelCards[0]);
    //    }
    //}

    //private void SetFormerCarFuelCard(Ajancy.FuelCard fuelCard)
    //{
    //    if (fuelCard == null)
    //    {
    //        this.txtFormerCarPAN.Text = null;
    //        this.drpFormerCarFuelCardType.SelectedIndex = 0;
    //    }
    //    else
    //    {
    //        this.txtFormerCarPAN.Text = fuelCard.PAN;
    //        this.drpFormerCarFuelCardType.SelectedValue = fuelCard.CardType.ToString();
    //    }
    //}

    //private bool HasFormerCar()
    //{
    //    return (this.drpFormerCarType.SelectedIndex > 0 &&
    //                this.drpFormerCarFuelType.SelectedIndex > 0 &&
    //                this.drpFormerCarFuelCardType.SelectedIndex > 0 &&
    //                !string.IsNullOrEmpty(this.txtFormerCarModel.Text) &&
    //                !string.IsNullOrEmpty(this.txtFormerCarChassisNo.Text) &&
    //                !string.IsNullOrEmpty(this.txtFormerCarEngineNo.Text) &&
    //                !string.IsNullOrEmpty(this.txtFormerCarVIN.Text) &&
    //                !string.IsNullOrEmpty(this.txtFormerCarPAN.Text));
    //}
    #endregion
}

