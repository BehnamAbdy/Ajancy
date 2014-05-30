using System;
using System.Linq;

public partial class Reports_FCDiscardsRep : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int fcsId;
            if (int.TryParse(Request.QueryString["fcsId"], out fcsId))
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.FuelCardSubstitutions.DeleteOnSubmit(db.FuelCardSubstitutions.First<Ajancy.FuelCardSubstitution>(fcs => fcs.FuelCardSubstituteID == fcsId));
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

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["dateFrom"] = this.txtDateFrom.GeorgianDate;
        e.InputParameters["dateTo"] = this.txtDateTo.GeorgianDate;
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["ajancyType"] = Public.ToByte(this.drpAjancyType.SelectedValue);
        e.InputParameters["nationalCode"] = this.txtNationalCode.Text.Trim();
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}