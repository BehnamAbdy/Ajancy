<%@ Page Title="مدیریت کاربران سیستم" Language="C#" MasterPageFile="~/SiteWide.master"
    AutoEventWireup="true" CodeFile="UsersList.aspx.cs" Inherits="Management_UsersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            مدیریت کاربران سیستم
        </div>
    </center>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <table class="centered">
                <tr>
                    <td class="fieldName-large">
                        استان :
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
                    <td class="fieldName-large">
                        شهر :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="dropdown-middle" DataTextField="Name"
                            DataValueField="CityID" Enabled="false">
                            <asp:ListItem Value="0" Text="- انتخاب کنید -" />
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
                        کد ملی :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNationalCode" runat="server" SkinID="TextBoxMedium" MaxLength="10"></asp:TextBox>
                    </td>
                    <td class="fieldName">
                        سمت :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpRoles" runat="server" CssClass="dropdown-middle">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="fieldName" colspan="2">
                    </td>
                    <td align="right">
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
            <br />
            <div style="margin: 0px 5px 10px 0px;">
                <asp:ListView ID="lstUsers" runat="server" DataSourceID="ObjectDataSource1">
                    <LayoutTemplate>
                        <table id="list-header">
                            <tr>
                                <th style="width: 30px;">
                                </th>
                                <th style="width: 80px;">
                                    کد ملی
                                </th>
                                <th>
                                    نام
                                </th>
                                <th>
                                    نام خانوادگی
                                </th>
                                <th style="width: 100px;">
                                    استان
                                </th>
                                <th style="width: 100px;">
                                    شهر
                                </th>
                                <th style="width: 100px;">
                                    آخرین ورود
                                </th>
                                <th style="width: 90px;">
                                    ویرایش گذرواژه
                                </th>
                                <th style="width: 60px;">
                                    سمت ها
                                </th>
                                <th style="width: 60px;">
                                    ویرایش
                                </th>
                            </tr>
                        </table>
                        <div id="itemPlaceHolder" runat="server">
                        </div>
                        <div id="pager">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
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
                                <td style="width: 80px;">
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td style="width: 100px;">
                                    <%#Eval("Province")%>
                                </td>
                                <td style="width: 100px;">
                                    <%#Eval("City")%>
                                </td>
                                <td style="width: 100px;">
                                    <%#Public.ToPersianDateTime(Eval("LastLoginDate"))%>
                                </td>
                                <td style="width: 90px;">
                                    <asp:LinkButton ID="lnkPassword" runat="server" CssClass="info" CommandArgument='<%#string.Format("{0} {1} : {2}|{3}", Eval("FirstName"), Eval("LastName"), Eval("NationalCode"), Eval("UserID"))%>'
                                        OnClick="lnkPassword_Click" CausesValidation="false" Text="ویرایش گذرواژه" />
                                </td>
                                <td style="width: 60px;">
                                    <a class="item-box" style="float: none;" href='<%#string.Format("../Management/UserRoles.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank">سمت</a>
                                </td>
                                <td style="width: 60px;">
                                    <a class="item-box" style="float: none;" href='<%#string.Format("../Management/Person.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank">ویرایش</a>
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
                                <td style="width: 80px;">
                                    <%#Eval("NationalCode")%>
                                </td>
                                <td>
                                    <%#Eval("FirstName")%>
                                </td>
                                <td>
                                    <%#Eval("LastName")%>
                                </td>
                                <td style="width: 100px;">
                                    <%#Eval("Province")%>
                                </td>
                                <td style="width: 100px;">
                                    <%#Eval("City")%>
                                </td>
                                <td style="width: 100px;">
                                    <%#Public.ToPersianDateTime(Eval("LastLoginDate"))%>
                                </td>
                                <td style="width: 90px;">
                                    <asp:LinkButton ID="lnkPassword" runat="server" CssClass="info" CommandArgument='<%#string.Format("{0} {1} : {2}|{3}", Eval("FirstName"), Eval("LastName"), Eval("NationalCode"), Eval("UserID"))%>'
                                        OnClick="lnkPassword_Click" CausesValidation="false" Text="ویرایش گذرواژه" />
                                </td>
                                <td style="width: 60px;">
                                    <a class="item-box" style="float: none;" href='<%#string.Format("../Management/UserRoles.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank">سمت</a>
                                </td>
                                <td style="width: 60px;">
                                    <a class="item-box" style="float: none;" href='<%#string.Format("../Management/Person.aspx?id={0}", TamperProofString.QueryStringEncode(Eval("PersonID").ToString()))%>'
                                        target="_blank">ویرایش</a>
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
            <div id="pnlPassword" runat="server" visible="false" style="border: dashed 1px #abb4c3;
                margin-right: 245px; padding: 4px; width: 290px;">
                <table>
                    <tr>
                        <td class="fieldName" id="tdPasswordPanelTitle" runat="server" style="font-size: 16px;
                            line-height: 30px; text-align: center; vertical-align: middle;" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName">
                            گذرواژه جدید :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" SkinID="TextBox" TextMode="Password"></asp:TextBox>
                            <span class="star">*</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                                SetFocusOnError="true" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName">
                            تکرار گذرواژه :
                        </td>
                        <td>
                            <asp:TextBox ID="txtRePassword" runat="server" SkinID="TextBox" TextMode="Password"></asp:TextBox>
                            <span class="star">*</span>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtRePassword"
                                ControlToCompare="txtPassword" CssClass="validator"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRePassword"
                                SetFocusOnError="true" Display="Dynamic" ErrorMessage="*" CssClass="validator"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left; padding-left: 25px;">
                            <asp:Button ID="btnBackPassword" runat="server" CssClass="button" OnClick="btnBack_Click"
                                Text="بازگشت" UseSubmitBehavior="false" CausesValidation="false" Style="font-size: 11px;
                                padding: 1px 5px 4px 5px;" />
                            <asp:Button ID="btnChangePassword" runat="server" CssClass="button" OnClick="btnChangePassword_Click"
                                Text="ذخیره" UseSubmitBehavior="false" Style="font-size: 11px; padding: 1px 5px 4px 5px;" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OnSelecting="ObjectDataSource1_Selecting"
                SelectCountMethod="GetUsersCount" SelectMethod="LoadUsers" TypeName="Paging"
                EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter Name="RoleId" Type="Int16" />
                    <asp:QueryStringParameter Name="NationalCode" Type="String" />
                    <asp:QueryStringParameter Name="FirstName" Type="String" />
                    <asp:QueryStringParameter Name="LastName" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
