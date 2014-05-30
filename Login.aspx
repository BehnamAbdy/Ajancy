<%@ Page Title="ورود به سیستم" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div id="login-header">
    </div>
    <div id="login-body">
        <br />
        <br />
        <img src="App_Themes/Default/images/phone.png" />
        <asp:Panel ID="pnl" runat="server" DefaultButton="btnLogin">
            <table style="margin: 0px 90px 0px 0px;">
                <tr>
                    <td class="label-login">
                        نام کاربری:
                    </td>
                    <td align="right">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox-login"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label-login">
                        گذرواژه:
                    </td>
                    <td align="right">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox-login" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trMembership" runat="server" visible="false">
                    <td class="label-login">
                        سمت:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMemberships" runat="server" CssClass="dropdown" Height="22px"
                            Width="164px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnLogin" runat="server" CssClass="button-login" Text="ورود" OnClick="btnLogin_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"
                            Font-Size="10px"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div id="login-footer">
    </div>
    <br />
    <br />
    <br />
    <br />
</asp:Content>
