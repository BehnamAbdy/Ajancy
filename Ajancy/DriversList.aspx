<%@ Page Title="لیست رانندگان آژانس" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="DriversList.aspx.cs" Inherits="Ajancy_DriversList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            لیست رانندگان آژانس
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 55px;">
                <tr>
                    <td class="fieldName">
                        صلاحیت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpDriverCertification" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Text="رانندگان دارای دفترچه صلاحیت" />
                            <asp:ListItem Text="رانندگان بدون دفترچه صلاحیت" />
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName">
                        خودرو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarType" runat="server" DataTextField="TypeName" DataValueField="CarTypeID"
                            CssClass="dropdown-middle">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        نوع سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFuelType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                            <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                            <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName">
                        کارت سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFuelCardType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="- همه موارد -"></asp:ListItem>
                            <asp:ListItem Value="0" Text="آژانس"></asp:ListItem>
                            <asp:ListItem Value="1" Text="شخصی"></asp:ListItem>
                            <asp:ListItem Value="2" Text="خطی و بین راهی"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        نام :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" SkinID="TextBoxMedium" MaxLength="25"></asp:TextBox>
                    </td>
                    <td class="fieldName">
                        نام خانوادگی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" SkinID="TextBoxMedium" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        پلاک خودرو :
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
                </tr>
            </table>
            <hr />
            <br />
            <div style="margin-right: 3px;">
                <asp:ListView ID="lstDrivers" runat="server" EnableViewState="false" DataSourceID="ObjectDataSource1">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 30px;">
                                </th>
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
                                    نوع خودرو
                                </th>
                                <%--<th>
                                پلاک خودرو
                            </th>--%>
                                <th>
                                    نوع سوخت
                                </th>
                                <th>
                                    نوع کارت سوخت
                                </th>
                                <th>
                                    شماره صلاحیت
                                </th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                        <div id="pager">
                            <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lstDrivers" PageSize="10">
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
                                    <%#Eval("TypeName")%>
                                </td>
                                <%--<td style="direction: ltr;">
                                <%#Public.GetPlateNumber(Eval("TwoDigits").ToString(), Eval("Alphabet").ToString(), Eval("ThreeDigits").ToString(), Eval("RegionIdentifier").ToString())%>
                            </td>--%>
                                <td>
                                    <%#Public.GetFuelTypeName((Public.FuelType)Public.ToByte(Eval("FuelType")))%>
                                </td>
                                <td>
                                    <%#Public.GetFuelCardTypeName((Public.FuelCardType)Public.ToByte(Eval("CardType")))%>
                                </td>
                                <td>
                                    <%#Eval("DriverCertificationNo") == null ? "ندارد" : Eval("DriverCertificationNo")%>
                                </td>
                            </tr>
                            <tr style="border-top: solid 1px #eeede5;">
                                <td colspan="8" style="height: 20px; padding-left: 3px; text-align: left; width: 800px;">
                                    <%#Eval("Utilities")%>
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
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td>
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("TypeName")%>
                                </td>
                                <%--<td style="direction: ltr;">
                                <%#Public.GetPlateNumber(Eval("TwoDigits").ToString(), Eval("Alphabet").ToString(), Eval("ThreeDigits").ToString(), Eval("RegionIdentifier").ToString())%>
                            </td>--%>
                                <td>
                                    <%#Public.GetFuelTypeName((Public.FuelType)Public.ToByte(Eval("FuelType")))%>
                                </td>
                                <td>
                                    <%#Public.GetFuelCardTypeName((Public.FuelCardType)Public.ToByte(Eval("CardType")))%>
                                </td>
                                <td>
                                    <%#Eval("DriverCertificationNo") == null ? "ندارد" : Eval("DriverCertificationNo")%>
                                </td>
                            </tr>
                            <tr style="border-top: solid 1px #eeede5;">
                                <td colspan="8" style="height: 20px; padding-left: 3px; text-align: left; width: 800px;">
                                    <%#Eval("Utilities")%>
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
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OnSelecting="ObjectDataSource1_Selecting"
                SelectCountMethod="GetAjancyDriversCount" SelectMethod="LoadAjancyDrivers" TypeName="Paging"
                EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="AjancyId" Type="Int32" />
                    <asp:QueryStringParameter Name="DriverCertification" Type="Int32" />
                    <asp:QueryStringParameter Name="CarType" Type="Byte" />
                    <asp:QueryStringParameter Name="FuelType" Type="Byte" />
                    <asp:QueryStringParameter Name="FuelCardType" Type="Byte" />
                    <asp:QueryStringParameter Name="FirstName" Type="String" />
                    <asp:QueryStringParameter Name="LastName" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_1" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_2" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_3" Type="String" />
                    <asp:QueryStringParameter Name="Alphabet" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="dialog" style="height: 164px; width: 340px;">
        <table class="exit-bar">
            <tr>
                <td style="width: 90%;">
                </td>
                <td style="text-align: left; width: 10%;">
                    <img src="../App_Themes/Default/images/close.gif" title="خروج" style="cursor: pointer;"
                        onclick="javascript:hideDialog()" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2">
                    <textarea id="txtComment" rows="2" cols="20" class="textbox" style="height: 80px;
                        width: 315px;"></textarea>
                    <span class="star">*</span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="padding-left: 14px;">
                    <div id="send" onclick="javascript:send()" style="float: left;">
                        ثبت</div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#content-wide').css('min-height', '400px');
        });

        function hideDialog() {
            $('.dialog .exit-bar td:first').text('');
            $('.dialog textarea#txtComment').val('');
            $('.dialog .star').css('color', '#a7a7a7');
            $('.dialog').removeData('id');
            $('.dialog').hide(300);
        }

        function showDialog(mode, element, ajancyDriverId) {
            var aryPosition = objectPosition(element);
            switch (mode) {
                case 0:
                    $('.dialog .exit-bar td:first').text('شکایت');
                    $('.dialog').data('id', { mode: mode, ajancyDriverId: ajancyDriverId });
                    break;

                case 1:
                    $('.dialog .exit-bar td:first').text('درخواست دوگانه سوز');
                    $('.dialog').data('id', { mode: mode, ajancyDriverId: ajancyDriverId });
                    break;

                case 2:
                    $('.dialog .exit-bar td:first').text('درخواست کارت سوخت آژانسی');
                    $('.dialog').data('id', { mode: mode, ajancyDriverId: ajancyDriverId });
                    break;

                case 3:
                    $('.dialog .exit-bar td:first').text('درخواست بیمه رانندگان');
                    $('.dialog').data('id', { mode: mode, ajancyDriverId: ajancyDriverId });
                    break;

                case 4:
                    $('.dialog .exit-bar td:first').text('درخواست پایان کار راننده');
                    $('.dialog').data('id', { mode: mode, ajancyDriverId: ajancyDriverId });
                    break;
            }

            $('.dialog textarea#txtComment').val('');
            $('.dialog .star').css('color', '#a7a7a7');
            $('.dialog').css({ 'left': aryPosition[0], 'top': aryPosition[1] });
            $('.dialog').show(300);
        }

        function send() {
            var txt = $('.dialog textarea#txtComment');
            var data = $('.dialog').data('id');
            switch (data.mode) {
                case 0: // compaint
                case 4: // end Membership
                    if (isValid(txt)) {
                        $.ajax({
                            type: 'GET',
                            data: ({ mode: data.mode, jdId: data.ajancyDriverId, text: txt.val() }),
                            url: '../Ajancy/DriversList.aspx',
                            dataType: 'json',
                            cache: false,
                            success: function (result) {
                                if (result == '1') {
                                    alert('ثبت درخواست انجام گردید');
                                    $('.dialog').hide(300);
                                }
                            }
                        });
                    }
                    break;

                case 1: // CNG
                case 2: // ajancyCardType
                case 3: // insurance
                    $.ajax({
                        type: 'GET',
                        data: ({ mode: data.mode, jdId: data.ajancyDriverId, text: txt.val() }),
                        url: '../Ajancy/DriversList.aspx',
                        dataType: 'json',
                        cache: false,
                        success: function (result) {
                            if (result == '1') {
                                alert('ثبت درخواست انجام گردید');
                                $('.dialog').hide(300); ;
                            }
                        }
                    });
                    break;
            }
        }

        function isValid(txt) {
            var isValid = true;
            if (txt.val() == '') {
                $('.dialog .star').css('color', '#fa4141');
                isValid = false;
            }
            else {
                $('.dialog .star').css('color', '#a7a7a7');
            }

            if (!isValid || $('.dialog').data('id') == '') {
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
