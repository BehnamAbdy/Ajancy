<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="FCExcel.aspx.cs" Inherits="Reports_FCExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            گزارش Excel ابطال و جایگزین کارت سوخت
        </div>
    </center>
    <table style="margin: 0px auto;">
        <tr>
            <td class="fieldName-large">استان :
            </td>
            <td colspan="3">
                <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" AutoPostBack="true"
                    OnSelectedIndexChanged="drpProvince_SelectedIndexChanged" Enabled="false">
                    <asp:ListItem Value="0" Text="- همه موارد -" />
                    <asp:ListItem Value="41" Text="آذربايجان شرقي" />
                    <asp:ListItem Value="44" Text="آذربايجان غربي" />
                    <asp:ListItem Value="45" Text="اردبيل" />
                    <asp:ListItem Value="31" Text="اصفهان" />
                    <asp:ListItem Value="88" Text="البرز" />
                    <asp:ListItem Value="84" Text="ايلام" />
                    <asp:ListItem Value="77" Text="بوشهر" />
                    <asp:ListItem Value="21" Text="تهران" />
                    <asp:ListItem Value="38" Text="چهارمحال بختياري" />
                    <asp:ListItem Value="56" Text="خراسان جنوبي" />
                    <asp:ListItem Value="51" Text="خراسان رضوي" />
                    <asp:ListItem Value="58" Text="خراسان شمالي" />
                    <asp:ListItem Value="61" Text="خوزستان" />
                    <asp:ListItem Value="24" Text="زنجان" />
                    <asp:ListItem Value="23" Text="سمنان" />
                    <asp:ListItem Value="54" Text="سيستان و بلوچستان" />
                    <asp:ListItem Value="71" Text="فارس" Selected="True" />
                    <asp:ListItem Value="28" Text="قزوين" />
                    <asp:ListItem Value="25" Text="قم" />
                    <asp:ListItem Value="87" Text="كردستان" />
                    <asp:ListItem Value="34" Text="كرمان" />
                    <asp:ListItem Value="83" Text="كرمانشاه" />
                    <asp:ListItem Value="74" Text="كهكيلويه و بويراحمد" />
                    <asp:ListItem Value="17" Text="گلستان" />
                    <asp:ListItem Value="13" Text="گيلان" />
                    <asp:ListItem Value="66" Text="لرستان" />
                    <asp:ListItem Value="15" Text="مازندران" />
                    <asp:ListItem Value="86" Text="مركزي" />
                    <asp:ListItem Value="76" Text="هرمزگان" />
                    <asp:ListItem Value="81" Text="همدان" />
                    <asp:ListItem Value="35" Text="يزد" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">شهر :
            </td>
            <td colspan="3">
                <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                    DataValueField="CityID" Enabled="false">
                    <asp:ListItem Value="0" Text="- انتخاب کنید -" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">نوع :
            </td>
            <td colspan="3">
                <asp:DropDownList ID="drpAjancyType" runat="server" CssClass="dropdown-middle" Enabled="false">
                    <asp:ListItem Value="0" Text="آژانس تاکسی تلفنی"></asp:ListItem>
                    <asp:ListItem Value="1" Text="آژانس موتور تلفنی"></asp:ListItem>
                    <asp:ListItem Value="2" Text="آموزشگاه رانندگی"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">گزارش از :
            </td>
            <td colspan="3">
                <asp:DropDownList ID="drpReportType" runat="server" CssClass="dropdown-middle" Width="240px">
                    <asp:ListItem Text="کارت سوخت های ابطالی بدون جایگزین" />
                    <asp:ListItem Text="کارت سوخت های ابطال و جایگزین" />
                    <asp:ListItem Text="کارت سوخت مسدود شده" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="fieldName-large">از تاریخ :
            </td>
            <td>
                <userControl:Date ID="txtDateFrom" runat="server" Required="false" />
            </td>
            <td class="fieldName">تا تاریخ :
            </td>
            <td>
                <userControl:Date ID="txtDateTo" runat="server" Required="false" />
            </td>
        </tr>
    </table>
    <div class="pane-left">
        <asp:Button ID="btnReport" runat="server" Text="Excel" CssClass="button" OnClick="btnReport_Click" />
        <asp:Button ID="btnZoneReport" runat="server" Text="پلاک منطقه آزاد Excel" CssClass="button" OnClick="btnZoneReport_Click" />
    </div>
    <table cellspacing="5" style="margin-right: 70px;">
        <tr>
            <td style="background-color: #ff6347; width: 40px"></td>
            <td>رنگ قرمز نشانگر کارت سوخت های ابطالی
            </td>
        </tr>
        <tr>
            <td style="background-color: #7cfc00; width: 40px"></td>
            <td>رنگ سبز نشانگر کارت سوخت های جایگزینی
            </td>
        </tr>
    </table>
</asp:Content>
