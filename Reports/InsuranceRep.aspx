<%@ Page Title="گزارش بیمه رانندگان" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="InsuranceRep.aspx.cs" Inherits="Reports_InsuranceRep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            گزارش بیمه رانندگان
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
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
                    <td class="fieldName-large">نوع درخواست :
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="drpCancelInsurance" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="درخواست بیمه"></asp:ListItem>
                            <asp:ListItem Text="درخواست لغو بیمه"></asp:ListItem>
                        </asp:DropDownList>
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
                    <td></td>
                    <td align="left">
                        <asp:Button ID="btnExcel" runat="server" CssClass="button" Text="Excel" OnClick="btnExcel_Click" />
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click" />
                    </td>
                    <td colspan="2" align="right">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="../App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <div style="margin: 10px 10px 5px 0px;">
                <asp:ListView ID="lstFCDiscards" runat="server" ItemPlaceholderID="itemPlaceHolder">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 30px;"></th>
                                <th style="width: 110px;">نام
                                </th>
                                <th style="width: 110px;">نام خانوادگی
                                </th>
                                <th style="width: 70px;">کد ملی
                                </th>
                                <th style="width: 105px;">PAN
                                </th>
                                <th style="width: 110px;">VIN
                                </th>
                                <th>پلاک خودرو
                                </th>
                                <th style="width: 70px;">شهر
                                </th>
                                <th style="width: 60px;">تاریخ</th>
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
                                <td style="width: 30px;">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("FirstName")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("LastName")%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td style="width: 105px;">
                                    <%#Eval("PAN")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("VIN")%>
                                </td>
                                <td>
                                    <%#Public.PlateNumberRenderToHTML(Eval("TwoDigits").ToString(), Eval("Alphabet").ToString(), Eval("ThreeDigits").ToString(), Eval("RegionIdentifier").ToString())%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Eval("City")%>
                                </td>
                                <td style="width: 60px;">
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td style="width: 30px;">
                                    <%#Eval("Edit")%>
                                </td>
                                <td style="width: 30px;">
                                    <span class="info" style="float: none;" onclick="javascript:deleteReq(this, '<%#Eval("DriverCertificationID")%>')">حذف</span>
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
                                <td style="width: 110px;">
                                    <%#Eval("FirstName")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("LastName")%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td style="width: 105px;">
                                    <%#Eval("PAN")%>
                                </td>
                                <td style="width: 110px;">
                                    <%#Eval("VIN")%>
                                </td>
                                <td>
                                    <%#Public.PlateNumberRenderToHTML(Eval("TwoDigits").ToString(), Eval("Alphabet").ToString(), Eval("ThreeDigits").ToString(), Eval("RegionIdentifier").ToString())%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Eval("City")%>
                                </td>
                                <td style="width: 60px;">
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td style="width: 30px;">
                                    <%#Eval("Edit")%>
                                </td>
                                <td style="width: 30px;">
                                    <span class="info" style="float: none;" onclick="javascript:deleteReq(this, '<%#Eval("DriverCertificationID")%>')">حذف</span>
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
                SelectCountMethod="GetInsuranceCount" SelectMethod="LoadInsurances" TypeName="Paging"
                EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="DateFrom" Type="DateTime" />
                    <asp:QueryStringParameter Name="DateTo" Type="DateTime" />
                    <asp:QueryStringParameter Name="ProvinceId" Type="Byte" />
                    <asp:QueryStringParameter Name="CityId" Type="Int16" />
                    <asp:QueryStringParameter Name="NationalCode" Type="String" />
                    <asp:QueryStringParameter Name="CancelInsurance" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function deleteReq(req, dcId) {
            if (confirm('آیا درخواست مورد نظر حذف گردد؟')) {
                $.ajax({
                    type: 'GET',
                    data: ({ dcId: dcId }),
                    url: '../Reports/InsuranceRep.aspx',
                    dataType: 'json',
                    cache: false,
                    success: function (result) {
                        if (result == '1') {
                            $(req).parent().parent().remove();
                        }
                    }
                });
            }
        }

    </script>
</asp:Content>
