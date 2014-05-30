using System;
using System.Linq;
using System.Data;
using System.Web;

public partial class Reports_DriversReport : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    protected bool IsVisitor = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        short roleId = Public.ActiveUserRole.RoleID;
        this.IsVisitor = (short)Public.Role.Visitor == roleId;
        if (!IsPostBack)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

            int personId = 0;
            if (Request.QueryString["mode"] == "del" && int.TryParse(Request.QueryString["pId"], out personId)) // delete driver
            {
                Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.PersonID == personId);
                bool hasManyRoles = person.User.UsersInRoles.Count > 1;
                if (hasManyRoles)
                {
                    db.DriverCertifications.DeleteOnSubmit(person.DriverCertifications.First(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy));
                    db.UsersInRoles.DeleteOnSubmit(person.User.UsersInRoles.First<Ajancy.UsersInRole>(ur => ur.RoleID == (byte)Public.Role.TaxiDriver));
                }
                else
                {
                    db.Persons.DeleteOnSubmit(person);
                }

                var cpnList = from dc in db.DriverCertifications
                              join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                              join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                              where dc.PersonID == personId
                              select cpn;

                foreach (Ajancy.CarPlateNumber cpn in cpnList)
                {
                    foreach (Ajancy.FuelCard fc in cpn.Car.FuelCards)
                    {
                        db.FuelCardSubstitutions.DeleteAllOnSubmit(db.FuelCardSubstitutions.Where<Ajancy.FuelCardSubstitution>(fcs => fcs.PersonalTypeFuelCardID == fc.FuelCardID));
                    }
                    db.CarPlateNumbers.DeleteOnSubmit(cpn);
                    if (cpn.PlateNumber != null)
                    {
                        db.PlateNumbers.DeleteOnSubmit(cpn.PlateNumber);
                    }
                    if (cpn.ZonePlateNumber != null)
                    {
                        db.ZonePlateNumbers.DeleteOnSubmit(cpn.ZonePlateNumber);
                    }
                    db.Cars.DeleteOnSubmit(cpn.Car);
                }

                db.SubmitChanges();
                DisposeContext();
                Response.Clear();
                Response.Write("1");
                Response.End();
            }

            this.drpAjancies.DataSource = db.Ajancies.Where(aj => aj.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) && aj.AjancyName != null).OrderBy(j => j.AjancyName).Select(aj => new { aj.AjancyID, aj.AjancyName });
            this.drpAjancies.DataBind();
            this.drpAjancies.Items.Insert(0, "- همه آژانس ها -");
            this.drpCarType.DataSource = db.CarTypes.OrderBy(ct => ct.TypeName);
            this.drpCarType.DataBind();
            this.drpCarType.Items.Insert(0, "- همه موارد -");

            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();
            LoadAjancies();
            this.drpProvince.Enabled = Public.ActiveUserRole.RoleID == (short)Public.Role.Admin;

            switch (roleId)
            {
                case (short)Public.Role.Admin:
                case (short)Public.Role.ProvinceManager:
                case (short)Public.Role.AcademyProvince:
                    this.drpCity.Enabled = true;
                    break;

                case (short)Public.Role.Visitor:
                    this.btnExcel.Visible = false;
                    this.drpProvince.Enabled = true;
                    this.drpCity.Enabled = true;
                    break;
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
            this.drpCity.Items.Insert(0, "- همه موارد -");
        }
        else
        {
            this.drpCity.Items.Clear();
            this.drpCity.Items.Insert(0, "- همه موارد -");
        }
        LoadAjancies();
    }

    protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAjancies();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ObjectDataSource1.Select();
        this.lstDrivers.DataBind();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable dtObj = new DataTable();
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

        switch (this.drpStatus.SelectedIndex)
        {
            case 0: // All
                var query = (from p in db.Persons
                             join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                             join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                             from pln in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join c in db.Cars on cpn.CarID equals c.CarID
                             join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                             join fc in db.FuelCards on c.CarID equals fc.CarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             where j.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue)
                             orderby pv.Name, ct.Name
                             select new
                             {
                                 OWNationalCode = owp.NationalCode,
                                 OWFirstName = owp.FirstName,
                                 OWLastName = owp.LastName,
                                 p.PersonID,
                                 p.FirstName,
                                 p.LastName,
                                 p.Father,
                                 p.NationalCode,
                                 p.BirthCertificateNo,
                                 p.BirthCertificatePlace,
                                 p.BirthDate,
                                 p.Marriage,
                                 p.BirthPlace,
                                 p.Mobile,
                                 p.Phone,
                                 p.PostalCode,
                                 p.Address,
                                 p.Gender,
                                 p.SubmitDate,
                                 j.AjancyID,
                                 j.AjancyName,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 Province = pv.Name,
                                 City = ct.Name,
                                 dc.DriverCertificationNo,
                                 c.CarTypeID,
                                 c.FuelType,
                                 c.Model,
                                 c.EngineNo,
                                 c.ChassisNo,
                                 crt.TypeName,
                                 c.VIN,
                                 ZCityID = (short?)zpn.CityID,
                                 ZNumber = zpn.Number,
                                 pln.TwoDigits,
                                 pln.ThreeDigits,
                                 pln.Alphabet,
                                 pln.RegionIdentifier,
                                 fc.CardType,
                                 fc.PAN
                             }).Distinct();

                if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex == 0) // Just province
                {
                    byte provinceId = Public.ToByte(this.drpProvince.SelectedValue);
                    query = from q in query
                            where q.ProvinceID == provinceId
                            select q;
                }

                if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex > 0) // province and city
                {
                    short cityId = Public.ToShort(this.drpCity.SelectedValue);
                    query = from q in query
                            where q.CityID == cityId
                            select q;
                }

                if (this.drpAjancies.SelectedIndex > 0)
                {
                    int ajancyId = Public.ToInt(this.drpAjancies.SelectedValue);
                    query = from q in query
                            where q.AjancyID == ajancyId
                            select q;
                }

                if (this.drpDriverCertification.SelectedIndex == 1)
                {
                    query = from q in query
                            where q.DriverCertificationNo != null
                            select q;
                }
                else if (this.drpDriverCertification.SelectedIndex == 2)
                {
                    query = from q in query
                            where q.DriverCertificationNo == null
                            select q;
                }

                if (this.drpDrivingLicenseType.SelectedIndex > 0)
                {
                    byte drivingLicense = Public.ToByte(this.drpDrivingLicenseType.SelectedValue);
                    query = from q in query
                            join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                            where dl.Type == drivingLicense
                            select q;
                }

                if (this.drpMarriage.SelectedIndex > 0)
                {
                    byte marriage = Public.ToByte(this.drpMarriage.SelectedValue);
                    query = from q in query
                            where q.Marriage == marriage
                            select q;
                }

                if (this.drpCarType.SelectedIndex > 0)
                {
                    short carTypeId = Public.ToShort(this.drpCarType.SelectedValue);
                    query = from q in query
                            join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                            where q.CarTypeID == carTypeId
                            select q;
                }

                if (this.drpFuelType.SelectedIndex > 0)
                {
                    byte fuelType = Public.ToByte(this.drpFuelType.SelectedValue);
                    query = from q in query
                            where q.FuelType == fuelType
                            select q;
                }

                if (this.drpFuelCardType.SelectedIndex > 0)
                {
                    byte fuelCardType = Public.ToByte(this.drpFuelCardType.SelectedValue);
                    query = from q in query
                            where q.CardType == fuelCardType
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtFuelCardPAN.Text))
                {
                    query = from q in query
                            where q.PAN.Equals(this.txtFuelCardPAN.Text.Trim())
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtCarVIN.Text))
                {
                    query = from q in query
                            where q.VIN.Equals(this.txtCarVIN.Text.Trim())
                            select q;
                }

                if (this.drpGender.SelectedIndex > 0)
                {
                    byte gender = Public.ToByte(this.drpGender.SelectedValue);
                    query = from q in query
                            where q.Gender == gender
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtFirstName.Text))
                {
                    query = from q in query
                            where q.FirstName.Contains(this.txtFirstName.Text.Trim())
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtLastName.Text))
                {
                    query = from q in query
                            where q.LastName.Contains(this.txtLastName.Text.Trim())
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
                {
                    query = from q in query
                            where q.NationalCode == this.txtNationalCode.Text.Trim()
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtBirthCertificateNo.Text))
                {
                    query = from q in query
                            where q.BirthCertificateNo == this.txtBirthCertificateNo.Text.Trim()
                            select q;
                }

                if (!string.IsNullOrEmpty(this.txtCarPlateNumber_1.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_2.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_3.Text))
                {
                    query = from q in query
                            where q.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                     q.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                     q.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim() &&
                                     q.Alphabet == this.drpCarPlateNumber.SelectedValue
                            select q;
                }

                if (this.txtDateFrom.HasDate && !this.txtDateTo.HasDate)
                {
                    query = from q in query
                            where q.SubmitDate == this.txtDateFrom.GeorgianDate.Value
                            select q;
                }
                else if (this.txtDateFrom.HasDate && this.txtDateTo.HasDate)
                {
                    query = from q in query
                            where q.SubmitDate >= this.txtDateFrom.GeorgianDate.Value && q.SubmitDate <= this.txtDateTo.GeorgianDate.Value
                            select q;
                }

                db.Connection.Open();
                dtObj.Load(db.GetCommand(query).ExecuteReader());
                db.Connection.Close();
                db.Dispose();
                break;

            case 1: // Actives
                var query1 = (from p in db.Persons
                              join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                              join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                              join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                              join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                              from pln in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                              from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                              join c in db.Cars on cpn.CarID equals c.CarID
                              join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                              join fc in db.FuelCards on c.CarID equals fc.CarID
                              join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                              join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                              join ct in db.Cities on j.CityID equals ct.CityID
                              join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                              join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                              where j.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) &&
                                    jd.LockOutDate == null &&
                                    dcc.LockOutDate == null
                              orderby pv.Name, ct.Name
                              select new
                              {
                                  OWNationalCode = owp.NationalCode,
                                  OWFirstName = owp.FirstName,
                                  OWLastName = owp.LastName,
                                  p.PersonID,
                                  p.FirstName,
                                  p.LastName,
                                  p.Father,
                                  p.NationalCode,
                                  p.BirthCertificateNo,
                                  p.BirthCertificatePlace,
                                  p.BirthDate,
                                  p.Marriage,
                                  p.BirthPlace,
                                  p.Mobile,
                                  p.Phone,
                                  p.PostalCode,
                                  p.Address,
                                  p.Gender,
                                  p.SubmitDate,
                                  j.AjancyID,
                                  j.AjancyName,
                                  ct.CityID,
                                  ct.ProvinceID,
                                  Province = pv.Name,
                                  City = ct.Name,
                                  dc.DriverCertificationNo,
                                  c.CarTypeID,
                                  c.FuelType,
                                  c.Model,
                                  c.EngineNo,
                                  c.ChassisNo,
                                  crt.TypeName,
                                  c.VIN,
                                  ZCityID = (short?)zpn.CityID,
                                  ZNumber = zpn.Number,
                                  pln.TwoDigits,
                                  pln.ThreeDigits,
                                  pln.Alphabet,
                                  pln.RegionIdentifier,
                                  fc.CardType,
                                  fc.PAN
                              }).Distinct();

                if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex == 0) // Just province
                {
                    byte provinceId = Public.ToByte(this.drpProvince.SelectedValue);
                    query1 = from q in query1
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex > 0) // province and city
                {
                    short cityId = Public.ToShort(this.drpCity.SelectedValue);
                    query1 = from q in query1
                             where q.CityID == cityId
                             select q;
                }

                if (this.drpAjancies.SelectedIndex > 0)
                {
                    int ajancyId = Public.ToInt(this.drpAjancies.SelectedValue);
                    query1 = from q in query1
                             where q.AjancyID == ajancyId
                             select q;
                }

                if (this.drpDriverCertification.SelectedIndex == 1)
                {
                    query1 = from q in query1
                             where q.DriverCertificationNo != null
                             select q;
                }
                else if (this.drpDriverCertification.SelectedIndex == 2)
                {
                    query1 = from q in query1
                             where q.DriverCertificationNo == null
                             select q;
                }

                if (this.drpDrivingLicenseType.SelectedIndex > 0)
                {
                    byte drivingLicense = Public.ToByte(this.drpDrivingLicenseType.SelectedValue);
                    query1 = from q in query1
                             join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                             where dl.Type == drivingLicense
                             select q;
                }

                if (this.drpMarriage.SelectedIndex > 0)
                {
                    byte marriage = Public.ToByte(this.drpMarriage.SelectedValue);
                    query1 = from q in query1
                             where q.Marriage == marriage
                             select q;
                }

                if (this.drpCarType.SelectedIndex > 0)
                {
                    short carTypeId = Public.ToShort(this.drpCarType.SelectedValue);
                    query1 = from q in query1
                             join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                             where q.CarTypeID == carTypeId
                             select q;
                }

                if (this.drpFuelType.SelectedIndex > 0)
                {
                    byte fuelType = Public.ToByte(this.drpFuelType.SelectedValue);
                    query1 = from q in query1
                             where q.FuelType == fuelType
                             select q;
                }

                if (this.drpFuelCardType.SelectedIndex > 0)
                {
                    byte fuelCardType = Public.ToByte(this.drpFuelCardType.SelectedValue);
                    query1 = from q in query1
                             where q.CardType == fuelCardType
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtFuelCardPAN.Text))
                {
                    query1 = from q in query1
                             where q.PAN.Equals(this.txtFuelCardPAN.Text.Trim())
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtCarVIN.Text))
                {
                    query1 = from q in query1
                             where q.VIN.Equals(this.txtCarVIN.Text.Trim())
                             select q;
                }

                if (this.drpGender.SelectedIndex > 0)
                {
                    byte gender = Public.ToByte(this.drpGender.SelectedValue);
                    query1 = from q in query1
                             where q.Gender == gender
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtFirstName.Text))
                {
                    query1 = from q in query1
                             where q.FirstName.Contains(this.txtFirstName.Text.Trim())
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtLastName.Text))
                {
                    query1 = from q in query1
                             where q.LastName.Contains(this.txtLastName.Text.Trim())
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
                {
                    query1 = from q in query1
                             where q.NationalCode == this.txtNationalCode.Text.Trim()
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtBirthCertificateNo.Text))
                {
                    query1 = from q in query1
                             where q.BirthCertificateNo == this.txtBirthCertificateNo.Text.Trim()
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtCarPlateNumber_1.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_2.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_3.Text))
                {
                    query1 = from q in query1
                             where q.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                      q.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                      q.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim() &&
                                      q.Alphabet == this.drpCarPlateNumber.SelectedValue
                             select q;
                }

                if (this.txtDateFrom.HasDate && !this.txtDateTo.HasDate)
                {
                    query1 = from q in query1
                             where q.SubmitDate == this.txtDateFrom.GeorgianDate.Value
                             select q;
                }
                else if (this.txtDateFrom.HasDate && this.txtDateTo.HasDate)
                {
                    query1 = from q in query1
                             where q.SubmitDate >= this.txtDateFrom.GeorgianDate.Value && q.SubmitDate <= this.txtDateTo.GeorgianDate.Value
                             select q;
                }

                db.Connection.Open();
                dtObj.Load(db.GetCommand(query1).ExecuteReader());
                db.Connection.Close();
                db.Dispose();
                break;

            case 2: // Inactives
                var query2 = (from p in db.Persons
                              join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                              join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                              join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                              join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                              from pln in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                              from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                              join c in db.Cars on cpn.CarID equals c.CarID
                              join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                              join fc in db.FuelCards on c.CarID equals fc.CarID
                              join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                              join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                              join ct in db.Cities on j.CityID equals ct.CityID
                              join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                              join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                              where j.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) &&
                                    jd.LockOutDate != null &&
                                    dcc.LockOutDate != null
                              orderby pv.Name, ct.Name
                              select new
                              {
                                  OWNationalCode = owp.NationalCode,
                                  OWFirstName = owp.FirstName,
                                  OWLastName = owp.LastName,
                                  p.PersonID,
                                  p.FirstName,
                                  p.LastName,
                                  p.Father,
                                  p.NationalCode,
                                  p.BirthCertificateNo,
                                  p.BirthCertificatePlace,
                                  p.BirthDate,
                                  p.Marriage,
                                  p.BirthPlace,
                                  p.Mobile,
                                  p.Phone,
                                  p.PostalCode,
                                  p.Address,
                                  p.Gender,
                                  p.SubmitDate,
                                  j.AjancyID,
                                  j.AjancyName,
                                  ct.CityID,
                                  ct.ProvinceID,
                                  Province = pv.Name,
                                  City = ct.Name,
                                  dc.DriverCertificationNo,
                                  c.CarTypeID,
                                  c.FuelType,
                                  c.Model,
                                  c.EngineNo,
                                  c.ChassisNo,
                                  crt.TypeName,
                                  c.VIN,
                                  ZCityID = (short?)zpn.CityID,
                                  ZNumber = zpn.Number,
                                  pln.TwoDigits,
                                  pln.ThreeDigits,
                                  pln.Alphabet,
                                  pln.RegionIdentifier,
                                  fc.CardType,
                                  fc.PAN
                              }).Distinct();

                if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex == 0) // Just province
                {
                    byte provinceId = Public.ToByte(this.drpProvince.SelectedValue);
                    query2 = from q in query2
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex > 0) // province and city
                {
                    short cityId = Public.ToShort(this.drpCity.SelectedValue);
                    query2 = from q in query2
                             where q.CityID == cityId
                             select q;
                }

                if (this.drpAjancies.SelectedIndex > 0)
                {
                    int ajancyId = Public.ToInt(this.drpAjancies.SelectedValue);
                    query2 = from q in query2
                             where q.AjancyID == ajancyId
                             select q;
                }

                if (this.drpDriverCertification.SelectedIndex == 1)
                {
                    query2 = from q in query2
                             where q.DriverCertificationNo != null
                             select q;
                }
                else if (this.drpDriverCertification.SelectedIndex == 2)
                {
                    query2 = from q in query2
                             where q.DriverCertificationNo == null
                             select q;
                }

                if (this.drpDrivingLicenseType.SelectedIndex > 0)
                {
                    byte drivingLicense = Public.ToByte(this.drpDrivingLicenseType.SelectedValue);
                    query2 = from q in query2
                             join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                             where dl.Type == drivingLicense
                             select q;
                }

                if (this.drpMarriage.SelectedIndex > 0)
                {
                    byte marriage = Public.ToByte(this.drpMarriage.SelectedValue);
                    query2 = from q in query2
                             where q.Marriage == marriage
                             select q;
                }

                if (this.drpCarType.SelectedIndex > 0)
                {
                    short carTypeId = Public.ToShort(this.drpCarType.SelectedValue);
                    query2 = from q in query2
                             join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                             where q.CarTypeID == carTypeId
                             select q;
                }

                if (this.drpFuelType.SelectedIndex > 0)
                {
                    byte fuelType = Public.ToByte(this.drpFuelType.SelectedValue);
                    query2 = from q in query2
                             where q.FuelType == fuelType
                             select q;
                }

                if (this.drpFuelCardType.SelectedIndex > 0)
                {
                    byte fuelCardType = Public.ToByte(this.drpFuelCardType.SelectedValue);
                    query2 = from q in query2
                             where q.CardType == fuelCardType
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtFuelCardPAN.Text))
                {
                    query2 = from q in query2
                             where q.PAN.Equals(this.txtFuelCardPAN.Text.Trim())
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtCarVIN.Text))
                {
                    query2 = from q in query2
                             where q.VIN.Equals(this.txtCarVIN.Text.Trim())
                             select q;
                }

                if (this.drpGender.SelectedIndex > 0)
                {
                    byte gender = Public.ToByte(this.drpGender.SelectedValue);
                    query2 = from q in query2
                             where q.Gender == gender
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtFirstName.Text))
                {
                    query2 = from q in query2
                             where q.FirstName.Contains(this.txtFirstName.Text.Trim())
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtLastName.Text))
                {
                    query2 = from q in query2
                             where q.LastName.Contains(this.txtLastName.Text.Trim())
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
                {
                    query2 = from q in query2
                             where q.NationalCode == this.txtNationalCode.Text.Trim()
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtBirthCertificateNo.Text))
                {
                    query2 = from q in query2
                             where q.BirthCertificateNo == this.txtBirthCertificateNo.Text.Trim()
                             select q;
                }

                if (!string.IsNullOrEmpty(this.txtCarPlateNumber_1.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_2.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_3.Text))
                {
                    query2 = from q in query2
                             where q.TwoDigits == this.txtCarPlateNumber_1.Text.Trim() &&
                                      q.ThreeDigits == this.txtCarPlateNumber_2.Text.Trim() &&
                                      q.RegionIdentifier == this.txtCarPlateNumber_3.Text.Trim() &&
                                      q.Alphabet == this.drpCarPlateNumber.SelectedValue
                             select q;
                }

                if (this.txtDateFrom.HasDate && !this.txtDateTo.HasDate)
                {
                    query2 = from q in query2
                             where q.SubmitDate == this.txtDateFrom.GeorgianDate.Value
                             select q;
                }
                else if (this.txtDateFrom.HasDate && this.txtDateTo.HasDate)
                {
                    query2 = from q in query2
                             where q.SubmitDate >= this.txtDateFrom.GeorgianDate.Value && q.SubmitDate <= this.txtDateTo.GeorgianDate.Value
                             select q;
                }

                db.Connection.Open();
                dtObj.Load(db.GetCommand(query2).ExecuteReader());
                db.Connection.Close();
                db.Dispose();
                break;
        }

        dtObj.TableName = "dt";
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.Load(HttpContext.Current.Server.MapPath("~/App_Data/Report/mrt/drivers.mrt"));
        report.RegData(dtObj);
        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    protected void drpAjancyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAjancies();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["ajancyType"] = Public.ToByte(this.drpAjancyType.SelectedValue);
        e.InputParameters["ajancyId"] = Public.ToInt(this.drpAjancies.SelectedValue);
        e.InputParameters["driverCertification"] = this.drpDriverCertification.SelectedIndex;
        e.InputParameters["drivingLicenseType"] = Public.ToByte(this.drpDrivingLicenseType.SelectedValue);
        e.InputParameters["marriage"] = Public.ToByte(this.drpMarriage.SelectedValue);
        e.InputParameters["carType"] = Public.ToShort(this.drpCarType.SelectedValue);
        e.InputParameters["fuelType"] = Public.ToByte(this.drpFuelType.SelectedValue);
        e.InputParameters["fuelCardType"] = Public.ToByte(this.drpFuelCardType.SelectedValue);
        e.InputParameters["carEngineNo"] = this.txtCarEngineNo.Text.Trim();
        e.InputParameters["carChassisNo"] = this.txtCarChassisNo.Text.Trim();
        e.InputParameters["pan"] = this.txtFuelCardPAN.Text.Trim();
        e.InputParameters["vin"] = this.txtCarVIN.Text.Trim();
        e.InputParameters["gender"] = Public.ToByte(this.drpGender.SelectedValue);
        e.InputParameters["firstName"] = this.txtFirstName.Text.Trim();
        e.InputParameters["lastName"] = this.txtLastName.Text.Trim();
        e.InputParameters["nationalCode"] = this.txtNationalCode.Text.Trim();
        e.InputParameters["birthCertificateNo"] = this.txtBirthCertificateNo.Text;
        e.InputParameters["zCityId"] = Public.ToShort(this.drpCarPlateNumberCity.SelectedValue);
        e.InputParameters["zNumber"] = this.txtCarPlateNumber_5.Text.Trim();
        e.InputParameters["carPlateNumber_1"] = this.txtCarPlateNumber_1.Text.Trim();
        e.InputParameters["carPlateNumber_2"] = this.txtCarPlateNumber_2.Text.Trim();
        e.InputParameters["carPlateNumber_3"] = this.txtCarPlateNumber_3.Text.Trim();
        e.InputParameters["alphabet"] = this.drpCarPlateNumber.SelectedValue;
        e.InputParameters["status"] = this.drpStatus.SelectedIndex.ToString();
        e.InputParameters["dateFrom"] = this.txtDateFrom.GeorgianDate;
        e.InputParameters["dateTo"] = this.txtDateTo.GeorgianDate;
    }

    private void LoadAjancies()
    {
        this.drpAjancies.Items.Clear();
        if (this.drpCity.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpAjancies.DataSource = db.Ajancies.Where(aj => aj.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) &&
                                                                                                     aj.CityID == Public.ToInt(this.drpCity.SelectedValue)).OrderBy(j => j.AjancyName).
                                                                                                      Select(aj => new { aj.AjancyID, aj.AjancyName });
            this.drpAjancies.DataBind();
        }
        this.drpAjancies.Items.Insert(0, "- همه آژانس ها -");
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}