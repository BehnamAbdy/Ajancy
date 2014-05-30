<%@ Page Title="مدیریت سمت های کاربران سیستم" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="UserRoles.aspx.cs" Inherits="Management_UserRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            مدیریت سمت های کاربران سیستم
        </div>
    </center>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin-right: 90px; width: 400px;">
                <tr>
                    <td class="section-title" id="tdTitle" runat="server">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBoxList ID="lstRoles" runat="server" RepeatDirection="Vertical" DataTextField="Role"
                            DataValueField="UserRoleID" AutoPostBack="true" OnSelectedIndexChanged="lstRoles_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <table style="margin-right: 90px; width: 400px;">
                <tr>
                    <td class="section-title" colspan="2">
                        سمت جدید
                    </td>
                </tr>
                <td>
                    <td>
                        <asp:DropDownList ID="drpRoles" runat="server" CssClass="dropdown-middle">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpRoles" ValueToCompare="0" Operator="GreaterThan" Type="Integer"
                            CssClass="validator"></asp:CompareValidator>
                    </td>
                    <td>
                    </td>
                </td>
            </table>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"
                    Text="ذخیره" UseSubmitBehavior="false" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
