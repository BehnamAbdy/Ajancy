<%@ Page Title="ویرایش  پیوند ها" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Links.aspx.cs" Inherits="Site_Links" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ویرایش پیوند ها
    </div>
    <table style="margin: 15px 15% 0 0;">
        <tr>
            <td>
            </td>
            <td colspan="2">
                <select id="drpLinks" class="dropdown" style="width: 200px;" onchange="javascript:setMode()">
                </select>
            </td>
        </tr>
        <tr>
            <td class="fieldName">
                عنوان پیوند :
            </td>
            <td colspan="2">
                <input type="text" id="txtTitle" class="textbox" style="width: 250px;" />
            </td>
        </tr>
        <tr>
            <td class="fieldName">
                آدرس پیوند :
            </td>
            <td>
                <input type="text" id="txtTarget" class="textbox" style="direction: ltr; width: 250px;" />
            </td>
            <td style="color: Gray; font-size: 11px; vertical-align: bottom; text-align: left;">
                http://www.ParsNavgan.ir
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="btnSave" onclick="javascript:commit()" class="button" value="ثبت" />
            </td>
            <td class="label-message" colspan="2">
            </td>
        </tr>
    </table>
    <script src="../Scripts/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            loadLinks();
        });

        function loadLinks() {
            var drp = document.getElementById('drpLinks');
            for (var i = drp.options.length - 1; i >= 0; i--) {
                drp.remove(i);
            }

            $.ajax({
                type: 'GET',
                data: ({ mode: 1 }),
                url: '../Site/Links.aspx',
                dataType: 'json',
                cache: false,
                success: function (resultdata) {
                    drp.options[0] = new Option('-- پیوند جدید --', '0');
                    for (index in resultdata) {
                        drp.options[drp.options.length] = new Option(resultdata[index].Title, resultdata[index].Id);
                    }
                }
            });
        }

        function setMode() {
            $('.label-message').text('');
            var drp = document.getElementById('drpLinks');
            if (drp.selectedIndex > 0) {
                $.ajax({
                    type: 'GET',
                    data: ({ mode: 2, id: drp.value }),
                    url: '../Site/Links.aspx',
                    dataType: 'json',
                    cache: false,
                    success: function (resultdata) {
                        for (index in resultdata) {
                            document.getElementById('txtTitle').value = resultdata[index].Title;
                            document.getElementById('txtTarget').value = resultdata[index].Target;
                            document.getElementById('btnSave').value = 'ویرایش';
                        }
                    }
                });
            }
            else {
                document.getElementById('txtTitle').value = '';
                document.getElementById('txtTarget').value = '';
                document.getElementById('btnSave').value = 'ثبت';
            }
        }

        function commit() {
            var title = document.getElementById('txtTitle').value;
            if (title == '') {
                $('.label-message').text('عنوان پیوند نوشته نشده');
                return;
            }

            var target = document.getElementById('txtTarget').value;
            if (target == '') {
                $('.label-message').text('آدرس پیوند نوشته نشده');
                return;
            }
            else if (!isValidURL(target)) {
                $('.label-message').text('فرمت آدرس پیوند نادرست میباشد');
                return;
            }

            var id = document.getElementById('drpLinks').value;
            $.ajax({
                type: 'GET',
                data: ({ id: id, tle: title, trg: target }),
                url: '../Site/Links.aspx',
                dataType: 'json',
                cache: false,
                success: function (result) {
                    if (result == '1') {
                        document.getElementById('txtTitle').value = '';
                        document.getElementById('txtTarget').value = '';
                        document.getElementById('btnSave').value = 'ثبت';
                        loadLinks();
                        $('.label-message').text(id == '0' ? 'ثبت پیوند جدید انجام گردید' : 'ویرابش پیوند انجام گردید');
                    }
                }
            });
        }

        function isValidURL(url) {
            var RegExp = /^(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$/;
            if (RegExp.test(url)) {
                return true;
            } else {
                return false;
            }
        } 

    </script>
</asp:Content>
