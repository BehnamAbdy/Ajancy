<%@ Page Title="شکایات مردمی" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Comment.aspx.cs" Inherits="Comment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <br />
    <center>
        <div class="title-bar">
            فرم شکایات مردمی
        </div>
    </center>
    <br />
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="fieldName">
                        نام شما :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        نام خانوادگی شما :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName"
                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        تلفن شما :
                    </td>
                    <td>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" runat="server"
                            FilterType="Numbers" TargetControlID="txtPhone" />
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="14" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                            ControlToValidate="txtPhone"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        ايميل شما :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" SkinID="TextBoxMedium"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="آدرس ايميل را صحيح وارد كنيد." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        رسته آژانس :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancyType" runat="server" CssClass="dropdown-middle" AutoPostBack="True"
                            OnSelectedIndexChanged="drpAjancyType_SelectedIndexChanged">
                            <asp:ListItem Value="-1" Text="- انتخاب کنید -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="آژانس تاکسی تلفنی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="آژانس موتور تلفنی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        آژانس مورد نظر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancies" runat="server" CssClass="dropdown-middle" DataValueField="AjancyID"
                            DataTextField="AjancyName">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="drpAjancies"
                            ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        نام راننده :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDriverFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        نام خانوادگی راننده :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDriverLastName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        شماره پلاک خودرو :
                    </td>
                    <td>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                            FilterType="Numbers" TargetControlID="txtCarPlateNumber_3" />
                        <asp:TextBox ID="txtCarPlateNumber_3" runat="server" SkinID="TextBox" MaxLength="2"
                            Width="30px"></asp:TextBox>
                        <span style="color: #019901; font-size: 14px;">ایران</span>
                        <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" Width="50px" SkinID="TextBoxMedium"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server"
                            FilterType="Numbers" TargetControlID="txtCarPlateNumber_2" />
                        <asp:DropDownList ID="drpCarPlateNumber" runat="server" CssClass="dropdown" Width="50px">
                            <asp:ListItem Value="0" Text="الف"></asp:ListItem>
                            <asp:ListItem Value="1" Text="ب"></asp:ListItem>
                            <asp:ListItem Value="2" Text="پ"></asp:ListItem>
                            <asp:ListItem Value="3" Text="ت"></asp:ListItem>
                            <asp:ListItem Value="4" Text="ث"></asp:ListItem>
                            <asp:ListItem Value="5" Text="ج"></asp:ListItem>
                            <asp:ListItem Value="6" Text="چ"></asp:ListItem>
                            <asp:ListItem Value="7" Text="ح"></asp:ListItem>
                            <asp:ListItem Value="8" Text="خ"></asp:ListItem>
                            <asp:ListItem Value="9" Text="د"></asp:ListItem>
                            <asp:ListItem Value="10" Text="ذ"></asp:ListItem>
                            <asp:ListItem Value="11" Text="ر"></asp:ListItem>
                            <asp:ListItem Value="12" Text="ز"></asp:ListItem>
                            <asp:ListItem Value="13" Text="ژ"></asp:ListItem>
                            <asp:ListItem Value="14" Text="س"></asp:ListItem>
                            <asp:ListItem Value="15" Text="ش"></asp:ListItem>
                            <asp:ListItem Value="16" Text="ص"></asp:ListItem>
                            <asp:ListItem Value="17" Text="ض"></asp:ListItem>
                            <asp:ListItem Value="18" Text="ط"></asp:ListItem>
                            <asp:ListItem Value="19" Text="ظ"></asp:ListItem>
                            <asp:ListItem Value="20" Text="ع"></asp:ListItem>
                            <asp:ListItem Value="21" Text="غ"></asp:ListItem>
                            <asp:ListItem Value="22" Text="ف"></asp:ListItem>
                            <asp:ListItem Value="23" Text="ق"></asp:ListItem>
                            <asp:ListItem Value="24" Text="ک"></asp:ListItem>
                            <asp:ListItem Value="25" Text="گ"></asp:ListItem>
                            <asp:ListItem Value="26" Text="ل"></asp:ListItem>
                            <asp:ListItem Value="27" Text="م"></asp:ListItem>
                            <asp:ListItem Value="28" Text="ن"></asp:ListItem>
                            <asp:ListItem Value="29" Text="و"></asp:ListItem>
                            <asp:ListItem Value="30" Text="ه"></asp:ListItem>
                            <asp:ListItem Value="31" Text="ی"></asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server"
                            FilterType="Numbers" TargetControlID="txtCarPlateNumber_1" />
                        <asp:TextBox ID="txtCarPlateNumber_1" runat="server" SkinID="TextBox" MaxLength="2"
                            Width="30px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        موضوع شکایت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpSubject" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="گرفتن کرایه زیاد"></asp:ListItem>
                            <asp:ListItem Value="1" Text="مشاجره راننده با مسافر"></asp:ListItem>
                            <asp:ListItem Value="2" Text="مشکل اخلاقی راننده"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        تاریخ بروز نارضایتی :
                    </td>
                    <td>
                        <userControl:Date ID="txtProblemDate" runat="server" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        ساعت بروز نارضایتی :
                    </td>
                    <td>
                        <userControl:Time ID="txtTime" runat="server" Required="true" Width="50" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        توضیحات :
                    </td>
                    <td>
                        <asp:TextBox ID="txtComment" runat="server" Height="70px" Width="350px" TextMode="MultiLine"
                            MaxLength="300" SkinID="TextBoxMedium"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="pane-left">
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
    </div>
    <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
    <br />
</asp:Content>
