<%@ Page Title="" Language="C#" MasterPageFile="~/SiteWide.master" AutoEventWireup="true"
    CodeFile="FCReplacementsRep.aspx.cs" Inherits="Reports_FCReplacementsRep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            گزارش ابطال و جایگزین کارت سوخت
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin: 0px auto;">
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
                            DataValueField="CityID" Enabled="false">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
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
                    <td class="fieldName-large">PAN :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">VIN :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">کدملی :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            CssClass="validator" ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع جستجو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="-- انتخاب کنید --" />
                            <asp:ListItem Text="مالک کارت سوخت ابطالی" />
                            <asp:ListItem Text="مالک کارت سوخت جایگزین" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="../App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
            <div style="margin: 10px 5px 5px 0px;">
                <asp:ListView ID="lstFCDiscards" runat="server" ItemPlaceholderID="itemPlaceHolder">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 30px;"></th>
                                <th style="font-size: 12px; width: 33px;"></th>
                                <th style="width: 110px;">مالک خودرو
                                </th>
                                <th style="width: 110px;">راننده خودرو
                                </th>
                                <th style="width: 105px;">PAN
                                </th>
                                <th style="width: 110px;">VIN
                                </th>
                                <th>پلاک خودرو
                                </th>
                                <th style="width: 110px;">نام آژانس
                                </th>
                                <th style="width: 70px;">شهر
                                </th>
                                <th style="width: 30px;"></th>
                                <th style="width: 30px;"></th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                        <div id="pager">
                            <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lstFCDiscards" PageSize="15">
                                <Fields>
                                    <asp:NextPreviousPagerField FirstPageText="<< " ShowFirstPageButton="True" ShowNextPageButton="False"
                                        PreviousPageText="<" />
                                    <asp:NumericPagerField />
                                    <asp:NextPreviousPagerField LastPageText=" >>" ShowLastPageButton="True" ShowPreviousPageButton="False"
                                        NextPageText=">" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <table class="list-item-even">
                            <tr>
                                <td style="width: 30px;" rowspan="2">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td style="color: #ee7339; font-size: 9px; width: 30px;">آژانسی
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Owner1")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Driver1")%>
                                </td>
                                <td style="width: 105px;">
                                    <%#Eval("PAN1")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("VIN1")%>
                                </td>
                                <td>
                                    <%#Convert.ToBoolean(Eval("IsZoneType1"))?string.Format("{0} {1}",Eval("ZNumber1"),Eval("ZCity1")): Public.PlateNumberRenderToHTML(Eval("TwoDigits1").ToString(), Eval("Alphabet1").ToString(), Eval("ThreeDigits1").ToString(), Eval("RegionIdentifier1").ToString())%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("AjancyName1")%>
                                </td>
                                <td style="width: 70px;" rowspan="2">
                                    <%#string.Format("{0}<br/>{1}", Eval("City"), Public.ToPersianDate(Eval("SubmitDate")))%>
                                    <%--<%#Public.ToPersianDate(Eval("SubmitDate"))%>--%>
                                </td>
                                <td style="width: 30px;" rowspan="2">
                                    <%#Eval("Edit")%>
                                </td>
                                <td style="width: 30px;" rowspan="2">
                                    <span class="info" style="float: none;" onclick="javascript:deleteReq(this, '<%#string.Format("{0}|{1}|{2}|{3}|{4}",Eval("FuelCardSubstituteID"),Eval("CarID2"),Eval("PlateNumberID2"),Eval("NationalCode1"),Eval("NationalCode2"))%>')">حذف</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #ee7339; font-size: 9px; width: 30px;">شخصی
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Owner2")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Driver2")%>
                                </td>
                                <td style="width: 105px;">
                                    <%#Eval("PAN2")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("VIN2")%>
                                </td>
                                <td>
                                    <%#Convert.ToBoolean(Eval("IsZoneType2"))?string.Format("{0} {1}",Eval("ZNumber2"),Eval("ZCity2")): Public.PlateNumberRenderToHTML(Eval("TwoDigits2").ToString(), Eval("Alphabet2").ToString(), Eval("ThreeDigits2").ToString(), Eval("RegionIdentifier2").ToString())%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("AjancyName2")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <table class="list-item-odd">
                            <tr>
                                <td style="width: 30px;" rowspan="2">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td style="color: #ee7339; font-size: 9px; width: 30px;">آژانسی
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Owner1")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Driver1")%>
                                </td>
                                <td style="width: 105px;">
                                    <%#Eval("PAN1")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("VIN1")%>
                                </td>
                                <td>
                                    <%#Public.PlateNumberRenderToHTML(Eval("TwoDigits1").ToString(), Eval("Alphabet1").ToString(), Eval("ThreeDigits1").ToString(), Eval("RegionIdentifier1").ToString())%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("AjancyName1")%>
                                </td>
                                <td style="width: 70px;" rowspan="2">
                                    <%#string.Format("{0}<br/>{1}", Eval("City"), Public.ToPersianDate(Eval("SubmitDate")))%>
                                    <%--<%#Public.ToPersianDate(Eval("SubmitDate"))%>--%>
                                </td>
                                <td style="width: 30px;" rowspan="2">
                                    <%#Eval("Edit")%>
                                </td>
                                <td style="width: 30px;" rowspan="2">
                                    <span class="info" style="float: none;" onclick="javascript:deleteReq(this, '<%#string.Format("{0}|{1}|{2}|{3}|{4}",Eval("FuelCardSubstituteID"),Eval("CarID2"),Eval("PlateNumberID2"),Eval("NationalCode1"),Eval("NationalCode2"))%>')">حذف</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #ee7339; font-size: 9px; width: 30px;">شخصی
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Owner2")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("Driver2")%>
                                </td>
                                <td style="width: 105px;">
                                    <%#Eval("PAN2")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("VIN2")%>
                                </td>
                                <td>
                                    <%#Public.PlateNumberRenderToHTML(Eval("TwoDigits2").ToString(), Eval("Alphabet2").ToString(), Eval("ThreeDigits2").ToString(), Eval("RegionIdentifier2").ToString())%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("AjancyName2")%>
                                </td>
                            </tr>
                        </table>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <h1>آیتمی یافت نشد</h1>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OnSelecting="ObjectDataSource1_Selecting"
                SelectCountMethod="GetFCReplacementsCount" SelectMethod="LoadFCReplacements"
                TypeName="Paging" EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="DateFrom" Type="DateTime" />
                    <asp:QueryStringParameter Name="DateTo" Type="DateTime" />
                    <asp:QueryStringParameter Name="ProvinceId" Type="Byte" />
                    <asp:QueryStringParameter Name="CityId" Type="Int16" />
                    <asp:QueryStringParameter Name="AjancyType" Type="Byte" />
                    <asp:QueryStringParameter Name="Type" Type="String" />
                    <asp:QueryStringParameter Name="NationalCode" Type="String" />
                    <asp:QueryStringParameter Name="PAN" Type="String" />
                    <asp:QueryStringParameter Name="VIN" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function deleteReq(req, fcs) {
            if (confirm('آیا درخواست مورد نظر حذف گردد؟')) {
                $.ajax({
                    type: 'GET',
                    data: ({ fcs: fcs, type: $get('<%=drpAjancyType.ClientID %>').value }),
                    url: '../Reports/FCReplacementsRep.aspx',
                    dataType: 'json',
                    cache: false,
                    success: function (result) {
                        if (result == '1') {
                            $(req).parent().parent().parent().remove();
                        }
                    }
                });
            }
        }

    </script>
</asp:Content>
