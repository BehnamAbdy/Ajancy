using System;
using System.Linq;

public partial class Comment : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void drpAjancyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAjancies();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Ajancy.Comment comment = new Ajancy.Comment();
            comment.DriverFirstName = this.txtDriverFirstName.Text;
            comment.DriverLastName = this.txtDriverLastName.Text;
            comment.FirstName = this.txtFirstName.Text;
            comment.LastName = this.txtLastName.Text;
            comment.Phone = this.txtPhone.Text;
            comment.Email = this.txtEmail.Text;
            comment.AjancyID = Public.ToInt(this.drpAjancies.SelectedValue);
            comment.Subject = Public.ToByte(this.drpSubject.SelectedValue);
            comment.ProblemDate = new DateTime(this.txtProblemDate.GeorgianDate.Value.Year, this.txtProblemDate.GeorgianDate.Value.Month, this.txtProblemDate.GeorgianDate.Value.Day, this.txtTime.Time.Hours, this.txtTime.Time.Minutes, 0);
            comment.CommentText = this.txtComment.Text;
            comment.SubmitDate = DateTime.Now;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

            if (!string.IsNullOrEmpty(this.txtCarPlateNumber_1.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_2.Text) && !string.IsNullOrEmpty(this.txtCarPlateNumber_3.Text))
            {
                Ajancy.PlateNumber plateNumber = db.PlateNumbers.FirstOrDefault<Ajancy.PlateNumber>(pn => pn.TwoDigits == this.txtCarPlateNumber_1.Text &&
                                                                                                                                                 pn.Alphabet == this.drpCarPlateNumber.SelectedValue &&
                                                                                                                                                 pn.ThreeDigits == this.txtCarPlateNumber_3.Text &&
                                                                                                                                                 pn.RegionIdentifier == this.txtCarPlateNumber_3.Text);
                if (plateNumber == null)
                {
                    this.lblMessage.Text = "کابر گرامی شماره پلاک مورد نظر شما متعلق به رانندگان این اتحادیه نمیباشد در صورت اطمینان از درست بودن شماره پلاک در قسمت توضیحات آنرا بنویسید";
                    return;
                }
                comment.PlateNumberID = plateNumber.PlateNumberID;
            }

            db.Comments.InsertOnSubmit(comment);
            db.SubmitChanges();
            Response.Redirect("~/Default.aspx");
        }
    }

    private void LoadAjancies()
    {
        if (this.drpAjancyType.SelectedIndex > 0)
        {
            this.drpAjancies.Items.Clear();
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpAjancies.DataSource = db.Ajancies.Where(aj => aj.AjancyType == Public.ToByte(this.drpAjancyType.SelectedValue)).Select(aj => new { aj.AjancyID, aj.AjancyName });
            this.drpAjancies.DataBind();
            this.drpAjancies.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
        }
        else
        {
            this.drpAjancies.Items.Clear();
            this.drpAjancies.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- انتخاب کنید -", "0"));
        }
    }
}
