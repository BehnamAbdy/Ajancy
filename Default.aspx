<%@ Page Title="پارس ناوگان-اتحادیه کشوری آژانسهای تاکسی تلفنی و پیک موتوری" Language="C#"
    MasterPageFile="~/Site.master" EnableViewState="false" %>

<%@ Register Src="UC/SiteLinks.ascx" TagName="SiteLinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div id="date">
        <%=Persia.Calendar.ConvertToPersian(DateTime.Now).Weekday %>
    </div>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="width: 510px;">
                <table class="panel-header" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="right"></td>
                        <td class="body"></td>
                        <td class="left"></td>
                    </tr>
                </table>
                <div class="panel-body">
                    <div id="letters-header">
                    </div>
                    <uc1:SiteLinks ID="SiteLinks1" runat="server" />
                </div>
                <div class="panel-footer">
                </div>
            </td>
            <td align="center" valign="top">
                <a href="http://www.leader.ir/langs/fa/">
                    <img src="App_Themes/Default/images/leader.jpg" class="link" alt="مقام معظم رهبری" /></a>
                <a href="http://dolat.ir/">
                    <img src="App_Themes/Default/images/gov.jpg" class="link" alt="دولت" /></a>
                <a href="http://www.parliran.ir/">
                    <img src="App_Themes/Default/images/parl.jpg" class="link" alt="مجلس" /></a>
                <a href="http://www.moi.ir/">
                    <img src="App_Themes/Default/images/embassy.jpg" class="link" alt="وزارت کشور" /></a>
            </td>
        </tr>
    </table>
    <div class="clear">
    </div>
    <br />
    <hr />
    <table style="margin: 20px auto; width: 650px;">
        <tr>
            <th valign="middle" align="right">
                <a href="Info.aspx">
                    <img src="App_Themes/Default/images/faq.png" alt="پارس ناوگان-اتحادیه کشوری آژانسهای تاکسی تلفنی و پیک موتوری" /></a>
            </th>
        </tr>
    </table>
    <div>
    </div>
</asp:Content>
