<%@ Page Title="شکایات آژانسها و رانندگان" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="HandleComplaints.aspx.cs" Inherits="Management_HandleComplaints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            رسیدگی به شکایات آژانسها و رانندگان
        </div>
    </center>
    <br />
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table style="margin-right: 130px;">
                <tr>
                    <td class="fieldName">
                        از تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDateFrom" runat="server" Required="false" />
                    </td>
                    <td>
                        <asp:DropDownList ID="drpComplaintType" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text=" شکایات آژانسها"></asp:ListItem>
                            <asp:ListItem Text="شکایات رانندگان"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName">
                        تا تاریخ :
                    </td>
                    <td>
                        <userControl:Date ID="txtDateTo" runat="server" Required="false" />
                    </td>
                    <td>
                        <asp:DropDownList ID="drpReplyMode" runat="server" CssClass="dropdown-middle">
                            <asp:ListItem Text="پاسخ داده شده"></asp:ListItem>
                            <asp:ListItem Text="پاسخ داده نشده"></asp:ListItem>
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
            <asp:ListView ID="lstComplaints" runat="server" ItemPlaceholderID="itemPlaceHolder">
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
                                شماره صلاحیت
                            </th>
                            <th>
                                تاریخ شکایت
                            </th>
                            <th>
                                آژانس
                            </th>
                        </tr>
                    </table>
                    <div id="itemPlaceHolder" runat="server">
                    </div>
                    <%-- <div id="pager">
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lstOrders" PageSize="10">
                    <Fields>
                        <asp:NumericPagerField RenderNonBreakingSpacesBetweenControls="true" CurrentPageLabelCssClass="CurrentPageButton"
                            NextPageText="Next 5" NextPreviousButtonCssClass="NextPrevPageButton" NumericButtonCssClass="PageButton"
                            PreviousPageText="Prev 5" />
                    </Fields>
                </asp:DataPager>
            </div>--%>
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
                                <%#Eval("DriverCertificationNo") == null ? "ندارد" : Eval("DriverCertificationNo")%>
                            </td>
                            <td>
                                <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                            </td>
                            <td>
                                <%#Eval("AjancyName")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" class="complaint">
                                <div>
                                    <%#Eval("Comment")%>
                                </div>
                            </td>
                            <td>
                                <img src="../App_Themes/Default/images/reply.png" onclick="javascript:showDialog(this, <%#Eval("AjancyComplaintID")%>)"
                                    style="cursor: pointer;" title="پاسخ" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <AlternatingItemTemplate>
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
                                <%#Eval("DriverCertificationNo") == null ? "ندارد" : Eval("DriverCertificationNo")%>
                            </td>
                            <td>
                                <%#Public.ToPersianDate(Eval("SubmitDate"))%>
                            </td>
                            <td>
                                <%#Eval("AjancyName")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" class="complaint">
                                <div>
                                    <%#Eval("Comment")%>
                                </div>
                            </td>
                            <td>
                                <img src="../App_Themes/Default/images/reply.png" onclick="javascript:showDialog(this, <%#Eval("AjancyComplaintID")%>)"
                                    style="cursor: pointer;" title="پاسخ" />
                            </td>
                        </tr>
                    </table>
                </AlternatingItemTemplate>
                <EmptyDataTemplate>
                    <h1>
                        آیتمی یافت نشد</h1>
                </EmptyDataTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="dialog" style="height: 160px; width: 340px;">
        <table class="exit-bar">
            <tr>
                <td style="width: 90%;">
                    پاسخ
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
            $('.dialog').removeData('d');
            $('.dialog').hide(300);
        }

        function showDialog(element, id) {
            $('.dialog').data('d', { id: id, item: element });
            if ($get('<%=drpReplyMode.ClientID %>').selectedIndex == 0) {
                $.ajax({
                    type: 'GET',
                    data: ({ id: id }),
                    url: '../Management/HandleComplaints.aspx',
                    dataType: 'html',
                    cache: true,
                    success: function (result) {
                        $('.dialog textarea#txtComment').val(result);
                    }
                });
            }

            var aryPosition = objectPosition(element);
            $('.dialog .star').css('color', '#a7a7a7');
            $('.dialog').css({ 'left': aryPosition[0], 'top': aryPosition[1] });
            $('.dialog').show(300);
        }

        function send() {
            var txt = $('.dialog textarea#txtComment');
            if (isValid(txt)) {
                $.ajax({
                    type: 'GET',
                    data: ({ id: $('.dialog').data('d').id, txt: txt.val() }),
                    url: '../Management/HandleComplaints.aspx',
                    dataType: 'json',
                    cache: false,
                    success: function (result) {
                        if (result == '1') {
                            alert('ثبت پاسخ انجام گردید');
                            $($('.dialog').data('d').item).parent().parent().parent().remove();
                            hideDialog();
                        }
                    }
                });
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
