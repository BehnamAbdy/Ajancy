<%@ Page Title="رسیدگی به شکایات مردمی" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="HandleComments.aspx.cs" Inherits="Management_HandleComments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            فرم رسیدگی به شکایات مردمی
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <br />
            <table style="margin-right: 130px;">
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
                    <td rowspan="2">
                        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="جستجو" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <asp:ListView ID="lstComplaints" runat="server" ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
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
                    <table class="comment">
                        <tr>
                            <th style="width: 30px;">
                                <img src="../App_Themes/Default/images/comment.png" alt="Alternate Text" />
                            </th>
                            <th colspan="4" style="width: 670px;">
                                <%# Subject(Eval("Subject").ToString())%>
                            </th>
                            <th style="direction: ltr; font-size: 12px; text-align: left; width: 100px;">
                                <%# Public.ToPersianDateTime(Eval("ProblemDate")) %>
                            </th>
                        </tr>
                        <tr>
                            <td class="field">
                                از طرف :
                            </td>
                            <td style="width: 220px;">
                                <%# string.Format("{0} {1} ({2})", Eval("FirstName"), Eval("LastName"), Eval("Phone"))%>
                            </td>
                            <td class="field">
                                آژانس :
                            </td>
                            <td style="width: 140px;">
                                <%# Eval("AjancyName")%>
                            </td>
                            <td class="field" style="width: 40px;">
                                راننده :
                            </td>
                            <td style="width: 275px;">
                                <%# string.Format("{0} {1} ({2})", Eval("DriverFirstName"), Eval("DriverLastName"), Public.PlateNumberRenderToHTML(Eval("PlateNumber") as Ajancy.PlateNumber))%>
                                <%--<%# string.Concat(Eval("DriverFirstName"), "  ", Eval("DriverLastName"))%>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <p class="text">
                                    <%# Eval("CommentText")%>
                                </p>
                            </div>
                        </tr>
                        <tr>
                            <td colspan="6" align="left">
                                <span class="item-box" onclick="javascript:deleteItem(this, <%# Eval("CommentID")%>)">
                                    حذف</span>
                            </td>
                        </tr>
                    </table>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <h1>
                        آیتمی یافت نشد</h1>
                </EmptyDataTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function deleteItem(item, id) {
            if (confirm(preDeleteMessagse)) {
                $.ajax({
                    type: 'GET',
                    data: ({ mode: 0, id: id }),
                    url: '../Management/HandleComments.aspx',
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
