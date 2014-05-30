<%@ Page Title="" Language="C#" MasterPageFile="~/SiteWide.master" AutoEventWireup="true"
    CodeFile="DriverInfo.aspx.cs" Inherits="Ajancy_DriverInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <br />
    <center>
        <div class="title-bar">
            فرم ثبت نام کارت صلاحیت رانندگان آژانس
        </div>
    </center>
    <br />
    <table id="request" class="public">
        <tr>
            <td class="fieldName-large">
                کد ملی :
            </td>
            <td>
                <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نام :
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نام خانوادگی :
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نام پدر :
            </td>
            <td>
                <asp:TextBox ID="txtFather" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره شناسنامه :
            </td>
            <td>
                <asp:TextBox ID="txtBirthCertificateNo" runat="server" SkinID="TextBoxMedium" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                سريال شناسنامه :
            </td>
            <td>
                <asp:TextBox ID="txtBirthCertificateSerial" runat="server" SkinID="TextBoxMedium"
                    MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                سري شناسنامه :
            </td>
            <td>
                <asp:TextBox CssClass="textbox" ID="txtBirthCertificateSerie" runat="server" Width="40px"
                    MaxLength="3"></asp:TextBox><span class="star">*</span> /
                <asp:DropDownList ID="drpBirthCertificateAlfa" runat="server" CssClass="dropdown"
                    Width="50px">
                    <asp:ListItem Value="A" Text="الف"></asp:ListItem>
                    <asp:ListItem Value="B" Text="ب"></asp:ListItem>
                    <asp:ListItem Value="C" Text="پ"></asp:ListItem>
                    <asp:ListItem Value="D" Text="ت"></asp:ListItem>
                    <asp:ListItem Value="E" Text="ث"></asp:ListItem>
                    <asp:ListItem Value="F" Text="ج"></asp:ListItem>
                    <asp:ListItem Value="G" Text="چ"></asp:ListItem>
                    <asp:ListItem Value="H" Text="ح"></asp:ListItem>
                    <asp:ListItem Value="I" Text="خ"></asp:ListItem>
                    <asp:ListItem Value="J" Text="د"></asp:ListItem>
                    <asp:ListItem Value="K" Text="ذ"></asp:ListItem>
                    <asp:ListItem Value="L" Text="ر"></asp:ListItem>
                    <asp:ListItem Value="M" Text="ز"></asp:ListItem>
                    <asp:ListItem Value="N" Text="ژ"></asp:ListItem>
                    <asp:ListItem Value="O" Text="س"></asp:ListItem>
                    <asp:ListItem Value="P" Text="ش"></asp:ListItem>
                    <asp:ListItem Value="Q" Text="ص"></asp:ListItem>
                    <asp:ListItem Value="R" Text="ض"></asp:ListItem>
                    <asp:ListItem Value="S" Text="ط"></asp:ListItem>
                    <asp:ListItem Value="T" Text="ظ"></asp:ListItem>
                    <asp:ListItem Value="U" Text="ع"></asp:ListItem>
                    <asp:ListItem Value="V" Text="غ"></asp:ListItem>
                    <asp:ListItem Value="W" Text="ف"></asp:ListItem>
                    <asp:ListItem Value="X" Text="ق"></asp:ListItem>
                    <asp:ListItem Value="Y" Text="ک"></asp:ListItem>
                    <asp:ListItem Value="Z" Text="گ"></asp:ListItem>
                    <asp:ListItem Value="1" Text="ل"></asp:ListItem>
                    <asp:ListItem Value="2" Text="م"></asp:ListItem>
                    <asp:ListItem Value="3" Text="ن"></asp:ListItem>
                    <asp:ListItem Value="4" Text="و"></asp:ListItem>
                    <asp:ListItem Value="5" Text="ه"></asp:ListItem>
                    <asp:ListItem Value="6" Text="ی"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                تاریخ تولد :
            </td>
            <td>
                <userControl:Date ID="txtBirthDate" runat="server" Required="true" />
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                محل تولد :
            </td>
            <td>
                <asp:TextBox ID="txtBirthPlace" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                محل صدور شناسنامه :
            </td>
            <td>
                <asp:TextBox ID="txtBirthCertificatePlace" runat="server" SkinID="TextBoxMedium"
                    MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                جنسیت :
            </td>
            <td>
                <asp:DropDownList ID="drpGender" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="مرد"></asp:ListItem>
                    <asp:ListItem Value="1" Text="زن"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                وضعيت تاهل
            </td>
            <td>
                <asp:DropDownList ID="drpMarriage" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="S" Text="مجرد"></asp:ListItem>
                    <asp:ListItem Value="M" Text="متاهل"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                تعداد افراد تحت تکفل :
            </td>
            <td>
                <asp:TextBox ID="txtFamilyMembersCount" runat="server" SkinID="TextBoxMedium" MaxLength="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                تحصیلات :
            </td>
            <td>
                <asp:DropDownList ID="drpEducation" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="بیسواد"></asp:ListItem>
                    <asp:ListItem Value="1" Text="نهضت"></asp:ListItem>
                    <asp:ListItem Value="2" Text="ابتدایی"></asp:ListItem>
                    <asp:ListItem Value="3" Text="سیکل"></asp:ListItem>
                    <asp:ListItem Value="4" Text="دیپلم" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="5" Text="کاردانی"></asp:ListItem>
                    <asp:ListItem Value="6" Text="لیسانس"></asp:ListItem>
                    <asp:ListItem Value="7" Text="فوق لیسانس"></asp:ListItem>
                    <asp:ListItem Value="8" Text="دکترا"></asp:ListItem>
                    <asp:ListItem Value="9" Text="فوق دکترا"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                وضعیت نظام وظيفه :
            </td>
            <td>
                <asp:DropDownList CssClass="dropdown-middle" ID="drpMilitaryService" runat="server">
                    <asp:ListItem Value="0" Text="دارای كارت پايان خدمت"></asp:ListItem>
                    <asp:ListItem Value="1" Text="دارای معافيت دایم"></asp:ListItem>
                    <asp:ListItem Value="2" Text="مشمول نمیباشد"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                دين :
            </td>
            <td>
                <asp:DropDownList ID="drpReligion" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="اسلام"></asp:ListItem>
                    <asp:ListItem Value="1" Text="مسيحي"></asp:ListItem>
                    <asp:ListItem Value="2" Text="يهودي"></asp:ListItem>
                    <asp:ListItem Value="3" Text="زرتشتي"></asp:ListItem>
                    <asp:ListItem Value="4" Text="ديگر"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                مذهب :
            </td>
            <td>
                <asp:TextBox ID="txtSubreligion" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                وضعیت شغلی :
            </td>
            <td>
                <asp:DropDownList ID="drpJobStatus" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="بیکار"></asp:ListItem>
                    <asp:ListItem Value="1" Text="بیکار مشمول بیمه بیکاری"></asp:ListItem>
                    <asp:ListItem Value="2" Text="شاغل"></asp:ListItem>
                    <asp:ListItem Value="3" Text="بازنشسته"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                تلفن ثابت :
            </td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server" MaxLength="14" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                تلفن همراه :
            </td>
            <td>
                <asp:TextBox ID="txtMobile" runat="server" MaxLength="11" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                کد پستی :
            </td>
            <td>
                <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="10" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                آدرس :
            </td>
            <td>
                <asp:TextBox ID="txtAddress" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                    MaxLength="150" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="section-title">
                مشخصات دفترچه صلاحیت راننده<br />
                <hr />
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره دفترچه صلاحیت :
            </td>
            <td>
                <asp:TextBox ID="txtDriverCertificationNo" runat="server" SkinID="TextBoxMedium"
                    MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="section-title">
                مشخصات گواهینامه<br />
                <hr />
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                تاریخ صدور :
            </td>
            <td>
                <userControl:Date ID="txtDrivingLicenseDate" runat="server" Required="true" />
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره گواهینامه :
            </td>
            <td>
                <asp:TextBox ID="txtDrivingLicenseNo" runat="server" SkinID="TextBoxMedium" MaxLength="15"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                محل صدور :
            </td>
            <td>
                <asp:TextBox ID="txtDrivingLicensePlace" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نوع گواهینامه :
            </td>
            <td>
                <asp:DropDownList ID="drpDrivingLicenseType" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="ب 1"></asp:ListItem>
                    <asp:ListItem Value="1" Text="ب 2"></asp:ListItem>
                    <asp:ListItem Value="2" Text="پایه یکم"></asp:ListItem>
                    <asp:ListItem Value="3" Text="پایه دوم"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="section-title">
                اطلاعات خودرو<br />
                <hr />
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نوع خودرو :
            </td>
            <td>
                <asp:DropDownList ID="drpCarType" runat="server" CssClass="dropdown-middle">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                مدل خودرو :
            </td>
            <td>
                <asp:TextBox ID="txtCarModel" runat="server" MaxLength="4" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                رنگ خودرو :
            </td>
            <td>
                <asp:TextBox ID="txtCarColor" runat="server" MaxLength="10" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره پلاک خودرو :
            </td>
            <td>
                <asp:TextBox ID="txtCarPlateNumber_3" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                    Width="30px"></asp:TextBox><span style="color: #019901; font-size: 14px;">ایران</span>
                <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" Width="50px" SkinID="TextBoxMedium"></asp:TextBox>
                <asp:DropDownList ID="drpCarPlateNumber" runat="server" CssClass="dropdown" Width="50px">
                    <asp:ListItem Value="A" Text="الف"></asp:ListItem>
                    <asp:ListItem Value="B" Text="ب"></asp:ListItem>
                    <asp:ListItem Value="C" Text="پ"></asp:ListItem>
                    <asp:ListItem Value="D" Text="ت"></asp:ListItem>
                    <asp:ListItem Value="E" Text="ث"></asp:ListItem>
                    <asp:ListItem Value="F" Text="ج"></asp:ListItem>
                    <asp:ListItem Value="G" Text="چ"></asp:ListItem>
                    <asp:ListItem Value="H" Text="ح"></asp:ListItem>
                    <asp:ListItem Value="I" Text="خ"></asp:ListItem>
                    <asp:ListItem Value="J" Text="د"></asp:ListItem>
                    <asp:ListItem Value="K" Text="ذ"></asp:ListItem>
                    <asp:ListItem Value="L" Text="ر"></asp:ListItem>
                    <asp:ListItem Value="M" Text="ز"></asp:ListItem>
                    <asp:ListItem Value="N" Text="ژ"></asp:ListItem>
                    <asp:ListItem Value="O" Text="س"></asp:ListItem>
                    <asp:ListItem Value="P" Text="ش"></asp:ListItem>
                    <asp:ListItem Value="Q" Text="ص"></asp:ListItem>
                    <asp:ListItem Value="R" Text="ض"></asp:ListItem>
                    <asp:ListItem Value="S" Text="ط"></asp:ListItem>
                    <asp:ListItem Value="T" Text="ظ"></asp:ListItem>
                    <asp:ListItem Value="U" Text="ع"></asp:ListItem>
                    <asp:ListItem Value="V" Text="غ"></asp:ListItem>
                    <asp:ListItem Value="W" Text="ف"></asp:ListItem>
                    <asp:ListItem Value="X" Text="ق"></asp:ListItem>
                    <asp:ListItem Value="Y" Text="ک"></asp:ListItem>
                    <asp:ListItem Value="Z" Text="گ"></asp:ListItem>
                    <asp:ListItem Value="1" Text="ل"></asp:ListItem>
                    <asp:ListItem Value="2" Text="م"></asp:ListItem>
                    <asp:ListItem Value="3" Text="ن"></asp:ListItem>
                    <asp:ListItem Value="4" Text="و"></asp:ListItem>
                    <asp:ListItem Value="5" Text="ه"></asp:ListItem>
                    <asp:ListItem Value="6" Text="ی"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtCarPlateNumber_1" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                    Width="30px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره موتور :
            </td>
            <td>
                <asp:TextBox ID="txtCarEngineNo" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره شاسی :
            </td>
            <td>
                <asp:TextBox ID="txtCarChassisNo" runat="server" MaxLength="18" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                VIN شماره :
            </td>
            <td>
                <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="section-title">
                اطلاعات سوخت خودرو<br />
                <hr />
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نوع کارت سوخت :
            </td>
            <td>
                <asp:DropDownList ID="drpFuelCardType" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="آژانس"></asp:ListItem>
                    <asp:ListItem Value="1" Text="شخصی"></asp:ListItem>
                    <asp:ListItem Value="2" Text="خطی و بین راهی"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                شماره PAN کارت سوخت :
            </td>
            <td>
                <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                نوع سوخت :
            </td>
            <td>
                <asp:DropDownList ID="drpFuelType" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                    <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                    <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                    <asp:ListItem Value="3" Text="CNG"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">
                درصورت گازسوز بودن ازطرف کدام ارگان بوده :
            </td>
            <td>
                <asp:DropDownList ID="drpGASProvider" runat="server" CssClass="dropdown-middle">
                    <asp:ListItem Value="0" Text="هیچکدام"></asp:ListItem>
                    <asp:ListItem Value="1" Text="آژانس"></asp:ListItem>
                    <asp:ListItem Value="2" Text="آزاد"></asp:ListItem>
                    <asp:ListItem Value="3" Text="کارخانه"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
