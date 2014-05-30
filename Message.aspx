<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <p class="alarm">
    </p>
    <script type="text/javascript">
        $(document).ready(function () {
            switch (getQueryString(window.location.search, 'mode')) {
                case '1': // BusinessLicense Request
                    if (getQueryString(location.search, 'c') != null) {
                        //string code = string.Format('<b style='direction:ltr; font-size: 15px; text-decoration: underline;'>{0}</b>', Request.QueryString['c']);
                        //$('.alarm').text( string.Format('کاربر گرامی درخواست صدور پروانه کسب آژانس شما ثبت گردید و کد رهگیری درخواست شما   {0} میباشد, لطفا آنرا یادداشت کنید و برای پیگیری درخواستتان با این کد بعدا به سایت مراجعه کنید', code);
                    }
                    break;

                case '2': // DriverCertification Request
                    $('.alarm').text('کاربر گرامی درخواست صدور کارت صلاحیت راننده آژانس شما ثبت گردید');
                    break;

                case '3': // Duplicate BusinessLicense Request for one place
                    $('.alarm').text('کاربر گرامی درخواست قبلا صدور پروانه برای این واحد ملکی ثبت شده و موقعیت و شرایط ملک تجاری مورد نظر مورد تایید قرار نگرفته و درخواست شما نیز مردود میباشد.');
                    break;

                case '4': // Submit BusinessLicense
                    $('.alarm').text('کاربر گرامی اطلاعات پروانه کسب آژانس شما ثبت گردید ');
                    break;

                case '5': // Submit BusinessLicense and Union/FCDiscard.aspx
                    $('.alarm').text('شخص مورد نظر قبلا بعنوان راننده درون سیستم ثبت گردیده برای ویرایش اطلاعات به - منوی مدیریت آژانس - لیست رانندگان آژانس - بروید');
                    break;

                case '6': // Duplicate BusinessLicense Postalcode
                    $('.alarm').text('کاربر گرامی قبلا پروانه برای این واحد ملکی ثبت شده.');
                    break;

                case '7': // DriverCertification Edit
                    $('.alarm').text('کاربر گرامی ویرایش اطلاعات راننده آژانس شما انجام گردید');
                    break;

                case '8': // Driver.aspx
                    $('.alarm').text('کاربر گرامی لطفا ابتدا اطلاعات پروانه کسب آژانس را ثبت کنید');
                    break;

                case '9': // Ajancy/BusinessLicense.aspx
                    $('.alarm').text('کاربر گرامی اطلاعات پروانه کسب آژانس شما قبلا ثبت گردیده است');
                    break;

                case '10': // Union/FCDiscard.aspx
                    $('.alarm').text('کاربر گرامی درخواست ابطال کارت سوخت شما ثبت گردید');
                    break;

                case '11': // Union/FCDiscard.aspx
                    $('.alarm').text('کاربر گرامی درخواست جایگزین کارت سوخت شما ثبت گردید');
                    break;

                case '12': // DriverCertification Request
                    $('.alarm').text('کاربر گرامی اطلاعات راننده آژانس شما ثبت گردید');
                    break;

                case '13': // Union/FCDiscard.aspx
                    $('.alarm').text('شخص مورد نظر قبلا بعنوان راننده در شهرستان دیگری درون سیستم ثبت گردیده، برای ویرایش شهرستان با مدیر سیستم تماس بگیرید');
                    break;

                case '14': // Union/RegisterDriverCertification.aspx
                    $('.alarm').text('ثبت نهایی صلاحیت راننده آژانس انجام گردیده');
                    break;

                case '15': // Union/FCDiscard.aspx, Uinon/Driver.aspx, Zone/Driver.aspx
                    $('.alarm').text('شخص مورد نظر قبلا بعنوان راننده در استان دیگری درون سیستم ثبت گردیده، برای ویرایش استان با مدیر سیستم تماس بگیرید');
                    break;

                case '16': // Union/FCReplacement.aspx
                    $('.alarm').text('شخص مورد نظر قبلا بعنوان راننده درون سیستم ثبت گردیده لطفا با مدیرسیستم تماس بگیرید');
                    break;

                case '17': // Management/Person.aspx
                    $('.alarm').text('کاربر گرامی اطلاعات شخص مورد نظر شما ویرایش گردید');
                    break;

                case '18': // Submit BusinessLicense
                    $('.alarm').text('کاربر گرامی اطلاعات آموزشگاه رانندگی شما ثبت گردید ');
                    break;

                case '19': // Union/DiscardedFCs.aspx
                    $('.alarm').text('کاربر گرامی ثبت کارت سوخت مسدود شده شما انجام گردید ');
                    break;

                case '20': // Union/Insurance.aspx
                    $('.alarm').text('کاربر گرامی ثبت درخواست بیمه شما انجام گردید ');
                    break;

                case '21': // Union/Insurance.aspx
                    $('.alarm').text('کاربر گرامی درخواست لغو بیمه شما ثبت گردید ');
                    break;

                case '22': // Union/FCDiscard.aspx,FCReplacement.aspx
                    $('.alarm').text('کاربر گرامی برای خودرو مورد نظر شما کارت سوخت فعالی درون سیستم یافت نشد.نخست اطلاعات کارت سوخت فعال فعلی خودرو را در سیستم ثبت نمایید ');
                    //$('<br/><a href="Union/AddFuelCard.aspx"><span style="font-weight: bold;color: #e84b41;text-decoration: underline;">ثبت کارت سوخت فعال</span></a>').appendTo('.alarm');
                    break;

                case '23': // Management/UserRoles.aspx
                    $('.alarm').text('شخص مورد نظر قبلا بعنوان کاربر در شهرستان دیگری درون سیستم ثبت گردیده');
                    break;

                case '24': // Management/UserRoles.aspx
                    $('.alarm').text('شخص مورد نظر قبلا بعنوان کاربر در استان دیگری درون سیستم ثبت گردیده');
                    break;

                case '25': // Union/TransferCarOwnerShip.aspx
                    $('.alarm').text('انتقال مالکیت (VIN) خوردو انجام گردید');
                    break;

                case '26': // Union/TransferCarOwnerShip.aspx
                    $('.alarm').text('VIN مورد نظر قبلا در شهرستان دیگری درون سیستم ثبت گردیده');
                    break;

                case '27': // Union/TransferCarOwnerShip.aspx
                    $('.alarm').text('VIN مورد نظر قبلا در استان دیگری درون سیستم ثبت گردیده');
                    break;

                case '28': // Union/FCReplacement.aspx or SelfReplacement.aspx
                    $('.alarm').text('برای راننده موردنظر خودروی فعالی درون سیستم یافت نگردید، برای ثبت خودروی فعال به بخش ثبت خودرو جدید بروید.');
                    $('<a href="Union/AddCar.aspx" class="link">ثبت خودرو جدید</a>').appendTo('.alarm');
                    break;
                    
                case '29': // Union/FCReplacement.aspx or SelfReplacement.aspx or FCdiscard.aspx
                    $('.alarm').text('آخرین پلاک ثبت شده درون سیستم برای راننده موردنظر از نوع منطقه آزاد میباشد،لطفا ابتدا به لینک زیر رفته و مشخصات پلاک و خودروی جدید را وارد نموده سپس اقدام به ثبت درخواست ابطال و جایگزین کنید.');
                    $('<a href="Union/AddCar.aspx" class="link">ثبت خودرو جدید</a>').appendTo('.alarm');
                    break;
                    
                case '30': // Zone/FCReplacement.aspx or SelfReplacement.aspx or FCdiscard.aspx
                    $('.alarm').text('آخرین پلاک ثبت شده درون سیستم برای راننده موردنظر از نوع ایران میباشد،لطفا ابتدا به لینک زیر رفته و مشخصات پلاک و خودروی جدید را وارد نموده سپس اقدام به ثبت درخواست ابطال و جایگزین کنید.');
                    $('<a href="Zone/AddCar.aspx" class="link">ثبت خودرو جدید</a>').appendTo('.alarm');
                    break;
            }
        });                
    </script>
</asp:Content>
