using System;
using System.Linq;
using System.Web;
using System.Data;

public partial class Reports_FCExcel : System.Web.UI.Page
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
                    this.drpProvince.Enabled = true;
                    this.drpCity.Enabled = true;
                    this.drpAjancyType.Enabled = true;
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
        }
        else
        {
            this.drpCity.Items.Clear();
        }
        this.drpCity.Items.Insert(0, "- همه موارد -");
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.Load(HttpContext.Current.Server.MapPath(this.drpReportType.SelectedIndex == 1 ? "~/App_Data/Report/mrt/fc_replace.mrt" : "~/App_Data/Report/mrt/fc_dis.mrt"));
        switch (this.drpReportType.SelectedIndex)
        {
            case 0:
                report.RegData(Discards());
                break;

            case 1:
                report.RegData(Replacement());
                break;

            case 2:
                report.RegData(Discardeds());
                break;
        }

        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    protected void btnZoneReport_Click(object sender, EventArgs e)
    {
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.Load(HttpContext.Current.Server.MapPath(this.drpReportType.SelectedIndex == 1 ? "~/App_Data/Report/mrt/fc_zone-replace.mrt" : "~/App_Data/Report/mrt/fc_zone-dis.mrt"));
        switch (this.drpReportType.SelectedIndex)
        {
            case 0:
                report.RegData(ZoneDiscards());
                break;

            case 1:
                report.RegData(ZoneReplacement());
                break;

            case 2:
                report.RegData(ZoneDiscardeds());
                break;
        }

        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    private DataTable Discards()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from fcs in db.FuelCardSubstitutions
                    join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.PlateNumbers on cpn.PlateNumberID equals pl.PlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                    where fcs.PersonalTypeFuelCardID == null && dcc.LockOutDate == null
                    orderby pv.Name, ct.Name, p.LastName
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        OWNationalCode = owp.NationalCode,
                        OWFirstName = owp.FirstName,
                        OWLastName = owp.LastName,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        c.Model,
                        crt.TypeName,
                        fc.PAN,
                        c.VIN,
                        pl.Alphabet,
                        pl.RegionIdentifier,
                        pl.ThreeDigits,
                        pl.TwoDigits,
                        j.AjancyType,
                        j.AjancyName,
                        fcs.SubmitDate
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
                    where q.CityID == Public.ToShort(this.drpCity.SelectedValue)
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

        switch (this.drpAjancyType.SelectedIndex)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TwoDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Alphabet", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ThreeDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Iran", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegionIdentifier", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.OWNationalCode;
            row[1] = item.OWFirstName;
            row[2] = item.OWLastName;
            row[3] = item.NationalCode;
            row[4] = item.FirstName;
            row[5] = item.LastName;
            row[6] = item.AjancyName;
            row[7] = item.VIN;
            row[8] = item.Model;
            row[9] = item.PAN;
            row[10] = item.TwoDigits;
            row[11] = Public.GetAlphabet(item.Alphabet);
            row[12] = item.ThreeDigits;
            row[13] = "ایران";
            row[14] = item.RegionIdentifier;
            row[15] = item.City;
            row[16] = item.Province;
            row[17] = item.TypeName;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        return dtObj;
    }

    private DataTable Discardeds()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from bfc in db.BlockedFuelCards
                    join fc in db.FuelCards on bfc.FuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.PlateNumbers on cpn.PlateNumberID equals pl.PlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                    where dcc.LockOutDate == null
                    orderby pv.Name, ct.Name, p.LastName
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        OWNationalCode = owp.NationalCode,
                        OWFirstName = owp.FirstName,
                        OWLastName = owp.LastName,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        c.Model,
                        crt.TypeName,
                        fc.PAN,
                        c.VIN,
                        pl.Alphabet,
                        pl.RegionIdentifier,
                        pl.ThreeDigits,
                        pl.TwoDigits,
                        j.AjancyType,
                        j.AjancyName,
                        p.SubmitDate
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
                    where q.CityID == Public.ToShort(this.drpCity.SelectedValue)
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

        switch (this.drpAjancyType.SelectedIndex)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TwoDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Alphabet", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ThreeDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Iran", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegionIdentifier", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.OWNationalCode;
            row[1] = item.OWFirstName;
            row[2] = item.OWLastName;
            row[3] = item.NationalCode;
            row[4] = item.FirstName;
            row[5] = item.LastName;
            row[6] = item.AjancyName;
            row[7] = item.VIN;
            row[8] = item.Model;
            row[9] = item.PAN;
            row[10] = item.TwoDigits;
            row[11] = Public.GetAlphabet(item.Alphabet);
            row[12] = item.ThreeDigits;
            row[13] = "ایران";
            row[14] = item.RegionIdentifier;
            row[15] = item.City;
            row[16] = item.Province;
            row[17] = item.TypeName;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        return dtObj;
    }

    private DataTable Replacement()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from fcs in db.FuelCardSubstitutions
                    join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.PlateNumbers on cpn.PlateNumberID equals pl.PlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID

                    join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                    join c2 in db.Cars on fc2.CarID equals c2.CarID
                    join crt2 in db.CarTypes on c2.CarTypeID equals crt2.CarTypeID
                    join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                    join pl2 in db.PlateNumbers on cpn2.PlateNumberID equals pl2.PlateNumberID
                    join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                    join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                    join p2 in db.Persons on dc2.PersonID equals p2.PersonID
                    join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                    join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                    join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID
                    join ct2 in db.Cities on j2.CityID equals ct2.CityID
                    join pv2 in db.Provinces on ct2.ProvinceID equals pv2.ProvinceID

                    where fcs.PersonalTypeFuelCardID != null &&
                            (dc.PersonID == dc2.PersonID && // Self replacement
                            (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                            (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                            ||
                            ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                            (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))
                    orderby pv.Name, ct.Name
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        OWNationalCode = owp.NationalCode,
                        OWFirstName = owp.FirstName,
                        OWLastName = owp.LastName,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        c.Model,
                        crt.TypeName,
                        fc.PAN,
                        c.VIN,
                        pl.Alphabet,
                        pl.RegionIdentifier,
                        pl.ThreeDigits,
                        pl.TwoDigits,
                        j.AjancyType,
                        j.AjancyName,
                        fcs.SubmitDate,

                        City2 = ct2.Name,
                        Province2 = pv2.Name,
                        OWNationalCode2 = owp2.NationalCode,
                        OWFirstName2 = owp2.FirstName,
                        OWLastName2 = owp2.LastName,
                        NationalCode2 = p2.NationalCode,
                        FirstName2 = p2.FirstName,
                        LastName2 = p2.LastName,
                        Model2 = c2.Model,
                        TypeName2 = crt2.TypeName,
                        PAN2 = fc2.PAN,
                        VIN2 = c2.VIN,
                        Alphabet2 = pl2.Alphabet,
                        RegionIdentifier2 = pl2.RegionIdentifier,
                        ThreeDigits2 = pl2.ThreeDigits,
                        TwoDigits2 = pl2.TwoDigits,
                        AjancyType2 = j2.AjancyType,
                        AjancyName2 = j2.AjancyName
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
                    where q.CityID == Public.ToShort(this.drpCity.SelectedValue)
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

        switch (this.drpAjancyType.SelectedIndex)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TwoDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Alphabet", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ThreeDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Iran", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegionIdentifier", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));

        dtObj.Columns.Add(new DataColumn("OWNationalCode2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TwoDigits2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Alphabet2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ThreeDigits2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegionIdentifier2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName2", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.OWNationalCode;
            row[1] = item.OWFirstName;
            row[2] = item.OWLastName;
            row[3] = item.NationalCode;
            row[4] = item.FirstName;
            row[5] = item.LastName;
            row[6] = item.AjancyName;
            row[7] = item.VIN;
            row[8] = item.Model;
            row[9] = item.PAN;
            row[10] = item.TwoDigits;
            row[11] = Public.GetAlphabet(item.Alphabet);
            row[12] = item.ThreeDigits;
            row[13] = "ایران";
            row[14] = item.RegionIdentifier;
            row[15] = item.City;
            row[16] = item.Province;
            row[17] = item.TypeName;

            row[18] = item.OWNationalCode2;
            row[19] = item.OWFirstName2;
            row[20] = item.OWLastName2;
            row[21] = item.NationalCode2;
            row[22] = item.FirstName2;
            row[23] = item.LastName2;
            row[24] = item.AjancyName2;
            row[25] = item.VIN2;
            row[26] = item.Model2;
            row[27] = item.PAN2;
            row[28] = item.TwoDigits2;
            row[29] = Public.GetAlphabet(item.Alphabet2);
            row[30] = item.ThreeDigits2;
            row[31] = item.RegionIdentifier2;
            row[32] = item.City2;
            row[33] = item.Province2;
            row[34] = item.TypeName2;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        return dtObj;
    }

    private DataTable ZoneDiscards()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from fcs in db.FuelCardSubstitutions
                    join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.ZonePlateNumbers on cpn.ZonePlateNumberID equals pl.ZonePlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                    where fcs.PersonalTypeFuelCardID == null && dcc.LockOutDate == null
                    orderby pv.Name, ct.Name, p.LastName
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        OWNationalCode = owp.NationalCode,
                        OWFirstName = owp.FirstName,
                        OWLastName = owp.LastName,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        c.Model,
                        crt.TypeName,
                        fc.PAN,
                        c.VIN,
                        ZoneNumber = pl.Number,
                        ZoneCity = pl.City.Name,
                        j.AjancyType,
                        j.AjancyName,
                        fcs.SubmitDate
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
                    where q.CityID == Public.ToShort(this.drpCity.SelectedValue)
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

        switch (this.drpAjancyType.SelectedIndex)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneNumber", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneCity", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.OWNationalCode;
            row[1] = item.OWFirstName;
            row[2] = item.OWLastName;
            row[3] = item.NationalCode;
            row[4] = item.FirstName;
            row[5] = item.LastName;
            row[6] = item.AjancyName;
            row[7] = item.VIN;
            row[8] = item.Model;
            row[9] = item.PAN;
            row[10] = item.ZoneNumber;
            row[11] = item.ZoneCity;
            row[12] = item.City;
            row[13] = item.Province;
            row[14] = item.TypeName;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        return dtObj;
    }

    private DataTable ZoneDiscardeds()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from bfc in db.BlockedFuelCards
                    join fc in db.FuelCards on bfc.FuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.ZonePlateNumbers on cpn.ZonePlateNumberID equals pl.ZonePlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                    where dcc.LockOutDate == null
                    orderby pv.Name, ct.Name, p.LastName
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        OWNationalCode = owp.NationalCode,
                        OWFirstName = owp.FirstName,
                        OWLastName = owp.LastName,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        c.Model,
                        crt.TypeName,
                        fc.PAN,
                        c.VIN,
                        ZoneNumber = pl.Number,
                        ZoneCity = pl.City.Name,
                        j.AjancyType,
                        j.AjancyName,
                        p.SubmitDate
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
                    where q.CityID == Public.ToShort(this.drpCity.SelectedValue)
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

        switch (this.drpAjancyType.SelectedIndex)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneNumber", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneCity", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.OWNationalCode;
            row[1] = item.OWFirstName;
            row[2] = item.OWLastName;
            row[3] = item.NationalCode;
            row[4] = item.FirstName;
            row[5] = item.LastName;
            row[6] = item.AjancyName;
            row[7] = item.VIN;
            row[8] = item.Model;
            row[9] = item.PAN;
            row[10] = item.ZoneNumber;
            row[11] = item.ZoneCity;
            row[12] = item.City;
            row[13] = item.Province;
            row[14] = item.TypeName;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        return dtObj;
    }

    private DataTable ZoneReplacement()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from fcs in db.FuelCardSubstitutions
                    join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.ZonePlateNumbers on cpn.ZonePlateNumberID equals pl.ZonePlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID

                    join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                    join c2 in db.Cars on fc2.CarID equals c2.CarID
                    join crt2 in db.CarTypes on c2.CarTypeID equals crt2.CarTypeID
                    join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                    join pl2 in db.ZonePlateNumbers on cpn2.ZonePlateNumberID equals pl2.ZonePlateNumberID
                    join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                    join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                    join p2 in db.Persons on dc2.PersonID equals p2.PersonID
                    join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                    join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                    join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID
                    join ct2 in db.Cities on j2.CityID equals ct2.CityID
                    join pv2 in db.Provinces on ct2.ProvinceID equals pv2.ProvinceID

                    where fcs.PersonalTypeFuelCardID != null &&
                            (dc.PersonID == dc2.PersonID && // Self replacement
                            (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                            (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                            ||
                            ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                            (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))
                    orderby pv.Name, ct.Name
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        OWNationalCode = owp.NationalCode,
                        OWFirstName = owp.FirstName,
                        OWLastName = owp.LastName,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        c.Model,
                        crt.TypeName,
                        fc.PAN,
                        c.VIN,
                        ZoneNumber = pl.Number,
                        ZoneCity = pl.City.Name,
                        j.AjancyType,
                        j.AjancyName,
                        fcs.SubmitDate,

                        City2 = ct2.Name,
                        Province2 = pv2.Name,
                        OWNationalCode2 = owp2.NationalCode,
                        OWFirstName2 = owp2.FirstName,
                        OWLastName2 = owp2.LastName,
                        NationalCode2 = p2.NationalCode,
                        FirstName2 = p2.FirstName,
                        LastName2 = p2.LastName,
                        Model2 = c2.Model,
                        TypeName2 = crt2.TypeName,
                        PAN2 = fc2.PAN,
                        VIN2 = c2.VIN,
                        ZoneNumber2 = pl2.Number,
                        ZoneCity2 = pl2.City.Name,
                        AjancyType2 = j2.AjancyType,
                        AjancyName2 = j2.AjancyName
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
                    where q.CityID == Public.ToShort(this.drpCity.SelectedValue)
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

        switch (this.drpAjancyType.SelectedIndex)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("OWNationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneNumber", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneCity", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));

        dtObj.Columns.Add(new DataColumn("OWNationalCode2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWFirstName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("OWLastName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FirstName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Model2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneNumber2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ZoneCity2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province2", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName2", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.OWNationalCode;
            row[1] = item.OWFirstName;
            row[2] = item.OWLastName;
            row[3] = item.NationalCode;
            row[4] = item.FirstName;
            row[5] = item.LastName;
            row[6] = item.AjancyName;
            row[7] = item.VIN;
            row[8] = item.Model;
            row[9] = item.PAN;
            row[10] = item.ZoneNumber;
            row[11] = item.ZoneCity;
            row[12] = item.City;
            row[13] = item.Province;
            row[14] = item.TypeName;

            row[15] = item.OWNationalCode2;
            row[16] = item.OWFirstName2;
            row[17] = item.OWLastName2;
            row[18] = item.NationalCode2;
            row[19] = item.FirstName2;
            row[20] = item.LastName2;
            row[21] = item.AjancyName2;
            row[22] = item.VIN2;
            row[23] = item.Model2;
            row[24] = item.PAN2;
            row[25] = item.ZoneNumber2;
            row[26] = item.ZoneCity2;
            row[27] = item.City2;
            row[28] = item.Province2;
            row[29] = item.TypeName2;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        return dtObj;
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}