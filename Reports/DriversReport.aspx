<%@ Page Title="گزارش آمار رانندگان" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="DriversReport.aspx.cs" Inherits="Reports_DriversReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            گزارش آمار رانندگان آژانس ها
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 90px;">
                <tr>
                    <td class="fieldName-large">استان :
                    </td>
                    <td>
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
                    <td class="fieldName-large">شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="drpCity_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">رسته :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancyType" runat="server" CssClass="dropdown-middle" AutoPostBack="True"
                            OnSelectedIndexChanged="drpAjancyType_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="آژانس تاکسی تلفنی"></asp:ListItem>
                            <asp:ListItem Value="1" Text="آژانس موتور تلفنی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName-large">آژانس :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancies" runat="server" CssClass="dropdown-middle" DataValueField="AjancyID"
                            DataTextField="AjancyName">
                            <asp:ListItem Text="- همه آژانس ها -"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">صلاحیت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpDriverCertification" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Text="رانندگان دارای دفترچه صلاحیت" />
                            <asp:ListItem Text="رانندگان بدون دفترچه صلاحیت" />
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName-large">وضعيت تاهل :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMarriage" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="2" Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="مجرد"></asp:ListItem>
                            <asp:ListItem Value="1" Text="متاهل"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">گواهینامه :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpDrivingLicenseType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="4" Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="ب 1"></asp:ListItem>
                            <asp:ListItem Value="1" Text="ب 2"></asp:ListItem>
                            <asp:ListItem Value="2" Text="پایه یکم"></asp:ListItem>
                            <asp:ListItem Value="3" Text="پایه دوم"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName-large">خودرو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarType" runat="server" CssClass="dropdown-middle" DataValueField="CarTypeID"
                            DataTextField="TypeName">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFuelType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="4" Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                            <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                            <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                            <asp:ListItem Value="3" Text="CNG"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName-large">کارت سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFuelCardType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="3" Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="آژانس"></asp:ListItem>
                            <asp:ListItem Value="1" Text="شخصی"></asp:ListItem>
                            <asp:ListItem Value="2" Text="خطی و بین راهی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شماره موتور :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarEngineNo" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>

                    </td>

                    <td class="fieldName-large">شماره شاسی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarChassisNo" runat="server" MaxLength="18" SkinID="TextBoxMedium"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">PAN :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                    </td>
                    <td class="fieldName-large">VIN :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">جنسیت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpGender" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="2" Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="مرد"></asp:ListItem>
                            <asp:ListItem Value="1" Text="زن"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName-large">پلاک خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarPlateNumber_3" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            Width="30px"></asp:TextBox><span style="color: #019901; font-size: 14px;">ایران</span>
                        <asp:TextBox ID="txtCarPlateNumber_2" runat="server" MaxLength="3" Width="50px" SkinID="TextBoxMedium"></asp:TextBox>
                        <asp:DropDownList ID="drpCarPlateNumber" runat="server" CssClass="dropdown" Width="50px">
                            <asp:ListItem Value="A" Text="الف"></asp:ListItem>
                            <asp:ListItem Value="B" Text="ب"></asp:ListItem>
                            <asp:ListItem Value="C" Text="پ"></asp:ListItem>
                            <asp:ListItem Value="D" Text="ت"></asp:ListItem>
                            <asp:ListItem Value="E" Text="ث"></asp:ListItem>
                            <asp:ListItem Value="F" Text="ج"></asp:ListItem>
                            <asp:ListItem Value="G" Text="چ"></asp:ListItem>
                            <asp:ListItem Value="H" Text="ح"></asp:ListItem>
                            <asp:ListItem Value="I" Text="خ"></asp:ListItem>
                            <asp:ListItem Value="J" Text="د"></asp:ListItem>
                            <asp:ListItem Value="K" Text="ذ"></asp:ListItem>
                            <asp:ListItem Value="L" Text="ر"></asp:ListItem>
                            <asp:ListItem Value="M" Text="ز"></asp:ListItem>
                            <asp:ListItem Value="N" Text="ژ"></asp:ListItem>
                            <asp:ListItem Value="O" Text="س"></asp:ListItem>
                            <asp:ListItem Value="P" Text="ش"></asp:ListItem>
                            <asp:ListItem Value="Q" Text="ص"></asp:ListItem>
                            <asp:ListItem Value="R" Text="ض"></asp:ListItem>
                            <asp:ListItem Value="S" Text="ط"></asp:ListItem>
                            <asp:ListItem Value="T" Text="ظ"></asp:ListItem>
                            <asp:ListItem Value="U" Text="ع"></asp:ListItem>
                            <asp:ListItem Value="V" Text="غ"></asp:ListItem>
                            <asp:ListItem Value="W" Text="ف"></asp:ListItem>
                            <asp:ListItem Value="X" Text="ق"></asp:ListItem>
                            <asp:ListItem Value="Y" Text="ک"></asp:ListItem>
                            <asp:ListItem Value="Z" Text="گ"></asp:ListItem>
                            <asp:ListItem Value="1" Text="ل"></asp:ListItem>
                            <asp:ListItem Value="2" Text="م"></asp:ListItem>
                            <asp:ListItem Value="3" Text="ن"></asp:ListItem>
                            <asp:ListItem Value="4" Text="و"></asp:ListItem>
                            <asp:ListItem Value="5" Text="ه"></asp:ListItem>
                            <asp:ListItem Value="6" Text="ی"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtCarPlateNumber_1" runat="server" SkinID="TextBoxMedium" MaxLength="2"
                            Width="30px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                    </td>
                    <td class="fieldName-large">نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                    <td class="fieldName-large">شماره شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateNo" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
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
            <table style="margin-right: 90px;">
                <tr>
                    <td class="fieldName-large">پلاک خودرو(منطقه آزاد) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarPlateNumber_5" runat="server" SkinID="TextBoxMedium" MaxLength="5"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarPlateNumberCity" runat="server" CssClass="dropdown" DataTextField="Name"
                            DataValueField="CityID">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarPlateNumberProvince" runat="server" CssClass="dropdown" AutoPostBack="true"
                            OnSelectedIndexChanged="drpCarPlateNumberProvince_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="- استان -" />
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
                            <asp:ListItem Value="71" Text="فارس" />
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
                    <td class="fieldName-large">آخرین وضعیت راننده :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Text="رانندگان فعال" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="رانندگان غیر فعال"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnExcel" runat="server" CssClass="button" Text="Excel" OnClick="btnExcel_Click" />
                    </td>
                    <td align="right" colspan="2">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="../App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>

            <hr />
            <div style="margin: 10px 10px 5px 0px;">
                <asp:ListView ID="lstDrivers" runat="server" EnableViewState="false" DataSourceID="ObjectDataSource1">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 30px;"></th>
                                <th style="width: 70px;">کد ملی
                                </th>
                                <th>نام
                                </th>
                                <th>نام خانوادگی
                                </th>
                                <th>شماره صلاحیت
                                </th>
                                <th>نام آژانس
                                </th>
                                <th>شهر
                                </th>
                                <th style="width: 70px;">تاریخ ثبت
                                </th>
                                <th style="width: 40px;"></th>
                                <th style="width: 40px;"></th>
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
                                <td style="width: 70px;">
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td>
                                    <%#Eval("DriverCertificationNo") == null ? "---" : Eval("DriverCertificationNo")%>
                                </td>
                                <td>
                                    <%#Eval("AjancyName")%>
                                </td>
                                <td>
                                    <%#Eval("City")%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td style="width: 40px;">
                                    <a class="info" href='<%#string.Format("../{0}/Driver.aspx?id={1}", ((short?) Eval("ZCityID")).HasValue?"Zone":"Ajancy", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank" style="visibility: <%#IsVisitor?"hidden":"visible"%>">جزییات</a>
                                </td>
                                <td style="width: 40px;">
                                    <img src="../App_Themes/Default/images/delete.png" style="cursor: pointer; visibility: <%#IsVisitor?"hidden":"visible"%>" onclick="javascript:deleteDriver(this, <%#Eval("PersonID")%>)"
                                        title="حذف" alt="حذف" />
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
                                <td style="width: 70px;">
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td>
                                    <%#Eval("DriverCertificationNo") == null ? "---" : Eval("DriverCertificationNo")%>
                                </td>
                                <td>
                                    <%#Eval("AjancyName")%>
                                </td>
                                <td>
                                    <%#Eval("City")%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td style="width: 40px;">
                                    <a class="info" href='<%#string.Format("../{0}/Driver.aspx?id={1}", ((short?) Eval("ZCityID")).HasValue?"Zone":"Ajancy", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank" style="visibility: <%#IsVisitor?"hidden":"visible"%>">جزییات</a>
                                </td>
                                <td style="width: 40px;">
                                    <img src="../App_Themes/Default/images/delete.png" style="cursor: pointer; visibility: <%#IsVisitor?"hidden":"visible"%>" onclick="javascript:deleteDriver(this, <%#Eval("PersonID")%>)"
                                        title="حذف" alt="حذف" />
                                </td>
                            </tr>
                        </table>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <h1>آیتمی یافت نشد</h1>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
            <div id="pager">
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lstDrivers" PageSize="20">
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
                SelectCountMethod="GetDriversCount" SelectMethod="LoadDrivers" TypeName="Paging"
                EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="ProvinceId" Type="Byte" />
                    <asp:QueryStringParameter Name="CityId" Type="Int16" />
                    <asp:QueryStringParameter Name="AjancyType" Type="Byte" />
                    <asp:QueryStringParameter Name="AjancyId" Type="Int32" />
                    <asp:QueryStringParameter Name="DriverCertification" Type="Int32" />
                    <asp:QueryStringParameter Name="DrivingLicenseType" Type="Byte" />
                    <asp:QueryStringParameter Name="Marriage" Type="Byte" />
                    <asp:QueryStringParameter Name="CarType" Type="Int16" />
                    <asp:QueryStringParameter Name="FuelType" Type="Byte" />
                    <asp:QueryStringParameter Name="FuelCardType" Type="Byte" />
                    <asp:QueryStringParameter Name="CarEngineNo" Type="String" />
                    <asp:QueryStringParameter Name="CarChassisNo" Type="String" />
                    <asp:QueryStringParameter Name="PAN" Type="String" />
                    <asp:QueryStringParameter Name="VIN" Type="String" />
                    <asp:QueryStringParameter Name="Gender" Type="Byte" />
                    <asp:QueryStringParameter Name="FirstName" Type="String" />
                    <asp:QueryStringParameter Name="LastName" Type="String" />
                    <asp:QueryStringParameter Name="NationalCode" Type="String" />
                    <asp:QueryStringParameter Name="BirthCertificateNo" Type="String" />
                    <asp:QueryStringParameter Name="ZCityId" Type="Int16" />
                    <asp:QueryStringParameter Name="ZNumber" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_1" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_2" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_3" Type="String" />
                    <asp:QueryStringParameter Name="Alphabet" Type="String" />
                    <asp:QueryStringParameter Name="Status" Type="String" />
                    <asp:QueryStringParameter Name="DateFrom" Type="DateTime" />
                    <asp:QueryStringParameter Name="DateTo" Type="DateTime" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function deleteDriver(item, pId) {
            if (confirm('آیا راننده مورد نظر به همراه کلیه سوابق و اطلاعاتش از سیستم حذف گردد؟')) {
                $.ajax({
                    type: 'GET',
                    data: ({ mode: 'del', pId: pId }),
                    url: '../Reports/DriversReport.aspx',
                    dataType: 'json',
                    cache: false,
                    success: function (result) {
                        if (result == '1') {
                            $(item).parent().parent().parent().remove();
                        }
                    }
                });
            }
        }

    </script>
</asp:Content>
