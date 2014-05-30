<%@ Page Title="بارگذاری مدارک" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="UploadDocument.aspx.cs" Inherits="Members_UploadDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            بارگذاری مدارک
        </div>
    </center>
    <table>
        <tr>
            <td class="fieldName">
                نوع فایل
            </td>
            <td>
                <asp:DropDownList ID="drpType" runat="server" CssClass="dropdown">
                    <asp:ListItem Value="1" Text="عکس کاربر" />
                </asp:DropDownList>
            </td>
            <td style="text-align: left; width: 16px;">
                <asp:Label ID="Throbber" runat="server" Style="display: none">
                    <img src="../App_Themes/Default/images/indicator.gif" alt="loading" />
                </asp:Label>
            </td>
            <td style="direction: ltr; width: 400px;">
                <ajaxToolkit:AsyncFileUpload ID="fluDocument" Width="400px" runat="server" OnClientUploadError="uploadError"
                    OnClientUploadStarted="StartUpload" OnClientUploadComplete="UploadComplete" CompleteBackColor="Lime"
                    UploaderStyle="Modern" ErrorBackColor="Red" ThrobberID="Throbber" OnUploadedComplete="fluDocument_UploadedComplete"
                    UploadingBackColor="#66CCFF" />
            </td>
        </tr>
    </table>
    <div class="pane-left">
        <label id="lblStatus" class="label-message">
        </label>
    </div>
    <div style="height: 210px; padding: 5px; width: 98%;">
        <img id="imgPersonPic" style="border: solid 1px #dadada; float: left; height: 200px;
            padding: 3px; width: 160px;" alt="عکس کاربر" />
    </div>
    <input type="button" onclick="loadPicture();" value="bbbbbbbb" />
    <asp:HiddenField ID="hdnPersonId" runat="server" />
    <script src="../Scripts/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $('#imgPersonPic').attr('src', '../Document.ashx?pId=' + $get('<%=hdnPersonId.ClientID%>').value);
        });

        function uploadError(sender, args) {
            document.getElementById('lblStatus').innerText = args.get_fileName(), "<span style='color:red;'>" + args.get_errorMessage() + "</span>";
        }

        function StartUpload(sender, args) {
            document.getElementById('lblStatus').innerText = 'درحال بارگذاری';
        }

        function UploadComplete(sender, args) {
            //            var filename = args.get_fileName();
            var contentType = args.get_contentType();
            //            var text = "Size of " + filename + " is " + args.get_length() + " bytes";
            //            if (contentType.length > 0) {
            //                text += " and content type is '" + contentType + "'.";
            //            }
            if (contentType == 'image/pjpeg' || contentType == 'image/x-png') {
                document.getElementById('lblStatus').innerText = 'فایل شما بارگذاری گردید';

                //                $.getJSON('../Document.ashx', { pId: $get('<%=hdnPersonId.ClientID%>').value }, function (pic) {
                //            
                //                });
                $('#imgPersonPic').attr('src', '../Document.ashx?pId=' + $get('<%=hdnPersonId.ClientID%>').value);
            }
            else {
                document.getElementById('lblStatus').innerText = 'فرمت فایل بارگذاری شده عکس نمیباشد';
            }
        }

        function loadPicture(pic) {
            $('#imgPersonPic').attr('src', '../Document.ashx?pId=' + $get('<%=hdnPersonId.ClientID%>').value);
        }      
    </script>
</asp:Content>
