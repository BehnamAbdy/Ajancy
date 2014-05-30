using System;
using System.Data;
using System.Linq;
using System.Web;

public partial class Reports_Ajancies : System.Web.UI.Page
{
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
            this.ObjectDataSource1.Select();
            this.lstAjancies.DataBind();
        }
    }

    protected void drpProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpProvince.SelectedIndex > 0)
        {
            Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCity.DataSource = db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue)).Select(c => new { c.CityID, c.Name });
            this.drpCity.DataBind();
            db.Dispose();
        }
        else
        {
            this.drpCity.Items.Clear();
        }
        this.drpCity.Items.Insert(0, "- انتخاب کنید -");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ObjectDataSource1.Select();
        this.lstAjancies.DataBind();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from j in db.Ajancies
                    join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                    join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                    join ur in db.UsersInRoles on jp.UserRoleID equals ur.UserRoleID
                    join u in db.Users on ur.UserID equals u.UserID
                    join p in db.Persons on u.PersonID equals p.PersonID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    join pv in db.Provinces on ct.ProvinceID equals pv.ProvinceID
                    where jp.LockOutDate == null &&
                               j.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue) && bl.LockOutDate == null
                    orderby pv.Name, ct.Name, j.AjancyName
                    select new
                    {
                        ct.ProvinceID,
                        ct.CityID,
                        Province = pv.Name,
                        City = ct.Name,
                        j.AjancyName,
                        j.Phone,
                        j.PostalCode,
                        j.Address,
                        bl.BusinessLicenseNo,
                        j.BusinessLicenseType,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        p.Mobile
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

        if (!string.IsNullOrEmpty(this.txtAjancyName.Text))
        {
            query = from q in query
                    where q.AjancyName.Contains(this.txtAjancyName.Text.Trim())
                    select q;
        }

        DataTable dtObj = new DataTable();
        dtObj.Columns.Add(new DataColumn("FirstName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("LastName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("NationalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Mobile", typeof(string)));
        dtObj.Columns.Add(new DataColumn("AjancyName", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BusinessLicenseNo", typeof(string)));
        dtObj.Columns.Add(new DataColumn("BusinessLicenseType", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Address", typeof(string)));
        dtObj.Columns.Add(new DataColumn("PostalCode", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Phone", typeof(string)));
        dtObj.Columns.Add(new DataColumn("Province", typeof(string)));
        dtObj.Columns.Add(new DataColumn("City", typeof(string)));

        foreach (var item in query)
        {
            DataRow row = dtObj.NewRow();
            row[0] = item.FirstName;
            row[1] = item.LastName;
            row[2] = item.NationalCode;
            row[3] = item.Mobile;
            row[4] = item.AjancyName;
            row[5] = item.BusinessLicenseNo;
            row[6] = item.BusinessLicenseType == 0 ? "عادی" : "ویژه ایثارگران و جانبازان و شهدا";
            row[7] = item.Address;
            row[8] = item.PostalCode;
            row[9] = item.Phone;
            row[10] = item.Province;
            row[11] = item.City;

            dtObj.Rows.Add(row);
        }

        db.Dispose();
        dtObj.TableName = "dt";
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.RegData(dtObj);
        report.Load(HttpContext.Current.Server.MapPath("~/App_Data/Report/mrt/ajancy_list.mrt"));
        report.Render();
        Public.ExportInfo(3, report);
        report.Dispose();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["ajancyType"] = Public.ToByte(this.drpAjancyType.SelectedValue);
        e.InputParameters["ajancyName"] = this.txtAjancyName.Text.Trim();
    }
}