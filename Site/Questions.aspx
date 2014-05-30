<%@ Page Title="ویرایش متن سوالات متداول" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" ValidateRequest="false" CodeFile="Questions.aspx.cs" Inherits="Admin_Questions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <div class="title-bar">
        ویرایش متن سوالات متداول
    </div>
    <table style="margin: 25px 5% 0 0;">
        <tr>
            <td>
                <textarea id="txtEditor" name="txtEditor" cols="60" rows="10" style="height: 400px;
                    width: 600px;"></textarea>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="btnSave" onclick="javascript:commit()" class="button" value="ثبت" />
                &nbsp;
                <label class="label-message">
                </label>
            </td>
        </tr>
    </table>
    <script src="../Scripts/javascript.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.cleditor.min.js" type="text/javascript"></script>
    <link href="../Styles/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {
            $('#txtEditor').cleditor();
            $.ajax({
                type: 'POST',
                data: ({ mode: 'r' }),
                url: '../Site/Questions.aspx',
                dataType: 'json',
                success: function (response) {
                    document.getElementById('txtEditor').value = response[0].Value;
                    $('#txtEditor').cleditor()[0].updateFrame();
                }
            });
        });

        function commit() {
            var txt = document.getElementById('txtEditor').value;
            if (txt == '') {
                $('.label-message').text('متن آیتم نوشته نشده');
                return;
            }

            $.ajax({
                type: 'POST',
                data: ({ mode: 'w', txt: txt }),
                url: '../Site/Questions.aspx',
                dataType: 'json',
                cache: false,
                success: function (result) {
                    if (result == '1') {
                        document.getElementById('btnSave').value = 'ثبت';
                        $('.label-message').text('ویرابش آیتم انجام گردید');
                    }
                }
            });
        }

    </script>
</asp:Content>
