using System;
using System.Linq;
using System.Data.Linq;

public partial class Reports_FCReplacementsRep : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Public.Role role = (Public.Role)Public.ActiveUserRole.RoleID;
            if (Request.QueryString["fcs"] != null && Request.QueryString["type"] != null)
            {
                string[] vals = Request.QueryString["fcs"].Split('|');
                if (vals.Length == 5)
                {
                    char result = '0';
                    if (vals[3] != vals[4])
                    {
                        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                        db.FuelCardSubstitutions.DeleteOnSubmit(db.FuelCardSubstitutions.First<Ajancy.FuelCardSubstitution>(fcs => fcs.FuelCardSubstituteID == Public.ToInt(vals[0])));
                        db.SubmitChanges();
                        result = '1';
                    }
                    else // Self repalcement mode
                    {
                        DataLoadOptions dlo = new DataLoadOptions();
                        dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
                        dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
                        dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
                        dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
                        dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
                        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                        db.LoadOptions = dlo;

                        Ajancy.Person person = db.Persons.First<Ajancy.Person>(p => p.NationalCode == vals[3]);
                        Ajancy.DriverCertification driverCertification = person.DriverCertifications.First<Ajancy.DriverCertification>(dc => dc.CertificationType == (byte)Public.ToByte(Request.QueryString["type"]));
                        Ajancy.DriverCertificationCar driverCertificationCar = driverCertification.DriverCertificationCars.Last<Ajancy.DriverCertificationCar>(dcc => dcc.LockOutDate != null);
                        Ajancy.FuelCard fuelCard = driverCertificationCar.CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>(fc => fc.DiscardDate != null);

                        driverCertificationCar.AjancyDrivers.Last<Ajancy.AjancyDriver>(jd => jd.LockOutDate != null).LockOutDate = null;
                        driverCertificationCar.LockOutDate = null;
                        fuelCard.DiscardDate = null;
                        db.Cars.DeleteOnSubmit(db.Cars.First<Ajancy.Car>(c => c.CarID == Public.ToInt(vals[1])));
                        db.PlateNumbers.DeleteOnSubmit(db.PlateNumbers.First<Ajancy.PlateNumber>(c => c.PlateNumberID == Public.ToInt(vals[2])));
                        db.FuelCardSubstitutions.DeleteOnSubmit(db.FuelCardSubstitutions.First<Ajancy.FuelCardSubstitution>(fcs => fcs.FuelCardSubstituteID == Public.ToInt(vals[0])));
                        db.SubmitChanges();
                        result = '1';
                    }

                    DisposeContext();
                    Response.Clear();
                    Response.Write(result);
                    Response.End();
                }
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
        e.InputParameters["type"] = this.drpType.SelectedIndex.ToString();
        e.InputParameters["nationalCode"] = this.txtNationalCode.Text.Trim();
        e.InputParameters["pan"] = this.txtFuelCardPAN.Text.Trim();
        e.InputParameters["vin"] = this.txtCarVIN.Text.Trim();
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}
