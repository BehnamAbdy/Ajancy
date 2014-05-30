<%@ Page Title="آژانس های دارای پروانه کسب" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="ActiveAjancies.aspx.cs" Inherits="Management_ActiveAjancies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            لیست آژانس های دارای پروانه کسب
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table class="centered">
                <tr>
                    <td class="fieldName-large">
                        استان :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" AutoPostBack="true"
                            OnSelectedIndexChanged="drpProvince_SelectedIndexChanged" Enabled="false">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
                            <asp:ListItem Value="41" Text="آذربايجان شرقي" />
                            <asp:ListItem Value="44" Text="آذربايجان غربي" />
                            <asp:ListItem Value="45" Text="اردبيل" />
                            <asp:ListItem Value="31" Text="اصفهان" />
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
                    <td class="fieldName-large">
                        شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" Enabled="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع آژانس :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancyType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="آژانس تاکسی تلفنی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="آژانس موتور تلفنی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName-large">
                        نام آژانس :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAjancyName" runat="server" SkinID="TextBoxMedium" MaxLength="20" />
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <table>
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click" />
                        </td>
                        <td align="right">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                                <ProgressTemplate>
                                    <img src="../App_Themes/Default/images/ajax-loader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-right: 51px;">
                <asp:ListView ID="lstAjancies" runat="server" ItemPlaceholderID="itemPlaceHolder"
                    DataSourceID="ObjectDataSource1">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 30px;">
                                </th>
                                <th>
                                    نام آژانس
                                </th>
                                <th style="width: 150px;">
                                    مدیر
                                </th>
                                <th>
                                    تاریخ ثبت درسیستم
                                </th>
                                <th>
                                    شماره پروانه کسب
                                </th>
                                <th>
                                    شهر
                                </th>
                                <th>
                                    جزییات
                                </th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <table class="list-item-even">
                            <tr>
                                <td style="width: 30px;">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td>
                                    <%#Eval("AjancyName")%>
                                </td>
                                <td style="width: 150px;">
                                    <%#Eval("Manager")%>
                                </td>
                                <td>
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td>
                                    <%#Eval("BusinessLicenseNo") == null ? "ندارد" : Eval("BusinessLicenseNo")%>
                                </td>
                                <td>
                                    <%#Eval("City")%>
                                </td>
                                <td>
                                    <a class="item-box" style="float: none;" href='<%#string.Format("../Ajancy/BusinessLicense.aspx?j={0}", TamperProofString.QueryStringEncode(Eval("AjancyID").ToString()))%>'
                                        target="_blank">جزییات</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <table class="list-item-odd">
                            <tr>
                                <td style="width: 30px;">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td>
                                    <%#Eval("AjancyName")%>
                                </td>
                                <td style="width: 150px;">
                                    <%#Eval("Manager")%>
                                </td>
                                <td>
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td>
                                    <%#Eval("BusinessLicenseNo") == null ? "ندارد" : Eval("BusinessLicenseNo")%>
                                </td>
                                <td>
                                    <%#Eval("City")%>
                                </td>
                                <td>
                                    <a class="item-box" style="float: none;" href='<%#string.Format("../Ajancy/BusinessLicense.aspx?j={0}", TamperProofString.QueryStringEncode(Eval("AjancyID").ToString()))%>'
                                        target="_blank">جزییات</a>
                                </td>
                            </tr>
                        </table>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <h1>
                            آیتمی یافت نشد</h1>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
            <div id="pager">
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lstAjancies" PageSize="20">
                    <Fields>
                        <asp:NextPreviousPagerField FirstPageText="<< " ShowFirstPageButton="True" ShowNextPageButton="False"
                            PreviousPageText="<" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField LastPageText=" >>" ShowLastPageButton="True" ShowPreviousPageButton="False"
                            NextPageText=">" />
                    </Fields>
                </asp:DataPager>
            </div>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OnSelecting="ObjectDataSource1_Selecting"
                SelectCountMethod="GetAjanciesCount" SelectMethod="LoadAjancies" TypeName="Paging"
                EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="ProvinceId" Type="Byte" />
                    <asp:QueryStringParameter Name="CityId" Type="Int16" />
                    <asp:QueryStringParameter Name="AjancyType" Type="Byte" />
                    <asp:QueryStringParameter Name="AjancyName" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
