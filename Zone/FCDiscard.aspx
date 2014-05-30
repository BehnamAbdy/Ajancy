<%@ Page Title="ثبت کارت سوخت خطی و بین راهی" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="FCDiscard.aspx.cs" Inherits="Zone_FCDiscard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ثبت کارت سوخت خطی و بین راهی - پلاک منطقه آزاد
    </div>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <div class="centered">
                <table>
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
                </table>
                <table>
                    <tr>
                        <td class="fieldName-large">پلاک :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCarPlateNumber_5" runat="server" SkinID="TextBoxMedium" MaxLength="5"
                                Width="40px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtCarPlateNumber_5"></asp:RequiredFieldValidator>

                        </td>
                        <td>
                            <asp:DropDownList ID="drpCarPlateNumberCity" runat="server" CssClass="dropdown" DataTextField="Name"
                                DataValueField="CityID">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="*"
                                ControlToValidate="drpCarPlateNumberCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpCarPlateNumberProvince" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="drpCarPlateNumberProvince_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="- استان -" />
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
                            <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="*"
                                ControlToValidate="drpCarPlateNumberProvince" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <table>
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
            </div>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
