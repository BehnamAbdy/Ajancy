<%@ Page Title="ساخت کاربر جدید" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="User.aspx.cs" Inherits="Management_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            فرم ساخت کاربر جدید
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 100px;">
                <tr>
                    <td class="fieldName-large">
                        کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            OnTextChanged="txtNationalCode_TextChanged" AutoPostBack="true" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNationalCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtFirstName"
                            SetFocusOnError="true" Display="Dynamic" CssClass="validator" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtLastName"
                            SetFocusOnError="true" Display="Dynamic" CssClass="validator" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تلفن همراه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="11" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
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
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*" CssClass="validator"
                            ControlToValidate="drpProvince" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" Enabled="false">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*" CssClass="validator"
                            ControlToValidate="drpCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        سمت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpRoles" runat="server" CssClass="dropdown-middle">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpRoles" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        گذرواژه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" SkinID="TextBoxMedium" TextMode="Password"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تکرار گذرواژه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRePassword" runat="server" SkinID="TextBoxMedium" TextMode="Password"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtRePassword"
                            ControlToCompare="txtPassword" CssClass="validator"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRePassword"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*" CssClass="validator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"
                    Text="ذخیره" UseSubmitBehavior="false" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
