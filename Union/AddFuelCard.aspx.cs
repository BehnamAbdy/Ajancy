using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Union_AddFuelCard : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.drpCars.Items.Insert(0, "- انتخاب کنید -");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void txtNationalCode_TextChanged(object sender, EventArgs e)
    {
        this.lblProvince.Text = null;
        this.lblCity.Text = null;
        this.lblOwner.Text = null;
        this.drpCars.Items.Clear();

        if (!string.IsNullOrEmpty(this.txtNationalCode.Text))
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var query = from p in db.Persons
                        join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                        join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                        join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                        join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                        join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                        join c in db.Cars on cpn.CarID equals c.CarID
                        join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                        join u in db.Users on p.PersonID equals u.PersonID
                        join ur in db.UsersInRoles on u.UserID equals ur.UserID
                        join ct in db.Cities on u.CityID equals ct.CityID
                        join pv in db.Provinces on u.ProvinceID equals pv.ProvinceID
                        where p.NationalCode == this.txtNationalCode.Text.Trim() &&
                                 p.PersonID == cpn.OwnerPersonID &&
                                 ur.RoleID == (byte)Public.Role.TaxiDriver
                        select new
                        {
                            Owner = string.Format("{0} {1}", p.FirstName, p.LastName),
                            City = ct.Name,
                            Province = pv.Name,
                            u.ProvinceID,
                            u.CityID,
                            c.CarID,
                            Car = string.Format("{4} --- {3} ایران {2} {1} {0 }", pn.TwoDigits, Public.GetAlphabet(pn.Alphabet), pn.ThreeDigits, pn.RegionIdentifier, crt.TypeName)
                        };

            if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
            {
                query = from q in query
                        where q.ProvinceID == Public.ActiveUserRole.User.ProvinceID
                        select q;
            }
            else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager)
            {
                query = from q in query
                        where q.CityID == Public.ActiveUserRole.User.CityID
                        select q;
            }

            foreach (var item in query)
            {
                this.lblProvince.Text = item.Province;
                this.lblCity.Text = item.City;
                this.lblOwner.Text = item.Owner;
                this.drpCars.Items.Add(new ListItem(item.Car, item.CarID.ToString()));
                this.drpCars.DataBind();
            }

            if (this.drpCars.Items.Count == 0) // Maybe owner is someone other than the driver
            {
                var ownerQuery = from p in db.Persons
                                 join u in db.Users on p.PersonID equals u.PersonID
                                 join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                 join ct in db.Cities on u.CityID equals ct.CityID
                                 join pv in db.Provinces on u.ProvinceID equals pv.ProvinceID
                                 where p.NationalCode == this.txtNationalCode.Text.Trim() &&
                                          ur.RoleID == (byte)Public.Role.CarOwner
                                 select new
                                 {
                                     Owner = string.Format("{0} {1}", p.FirstName, p.LastName),
                                     City = ct.Name,
                                     Province = pv.Name,
                                     u.ProvinceID,
                                     u.CityID,
                                     Cars = from cpn in db.CarPlateNumbers
                                            join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                                            join c in db.Cars on cpn.CarID equals c.CarID
                                            join crt in db.CarTypes on c.CarTypeID equals crt.CarTypeID
                                            where cpn.OwnerPersonID == p.PersonID
                                            select new
                                            {
                                                c.CarID,
                                                Car = string.Format("{4} --- {3} ایران {2} {1} {0 }", pn.TwoDigits, Public.GetAlphabet(pn.Alphabet), pn.ThreeDigits, pn.RegionIdentifier, crt.TypeName)
                                            }
                                 };

                if (Public.ActiveUserRole.RoleID == (short)Public.Role.ProvinceManager)
                {
                    query = from q in query
                            where q.ProvinceID == Public.ActiveUserRole.User.ProvinceID
                            select q;
                }
                else if (Public.ActiveUserRole.RoleID == (short)Public.Role.CityManager)
                {
                    query = from q in query
                            where q.CityID == Public.ActiveUserRole.User.CityID
                            select q;
                }

                foreach (var item in ownerQuery)
                {
                    this.lblProvince.Text = item.Province;
                    this.lblCity.Text = item.City;
                    this.lblOwner.Text = item.Owner;
                    this.drpCars.DataSource = item.Cars;
                    this.drpCars.DataBind();
                }
            }
        }

        this.drpCars.Items.Insert(0, "- انتخاب کنید -");
    }

    protected void drpCars_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCars.SelectedIndex > 0)
        {
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            var fcList = db.FuelCards.Where(fc => fc.CarID == Public.ToInt(this.drpCars.SelectedValue)).Select(fc => new { fc.CardType, fc.PAN, fc.DiscardDate });
            string _html = "<table id='list'>";
            _html += "<tr>";
            _html += "<th style='width: 20px;'></th>";
            _html += "<th>PAN</th>";
            _html += "<th>نوع کارت سوخت</th>";
            _html += "<th></th>";
            _html += "</tr>";
            string color = "#ffffff";
            byte counter = 1;
            foreach (var item in fcList)
            {
                _html += string.Format("<tr style='background-color: {0};'><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", color, counter, item.PAN, Public.GetFuelCardTypeName((Public.FuelCardType)item.CardType), item.DiscardDate == null ? "فعال" : "غیرفعال");
                if (counter % 2 == 1) // change color for alternative row
                {
                    color = "#f8faff";
                }
                else
                {
                    color = "#ffffff";
                }
                counter++;
            }
            _html += "</table>";
            this.dvFCcontainer.InnerHtml = _html;
        }
        else
        {
            this.dvFCcontainer.InnerHtml = null;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            int carId = Public.ToInt(this.drpCars.SelectedValue);
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            db.FuelCards.First<Ajancy.FuelCard>(fc => fc.CarID == carId && fc.DiscardDate == null).DiscardDate = DateTime.Now;
            db.FuelCards.InsertOnSubmit(new Ajancy.FuelCard
            {
                CarID = carId
                                 ,
                CardType = Public.ToByte(this.drpFuelCardType.SelectedValue)
                                 ,
                PAN = this.txtFuelCardPAN.Text.Trim()
                                 ,
                SubmitDate = DateTime.Now
            });

            try
            {
                db.SubmitChanges();
                DisposeContext();
                this.drpCars_SelectedIndexChanged(sender, e);
                this.lblMessage.Text = Public.SAVEMESSAGE;
                this.drpFuelCardType.SelectedIndex = 0;
                this.txtFuelCardPAN.Text = null;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    this.lblMessage.Text = "شماره PAN کارت سوخت قبلا برای خودرو دیگری ثبت شده";
                }
                else
                {
                    throw ex;
                }
            }
        }
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}