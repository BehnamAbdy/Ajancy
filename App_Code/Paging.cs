using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Paging
{
    Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);

    #region DriversReport.aspx

    public int GetDriversCount(byte provinceId, short cityId, byte ajancyType, int ajancyId, int driverCertification, byte drivingLicenseType, byte marriage, short carType, byte fuelType, byte fuelCardType, string carEngineNo, short zCityId, string zNumber, string carChassisNo, string pan, string vin, byte gender, string firstName, string lastName, string nationalCode, string birthCertificateNo, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet, string status, DateTime? dateFrom, DateTime? dateTo)
    {
        int count = 0;
        switch (status)
        {
            case "0": // All
                var query = from p in db.Persons
                            join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                            join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                            join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                            join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                            from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                            from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                            join c in db.Cars on cpn.CarID equals c.CarID
                            join fc in db.FuelCards on c.CarID equals fc.CarID
                            join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                            join ct in db.Cities on j.CityID equals ct.CityID
                            where j.AjancyType == ajancyType
                            group p by new
                            {
                                p.PersonID,
                                p.FirstName,
                                p.LastName,
                                p.NationalCode,
                                p.BirthCertificateNo,
                                p.Marriage,
                                p.Gender,
                                p.SubmitDate,
                                j.AjancyID,
                                ct.CityID,
                                ct.ProvinceID,
                                dc.DriverCertificationNo,
                                c.CarTypeID,
                                c.FuelType,
                                c.ChassisNo,
                                c.EngineNo,
                                ZCityID = (short?)zpn.CityID,
                                ZNumber = zpn.Number,
                                pn.TwoDigits,
                                pn.ThreeDigits,
                                pn.Alphabet,
                                pn.RegionIdentifier,
                                c.VIN,
                                fc.CardType,
                                fc.PAN
                            } into grp
                            select new
                            {
                                grp.Key.PersonID,
                                grp.Key.FirstName,
                                grp.Key.LastName,
                                grp.Key.NationalCode,
                                grp.Key.BirthCertificateNo,
                                grp.Key.Marriage,
                                grp.Key.Gender,
                                grp.Key.AjancyID,
                                grp.Key.CityID,
                                grp.Key.ProvinceID,
                                grp.Key.DriverCertificationNo,
                                grp.Key.SubmitDate,
                                grp.Key.CarTypeID,
                                grp.Key.FuelType,
                                grp.Key.ChassisNo,
                                grp.Key.EngineNo,
                                grp.Key.ZCityID,
                                grp.Key.ZNumber,
                                grp.Key.TwoDigits,
                                grp.Key.ThreeDigits,
                                grp.Key.Alphabet,
                                grp.Key.RegionIdentifier,
                                grp.Key.VIN,
                                grp.Key.CardType,
                                grp.Key.PAN
                            };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query = from q in query
                            where q.ProvinceID == provinceId
                            select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query = from q in query
                            where q.CityID == cityId
                            select q;
                }

                if (ajancyId > 0)
                {
                    query = from q in query
                            where q.AjancyID == ajancyId
                            select q;
                }

                if (driverCertification == 1)
                {
                    query = from q in query
                            where q.DriverCertificationNo != null
                            select q;
                }
                else if (driverCertification == 2)
                {
                    query = from q in query
                            where q.DriverCertificationNo == null
                            select q;
                }

                if (drivingLicenseType < 4)
                {
                    query = from q in query
                            join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                            where dl.Type == drivingLicenseType
                            select q;
                }

                if (marriage < 2)
                {
                    query = from q in query
                            where q.Marriage == marriage
                            select q;
                }

                if (carType > 0)
                {
                    query = from q in query
                            join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                            where q.CarTypeID == carType
                            select q;
                }

                if (fuelType < 4)
                {
                    query = from q in query
                            where q.FuelType == fuelType
                            select q;
                }

                if (fuelCardType < 3)
                {
                    query = from q in query
                            where q.CardType == fuelCardType
                            select q;
                }

                if (!string.IsNullOrEmpty(carEngineNo))
                {
                    query = from q in query
                            where q.EngineNo.Equals(carEngineNo)
                            select q;
                }

                if (!string.IsNullOrEmpty(carChassisNo))
                {
                    query = from q in query
                            where q.ChassisNo.Equals(carChassisNo)
                            select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query = from q in query
                            where q.PAN.Equals(pan)
                            select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query = from q in query
                            where q.VIN.Equals(vin)
                            select q;
                }

                if (gender < 2)
                {
                    query = from q in query
                            where q.Gender == gender
                            select q;
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query = from q in query
                            where q.FirstName.Contains(firstName)
                            select q;
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query = from q in query
                            where q.LastName.Contains(lastName)
                            select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query = from q in query
                            where q.NationalCode == nationalCode
                            select q;
                }

                if (!string.IsNullOrEmpty(birthCertificateNo))
                {
                    query = from q in query
                            where q.BirthCertificateNo == birthCertificateNo
                            select q;
                }

                if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
                {
                    query = from q in query
                            where q.TwoDigits == carPlateNumber_1 &&
                                     q.ThreeDigits == carPlateNumber_2 &&
                                     q.RegionIdentifier == carPlateNumber_3 &&
                                     q.Alphabet == alphabet
                            select q;
                }
                else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
                {
                    query = from q in query
                            where q.ZCityID == zCityId &&
                                     q.ZNumber == zNumber
                            select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query = from q in query
                            where q.SubmitDate == dateFrom
                            select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query = from q in query
                            where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                            select q;
                }
                count = query.Count();
                break;

            case "1": // Actives
                var query1 = from p in db.Persons
                             join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                             join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join c in db.Cars on cpn.CarID equals c.CarID
                             join fc in db.FuelCards on c.CarID equals fc.CarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == ajancyType &&
                                        jd.LockOutDate == null &&
                                        dcc.LockOutDate == null
                             group p by new
                             {
                                 p.PersonID,
                                 p.FirstName,
                                 p.LastName,
                                 p.NationalCode,
                                 p.BirthCertificateNo,
                                 p.Marriage,
                                 p.Gender,
                                 p.SubmitDate,
                                 j.AjancyID,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 dc.DriverCertificationNo,
                                 c.CarTypeID,
                                 c.FuelType,
                                 c.ChassisNo,
                                 c.EngineNo,
                                 ZCityID = (short?)zpn.CityID,
                                 ZNumber = zpn.Number,
                                 pn.TwoDigits,
                                 pn.ThreeDigits,
                                 pn.Alphabet,
                                 pn.RegionIdentifier,
                                 c.VIN,
                                 fc.CardType,
                                 fc.PAN
                             } into grp
                             select new
                             {
                                 grp.Key.PersonID,
                                 grp.Key.FirstName,
                                 grp.Key.LastName,
                                 grp.Key.NationalCode,
                                 grp.Key.BirthCertificateNo,
                                 grp.Key.Marriage,
                                 grp.Key.Gender,
                                 grp.Key.AjancyID,
                                 grp.Key.CityID,
                                 grp.Key.ProvinceID,
                                 grp.Key.DriverCertificationNo,
                                 grp.Key.SubmitDate,
                                 grp.Key.CarTypeID,
                                 grp.Key.FuelType,
                                 grp.Key.ChassisNo,
                                 grp.Key.EngineNo,
                                 grp.Key.ZCityID,
                                 grp.Key.ZNumber,
                                 grp.Key.TwoDigits,
                                 grp.Key.ThreeDigits,
                                 grp.Key.Alphabet,
                                 grp.Key.RegionIdentifier,
                                 grp.Key.VIN,
                                 grp.Key.CardType,
                                 grp.Key.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query1 = from q in query1
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query1 = from q in query1
                             where q.CityID == cityId
                             select q;
                }

                if (ajancyId > 0)
                {
                    query1 = from q in query1
                             where q.AjancyID == ajancyId
                             select q;
                }

                if (driverCertification == 1)
                {
                    query1 = from q in query1
                             where q.DriverCertificationNo != null
                             select q;
                }
                else if (driverCertification == 2)
                {
                    query1 = from q in query1
                             where q.DriverCertificationNo == null
                             select q;
                }

                if (drivingLicenseType < 4)
                {
                    query1 = from q in query1
                             join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                             where dl.Type == drivingLicenseType
                             select q;
                }

                if (marriage < 2)
                {
                    query1 = from q in query1
                             where q.Marriage == marriage
                             select q;
                }

                if (carType > 0)
                {
                    query1 = from q in query1
                             join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                             where q.CarTypeID == carType
                             select q;
                }

                if (fuelType < 4)
                {
                    query1 = from q in query1
                             where q.FuelType == fuelType
                             select q;
                }

                if (fuelCardType < 3)
                {
                    query1 = from q in query1
                             where q.CardType == fuelCardType
                             select q;
                }

                if (!string.IsNullOrEmpty(carEngineNo))
                {
                    query1 = from q in query1
                             where q.EngineNo.Equals(carEngineNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(carChassisNo))
                {
                    query1 = from q in query1
                             where q.ChassisNo.Equals(carChassisNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query1 = from q in query1
                             where q.PAN.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query1 = from q in query1
                             where q.VIN.Equals(vin)
                             select q;
                }

                if (gender < 2)
                {
                    query1 = from q in query1
                             where q.Gender == gender
                             select q;
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query1 = from q in query1
                             where q.FirstName.Contains(firstName)
                             select q;
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query1 = from q in query1
                             where q.LastName.Contains(lastName)
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query1 = from q in query1
                             where q.NationalCode == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(birthCertificateNo))
                {
                    query1 = from q in query1
                             where q.BirthCertificateNo == birthCertificateNo
                             select q;
                }

                if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
                {
                    query1 = from q in query1
                             where q.TwoDigits == carPlateNumber_1 &&
                                      q.ThreeDigits == carPlateNumber_2 &&
                                      q.RegionIdentifier == carPlateNumber_3 &&
                                      q.Alphabet == alphabet
                             select q;
                }
                else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
                {
                    query1 = from q in query1
                             where q.ZCityID == zCityId &&
                                      q.ZNumber == zNumber
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query1 = from q in query1
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query1 = from q in query1
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }
                count = query1.Count();
                break;

            case "2": // Inactives
                var query2 = from p in db.Persons
                             join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                             join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join c in db.Cars on cpn.CarID equals c.CarID
                             join fc in db.FuelCards on c.CarID equals fc.CarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == ajancyType &&
                                   jd.LockOutDate != null &&
                                   dcc.LockOutDate != null
                             group p by new
                             {
                                 p.PersonID,
                                 p.FirstName,
                                 p.LastName,
                                 p.NationalCode,
                                 p.BirthCertificateNo,
                                 p.Marriage,
                                 p.Gender,
                                 p.SubmitDate,
                                 j.AjancyID,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 dc.DriverCertificationNo,
                                 c.CarTypeID,
                                 c.FuelType,
                                 c.ChassisNo,
                                 c.EngineNo,
                                 ZCityID = (short?)zpn.CityID,
                                 ZNumber = zpn.Number,
                                 pn.TwoDigits,
                                 pn.ThreeDigits,
                                 pn.Alphabet,
                                 pn.RegionIdentifier,
                                 c.VIN,
                                 fc.CardType,
                                 fc.PAN
                             } into grp
                             select new
                             {
                                 grp.Key.PersonID,
                                 grp.Key.FirstName,
                                 grp.Key.LastName,
                                 grp.Key.NationalCode,
                                 grp.Key.BirthCertificateNo,
                                 grp.Key.Marriage,
                                 grp.Key.Gender,
                                 grp.Key.AjancyID,
                                 grp.Key.CityID,
                                 grp.Key.ProvinceID,
                                 grp.Key.DriverCertificationNo,
                                 grp.Key.SubmitDate,
                                 grp.Key.CarTypeID,
                                 grp.Key.FuelType,
                                 grp.Key.ChassisNo,
                                 grp.Key.EngineNo,
                                 grp.Key.ZCityID,
                                 grp.Key.ZNumber,
                                 grp.Key.TwoDigits,
                                 grp.Key.ThreeDigits,
                                 grp.Key.Alphabet,
                                 grp.Key.RegionIdentifier,
                                 grp.Key.VIN,
                                 grp.Key.CardType,
                                 grp.Key.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query2 = from q in query2
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query2 = from q in query2
                             where q.CityID == cityId
                             select q;
                }

                if (ajancyId > 0)
                {
                    query2 = from q in query2
                             where q.AjancyID == ajancyId
                             select q;
                }

                if (driverCertification == 1)
                {
                    query2 = from q in query2
                             where q.DriverCertificationNo != null
                             select q;
                }
                else if (driverCertification == 2)
                {
                    query2 = from q in query2
                             where q.DriverCertificationNo == null
                             select q;
                }

                if (drivingLicenseType < 4)
                {
                    query2 = from q in query2
                             join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                             where dl.Type == drivingLicenseType
                             select q;
                }

                if (marriage < 2)
                {
                    query2 = from q in query2
                             where q.Marriage == marriage
                             select q;
                }

                if (carType > 0)
                {
                    query2 = from q in query2
                             join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                             where q.CarTypeID == carType
                             select q;
                }

                if (fuelType < 4)
                {
                    query2 = from q in query2
                             where q.FuelType == fuelType
                             select q;
                }

                if (fuelCardType < 3)
                {
                    query2 = from q in query2
                             where q.CardType == fuelCardType
                             select q;
                }

                if (!string.IsNullOrEmpty(carEngineNo))
                {
                    query2 = from q in query2
                             where q.EngineNo.Equals(carEngineNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(carChassisNo))
                {
                    query2 = from q in query2
                             where q.ChassisNo.Equals(carChassisNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query2 = from q in query2
                             where q.PAN.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query2 = from q in query2
                             where q.VIN.Equals(vin)
                             select q;
                }

                if (gender < 2)
                {
                    query2 = from q in query2
                             where q.Gender == gender
                             select q;
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query2 = from q in query2
                             where q.FirstName.Contains(firstName)
                             select q;
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query2 = from q in query2
                             where q.LastName.Contains(lastName)
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query2 = from q in query2
                             where q.NationalCode == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(birthCertificateNo))
                {
                    query2 = from q in query2
                             where q.BirthCertificateNo == birthCertificateNo
                             select q;
                }

                if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
                {
                    query2 = from q in query2
                             where q.TwoDigits == carPlateNumber_1 &&
                                      q.ThreeDigits == carPlateNumber_2 &&
                                      q.RegionIdentifier == carPlateNumber_3 &&
                                      q.Alphabet == alphabet
                             select q;
                }
                else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
                {
                    query2 = from q in query2
                             where q.ZCityID == zCityId &&
                                      q.ZNumber == zNumber
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query2 = from q in query2
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query2 = from q in query2
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }
                count = query2.Count();
                break;
        }

        db.Dispose();
        return count;
    }

    public IList<object> LoadDrivers(int maximumRows, int startRowIndex, byte provinceId, short cityId, byte ajancyType, int ajancyId, int driverCertification, byte drivingLicenseType, byte marriage, short carType, byte fuelType, byte fuelCardType, string carEngineNo, short zCityId, string zNumber, string carChassisNo, string pan, string vin, byte gender, string firstName, string lastName, string nationalCode, string birthCertificateNo, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet, string status, DateTime? dateFrom, DateTime? dateTo)
    {
        switch (status)
        {
            case "0": // All
                var query = from p in db.Persons
                            join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                            join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                            join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                            join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                            from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                            from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                            join c in db.Cars on cpn.CarID equals c.CarID
                            join fc in db.FuelCards on c.CarID equals fc.CarID
                            join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                            join ct in db.Cities on j.CityID equals ct.CityID
                            where j.AjancyType == ajancyType
                            orderby ct.ProvinceID, ct.Name, p.LastName
                            group p by new
                            {
                                p.PersonID,
                                p.FirstName,
                                p.LastName,
                                p.NationalCode,
                                p.BirthCertificateNo,
                                p.Marriage,
                                p.Gender,
                                p.SubmitDate,
                                j.AjancyID,
                                j.AjancyName,
                                ct.CityID,
                                ct.ProvinceID,
                                City = ct.Name,
                                dc.DriverCertificationNo,
                                c.CarTypeID,
                                c.FuelType,
                                c.ChassisNo,
                                c.EngineNo,
                                ZCityID = (short?)zpn.CityID,
                                ZNumber = zpn.Number,
                                pn.TwoDigits,
                                pn.ThreeDigits,
                                pn.Alphabet,
                                pn.RegionIdentifier,
                                c.VIN,
                                fc.CardType,
                                fc.PAN
                            } into grp
                            select new
                            {
                                grp.Key.PersonID,
                                grp.Key.FirstName,
                                grp.Key.LastName,
                                grp.Key.NationalCode,
                                grp.Key.BirthCertificateNo,
                                grp.Key.Marriage,
                                grp.Key.Gender,
                                grp.Key.AjancyID,
                                grp.Key.AjancyName,
                                grp.Key.CityID,
                                grp.Key.ProvinceID,
                                grp.Key.City,
                                grp.Key.DriverCertificationNo,
                                grp.Key.SubmitDate,
                                grp.Key.CarTypeID,
                                grp.Key.FuelType,
                                grp.Key.ChassisNo,
                                grp.Key.EngineNo,
                                grp.Key.ZCityID,
                                grp.Key.ZNumber,
                                grp.Key.TwoDigits,
                                grp.Key.ThreeDigits,
                                grp.Key.Alphabet,
                                grp.Key.RegionIdentifier,
                                grp.Key.VIN,
                                grp.Key.CardType,
                                grp.Key.PAN
                            };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query = from q in query
                            where q.ProvinceID == provinceId
                            select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query = from q in query
                            where q.CityID == cityId
                            select q;
                }

                if (ajancyId > 0)
                {
                    query = from q in query
                            where q.AjancyID == ajancyId
                            select q;
                }

                if (driverCertification == 1)
                {
                    query = from q in query
                            where q.DriverCertificationNo != null
                            select q;
                }
                else if (driverCertification == 2)
                {
                    query = from q in query
                            where q.DriverCertificationNo == null
                            select q;
                }

                if (drivingLicenseType < 4)
                {
                    query = from q in query
                            join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                            where dl.Type == drivingLicenseType
                            select q;
                }

                if (marriage < 2)
                {
                    query = from q in query
                            where q.Marriage == marriage
                            select q;
                }

                if (carType > 0)
                {
                    query = from q in query
                            join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                            where q.CarTypeID == carType
                            select q;
                }

                if (fuelType < 4)
                {
                    query = from q in query
                            where q.FuelType == fuelType
                            select q;
                }

                if (fuelCardType < 3)
                {
                    query = from q in query
                            where q.CardType == fuelCardType
                            select q;
                }

                if (!string.IsNullOrEmpty(carEngineNo))
                {
                    query = from q in query
                            where q.EngineNo.Equals(carEngineNo)
                            select q;
                }

                if (!string.IsNullOrEmpty(carChassisNo))
                {
                    query = from q in query
                            where q.ChassisNo.Equals(carChassisNo)
                            select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query = from q in query
                            where q.PAN.Equals(pan)
                            select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query = from q in query
                            where q.VIN.Equals(vin)
                            select q;
                }

                if (gender < 2)
                {
                    query = from q in query
                            where q.Gender == gender
                            select q;
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query = from q in query
                            where q.FirstName.Contains(firstName)
                            select q;
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query = from q in query
                            where q.LastName.Contains(lastName)
                            select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query = from q in query
                            where q.NationalCode == nationalCode
                            select q;
                }

                if (!string.IsNullOrEmpty(birthCertificateNo))
                {
                    query = from q in query
                            where q.BirthCertificateNo == birthCertificateNo
                            select q;
                }

                if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
                {
                    query = from q in query
                            where q.TwoDigits == carPlateNumber_1 &&
                                     q.ThreeDigits == carPlateNumber_2 &&
                                     q.RegionIdentifier == carPlateNumber_3 &&
                                     q.Alphabet == alphabet
                            select q;
                }
                else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
                {
                    query = from q in query
                            where q.ZCityID == zCityId &&
                                     q.ZNumber == zNumber
                            select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query = from q in query
                            where q.SubmitDate == dateFrom
                            select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query = from q in query
                            where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                            select q;
                }
                return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();

            case "1": // Actives
                var query1 = from p in db.Persons
                             join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                             join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join c in db.Cars on cpn.CarID equals c.CarID
                             join fc in db.FuelCards on c.CarID equals fc.CarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == ajancyType &&
                                        jd.LockOutDate == null &&
                                        dcc.LockOutDate == null
                             orderby ct.ProvinceID, ct.Name, p.LastName
                             group p by new
                             {
                                 p.PersonID,
                                 p.FirstName,
                                 p.LastName,
                                 p.NationalCode,
                                 p.BirthCertificateNo,
                                 p.Marriage,
                                 p.Gender,
                                 p.SubmitDate,
                                 j.AjancyID,
                                 j.AjancyName,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 City = ct.Name,
                                 dc.DriverCertificationNo,
                                 c.CarTypeID,
                                 c.FuelType,
                                 c.ChassisNo,
                                 c.EngineNo,
                                 ZCityID = (short?)zpn.CityID,
                                 ZNumber = zpn.Number,
                                 pn.TwoDigits,
                                 pn.ThreeDigits,
                                 pn.Alphabet,
                                 pn.RegionIdentifier,
                                 c.VIN,
                                 fc.CardType,
                                 fc.PAN
                             } into grp
                             select new
                             {
                                 grp.Key.PersonID,
                                 grp.Key.FirstName,
                                 grp.Key.LastName,
                                 grp.Key.NationalCode,
                                 grp.Key.BirthCertificateNo,
                                 grp.Key.Marriage,
                                 grp.Key.Gender,
                                 grp.Key.AjancyID,
                                 grp.Key.AjancyName,
                                 grp.Key.CityID,
                                 grp.Key.ProvinceID,
                                 grp.Key.City,
                                 grp.Key.DriverCertificationNo,
                                 grp.Key.SubmitDate,
                                 grp.Key.CarTypeID,
                                 grp.Key.FuelType,
                                 grp.Key.ChassisNo,
                                 grp.Key.EngineNo,
                                 grp.Key.ZCityID,
                                 grp.Key.ZNumber,
                                 grp.Key.TwoDigits,
                                 grp.Key.ThreeDigits,
                                 grp.Key.Alphabet,
                                 grp.Key.RegionIdentifier,
                                 grp.Key.VIN,
                                 grp.Key.CardType,
                                 grp.Key.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query1 = from q in query1
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query1 = from q in query1
                             where q.CityID == cityId
                             select q;
                }

                if (ajancyId > 0)
                {
                    query1 = from q in query1
                             where q.AjancyID == ajancyId
                             select q;
                }

                if (driverCertification == 1)
                {
                    query1 = from q in query1
                             where q.DriverCertificationNo != null
                             select q;
                }
                else if (driverCertification == 2)
                {
                    query1 = from q in query1
                             where q.DriverCertificationNo == null
                             select q;
                }

                if (drivingLicenseType < 4)
                {
                    query1 = from q in query1
                             join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                             where dl.Type == drivingLicenseType
                             select q;
                }

                if (marriage < 2)
                {
                    query1 = from q in query1
                             where q.Marriage == marriage
                             select q;
                }

                if (carType > 0)
                {
                    query1 = from q in query1
                             join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                             where q.CarTypeID == carType
                             select q;
                }

                if (fuelType < 4)
                {
                    query1 = from q in query1
                             where q.FuelType == fuelType
                             select q;
                }

                if (fuelCardType < 3)
                {
                    query1 = from q in query1
                             where q.CardType == fuelCardType
                             select q;
                }

                if (!string.IsNullOrEmpty(carEngineNo))
                {
                    query1 = from q in query1
                             where q.EngineNo.Equals(carEngineNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(carChassisNo))
                {
                    query1 = from q in query1
                             where q.ChassisNo.Equals(carChassisNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query1 = from q in query1
                             where q.PAN.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query1 = from q in query1
                             where q.VIN.Equals(vin)
                             select q;
                }

                if (gender < 2)
                {
                    query1 = from q in query1
                             where q.Gender == gender
                             select q;
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query1 = from q in query1
                             where q.FirstName.Contains(firstName)
                             select q;
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query1 = from q in query1
                             where q.LastName.Contains(lastName)
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query1 = from q in query1
                             where q.NationalCode == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(birthCertificateNo))
                {
                    query1 = from q in query1
                             where q.BirthCertificateNo == birthCertificateNo
                             select q;
                }

                if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
                {
                    query1 = from q in query1
                             where q.TwoDigits == carPlateNumber_1 &&
                                      q.ThreeDigits == carPlateNumber_2 &&
                                      q.RegionIdentifier == carPlateNumber_3 &&
                                      q.Alphabet == alphabet
                             select q;
                }
                else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
                {
                    query1 = from q in query1
                             where q.ZCityID == zCityId &&
                                      q.ZNumber == zNumber
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query1 = from q in query1
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query1 = from q in query1
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }
                return query1.Skip(startRowIndex).Take(maximumRows).ToList<object>();

            case "2": // Inactives
                var query2 = from p in db.Persons
                             join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                             join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join c in db.Cars on cpn.CarID equals c.CarID
                             join fc in db.FuelCards on c.CarID equals fc.CarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID
                             where j.AjancyType == ajancyType &&
                                        jd.LockOutDate != null &&
                                        dcc.LockOutDate != null
                             orderby ct.ProvinceID, ct.Name, p.LastName
                             group p by new
                             {
                                 p.PersonID,
                                 p.FirstName,
                                 p.LastName,
                                 p.NationalCode,
                                 p.BirthCertificateNo,
                                 p.Marriage,
                                 p.Gender,
                                 p.SubmitDate,
                                 j.AjancyID,
                                 j.AjancyName,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 City = ct.Name,
                                 dc.DriverCertificationNo,
                                 c.CarTypeID,
                                 c.FuelType,
                                 c.ChassisNo,
                                 c.EngineNo,
                                 ZCityID = (short?)zpn.CityID,
                                 ZNumber = zpn.Number,
                                 pn.TwoDigits,
                                 pn.ThreeDigits,
                                 pn.Alphabet,
                                 pn.RegionIdentifier,
                                 c.VIN,
                                 fc.CardType,
                                 fc.PAN
                             } into grp
                             select new
                             {
                                 grp.Key.PersonID,
                                 grp.Key.FirstName,
                                 grp.Key.LastName,
                                 grp.Key.NationalCode,
                                 grp.Key.BirthCertificateNo,
                                 grp.Key.Marriage,
                                 grp.Key.Gender,
                                 grp.Key.AjancyID,
                                 grp.Key.AjancyName,
                                 grp.Key.CityID,
                                 grp.Key.ProvinceID,
                                 grp.Key.City,
                                 grp.Key.DriverCertificationNo,
                                 grp.Key.SubmitDate,
                                 grp.Key.CarTypeID,
                                 grp.Key.FuelType,
                                 grp.Key.ChassisNo,
                                 grp.Key.EngineNo,
                                 grp.Key.ZCityID,
                                 grp.Key.ZNumber,
                                 grp.Key.TwoDigits,
                                 grp.Key.ThreeDigits,
                                 grp.Key.Alphabet,
                                 grp.Key.RegionIdentifier,
                                 grp.Key.VIN,
                                 grp.Key.CardType,
                                 grp.Key.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query2 = from q in query2
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query2 = from q in query2
                             where q.CityID == cityId
                             select q;
                }

                if (ajancyId > 0)
                {
                    query2 = from q in query2
                             where q.AjancyID == ajancyId
                             select q;
                }

                if (driverCertification == 1)
                {
                    query2 = from q in query2
                             where q.DriverCertificationNo != null
                             select q;
                }
                else if (driverCertification == 2)
                {
                    query2 = from q in query2
                             where q.DriverCertificationNo == null
                             select q;
                }

                if (drivingLicenseType < 4)
                {
                    query2 = from q in query2
                             join dl in db.DrivingLicenses on q.PersonID equals dl.PersonID
                             where dl.Type == drivingLicenseType
                             select q;
                }

                if (marriage < 2)
                {
                    query2 = from q in query2
                             where q.Marriage == marriage
                             select q;
                }

                if (carType > 0)
                {
                    query2 = from q in query2
                             join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                             where q.CarTypeID == carType
                             select q;
                }

                if (fuelType < 4)
                {
                    query2 = from q in query2
                             where q.FuelType == fuelType
                             select q;
                }

                if (fuelCardType < 3)
                {
                    query2 = from q in query2
                             where q.CardType == fuelCardType
                             select q;
                }

                if (!string.IsNullOrEmpty(carEngineNo))
                {
                    query2 = from q in query2
                             where q.EngineNo.Equals(carEngineNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(carChassisNo))
                {
                    query2 = from q in query2
                             where q.ChassisNo.Equals(carChassisNo)
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query2 = from q in query2
                             where q.PAN.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query2 = from q in query2
                             where q.VIN.Equals(vin)
                             select q;
                }

                if (gender < 2)
                {
                    query2 = from q in query2
                             where q.Gender == gender
                             select q;
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query2 = from q in query2
                             where q.FirstName.Contains(firstName)
                             select q;
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query2 = from q in query2
                             where q.LastName.Contains(lastName)
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query2 = from q in query2
                             where q.NationalCode == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(birthCertificateNo))
                {
                    query2 = from q in query2
                             where q.BirthCertificateNo == birthCertificateNo
                             select q;
                }

                if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
                {
                    query2 = from q in query2
                             where q.TwoDigits == carPlateNumber_1 &&
                                      q.ThreeDigits == carPlateNumber_2 &&
                                      q.RegionIdentifier == carPlateNumber_3 &&
                                      q.Alphabet == alphabet
                             select q;
                }
                else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
                {
                    query2 = from q in query2
                             where q.ZCityID == zCityId &&
                                      q.ZNumber == zNumber
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query2 = from q in query2
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query2 = from q in query2
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }
                return query2.Skip(startRowIndex).Take(maximumRows).ToList<object>();
        }
        return null;
    }

    #endregion

    #region DriversReport2.aspx

    public int GetDriversCount2(byte provinceId, short cityId, byte ajancyType, int ajancyId, int driverCertification, short carType, byte fuelType, byte fuelCardType, string carEngineNo, short zCityId, string zNumber, string carChassisNo, string pan, string vin, byte gender, string firstName, string lastName, string nationalCode, string birthCertificateNo, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet, DateTime? dateFrom, DateTime? dateTo)
    {
        var drivers = (from p in db.Persons
                       join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                       join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                       join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                       join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                       from pln in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                       from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                       join c in db.Cars on cpn.CarID equals c.CarID
                       join fc in db.FuelCards on c.CarID equals fc.CarID
                       join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                       join ct in db.Cities on j.CityID equals ct.CityID
                       where j.AjancyType == ajancyType && !(from fcs in db.FuelCardSubstitutions
                                                             join fc2 in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc2.FuelCardID
                                                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                                                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                                                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                                                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                                                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                                                             join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID
                                                             where fcs.PersonalTypeFuelCardID != null && j2.AjancyType == ajancyType &&
                                                                   (c2.ChassisNo == null && c2.EngineNo == null) &&
                                                                   fcs.SubmitDate < new DateTime(2013, 9, 21) &&
                                                                   ((fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))
                                                             select dc2.PersonID).Contains(p.PersonID)
                       select new
                       {
                           p.PersonID,
                           p.NationalCode,
                           p.FirstName,
                           p.LastName,
                           p.Gender,
                           p.BirthCertificateNo,
                           p.SubmitDate,
                           j.AjancyID,
                           dc.DriverCertificationNo,
                           c.CarTypeID,
                           c.FuelType,
                           c.EngineNo,
                           c.ChassisNo,
                           c.VIN,
                           fc.PAN,
                           fc.CardType,
                           ct.CityID,
                           ct.ProvinceID,
                           pln.TwoDigits,
                           pln.ThreeDigits,
                           pln.Alphabet,
                           pln.RegionIdentifier,
                           ZCityID = (short?)zpn.CityID,
                           ZNumber = zpn.Number
                       }).Distinct();

        if (provinceId > 0 && cityId == 0) // Just province
        {
            drivers = from q in drivers
                      where q.ProvinceID == provinceId
                      select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            drivers = from q in drivers
                      where q.CityID == cityId
                      select q;
        }

        if (ajancyId > 0)
        {
            drivers = from q in drivers
                      where q.AjancyID == ajancyId
                      select q;
        }

        if (driverCertification == 1)
        {
            drivers = from q in drivers
                      where q.DriverCertificationNo != null
                      select q;
        }
        else if (driverCertification == 2)
        {
            drivers = from q in drivers
                      where q.DriverCertificationNo == null
                      select q;
        }

        if (carType > 0)
        {
            drivers = from q in drivers
                      join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                      where q.CarTypeID == carType
                      select q;
        }

        if (fuelType < 4)
        {
            drivers = from q in drivers
                      where q.FuelType == fuelType
                      select q;
        }

        if (fuelCardType < 3)
        {
            drivers = from q in drivers
                      where q.CardType == fuelCardType
                      select q;
        }

        if (!string.IsNullOrEmpty(carEngineNo))
        {
            drivers = from q in drivers
                      where q.EngineNo.Equals(carEngineNo)
                      select q;
        }

        if (!string.IsNullOrEmpty(carChassisNo))
        {
            drivers = from q in drivers
                      where q.ChassisNo.Equals(carChassisNo)
                      select q;
        }

        if (!string.IsNullOrEmpty(pan))
        {
            drivers = from q in drivers
                      where q.PAN.Equals(pan)
                      select q;
        }

        if (!string.IsNullOrEmpty(vin))
        {
            drivers = from q in drivers
                      where q.VIN.Equals(vin)
                      select q;
        }

        if (gender < 2)
        {
            drivers = from q in drivers
                      where q.Gender == gender
                      select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            drivers = from q in drivers
                      where q.FirstName.Contains(firstName)
                      select q;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            drivers = from q in drivers
                      where q.LastName.Contains(lastName)
                      select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            drivers = from q in drivers
                      where q.NationalCode == nationalCode
                      select q;
        }

        if (!string.IsNullOrEmpty(birthCertificateNo))
        {
            drivers = from q in drivers
                      where q.BirthCertificateNo == birthCertificateNo
                      select q;
        }

        if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) &&
            !string.IsNullOrEmpty(carPlateNumber_3))
        {
            drivers = from q in drivers
                      where q.TwoDigits == carPlateNumber_1 &&
                            q.ThreeDigits == carPlateNumber_2 &&
                            q.RegionIdentifier == carPlateNumber_3 &&
                            q.Alphabet == alphabet
                      select q;
        }
        else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
        {
            drivers = from q in drivers
                      where q.ZCityID == zCityId &&
                            q.ZNumber == zNumber
                      select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            drivers = from q in drivers
                      where q.SubmitDate == dateFrom
                      select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            drivers = from q in drivers
                      where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                      select q;
        }

        int count = drivers.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadDrivers2(int maximumRows, int startRowIndex, byte provinceId, short cityId, byte ajancyType,
        int ajancyId, int driverCertification, short carType, byte fuelType, byte fuelCardType, string carEngineNo,
        short zCityId, string zNumber, string carChassisNo, string pan, string vin, byte gender, string firstName,
        string lastName, string nationalCode, string birthCertificateNo, string carPlateNumber_1,
        string carPlateNumber_2, string carPlateNumber_3, string alphabet, DateTime? dateFrom, DateTime? dateTo)
    {
        var drivers = (from p in db.Persons
                       join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                       join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                       join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                       join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                       from pln in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                       from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                       join c in db.Cars on cpn.CarID equals c.CarID
                       join fc in db.FuelCards on c.CarID equals fc.CarID
                       join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                       join ct in db.Cities on j.CityID equals ct.CityID
                       where j.AjancyType == ajancyType && !(from fcs in db.FuelCardSubstitutions
                                                             join fc2 in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc2.FuelCardID
                                                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                                                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                                                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                                                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                                                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                                                             join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID
                                                             where fcs.PersonalTypeFuelCardID != null && j2.AjancyType == ajancyType &&
                                                                   (c2.ChassisNo == null && c2.EngineNo == null) &&
                                                                   fcs.SubmitDate < new DateTime(2013, 9, 21) &&
                                                                   ((fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))
                                                             select dc2.PersonID).Contains(p.PersonID)
                       select new
                        {
                            p.PersonID,
                            p.NationalCode,
                            p.FirstName,
                            p.LastName,
                            p.Gender,
                            p.BirthCertificateNo,
                            p.SubmitDate,
                            j.AjancyID,
                            j.AjancyName,
                            City = ct.Name,
                            dc.DriverCertificationNo,
                            c.CarTypeID,
                            c.FuelType,
                            c.EngineNo,
                            c.ChassisNo,
                            c.VIN,
                            fc.PAN,
                            fc.CardType,
                            ct.CityID,
                            ct.ProvinceID,
                            pln.TwoDigits,
                            pln.ThreeDigits,
                            pln.Alphabet,
                            pln.RegionIdentifier,
                            ZCityID = (short?)zpn.CityID,
                            ZNumber = zpn.Number
                        }).Distinct();

        if (provinceId > 0 && cityId == 0) // Just province
        {
            drivers = from q in drivers
                      where q.ProvinceID == provinceId
                      select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            drivers = from q in drivers
                      where q.CityID == cityId
                      select q;
        }

        if (ajancyId > 0)
        {
            drivers = from q in drivers
                      where q.AjancyID == ajancyId
                      select q;
        }

        if (driverCertification == 1)
        {
            drivers = from q in drivers
                      where q.DriverCertificationNo != null
                      select q;
        }
        else if (driverCertification == 2)
        {
            drivers = from q in drivers
                      where q.DriverCertificationNo == null
                      select q;
        }

        if (carType > 0)
        {
            drivers = from q in drivers
                      join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                      where q.CarTypeID == carType
                      select q;
        }

        if (fuelType < 4)
        {
            drivers = from q in drivers
                      where q.FuelType == fuelType
                      select q;
        }

        if (fuelCardType < 3)
        {
            drivers = from q in drivers
                      where q.CardType == fuelCardType
                      select q;
        }

        if (!string.IsNullOrEmpty(carEngineNo))
        {
            drivers = from q in drivers
                      where q.EngineNo.Equals(carEngineNo)
                      select q;
        }

        if (!string.IsNullOrEmpty(carChassisNo))
        {
            drivers = from q in drivers
                      where q.ChassisNo.Equals(carChassisNo)
                      select q;
        }

        if (!string.IsNullOrEmpty(pan))
        {
            drivers = from q in drivers
                      where q.PAN.Equals(pan)
                      select q;
        }

        if (!string.IsNullOrEmpty(vin))
        {
            drivers = from q in drivers
                      where q.VIN.Equals(vin)
                      select q;
        }

        if (gender < 2)
        {
            drivers = from q in drivers
                      where q.Gender == gender
                      select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            drivers = from q in drivers
                      where q.FirstName.Contains(firstName)
                      select q;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            drivers = from q in drivers
                      where q.LastName.Contains(lastName)
                      select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            drivers = from q in drivers
                      where q.NationalCode == nationalCode
                      select q;
        }

        if (!string.IsNullOrEmpty(birthCertificateNo))
        {
            drivers = from q in drivers
                      where q.BirthCertificateNo == birthCertificateNo
                      select q;
        }

        if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) &&
            !string.IsNullOrEmpty(carPlateNumber_3))
        {
            drivers = from q in drivers
                      where q.TwoDigits == carPlateNumber_1 &&
                            q.ThreeDigits == carPlateNumber_2 &&
                            q.RegionIdentifier == carPlateNumber_3 &&
                            q.Alphabet == alphabet
                      select q;
        }
        else if (zCityId > 0 && !string.IsNullOrEmpty(zNumber))
        {
            drivers = from q in drivers
                      where q.ZCityID == zCityId &&
                            q.ZNumber == zNumber
                      select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            drivers = from q in drivers
                      where q.SubmitDate == dateFrom
                      select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            drivers = from q in drivers
                      where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                      select q;
        }

        return (from q in drivers
                select new
                {
                    q.PersonID,
                    q.NationalCode,
                    q.FirstName,
                    q.LastName,
                    q.SubmitDate,
                    q.AjancyName,
                    q.City,
                    q.DriverCertificationNo,
                    q.ZCityID
                }).Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region DriversList.aspx

    public int GetAjancyDriversCount(int ajancyId, int driverCertification, byte carType, byte fuelType, byte fuelCardType, string firstName, string lastName, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet)
    {
        var query = from p in db.Persons
                    join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join fc in db.FuelCards on c.CarID equals fc.CarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    where j.AjancyID == ajancyId && dcc.LockOutDate == null && jd.LockOutDate == null && fc.DiscardDate == null
                    select new
                    {
                        p.PersonID,
                        p.FirstName,
                        p.LastName,
                        dc.DriverCertificationNo,
                        c.CarTypeID,
                        c.FuelType,
                        pn.TwoDigits,
                        pn.ThreeDigits,
                        pn.Alphabet,
                        pn.RegionIdentifier,
                        fc.CardType
                    };

        if (driverCertification == 1)
        {
            query = from q in query
                    where q.DriverCertificationNo != null
                    select q;
        }
        else if (driverCertification == 2)
        {
            query = from q in query
                    where q.DriverCertificationNo == null
                    select q;
        }

        if (carType > 0)
        {
            query = from q in query
                    join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                    where q.CarTypeID == carType
                    select q;
        }
        if (fuelType > 0)
        {
            query = from q in query
                    where q.FuelType == fuelType
                    select q;
        }
        if (fuelCardType > 0)
        {
            query = from q in query
                    where q.CardType == fuelCardType
                    select q;
        }
        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }
        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }
        if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
        {
            query = from q in query
                    where q.TwoDigits == carPlateNumber_1 &&
                             q.ThreeDigits == carPlateNumber_2 &&
                             q.RegionIdentifier == carPlateNumber_3 &&
                             q.Alphabet == alphabet
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadAjancyDrivers(int maximumRows, int startRowIndex, int ajancyId, int driverCertification, byte carType, byte fuelType, byte fuelCardType, string firstName, string lastName, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet)
    {
        var query = from p in db.Persons
                    join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join fc in db.FuelCards on c.CarID equals fc.CarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join u in db.Users on p.PersonID equals u.PersonID
                    join cty in db.Cities on u.CityID equals cty.CityID
                    where j.AjancyID == ajancyId && dcc.LockOutDate == null && jd.LockOutDate == null && fc.DiscardDate == null
                    orderby cty.Name, p.LastName
                    select new
                    {
                        p.PersonID,
                        p.FirstName,
                        p.LastName,
                        p.NationalCode,
                        dc.DriverCertificationNo,
                        c.CarTypeID,
                        c.CarType.TypeName,
                        c.FuelType,
                        pn.TwoDigits,
                        pn.ThreeDigits,
                        pn.Alphabet,
                        pn.RegionIdentifier,
                        fc.CardType,
                        Utilities = Utilities(p.PersonID, dc.DriverCertificationNo, jd.AjancyDriverID, c.FuelType, fc.CardType)
                    };

        if (driverCertification == 1)
        {
            query = from q in query
                    where q.DriverCertificationNo != null
                    select q;
        }
        else if (driverCertification == 2)
        {
            query = from q in query
                    where q.DriverCertificationNo == null
                    select q;
        }

        if (carType > 0)
        {
            query = from q in query
                    join ct in db.CarTypes on q.CarTypeID equals ct.CarTypeID
                    where q.CarTypeID == carType
                    select q;
        }
        if (fuelType > 0)
        {
            query = from q in query
                    where q.FuelType == fuelType
                    select q;
        }
        if (fuelCardType > 0)
        {
            query = from q in query
                    where q.CardType == fuelCardType
                    select q;
        }
        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }
        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }
        if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
        {
            query = from q in query
                    where q.TwoDigits == carPlateNumber_1 &&
                             q.ThreeDigits == carPlateNumber_2 &&
                             q.RegionIdentifier == carPlateNumber_3 &&
                             q.Alphabet == alphabet
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    private string Utilities(int personId, string driverCertificationNo, int ajancyDriverId, byte fuelType, byte fuelCardType)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("<span class='item-box' onclick='javascript:showDialog(0, this, {0})'>شکایت</span>", ajancyDriverId);
        sb.AppendFormat("<span class='item-box' onclick='javascript:showDialog(4, this, {0})'>درخواست پایان کار</span>", ajancyDriverId);

        if (!string.IsNullOrEmpty(driverCertificationNo))
        {
            if (fuelType != (byte)Public.FuelType.Petrol_AND_CNG)
            {
                sb.AppendFormat("<span class='item-box' onclick='javascript:showDialog(1, this, {0})'>درخواست دوگانه سوز</span>", ajancyDriverId);
            }
            //if (fuelCardType == (byte)Public.FuelCardType.Personal)
            //{
            //    sb.AppendFormat("<span class='item-box' onclick='javascript:showDialog(2, this, {0})'>درخواست کارت سوخت آژانسی</span>", ajancyDriverId);
            //}
            if (!string.IsNullOrEmpty(driverCertificationNo))
            {
                sb.AppendFormat("<span class='item-box' onclick='javascript:showDialog(3, this, {0})'>درخواست بیمه</span>", ajancyDriverId);
            }
        }
        //else
        {
            sb.AppendFormat("<a class='item-box' target='_blank' href='Driver.aspx?id={0}'>اطلاعات راننده</a>", TamperProofString.QueryStringEncode(personId.ToString()));
        }
        return sb.ToString();
    }

    #endregion

    #region CertificationlessDrivers.aspx

    public int GetCertificationlessDriversCount(DateTime? dateFrom, DateTime? dateTo, int ajancyId, byte carType, string firstName, string lastName, string nationalCode, string birthCertificateNo, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet)
    {
        var query = from p in db.Persons
                    join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    where dc.DriverCertificationNo == null && dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy && dcc.LockOutDate == null
                    select new
                    {
                        p.NationalCode,
                        p.BirthCertificateNo,
                        p.FirstName,
                        p.LastName,
                        j.AjancyID,
                        j.AjancyName,
                        c.CarTypeID,
                        dc.SubmitDate,
                        pn.TwoDigits,
                        pn.ThreeDigits,
                        pn.Alphabet,
                        pn.RegionIdentifier,
                    };

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        if (ajancyId > 0)
        {
            query = from q in query
                    where q.AjancyID == ajancyId
                    select q;
        }

        if (carType > 0)
        {
            query = from q in query
                    where q.CarTypeID == carType
                    select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }
        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode == nationalCode
                    select q;
        }

        if (!string.IsNullOrEmpty(birthCertificateNo))
        {
            query = from q in query
                    where q.BirthCertificateNo == birthCertificateNo
                    select q;
        }

        if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
        {
            query = from q in query
                    where q.TwoDigits == carPlateNumber_1 &&
                             q.ThreeDigits == carPlateNumber_2 &&
                             q.RegionIdentifier == carPlateNumber_3 &&
                             q.Alphabet == alphabet
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadCertificationlessDrivers(int maximumRows, int startRowIndex, DateTime? dateFrom, DateTime? dateTo, int ajancyId, byte carType, string firstName, string lastName, string nationalCode, string birthCertificateNo, string carPlateNumber_1, string carPlateNumber_2, string carPlateNumber_3, string alphabet)
    {
        var query = from p in db.Persons
                    join dc in db.DriverCertifications on p.PersonID equals dc.PersonID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join pn in db.PlateNumbers on cpn.PlateNumberID equals pn.PlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    where dc.DriverCertificationNo == null && dc.CertificationType == (byte)Public.AjancyType.TaxiAjancy && dcc.LockOutDate == null
                    orderby p.LastName
                    select new
                    {
                        p.PersonID,
                        p.NationalCode,
                        p.BirthCertificateNo,
                        p.FirstName,
                        p.LastName,
                        Gender = p.Gender == 0 ? "مرد" : "زن",
                        dc.DriverCertificationID,
                        j.AjancyID,
                        j.AjancyName,
                        c.CarTypeID,
                        dc.SubmitDate,
                        pn.TwoDigits,
                        pn.ThreeDigits,
                        pn.Alphabet,
                        pn.RegionIdentifier,
                    };

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        if (ajancyId > 0)
        {
            query = from q in query
                    where q.AjancyID == ajancyId
                    select q;
        }

        if (carType > 0)
        {
            query = from q in query
                    where q.CarTypeID == carType
                    select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }
        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode == nationalCode
                    select q;
        }

        if (!string.IsNullOrEmpty(birthCertificateNo))
        {
            query = from q in query
                    where q.BirthCertificateNo == birthCertificateNo
                    select q;
        }

        if (!string.IsNullOrEmpty(carPlateNumber_1) && !string.IsNullOrEmpty(carPlateNumber_2) && !string.IsNullOrEmpty(carPlateNumber_3))
        {
            query = from q in query
                    where q.TwoDigits == carPlateNumber_1 &&
                             q.ThreeDigits == carPlateNumber_2 &&
                             q.RegionIdentifier == carPlateNumber_3 &&
                             q.Alphabet == alphabet
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region BusinessLicenseRequests.aspx

    public int GetBusinessLicenseRequestsCount(DateTime? dateFrom, DateTime? dateTo, byte ajancyType)
    {
        var query = from p in db.Persons
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ur in db.UsersInRoles on u.UserID equals ur.UserID
                    join jp in db.AjancyPartners on ur.UserRoleID equals jp.UserRoleID
                    join j in db.Ajancies on jp.AjancyID equals j.AjancyID
                    join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID into ajcbl
                    from jbl in ajcbl.DefaultIfEmpty()
                    where jbl.BusinessLicenseNo == null &&
                             ur.RoleID == (short)Public.Role.AjancyManager &&
                             j.AjancyType == ajancyType
                    select new
                    {
                        j.SubmitDate,
                    };

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadBusinessLicenseRequests(int maximumRows, int startRowIndex, DateTime? dateFrom, DateTime? dateTo, byte ajancyType)
    {
        var query = from p in db.Persons
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ur in db.UsersInRoles on u.UserID equals ur.UserID
                    join jp in db.AjancyPartners on ur.UserRoleID equals jp.UserRoleID
                    join j in db.Ajancies on jp.AjancyID equals j.AjancyID
                    join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID into ajcbl
                    from jbl in ajcbl.DefaultIfEmpty()
                    where jbl.BusinessLicenseNo == null &&
                             ur.RoleID == (short)Public.Role.AjancyManager &&
                             j.AjancyType == ajancyType
                    orderby j.SubmitDate
                    select new
                    {
                        j.AjancyID,
                        j.SubmitDate,
                        p.PersonID,
                        p.FirstName,
                        p.LastName,
                        p.NationalCode,
                        Gender = p.Gender == 0 ? "مرد" : "زن"
                    };

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region UsersList.aspx

    public int GetUsersCount(byte provinceId, short cityId, short roleId, string firstName, string lastName, string nationalCode)
    {
        var query = from p in db.Persons
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ur in db.UsersInRoles on u.UserID equals ur.UserID
                    select new
                    {
                        u.ProvinceID,
                        u.CityID,
                        ur.RoleID,
                        p.PersonID,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (roleId > 0)
        {
            query = from q in query
                    where q.RoleID == roleId
                    select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode.Equals(nationalCode)
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadUsers(byte provinceId, short cityId, int maximumRows, int startRowIndex, short roleId, string firstName, string lastName, string nationalCode)
    {
        var query = from p in db.Persons
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ur in db.UsersInRoles on u.UserID equals ur.UserID
                    join pv in db.Provinces on u.ProvinceID equals pv.ProvinceID
                    join ct in db.Cities on u.CityID equals ct.CityID
                    orderby p.LastName
                    select new
                    {
                        u.UserID,
                        u.ProvinceID,
                        u.CityID,
                        ur.RoleID,
                        ur.LastLoginDate,
                        p.PersonID,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        Province = pv.Name,
                        City = ct.Name
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (roleId > 0)
        {
            query = from q in query
                    where q.RoleID == roleId
                    select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode.Equals(nationalCode)
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region Union_UsersList.aspx

    public int GetUnionUsersCount(byte provinceId, short cityId, short roleId, string firstName, string lastName, string nationalCode)
    {
        var query = from p in db.Persons
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ur in db.UsersInRoles on u.UserID equals ur.UserID
                    select new
                    {
                        u.ProvinceID,
                        u.CityID,
                        ur.RoleID,
                        p.PersonID,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (roleId > 0)
        {
            query = from q in query
                    where q.RoleID == roleId
                    select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode.Equals(nationalCode)
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadUnionUsers(int maximumRows, int startRowIndex, byte provinceId, short cityId, short roleId, string firstName, string lastName, string nationalCode)
    {
        var query = from p in db.Persons
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ur in db.UsersInRoles on u.UserID equals ur.UserID
                    join pv in db.Provinces on u.ProvinceID equals pv.ProvinceID
                    join ct in db.Cities on u.CityID equals ct.CityID
                    orderby p.LastName
                    select new
                    {
                        u.ProvinceID,
                        u.CityID,
                        ur.RoleID,
                        ur.LastLoginDate,
                        u.UserID,
                        p.PersonID,
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        Province = pv.Name,
                        City = ct.Name
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (roleId > 0)
        {
            query = from q in query
                    where q.RoleID == roleId
                    select q;
        }

        if (!string.IsNullOrEmpty(firstName))
        {
            query = from q in query
                    where q.FirstName.Contains(firstName)
                    select q;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            query = from q in query
                    where q.LastName.Contains(lastName)
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode.Equals(nationalCode)
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region ActiveAjancies.aspx

    public int GetAjanciesCount(byte provinceId, short cityId, byte ajancyType, string ajancyName)
    {
        var query = from j in db.Ajancies
                    join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                    join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                    join ur in db.UsersInRoles on jp.UserRoleID equals ur.UserRoleID
                    join u in db.Users on ur.UserID equals u.UserID
                    join p in db.Persons on u.PersonID equals p.PersonID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where jp.LockOutDate == null &&
                               j.AjancyType == ajancyType && bl.LockOutDate == null
                    select new
                    {
                        ct.CityID,
                        ct.ProvinceID,
                        j.AjancyID,
                        j.AjancyType,
                        j.AjancyName
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        switch (ajancyType)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (byte)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (byte)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (byte)Public.AjancyType.Academy
                        select q;
                break;
        }

        if (!string.IsNullOrEmpty(ajancyName))
        {
            query = from q in query
                    where q.AjancyName.Contains(ajancyName)
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadAjancies(int maximumRows, int startRowIndex, byte provinceId, short cityId, byte ajancyType, string ajancyName)
    {
        var query = from j in db.Ajancies
                    join bl in db.BusinessLicenses on j.AjancyID equals bl.AjancyID
                    join jp in db.AjancyPartners on j.AjancyID equals jp.AjancyID
                    join ur in db.UsersInRoles on jp.UserRoleID equals ur.UserRoleID
                    join u in db.Users on ur.UserID equals u.UserID
                    join p in db.Persons on u.PersonID equals p.PersonID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where jp.LockOutDate == null &&
                               j.AjancyType == ajancyType && bl.LockOutDate == null
                    orderby ct.ProvinceID, ct.Name, j.AjancyName
                    select new
                    {
                        ct.ProvinceID,
                        ct.CityID,
                        j.AjancyID,
                        j.AjancyType,
                        j.AjancyName,
                        bl.SubmitDate,
                        bl.BusinessLicenseNo,
                        City = ct.Name,
                        Manager = string.Format("{0} {1}", p.FirstName, p.LastName)
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        switch (ajancyType)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (byte)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (byte)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (byte)Public.AjancyType.Academy
                        select q;
                break;
        }

        if (!string.IsNullOrEmpty(ajancyName))
        {
            query = from q in query
                    where q.AjancyName.Contains(ajancyName)
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region FCDiscardsRep.aspx

    public int GetFCDiscardsCount(byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, byte ajancyType, string nationalCode)
    {
        var query = from fcs in db.FuelCardSubstitutions
                    join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where fcs.PersonalTypeFuelCardID == null && dcc.LockOutDate == null
                    select new
                    {
                        owp.NationalCode,
                        j.AjancyType,
                        ct.CityID,
                        ct.ProvinceID,
                        fcs.SubmitDate
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode == nationalCode
                    select q;
        }

        switch (ajancyType)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadFCDiscards(int maximumRows, int startRowIndex, byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, byte ajancyType, string nationalCode)
    {
        var query = from fcs in db.FuelCardSubstitutions
                    join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                    from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where fcs.PersonalTypeFuelCardID == null && dcc.LockOutDate == null
                    orderby fcs.SubmitDate, owp.LastName
                    select new
                    {
                        fcs.FuelCardSubstituteID,
                        OwnerNationalCode = owp.NationalCode,
                        p.NationalCode,
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Owner = string.Format("{0} {1}", owp.FirstName, owp.LastName),
                        Driver = string.Format("{0} {1}", p.FirstName, p.LastName),
                        fc.PAN,
                        c.VIN,
                        ZCityID = (short?)zpn.CityID,
                        ZCity = zpn.City.Name,
                        ZNumber = zpn.Number,
                        pn.Alphabet,
                        pn.RegionIdentifier,
                        pn.ThreeDigits,
                        pn.TwoDigits,
                        j.AjancyType,
                        j.AjancyName,
                        fcs.SubmitDate,
                        Edit = FCD_Edit(ajancyType, p.NationalCode, ((short?)zpn.CityID).HasValue)
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.OwnerNationalCode == nationalCode
                    select q;
        }

        switch (ajancyType)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        //var result = query.Skip(startRowIndex).Take(maximumRows).ToList();

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    private string FCD_Edit(byte ajancyType, string nationalCode, bool isZoneType)
    {
        string link = null;
        switch (ajancyType)
        {
            case 0:
                link = string.Format("<a class='info' style='float: none;' href='../{0}/FCDiscard.aspx?nc={1}' target='_blank'>ویرایش</a>", isZoneType ? "Zone" : "Ajancy", TamperProofString.QueryStringEncode(nationalCode));
                break;

            case 2:
                link = string.Format("<a class='info' style='float: none;' href='../Academy/FCDiscard.aspx?nc={0}' target='_blank'>ویرایش</a>", TamperProofString.QueryStringEncode(nationalCode));
                break;
        }
        return link;
    }

    #endregion

    #region FCReplacementsRep.aspx

    public int GetFCReplacementsCount(byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, byte ajancyType, string type, string nationalCode, string pan, string vin)
    {
        int count = 0;
        switch (type)
        {
            case "1": // Ajancy type
                var query1 = from fcs in db.FuelCardSubstitutions
                             join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                             join c in db.Cars on fc.CarID equals c.CarID
                             join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                             join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                             join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                             join p in db.Persons on dc.PersonID equals p.PersonID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID

                             join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                             join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID

                             where fcs.PersonalTypeFuelCardID != null &&
                                     ((dc.PersonID == dc2.PersonID && // Self replacement
                                     (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                                      ||
                                     ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null)))
                             select new
                             {
                                 j.AjancyType,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 fcs.SubmitDate,
                                 NationalCode1 = owp.NationalCode,
                                 NationalCode2 = owp2.NationalCode,
                                 VIN1 = c.VIN,
                                 PAN1 = fc.PAN,
                                 VIN2 = c2.VIN,
                                 PAN2 = fc2.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query1 = from q in query1
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query1 = from q in query1
                             where q.CityID == cityId
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query1 = from q in query1
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query1 = from q in query1
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query1 = from q in query1
                             where q.NationalCode1 == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query1 = from q in query1
                             where q.PAN1.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query1 = from q in query1
                             where q.VIN1.Equals(vin)
                             select q;
                }

                switch (ajancyType)
                {
                    case 0:
                        query1 = from q in query1
                                 where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                                 select q;
                        break;

                    case 1:
                        query1 = from q in query1
                                 where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                                 select q;
                        break;

                    case 2:
                        query1 = from q in query1
                                 where q.AjancyType == (short)Public.AjancyType.Academy
                                 select q;
                        break;
                }
                count = query1.Count();
                break;

            case "2": // Personal type
                var query2 = from fcs in db.FuelCardSubstitutions
                             join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                             join c in db.Cars on fc.CarID equals c.CarID
                             join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                             join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                             join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                             join p in db.Persons on dc.PersonID equals p.PersonID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID

                             join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                             join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID

                             where fcs.PersonalTypeFuelCardID != null &&
                                     ((dc.PersonID == dc2.PersonID && // Self replacement
                                     (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                                      ||
                                     ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null)))
                             select new
                             {
                                 j.AjancyType,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 fcs.SubmitDate,
                                 NationalCode1 = owp.NationalCode,
                                 NationalCode2 = owp2.NationalCode,
                                 VIN1 = c.VIN,
                                 PAN1 = fc.PAN,
                                 VIN2 = c2.VIN,
                                 PAN2 = fc2.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query2 = from q in query2
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query2 = from q in query2
                             where q.CityID == cityId
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query2 = from q in query2
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query2 = from q in query2
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query2 = from q in query2
                             where q.NationalCode2 == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query2 = from q in query2
                             where q.PAN2.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query2 = from q in query2
                             where q.VIN2.Equals(vin)
                             select q;
                }

                switch (ajancyType)
                {
                    case 0:
                        query2 = from q in query2
                                 where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                                 select q;
                        break;

                    case 1:
                        query2 = from q in query2
                                 where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                                 select q;
                        break;

                    case 2:
                        query2 = from q in query2
                                 where q.AjancyType == (short)Public.AjancyType.Academy
                                 select q;
                        break;
                }
                count = query2.Count();
                break;

            default:
                var query0 = from fcs in db.FuelCardSubstitutions
                             join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                             join c in db.Cars on fc.CarID equals c.CarID
                             join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                             join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                             join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                             join p in db.Persons on dc.PersonID equals p.PersonID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID

                             join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                             join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID

                             where fcs.PersonalTypeFuelCardID != null &&
                                     ((dc.PersonID == dc2.PersonID && // Self replacement
                                     (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                                      ||
                                     ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null)))
                             select new
                             {
                                 j.AjancyType,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 fcs.SubmitDate,
                                 NationalCode1 = owp.NationalCode,
                                 NationalCode2 = owp2.NationalCode,
                                 VIN1 = c.VIN,
                                 PAN1 = fc.PAN,
                                 VIN2 = c2.VIN,
                                 PAN2 = fc2.PAN
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query0 = from q in query0
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query0 = from q in query0
                             where q.CityID == cityId
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query0 = from q in query0
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query0 = from q in query0
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }

                switch (ajancyType)
                {
                    case 0:
                        query0 = from q in query0
                                 where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                                 select q;
                        break;

                    case 1:
                        query0 = from q in query0
                                 where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                                 select q;
                        break;

                    case 2:
                        query0 = from q in query0
                                 where q.AjancyType == (short)Public.AjancyType.Academy
                                 select q;
                        break;
                }
                count = query0.Count();
                break;
        }

        db.Dispose();
        return count;
    }

    public IList<object> LoadFCReplacements(int maximumRows, int startRowIndex, byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, byte ajancyType, string type, string nationalCode, string pan, string vin)
    {
        switch (type)
        {
            case "1": // Ajancy type
                var query1 = from fcs in db.FuelCardSubstitutions
                             join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                             join c in db.Cars on fc.CarID equals c.CarID
                             join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                             join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                             join p in db.Persons on dc.PersonID equals p.PersonID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID

                             join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                             from pn2 in db.PlateNumbers.Where(number => number.PlateNumberID == cpn2.PlateNumberID).DefaultIfEmpty()
                             from zpn2 in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn2.ZonePlateNumberID).DefaultIfEmpty()
                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                             join p2 in db.Persons on dc2.PersonID equals p2.PersonID
                             join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                             join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID

                             where fcs.PersonalTypeFuelCardID != null &&
                                     ((dc.PersonID == dc2.PersonID && // Self replacement
                                     (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                                      ||
                                     ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null)))
                             orderby fcs.SubmitDate, owp.LastName
                             select new
                             {
                                 fcs.FuelCardSubstituteID,
                                 fcs.SubmitDate,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 City = ct.Name,
                                 j.AjancyType,
                                 Edit = FCR_Edit(ajancyType, p.NationalCode, p2.NationalCode, ((short?)zpn.CityID).HasValue, ((short?)zpn2.CityID).HasValue),

                                 NationalCode1 = p.NationalCode,
                                 Owner1 = string.Format("{0} {1}", owp.FirstName, owp.LastName),
                                 Driver1 = string.Format("{0} {1}", p.FirstName, p.LastName),
                                 PAN1 = fc.PAN,
                                 VIN1 = c.VIN,
                                 IsZoneType1 = cpn.ZonePlateNumberID.HasValue,
                                 ZCity1 = zpn.City.Name,
                                 ZNumber1 = zpn.Number,
                                 Alphabet1 = pn.Alphabet,
                                 RegionIdentifier1 = pn.RegionIdentifier,
                                 ThreeDigits1 = pn.ThreeDigits,
                                 TwoDigits1 = pn.TwoDigits,
                                 AjancyName1 = j.AjancyName,

                                 NationalCode2 = p2.NationalCode,
                                 Owner2 = string.Format("{0} {1}", owp2.FirstName, owp2.LastName),
                                 Driver2 = string.Format("{0} {1}", p2.FirstName, p2.LastName),
                                 PAN2 = fc2.PAN,
                                 VIN2 = c2.VIN,
                                 IsZoneType2 = cpn2.ZonePlateNumberID.HasValue,
                                 ZCity2 = zpn2.City.Name,
                                 ZNumber2 = zpn2.Number,
                                 Alphabet2 = pn2.Alphabet,
                                 RegionIdentifier2 = pn2.RegionIdentifier,
                                 ThreeDigits2 = pn2.ThreeDigits,
                                 TwoDigits2 = pn2.TwoDigits,
                                 AjancyName2 = j2.AjancyName,
                                 CarID2 = c2.CarID,
                                 PlateNumberID2 = pn2.PlateNumberID
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query1 = from q in query1
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query1 = from q in query1
                             where q.CityID == cityId
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query1 = from q in query1
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query1 = from q in query1
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query1 = from q in query1
                             where q.NationalCode1 == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query1 = from q in query1
                             where q.PAN1.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query1 = from q in query1
                             where q.VIN1.Equals(vin)
                             select q;
                }

                switch (ajancyType)
                {
                    case 0:
                        query1 = from q in query1
                                 where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                                 select q;
                        break;

                    case 1:
                        query1 = from q in query1
                                 where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                                 select q;
                        break;

                    case 2:
                        query1 = from q in query1
                                 where q.AjancyType == (short)Public.AjancyType.Academy
                                 select q;
                        break;
                }
                return query1.Skip(startRowIndex).Take(maximumRows).ToList<object>();

            case "2": // Personal type
                var query2 = from fcs in db.FuelCardSubstitutions
                             join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                             join c in db.Cars on fc.CarID equals c.CarID
                             join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                             join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                             join p in db.Persons on dc.PersonID equals p.PersonID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID

                             join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                             from pn2 in db.PlateNumbers.Where(number => number.PlateNumberID == cpn2.PlateNumberID).DefaultIfEmpty()
                             from zpn2 in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn2.ZonePlateNumberID).DefaultIfEmpty()
                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                             join p2 in db.Persons on dc2.PersonID equals p2.PersonID
                             join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                             join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID

                             where fcs.PersonalTypeFuelCardID != null &&
                                    ((dc.PersonID == dc2.PersonID && // Self replacement
                                     (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                                      ||
                                     ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null)))
                             orderby fcs.SubmitDate, owp.LastName
                             select new
                             {
                                 fcs.FuelCardSubstituteID,
                                 fcs.SubmitDate,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 City = ct.Name,
                                 j.AjancyType,
                                 Edit = FCR_Edit(ajancyType, p.NationalCode, p2.NationalCode, ((short?)zpn.CityID).HasValue, ((short?)zpn2.CityID).HasValue),

                                 NationalCode1 = p.NationalCode,
                                 Owner1 = string.Format("{0} {1}", owp.FirstName, owp.LastName),
                                 Driver1 = string.Format("{0} {1}", p.FirstName, p.LastName),
                                 PAN1 = fc.PAN,
                                 VIN1 = c.VIN,
                                 IsZoneType1 = cpn.ZonePlateNumberID.HasValue,
                                 ZCity1 = zpn.City.Name,
                                 ZNumber1 = zpn.Number,
                                 Alphabet1 = pn.Alphabet,
                                 RegionIdentifier1 = pn.RegionIdentifier,
                                 ThreeDigits1 = pn.ThreeDigits,
                                 TwoDigits1 = pn.TwoDigits,
                                 AjancyName1 = j.AjancyName,

                                 NationalCode2 = p2.NationalCode,
                                 Owner2 = string.Format("{0} {1}", owp2.FirstName, owp2.LastName),
                                 Driver2 = string.Format("{0} {1}", p2.FirstName, p2.LastName),
                                 PAN2 = fc2.PAN,
                                 VIN2 = c2.VIN,
                                 IsZoneType2 = cpn2.ZonePlateNumberID.HasValue,
                                 ZCity2 = zpn2.City.Name,
                                 ZNumber2 = zpn2.Number,
                                 Alphabet2 = pn2.Alphabet,
                                 RegionIdentifier2 = pn2.RegionIdentifier,
                                 ThreeDigits2 = pn2.ThreeDigits,
                                 TwoDigits2 = pn2.TwoDigits,
                                 AjancyName2 = j2.AjancyName,
                                 CarID2 = c2.CarID,
                                 PlateNumberID2 = pn2.PlateNumberID
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query2 = from q in query2
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query2 = from q in query2
                             where q.CityID == cityId
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query2 = from q in query2
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query2 = from q in query2
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query2 = from q in query2
                             where q.NationalCode2 == nationalCode
                             select q;
                }

                if (!string.IsNullOrEmpty(pan))
                {
                    query2 = from q in query2
                             where q.PAN2.Equals(pan)
                             select q;
                }

                if (!string.IsNullOrEmpty(vin))
                {
                    query2 = from q in query2
                             where q.VIN2.Equals(vin)
                             select q;
                }

                switch (ajancyType)
                {
                    case 0:
                        query2 = from q in query2
                                 where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                                 select q;
                        break;

                    case 1:
                        query2 = from q in query2
                                 where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                                 select q;
                        break;

                    case 2:
                        query2 = from q in query2
                                 where q.AjancyType == (short)Public.AjancyType.Academy
                                 select q;
                        break;
                }
                return query2.Skip(startRowIndex).Take(maximumRows).ToList<object>();

            default:
                var query0 = from fcs in db.FuelCardSubstitutions
                             join fc in db.FuelCards on fcs.AjancyTypeFuelCardID equals fc.FuelCardID
                             join c in db.Cars on fc.CarID equals c.CarID
                             join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                             from pn in db.PlateNumbers.Where(number => number.PlateNumberID == cpn.PlateNumberID).DefaultIfEmpty()
                             from zpn in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn.ZonePlateNumberID).DefaultIfEmpty()
                             join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                             join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                             join p in db.Persons on dc.PersonID equals p.PersonID
                             join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                             join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                             join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                             join ct in db.Cities on j.CityID equals ct.CityID

                             join fc2 in db.FuelCards on fcs.PersonalTypeFuelCardID equals fc2.FuelCardID
                             join c2 in db.Cars on fc2.CarID equals c2.CarID
                             join cpn2 in db.CarPlateNumbers on c2.CarID equals cpn2.CarID
                             from pn2 in db.PlateNumbers.Where(number => number.PlateNumberID == cpn2.PlateNumberID).DefaultIfEmpty()
                             from zpn2 in db.ZonePlateNumbers.Where(number => number.ZonePlateNumberID == cpn2.ZonePlateNumberID).DefaultIfEmpty()
                             join dcc2 in db.DriverCertificationCars on cpn2.CarPlateNumberID equals dcc2.CarPlateNumberID
                             join dc2 in db.DriverCertifications on dcc2.DriverCertificationID equals dc2.DriverCertificationID
                             join p2 in db.Persons on dc2.PersonID equals p2.PersonID
                             join owp2 in db.Persons on cpn2.OwnerPersonID equals owp2.PersonID
                             join jd2 in db.AjancyDrivers on dcc2.DriverCertificationCarID equals jd2.DriverCertificationCarID
                             join j2 in db.Ajancies on jd2.AjancyID equals j2.AjancyID

                             where fcs.PersonalTypeFuelCardID != null &&
                                     ((dc.PersonID == dc2.PersonID && // Self replacement
                                     (((fc.DiscardDate != null && dcc.LockOutDate != null && jd.LockOutDate != null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null))))
                                      ||
                                     ((fc.DiscardDate == null && dcc.LockOutDate == null && jd.LockOutDate == null) &&
                                      (fc2.DiscardDate == null && dcc2.LockOutDate == null && jd2.LockOutDate == null)))
                             orderby fcs.SubmitDate, owp.LastName
                             select new
                             {
                                 fcs.FuelCardSubstituteID,
                                 fcs.SubmitDate,
                                 ct.CityID,
                                 ct.ProvinceID,
                                 City = ct.Name,
                                 j.AjancyType,
                                 Edit = FCR_Edit(ajancyType, p.NationalCode, p2.NationalCode, ((short?)zpn.CityID).HasValue, ((short?)zpn2.CityID).HasValue),

                                 NationalCode1 = p.NationalCode,
                                 Owner1 = string.Format("{0} {1}", owp.FirstName, owp.LastName),
                                 Driver1 = string.Format("{0} {1}", p.FirstName, p.LastName),
                                 PAN1 = fc.PAN,
                                 VIN1 = c.VIN,
                                 IsZoneType1 = cpn.ZonePlateNumberID.HasValue,
                                 ZCity1 = zpn.City.Name,
                                 ZNumber1 = zpn.Number,
                                 Alphabet1 = pn.Alphabet,
                                 RegionIdentifier1 = pn.RegionIdentifier,
                                 ThreeDigits1 = pn.ThreeDigits,
                                 TwoDigits1 = pn.TwoDigits,
                                 AjancyName1 = j.AjancyName,

                                 NationalCode2 = p2.NationalCode,
                                 Owner2 = string.Format("{0} {1}", owp2.FirstName, owp2.LastName),
                                 Driver2 = string.Format("{0} {1}", p2.FirstName, p2.LastName),
                                 PAN2 = fc2.PAN,
                                 VIN2 = c2.VIN,
                                 IsZoneType2 = cpn2.ZonePlateNumberID.HasValue,
                                 ZCity2 = zpn2.City.Name,
                                 ZNumber2 = zpn2.Number,
                                 Alphabet2 = pn2.Alphabet,
                                 RegionIdentifier2 = pn2.RegionIdentifier,
                                 ThreeDigits2 = pn2.ThreeDigits,
                                 TwoDigits2 = pn2.TwoDigits,
                                 AjancyName2 = j2.AjancyName,
                                 CarID2 = c2.CarID,
                                 PlateNumberID2 = pn2.PlateNumberID
                             };

                if (provinceId > 0 && cityId == 0) // Just province
                {
                    query0 = from q in query0
                             where q.ProvinceID == provinceId
                             select q;
                }

                if (provinceId > 0 && cityId > 0) // province and city
                {
                    query0 = from q in query0
                             where q.CityID == cityId
                             select q;
                }

                if (dateFrom != null && dateTo == null)
                {
                    query0 = from q in query0
                             where q.SubmitDate == dateFrom
                             select q;
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query0 = from q in query0
                             where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                             select q;
                }

                switch (ajancyType)
                {
                    case 0:
                        query0 = from q in query0
                                 where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                                 select q;
                        break;

                    case 1:
                        query0 = from q in query0
                                 where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                                 select q;
                        break;

                    case 2:
                        query0 = from q in query0
                                 where q.AjancyType == (short)Public.AjancyType.Academy
                                 select q;
                        break;
                }
                return query0.Skip(startRowIndex).Take(maximumRows).ToList<object>();
        }
    }

    private string FCR_Edit(byte ajancyType, string nationalCode, string nationalCode2, bool isZoneType1, bool isZoneType2)
    {
        string link = null;
        switch (ajancyType)
        {
            case 0:
                if (nationalCode == nationalCode2)
                {
                    link = string.Format("<a class='info' style='float: none;' href='../{0}/SelfReplacement.aspx?nc={1}' target='_blank'>ویرایش</a>", (isZoneType1 && isZoneType2) ? "Zone" : "Union", TamperProofString.QueryStringEncode(nationalCode));
                }
                else
                {
                    link = string.Format("<a class='info' style='float: none;' href='../{0}/FCReplacement.aspx?nc1={1}&nc2={2}' target='_blank'>ویرایش</a>", (isZoneType1 && isZoneType2) ? "Zone" : "Union", TamperProofString.QueryStringEncode(nationalCode), TamperProofString.QueryStringEncode(nationalCode2));
                }
                break;

            case 2:
                if (nationalCode == nationalCode2)
                {
                    link = string.Format("<a class='info' style='float: none;' href='../Academy/SelfReplacement.aspx?nc={0}' target='_blank'>ویرایش</a>", TamperProofString.QueryStringEncode(nationalCode));
                }
                else
                {
                    link = string.Format("<a class='info' style='float: none;' href='../Academy/FCReplacement.aspx?nc1={0}&nc2={1}' target='_blank'>ویرایش</a>", TamperProofString.QueryStringEncode(nationalCode), TamperProofString.QueryStringEncode(nationalCode2));
                }
                break;
        }
        return link;
    }

    #endregion

    #region DiscardedFCsRep.aspx

    public int GetDiscardedFCsCount(byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, byte ajancyType)
    {
        var query = from bfc in db.BlockedFuelCards
                    join fc in db.FuelCards on bfc.FuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where dcc.LockOutDate == null
                    select new
                    {
                        j.AjancyType,
                        ct.CityID,
                        ct.ProvinceID,
                        p.SubmitDate
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        switch (ajancyType)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadDiscardedFCs(int maximumRows, int startRowIndex, byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, byte ajancyType)
    {
        var query = from bfc in db.BlockedFuelCards
                    join fc in db.FuelCards on bfc.FuelCardID equals fc.FuelCardID
                    join c in db.Cars on fc.CarID equals c.CarID
                    join cpn in db.CarPlateNumbers on c.CarID equals cpn.CarID
                    join pl in db.PlateNumbers on cpn.PlateNumberID equals pl.PlateNumberID
                    join dcc in db.DriverCertificationCars on cpn.CarPlateNumberID equals dcc.CarPlateNumberID
                    join dc in db.DriverCertifications on dcc.DriverCertificationID equals dc.DriverCertificationID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join owp in db.Persons on cpn.OwnerPersonID equals owp.PersonID
                    join jd in db.AjancyDrivers on dcc.DriverCertificationCarID equals jd.DriverCertificationCarID
                    join j in db.Ajancies on jd.AjancyID equals j.AjancyID
                    join ct in db.Cities on j.CityID equals ct.CityID
                    where dcc.LockOutDate == null
                    orderby p.LastName
                    select new
                    {
                        p.NationalCode,
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        Owner = string.Format("{0} {1}", owp.FirstName, owp.LastName),
                        Driver = string.Format("{0} {1}", p.FirstName, p.LastName),
                        fc.FuelCardID,
                        fc.PAN,
                        c.VIN,
                        pl.Alphabet,
                        pl.RegionIdentifier,
                        pl.ThreeDigits,
                        pl.TwoDigits,
                        j.AjancyType,
                        j.AjancyName,
                        p.SubmitDate,
                        Edit = string.Format("<a class='info' style='float: none;' href='../Union/DiscardedFCs.aspx?nc={0}' target='_blank'>ویرایش</a>", TamperProofString.QueryStringEncode(p.NationalCode))
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        switch (ajancyType)
        {
            case 0:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.TaxiAjancy
                        select q;
                break;

            case 1:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.MotorCycleAjancy
                        select q;
                break;

            case 2:
                query = from q in query
                        where q.AjancyType == (short)Public.AjancyType.Academy
                        select q;
                break;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion

    #region Insurance.aspx

    public int GetInsuranceCount(byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, string nationalCode, bool cancelInsurance)
    {
        var query = from ins in db.Insurances
                    join dc in db.DriverCertifications on ins.DriverCertificationID equals dc.DriverCertificationID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join fc in db.FuelCards on c.CarID equals fc.CarID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ct in db.Cities on u.CityID equals ct.CityID
                    where dcc.LockOutDate == null && ins.CancelInsurance == cancelInsurance
                    select new
                    {
                        p.NationalCode,
                        ct.CityID,
                        ct.ProvinceID,
                        ins.SubmitDate
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode == nationalCode
                    select q;
        }

        int count = query.Count();
        db.Dispose();
        return count;
    }

    public IList<object> LoadInsurances(int maximumRows, int startRowIndex, byte provinceId, short cityId, DateTime? dateFrom, DateTime? dateTo, string nationalCode, bool cancelInsurance)
    {
        var query = from ins in db.Insurances
                    join dc in db.DriverCertifications on ins.DriverCertificationID equals dc.DriverCertificationID
                    join dcc in db.DriverCertificationCars on dc.DriverCertificationID equals dcc.DriverCertificationID
                    join cpn in db.CarPlateNumbers on dcc.CarPlateNumberID equals cpn.CarPlateNumberID
                    join pl in db.PlateNumbers on cpn.PlateNumberID equals pl.PlateNumberID
                    join c in db.Cars on cpn.CarID equals c.CarID
                    join fc in db.FuelCards on c.CarID equals fc.CarID
                    join p in db.Persons on dc.PersonID equals p.PersonID
                    join u in db.Users on p.PersonID equals u.PersonID
                    join ct in db.Cities on u.CityID equals ct.CityID
                    where dcc.LockOutDate == null && ins.CancelInsurance == cancelInsurance
                    orderby ins.SubmitDate, p.LastName
                    select new
                    {
                        p.NationalCode,
                        p.FirstName,
                        p.LastName,
                        ct.CityID,
                        ct.ProvinceID,
                        City = ct.Name,
                        fc.FuelCardID,
                        fc.PAN,
                        c.VIN,
                        pl.Alphabet,
                        pl.RegionIdentifier,
                        pl.ThreeDigits,
                        pl.TwoDigits,
                        ins.SubmitDate,
                        ins.DriverCertificationID,
                        Edit = string.Format("<a class='info' style='float: none;' href='../Union/Insurance.aspx?nc={0}' target='_blank'>ویرایش</a>", TamperProofString.QueryStringEncode(p.NationalCode))
                    };

        if (provinceId > 0 && cityId == 0) // Just province
        {
            query = from q in query
                    where q.ProvinceID == provinceId
                    select q;
        }

        if (provinceId > 0 && cityId > 0) // province and city
        {
            query = from q in query
                    where q.CityID == cityId
                    select q;
        }

        if (dateFrom != null && dateTo == null)
        {
            query = from q in query
                    where q.SubmitDate == dateFrom
                    select q;
        }
        else if (dateFrom != null && dateTo != null)
        {
            query = from q in query
                    where q.SubmitDate >= dateFrom && q.SubmitDate <= dateTo
                    select q;
        }

        if (!string.IsNullOrEmpty(nationalCode))
        {
            query = from q in query
                    where q.NationalCode == nationalCode
                    select q;
        }

        return query.Skip(startRowIndex).Take(maximumRows).ToList<object>();
    }

    #endregion
}


