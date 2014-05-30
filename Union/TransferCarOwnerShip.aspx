<%@ Page Title="(VIN) انتقال مالکیت خوردو" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="TransferCarOwnerShip.aspx.cs" Inherits="Union_TransferCarOwnerShip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        انتقال مالکیت خوردو(VIN)
    </div>
    <div class="alarm">
        خودرویی که با شماره VIN آن قبلا در این سامانه ثبت شده و سپس مالکیت آن خودرو به شخص
        دیگری واگذار گردیده است، اطلاعات مالک و راننده جدید به همراه شماره پلاک جدید خودرو
        در این قسمت وارد گردد.
    </div>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 50px;">
                <tr>
                    <td class="fieldName-large" style="width: 150px;">
                        VIN شماره :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBox"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click"
                            CausesValidation="false" />
                    </td>
                    <td style="width: 18px;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="../App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="شماره VIN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtCarVIN"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="کد VIN باید 17 رقم باشد"
                            ControlToValidate="txtCarVIN" ClientValidationFunction="VINValidate"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <table style="margin-right: 50px;">
                <tr>
                    <td class="fieldName">
                        نوع خودرو
                    </td>
                    <td>
                        <asp:Label ID="lblTypeName" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        پلاک خودرو
                    </td>
                    <td>
                        <asp:Label ID="lblPlateNumber" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        PAN
                    </td>
                    <td>
                        <asp:Label ID="lblPAN" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد ملی راننده :
                    </td>
                    <td>
                        <asp:Label ID="lblDriverNationalCode" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام راننده :
                    </td>
                    <td>
                        <asp:Label ID="lblDriverName" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد ملی مالک :
                    </td>
                    <td>
                        <asp:Label ID="lblOwnerNationalCode" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام مالک :
                    </td>
                    <td>
                        <asp:Label ID="lblOwnerName" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="section-title" colspan="2">
                        مشخصات مالک و راننده (بهره بردار) جدید خودرو
                    </td>
                    <tr>
                        <td class="fieldName-large">
                            استان :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" Enabled="false">
                                <asp:ListItem Text="- انتخاب کنید -" Value="0" />
                                <asp:ListItem Text="آذربايجان شرقي" Value="41" />
                                <asp:ListItem Text="آذربايجان غربي" Value="44" />
                                <asp:ListItem Text="اردبيل" Value="45" />
                                <asp:ListItem Text="اصفهان" Value="31" />
                                <asp:ListItem Text="البرز" Value="88" />
                                <asp:ListItem Text="ايلام" Value="84" />
                                <asp:ListItem Text="بوشهر" Value="77" />
                                <asp:ListItem Text="تهران" Value="21" />
                                <asp:ListItem Text="چهارمحال بختياري" Value="38" />
                                <asp:ListItem Text="خراسان جنوبي" Value="56" />
                                <asp:ListItem Text="خراسان رضوي" Value="51" />
                                <asp:ListItem Text="خراسان شمالي" Value="58" />
                                <asp:ListItem Text="خوزستان" Value="61" />
                                <asp:ListItem Text="زنجان" Value="24" />
                                <asp:ListItem Text="سمنان" Value="23" />
                                <asp:ListItem Text="سيستان و بلوچستان" Value="54" />
                                <asp:ListItem Selected="True" Text="فارس" Value="71" />
                                <asp:ListItem Text="قزوين" Value="28" />
                                <asp:ListItem Text="قم" Value="25" />
                                <asp:ListItem Text="كردستان" Value="87" />
                                <asp:ListItem Text="كرمان" Value="34" />
                                <asp:ListItem Text="كرمانشاه" Value="83" />
                                <asp:ListItem Text="كهكيلويه و بويراحمد" Value="74" />
                                <asp:ListItem Text="گلستان" Value="17" />
                                <asp:ListItem Text="گيلان" Value="13" />
                                <asp:ListItem Text="لرستان" Value="66" />
                                <asp:ListItem Text="مازندران" Value="15" />
                                <asp:ListItem Text="مركزي" Value="86" />
                                <asp:ListItem Text="هرمزگان" Value="76" />
                                <asp:ListItem Text="همدان" Value="81" />
                                <asp:ListItem Text="يزد" Value="35" />
                            </asp:DropDownList>
                            <span class="star">*</span>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="drpProvince"
                                ErrorMessage="*" Operator="GreaterThan" SkinID="CompareValidator" Type="Integer"
                                ValueToCompare="0"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            شهر :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpCity" runat="server" AutoPostBack="True" CssClass="dropdown-middle"
                                DataTextField="Name" DataValueField="CityID" OnSelectedIndexChanged="drpCity_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="star">*</span>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="drpCity"
                                ErrorMessage="*" Operator="GreaterThan" SkinID="CompareValidator" Type="Integer"
                                ValueToCompare="0"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            آژانس :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpAjancies" runat="server" CssClass="dropdown-middle" DataTextField="AjancyName"
                                DataValueField="AjancyID">
                            </asp:DropDownList>
                            <span class="star">*</span>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="drpAjancies"
                                CssClass="validator" ErrorMessage="آژانس را انتخاب کنید" Operator="GreaterThan"
                                Type="Integer" ValueToCompare="0"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="section-title" colspan="2">
                            مشخصات راننده خودرو (بهره بردار )
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            کد ملی :
                        </td>
                        <td>
                            <asp:TextBox ID="txtNationalCode" runat="server" AutoPostBack="true" MaxLength="10"
                                onkeypress="javascript:return isNumberKey(event)" OnTextChanged="txtNationalCode_TextChanged"
                                SkinID="TextBoxMedium"></asp:TextBox>
                            <span class="star">*</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtNationalCode"
                                CssClass="validator" Display="Dynamic" ErrorMessage="کد ملی را وارد کنید" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator11" runat="server" ClientValidationFunction="nationalCodeValidate"
                                ControlToValidate="txtNationalCode" CssClass="validator" ErrorMessage="کد ملی باید 10 رقم باشد"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            نام :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="25" SkinID="TextBoxMedium"></asp:TextBox>
                            <span class="star">*</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                                CssClass="validator" Display="Dynamic" ErrorMessage="نام را وارد کنید" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            نام خانوادگی :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" SkinID="TextBoxMedium"></asp:TextBox>
                            <span class="star">*</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName"
                                CssClass="validator" Display="Dynamic" ErrorMessage="نام خانوادگی را وارد کنید"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            پلاک جدید خودرو :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCarPlateNumber_3" runat="server" MaxLength="2" onkeypress="javascript:return isNumberKey(event)"
                                SkinID="TextBoxMedium" Width="30px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtCarPlateNumber_3"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                            <span style="color: #019901; font-size: 14px;">ایران</span>
                            <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" onkeypress="javascript:return isNumberKey(event)"
                                SkinID="TextBoxMedium" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCarPlateNumber_2"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="drpCarPlateNumber" runat="server" CssClass="dropdown" Width="50px">
                                <asp:ListItem Text="الف" Value="A"></asp:ListItem>
                                <asp:ListItem Text="ب" Value="B"></asp:ListItem>
                                <asp:ListItem Text="پ" Value="C"></asp:ListItem>
                                <asp:ListItem Text="ت" Value="D"></asp:ListItem>
                                <asp:ListItem Text="ث" Value="E"></asp:ListItem>
                                <asp:ListItem Text="ج" Value="F"></asp:ListItem>
                                <asp:ListItem Text="چ" Value="G"></asp:ListItem>
                                <asp:ListItem Text="ح" Value="H"></asp:ListItem>
                                <asp:ListItem Text="خ" Value="I"></asp:ListItem>
                                <asp:ListItem Text="د" Value="J"></asp:ListItem>
                                <asp:ListItem Text="ذ" Value="K"></asp:ListItem>
                                <asp:ListItem Text="ر" Value="L"></asp:ListItem>
                                <asp:ListItem Text="ز" Value="M"></asp:ListItem>
                                <asp:ListItem Text="ژ" Value="N"></asp:ListItem>
                                <asp:ListItem Text="س" Value="O"></asp:ListItem>
                                <asp:ListItem Text="ش" Value="P"></asp:ListItem>
                                <asp:ListItem Text="ص" Value="Q"></asp:ListItem>
                                <asp:ListItem Text="ض" Value="R"></asp:ListItem>
                                <asp:ListItem Text="ط" Value="S"></asp:ListItem>
                                <asp:ListItem Text="ظ" Value="T"></asp:ListItem>
                                <asp:ListItem Text="ع" Value="U"></asp:ListItem>
                                <asp:ListItem Text="غ" Value="V"></asp:ListItem>
                                <asp:ListItem Text="ف" Value="W"></asp:ListItem>
                                <asp:ListItem Text="ق" Value="X"></asp:ListItem>
                                <asp:ListItem Text="ک" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="گ" Value="Z"></asp:ListItem>
                                <asp:ListItem Text="ل" Value="1"></asp:ListItem>
                                <asp:ListItem Text="م" Value="2"></asp:ListItem>
                                <asp:ListItem Text="ن" Value="3"></asp:ListItem>
                                <asp:ListItem Text="و" Value="4"></asp:ListItem>
                                <asp:ListItem Text="ه" Value="5"></asp:ListItem>
                                <asp:ListItem Text="ی" Value="6"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtCarPlateNumber_1" runat="server" MaxLength="2" onkeypress="javascript:return isNumberKey(event)"
                                SkinID="TextBoxMedium" Width="30px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtCarPlateNumber_1"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="section-title" colspan="2">
                            مالک خودرو (این قسمت درصورتی پر شود که مالک خودرو شخصی غیر از بهره بردار باشد)
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            کد ملی :
                        </td>
                        <td>
                            <asp:TextBox ID="txtOwnerNationalCode" runat="server" MaxLength="10" SkinID="TextBoxMedium"
                                AutoPostBack="true" onkeypress="javascript:return isNumberKey(event)" OnTextChanged="txtOwnerNationalCode_TextChanged"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="nationalCodeValidate"
                                ControlToValidate="txtOwnerNationalCode" CssClass="validator" ErrorMessage="کد ملی باید 10 رقم باشد"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            نام :
                        </td>
                        <td>
                            <asp:TextBox ID="txtOwnerName" runat="server" MaxLength="25" SkinID="TextBoxMedium"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName-large">
                            نام خانوادگی :
                        </td>
                        <td>
                            <asp:TextBox ID="txtOwnerFamily" runat="server" MaxLength="30" SkinID="TextBoxMedium"></asp:TextBox>
                        </td>
                    </tr>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" Enabled="false" CssClass="button" Text="ثبت"
                    OnClick="btnSave_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
            <asp:HiddenField ID="hdnValues" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
