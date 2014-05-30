using System;
using System.Linq;

public partial class Ajancy_Complaints : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int ajancyComplaintId = 0;
            if (int.TryParse(Request.QueryString["id"], out ajancyComplaintId))
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                Ajancy.AjancyComplaint complaint = db.AjancyComplaints.FirstOrDefault<Ajancy.AjancyComplaint>(jc => jc.AjancyComplaintID == ajancyComplaintId);
                DisposeContext();
                Response.Clear();
                Response.Write(complaint == null ? null : complaint.Reply);
                Response.End();
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from jd in db.AjancyDrivers
                    join c in db.AjancyComplaints on jd.AjancyDriverID equals c.AjancyDriverID
                    join dcc in db.DriverCertificationCars on jd.DriverCertificationCarID equals dcc.DriverCertificationCarID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    where j.AjancyID == Public.ActiveAjancy.AjancyID &&
                            (c.RoleID == (byte)(Public.Role.AjancyManager) ||
                             c.RoleID == (byte)(Public.Role.AjancySupervisor))
                    select new
                    {
                        p.FirstName,
                        p.LastName,
                        p.NationalCode,
                        Gender = p.Gender == 0 ? "مرد" : "زن",
                        dc.DriverCertificationNo,
                        c.AjancyComplaintID,
                        c.RoleID,
                        c.SubmitDate,
                        c.Comment,
                        c.ReplyDate,
                        j.AjancyName
                    };


        if (this.drpReplyMode.SelectedIndex == 0)
        {
            query = from q in query
                    where q.ReplyDate != null
                    select q;
        }
        else
        {
            query = from q in query
                    where q.ReplyDate == null
                    select q;
        }

        if (this.txtDateFrom.HasDate && this.txtDateTo.HasDate)
        {
            query = from q in query
                    where q.SubmitDate >= this.txtDateFrom.GeorgianDate && q.SubmitDate <= this.txtDateTo.GeorgianDate
                    select q;
        }

        this.lstComplaints.DataSource = query;
        this.lstComplaints.DataBind();
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
