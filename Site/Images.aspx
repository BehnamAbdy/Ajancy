<%@ Page Title="ویرایش متن بخشنامه های سازمان" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Images.aspx.cs" Inherits="Site_Images" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ویرایش متن بخشنامه های سازمان
    </div>
    <table style="margin: 10px 120px 10px 0;">
        <tr>
            <td class="fieldName">
            </td>
            <td>
                <asp:DropDownList ID="drpLetters" runat="server" CssClass="dropdown-middle" DataTextField="Title"
                    DataValueField="Id" OnSelectedIndexChanged="drpLetters_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName">
                عنوان :
            </td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="150px" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="color: #df2328;">
                تنها عکس های با فرمت jpg را بارگذاری نمایید
            </td>
        </tr>
        <tr>
            <td class="fieldName">
            </td>
            <td>
                <asp:FileUpload ID="fluLetter" runat="server" Width="200px" />
            </td>
        </tr>
        <tr>
            <td class="fieldName">
            </td>
            <td>
                <asp:CheckBox ID="chkIsActive" Text="فعال" runat="server" CssClass="checkBox" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Image ID="imgLetter" runat="server" Style="border: solid 1px #c8c8c8; padding: 3px;
                    height: 300px; width: 300px;" />
            </td>
        </tr>
    </table>
    <div class="pane-left" style="clear: both; margin-top: 15px;">
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ذخیره" OnClick="btnSave_Click" />
        <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
    </div>
</asp:Content>
