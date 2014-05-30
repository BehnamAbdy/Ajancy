using System;
using System.Linq;
using System.Data;

public partial class Reports_InsuranceRep : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int dcId;
            if (int.TryParse(Request.QueryString["dcId"], out dcId))
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.Insurances.DeleteOnSubmit(db.Insurances.First<Ajancy.Insurance>(ins => ins.DriverCertificationID == dcId));
                db.SubmitChanges();
                DisposeContext();
                Response.Clear();
                Response.Write("1");
                Response.End();
            }

            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();

            switch ((Public.Role)Public.ActiveUserRole.RoleID)
            {
                case Public.Role.Admin:
                    this.drpProvince.Enabled = true;
                    this.drpCity.Enabled = true;
                    break;

                case Public.Role.ProvinceManager:
                    this.drpCity.Enabled = true;
                    break;

                case Public.Role.AcademyProvince:
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
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.lstFCDiscards.DataSourceID = "ObjectDataSource1";
        this.ObjectDataSource1.Select();
        this.lstFCDiscards.DataBind();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from ins in db.Insurances
                    join dc in db.DriverCertifications on ins.DriverCertificationID equals dc.DriverCertificationID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join pl in db.PlateNumbers on cpn.PlateNumberID equals pl.PlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                    join fc in db.FuelCards on c.CarID equals fc.CarID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ct in db.Cities on u.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                    where dcc.LockOutDate == null && ins.CancelInsurance == Convert.ToBoolean(this.drpCancelInsurance.SelectedIndex)
                    orderby p.LastName
                    select new
                    {
                        p.PersonID,
                        p.FirstName,
                        p.LastName,
                        p.Father,
                        p.NationalCode,
                        p.BirthCertificateNo,
                        p.BirthDate,
                        u.CityID,
                        u.ProvinceID,
                        City = ct.Name,
                        Province = pv.Name,
                        c.FuelType,
                        crt.TypeName,
                        crt.Capacity,
                        c.VIN,
                        cpn.OwnerPersonID,
                        pl.TwoDigits,
                        pl.ThreeDigits,
                        pl.Alphabet,
                        pl.RegionIdentifier,
                        fc.PAN,
                        ins.InsuranceNo,
                        ins.BranchNo,
                        ins.SubmitDate
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

        if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
        {
            query = from q in query
                    where q.NationalCode == this.txtNationalCode.Text.Trim()
                    select q;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Father", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthCertificateNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BirthDate", typeof(string)));
        dtObj.Columns.Add(new DataColumn("InsuranceNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BranchNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("VIN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PAN", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TypeName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("FuelType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("TwoDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Alphabet", typeof(string)));
        dtObj.Columns.Add(new DataColumn("ThreeDigits", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Iran", typeof(string)));
        dtObj.Columns.Add(new DataColumn("RegionIdentifier", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Ownership", typeof(string)));
        dtObj.Columns.Add(new DataColumn("WorkType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Scope", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Capacity", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.FirstName;
            row[1] = item.LastName;
            row[2] = item.Father;
            row[3] = item.NationalCode;
            row[4] = item.BirthCertificateNo;
            row[5] = Public.ToPersianDate(item.BirthDate);
            row[6] = item.InsuranceNo;
            row[7] = item.BranchNo;
            row[8] = item.VIN;
            row[9] = item.PAN;
            row[10] = item.TypeName;
            row[11] = Public.GetFuelTypeName((Public.FuelType)item.FuelType);
            row[12] = item.TwoDigits;
            row[13] = Public.GetAlphabet(item.Alphabet);
            row[14] = item.ThreeDigits;
            row[15] = "ایران";
            row[16] = item.RegionIdentifier;
            row[17] = item.City;
            row[18] = item.Province;
            row[19] = item.OwnerPersonID == item.PersonID ? "مالک" : "بهره بردار";
            row[20] = "آژانس";
            row[21] = "درون شهری";
            row[22] = item.Capacity;
            dtObj.Rows.Add(row);
        }

        dtObj.TableName = "dt";
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.RegData(dtObj);
        report.Load(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Report/mrt/Insurance.mrt"));
        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["dateFrom"] = this.txtDateFrom.GeorgianDate;
        e.InputParameters["dateTo"] = this.txtDateTo.GeorgianDate;
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["nationalCode"] = this.txtNationalCode.Text.Trim();
        e.InputParameters["cancelInsurance"] = this.drpCancelInsurance.SelectedIndex == 0 ? false : true;
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}