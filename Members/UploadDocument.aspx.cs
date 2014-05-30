using System;
using System.Linq;
using System.Data.Linq;

public partial class Members_UploadDocument : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hdnPersonId.Value = Public.ActiveUserRole.User.PersonID.ToString();
        }
    }

    protected void fluDocument_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        if (this.fluDocument.HasFile)
        {
            if (this.fluDocument.PostedFile.ContentType.Equals("image/pjpeg") || this.fluDocument.PostedFile.ContentType.Equals("image/x-png"))
            {
                byte[] fileByte = this.fluDocument.FileBytes;
                Binary binaryObj = new Binary(fileByte);
                Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                Ajancy.Doument doc = db.Douments.FirstOrDefault<Ajancy.Doument>(d => d.PersonID == Public.ActiveUserRole.User.PersonID && d.DocumentType == 1);

                if (doc == null)
                {
                    doc = new Ajancy.Doument
                                {
                                    DocumentType = Public.ToByte(this.drpType.SelectedValue),
                                    PersonID = Public.ActiveUserRole.User.PersonID,
                                    Picture = binaryObj,
                                    SubmitDate = DateTime.Now
                                };
                    db.Douments.InsertOnSubmit(doc);
                }
                else
                {
                    doc.Picture = binaryObj;
                }
                db.SubmitChanges();
                db.Dispose();
            }
        }
    }
}