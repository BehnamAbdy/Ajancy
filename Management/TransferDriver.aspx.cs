using System;
using System.Linq;
using System.Web.UI;

public partial class Management_TransferDriver : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bool found = false;
        if (Page.IsValid && !string.IsNullOrEmpty(this.txtNationalCode.Text))
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var driver = from jd in db.AjancyDrivers
                         join dcc in db.DriverCertificationCars on jd.DriverCertificationCarID equals dcc.DriverCertificationCarID
                         join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                         join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                         join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                         join c in db.Cars on cpn.CarID equals c.CarID
                         join ct in db.CarTypes on c.CarTypeID equals ct.CarTypeID
                         join fc in db.FuelCards on c.CarID equals fc.CarID
                         join p in db.Persons on dc.PersonID equals p.PersonID
                         join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                         join cty in db.Cities on j.CityID equals cty.CityID
                         where p.NationalCode == this.txtNationalCode.Text.Trim() &&
                                    jd.LockOutDate == null &&
                                    fc.DiscardDate == null &&
                                    dc.LockOutDate == null &&
                                    dcc.LockOutDate == null
                         select
                         new
                         {
                             DriverName = string.Concat(p.FirstName, " ", p.LastName),
                             p.Father,
                             ct.TypeName,
                             pn.TwoDigits,
                             pn.ThreeDigits,
                             pn.Alphabet,
                             pn.RegionIdentifier,
                             fc.PAN,
                             c.VIN,
                             dc.DriverCertificationNo,
                             j.AjancyName,
                             j.AjancyID,
                             jd.AjancyDriverID,
                             cty.ProvinceID,
                             cty.CityID
                         };

            foreach (var item in driver)
            {
                this.lblDriverName.Text = item.DriverName;
                this.lblFather.Text = item.Father;
                this.lblTypeName.Text = item.TypeName;
                this.lblPlateNumber.Text = Public.PlateNumberRenderToHTML(item.TwoDigits, item.Alphabet, item.ThreeDigits, item.RegionIdentifier);
                this.lblVIN.Text = item.VIN;
                this.lblPAN.Text = item.PAN;
                this.lblDriverCertificationNo.Text = string.IsNullOrEmpty(item.DriverCertificationNo) ? "ندارد" : item.DriverCertificationNo;
                this.lblAjancyName.Text = item.AjancyName;
                this.ViewState["AjancyDriverID"] = string.Concat(item.AjancyDriverID, "|", item.AjancyID);

                this.drpProvince.SelectedValue = item.ProvinceID.ToString();
                this.drpProvince_SelectedIndexChanged(sender, e);
                this.drpCity.SelectedValue = item.CityID.ToString();
                LoadAjancies(true);
                this.drpAjancies.SelectedValue = item.AjancyID.ToString();
                this.drpAjancies.Enabled = true;
                this.btnSave.Enabled = true;
                found = true;
            }
        }

        if (!found) // Driver not exists
        {
            this.lblDriverName.Text = null;
            this.lblFather.Text = null;
            this.lblTypeName.Text = null;
            this.lblPlateNumber.Text = null;
            this.lblVIN.Text = null;
            this.lblPAN.Text = null;
            this.lblDriverCertificationNo.Text = null;
            this.lblAjancyName.Text = null;
            this.ViewState["AjancyDriverID"] = null;
            this.drpAjancies.SelectedIndex = 0;
            this.drpAjancies.Enabled = false;
            this.btnSave.Enabled = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void drpProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpProvince.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            this.drpCity.DataSource = db.Cities.Where<Ajancy.City>(c => c.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue)).Select(c => new { c.CityID, c.Name });
            this.drpCity.DataBind();
        }
        else
        {
            this.drpCity.Items.Clear();
        }
        this.drpCity.Items.Insert(0, "- انتخاب کنید -");
        LoadAjancies(false);
    }

    protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCity.SelectedIndex > 0)
        {
            LoadAjancies(true);
        }
        else
        {
            LoadAjancies(false);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid && this.ViewState["AjancyDriverID"] != null)
        {
            string[] vals = this.ViewState["AjancyDriverID"].ToString().Split('|');
            if (vals[1] == this.drpAjancies.SelectedValue) // No new ajancy is selected
            {
                return;
            }

            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            Ajancy.AjancyDriver ajancyDriver = db.AjancyDrivers.First<Ajancy.AjancyDriver>(jd => jd.AjancyDriverID == Public.ToInt(vals[0]));
            ajancyDriver.LockOutDate = DateTime.Now;
            db.AjancyDrivers.InsertOnSubmit(new Ajancy.AjancyDriver { AjancyID = Public.ToInt(this.drpAjancies.SelectedValue), DriverCertificationCarID = ajancyDriver.DriverCertificationCarID, MembershipDate = DateTime.Now });
            db.SubmitChanges();
            this.lblMessage.Text = Public.SAVEMESSAGE;
            this.ViewState["AjancyDriverID"] = null;
            this.txtNationalCode.Text = null;
            this.drpAjancies.Items.Clear();
            this.drpAjancies.Enabled = false;
            this.btnSave.Enabled = false;
        }
    }

    private void LoadAjancies(bool loadItems)
    {
        this.drpAjancies.Items.Clear();

        if (loadItems)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var ajancyList = from j in db.Ajancies
                             join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == (byte)Public.AjancyType.TaxiAjancy &&
                                        ct.ProvinceID == Public.ToByte(this.drpProvince.SelectedValue) &&
                                        ct.CityID == Public.ToShort(this.drpCity.SelectedValue) &&
                                        bl.LockOutDate == null
                             orderby j.AjancyName
                             select new { j.AjancyID, j.AjancyName };

            this.drpAjancies.DataSource = ajancyList;
            this.drpAjancies.DataBind();
        }
        this.drpAjancies.Items.Insert(0, "- انتخاب کنید -");
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}