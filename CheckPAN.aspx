<%@ Page Title="پیگیری وضعیت کارت سوخت هوشمند" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="CheckPAN.aspx.cs" Inherits="CheckPAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            پیگیری وضعیت کارت سوخت هوشمند
        </div>
    </center>
    <br />
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 110px;">
                <tr>
                    <td class="fieldName-large">
                        PAN کارت سوخت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtPAN"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="margin-right: 110px;">
                <tr>
                    <td class="fieldName-large">
                    </td>
                    <td align="left">
                        <asp:Button ID="btnCheck" runat="server" CssClass="button" Text="ادامه" OnClick="btnCheck_Click" />
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="./App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <br />
            <div id="lblMessage" runat="server" style="background-color: #ffffbb; border: solid 1px #fce4da;
                color: #e9846c; font: bold 13px tahoma; text-align: center; margin: 0px auto;
                padding: 5px; width: 90%;">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
