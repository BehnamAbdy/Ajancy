<%@ Page Title="ابطال و جایگزین کارت سوخت" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="FCReplacement.aspx.cs" Inherits="Union_FCReplacement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            ابطال و جایگزین کارت سوخت
        </div>
    </center>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="section-title">
                        مشخصات کارت سوخت آژانسی (ابطالی)
                    </td>
                    <td class="section-title">
                        مشخصات کارت سوخت شخصی (جایگزینی)
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="fieldName">
                                    استان :
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
                                <td class="fieldName">
                                    شهر :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                                        DataValueField="CityID" AutoPostBack="true" OnSelectedIndexChanged="drpCity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                                        ControlToValidate="drpCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    آژانس :
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
                                <td class="fieldName">
                                    کد ملی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBox" MaxLength="10"
                                        OnTextChanged="txtNationalCode_TextChanged" AutoPostBack="true" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtNationalCode"
                                        Font-Size="8px" CssClass="validator" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="کد ملی را وارد کنید"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                                        Font-Size="8px" CssClass="validator" ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                                        CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام را وارد کنید"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام خانوادگی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName"
                                        CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    PAN کارت سوخت :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBox"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                                        CssClass="validator" ControlToValidate="txtFuelCardPAN"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    VIN شماره :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBox"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="شماره VIN را وارد کنید"
                                        Font-Size="8px" CssClass="validator" ControlToValidate="txtCarVIN"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="VIN باید 17 رقم باشد"
                                        Font-Size="8px" ControlToValidate="txtCarVIN" ClientValidationFunction="VINValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نوع خودرو :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCarType" runat="server" CssClass="dropdown" DataValueField="CarTypeID"
                                        DataTextField="TypeName">
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                    <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="drpCarType"
                                        ValueToCompare="0" Operator="GreaterThan" Type="Integer" CssClass="validator"
                                        ErrorMessage="نوع خودرو را انتخاب کنید"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    مدل خودرو :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarModel" runat="server" MaxLength="4" SkinID="TextBox" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="مدل خودرو را وارد کنید"
                                        ControlToValidate="txtCarModel" CssClass="validator" Font-Size="8px"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator3" runat="server" MinimumValue="1350" MaximumValue="1395"
                                        ErrorMessage="مدل باید بالاتر از 1349 باشد" Type="Integer" ControlToValidate="txtCarModel"
                                        CssClass="validator" Font-Size="8px"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نوع سوخت :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpFuelType" runat="server" CssClass="dropdown">
                                        <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="CNG"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    پلاک خودرو :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarPlateNumber_3" runat="server" SkinID="TextBox" MaxLength="2"
                                        Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtCarPlateNumber_3"></asp:RequiredFieldValidator>
                                    <span style="color: #019901; font-size: 14px;">ایران</span>
                                    <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" Width="50px" SkinID="TextBox"
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
                                    <asp:TextBox ID="txtCarPlateNumber_1" runat="server" SkinID="TextBox" MaxLength="2"
                                        Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtCarPlateNumber_1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="section-title" colspan="2">
                                    مالک خودرو (درصورتی پرشود که مالک خودرو شخصی غیر از بهره بردار باشد)
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    کد ملی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerNationalCode" runat="server" SkinID="TextBox" MaxLength="10"
                                        onkeypress="javascript:return isNumberKey(event)" OnTextChanged="txtOwnerNationalCode_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                                        CssClass="validator" ControlToValidate="txtOwnerNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerName" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام خانوادگی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerFamily" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td class="fieldName">
                                    استان :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpProvince_2" runat="server" CssClass="dropdown-middle" Enabled="false">
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
                                    <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                                        ControlToValidate="drpProvince_2" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    شهر :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCity_2" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                                        DataValueField="CityID" AutoPostBack="true" OnSelectedIndexChanged="drpCity_2_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                                        ControlToValidate="drpCity_2" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    آژانس :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpAjancies_2" runat="server" CssClass="dropdown-middle" DataValueField="AjancyID"
                                        DataTextField="AjancyName">
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                    <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="آژانس را انتخاب کنید"
                                        CssClass="validator" ControlToValidate="drpAjancies_2" ValueToCompare="0" Operator="GreaterThan"
                                        Type="Integer"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    کد ملی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNationalCode_2" runat="server" SkinID="TextBox" MaxLength="10"
                                        OnTextChanged="txtNationalCode_2_TextChanged" AutoPostBack="true" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNationalCode_2"
                                        Font-Size="8px" CssClass="validator" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="کد ملی را وارد کنید"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                                        Font-Size="8px" CssClass="validator" ControlToValidate="txtNationalCode_2" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFirstName_2" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName_2"
                                        CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام را وارد کنید"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام خانوادگی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastName_2" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLastName_2"
                                        CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    PAN کارت سوخت :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFuelCardPAN_2" runat="server" MaxLength="16" SkinID="TextBox"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                                        CssClass="validator" ControlToValidate="txtFuelCardPAN_2"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    VIN شماره :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarVIN_2" runat="server" MaxLength="17" SkinID="TextBox"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="شماره VIN را وارد کنید"
                                        Font-Size="8px" CssClass="validator" ControlToValidate="txtCarVIN_2"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="VIN باید 17 رقم باشد"
                                        Font-Size="8px" ControlToValidate="txtCarVIN_2" ClientValidationFunction="VINValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نوع خودرو :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCarType_2" runat="server" CssClass="dropdown" DataValueField="CarTypeID"
                                        DataTextField="TypeName">
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                    <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="drpCarType_2"
                                        ValueToCompare="0" Operator="GreaterThan" Type="Integer" CssClass="validator"
                                        ErrorMessage="نوع خودرو را انتخاب کنید"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    مدل خودرو :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarModel_2" runat="server" MaxLength="4" SkinID="TextBox" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <span class="star">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="مدل خودرو را وارد کنید"
                                        ControlToValidate="txtCarModel_2" CssClass="validator" Font-Size="8px"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" MinimumValue="1380" MaximumValue="1395"
                                        ErrorMessage="مدل باید بالاتر از 1380 باشد" Type="Integer" ControlToValidate="txtCarModel_2"
                                        CssClass="validator" Font-Size="8px"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نوع سوخت :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpFuelType_2" runat="server" CssClass="dropdown">
                                        <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="CNG"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="star">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    پلاک خودرو :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarPlateNumber_3_2" runat="server" SkinID="TextBox" MaxLength="2"
                                        Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtCarPlateNumber_3_2"></asp:RequiredFieldValidator>
                                    <span style="color: #019901; font-size: 14px;">ایران</span>
                                    <asp:TextBox ID="txtCarPlateNumber_2_2" runat="server" MaxLength="3" Width="50px"
                                        SkinID="TextBox" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtCarPlateNumber_2_2"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="drpCarPlateNumber_2" runat="server" CssClass="dropdown" Width="50px">
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
                                    <asp:TextBox ID="txtCarPlateNumber_1_2" runat="server" SkinID="TextBox" MaxLength="2"
                                        Width="30px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtCarPlateNumber_1_2"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="section-title" colspan="2">
                                    مالک خودرو (درصورتی پرشود که مالک خودرو شخصی غیر از بهره بردار باشد)
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    کد ملی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerNationalCode_2" runat="server" SkinID="TextBox" MaxLength="10"
                                        onkeypress="javascript:return isNumberKey(event)" OnTextChanged="txtOwnerNationalCode_2_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                                        CssClass="validator" ControlToValidate="txtOwnerNationalCode_2" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerName_2" runat="server" SkinID="TextBox" MaxLength="25"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldName">
                                    نام خانوادگی :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerFamily_2" runat="server" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
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
