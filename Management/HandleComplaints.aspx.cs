using System;
using System.Linq;

public partial class Management_HandleComplaints : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int ajancyComplaintId = 0;
            if (int.TryParse(Request.QueryString["id"], out ajancyComplaintId) && Request.QueryString["txt"] != null)
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                Ajancy.AjancyComplaint complaint = db.AjancyComplaints.FirstOrDefault<Ajancy.AjancyComplaint>(jc => jc.AjancyComplaintID == ajancyComplaintId);
                complaint.Reply = Request.QueryString["txt"].Length > 200 ? Request.QueryString["txt"].Substring(0, 200) : Request.QueryString["txt"];
                complaint.ReplyDate = DateTime.Now;
                db.SubmitChanges();
                DisposeContext();
                Response.Clear();
                Response.Write("1");
                Response.End();
            }
            else if (int.TryParse(Request.QueryString["id"], out ajancyComplaintId) && Request.QueryString["txt"] == null)
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
        var query = from c in db.AjancyComplaints
                    join jd in db.AjancyDrivers on c.AjancyDriverID equals jd.AjancyDriverID
                    join dcc in db.DriverCertificationCars on jd.DriverCertificationCarID equals dcc.DriverCertificationCarID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    where jd.LockOutDate == null
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

        if (this.drpComplaintType.SelectedIndex == 0)
        {
            query = from q in query
                    where q.RoleID == (byte)(Public.Role.AjancyManager) ||
                             q.RoleID == (byte)(Public.Role.AjancySupervisor)
                    select q;
        }
        else
        {
            query = from q in query
                    where q.RoleID == (byte)(Public.Role.TaxiDriver)
                    select q;
        }


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
