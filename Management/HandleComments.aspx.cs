using System;
using System.Linq;

public partial class Management_HandleComments : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int commentId = 0;
            if (Request.QueryString["mode"] != null)
            {
                switch (Request.QueryString["mode"])
                {
                    case "0": // Delete mode
                        if (int.TryParse(Request.QueryString["id"], out commentId))
                        {
                            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                            db.Comments.DeleteOnSubmit(db.Comments.First<Ajancy.Comment>(c => c.CommentID == commentId));
                            db.SubmitChanges();
                            DisposeContext();
                            Response.Clear();
                            Response.Write("1");
                            Response.End();
                        }
                        break;
                }
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    private void FillGrid()
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        var query = from c in db.Comments
                    join j in db.Ajancies on c.AjancyID equals j.AjancyID
                    select new
                    {
                        j.AjancyName,
                        c.CommentID,
                        c.Subject,
                        c.DriverFirstName,
                        c.DriverLastName,
                        c.FirstName,
                        c.LastName,
                        c.Phone,
                        c.PlateNumber,
                        c.ProblemDate,
                        c.Email,
                        c.CommentText,
                        c.SubmitDate
                    };
        if (this.txtDateFrom.HasDate)
        {
            query = from q in query
                    where q.SubmitDate >= this.txtDateFrom.GeorgianDate
                    select q;
        }
        if (this.txtDateFrom.HasDate && this.txtDateTo.HasDate)
        {
            query = from q in query
                    where q.SubmitDate <= this.txtDateTo.GeorgianDate
                    select q;
        }

        this.lstComplaints.DataSource = query;
        this.lstComplaints.DataBind();
    }

    protected string Subject(string subject)
    {
        string title = null;
        switch (subject)
        {
            case "0":
                title = "گرفتن کرایه زیاد";
                break;

            case "1":
                title = "مشاجره راننده با مسافر";
                break;

            case "2":
                title = "مشکل اخلاقی راننده";
                break;
        }
        return title;
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
