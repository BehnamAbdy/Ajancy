<%@ Page Title="ویرایش متن بخشنامه های سازمان" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" ValidateRequest="false" CodeFile="Letters.aspx.cs" Inherits="Site_Letters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ویرایش متن بخشنامه های سازمان
    </div>
    <table style="margin: 10px 120px 10px 0;">
        <tr>
            <td class="fieldName"></td>
            <td colspan="2">
                <asp:DropDownList ID="drpLetters" runat="server" CssClass="dropdown-middle" DataTextField="Title"
                    DataValueField="Id" OnSelectedIndexChanged="drpLetters_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName">عنوان :
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="240px" />
            </td>
        </tr>
        <tr>
            <td class="fieldName">تاریخ :
            </td>
            <td>
                <userControl:Date ID="txtDate" runat="server" Required="true" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td class="fieldName">قابل دید برای :
            </td>
            <td>
                <asp:RadioButtonList ID="lstScope" runat="server" RepeatDirection="Horizontal" Width="250px">
                    <asp:ListItem Value="0" Text="همگان" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" Text="کاربران"></asp:ListItem>
                    <asp:ListItem Value="2" Text="دیده نشود"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="fieldName">فایل پیوستی :
            </td>
            <td colspan="2">
                <asp:FileUpload ID="fluAttachment" runat="server" Width="220px" />
            </td>
        </tr>
        <tr>
            <td class="fieldName">عکس :
            </td>
            <td colspan="2" style="color: #df2328;">تنها عکس های با فرمت jpeg و سایز 80 × 75 را بارگذاری نمایید
            </td>
        </tr>
        <tr>
            <td class="fieldName"></td>
            <td colspan="2">
                <asp:FileUpload ID="fluLetter" runat="server" Width="220px" />
            </td>
        </tr>
    </table>
    <center>
        <textarea id="txtEditor" class="txtEditor" name="txtEditor" runat="server" cols="60"
            rows="10" style="height: 250px; width: 600px;"></textarea>
    </center>
    <div class="pane-left" style="clear: both; margin-top: 15px;">
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ذخیره" OnClick="btnSave_Click" />
        <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
    </div>
    <%--<script src="../Scripts/javascript.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.cleditor.min.js" type="text/javascript"></script>
    <link href="../Styles/jquery.cleditor.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">

        //$(document).ready(function () {
        //    $('.txtEditor').cleditor();
        //});

    </script>
</asp:Content>
