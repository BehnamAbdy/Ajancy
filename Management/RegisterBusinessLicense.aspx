<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="RegisterBusinessLicense.aspx.cs" Inherits="Management_RegisterBusinessLicense" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            فرم صدور پروانه کسب آژانس
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 10px;">
                <tr>
                    <td colspan="4" class="section-title">
                        استعلام دارایی
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره نامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDaraeeNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDaraeeNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDaraeeDate" runat="server" />
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
                        استعلام شهرداری
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره نامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtShhrdariNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtShhrdariNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtShhrdariDate" runat="server" />
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtAmakenNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtAmakenDate" runat="server" />
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtAjancyName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtAngoshNegariDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        توضیحات
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComment_3" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="200" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        گواهینامه آموزش اصناف
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره نامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAsnafNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtAsnafNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtAsnafDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        توضیحات
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComment_4" runat="server" Height="40px" Width="350px" TextMode="MultiLine"
                            MaxLength="200" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        شماره و مبلغ فیش های پرداختی
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        خزانه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNoKhazane" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtBillNoKhazane"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillKhazaneAmount" runat="server" SkinID="TextBox" MaxLength="6"
                            onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtBillKhazaneAmount"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        اتحادیه کشوری :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNoEtehadieh" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtBillNoEtehadieh"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillEtehadiehAmount" runat="server" SkinID="TextBox" MaxLength="6"
                            onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtBillEtehadiehAmount"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شورای اصناف کشوری :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNoAsnaf" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtBillNoAsnaf"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillAsnafAmount" runat="server" SkinID="TextBox" MaxLength="6"
                            onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtBillAsnafAmount"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        حق عضویت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNoOzviat" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtBillNoOzviat"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillOzviatAmount" runat="server" SkinID="TextBox" MaxLength="6"
                            onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtBillOzviatAmount"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        ورودی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNoVorodi" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtBillNoVorodi"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillVorodiAmount" runat="server" SkinID="TextBox" MaxLength="6"
                            onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtBillVorodiAmount"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        اهدایی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNoEhdaee" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtBillNoEhdaee"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="fieldName">
                        مبلغ :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillEhdaeeAmount" runat="server" SkinID="TextBox" MaxLength="6"
                            onkeypress="javascript:return isNumberKey(event)" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtBillEhdaeeAmount"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title" style="font-weight: bold; color: #de3e27;">
                        صدور پروانه کسب
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام آژانس :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAjancyName" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAjancyName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره پروانه کسب :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtBusinessLicenseNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBusinessLicenseNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره عضویت :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtMemberShipCode" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMemberShipCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره بارکد ملی :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNationalCardBarCode" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNationalCardBarCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره مسلسل :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtSerialNo" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSerialNo"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شماره رسته :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCategoryCode" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCategoryCode"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        ISIC :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtISIC" runat="server" SkinID="TextBox" MaxLength="15" />
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtISIC"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تاریخ صدور :
                    </td>
                    <td colspan="3">
                        <userControl:Date ID="txtStartDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        تاریخ انقضا :
                    </td>
                    <td colspan="3">
                        <userControl:Date ID="txtEndDate" runat="server" Required="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="section-title">
                        <asp:CheckBox ID="chkVerification" runat="server" ForeColor="#de3e27" Font-Bold="true"
                            Text="تایید نهایی مدارک پرونده و صدور   پروانه کسب آژانس" />
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
