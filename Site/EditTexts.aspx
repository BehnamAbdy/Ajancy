<%@ Page Title="ویرایش متن آیین نامه ها" Language="C#" MasterPageFile="~/SiteWide.master"
    ValidateRequest="false" AutoEventWireup="true" CodeFile="EditTexts.aspx.cs" Inherits="Site_EditTexts" %>

<%@ Register Src="../UC/NEditor.ascx" TagName="NEditor" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <br />
    <center>
        <div class="title-bar">
            ویرایش متن آیین نامه های سازمان
        </div>
    </center>
    <br />
    <div style="background-color: #fafafa; border: solid 1px #cfd6e0; direction: rtl;
        float: right; height: 400px; margin: 31px 0px 20px 0px; width: 230px;">
        <asp:RadioButtonList ID="lstAnnouncements" runat="server" RepeatDirection="Vertical"
            DataTextField="Title" DataValueField="Id" AutoPostBack="true" Font-Names="Tahoma"
            OnSelectedIndexChanged="lstAnnouncements_SelectedIndexChanged">
        </asp:RadioButtonList>
    </div>
    <center style="float: left;">
        <uc1:NEditor ID="textEditor" runat="server" />
    </center>
    <div class="pane-left" style="clear: both; margin-top: 15px;">
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ذخیره" OnClick="btnSave_Click" />
        <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
    </div>
</asp:Content>
