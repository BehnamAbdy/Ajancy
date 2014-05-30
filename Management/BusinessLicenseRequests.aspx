<%@ Page Title="افراد متقاضی پروانه کسب آژانس" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="BusinessLicenseRequests.aspx.cs" Inherits="Management_BusinessLicenseRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            لیست افراد متقاضی پروانه کسب آژانس
        </div>
    </center>
    <br />
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 70px;">
                <tr>
                    <td class="fieldName">
                        از تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDateFrom" runat="server" Required="false" />
                    </td>
                    <td colspan="2" class="fieldName">
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        تا تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDateTo" runat="server" Required="false" />
                    </td>
                    <td class="fieldName">
                        رسته :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancyType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="آژانس تاکسی تلفنی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="آژانس موتور تلفنی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td rowspan="2">
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click"
                            CausesValidation="false" />
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <center>
                <asp:ListView ID="lstRequests" runat="server" ItemPlaceholderID="itemPlaceHolder">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th>
                                    نام
                                </th>
                                <th>
                                    نام خانوادگی
                                </th>
                                <th>
                                    کد ملی
                                </th>
                                <th>
                                    جنسیت
                                </th>
                                <th>
                                    تاریخ درخواست
                                </th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                        <div id="pager">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="10">
                                <Fields>
                                    <asp:NumericPagerField RenderNonBreakingSpacesBetweenControls="true" CurrentPageLabelCssClass="CurrentPageButton"
                                        NextPageText="Next 5" NextPreviousButtonCssClass="NextPrevPageButton" NumericButtonCssClass="PageButton"
                                        PreviousPageText="Prev 5" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <table class="list-item-even">
                            <tr>
                                <td>
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td>
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("Gender")%>
                                </td>
                                <td>
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="height: 20px; padding-left: 3px; text-align: left; width: 100%;">
                                    <a class='item-box' target='_blank' href='<%#string.Format("../Requests/BusinessLicense.aspx?j={0}", TamperProofString.QueryStringEncode(Eval("AjancyID").ToString()))%>'>
                                        جزییات درخواست</a> <a class='item-box' target='_blank' href='<%#string.Format("RegisterBusinessLicense.aspx?j={0}", TamperProofString.QueryStringEncode(Eval("AjancyID").ToString()))%>'>
                                            تبدیل به پروانه کسب</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <table class="list-item-odd">
                            <tr>
                                <td>
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td>
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("Gender")%>
                                </td>
                                <td>
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="height: 20px; padding-left: 3px; text-align: left; width: 100%;">
                                    <a class='item-box' target='_blank' href='<%#string.Format("../Requests/BusinessLicense.aspx?j={0}", TamperProofString.QueryStringEncode(Eval("AjancyID").ToString()))%>'>
                                        جزییات درخواست</a> <a class='item-box' target='_blank' href='<%#string.Format("RegisterBusinessLicense.aspx?j={0}", TamperProofString.QueryStringEncode(Eval("AjancyID").ToString()))%>'>
                                            تبدیل به پروانه کسب</a>
                                </td>
                            </tr>
                        </table>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <h1>
                            آیتمی یافت نشد</h1>
                    </EmptyDataTemplate>
                </asp:ListView>
            </center>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OnSelecting="ObjectDataSource1_Selecting"
                SelectCountMethod="GetBusinessLicenseRequestsCount" SelectMethod="LoadBusinessLicenseRequests"
                TypeName="Paging" EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="DateFrom" Type="DateTime" />
                    <asp:QueryStringParameter Name="DateTo" Type="DateTime" />
                    <asp:QueryStringParameter Name="AjancyType" Type="Byte" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
