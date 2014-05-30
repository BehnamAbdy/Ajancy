<%@ Page Title="ثبت خودرو جدید برای راننده" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="AddCar.aspx.cs" Inherits="Zone_AddCar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ثبت خودرو جدید برای راننده - پلاک منطقه آزاد
    </div>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 20px;">
                <tr>
                    <td class="fieldName-large">کد ملی راننده :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBox" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click"
                            CausesValidation="false" />
                    </td>
                    <td style="width: 18px;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="../App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <table style="margin-right: 20px;">
                <tr>
                    <td class="fieldName-large">نام :
                    </td>
                    <td style="width: 120px;">
                        <asp:Label ID="lblFirstName" runat="server" CssClass="label" />
                    </td>
                    <td class="fieldName-large">نام خانوادگی :
                    </td>
                    <td>
                        <asp:Label ID="lblLastName" runat="server" CssClass="label" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نام پدر :
                    </td>
                    <td style="width: 100px;">
                        <asp:Label ID="lblFather" runat="server" CssClass="label" />
                    </td>
                    <td class="fieldName-large">شماره شناسنامه :
                    </td>
                    <td>
                        <asp:Label ID="lblBirthCertificateNo" runat="server" CssClass="label" />
                    </td>
                </tr>
            </table>
            <div style="margin-right: 22px;">
                <asp:ListView ID="lstCars" runat="server" ItemPlaceholderID="itemPlaceHolder">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 20px;"></th>
                                <th style="width: 70px;">نوع خودرو
                                </th>
                                <th style="width: 120px;">VIN
                                </th>
                                <th>پلاک خودرو
                                </th>
                                <th style="width: 40px;">وضعیت
                                </th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <table class="list-item-even">
                            <tr>
                                <td style="width: 20px;">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Eval("TypeName")%>
                                </td>
                                <td style="width: 120px;">
                                    <%#Eval("VIN")%>
                                </td>
                                <td>
                                    <%#Convert.ToBoolean(Eval("IsZoneType"))?string.Format("{0} {1}",Eval("ZNumber"),Eval("ZCity")): Public.PlateNumberRenderToHTML(Eval("TwoDigits").ToString(), Eval("Alphabet").ToString(), Eval("ThreeDigits").ToString(), Eval("RegionIdentifier").ToString())%>
                        
                                </td>
                                <td style="width: 40px;">
                                    <%#Eval("Status")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <table class="list-item-odd">
                            <tr>
                                <td style="width: 20px;">
                                    <%#Container.DataItemIndex + 1%>
                                </td>
                                <td style="width: 70px;">
                                    <%#Eval("TypeName")%>
                                </td>
                                <td style="width: 120px;">
                                    <%#Eval("VIN")%>
                                </td>
                                <td>
                                    <%#Convert.ToBoolean(Eval("IsZoneType"))?string.Format("{0} {1}",Eval("ZNumber"),Eval("ZCity")): Public.PlateNumberRenderToHTML(Eval("TwoDigits").ToString(), Eval("Alphabet").ToString(), Eval("ThreeDigits").ToString(), Eval("RegionIdentifier").ToString())%>
                                </td>
                                <td style="width: 40px;">
                                    <%#Eval("Status")%>
                                </td>
                            </tr>
                        </table>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <h1>آیتمی یافت نشد</h1>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
            <div class="alarm">
                با ثبت اطلاعات در این قسمت، آخرین خودروی ثبت شده برای این راننده در این سامانه غیرفعال
                شده و خودروی جدید برای راننده موردنظر بعنوان خودروی فعال ثبت و شناخته میگردد
            </div>
            <table style="margin-right: 20px;">
                <tr>
                    <td class="section-title" colspan="2">مشخصات خودرو جدید
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">PAN کارت سوخت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtFuelCardPAN"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">VIN شماره :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarVIN" runat="server" MaxLength="17" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="شماره VIN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtCarVIN"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="کد VIN باید 17 رقم باشد"
                            ControlToValidate="txtCarVIN" ClientValidationFunction="VINValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع خودرو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarType" runat="server" CssClass="dropdown-middle" DataValueField="CarTypeID"
                            DataTextField="TypeName">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="drpCarType"
                            ValueToCompare="0" Operator="GreaterThan" Type="Integer" CssClass="validator"
                            ErrorMessage="نوع خودرو را انتخاب کنید"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">مدل خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarModel" runat="server" MaxLength="4" SkinID="TextBoxMedium"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" MinimumValue="1350" MaximumValue="1391"
                            ErrorMessage="مدل باید بالاتر از 1349 باشد" Type="Integer" ControlToValidate="txtCarModel"
                            CssClass="validator"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">نوع سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFuelType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="0" Text="بنزین"></asp:ListItem>
                            <asp:ListItem Value="1" Text="بنزین و CNG"></asp:ListItem>
                            <asp:ListItem Value="2" Text="بنزین و LPG"></asp:ListItem>
                            <asp:ListItem Value="3" Text="CNG"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="star">*</span>
                    </td>
                </tr>
            </table>
            <table style="margin-right: 20px;">
                <tr>
                    <td class="fieldName-large">پلاک :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCarPlateNumber_5" runat="server" SkinID="TextBoxMedium" MaxLength="5"
                            Width="40px" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtCarPlateNumber_5"></asp:RequiredFieldValidator>

                    </td>
                    <td>
                        <asp:DropDownList ID="drpCarPlateNumberCity" runat="server" CssClass="dropdown" DataTextField="Name"
                            DataValueField="CityID">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="*"
                            ControlToValidate="drpCarPlateNumberCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
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
                        <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="*"
                            ControlToValidate="drpCarPlateNumberProvince" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <table style="margin-right: 20px;">
                <tr>
                    <td class="fieldName-large">استان :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="dropdown-middle" AutoPostBack="true"
                            OnSelectedIndexChanged="drpProvince_SelectedIndexChanged" Enabled="false">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
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
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpProvince" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" AutoPostBack="True" OnSelectedIndexChanged="drpCity_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpCity" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">آژانس :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancies" runat="server" CssClass="dropdown-middle" DataValueField="AjancyID"
                            DataTextField="AjancyName">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="آژانس را انتخاب کنید"
                            CssClass="validator" ControlToValidate="drpAjancies" ValueToCompare="0" Operator="GreaterThan"
                            Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <div class="pane-left">
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="ثبت" OnClick="btnSave_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="label-message" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function getCarOwner() {
            var nc = document.getElementById('txtNationalCode').value;
            if (nc.length == 10) {
                $.ajax({
                    type: 'GET',
                    data: ({ nc: nc }),
                    url: '../Union/AddFuelCard.aspx',
                    dataType: 'json',
                    cache: true,
                    success: function (resultdata) {
                        var _html = '<table id="list">';
                        _html += '<tr>';
                        _html += '<th></th>';
                        _html += '<th>PAN</th>';
                        _html += '<th>نوع کارت سوخت</th>';
                        _html += '<th></th>';
                        _html += '</tr>';
                        var color = '#ffffff';
                        var counter = 1;

                        for (index in resultdata) {
                            _html += '<tr style="background-color: ' + color + ';">';
                            _html += '<td>' + (parseInt(index) + 1) + '</td>';
                            _html += '<td>' + resultdata[index].PAN + '</td>';
                            _html += '<td>' + resultdata[index].FuelCardTypeName + '</td>';
                            _html += '<td>' + resultdata[index].Price + '</td>';
                            _html += '<td>' + resultdata[index].Weight + '</td>';
                            _html += '<td>' + (resultdata[index].Discount == null ? 0 : resultdata[index].Discount) + '</td>';
                            _html += '<td>' + resultdata[index].Count + '</td>';
                            _html += '</tr>';

                            if (counter % 2 == 1) { // change color for alternative row
                                color = '#f8faff';
                            }
                            else {
                                color = '#ffffff';
                            }
                            counter++;
                        }

                        _html += '</table>';
                        $('#fc-container').html(_html);
                    }
                });
            }
        }

    </script>
</asp:Content>
