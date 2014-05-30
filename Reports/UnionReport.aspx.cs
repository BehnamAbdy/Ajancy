using System;
using System.Linq;
using System.Web;
using System.Data;

public partial class Reports_UnionReport : System.Web.UI.Page
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

            switch ((Public.Role)Public.ActiveUserRole.RoleID)
            {
                case Public.Role.Admin:
                    //this.drpProvince.Enabled = true;
                    //this.drpCity.Enabled = true;
                    //this.drpAjancyType.Enabled = true;
                    break;

                case Public.Role.CityManager:
                    this.drpAjancyType.SelectedIndex = 0;
                    break;

                case Public.Role.ProvinceManager:
                    this.drpCity.Enabled = true;
                    this.drpAjancyType.SelectedIndex = 0;
                    break;

                case Public.Role.AcademyCity:
                    this.drpAjancyType.SelectedIndex = 2;
                    break;

                case Public.Role.AcademyProvince:
                    this.drpCity.Enabled = true;
                    this.drpAjancyType.SelectedIndex = 2;
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
    }

    protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpAjancies.Items.Clear();
        if (this.drpCity.SelectedIndex > 0)
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

    protected void btnDrivers_Click(object sender, EventArgs e)
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = (from p in db.Persons
                     join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                     join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                     join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                     join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                     join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                     join c in db.Cars on cpn.CarID equals c.CarID
                     join fc in db.FuelCards on c.CarID equals fc.CarID
                     join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                     join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                     join ur in db.UsersInRoles on jp.UserRoleID equals ur.UserRoleID
                     join ct in db.Cities on j.CityID equals ct.CityID
                     join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                     where j.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) &&
                                ur.RoleID == (short)Public.Role.AjancyManager &&
                                jd.LockOutDate == null &&
                                dcc.LockOutDate == null &&
                                fc.DiscardDate == null
                     orderby p.LastName
                     select new
                     {
                         Driver = p,
                         Owner = new { owp.NationalCode, owp.FirstName, owp.LastName, },
                         DrivingLicense = p.DrivingLicenses.Count > 0 ? p.DrivingLicenses[0] : null,
                         j.AjancyName,
                         City = ct.Name,
                         Car = new
                         {
                             Color = c.Color,
                             Model = c.Model,
                             EngineNo = c.EngineNo,
                             ChassisNo = c.ChassisNo,
                             TypeName = c.CarType.TypeName,
                             FuelType = c.FuelType,
                             VIN = c.VIN,
                             TwoDigits = cpn.PlateNumber.TwoDigits,
                             Alphabet = cpn.PlateNumber.Alphabet,
                             ThreeDigits = cpn.PlateNumber.ThreeDigits,
                             Iran = "ایران",
                             RegionIdentifier = cpn.PlateNumber.RegionIdentifier,
                         },
                         ct.ProvinceID,
                         ct.CityID,
                         dc.DriverCertificationNo,
                         FuelCard = new { fc.CardType, fc.PAN },
                         j.AjancyID
                     });

        if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue)
                    select q;
        }

        if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex > 0) // province and city
        {
            query = from q in query
                    where q.CityID == Public.ToByte(this.drpCity.SelectedValue)
                    select q;
        }

        if (this.drpAjancies.SelectedIndex > 0)
        {
            query = from q in query
                    where q.AjancyID == Public.ToInt(this.drpAjancies.SelectedValue)
                    select q;
        }


        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Father", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthCertificateNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthCertificateAlfa", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthCertificateSerie", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthCertificateSerial", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthCertificatePlace", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthDate", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthPlace", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Gender", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Marriage", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FamilyMembersCount", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Education", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Religion", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Subreligion", typeof(string)));
        dtObj.Columns.Add(new DataColumn("JobStatus", typeof(string)));
        dtObj.Columns.Add(new DataColumn("MilitaryService", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Mobile", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Phone", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PostalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Address", typeof(string)));

        dtObj.Columns.Add(new DataColumn("DrivingLicenseNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ExportDate", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Type", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ExportPlace", typeof(string)));

        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("CardType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Color", typeof(string)));
        dtObj.Columns.Add(new DataColumn("EngineNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FuelType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TwoDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Alphabet", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ThreeDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Iran", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegionIdentifier", typeof(string)));

        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("DriverCertificationNo", typeof(string)));


        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.Owner.NationalCode;
            row[1] = item.Owner.FirstName;
            row[2] = item.Owner.LastName;
            row[3] = item.Driver.NationalCode;
            row[4] = item.Driver.FirstName;
            row[5] = item.Driver.LastName;
            row[6] = item.Driver.Father;
            row[7] = item.Driver.BirthCertificateNo;
            row[8] = Public.GetAlphabet(item.Driver.BirthCertificateAlfa);
            row[9] = item.Driver.BirthCertificateSerie;
            row[10] = item.Driver.BirthCertificateSerial;
            row[11] = item.Driver.BirthCertificatePlace;
            row[12] = item.Driver.BirthDate;
            row[13] = item.Driver.BirthPlace;
            row[14] = item.Driver.Gender.GetValueOrDefault() == 0 ? "مرد" : "زن";
            row[15] = item.Driver.Marriage.GetValueOrDefault() == 1 ? "متاهل" : "مجرد";
            row[16] = item.Driver.FamilyMembersCount;
            row[17] = Public.GetEducation(item.Driver.Education.GetValueOrDefault());
            row[18] = Public.GetReligion(item.Driver.Religion.GetValueOrDefault());
            row[19] = item.Driver.Subreligion;
            row[20] = Public.GetJobStatus(item.Driver.JobStatus.GetValueOrDefault());
            row[21] = Public.GetMilitaryService(item.Driver.MilitaryService.GetValueOrDefault());
            row[22] = item.Driver.Mobile;
            row[23] = item.Driver.Phone;
            row[24] = item.Driver.PostalCode;
            row[25] = item.Driver.Address;

            if (item.DrivingLicense != null)
            {
                row[26] = item.DrivingLicense.DrivingLicenseNo;
                row[27] = Public.ToPersianDate(item.DrivingLicense.ExportDate);
                row[28] = Public.GetDrivingLicenseType(item.DrivingLicense.Type);
                row[29] = item.DrivingLicense.ExportPlace;
            }
            else
            {
                row[26] = string.Empty;
                row[27] = string.Empty;
                row[28] = string.Empty;
                row[29] = string.Empty;
            }

            row[30] = item.Car.VIN;
            row[31] = item.FuelCard.PAN;
            row[32] = Public.GetFuelCardTypeName((Public.FuelCardType)item.FuelCard.CardType);
            row[33] = item.Car.TypeName;
            row[34] = item.Car.Model;
            row[35] = item.Car.Color;
            row[36] = item.Car.EngineNo;
            row[37] = item.Car.ChassisNo;
            row[38] = Public.GetFuelTypeName((Public.FuelType)item.Car.FuelType);
            row[39] = item.Car.TwoDigits;
            row[40] = Public.GetAlphabet(item.Car.Alphabet);
            row[41] = item.Car.ThreeDigits;
            row[42] = "ایران";
            row[43] = item.Car.RegionIdentifier;
            row[44] = item.AjancyName;
            row[45] = item.City;
            row[46] = item.DriverCertificationNo;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.Load(HttpContext.Current.Server.MapPath("~/App_Data/Report/mrt/drivers_all.mrt"));
        report.RegData(dtObj);
        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    protected void btnAjancy_Click(object sender, EventArgs e)
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from j in db.Ajancies
                    join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                    join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                    join ur in db.UsersInRoles on jp.UserRoleID equals ur.UserRoleID
                    join u in db.Users on ur.UserID equals u.UserID
                    join p in db.Persons on u.PersonID equals p.PersonID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where jp.LockOutDate == null &&
                               j.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) &&
                               bl.LockOutDate == null
                    orderby j.AjancyName
                    select new
                    {
                        Ajancy = j,
                        Utility = j.AjancyUtilities.Select(ju => new { ju.Utility }),
                        BusinessLicense = j.BusinessLicenses[0],
                        ct.ProvinceID,
                        ct.CityID,
                        City = ct.Name,
                        Manager = string.Format("{0} {1}", p.FirstName, p.LastName)
                    };

        if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue)
                    select q;
        }

        if (this.drpProvince.SelectedIndex > 0 && this.drpCity.SelectedIndex > 0) // province and city
        {
            query = from q in query
                    where q.CityID == Public.ToByte(this.drpCity.SelectedValue)
                    select q;
        }

        if (this.drpAjancies.SelectedIndex > 0)
        {
            query = from q in query
                    where q.Ajancy.AjancyID == Public.ToInt(this.drpAjancies.SelectedValue)
                    select q;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("BusinessLicenseType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OfficePosition", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OfficeLevel", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OfficeSpace", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BalconySpace", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BalconyHeight", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ParkingSpace", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ParkingState", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BusinessScope", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PoliceStation", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Mayor", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Address", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PostalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Phone", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegisteredPelak", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BluePelak", typeof(string)));
        dtObj.Columns.Add(new DataColumn("EstateType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("DocumentType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PlaceOwner", typeof(string)));
        dtObj.Columns.Add(new DataColumn("WaterBillSerial", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ElectricityBillSerial", typeof(string)));
        dtObj.Columns.Add(new DataColumn("GasBillSerial", typeof(string)));

        dtObj.Columns.Add(new DataColumn("BusinessLicenseNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("MemberShipCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("SerialNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ISIC", typeof(string)));
        dtObj.Columns.Add(new DataColumn("CategoryCode", typeof(string)));

        dtObj.Columns.Add(new DataColumn("Water", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Electricity", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Gas", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TelePhone", typeof(string)));
        dtObj.Columns.Add(new DataColumn("WC", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Fax", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Balcon", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Manager", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.Ajancy.BusinessLicenseType == 0 ? "عادی" : "ویژه ایثارگران و جانبازان و شهدا";
            row[1] = item.Ajancy.AjancyName;
            row[2] = OfficePosition(item.Ajancy.OfficePosition.GetValueOrDefault());
            row[3] = !string.IsNullOrEmpty(item.Ajancy.OfficeLevel) ? item.Ajancy.OfficeLevel : "1";
            row[4] = item.Ajancy.OfficeSpace;
            row[5] = !string.IsNullOrEmpty(item.Ajancy.BalconySpace) ? item.Ajancy.BalconySpace : "0";
            row[6] = !string.IsNullOrEmpty(item.Ajancy.BalconyHeight) ? item.Ajancy.BalconyHeight : "0";
            row[7] = item.Ajancy.ParkingSpace;
            row[8] = item.Ajancy.ParkingState.GetValueOrDefault() ? "بلی" : "خیر";
            row[9] = !string.IsNullOrEmpty(item.Ajancy.BusinessScope) ? item.Ajancy.BusinessScope : "...";
            row[10] = !string.IsNullOrEmpty(item.Ajancy.PoliceStation) ? item.Ajancy.PoliceStation : "...";
            row[11] = Mayor(item.Ajancy.Mayor.GetValueOrDefault());
            row[12] = item.Ajancy.Address;
            row[13] = item.Ajancy.PostalCode;
            row[14] = item.Ajancy.Phone;
            row[15] = item.Ajancy.RegisteredPelak;
            row[16] = !string.IsNullOrEmpty(item.Ajancy.BluePelak) ? item.Ajancy.BluePelak : "...";
            row[17] = item.Ajancy.EstateType.GetValueOrDefault() == 0 ? "ملکی" : "استیجاری";
            row[18] = DocumentType(item.Ajancy.DocumentType.GetValueOrDefault());
            row[19] = !string.IsNullOrEmpty(item.Ajancy.PlaceOwner) ? item.Ajancy.PlaceOwner : "...";
            row[20] = item.Ajancy.WaterBillSerial;
            row[21] = item.Ajancy.ElectricityBillSerial;
            row[22] = item.Ajancy.GasBillSerial;

            row[23] = item.BusinessLicense.BusinessLicenseNo;
            row[24] = item.BusinessLicense.MemberShipCode;
            row[25] = item.BusinessLicense.SerialNo;
            row[26] = item.BusinessLicense.ISIC;
            row[27] = item.BusinessLicense.CategoryCode;

            row[28] = "ندارد";
            row[29] = "ندارد";
            row[30] = "ندارد";
            row[31] = "ندارد";
            row[32] = "ندارد";
            row[33] = "ندارد";
            foreach (var util in item.Utility)
            {
                switch (util.Utility)
                {
                    case 0:
                        row[28] = "دارد";
                        break;

                    case 1:
                        row[29] = "دارد";
                        break;

                    case 2:
                        row[30] = "دارد";
                        break;

                    case 3:
                        row[31] = "دارد";
                        break;

                    case 4:
                        row[32] = "دارد";
                        break;

                    case 5:
                        row[33] = "دارد";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(item.Ajancy.BalconySpace) && item.Ajancy.BalconySpace.ToString() != "0")
            {
                row[34] = "بلی";
            }
            else
            {
                row[34] = "خیر";
            }
            row[35] = item.Manager;
            row[36] = item.City;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.Load(HttpContext.Current.Server.MapPath("~/App_Data/Report/mrt/ajancy_all.mrt"));
        report.RegData(dtObj);
        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    public string OfficePosition(byte officePosition)
    {
        switch (officePosition)
        {
            case 0: return "مستقل";
            case 1: return "درون مجتمع (طبقه همکف)";
            case 2: return "درون مجتمع (طبقه غیر همکف)";
            case 3: return "درون مجتمع کارگاهی";
            default: return string.Empty;
        }
    }

    public string Mayor(byte mayor)
    {
        switch (mayor)
        {
            case 0: return "شهرداری منطقه 1";
            case 1: return "شهرداری منطقه 2";
            case 2: return "شهرداری منطقه 3";
            case 3: return "شهرداری منطقه 4";
            case 4: return "شهرداری منطقه 5";
            case 5: return "شهرداری منطقه 6";
            case 6: return "شهرداری منطقه 7";
            case 7: return "شهرداری منطقه 8";
            case 8: return "شهرداری منطقه 9";
            case 9: return "گلستان";
            case 10: return "قلات";
            case 11: return "لپویی";
            case 12: return "گویم";
            case 13: return "مرکزی";
            case 14: return "دوکوهک";
            case 15: return "کوار";
            case 16: return "صدرا";
            default: return string.Empty;
        }
    }

    public string DocumentType(byte documentType)
    {
        switch (documentType)
        {
            case 0: return "رسمی";
            case 1: return "شهرداری منطقه 2";
            case 2: return "اوقاف";
            case 3: return "اداری";
            case 4: return "عادی";
            case 5: return "محضری";
            default: return string.Empty;
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