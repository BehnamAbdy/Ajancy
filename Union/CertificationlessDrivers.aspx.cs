using System;
using System.Linq;

public partial class Union_CertificationlessDrivers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpAjancies.DataSource = db.Ajancies.Where(aj => aj.AjancyType == (byte)Public.AjancyType.TaxiAjancy && aj.AjancyName != null).OrderBy(j => j.AjancyName).Select(aj => new { aj.AjancyID, aj.AjancyName });
            this.drpAjancies.DataBind();
            this.drpAjancies.Items.Insert(0, "- همه آژانس ها -");
            this.drpCarType.DataSource = db.CarTypes;
            this.drpCarType.DataBind();
            this.drpCarType.Items.Insert(0, "- همه موارد -");
            db.Dispose();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ObjectDataSource1.Select();
        this.lstDrivers.DataBind();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["dateFrom"] = this.txtDateFrom.GeorgianDate;
        e.InputParameters["dateTo"] = this.txtDateTo.GeorgianDate;
        e.InputParameters["ajancyId"] = Public.ToInt(this.drpAjancies.SelectedValue);
        e.InputParameters["firstName"] = this.txtFirstName.Text.Trim();
        e.InputParameters["lastName"] = this.txtLastName.Text.Trim();
        e.InputParameters["nationalCode"] = this.txtNationalCode.Text.Trim();
        e.InputParameters["birthCertificateNo"] = this.txtBirthCertificateNo.Text.Trim();
        e.InputParameters["carType"] = Public.ToByte(this.drpCarType.SelectedValue);
        e.InputParameters["carPlateNumber_1"] = this.txtCarPlateNumber_1.Text.Trim();
        e.InputParameters["carPlateNumber_2"] = this.txtCarPlateNumber_2.Text.Trim();
        e.InputParameters["carPlateNumber_3"] = this.txtCarPlateNumber_3.Text.Trim();
        e.InputParameters["alphabet"] = this.drpCarPlateNumber.SelectedValue;
    }
}
