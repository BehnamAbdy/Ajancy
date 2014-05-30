<%@ Page Title="شکایات" Language="C#" MasterPageFile="~/SiteWide.master" AutoEventWireup="true"
    CodeFile="Complaints.aspx.cs" Inherits="Ajancy_Complaints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            شکایات ارسال شده به اتحادیه
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
                        وضعیت :
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
    <div class="dialog" style="height: 139px; width: 340px;">
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
        <div style="border: solid 1px #d8dcdb; height: 90px; padding: 3px; width: 332px;">
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#content-wide').css('min-height', '400px');
        });

        function hideDialog() {
            $('.dialog div').text('');
            $('.dialog').hide(300);
        }

        function showDialog(element, id) {
            $('.dialog').data('id', id);
            if ($get('<%=drpReplyMode.ClientID %>').selectedIndex == 0) {
                $.ajax({
                    type: 'GET',
                    data: ({ id: id }),
                    url: '../Ajancy/Complaints.aspx',
                    dataType: 'html',
                    cache: true,
                    success: function (result) {
                        $('.dialog div').text(result);
                    }
                });
            }

            var aryPosition = objectPosition(element);
            $('.dialog').css({ 'left': aryPosition[0], 'top': aryPosition[1] });
            $('.dialog').show(300);
        }

    </script>
</asp:Content>
