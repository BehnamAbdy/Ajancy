<%@ Page Title="ویرایش اطلاعات پرسنل" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Person.aspx.cs" Inherits="Management_Person" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            فرم ویرایش اطلاعات پرسنل
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 80px;">
                <tr>
                    <td colspan="2">
                        <p class="section-title">
                            مشخصات شخص
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        استان :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" AutoPostBack="true"
                            OnSelectedIndexChanged="drpProvince_SelectedIndexChanged" Enabled="false">
                            <asp:ListItem Value="0" Text="- همه موارد -" />
                            <asp:ListItem Value="41" Text="آذربايجان شرقي" />
                            <asp:ListItem Value="44" Text="آذربايجان غربي" />
                            <asp:ListItem Value="45" Text="اردبيل" />
                            <asp:ListItem Value="31" Text="اصفهان" />
                            <asp:ListItem Value="88" Text="البرز" />
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
                            <asp:ListItem Value="71" Text="فارس" Selected="True" />
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
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" Enabled="false">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtNationalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد ملی را وارد کنید" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"
                            CssClass="validator"></asp:CustomValidator>
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
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام را وارد کنید" CssClass="validator"></asp:RequiredFieldValidator>
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
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"
                            CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام پدر :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFather" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
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
                        <userControl:Date ID="txtBirthDate" runat="server" Required="false" />
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
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        آدرس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="150" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
