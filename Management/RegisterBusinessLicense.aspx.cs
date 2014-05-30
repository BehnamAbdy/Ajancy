using System;
using System.Linq;
using System.Data.Linq;

public partial class Management_RegisterBusinessLicense : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //int ajancyId = 0;
        //if (Request.QueryString["j"] == null || !int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["j"]), out ajancyId))
        //{
        //    Response.Redirect("~/Management/BusinessLicenseRequests.aspx");
        //}
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            int ajancyId = 0;
            if (Request.QueryString["j"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["j"]), out ajancyId))
            {
                new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Ajancy>(j => j.AjancyPartners);
                dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.UsersInRole);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.User);
                dlo.LoadWith<Ajancy.User>(u => u.Person);
                db.LoadOptions = dlo;
                Ajancy.Ajancy ajancy = db.Ajancies.FirstOrDefault<Ajancy.Ajancy>(j => j.AjancyID == ajancyId);
                if (ajancy != null)
                {
                    Ajancy.BusinessLicense businessLicense = new Ajancy.BusinessLicense();

                    #region BusinessLicenseInquiry

                    Ajancy.BusinessLicenseInquiry blq_0 = businessLicense.BusinessLicenseInquiries.SingleOrDefault<Ajancy.BusinessLicenseInquiry>(blq => blq.Type == 0);
                    Ajancy.BusinessLicenseInquiry blq_1 = businessLicense.BusinessLicenseInquiries.SingleOrDefault<Ajancy.BusinessLicenseInquiry>(blq => blq.Type == 1);
                    Ajancy.BusinessLicenseInquiry blq_2 = businessLicense.BusinessLicenseInquiries.SingleOrDefault<Ajancy.BusinessLicenseInquiry>(blq => blq.Type == 2);
                    Ajancy.BusinessLicenseInquiry blq_3 = businessLicense.BusinessLicenseInquiries.SingleOrDefault<Ajancy.BusinessLicenseInquiry>(blq => blq.Type == 3);
                    Ajancy.BusinessLicenseInquiry blq_4 = businessLicense.BusinessLicenseInquiries.SingleOrDefault<Ajancy.BusinessLicenseInquiry>(blq => blq.Type == 4);

                    if (blq_0 == null && !string.IsNullOrEmpty(this.txtDaraeeNo.Text) && this.txtDaraeeDate.HasDate)
                    {
                        blq_0 = new Ajancy.BusinessLicenseInquiry { Type = 0, SerialNumber = this.txtDaraeeNo.Text, InquiryDate = this.txtDaraeeDate.GeorgianDate.Value, Comment = this.txtComment_0.Text, SubmitDate = DateTime.Now };
                        businessLicense.BusinessLicenseInquiries.Add(blq_0);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtDaraeeNo.Text))
                        {
                            blq_0.SerialNumber = this.txtDaraeeNo.Text;
                        }
                        if (this.txtDaraeeDate.HasDate)
                        {
                            blq_0.InquiryDate = this.txtDaraeeDate.GeorgianDate.Value;
                        }
                        blq_0.Comment = this.txtComment_0.Text;
                    }

                    if (blq_1 == null && !string.IsNullOrEmpty(this.txtShhrdariNo.Text) && this.txtShhrdariDate.HasDate)
                    {
                        blq_1 = new Ajancy.BusinessLicenseInquiry { Type = 1, SerialNumber = this.txtShhrdariNo.Text, InquiryDate = this.txtShhrdariDate.GeorgianDate.Value, Comment = this.txtComment_1.Text, SubmitDate = DateTime.Now };
                        businessLicense.BusinessLicenseInquiries.Add(blq_1);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtShhrdariNo.Text))
                        {
                            blq_1.SerialNumber = this.txtShhrdariNo.Text;
                        }
                        if (this.txtShhrdariDate.HasDate)
                        {
                            blq_1.InquiryDate = this.txtShhrdariDate.GeorgianDate.Value;
                        }
                        blq_1.Comment = this.txtComment_1.Text;
                    }

                    if (blq_2 == null && !string.IsNullOrEmpty(this.txtAmakenNo.Text) && this.txtAmakenDate.HasDate)
                    {
                        blq_2 = new Ajancy.BusinessLicenseInquiry { Type = 2, SerialNumber = this.txtAmakenNo.Text, InquiryDate = this.txtAmakenDate.GeorgianDate.Value, Comment = this.txtComment_2.Text, SubmitDate = DateTime.Now };
                        businessLicense.BusinessLicenseInquiries.Add(blq_2);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtAmakenNo.Text))
                        {
                            blq_2.SerialNumber = this.txtAmakenNo.Text;
                        }
                        if (this.txtAmakenDate.HasDate)
                        {
                            blq_2.InquiryDate = this.txtAmakenDate.GeorgianDate.Value;
                        }
                        blq_2.Comment = this.txtComment_2.Text;
                    }

                    if (blq_3 == null && !string.IsNullOrEmpty(this.txtAngoshNegariNo.Text) && this.txtAngoshNegariDate.HasDate)
                    {
                        blq_3 = new Ajancy.BusinessLicenseInquiry { Type = 3, SerialNumber = this.txtAngoshNegariNo.Text, InquiryDate = this.txtAngoshNegariDate.GeorgianDate.Value, Comment = this.txtComment_3.Text, SubmitDate = DateTime.Now };
                        businessLicense.BusinessLicenseInquiries.Add(blq_3);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtAngoshNegariNo.Text))
                        {
                            blq_3.SerialNumber = this.txtAngoshNegariNo.Text;
                        }
                        if (this.txtAngoshNegariDate.HasDate)
                        {
                            blq_3.InquiryDate = this.txtAngoshNegariDate.GeorgianDate.Value;
                        }
                        blq_3.Comment = this.txtComment_3.Text;
                    }

                    if (blq_4 == null && !string.IsNullOrEmpty(this.txtAsnafNo.Text) && this.txtAsnafDate.HasDate)
                    {
                        blq_4 = new Ajancy.BusinessLicenseInquiry { Type = 4, SerialNumber = this.txtAsnafNo.Text, InquiryDate = this.txtAsnafDate.GeorgianDate.Value, Comment = this.txtComment_4.Text, SubmitDate = DateTime.Now };
                        businessLicense.BusinessLicenseInquiries.Add(blq_4);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtAsnafNo.Text))
                        {
                            blq_4.SerialNumber = this.txtAsnafNo.Text;
                        }
                        if (this.txtAsnafDate.HasDate)
                        {
                            blq_4.InquiryDate = this.txtAsnafDate.GeorgianDate.Value;
                        }
                        blq_4.Comment = this.txtComment_4.Text;
                    }

                    #endregion

                    #region BusinessLicenseInitialPayment

                    Ajancy.BusinessLicenseInitialPayment blp_0 = businessLicense.BusinessLicenseInitialPayments.SingleOrDefault<Ajancy.BusinessLicenseInitialPayment>(blp => blp.Type == 0);
                    Ajancy.BusinessLicenseInitialPayment blp_1 = businessLicense.BusinessLicenseInitialPayments.SingleOrDefault<Ajancy.BusinessLicenseInitialPayment>(blp => blp.Type == 1);
                    Ajancy.BusinessLicenseInitialPayment blp_2 = businessLicense.BusinessLicenseInitialPayments.SingleOrDefault<Ajancy.BusinessLicenseInitialPayment>(blp => blp.Type == 2);
                    Ajancy.BusinessLicenseInitialPayment blp_3 = businessLicense.BusinessLicenseInitialPayments.SingleOrDefault<Ajancy.BusinessLicenseInitialPayment>(blp => blp.Type == 3);
                    Ajancy.BusinessLicenseInitialPayment blp_4 = businessLicense.BusinessLicenseInitialPayments.SingleOrDefault<Ajancy.BusinessLicenseInitialPayment>(blp => blp.Type == 4);
                    Ajancy.BusinessLicenseInitialPayment blp_5 = businessLicense.BusinessLicenseInitialPayments.SingleOrDefault<Ajancy.BusinessLicenseInitialPayment>(blp => blp.Type == 5);

                    if (blp_0 == null && !string.IsNullOrEmpty(this.txtBillNoKhazane.Text) && !string.IsNullOrEmpty(this.txtBillKhazaneAmount.Text))
                    {
                        blp_0 = new Ajancy.BusinessLicenseInitialPayment { Type = 0, BillNumber = this.txtBillNoKhazane.Text, Amount = Public.ToInt(this.txtBillKhazaneAmount.Text) };
                        businessLicense.BusinessLicenseInitialPayments.Add(blp_0);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtBillNoKhazane.Text))
                        {
                            blp_0.BillNumber = this.txtBillNoKhazane.Text;
                        }
                        if (!string.IsNullOrEmpty(this.txtBillKhazaneAmount.Text))
                        {
                            blp_0.Amount = Public.ToInt(this.txtBillKhazaneAmount.Text);
                        }
                    }

                    if (blp_1 == null && !string.IsNullOrEmpty(this.txtBillNoEtehadieh.Text) && !string.IsNullOrEmpty(this.txtBillEtehadiehAmount.Text))
                    {
                        blp_1 = new Ajancy.BusinessLicenseInitialPayment { Type = 1, BillNumber = this.txtBillNoEtehadieh.Text, Amount = Public.ToInt(this.txtBillEtehadiehAmount.Text) };
                        businessLicense.BusinessLicenseInitialPayments.Add(blp_1);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtBillNoEtehadieh.Text))
                        {
                            blp_1.BillNumber = this.txtBillNoEtehadieh.Text;
                        }
                        if (!string.IsNullOrEmpty(this.txtBillEtehadiehAmount.Text))
                        {
                            blp_1.Amount = Public.ToInt(this.txtBillEtehadiehAmount.Text);
                        }
                    }

                    if (blp_2 == null && !string.IsNullOrEmpty(this.txtBillNoAsnaf.Text) && !string.IsNullOrEmpty(this.txtBillAsnafAmount.Text))
                    {
                        blp_2 = new Ajancy.BusinessLicenseInitialPayment { Type = 2, BillNumber = this.txtBillNoAsnaf.Text, Amount = Public.ToInt(this.txtBillAsnafAmount.Text) };
                        businessLicense.BusinessLicenseInitialPayments.Add(blp_2);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtBillNoAsnaf.Text))
                        {
                            blp_2.BillNumber = this.txtBillNoAsnaf.Text;
                        }
                        if (!string.IsNullOrEmpty(this.txtBillAsnafAmount.Text))
                        {
                            blp_2.Amount = Public.ToInt(this.txtBillAsnafAmount.Text);
                        }
                    }

                    if (blp_3 == null && !string.IsNullOrEmpty(this.txtBillNoOzviat.Text) && !string.IsNullOrEmpty(this.txtBillOzviatAmount.Text))
                    {
                        blp_3 = new Ajancy.BusinessLicenseInitialPayment { Type = 3, BillNumber = this.txtBillNoOzviat.Text, Amount = Public.ToInt(this.txtBillOzviatAmount.Text) };
                        businessLicense.BusinessLicenseInitialPayments.Add(blp_3);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtBillNoOzviat.Text))
                        {
                            blp_3.BillNumber = this.txtBillNoOzviat.Text;
                        }
                        if (!string.IsNullOrEmpty(this.txtBillOzviatAmount.Text))
                        {
                            blp_3.Amount = Public.ToInt(this.txtBillOzviatAmount.Text);
                        }
                    }

                    if (blp_4 == null && !string.IsNullOrEmpty(this.txtBillNoVorodi.Text) && !string.IsNullOrEmpty(this.txtBillVorodiAmount.Text))
                    {
                        blp_4 = new Ajancy.BusinessLicenseInitialPayment { Type = 4, BillNumber = this.txtBillNoVorodi.Text, Amount = Public.ToInt(this.txtBillVorodiAmount.Text) };
                        businessLicense.BusinessLicenseInitialPayments.Add(blp_4);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtBillNoVorodi.Text))
                        {
                            blp_4.BillNumber = this.txtBillNoVorodi.Text;
                        }
                        if (!string.IsNullOrEmpty(this.txtBillVorodiAmount.Text))
                        {
                            blp_4.Amount = Public.ToInt(this.txtBillVorodiAmount.Text);
                        }
                    }

                    if (blp_5 == null && !string.IsNullOrEmpty(this.txtBillNoEhdaee.Text) && !string.IsNullOrEmpty(this.txtBillEhdaeeAmount.Text))
                    {
                        blp_5 = new Ajancy.BusinessLicenseInitialPayment { Type = 5, BillNumber = this.txtBillNoEhdaee.Text, Amount = Public.ToInt(this.txtBillEhdaeeAmount.Text) };
                        businessLicense.BusinessLicenseInitialPayments.Add(blp_5);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtBillNoEhdaee.Text))
                        {
                            blp_5.BillNumber = this.txtBillNoEhdaee.Text;
                        }
                        if (!string.IsNullOrEmpty(this.txtBillEhdaeeAmount.Text))
                        {
                            blp_5.Amount = Public.ToInt(this.txtBillEhdaeeAmount.Text);
                        }
                    }

                    #endregion

                    if (this.chkVerification.Checked)
                    {
                        if (!this.txtStartDate.HasDate || !this.txtEndDate.HasDate)
                        {
                            this.lblMessage.Text = "تاریخ صدور و تاریخ انقضا را وارد کنید";
                            return;
                        }
                        if (string.IsNullOrEmpty(this.txtBusinessLicenseNo.Text))
                        {
                            this.lblMessage.Text = "شماره پروانه کسب را وارد کنید";
                            return;
                        }

                        businessLicense.BusinessLicenseNo = this.txtBusinessLicenseNo.Text;
                        businessLicense.MemberShipCode = this.txtMemberShipCode.Text;
                        businessLicense.NationalCardBarCode = this.txtNationalCardBarCode.Text;
                        businessLicense.SerialNo = this.txtSerialNo.Text;
                        businessLicense.CategoryCode = this.txtCategoryCode.Text;
                        businessLicense.ISIC = this.txtISIC.Text;
                        businessLicense.SubmitDate = DateTime.Now;
                        businessLicense.BusinessLicenseRevivals.Add(new Ajancy.BusinessLicenseRevival { StartDate = this.txtStartDate.GeorgianDate.Value, EndDate = this.txtEndDate.GeorgianDate.Value, SubmitDate = DateTime.Now });
                        ajancy.AjancyName = this.txtAjancyName.Text;

                        //foreach (Ajancy.AjancyPartner partner in ajancy.AjancyPartners)
                        //{
                        //    partner.LockOutDate = null;
                        //    partner.UsersInRole.LockOutDate = null;
                        //    if (partner.UsersInRole.User.PassWord == null)
                        //    {
                        //        partner.UsersInRole.User.PassWord = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(partner.UsersInRole.User.Person.NationalCode, "SHA1");
                        //    }
                        //}
                    }

                    db.SubmitChanges();
                    ClearControls();
                    this.lblMessage.Text = Public.SAVEMESSAGE;
                }
            }
        }
    }

    private void ClearControls()
    {
        this.txtBusinessLicenseNo.Text = null;
        this.txtMemberShipCode.Text = null;
        this.txtNationalCardBarCode.Text = null;
        this.txtSerialNo.Text = null;
        this.txtCategoryCode.Text = null;
        this.txtISIC.Text = null;
        this.txtStartDate.Text = null;
        this.txtEndDate.Text = null;
        this.txtDaraeeNo.Text = null;
        this.txtDaraeeDate.Text = null;
        this.txtShhrdariNo.Text = null;
        this.txtShhrdariDate.Text = null;
        this.txtAmakenNo.Text = null;
        this.txtAmakenDate.Text = null;
        this.txtAngoshNegariNo.Text = null;
        this.txtAngoshNegariDate.Text = null;
        this.txtAsnafNo.Text = null;
        this.txtAsnafDate.Text = null;
        this.txtAjancyName.Text = null;
        this.txtBillNoKhazane.Text = null;
        this.txtBillNoEtehadieh.Text = null;
        this.txtBillNoAsnaf.Text = null;
        this.txtBillNoOzviat.Text = null;
        this.txtBillNoVorodi.Text = null;
        this.txtBillNoEhdaee.Text = null;
        this.txtBillEhdaeeAmount.Text = null;
        this.txtBillVorodiAmount.Text = null;
        this.txtBillOzviatAmount.Text = null;
        this.txtBillAsnafAmount.Text = null;
        this.txtBillEtehadiehAmount.Text = null;
        this.txtBillKhazaneAmount.Text = null;
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}