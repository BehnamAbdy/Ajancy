<%@ Page Title="ثبت کارت سوخت صادره جدید برای خودرو" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="AddFuelCard.aspx.cs" Inherits="Union_AddFuelCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ثبت کارت سوخت صادره برای خودرو
    </div>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table class="centered" style="width: 555px;">
                <tr>
                    <td class="fieldName-large">
                        کد ملی مالک خودرو :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            OnTextChanged="txtNationalCode_TextChanged" AutoPostBack="true" onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtNationalCode"
                            CssClass="validator" SetFocusOnError="true" Display="Dynamic" ErrorMessage="کد ملی را وارد کنید"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="کد ملی باید 10 رقم باشد"
                            CssClass="validator" ControlToValidate="txtNationalCode" ClientValidationFunction="nationalCodeValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        استان :
                    </td>
                    <td>
                        <asp:Label ID="lblProvince" runat="server" CssClass="label" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        شهر :
                    </td>
                    <td>
                        <asp:Label ID="lblCity" runat="server" CssClass="label" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نام :
                    </td>
                    <td>
                        <asp:Label ID="lblOwner" runat="server" CssClass="label" />
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        خودرو :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCars" runat="server" CssClass="dropdown-middle" DataTextField="Car"
                            DataValueField="CarID" AutoPostBack="True" OnSelectedIndexChanged="drpCars_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span class="star">*</span>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpCars" ValueToCompare="0" Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <div id="dvFCcontainer" runat="server">
            </div>
            <div class="alarm">
                با ثبت اطلاعات در این قسمت، کارت سوخت فعلی غیرفعال شده و کارت سوخت جدید برای خودروی
                انتخابی بعنوان کارت سوخت فعال ثبت و شناخته میگردد
            </div>
            <table class="centered" style="width: 555px;">
                <tr>
                    <td class="section-title" colspan="2">
                        مشخصات کارت سوخت جدید
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        PAN کارت سوخت :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuelCardPAN" runat="server" MaxLength="16" SkinID="TextBoxMedium"></asp:TextBox>
                        <span class="star">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="شماره PAN را وارد کنید"
                            CssClass="validator" ControlToValidate="txtFuelCardPAN"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName-large">
                        نوع کارت سوخت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpFuelCardType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Value="-1" Text="- انتخاب کنید -" />
                            <asp:ListItem Value="0" Text="آژانس"></asp:ListItem>
                            <asp:ListItem Value="1" Text="شخصی"></asp:ListItem>
                            <asp:ListItem Value="2" Text="خطی و بین راهی"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" SkinID="CompareValidator"
                            ControlToValidate="drpFuelCardType" ValueToCompare="-1" Operator="GreaterThan"
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
