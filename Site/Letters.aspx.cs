using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public partial class Site_Letters : System.Web.UI.Page
{
    XDocument doc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            doc = XDocument.Load(Server.MapPath("~/App_Data/Letters.xml"));
            this.drpLetters.DataSource = doc.Element("Letters").Elements("Letter").Select(an => new { Id = an.Attribute("id").Value, Title = an.Element("Title").Value });
            this.drpLetters.DataBind();
            this.drpLetters.Items.Insert(0, "- جدید -");
            this.txtDate.SetDate(DateTime.Now);
        }
    }

    protected void drpLetters_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpLetters.SelectedIndex > 0)
        {
            doc = XDocument.Load(Server.MapPath("~/App_Data/Letters.xml"));
            var text = doc.Element("Letters").Elements("Letter").Where(let => let.Attribute("id").Value == this.drpLetters.SelectedValue).Select(let =>
                new
                {
                    Title = let.Element("Title").Value,
                    Text = let.Element("Text").Value,
                    Date = let.Element("Date").Value,
                    Scope = let.Attribute("scope").Value
                });
            foreach (var item in text)
            {
                this.lstScope.SelectedValue = item.Scope;
                this.txtTitle.Text = item.Title;
                this.txtDate.Text = item.Date;
                this.txtEditor.Value = item.Text;
            }
        }
        else
        {
            this.lstScope.SelectedIndex = 0;
            this.txtTitle.Text = null;
            this.txtEditor.Value = null;
            this.txtDate.SetDate(DateTime.Now);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        doc = XDocument.Load(Server.MapPath("~/App_Data/Letters.xml"));
        if (this.drpLetters.SelectedIndex > 0) // Edit mode
        {
            string fileName = string.Empty;
            if (this.fluAttachment.HasFile)
            {
                fileName = string.Format("{0}.{1}", this.drpLetters.SelectedValue, this.fluAttachment.PostedFile.FileName.Split('.')[1]);
                var oldFile = doc.Element("Letters").Elements("Letter").Where(an => an.Attribute("id").Value == this.drpLetters.SelectedValue).Select(f => new { Attachment = f.Element("Attachment").Value });
                foreach (var item in oldFile)
                {
                    if (!string.IsNullOrEmpty(item.Attachment))
                    {
                        System.IO.File.Delete(string.Format("{0}/{1}", Server.MapPath("~/Attachments"), item.Attachment));
                    }
                }
                this.fluAttachment.PostedFile.SaveAs(string.Format("{0}/{1}", Server.MapPath("~/Attachments"), fileName));
            }

            IEnumerable<XElement> elements = doc.Element("Letters").Elements("Letter").Where(an => an.Attribute("id").Value == this.drpLetters.SelectedValue);
            foreach (XElement item in elements)
            {
                item.Attribute("scope").Value = this.lstScope.SelectedValue;
                item.Element("Date").Value = this.txtDate.Text;
                item.Element("Title").Value = this.txtTitle.Text;
                item.Element("Text").Value = this.txtEditor.Value;
                item.Element("Attachment").Value = fileName;
            }
            doc.Save(Server.MapPath("~/App_Data/Letters.xml"));

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
                    return;
                }
            }

            this.lblMessage.Text = Public.EDITMESSAGE;
        }
        else // Add mode
        {
            if (this.fluLetter.HasFile && !(this.fluLetter.PostedFile.ContentType.Equals("image/jpeg") || this.fluLetter.PostedFile.ContentType.Equals("image/x-png")))
            {
                this.lblMessage.Text = "فرمت عکس jpg نمیباشد";
                return;
            }

            int nextId = 1;
            IEnumerable<XElement> lastItem = doc.Element("Letters").Elements("Letter").Reverse().Take(1);
            foreach (XElement item in lastItem)
            {
                nextId = Public.ToShort(item.Attribute("id").Value) + 1;
            }

            this.fluLetter.PostedFile.SaveAs(string.Format("{0}/{1}.jpg", Server.MapPath("~/LettImg"), nextId));
            string fileName = null;
            if (this.fluAttachment.HasFile)
            {
                fileName = string.Format("{0}.{1}", nextId, this.fluAttachment.PostedFile.FileName.Split('.')[1]);
                string file = string.Format("{0}/{1}", Server.MapPath("~/Attachments"), fileName);
                this.fluAttachment.PostedFile.SaveAs(file);
            }

            doc.Element("Letters").Add(new XElement("Letter", new XAttribute("id", nextId),
                                                              new XAttribute("scope", this.lstScope.SelectedValue),
                                                              new XElement("Date", this.txtDate.Text.Trim()),
                                                              new XElement("Title", this.txtTitle.Text.Trim()),
                                                              new XElement("Text", this.txtEditor.Value.Trim()),
                                                              new XElement("Attachment", fileName)));
            doc.Save(Server.MapPath("~/App_Data/Letters.xml"));

            this.lblMessage.Text = Public.SAVEMESSAGE;
            this.drpLetters.DataSource = doc.Element("Letters").Elements("Letter").Select(an => new { Id = an.Attribute("id").Value, Title = an.Element("Title").Value });
            this.drpLetters.DataBind();
            this.drpLetters.Items.Insert(0, "- جدید -");
            this.drpLetters.SelectedIndex = 0;
        }

        this.lstScope.SelectedIndex = 0;
        this.txtTitle.Text = null;
        this.txtEditor.Value = null;
        this.txtDate.SetDate(DateTime.Now);
    }
}