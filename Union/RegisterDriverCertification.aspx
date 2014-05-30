<%@ Page Title="فرم صدور دفترچه صلاحیت" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="RegisterDriverCertification.aspx.cs" Inherits="Union_RegisterDriverCertification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            فرم صدور دفترچه صلاحیت
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 10px;">
                <tr>
                    <td class="fieldName">
                        نام راننده
                    </td>
                    <td>
                        <asp:Label ID="lblDriverName" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        نام پدر
                    </td>
                    <td>
                        <asp:Label ID="lblFather" runat="server" ForeColor="#1c5eb7" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        کد ملی :
                    </td>
                    <td>
                        <asp:Label ID="lblNationalCode" runat="server" ForeColor="#1c5eb7" />
                    </td>
            </table>
            <table style="margin-right: 10px;">
                <tr>
                    <td colspan="4" class="section-title">
                        استعلام انگشت نگاری
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره نامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAngoshNegariNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtAngoshNegariDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        توضیحات
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComment_0" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="200" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        گواهی عدم اعتیاد
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره نامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEtiadNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtEtiadDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        توضیحات
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComment_1" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="200" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        استعلام اماکن
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره نامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAmakenNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtAmakenDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        توضیحات
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComment_2" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="200" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        فیش های پرداختی
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره فیش بانکی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillAmount" runat="server" SkinID="TextBox" MaxLength="6" onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        دفترچه صلاحیت
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره دفترچه صلاحیت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDriverCertificationNo" runat="server" SkinID="TextBox" MaxLength="20" />
                        <span class="star">*</span>
                    </td>
                    <td class="fieldName">
                        کد راننده :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDriverCode" runat="server" SkinID="TextBox" MaxLength="5" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تاریخ صدور :
                    </td>
                    <td>
                        <userControl:Date ID="txtStartDate" runat="server" Required="false" />
                    </td>
                    <td class="fieldName">
                        تاریخ انقضا :
                    </td>
                    <td>
                        <userControl:Date ID="txtEndDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        <asp:CheckBox ID="chkVerification" runat="server" ForeColor="#de3e27" Font-Bold="true"
                            Text="تایید نهایی مدارک پرونده و صدور  دفترچه صلاحیت" />
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
