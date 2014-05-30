using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public partial class Site_Images : System.Web.UI.Page
{
    XDocument doc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            doc = XDocument.Load(Server.MapPath("~/App_Data/Images.xml"));
            this.drpLetters.DataSource = doc.Element("Images").Elements("Image").Select(an => new { Id = an.Attribute("id").Value, Title = an.Element("Title").Value });
            this.drpLetters.DataBind();
            this.drpLetters.Items.Insert(0, "- جدید -");
        }
    }

    protected void drpLetters_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpLetters.SelectedIndex > 0)
        {
            doc = XDocument.Load(Server.MapPath("~/App_Data/Images.xml"));
            var text = doc.Element("Images").Elements("Image").Where(let => let.Attribute("id").Value == this.drpLetters.SelectedValue).Select(let =>
                new
                {
                    Title = let.Element("Title").Value,
                    IsActive = bool.Parse(let.Attribute("isActive").Value)
                });
            foreach (var item in text)
            {
                this.chkIsActive.Checked = item.IsActive;
                this.txtTitle.Text = item.Title;
                this.imgLetter.ImageUrl = string.Format("~/LettImg/{0}.jpg", this.drpLetters.SelectedValue);
            }
        }
        else
        {
            this.chkIsActive.Checked = false;
            this.txtTitle.Text = null;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        doc = XDocument.Load(Server.MapPath("~/App_Data/Images.xml"));
        if (this.drpLetters.SelectedIndex > 0) // Edit mode
        {
            IEnumerable<XElement> elements = doc.Element("Images").Elements("Image").Where(an => an.Attribute("id").Value == this.drpLetters.SelectedValue);
            foreach (XElement item in elements)
            {
                item.Attribute("isActive").Value = this.chkIsActive.Checked.ToString();
                item.Element("Title").Value = this.txtTitle.Text;
            }
            doc.Save(Server.MapPath("~/App_Data/Images.xml"));
            if (this.fluLetter.HasFile)
            {
                if (this.fluLetter.PostedFile.ContentType.Equals("image/pjpeg") || this.fluLetter.PostedFile.ContentType.Equals("image/x-png"))
                {                    
                    string file = string.Format("{0}/{1}.jpg", Server.MapPath("~/LettImg"), this.drpLetters.SelectedValue);
                    System.IO.File.Delete(file);
                    this.fluLetter.PostedFile.SaveAs(file);
                }
                else
                {
                    this.lblMessage.Text = "فرمت عکس jpg نمیباشد";
                }
            }
            this.lblMessage.Text = Public.EDITMESSAGE;
        }
        else // Add mode
        {
            if (this.fluLetter.HasFile)
            {
                if (this.fluLetter.PostedFile.ContentType.Equals("image/pjpeg") || this.fluLetter.PostedFile.ContentType.Equals("image/x-png"))
                {
                    string maxId = doc.Element("Images").Elements("Image").Max(tst => tst.Attribute("id").Value);
                    string nextId = maxId == null ? "1" : (byte.Parse(maxId) + 1).ToString();
                    doc.Element("Images").Add(new XElement("Image", new XAttribute("id", nextId),
                                                                                         new XAttribute("isActive", this.chkIsActive.Checked.ToString()),
                                                                                         new XElement("Title", this.txtTitle.Text.Trim())));

                    doc.Save(Server.MapPath("~/App_Data/Images.xml"));
                    this.fluLetter.PostedFile.SaveAs(string.Format("{0}/{1}.jpg", Server.MapPath("~/LettImg"), nextId));
                    this.lblMessage.Text = Public.SAVEMESSAGE;
                }
                else
                {
                    this.lblMessage.Text = "فرمت عکس jpg نمیباشد";
                }
            }
        }

        this.drpLetters.DataSource = doc.Element("Images").Elements("Image").Select(an => new { Id = an.Attribute("id").Value, Title = an.Element("Title").Value });
        this.drpLetters.DataBind();
        this.drpLetters.Items.Insert(0, "- جدید -");
        this.drpLetters.SelectedIndex = 0;
        this.chkIsActive.Checked = false;
        this.txtTitle.Text = null;
        this.imgLetter.ImageUrl = null;
    }
}
