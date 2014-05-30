using System;
using System.Linq;

public partial class Management_ActiveAjancies : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();

            if (Public.ActiveUserRole.RoleID == (short)Public.Role.Admin)
            {
                this.drpProvince.Enabled = true;
                this.drpCity.Enabled = true;
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

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["ajancyType"] = Public.ToByte(this.drpAjancyType.SelectedValue);
        e.InputParameters["ajancyName"] = this.txtAjancyName.Text.Trim();
    }
}