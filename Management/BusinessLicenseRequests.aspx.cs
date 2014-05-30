using System;
using System.Linq;

public partial class Management_BusinessLicenseRequests : System.Web.UI.Page
{
    protected void btnSearch_Click(object sender, EventArgs e)
    {       
        this.ObjectDataSource1.Select(); 
        this.lstRequests.DataSourceID = "ObjectDataSource1";
        this.lstRequests.DataBind();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["dateFrom"] = this.txtDateFrom.GeorgianDate;
        e.InputParameters["dateTo"] = this.txtDateTo.GeorgianDate;
        e.InputParameters["ajancyType"] = Public.ToByte(this.drpAjancyType.SelectedValue);
    }
}