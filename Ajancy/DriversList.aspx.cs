using System;
using System.Linq;
using System.Web;

public partial class Ajancy_DriversList : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            if (Request.QueryString["mode"] != null)
            {
                int ajancyDriverId = 0;
                switch (Request.QueryString["mode"])
                {
                    case "0": // Complaint
                        if (int.TryParse(Request.QueryString["jdId"], out ajancyDriverId) && Request.QueryString["text"] != null)
                        {
                            db.AjancyComplaints.InsertOnSubmit(new Ajancy.AjancyComplaint { AjancyDriverID = ajancyDriverId, RoleID = Public.ActiveUserRole.RoleID, Comment = Request.QueryString["text"].Length > 200 ? Request.QueryString["text"].Substring(0, 200) : Request.QueryString["text"], SubmitDate = DateTime.Now });
                            db.SubmitChanges();
                            DisposeContext();
                            Response.Clear();
                            Response.Write("1");
                            Response.End();
                        }
                        break;

                    case "1": // CNG
                        if (int.TryParse(Request.QueryString["jdId"], out ajancyDriverId) && Request.QueryString["text"] != null)
                        {
                            db.CNG_ConsumptionRequests.InsertOnSubmit(new Ajancy.CNG_ConsumptionRequest { AjancyDriverID = ajancyDriverId, Comment = string.IsNullOrEmpty(Request.QueryString["text"]) ? null : Request.QueryString["text"].Length > 200 ? Request.QueryString["text"].Substring(0, 200) : Request.QueryString["text"], SubmitDate = DateTime.Now });
                            db.SubmitChanges();
                            DisposeContext();
                            Response.Clear();
                            Response.Write("1");
                            Response.End();
                        }
                        break;

                    //case "2": // ajancyCardType
                    //    if (int.TryParse(Request.QueryString["jdId"], out ajancyDriverId) && Request.QueryString["text"] != null)
                    //    {
                    //        db.AjancyFuelCardTypeRequests.InsertOnSubmit(new Ajancy.AjancyFuelCardTypeRequest { AjancyDriverID = ajancyDriverId, Comment = string.IsNullOrEmpty(Request.QueryString["text"]) ? null : Request.QueryString["text"].Length > 200 ? Request.QueryString["text"].Substring(0, 200) : Request.QueryString["text"], SubmitDate = DateTime.Now });
                    //        db.SubmitChanges();
                    //        Response.Clear();
                    //        Response.Write("1");
                    //        Response.End();
                    //    }
                    //    break;

                    case "3": // Insurance
                        if (int.TryParse(Request.QueryString["jdId"], out ajancyDriverId) && Request.QueryString["text"] != null)
                        {
                            db.DriverInsuranceRequests.InsertOnSubmit(new Ajancy.DriverInsuranceRequest { AjancyDriverID = ajancyDriverId, Comment = string.IsNullOrEmpty(Request.QueryString["text"]) ? null : Request.QueryString["text"].Length > 200 ? Request.QueryString["text"].Substring(0, 200) : Request.QueryString["text"], SubmitDate = DateTime.Now });
                            db.SubmitChanges();
                            DisposeContext();
                            Response.Clear();
                            Response.Write("1");
                            Response.End();
                        }
                        break;

                    case "4": // End Membership
                        if (int.TryParse(Request.QueryString["jdId"], out ajancyDriverId) && Request.QueryString["text"] != null)
                        {
                            db.DriverEndMembershipRequests.InsertOnSubmit(new Ajancy.DriverEndMembershipRequest { AjancyDriverID = ajancyDriverId, RoleID = Public.ActiveUserRole.RoleID, Comment = Request.QueryString["text"].Length > 200 ? Request.QueryString["text"].Substring(0, 200) : Request.QueryString["text"], SubmitDate = DateTime.Now, Confirmed = false });
                            db.SubmitChanges();
                            DisposeContext();
                            Response.Clear();
                            Response.Write("1");
                            Response.End();
                        }
                        break;
                }
            }
            this.drpCarType.DataSource = db.CarTypes;
            this.drpCarType.DataBind();
            this.drpCarType.Items.Insert(0, "- همه موارد -");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["ajancyId"] = Public.ActiveAjancy.AjancyID;
        e.InputParameters["driverCertification"] = this.drpDriverCertification.SelectedIndex;
        e.InputParameters["carType"] = Public.ToByte(this.drpCarType.SelectedValue);
        e.InputParameters["fuelType"] = Public.ToByte(this.drpFuelType.SelectedValue);
        e.InputParameters["fuelCardType"] = Public.ToByte(this.drpFuelCardType.SelectedValue);
        e.InputParameters["firstName"] = this.txtFirstName.Text;
        e.InputParameters["lastName"] = this.txtLastName.Text;
        e.InputParameters["carPlateNumber_1"] = this.txtCarPlateNumber_1.Text;
        e.InputParameters["carPlateNumber_2"] = this.txtCarPlateNumber_2.Text;
        e.InputParameters["carPlateNumber_3"] = this.txtCarPlateNumber_3.Text;
        e.InputParameters["alphabet"] = this.drpCarPlateNumber.SelectedValue;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ObjectDataSource1.Select();
        this.lstDrivers.DataBind();
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
