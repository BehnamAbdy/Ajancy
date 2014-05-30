<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="EditPassword.aspx.cs" Inherits="Members_EditPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            ویرایش گذرواژه
        </div>
    </center>
    <center>
        <p id="warning">
            گذرواژه جدید نباید کمتر از 5 حرف باشد
        </p>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 150px;">
                <tr>
                    <td class="fieldName">
                        نام کاربری :
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" SkinID="TextBoxMedium" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        گذرواژه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOldPassword" runat="server" SkinID="TextBoxMedium" TextMode="Password"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOldPassword"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        گذرواژه جدید :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewPassword" runat="server" SkinID="TextBoxMedium" TextMode="Password"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewPassword"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valPasswordLength" runat="server" ControlToValidate="txtNewPassword"
                            SetFocusOnError="true" Display="Dynamic" ValidationExpression="\w{5,}" ErrorMessage="طول گذرواژه نباید کمتر از 5 حرف باشد."
                            ToolTip="طول گذرواژه نباید کمتر از 5 حرف باشد." CssClass="validator"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        تکرار گذرواژه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRePassword" runat="server" SkinID="TextBoxMedium" TextMode="Password"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtRePassword"
                            ControlToCompare="txtNewPassword" CssClass="validator"></asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="تایید" OnClick="btnSave_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
