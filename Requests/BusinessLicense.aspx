<%@ Page Title="درخواست پروانه کسب آژانس" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="BusinessLicense.aspx.cs" Inherits="Requests_BusinessLicense" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            فرم ثبت نام درخواست پروانه کسب آژانس
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 135px;">
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            نوع پروانه کسب مورد درخواست
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        رسته شغلی :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancyType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="آژانس تاکسی تلفنی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="آژانس موتور تلفنی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع درخواست :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpBusinessLicenseType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="عادی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="ویژه ایثارگران و جانبازان و شهدا"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            اطلاعات درخواست کننده
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            AutoPostBack="true" OnTextChanged="txtNationalCode_TextChanged" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtNationalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد ملی را وارد کنید"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام پدر :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFather" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFather"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام پدر را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateNo" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBirthCertificateNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="شماره شناسنامه را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        سريال شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateSerial" runat="server" SkinID="TextBoxMedium"
                            MaxLength="10" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        سري شناسنامه :
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
                        <span class="star">*</span>
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
                        <span class="star">*</span>
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
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        وضعيت تاهل
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMarriage" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="مجرد"></asp:ListItem>
                            <asp:ListItem Value="1" Text="متاهل"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تعداد افراد تحت تکفل :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFamilyMembersCount" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
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
                        <span class="star">*</span>
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
                        <span class="star">*</span>
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
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تلفن ثابت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="14" SkinID="TextBoxMedium" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="تلفن ثابت را وارد کنید"
                            ControlToValidate="txtPhone"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تلفن همراه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="11" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="تلفن همراه را وارد کنید"
                            ControlToValidate="txtMobile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد پستی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="10" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPostalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد پستی را وارد کنید"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="کد پستی باید 10 رقم باشد"
                            ControlToValidate="txtPostalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        آدرس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="150" SkinID="TextBox"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="آدرس را وارد کنید"
                            ControlToValidate="txtAddress"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            اطلاعات واحد تجاری
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        موقعیت مکانی دفتر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpOfficePosition" runat="server" CssClass="dropdown-middle"
                            onchange="javascript:officeLevel()">
                            <asp:ListItem Value="0" Text="مستقل"></asp:ListItem>
                            <asp:ListItem Value="1" Text="درون مجتمع (طبقه همکف)"></asp:ListItem>
                            <asp:ListItem Value="2" Text="درون مجتمع (طبقه غیر همکف)"></asp:ListItem>
                            <asp:ListItem Value="3" Text="درون مجتمع کارگاهی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        موقعیت دفتر (طبقه چندم) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOfficeLevel" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            Enabled="false" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        مساحت دفتر :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOfficeSpace" runat="server" SkinID="TextBoxMedium" MaxLength="6"
                            onkeypress="javascript:return isFloatKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="مساحت دفتر را وارد کنید"
                            ControlToValidate="txtOfficeSpace"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        مساحت پارکینگ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtParkingSpace" runat="server" SkinID="TextBoxMedium" MaxLength="6"
                            onkeypress="javascript:return isFloatKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="مساحت پارکینگ را وارد کنید"
                            ControlToValidate="txtParkingSpace"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        پارکینگ به دفتر چسبیده است :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpParkingState" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="بلی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="خیر"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        بالکن دارد :
                    </td>
                    <td>
                        <select id="drpBalcony" class="dropdown-middle" onchange="javascript:balcony()">
                            <option>بلی</option>
                            <option>خیر</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        مساحت بالکن :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBalconySpace" runat="server" SkinID="TextBoxMedium" MaxLength="6"
                            onkeypress="javascript:return isFloatKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        ارتفاع بالکن :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBalconyHeight" runat="server" SkinID="TextBoxMedium" MaxLength="5"
                            onkeypress="javascript:return isFloatKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تلفن :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBusinessPlacePhone" runat="server" MaxLength="15" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="txtBusinessPlacePhone"
                            ErrorMessage="تلفن ثابت را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره پلاک ثبتی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRegisteredPelak" runat="server" SkinID="TextBoxMedium" MaxLength="10"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره پلاک ثبتی را وارد کنید"
                            ControlToValidate="txtRegisteredPelak"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره پلاک آبی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBluePelak" runat="server" SkinID="TextBoxMedium" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        استان :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" Enabled="false"
                            AutoPostBack="true" OnSelectedIndexChanged="drpProvince_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
                            <asp:ListItem Value="41" Text="آذربايجان شرقي" />
                            <asp:ListItem Value="44" Text="آذربايجان غربي" />
                            <asp:ListItem Value="45" Text="اردبيل" />
                            <asp:ListItem Value="31" Text="اصفهان" />
                            <asp:ListItem Value="84" Text="ايلام" />
                            <asp:ListItem Value="77" Text="بوشهر" />
                            <asp:ListItem Value="21" Text="تهران" />
                            <asp:ListItem Value="38" Text="چهارمحال بختياري" />
                            <asp:ListItem Value="56" Text="خراسان جنوبي" />
                            <asp:ListItem Value="51" Text="خراسان رضوي" />
                            <asp:ListItem Value="58" Text="خراسان شمالي" />
                            <asp:ListItem Value="61" Text="خوزستان" />
                            <asp:ListItem Value="24" Text="زنجان" />
                            <asp:ListItem Value="23" Text="سمنان" />
                            <asp:ListItem Value="54" Text="سيستان و بلوچستان" />
                            <asp:ListItem Value="71" Text="فارس" />
                            <asp:ListItem Value="28" Text="قزوين" />
                            <asp:ListItem Value="25" Text="قم" />
                            <asp:ListItem Value="87" Text="كردستان" />
                            <asp:ListItem Value="34" Text="كرمان" />
                            <asp:ListItem Value="83" Text="كرمانشاه" />
                            <asp:ListItem Value="74" Text="كهكيلويه و بويراحمد" />
                            <asp:ListItem Value="17" Text="گلستان" />
                            <asp:ListItem Value="13" Text="گيلان" />
                            <asp:ListItem Value="66" Text="لرستان" />
                            <asp:ListItem Value="15" Text="مازندران" />
                            <asp:ListItem Value="86" Text="مركزي" />
                            <asp:ListItem Value="76" Text="هرمزگان" />
                            <asp:ListItem Value="81" Text="همدان" />
                            <asp:ListItem Value="35" Text="يزد" />
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpProvince" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد پستی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlacePostalCode" runat="server" MaxLength="10" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="کد پستی باید 10 رقم باشد"
                            ControlToValidate="txtPlacePostalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع ملک:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpEstateType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="ملکی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="استیجاری"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع سند:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpDocumentType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="رسمی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="اوقاف"></asp:ListItem>
                            <asp:ListItem Value="2" Text="اداری"></asp:ListItem>
                            <asp:ListItem Value="3" Text="عادی"></asp:ListItem>
                            <asp:ListItem Value="4" Text="محضری"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام مالک :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlaceOwner" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        امکانات :
                    </td>
                    <td>
                        <asp:CheckBoxList ID="lstUtility" runat="server" RepeatDirection="Horizontal" Width="350px">
                            <asp:ListItem Value="0" Text="آب"></asp:ListItem>
                            <asp:ListItem Value="1" Text="برق"></asp:ListItem>
                            <asp:ListItem Value="2" Text="گاز"></asp:ListItem>
                            <asp:ListItem Value="3" Text="تلفن"></asp:ListItem>
                            <asp:ListItem Value="4" Text="فکس"></asp:ListItem>
                            <asp:ListItem Value="5" Text="سرویس بهداشتی"></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره سریال قبض آب :
                    </td>
                    <td>
                        <asp:TextBox ID="txtWaterBillSerial" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtWaterBillSerial"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="شماره قبض آب را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره سریال قبض برق :
                    </td>
                    <td>
                        <asp:TextBox ID="txtElectricityBillSerial" runat="server" SkinID="TextBoxMedium"
                            MaxLength="20"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtElectricityBillSerial"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="شماره قبض برق را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره سریال قبض گاز :
                    </td>
                    <td>
                        <asp:TextBox ID="txtGasBillSerial" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtGasBillSerial"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="شماره قبض گاز را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        فاصله تا اولین آژانس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBusinessScope" runat="server" SkinID="TextBoxMedium" MaxLength="5"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtBusinessScope"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="فاصله تا اولین آژانس را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        حوزه کلانتری :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPoliceStation" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtPoliceStation"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="حوزه کلانتری را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        حوزه شهرداری :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMayor" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="1" Text="شهرداری منطقه 1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="شهرداری منطقه 2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="شهرداری منطقه 3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="شهرداری منطقه 4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="شهرداری منطقه 5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="شهرداری منطقه 6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="شهرداری منطقه 7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="شهرداری منطقه 8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="شهرداری منطقه 9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="گلستان"></asp:ListItem>
                            <asp:ListItem Value="11" Text="قلات"></asp:ListItem>
                            <asp:ListItem Value="12" Text="لپویی"></asp:ListItem>
                            <asp:ListItem Value="13" Text="گویم"></asp:ListItem>
                            <asp:ListItem Value="14" Text="مرکزی"></asp:ListItem>
                            <asp:ListItem Value="15" Text="دوکوهک"></asp:ListItem>
                            <asp:ListItem Value="16" Text="کوار"></asp:ListItem>
                            <asp:ListItem Value="17" Text="صدرا"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        آدرس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlaceAddress" runat="server" SkinID="TextBox" Width="350px" Height="40px"
                            Columns="3" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="آدرس را وارد کنید"
                            ControlToValidate="txtPlaceAddress"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            اطلاعات شغل قبلی
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تاریخ پروانه کسب :
                    </td>
                    <td>
                        <userControl:Date ID="txtFormerBusinessLicenseDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تاریخ افتتاح محل کسب :
                    </td>
                    <td>
                        <userControl:Date ID="txtFormerBusinessStartDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره پروانه کسب :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerBusinessLicenseNo" runat="server" SkinID="TextBoxMedium"
                            MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام تابلو محل کسب :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormerBusinessBoard" runat="server" SkinID="TextBoxMedium" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table id="partners">
                            <tr>
                                <td colspan="4" class="header">
                                    اسامی شریک یا شرکا
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ردیف
                                </td>
                                <td class="info">
                                    کد ملی
                                </td>
                                <td class="info">
                                    نام
                                </td>
                                <td class="info">
                                    نام خانوادگی
                                </td>
                            </tr>
                            <tr>
                                <td class="counter">
                                    1
                                </td>
                                <td>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server"
                                        FilterType="Numbers" TargetControlID="txtPartnerNationalCode1" />
                                    <asp:TextBox ID="txtPartnerNationalCode1" runat="server" SkinID="TextBox" MaxLength="10"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="*" Font-Size="16px"
                                        ControlToValidate="txtPartnerNationalCode1" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartnerName1" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartnerFamily1" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="counter">
                                    2
                                </td>
                                <td>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server"
                                        FilterType="Numbers" TargetControlID="txtPartnerNationalCode2" />
                                    <asp:TextBox ID="txtPartnerNationalCode2" runat="server" SkinID="TextBox" MaxLength="10"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="*" Font-Size="16px"
                                        ControlToValidate="txtPartnerNationalCode2" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartnerName2" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartnerFamily2" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="counter">
                                    3
                                </td>
                                <td>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server"
                                        FilterType="Numbers" TargetControlID="txtPartnerNationalCode3" />
                                    <asp:TextBox ID="txtPartnerNationalCode3" runat="server" SkinID="TextBox" MaxLength="10"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="*" Font-Size="16px"
                                        ControlToValidate="txtPartnerNationalCode3" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartnerName3" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartnerFamily3" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
            </div>
            <asp:HiddenField ID="hdnCityID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function officeLevel() {
            if ($get('<%=drpOfficePosition.ClientID %>').selectedIndex == 2) {
                $get('<%=txtOfficeLevel.ClientID %>').disabled = false;
            }
            else {
                $get('<%=txtOfficeLevel.ClientID %>').disabled = true;
                $get('<%=txtOfficeLevel.ClientID %>').value = '';
            }
        }

        function balcony() {
            if (document.getElementById('drpBalcony').selectedIndex == 0) {
                $get('<%=txtBalconySpace.ClientID %>').disabled = false;
                $get('<%=txtBalconyHeight.ClientID %>').disabled = false;
            }
            else {
                $get('<%=txtBalconySpace.ClientID %>').disabled = true;
                $get('<%=txtBalconyHeight.ClientID %>').disabled = true;
                $get('<%=txtBalconySpace.ClientID %>').value = '';
                $get('<%=txtBalconyHeight.ClientID %>').value = '';
            }
        }

        function setCity() {
            if ($get('<%=hdnCityID.ClientID %>').selectedUndex == 0) {
                $get('<%=hdnCityID.ClientID %>').value = '';
            }
            else {
                $get('<%=hdnCityID.ClientID %>').value = $get('<%=drpProvince.ClientID %>').value + '|' + $get('<%=drpCity.ClientID %>').value;
            }
        }

    </script>
</asp:Content>
