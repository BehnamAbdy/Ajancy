<%@ Page Title="رانندگان متقاضی کارت صلاحیت آژانس" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="CertificationlessDrivers.aspx.cs" Inherits="Union_CertificationlessDrivers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            لیست رانندگان متقاضی دفترچه صلاحیت آژانس
        </div>
    </center>
    <br />
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 55px;">
                <tr>
                    <td class="fieldName">
                        از تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDateFrom" runat="server" Required="false" />
                    </td>
                    <td class="fieldName">
                        تا تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDateTo" runat="server" Required="false" />
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
                    <td class="fieldName-large">
                        کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                    <td class="fieldName-large">
                        شماره شناسنامه :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthCertificateNo" runat="server" SkinID="TextBoxMedium" MaxLength="10"
                            onkeypress="javascript:return isNumberKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        آژانس :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpAjancies" runat="server" DataTextField="AjancyName" DataValueField="AjancyID"
                            CssClass="dropdown-middle">
                            <asp:ListItem Text="- همه آژانس ها -"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="fieldName">
                        خودرو :
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="drpCarType" runat="server" DataTextField="TypeName" DataValueField="CarTypeID"
                            CssClass="dropdown-middle">
                        </asp:DropDownList>
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
                        <asp:Button ID="Button1" runat="server" CssClass="button" OnClick="btnSearch_Click"
                            Text="جستجو" />
                    </td>
                    <td align="right">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnl">
                            <ProgressTemplate>
                                <img src="../App_Themes/Default/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <hr />
            <div style="margin: 10px;">
                <asp:ListView ID="lstDrivers" runat="server" DataSourceID="ObjectDataSource1">
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
                                    جنسیت
                                </th>
                                <th>
                                    تاریخ درخواست
                                </th>
                                <th>
                                    آژانس
                                </th>
                                <th style="width: 50px;">
                                    جزییات
                                </th>
                                <th style="width: 50px;">
                                    تبدیل
                                </th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                        <div id="pager">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
                                <Fields>
                                    <asp:NumericPagerField RenderNonBreakingSpacesBetweenControls="true" CurrentPageLabelCssClass="CurrentPageButton"
                                        NextPageText=">>" NextPreviousButtonCssClass="NextPrevPageButton" NumericButtonCssClass="PageButton"
                                        PreviousPageText="<<" />
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
                                    <%#Eval("Gender")%>
                                </td>
                                <td>
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td>
                                    <%#Eval("AjancyName")%>
                                </td>
                                <td style="width: 50px;">
                                    <a class="info" href='<%#string.Format("Driver.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank">جزییات</a>
                                </td>
                                <td style="width: 50px;">
                                    <a class="info" href='<%#string.Format("RegisterDriverCertification.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("DriverCertificationID").ToString()))%>'
                                        target="_blank">تبدیل</a>
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
                                    <%#Eval("Gender")%>
                                </td>
                                <td>
                                    <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                                </td>
                                <td>
                                    <%#Eval("AjancyName")%>
                                </td>
                                <td style="width: 50px;">
                                    <a class="info" href='<%#string.Format("Driver.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank">جزییات</a>
                                </td>
                                <td style="width: 50px;">
                                    <a class="info" href='<%#string.Format("RegisterDriverCertification.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("DriverCertificationID").ToString()))%>'
                                        target="_blank">تبدیل</a>
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
                SelectCountMethod="GetCertificationlessDriversCount" SelectMethod="LoadCertificationlessDrivers"
                TypeName="Paging" EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="AjancyId" Type="Int32" />
                    <asp:QueryStringParameter Name="CarType" Type="Byte" />
                    <asp:QueryStringParameter Name="FirstName" Type="String" />
                    <asp:QueryStringParameter Name="LastName" Type="String" />
                    <asp:QueryStringParameter Name="NationalCode" Type="String" />
                    <asp:QueryStringParameter Name="BirthCertificateNo" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_1" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_2" Type="String" />
                    <asp:QueryStringParameter Name="CarPlateNumber_3" Type="String" />
                    <asp:QueryStringParameter Name="Alphabet" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="dialog" style="height: 165px; width: 370px;">
        <div class="exit-bar">
            <img src="../App_Themes/Default/images/close.gif" title="خروج" style="cursor: pointer;"
                onclick="javascript:hideDialog(null, 0)" />
        </div>
        <table>
            <tr>
                <td class="fieldName">
                    شماره کارت صلاحیت :
                </td>
                <td>
                    <input type="text" id="txtDriverCertificationNo" class="textbox-middle" style="width: 200px;"
                        maxlength="20" />
                    <span class="star">*</span>
                </td>
            </tr>
            <tr>
                <td class="fieldName">
                    تاریخ صدور :
                </td>
                <td>
                    <userControl:Date ID="txtStartDate" runat="server" Required="false" />
                </td>
            </tr>
            <tr>
                <td class="fieldName">
                    تاریخ انقضا :
                </td>
                <td>
                    <userControl:Date ID="txtEndDate" runat="server" Required="false" />
                </td>
            </tr>
            <tr>
                <td class="fieldName">
                </td>
                <td>
                    <input type="button" id="btnVerify" class="button" onclick="javascript:verify()"
                        value="تایید" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function showDialog(item) {
            var aryPosition = objectPosition(item);
            $('.dialog').css({ 'left': aryPosition[0], 'top': aryPosition[1] });
            $('.dialog').slideDown(500);

        }

        function hideDialog() {
            $('.dialog :text').val('');
            document.getElementById('<%=txtStartDate.FindControl("txtDate").ClientID%>').value = '____/__/__';
            document.getElementById('<%=txtEndDate.FindControl("txtDate").ClientID%>').value = '____/__/__';
            $('.dialog span').css('color', '#a7a7a7');
            $('.dialog').slideUp(500);
        }

        function verify() {
            if ($('.dialog :text').val() == '') {
                $('.dialog span').css('color', '#fa4141');
                return;
            }

            if (document.getElementById('<%=txtStartDate.FindControl("txtDate").ClientID%>').value == '____/__/__' ||
                    document.getElementById('<%=txtEndDate.FindControl("txtDate").ClientID%>').value == '____/__/__') {
                return;
            }

            if (isItemSelected(grdRequests)) {
                if (confirm('برای این درخواست کارت صلاحیت صادر گردد؟')) {
                    var selectedItem = grdRequests.getSelectedItems()[0];
                    $.ajax({
                        type: 'GET',
                        data: ({ id: selectedItem.getMember('DriverCertificationID').get_text(), dcn: $('.dialog :text').val(), sd: document.getElementById('<%=txtStartDate.FindControl("txtDate").ClientID%>').value, ed: document.getElementById('<%=txtEndDate.FindControl("txtDate").ClientID%>').value }),
                        url: '../Management/DriverRequests.aspx',
                        dataType: 'json',
                        cache: false,
                        success: function (result) {
                            if (result == '1') {
                                grdRequests.deleteItem(selectedItem);
                                grdRequests.Render();
                                hideDialog();
                            }
                        }
                    });
                }
            }
        }
    </script>
</asp:Content>
