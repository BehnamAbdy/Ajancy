<%@ Page Title="مدیریت دسترسی کاربران" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserRolesStatus.aspx.cs" Inherits="Management_UserRolesStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <div class="title-bar">
            مدیریت دسترسی کاربران 
        </div>
    </center>
    <table>
        <tr>
            <td class="fieldName">سمت :
            </td>
            <td>
                <select id="drpRoles" class="dropdown-middle" onchange="javascript:loadTree();">
                    <option>مدیر آژانس های استان</option>
                    <option>مدیر آژانس های شهرستان</option>
                    <option>مدیر آموزشگاه های استان</option>
                    <option>مدیر آموزشگاه های شهرستان</option>
                </select>
                <img id="snipper" src="../App_Themes/Default/images/ajax-loader.gif" />
            </td>
        </tr>
    </table>
    <div id="tree" style="margin-top: 10px; width: 560px;">
    </div>

    <script type="text/javascript" src="../Scripts/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.cookie.js"></script>
    <script type="text/javascript" src="../Scripts/src/jquery.dynatree.js"></script>
    <link href="../Scripts/src/skin/ui.dynatree.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function loadTree() {
            $('#snipper').css('visibility', 'visible');
            $.ajax({
                type: 'GET',
                data: { mode: document.getElementById('drpRoles').selectedIndex },
                url: '../Management/UserRolesStatus.aspx',
                dataType: 'json',
                cache: false,
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    $('#tree').html('');
                    var dv = $("<div style='height: 460px;width: 400px;'></div>");
                    $(dv).dynatree({
                        checkbox: true,
                        selectMode: 3,
                        children: result,
                        onCreate: function (node, nodeSpan) {
                            if (node.data.selected) {
                                node._select(true, false, false);
                            }
                        },
                        onSelect: function (select, node) {
                            var nodeKeys = [];
                            if (node.data.key == 0 && node.childList != null) {
                                if (select) {
                                    for (var i = 0; i < node.childList.length; i++) {
                                        if (!node.childList[i].data.selected) {
                                            nodeKeys.push(node.childList[i].data.key);
                                            node.childList[i].data.selected = true;
                                        }
                                    }
                                } else {
                                    for (var i = 0; i < node.childList.length; i++) {
                                        if (node.childList[i].data.selected) {
                                            nodeKeys.push(node.childList[i].data.key);
                                            node.childList[i].data.selected = false;
                                        }
                                    }
                                }
                            } else if (node.data.key > 0) {
                                nodeKeys.push(node.data.key);
                            }

                            if (nodeKeys.length > 0) {
                                $('#snipper').css('visibility', 'visible');
                                $.ajax({
                                    type: 'POST',
                                    cache: false,
                                    data: { keys: nodeKeys.length == 1 ? nodeKeys[0] : nodeKeys.join(','), status: select },
                                    url: '../Management/UserRolesStatus.aspx',
                                    dataType: 'json',
                                    success: function () {
                                        $('#snipper').css('visibility', 'hidden');
                                    }
                                });
                            }
                        },
                        onDblClick: function (node, event) {
                            node.toggleSelect();
                        },
                        onKeydown: function (node, event) {
                            if (event.which == 32) {
                                node.toggleSelect();
                                return false;
                            }
                        },
                        cookieId: "dynatree-Cb3",
                        idPrefix: "dynatree-Cb3-"
                    });

                    $(dv).appendTo('#tree');
                    $('#snipper').css('visibility', 'hidden');
                }
            });
        }

        $(document).ready(function () {
            loadTree();
        });

    </script>
</asp:Content>

