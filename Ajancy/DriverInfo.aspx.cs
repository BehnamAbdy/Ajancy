using System;
using System.Linq;
using System.Data.Linq;

public partial class Ajancy_DriverInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int personId = 0;
            if (Request.QueryString["id"] != null && int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId))
            {
                Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.DrivingLicenses);
                dlo.LoadWith<Ajancy.Person>(p => p.DriverCertifications);
                dlo.LoadWith<Ajancy.DriverCertification>(dc => dc.DriverCertificationCars);
                dlo.LoadWith<Ajancy.DriverCertificationCar>(dcc => dcc.CarPlateNumber);
                dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.PlateNumber);
                dlo.LoadWith<Ajancy.CarPlateNumber>(cpn => cpn.Car);
                dlo.LoadWith<Ajancy.Car>(c => c.FuelCards);
                dlo.LoadWith<Ajancy.Car>(c => c.CarType);
                db.LoadOptions = dlo;
                SetPerson(db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == personId));
                db.Dispose();
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }

    private void SetPerson(Ajancy.Person person)
    {
        if (person == null)
        {
            this.txtFirstName.Text = null;
            this.txtLastName.Text = null;
            this.txtFather.Text = null;
            this.txtBirthCertificateNo.Text = null;
            this.txtBirthCertificateSerial.Text = null;
            this.txtBirthCertificateSerie.Text = null;
            this.drpBirthCertificateAlfa.SelectedIndex = 0;
            this.drpGender.SelectedIndex = 0;
            this.txtBirthDate.Text = null;
            this.txtBirthPlace.Text = null;
            this.txtBirthCertificatePlace.Text = null;
            this.drpMarriage.SelectedIndex = 0;
            this.txtFamilyMembersCount.Text = null;
            this.drpEducation.SelectedIndex = 0;
            this.drpMilitaryService.SelectedIndex = 0;
            this.drpReligion.SelectedIndex = 0;
            this.txtSubreligion.Text = null;
            this.drpJobStatus.SelectedIndex = 0;
            this.txtPhone.Text = null;
            this.txtMobile.Text = null;
            this.txtPostalCode.Text = null;
            this.txtAddress.Text = null;
        }
        else
        {
            this.txtFirstName.Text = person.FirstName;
            this.txtLastName.Text = person.LastName;
            this.txtFather.Text = person.Father;
            this.txtNationalCode.Text = person.NationalCode;
            this.txtBirthCertificateNo.Text = person.BirthCertificateNo;
            this.txtBirthCertificateSerial.Text = person.BirthCertificateSerial;
            this.txtBirthCertificateSerie.Text = person.BirthCertificateSerie;
            this.drpBirthCertificateAlfa.SelectedValue = person.BirthCertificateAlfa;
            this.drpGender.SelectedValue = person.Gender.GetValueOrDefault().ToString();
            this.txtBirthDate.SetDate(person.BirthDate);
            this.txtBirthPlace.Text = person.BirthPlace;
            this.txtBirthCertificatePlace.Text = person.BirthCertificatePlace;
            this.drpMarriage.SelectedValue = person.Marriage.GetValueOrDefault().ToString();
            this.txtFamilyMembersCount.Text = person.FamilyMembersCount;
            this.drpEducation.SelectedValue = person.Education.GetValueOrDefault().ToString();
            this.drpMilitaryService.SelectedValue = person.MilitaryService.GetValueOrDefault().ToString();
            this.drpReligion.SelectedValue = person.Religion.GetValueOrDefault().ToString();
            this.txtSubreligion.Text = person.Subreligion;
            this.drpJobStatus.SelectedValue = person.JobStatus.GetValueOrDefault().ToString();
            this.txtPhone.Text = person.Phone;
            this.txtMobile.Text = person.Mobile;
            this.txtPostalCode.Text = person.PostalCode;
            this.txtAddress.Text = person.Address;
            if (person.DriverCertifications.Count == 1)
            {
                this.txtDriverCertificationNo.Text = person.DriverCertifications[0].DriverCertificationNo;
                SetDrivingLicense(person.DrivingLicenses.LastOrDefault<Ajancy.DrivingLicense>());
                Ajancy.CarPlateNumber cpn = person.DriverCertifications.LastOrDefault<Ajancy.DriverCertification>().DriverCertificationCars.LastOrDefault<Ajancy.DriverCertificationCar>().CarPlateNumber;
                SetCar(cpn.Car);
                SetFuelCard(person.DriverCertifications.LastOrDefault<Ajancy.DriverCertification>().DriverCertificationCars.LastOrDefault<Ajancy.DriverCertificationCar>().CarPlateNumber.Car.FuelCards.Last<Ajancy.FuelCard>());
            }
        }
    }

    private void SetDrivingLicense(Ajancy.DrivingLicense drivingLicense)
    {
        if (drivingLicense == null)
        {
            this.txtDrivingLicenseNo.Text = null;
            this.txtDrivingLicenseDate.Text = null;
            this.txtDrivingLicensePlace.Text = null;
            this.drpDrivingLicenseType.SelectedIndex = 0;
        }
        else
        {
            this.txtDrivingLicenseNo.Text = drivingLicense.DrivingLicenseNo;
            this.txtDrivingLicenseDate.SetDate(drivingLicense.ExportDate);
            this.txtDrivingLicensePlace.Text = drivingLicense.ExportPlace;
            this.drpDrivingLicenseType.SelectedValue = drivingLicense.Type.ToString();
        }
    }

    private void SetCar(Ajancy.Car car)
    {
        if (car == null)
        {
            this.drpCarType.SelectedIndex = 0;
            this.txtCarVIN.Text = null;
            this.txtCarModel.Text = null;
            this.txtCarColor.Text = null;
            this.txtCarEngineNo.Text = null;
            this.txtCarChassisNo.Text = null;
            this.drpFuelType.SelectedValue = null;
            this.drpGASProvider.SelectedIndex = 0;
            SetPlateNumber(null);
        }
        else
        {
            this.drpCarType.Items.Add(car.CarType.TypeName);
            this.txtCarVIN.Text = car.VIN;
            this.txtCarModel.Text = car.Model;
            this.txtCarColor.Text = car.Color;
            this.txtCarEngineNo.Text = car.EngineNo;
            this.txtCarChassisNo.Text = car.ChassisNo;
            this.drpFuelType.SelectedValue = car.FuelType.ToString();
            this.drpGASProvider.SelectedValue = car.GASProvider.GetValueOrDefault().ToString();
            SetPlateNumber(car.CarPlateNumbers.Last<Ajancy.CarPlateNumber>().PlateNumber);
        }
    }

    private void SetPlateNumber(Ajancy.PlateNumber plateNumber)
    {
        if (plateNumber != null)
        {
            this.txtCarPlateNumber_1.Text = plateNumber.TwoDigits;
            this.drpCarPlateNumber.SelectedValue = plateNumber.Alphabet;
            this.txtCarPlateNumber_2.Text = plateNumber.ThreeDigits;
            this.txtCarPlateNumber_3.Text = plateNumber.RegionIdentifier;
        }
        else
        {
            this.txtCarPlateNumber_1.Text = null;
            this.drpCarPlateNumber.SelectedIndex = 0;
            this.txtCarPlateNumber_2.Text = null;
            this.txtCarPlateNumber_3.Text = null;
        }
    }

    private void SetFuelCard(Ajancy.FuelCard fuelCard)
    {
        if (fuelCard == null)
        {
            this.txtFuelCardPAN.Text = null;
            this.drpFuelCardType.SelectedIndex = 0;
        }
        else
        {
            this.txtFuelCardPAN.Text = fuelCard.PAN;
            this.drpFuelCardType.SelectedValue = fuelCard.CardType.ToString();
        }
    }
}
