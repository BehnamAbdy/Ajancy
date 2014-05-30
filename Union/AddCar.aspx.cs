using System;
using System.Linq;
using System.Data.Linq;

public partial class Union_AddCar : System.Web.UI.Page
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
        db.Dispose();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var driver = (from p in db.Persons
                          join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                          join u in db.Users on p.PersonID equals u.PersonID
                          where p.NationalCode == this.txtNationalCode.Text.Trim() && dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy
                          select
                          new
                          {
                              p.FirstName,
                              p.LastName,
                              p.BirthCertificateNo,
                              p.Father,
                              u.ProvinceID,
                              u.CityID,
                              dc.DriverCertificationID
                          }).FirstOrDefault();

            if (driver != null)
            {
                if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager && driver.ProvinceID != Public.ActiveUserRole.User.ProvinceID) // Is ProvinceManager
                {
                    db.Dispose();
                    Response.Redirect("~/Message.aspx?mode=27"); // This is a driver of an other province
                }
                else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager && driver.CityID != Public.ActiveUserRole.User.CityID)
                {
                    db.Dispose();
                    Response.Redirect("~/Message.aspx?mode=26"); // This is a driver of an other city
                }

                this.LoadDriverCars(driver.DriverCertificationID, db);
                this.ViewState["DCID"] = driver.DriverCertificationID;
                this.lblFirstName.Text = driver.FirstName;
                this.lblLastName.Text = driver.LastName;
                this.lblFather.Text = driver.Father;
                this.lblBirthCertificateNo.Text = driver.BirthCertificateNo;
                this.btnSave.Enabled = true;

                if (this.drpCarType.Items.Count == 0)
                {
                    this.drpCarType.DataSource = db.CarTypes.OrderBy(ct => ct.TypeName);
                    this.drpCarType.DataBind();
                    this.drpCarType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
                }
                return;
            }
        }

        this.ClearControls(true);
        this.btnSave.Enabled = false;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid && this.ViewState["DCID"] != null)
        {
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Ajancy.PlateNumber>(pl => pl.CarPlateNumbers);
            dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.DriverCertificationCars);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.LoadOptions = dlo;

            if (db.Cars.Any<Ajancy.Car>(c => c.VIN == this.txtCarVIN.Text.Trim()))
            {
                this.lblMessage.Text = "خودرو این شماره VIN متعلق به شخص دیگری میباشد";
                return;
            }

            if (db.FuelCards.Any<Ajancy.FuelCard>(fc => fc.PAN == this.txtFuelCardPAN.Text))
            {
                this.lblMessage.Text = "شماره PAN کارت سوخت قبلا برای خودرو دیگری ثبت شده";
                return;
            }

            int driverCertificationId = Public.ToInt(this.ViewState["DCID"]);
            Ajancy.DriverCertification driverCertification = db.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.DriverCertificationID == driverCertificationId);
            Ajancy.DriverCertificationCar lastDCC = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>();
            Ajancy.PlateNumber pln = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pl => pl.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                                                                                                               pl.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                               pl.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                                                                                                               pl.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim());
            if (pln != null && pln.PlateNumberID != lastDCC.CarPlateNumber.PlateNumberID)
            {
                foreach (Ajancy.CarPlateNumber cpn in pln.CarPlateNumbers)
                {
                    foreach (Ajancy.DriverCertificationCar dcc in cpn.DriverCertificationCars)
                    {
                        if (dcc.LockOutDate == null)
                        {
                            this.lblMessage.Text = "شماره پلاک وارد شده متعلق به شخص دیگری میباشد";
                            return;
                        }
                    }
                }
            }

            Ajancy.Car car = new Ajancy.Car
            {
                CarTypeID = Public.ToShort(this.drpCarType.SelectedValue),
                FuelType = Public.ToByte(this.drpFuelType.SelectedValue),
                Model = this.txtCarModel.Text,
                VIN = this.txtCarVIN.Text.Trim().ToUpper()
            };
            car.FuelCards.Add(new Ajancy.FuelCard
            {
                CardType = (byte)Public.FuelCardType.Ajancy,
                PAN = this.txtFuelCardPAN.Text.Trim(),
                SubmitDate = DateTime.Now
            });

            Ajancy.CarPlateNumber carPlateNumber = new Ajancy.CarPlateNumber
            {
                OwnerPersonID = driverCertification.PersonID,
                Car = car,
                PlateNumber = pln != null ? pln : new Ajancy.PlateNumber
                {
                    TwoDigits = this.txtCarPlateNumber_1.Text.Trim(),
                    Alphabet = this.drpCarPlateNumber.SelectedValue,
                    ThreeDigits = this.txtCarPlateNumber_2.Text.Trim(),
                    RegionIdentifier = this.txtCarPlateNumber_3.Text.Trim()
                }
            };

            if (lastDCC.LockOutDate == null)
            {
                lastDCC.LockOutDate = DateTime.Now;
                lastDCC.AjancyDrivers.Last<Ajancy.AjancyDriver>().LockOutDate = DateTime.Now;
                lastDCC.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>().DiscardDate = DateTime.Now;
            }

            Ajancy.DriverCertificationCar driverCertificationCar = new Ajancy.DriverCertificationCar { CarPlateNumber = carPlateNumber, SubmitDate = DateTime.Now };
            driverCertificationCar.AjancyDrivers.Add(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), MembershipDate = DateTime.Now });
            driverCertification.DriverCertificationCars.Add(driverCertificationCar);
            db.SubmitChanges();
            this.lblMessage.Text = Public.SAVEMESSAGE;
            this.ClearControls(false);
            this.LoadDriverCars(driverCertificationId, db);
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

    private void LoadDriverCars(int driverCertificationId, Ajancy.Kimia_Ajancy db)
    {
        var cars = from dcc in db.DriverCertificationCars
                   join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                   from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty() 
                   from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty() 
                   join c in db.Cars on cpn.CarID equals c.CarID
                   join ct in db.CarTypes on c.CarTypeID equals ct.CarTypeID
                   where dcc.DriverCertificationID == driverCertificationId
                   select new
                   {
                       dcc.DriverCertificationCarID,
                       ct.TypeName,
                       IsZoneType = cpn.ZonePlateNumberID.HasValue,
                       ZCity = zpn.City.Name,
                       ZNumber = zpn.Number,
                       pn.TwoDigits,
                       pn.ThreeDigits,
                       pn.Alphabet,
                       pn.RegionIdentifier,
                       c.VIN,
                       DccLock = dcc.LockOutDate
                   };

        System.Collections.ArrayList list = new System.Collections.ArrayList();
        foreach (var item in cars)
        {
            var jdStatus = (from jd in db.AjancyDrivers
                            where jd.DriverCertificationCarID == item.DriverCertificationCarID
                            orderby jd.AjancyDriverID descending
                            select new { jd.LockOutDate }).Take(1);

            foreach (var jd in jdStatus)
            {
                list.Add(new
                {
                    item.TypeName,
                    item.IsZoneType,
                    item.ZCity,
                    item.ZNumber,
                    item.TwoDigits,
                    item.ThreeDigits,
                    item.Alphabet,
                    item.RegionIdentifier,
                    item.VIN,
                    Status = (item.DccLock == null && jd.LockOutDate == null) ? "فعال" : "غیرفعال"
                });
            }
        }
        this.lstCars.DataSource = list;
        this.lstCars.DataBind();
    }

    private void ClearControls(bool clearName)
    {
        this.ViewState["DCID"] = null;
        if (clearName)
        {
            this.lblFirstName.Text = null;
            this.lblLastName.Text = null;
            this.lblFather.Text = null;
            this.lblBirthCertificateNo.Text = null;
        }
        this.lstCars.Items.Clear();
        this.txtFuelCardPAN.Text = null;
        this.txtCarVIN.Text = null;
        this.txtCarModel.Text = null;
        this.drpCarType.SelectedIndex = 0;
        this.drpFuelType.SelectedIndex = 0;
        this.drpCarPlateNumber.SelectedIndex = 0;
        this.txtCarPlateNumber_3.Text = null;
        this.txtCarPlateNumber_1.Text = null;
        this.txtCarPlateNumber_2.Text = null;
        this.drpCity.SelectedIndex = 0;
        this.drpAjancies.SelectedIndex = 0;
    }
}