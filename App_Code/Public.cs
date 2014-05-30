using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Globalization;
using Stimulsoft.Report;

public static class Public
{
    public const string SAVEMESSAGE = "ثبت اطلاعات انجام گردید";
    public const string EDITMESSAGE = "ویرایش اطلاعات انجام گردید";
    public const string DELETEMESSAGE = "حذف اطلاعات انجام گردید";
    public const string REFERENCE_CONSTRAINT = "اطلاعات مورد نظر در جای دیگر سیستم در حال استفاده میباشد!";

    public static string ConnectionString
    {
        get
        {
            string connectionString = HttpContext.Current.Cache["ConnStr"] as string;
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = ConfigurationManager.ConnectionStrings["Kimia_AjancyConnection"].ConnectionString;
                HttpContext.Current.Cache.Insert("ConnStr", connectionString, null, DateTime.MaxValue, TimeSpan.FromMinutes(10));
            }
            return connectionString;
        }
    }

    public static string GetTraceKey()
    {
        long i = 1;
        foreach (byte b in Guid.NewGuid().ToByteArray())
        {
            i *= ((int)b + 1);
        }
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }

    public static string[] GetRoles(short roleId)
    {
        string[] roles = new string[1];
        switch (roleId)
        {
            case 1:
                roles[0] = "Admin";
                break;

            case 2:
                roles[0] = "AjancyManager";
                break;

            case 3:
                roles[0] = "AjancySupervisor";
                break;

            case 4:
                roles[0] = "AjancyPartner";
                break;

            case 5:
                roles[0] = "AjancySecretary";
                break;

            case 6:
                roles[0] = "TaxiDriver";
                break;

            case 7:
                roles[0] = "MotorCycleDriver";
                break;

            case 8:
                roles[0] = "CarOwner";
                break;

            case 9:
                roles[0] = "CityManager";
                break;

            case 10:
                roles[0] = "ProvinceManager";
                break;

            case 11:
                roles[0] = "AcademyManager";
                break;

            case 12:
                roles[0] = "AcademyPartner";
                break;

            case 13:
                roles[0] = "AcademyCity";
                break;

            case 14:
                roles[0] = "AcademyProvince";
                break;

            case 15:
                roles[0] = "AcademyTeacher";
                break;

            case 16:
                roles[0] = "Visitor";
                break;
        }
        return roles;
    }

    public static string GetRoleName(short roleId)
    {
        string roleName = null;
        switch (roleId)
        {
            case 1:
                roleName = "مدیر سایت";
                break;

            case 2:
                roleName = "مدیر آژانس";
                break;

            case 3:
                roleName = "مباشر آژانس";
                break;

            case 4:
                roleName = "شریک آژانس";
                break;

            case 5:
                roleName = "کارپرداز آژانس";
                break;

            case 6:
                roleName = "راننده تاکسی تلفنی";
                break;

            case 7:
                roleName = "راننده پیک موتوری";
                break;

            case 8:
                roleName = "مالک خودرو";
                break;

            case 9:
            case 13:
                roleName = "مدیر شهرستان";
                break;

            case 10:
            case 14:
                roleName = "مدیر استان";
                break;

            case 11:
                roleName = "مدیر آموزشگاه";
                break;

            case 12:
                roleName = "شریک آموزشگاه";
                break;

            case 15:
                roleName = "مربی آموزشگاه";
                break;

            case 16:
                roleName = "بازدید کننده";
                break;
        }
        return roleName;
    }

    public static string GetRoleName(Role role)
    {
        string roleName = null;
        switch (role)
        {
            case Role.Admin:
                roleName = "مدیر سایت";
                break;

            case Role.AjancyManager:
                roleName = "مدیر آژانس";
                break;

            case Role.AjancySupervisor:
                roleName = "مباشر آژانس";
                break;

            case Role.AjancyPartner:
                roleName = "شریک آژانس";
                break;

            case Role.AjancySecretary:
                roleName = "کارپرداز آژانس";
                break;

            case Role.TaxiDriver:
                roleName = "راننده تاکسی تلفنی";
                break;

            case Role.MotorCycleDriver:
                roleName = "راننده پیک موتوری";
                break;

            case Role.CarOwner:
                roleName = "مالک خودرو";
                break;

            case Role.CityManager:
                roleName = "مدیر آژانس های شهرستان";
                break;

            case Role.AcademyCity:
                roleName = "مدیر آموزشگاه های شهرستان";
                break;

            case Role.ProvinceManager:
                roleName = "مدیر آژانس های استان";
                break;

            case Role.AcademyProvince:
                roleName = "مدیر آموزشگاه های استان";
                break;

            case Role.AcademyManager:
                roleName = "مدیر آموزشگاه";
                break;

            case Role.AcademyPartner:
                roleName = "شریک آموزشگاه";
                break;

            case Role.AcademyTeacher:
                roleName = "مربی آموزشگاه";
                break;

            case Role.Visitor:
                roleName = "بازدید کننده";
                break;
        }
        return roleName;
    }

    public static Ajancy.Ajancy ActiveAjancy
    {
        get
        {
            Ajancy.Ajancy ajancy = HttpContext.Current.Session["Ajancy"] as Ajancy.Ajancy;
            if (ajancy == null)
            {
                HttpContext.Current.Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }
            return ajancy;
        }
    }

    public static Ajancy.UsersInRole ActiveUserRole
    {
        get
        {
            Ajancy.UsersInRole userRole = HttpContext.Current.Session["UserRole"] as Ajancy.UsersInRole;
            if (userRole == null)
            {
                HttpContext.Current.Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }
            return userRole;
        }
    }

    public static string PlateNumberRenderToHTML(Ajancy.PlateNumber plateNumber)
    {
        if (plateNumber != null)
        {
            return string.Format("<div style='height: 28px;width: 100%;'><div style='float: right;width: 23px;'>{3}</div><div style='float: right;width: 23px;'>ایران</div><div style='float: right;width: 30px;'>{2}</div><div style='float: right;width: 15px;'>{1}</div><div style='float: right;width: 23px;'>{0}</div></div>", plateNumber.TwoDigits, GetAlphabet(plateNumber.Alphabet), plateNumber.ThreeDigits, plateNumber.RegionIdentifier);
        }
        return null;
    }

    public static string PlateNumberRenderToHTML(string twoDigits, string alphabet, string threeDigits, string regionIdentifier)
    {
        return string.Format("<div style='height: 28px;width: 100%;'><div style='float: right;width: 23px;'>{3}</div><div style='float: right;width: 23px;'>ایران</div><div style='float: right;width: 30px;'>{2}</div><div style='float: right;width: 15px;'>{1}</div><div style='float: right;width: 23px;'>{0}</div></div>", twoDigits, GetAlphabet(alphabet), threeDigits, regionIdentifier);
    }

    public static string PlateNumberRenderForPrint(string twoDigits, string alphabet, string threeDigits, string regionIdentifier)
    {
        return string.Format(" {1} {0} {2} {3} {4}", "ایران", regionIdentifier, threeDigits, GetAlphabet(alphabet), twoDigits);
        //return string.Format("{4} {1} {2} {3} {0}", twoDigits, GetAlphabet(alphabet), threeDigits, "ایران", regionIdentifier);
    }

    public static string GetAlphabet(string alphabet)
    {
        switch (alphabet)
        {
            case "A": return "الف";
            case "B": return "ب";
            case "C": return "پ";
            case "D": return "ت";
            case "E": return "ث";
            case "F": return "ج";
            case "G": return "چ";
            case "H": return "ح";
            case "I": return "خ";
            case "J": return "د";
            case "K": return "ذ";
            case "L": return "ر";
            case "M": return "ز";
            case "N": return "ژ";
            case "O": return "س";
            case "P": return "ش";
            case "Q": return "ص";
            case "R": return "ض";
            case "S": return "ط";
            case "T": return "ظ";
            case "U": return "ع";
            case "V": return "غ";
            case "W": return "ف";
            case "X": return "ق";
            case "Y": return "ک";
            case "Z": return "گ";
            case "1": return "ل";
            case "2": return "م";
            case "3": return "ن";
            case "4": return "و";
            case "5": return "ه";
            case "6": return "ی";
            default: return string.Empty;
        }
    }

    public static string GetEducation(byte education)
    {
        switch (education)
        {
            case 0: return "بیسواد";
            case 1: return "نهضت";
            case 2: return "ابتدایی";
            case 3: return "سیکل";
            case 4: return "دیپلم";
            case 5: return "کاردانی";
            case 6: return "لیسانس";
            case 7: return "فوق لیسانس";
            case 8: return "دکترا";
            case 9: return "فوق دکترا";
            default: return string.Empty;
        }
    }

    public static string GetReligion(byte religion)
    {
        switch (religion)
        {
            case 0: return "اسلام";
            case 1: return "مسيحي";
            case 2: return "يهودي";
            case 3: return "زرتشتي";
            case 4: return "ديگر";
            default: return string.Empty;
        }
    }

    public static string GetJobStatus(byte jobStatus)
    {
        switch (jobStatus)
        {
            case 0: return "بیکار";
            case 1: return "بیکار مشمول بیمه بیکاری";
            case 2: return "شاغل";
            case 3: return "بازنشسته";
            default: return string.Empty;
        }
    }

    public static string GetMilitaryService(byte militaryService)
    {
        switch (militaryService)
        {
            case 0: return "دارای كارت پايان خدمت";
            case 1: return "دارای معافيت دایم";
            case 2: return "مشمول نمیباشد";
            default: return string.Empty;
        }
    }

    public static string GetDrivingLicenseType(byte drivingLicenseType)
    {
        switch (drivingLicenseType)
        {
            case 0: return "ب 1";
            case 1: return "ب 2";
            case 2: return "پایه یکم";
            case 3: return "پایه دوم";
            default: return string.Empty;
        }
    }

    public static string GetFuelTypeName(FuelType fuelType)
    {
        string result = null;
        switch (fuelType)
        {
            case FuelType.Petrol:
                result = "بنزین";
                break;

            case FuelType.Petrol_AND_CNG:
                result = "بنزین و CNG";
                break;

            case FuelType.Petrol_AND_LPG:
                result = "بنزین و LPG";
                break;
        }
        return result;
    }

    public static string GetFuelCardTypeName(FuelCardType fuelCardType)
    {
        string result = null;
        switch (fuelCardType)
        {
            case FuelCardType.Ajancy:
                result = "آژانس";
                break;

            case FuelCardType.Personal:
                result = "شخصی";
                break;

            case FuelCardType.OutCityTaxi:
                result = "خطی و بین راهی";
                break;
        }
        return result;
    }

    public static void ExportInfo(byte exportType, StiReport report)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            string fileName = "rep";
            switch (exportType)
            {
                case 0:// "pdf"
                    report.ExportDocument(StiExportFormat.Pdf, HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Report/Tmp/{0}.Pdf", fileName)));
                    report.ExportDocument(StiExportFormat.Pdf, ms);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.pdf", fileName));
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    break;
                case 1:// "jpeg"
                    report.ExportDocument(StiExportFormat.ImageJpeg, HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Report/Tmp/{0}.jpeg", fileName)));
                    report.ExportDocument(StiExportFormat.ImageJpeg, ms);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.jpeg", fileName));
                    HttpContext.Current.Response.ContentType = "application/ImageJpeg";
                    break;
                case 2: // "html":
                    report.ExportDocument(StiExportFormat.Html, HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Report/Tmp/{0}.html", fileName)));
                    report.ExportDocument(StiExportFormat.Html, ms);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.html", fileName));
                    HttpContext.Current.Response.ContentType = "application/html";
                    break;
                case 3: // "xls":
                    report.ExportDocument(StiExportFormat.Excel, HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Report/Tmp/{0}.xls", fileName)));
                    report.ExportDocument(StiExportFormat.Excel, ms);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
                    HttpContext.Current.Response.ContentType = "application/xls";
                    break;
            }
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
        }
    }


    #region Enums

    public enum Gender : byte { Female, Male };
    public enum Marriage : byte { Single, Married };
    public enum AjancyType : byte { TaxiAjancy, MotorCycleAjancy, Academy };
    public enum FuelCardType : byte { Ajancy, Personal, OutCityTaxi };
    public enum FuelType : byte { Petrol, Petrol_AND_CNG, Petrol_AND_LPG, CNG };
    public enum DrivingLicenseType : byte { B1, B2, Level_1, Level_2 };
    public enum Role : byte
    {
        Admin = 1, AjancyManager = 2, AjancySupervisor = 3, AjancyPartner = 4, AjancySecretary = 5,
        TaxiDriver = 6, MotorCycleDriver = 7, CarOwner = 8, CityManager = 9, ProvinceManager = 10,
        AcademyManager = 11, AcademyPartner = 12, AcademyCity = 13, AcademyProvince = 14,
        AcademyTeacher = 15, Visitor = 16
    };

    #endregion

    #region Convertion Methods

    public static short ToShort(object input)
    {
        if (input == null)
        {
            return 0;
        }
        short result = 0;
        short.TryParse(input.ToString(), out result);
        return result;
    }

    public static int ToInt(object input)
    {
        if (input == null)
        {
            return 0;
        }
        int result = 0;
        int.TryParse(input.ToString(), out result);
        return result;
    }

    public static long ToLong(object input)
    {
        if (input == null)
        {
            return 0;
        }
        long result = 0;
        long.TryParse(input.ToString(), out result);
        return result;
    }

    public static decimal ToDecimal(object input)
    {
        if (input == null)
        {
            return 0;
        }
        decimal result = 0;
        decimal.TryParse(input.ToString(), out result);
        return result;
    }

    public static byte ToByte(object input)
    {
        if (input == null)
        {
            return 0;
        }
        byte result = 0;
        byte.TryParse(input.ToString(), out result);
        return result;
    }

    public static bool ToBool(object input)
    {
        bool result = false;
        bool.TryParse(input.ToString(), out result);
        return result;
    }

    public static string ToHex(object input)
    {
        return string.Format("{0:X}", input);
    }

    public static double ToDouble(object input)
    {
        if (input == null)
        {
            return 0;
        }
        double result = 0;
        double.TryParse(input.ToString(), out result);
        return result;
    }

    public static string GetTime(TimeSpan time)
    {
        if (time != null)
        {
            return string.Format("{0} : {1}", time.Minutes, time.Hours);
        }
        else
        {
            return null;
        }
    }

    public static string ToPersianDate(object date)
    {
        string result = null;
        if (date != null)
        {
            DateTime dt = (DateTime)date;
            PersianCalendar objPersianCalendar = new PersianCalendar();
            int year = objPersianCalendar.GetYear(dt);
            int month = objPersianCalendar.GetMonth(dt);
            int day = objPersianCalendar.GetDayOfMonth(dt);
            int hour = objPersianCalendar.GetHour(dt);
            int min = objPersianCalendar.GetMinute(dt);
            int sec = objPersianCalendar.GetSecond(dt);
            result = string.Concat(year.ToString().PadLeft(4, '0'), DateTimeFormatInfo.CurrentInfo.DateSeparator, month.ToString().PadLeft(2, '0'), DateTimeFormatInfo.CurrentInfo.DateSeparator, day.ToString().PadLeft(2, '0'));
        }
        return result;
    }

    public static string ToPersianDateTime(object date)
    {
        string result = null;
        if (date != null)
        {
            DateTime dt = (DateTime)date;
            PersianCalendar objPersianCalendar = new PersianCalendar();
            int year = objPersianCalendar.GetYear(dt);
            int month = objPersianCalendar.GetMonth(dt);
            int day = objPersianCalendar.GetDayOfMonth(dt);
            int hour = objPersianCalendar.GetHour(dt);
            int min = objPersianCalendar.GetMinute(dt);
            int sec = objPersianCalendar.GetSecond(dt);
            result = string.Concat(year.ToString().PadLeft(4, '0'), DateTimeFormatInfo.CurrentInfo.DateSeparator, month.ToString().PadLeft(2, '0'), DateTimeFormatInfo.CurrentInfo.DateSeparator, day.ToString().PadLeft(2, '0'), " ", hour.ToString().PadLeft(2, '0'), DateTimeFormatInfo.CurrentInfo.TimeSeparator, min.ToString().PadLeft(2, '0'));
        }
        return result;
    }

    public static string ToDateForSQLQuery(DateTime? date)
    {
        if (date != null)
        {
            return string.Format("{0}-{1}-{2}", date.Value.Year, date.Value.Month, date.Value.Day);
        }
        return null;
    }

    public static string MoneyToString(object input)
    {
        string result = "0";
        try
        {
            result = Convert.ToInt64(input).ToString();
        }
        catch
        {
        }
        return result;
    }

    #endregion
}

public static class TamperProofString
{
    private static string TamperProofStringEncode(string value, string key)
    {
        System.Security.Cryptography.MACTripleDES mac3des = new System.Security.Cryptography.MACTripleDES();
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        mac3des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
        return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value)) + System.Convert.ToChar("-") + System.Convert.ToBase64String(mac3des.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value)));
    }

    private static string TamperProofStringDecode(string value, string key)
    {
        String dataValue = "";
        String calcHash = "";
        String storedHash = "";

        System.Security.Cryptography.MACTripleDES mac3des = new System.Security.Cryptography.MACTripleDES();
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        mac3des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));

        try
        {
            dataValue = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(value.Split(System.Convert.ToChar("-"))[0]));
            storedHash = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(value.Split(System.Convert.ToChar("-"))[1]));
            calcHash = System.Text.Encoding.UTF8.GetString(mac3des.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataValue)));

            if (storedHash != calcHash)
            {
                throw new ArgumentException("Hash value does not match");
            }
        }
        catch (System.Exception)
        {
            throw new ArgumentException("Invalid TamperProofString");
        }
        return dataValue;
    }

    public static string QueryStringEncode(string value)
    {
        return System.Web.HttpUtility.UrlEncode(TamperProofStringEncode(value, System.Configuration.ConfigurationManager.AppSettings["TamperProofKey"]));
    }

    public static string QueryStringDecode(string value)
    {
        return TamperProofStringDecode(value, System.Configuration.ConfigurationManager.AppSettings["TamperProofKey"]);
    }
}

