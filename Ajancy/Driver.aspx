<%@ Page Title="فرم ثبت نام کارت صلاحیت رانندگان آژانس" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Driver.aspx.cs" Inherits="Ajancy_Driver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        فرم ثبت راننده جدید
    </div>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 40px;">
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            مشخصات راننده
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            AutoPostBack="True" OnTextChanged="txtNationalCode_TextChanged" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNationalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد ملی را وارد کنید" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"
                            CssClass="validator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام را وارد کنید" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"
                            CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام پدر :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFather" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFather"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام پدر را وارد کنید"
                            CssClass="validator"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateNo" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBirthCertificateNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="شماره شناسنامه را وارد کنید"
                            CssClass="validator"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">سريال شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateSerial" runat="server" SkinID="TextBoxMedium"
                            MaxLength="10" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">سري شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox CssClass="textbox" ID="txtBirthCertificateSerie" runat="server" Width="40px"
                            MaxLength="3" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox><span
                                class="star">*</span> /
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
                    <td class="fieldName-large">تاریخ تولد :
                    </td>
                    <td>
                        <userControl:Date ID="txtBirthDate" runat="server" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">محل تولد :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthPlace" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">محل صدور شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificatePlace" runat="server" SkinID="TextBoxMedium"
                            MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">جنسیت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpGender" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="مرد"></asp:ListItem>
                            <asp:ListItem Value="1" Text="زن"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">وضعيت تاهل :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMarriage" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="مجرد"></asp:ListItem>
                            <asp:ListItem Value="1" Text="متاهل"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">تعداد افراد تحت تکفل :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFamilyMembersCount" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">تحصیلات :
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
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">وضعیت نظام وظيفه :
                    </td>
                    <td>
                        <asp:DropDownList CssClass="dropdown-middle" ID="drpMilitaryService" runat="server">
                            <asp:ListItem Value="0" Text="دارای كارت پايان خدمت"></asp:ListItem>
                            <asp:ListItem Value="1" Text="دارای معافيت دایم"></asp:ListItem>
                            <asp:ListItem Value="2" Text="مشمول نمیباشد"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">دين :
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
                    <td class="fieldName-large">مذهب :
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubreligion" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">وضعیت شغلی :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpJobStatus" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="بیکار"></asp:ListItem>
                            <asp:ListItem Value="1" Text="بیکار مشمول بیمه بیکاری"></asp:ListItem>
                            <asp:ListItem Value="2" Text="شاغل"></asp:ListItem>
                            <asp:ListItem Value="3" Text="بازنشسته"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">تلفن ثابت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="14" SkinID="TextBoxMedium" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="تلفن ثابت را وارد کنید"
                            ControlToValidate="txtPhone" CssClass="validator"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">تلفن همراه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="11" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کد پستی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="10" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPostalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد پستی را وارد کنید"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="کد پستی باید 10 رقم باشد"
                            ControlToValidate="txtPostalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">آدرس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="150" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="آدرس را وارد کنید"
                            ControlToValidate="txtAddress"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            مشخصات دفترچه صلاحیت راننده
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">دفترچه صلاحیت دارد :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpDriverCertificationNo" runat="server" CssClass="dropdown-middle"
                            onchange="javascript:license(this)">
                            <asp:ListItem Text="بلی"></asp:ListItem>
                            <asp:ListItem Text="خیر"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره پرونده صلاحیت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDriverCertificationNo" runat="server" SkinID="TextBoxMedium"
                            MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            مشخصات گواهینامه
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">تاریخ صدور :
                    </td>
                    <td>
                        <userControl:Date ID="txtDrivingLicenseDate" runat="server" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره گواهینامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDrivingLicenseNo" runat="server" SkinID="TextBoxMedium" MaxLength="15"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="شماره گواهینامه را وارد کنید"
                            ControlToValidate="txtDrivingLicenseNo" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">محل صدور :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDrivingLicensePlace" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="محل صدور گواهینامه را وارد کنید"
                            ControlToValidate="txtDrivingLicensePlace" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع گواهینامه :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpDrivingLicenseType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="ب 1"></asp:ListItem>
                            <asp:ListItem Value="1" Text="ب 2"></asp:ListItem>
                            <asp:ListItem Value="2" Text="پایه یکم"></asp:ListItem>
                            <asp:ListItem Value="3" Text="پایه دوم"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            اطلاعات خودرو
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع خودرو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarType" runat="server" CssClass="dropdown-middle" DataValueField="CarTypeID"
                            DataTextField="TypeName">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="drpCarType"
                            ValueToCompare="0" Operator="GreaterThan" Type="Integer" CssClass="validator"
                            ErrorMessage="نوع خودرو را انتخاب کنید"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">مدل خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarModel" runat="server" MaxLength="4" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="مدل خودرو گواهینامه را وارد کنید"
                            ControlToValidate="txtCarModel" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" MinimumValue="1340" MaximumValue="1395"
                            ErrorMessage="مدل خودرو نادرست میباشد" Type="Integer" ControlToValidate="txtCarModel"
                            CssClass="validator"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">رنگ خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarColor" runat="server" MaxLength="10" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="رنگ خودرو را وارد کنید"
                            ControlToValidate="txtCarColor" CssClass="validator"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره پلاک خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarPlateNumber_3" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCarPlateNumber_3" CssClass="validator"></asp:RequiredFieldValidator>
                        <span style="color: #019901; font-size: 14px;">ایران</span>
                        <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" Width="50px" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCarPlateNumber_2" CssClass="validator"></asp:RequiredFieldValidator>
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
                            Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCarPlateNumber_1" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره موتور :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarEngineNo" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="شماره موتور را وارد کنید"
                            ControlToValidate="txtCarEngineNo" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره شاسی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarChassisNo" runat="server" MaxLength="18" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="شماره شاسی را وارد کنید"
                            ControlToValidate="txtCarChassisNo" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">VIN شماره :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="شماره VIN را وارد کنید"
                            ControlToValidate="txtCarVIN" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="کد VIN باید 17 رقم باشد"
                            ControlToValidate="txtCarVIN" ClientValidationFunction="VINValidate" CssClass="validator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            اطلاعات سوخت خودرو
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع کارت سوخت :
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
                    <td class="fieldName-large">PAN کارت سوخت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                            ControlToValidate="txtFuelCardPAN" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع سوخت :
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
                    <td class="fieldName-large">درصورت گازسوز بودن ازطرف کدام ارگان بوده :
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
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            مالک خودرو: (این قسمت درصورتی پر شود که مالک خودرو شخصی غیر از بهره بردار باشد)
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOwnerNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            ControlToValidate="txtOwnerNationalCode" ClientValidationFunction="nationalCodeValidate"
                            CssClass="validator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOwnerName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOwnerFamily" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="2">
                        <p class="section-title">
                            اگر در دفترچه صلاحیت مشخصات خودرو دیگری ثبت شده است قسمت زیر کامل تکمیل گردد
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع خودرو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFormerCarType" runat="server" CssClass="dropdown-middle"
                            DataValueField="CarTypeID" DataTextField="TypeName">
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        مدل خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerCarModel" runat="server" MaxLength="4" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" MinimumValue="1340" MaximumValue="1390"
                            ErrorMessage="مدل خودرو نادرست میباشد" Type="Integer" ControlToValidate="txtFormerCarModel"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره موتور :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerCarEngineNo" runat="server" MaxLength="11" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره شاسی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerCarChassisNo" runat="server" MaxLength="18" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        VIN شماره :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="کد VIN باید 17 رقم باشد"
                            ControlToValidate="txtFormerCarVIN" ClientValidationFunction="VINValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFormerCarFuelType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="-1" Text="- انتخاب کنید -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                            <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                            <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع کارت سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFormerCarFuelCardType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="-1" Text="- انتخاب کنید -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="آژانس"></asp:ListItem>
                            <asp:ListItem Value="1" Text="شخصی"></asp:ListItem>
                            <asp:ListItem Value="2" Text="خطی و بین راهی"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره PAN کارت سوخت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerCarPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>--%>
                <%--<tr>
                    <td colspan="2">
                        <p class="section-title">
                            اطلاعات شغل قبلی
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شغل قبلی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerJob" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع شغل :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFormerJobType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -"></asp:ListItem>
                            <asp:ListItem Value="1" Text="نظامی شاغل"></asp:ListItem>
                            <asp:ListItem Value="2" Text="نظامی بازنشسته"></asp:ListItem>
                            <asp:ListItem Value="3" Text="کارمند شاغل"></asp:ListItem>
                            <asp:ListItem Value="4" Text="کارمند بازنشسته"></asp:ListItem>
                            <asp:ListItem Value="5" Text="کارمند بازخرید"></asp:ListItem>
                            <asp:ListItem Value="6" Text="آزاد"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>--%>
            </table>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function license(drp) {
            if (drp.selectedIndex == 0) {
                $get('<%=txtDriverCertificationNo.ClientID %>').disabled = false;
            }
            else {
                $get('<%=txtDriverCertificationNo.ClientID %>').disabled = true;
                $get('<%=txtDriverCertificationNo.ClientID %>').value = '';
            }
        }

    </script>
</asp:Content>
