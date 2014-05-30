<%@ Page Title="فرم ثبت نام کارت صلاحیت رانندگان آژانس" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Driver.aspx.cs" Inherits="Union_Driver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        فرم ثبت راننده جدید
    </div>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table class="centered">
                <tr>
                    <td class="fieldName-large">استان :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" AutoPostBack="true"
                            OnSelectedIndexChanged="drpProvince_SelectedIndexChanged" Enabled="false">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
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
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpProvince" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" AutoPostBack="True" OnSelectedIndexChanged="drpCity_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">آژانس :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancies" runat="server" CssClass="dropdown-middle" DataValueField="AjancyID"
                            DataTextField="AjancyName">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="آژانس را انتخاب کنید"
                            CssClass="validator" ControlToValidate="drpAjancies" ValueToCompare="0" Operator="GreaterThan"
                            Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="section-title" colspan="2">مشخصات راننده خودرو (بهره بردار )
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            OnTextChanged="txtNationalCode_TextChanged" AutoPostBack="true" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtNationalCode"
                            CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد ملی را وارد کنید"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            CssClass="validator" ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                            CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName"
                            CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام پدر :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFather" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFather"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام پدر را وارد کنید"
                            CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateNo" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBirthCertificateNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="شماره شناسنامه را وارد کنید"
                            CssClass="validator"></asp:RequiredFieldValidator>
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
                    <td class="fieldName-large">تلفن ثابت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="14" SkinID="TextBoxMedium" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="تلفن ثابت را وارد کنید"
                            ControlToValidate="txtPhone" CssClass="validator"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPostalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد پستی را وارد کنید"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="کد پستی باید 10 رقم باشد"
                            ControlToValidate="txtPostalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">آدرس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="150" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="آدرس را وارد کنید"
                            ControlToValidate="txtAddress"></asp:RequiredFieldValidator>
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
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtFuelCardPAN"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">VIN شماره :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="شماره VIN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtCarVIN"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="کد VIN باید 17 رقم باشد"
                            ControlToValidate="txtCarVIN" ClientValidationFunction="VINValidate"></asp:CustomValidator>
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
                        <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="drpCarType"
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="مدل خودرو را وارد کنید"
                            ControlToValidate="txtCarModel" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" MinimumValue="1350" MaximumValue="1395"
                            ErrorMessage="مدل باید بالاتر از 1349 باشد" Type="Integer" ControlToValidate="txtCarModel"
                            CssClass="validator"></asp:RangeValidator>
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
                        <span class="star">*</span>
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
                    <td class="fieldName-large">پلاک خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarPlateNumber_3" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCarPlateNumber_3"></asp:RequiredFieldValidator>
                        <span style="color: #019901; font-size: 14px;">ایران</span>
                        <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" Width="50px" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCarPlateNumber_2"></asp:RequiredFieldValidator>
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
                            ControlToValidate="txtCarPlateNumber_1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="section-title" colspan="2">مالک خودرو (این قسمت درصورتی پر شود که مالک خودرو شخصی غیر از بهره بردار باشد)
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOwnerNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)" OnTextChanged="txtOwnerNationalCode_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            CssClass="validator" ControlToValidate="txtOwnerNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
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
