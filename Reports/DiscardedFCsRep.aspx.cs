using System;
using System.Linq;
using System.Data.Linq;

public partial class Reports_FCDiscardsRep : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Public.Role role = (Public.Role)Public.ActiveUserRole.RoleID;
            if (Request.QueryString["dfc"] != null)
            {
                string[] vals = Request.QueryString["dfc"].Split('|');
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
                dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
                dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
                dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
                dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;

                Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.NationalCode == vals[1]);
                Ajancy.DriverCertification driverCertification = (role == Public.Role.ProvinceManager || role == Public.Role.CityManager) ? person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy) :
                                                                                                                                                                                  person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.AjancyType.Academy);
                //Ajancy.FuelCard fuelCard = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate == null).CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>();
                //fuelCard.DiscardDate = null;
                db.BlockedFuelCards.DeleteOnSubmit(db.BlockedFuelCards.First<Ajancy.BlockedFuelCard>(fcs => fcs.FuelCardID == Public.ToInt(vals[0])));
                db.SubmitChanges();
                DisposeContext();
                Response.Clear();
                Response.Write("1");
                Response.End();
            }

            Ajancy.User user = Public.ActiveUserRole.User;
            this.drpProvince.SelectedValue = user.ProvinceID.ToString();
            this.drpProvince_SelectedIndexChanged(sender, e);
            this.drpCity.SelectedValue = user.CityID.ToString();

            switch (role)
            {
                case Public.Role.Admin:
                    this.drpProvince.Enabled = true;
                    this.drpCity.Enabled = true;
                    this.drpAjancyType.Enabled = true;
                    break;

                case Public.Role.CityManager:
                    this.drpAjancyType.SelectedIndex = 0;
                    break;

                case Public.Role.ProvinceManager:
                    this.drpCity.Enabled = true;
                    this.drpAjancyType.SelectedIndex = 0;
                    break;

                case Public.Role.AcademyCity:
                    this.drpAjancyType.SelectedIndex = 2;
                    break;

                case Public.Role.AcademyProvince:
                    this.drpCity.Enabled = true;
                    this.drpAjancyType.SelectedIndex = 2;
                    break;
            }
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
            this.drpCity.Items.Insert(0, "- همه موارد -");
            db.Dispose();
        }
        else
        {
            this.drpCity.Items.Clear();
            this.drpCity.Items.Insert(0, "- همه موارد -");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.lstFCDiscards.DataSourceID = "ObjectDataSource1";
        this.ObjectDataSource1.Select();
        this.lstFCDiscards.DataBind();
    }

    protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["dateFrom"] = this.txtDateFrom.GeorgianDate;
        e.InputParameters["dateTo"] = this.txtDateTo.GeorgianDate;
        e.InputParameters["provinceId"] = Public.ToByte(this.drpProvince.SelectedValue);
        e.InputParameters["cityId"] = Public.ToShort(this.drpCity.SelectedValue);
        e.InputParameters["ajancyType"] = Public.ToByte(this.drpAjancyType.SelectedValue);
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}