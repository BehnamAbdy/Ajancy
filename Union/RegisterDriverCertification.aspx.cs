using System;
using System.Linq;
using System.Data.Linq;

public partial class Union_RegisterDriverCertification : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int driverCertificationId = 0;
            if (Request.QueryString["id"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out driverCertificationId))
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                var driver = from dc in db.DriverCertifications
                             join p in db.Persons on dc.DriverCertificationID equals p.PersonID
                             where dc.DriverCertificationID == driverCertificationId
                             select
                             new
                             {
                                 p.NationalCode,
                                 p.FirstName,
                                 p.LastName,
                                 p.Father,
                                 dc.DriverCertificationInquiries,
                                 dc.DriverCertificationRevivals
                             };

                foreach (var item in driver)
                {
                    this.lblDriverName.Text = string.Format("{0} {1}", item.FirstName, item.LastName);
                    this.lblFather.Text = item.Father;
                    this.lblNationalCode.Text = item.NationalCode;

                    if (item.DriverCertificationRevivals.Count > 0)
                    {
                        Ajancy.DriverCertificationRevival dcr = item.DriverCertificationRevivals.LastOrDefault<Ajancy.DriverCertificationRevival>();
                        this.txtStartDate.SetDate(dcr.StartDate);
                        this.txtEndDate.SetDate(dcr.EndDate);
                        this.txtBillNo.Text = dcr.AbonnementBillNo;
                        this.txtBillAmount.Text = dcr.AbonnementAmount.GetValueOrDefault().ToString();
                    }

                    foreach (Ajancy.DriverCertificationInquiry dcq in item.DriverCertificationInquiries)
                    {
                        switch (dcq.Type)
                        {
                            case 0:
                                this.txtAngoshNegariNo.Text = dcq.SerialNumber;
                                this.txtAngoshNegariDate.SetDate(dcq.InquiryDate);
                                this.txtComment_0.Text = dcq.Comment;
                                break;

                            case 1:
                                this.txtEtiadNo.Text = dcq.SerialNumber;
                                this.txtEtiadDate.SetDate(dcq.InquiryDate);
                                this.txtComment_1.Text = dcq.Comment;
                                break;

                            case 2:
                                this.txtAmakenNo.Text = dcq.SerialNumber;
                                this.txtAmakenDate.SetDate(dcq.InquiryDate);
                                this.txtComment_2.Text = dcq.Comment;
                                break;
                        }
                    }
                }
            }
            else
            {
                DisposeContext();
                Response.Redirect("~/DriversList.aspx");
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            int driverCertificationId = 0;
            if (Request.QueryString["id"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out driverCertificationId))
            {
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationInquiries);
                dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationRevivals);
                dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.Person);
                dlo.LoadWith<Ajancy.Person>(p => p.User);
                db.LoadOptions = dlo;
                Ajancy.DriverCertification driverCertification = db.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.DriverCertificationID == driverCertificationId);

                #region DriverCertificationInquiry

                Ajancy.DriverCertificationInquiry dcq_0 = driverCertification.DriverCertificationInquiries.FirstOrDefault<Ajancy.DriverCertificationInquiry>(dcq => dcq.Type == 0);
                Ajancy.DriverCertificationInquiry dcq_1 = driverCertification.DriverCertificationInquiries.FirstOrDefault<Ajancy.DriverCertificationInquiry>(dcq => dcq.Type == 1);
                Ajancy.DriverCertificationInquiry dcq_2 = driverCertification.DriverCertificationInquiries.FirstOrDefault<Ajancy.DriverCertificationInquiry>(dcq => dcq.Type == 2);

                if (dcq_0 == null && !string.IsNullOrEmpty(this.txtAngoshNegariNo.Text) && this.txtAngoshNegariDate.HasDate)
                {
                    dcq_0 = new Ajancy.DriverCertificationInquiry { Type = 0, SerialNumber = this.txtAngoshNegariNo.Text, InquiryDate = this.txtAngoshNegariDate.GeorgianDate.Value, Comment = this.txtComment_0.Text, SubmitDate = DateTime.Now };
                    driverCertification.DriverCertificationInquiries.Add(dcq_0);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.txtAngoshNegariNo.Text))
                    {
                        dcq_0.SerialNumber = this.txtAngoshNegariNo.Text;
                    }
                    if (this.txtAngoshNegariDate.HasDate)
                    {
                        dcq_0.InquiryDate = this.txtAngoshNegariDate.GeorgianDate.Value;
                    }
                    dcq_0.Comment = this.txtComment_0.Text;
                }

                if (dcq_1 == null && !string.IsNullOrEmpty(this.txtEtiadNo.Text) && this.txtEtiadDate.HasDate)
                {
                    dcq_1 = new Ajancy.DriverCertificationInquiry { Type = 1, SerialNumber = this.txtEtiadNo.Text, InquiryDate = this.txtEtiadDate.GeorgianDate.Value, Comment = this.txtComment_1.Text, SubmitDate = DateTime.Now };
                    driverCertification.DriverCertificationInquiries.Add(dcq_1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.txtEtiadNo.Text))
                    {
                        dcq_1.SerialNumber = this.txtEtiadNo.Text;
                    }
                    if (this.txtEtiadDate.HasDate)
                    {
                        dcq_1.InquiryDate = this.txtEtiadDate.GeorgianDate.Value;
                    }
                    dcq_1.Comment = this.txtComment_1.Text;
                }

                if (dcq_2 == null && !string.IsNullOrEmpty(this.txtAmakenNo.Text) && this.txtAmakenDate.HasDate)
                {
                    dcq_2 = new Ajancy.DriverCertificationInquiry { Type = 2, SerialNumber = this.txtAmakenNo.Text, InquiryDate = this.txtAmakenDate.GeorgianDate.Value, Comment = this.txtComment_2.Text, SubmitDate = DateTime.Now };
                    driverCertification.DriverCertificationInquiries.Add(dcq_2);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.txtAmakenNo.Text))
                    {
                        dcq_2.SerialNumber = this.txtAmakenNo.Text;
                    }
                    if (this.txtAmakenDate.HasDate)
                    {
                        dcq_2.InquiryDate = this.txtAmakenDate.GeorgianDate.Value;
                    }
                    dcq_2.Comment = this.txtComment_2.Text;
                }

                #endregion

                Ajancy.DriverCertificationRevival dcRevival = driverCertification.DriverCertificationRevivals.LastOrDefault<Ajancy.DriverCertificationRevival>();
                if (dcRevival == null && !string.IsNullOrEmpty(this.txtBillNo.Text) && !string.IsNullOrEmpty(this.txtBillAmount.Text))
                {
                    dcRevival = new Ajancy.DriverCertificationRevival { StartDate = this.txtStartDate.GeorgianDate, EndDate = this.txtEndDate.GeorgianDate, AbonnementBillNo = this.txtBillNo.Text, AbonnementAmount = Public.ToInt(this.txtBillAmount.Text), SubmitDate = DateTime.Now };
                    driverCertification.DriverCertificationRevivals.Add(dcRevival);
                }
                else if (dcRevival != null)
                {
                    if (this.txtStartDate.HasDate)
                    {
                        dcRevival.StartDate = this.txtStartDate.GeorgianDate;
                    }
                    if (this.txtEndDate.HasDate)
                    {
                        dcRevival.EndDate = this.txtEndDate.GeorgianDate;
                    }
                    dcRevival.AbonnementBillNo = this.txtBillNo.Text;
                    dcRevival.AbonnementAmount = Public.ToInt(this.txtBillAmount.Text);
                }

                if (!string.IsNullOrEmpty(this.txtDriverCertificationNo.Text) && this.chkVerification.Checked) // final verification
                {
                    if (db.DriverCertifications.Any<Ajancy.DriverCertification>(dc => dc.DriverCertificationNo == this.txtDriverCertificationNo.Text))
                    {
                        this.lblMessage.Text = "شماره دفترچه صلاحیت تکراری میباشد";
                        return;
                    }

                    driverCertification.DriverCertificationNo = this.txtDriverCertificationNo.Text;
                    driverCertification.DriverCode = this.txtDriverCode.Text;
                    driverCertification.VerificationDate = DateTime.Now;

                    if (driverCertification.Person.User.PassWord == null)
                    {
                        driverCertification.Person.User.PassWord = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(driverCertification.Person.NationalCode, "SHA1");
                    }
                }

                db.SubmitChanges();
                if (this.chkVerification.Checked)
                {
                    DisposeContext();
                    Response.Redirect("~/Message.aspx?mode=14");
                }
                ClearControls();
                this.lblMessage.Text = Public.SAVEMESSAGE;
            }
        }
    }

    private void ClearControls()
    {
        this.txtDriverCertificationNo.Text = null;
        this.txtDriverCode.Text = null;
        this.txtStartDate.Text = null;
        this.txtEndDate.Text = null;
        this.txtAmakenNo.Text = null;
        this.txtAmakenDate.Text = null;
        this.txtAngoshNegariNo.Text = null;
        this.txtAngoshNegariDate.Text = null;
        this.txtEtiadNo.Text = null;
        this.txtEtiadDate.Text = null;
        this.txtBillNo.Text = null;
        this.txtBillAmount.Text = null;
        this.txtComment_0.Text = null;
        this.txtComment_1.Text = null;
        this.txtComment_2.Text = null;
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
