<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NEditor.ascx.cs" Inherits="NEditor" %>

<script src="../Scripts/EditorScripts.js" type="text/javascript"></script>

<div dir="ltr">
    &nbsp;<img src="../App_Themes/Default/images/editorimages/toolbarstart.gif" />
    <img src="../App_Themes/Default/images/editorimages/Bold.gif" class="buttonStyle"
        onclick="doAction('Bold')" title="Bold" />
    <img src="../App_Themes/Default/images/editorimages/Italic.gif" class="buttonStyle"
        onclick="doAction('Italic')" title="Italic" />
    <img src="../App_Themes/Default/images/editorimages/Underline.gif" class="buttonStyle"
        onclick="doAction('Underline')" title="Underline" />
    <img src="../App_Themes/Default/images/editorimages/StrikeThrough.gif" class="buttonStyle"
        onclick="doAction('Strikethrough')" title="StrikeThrough" />
    <img src="../App_Themes/Default/images/editorimages/Font.gif" class="buttonStyle"
        onclick="showFontDiv()" title="Font" />
    <img src="../App_Themes/Default/images/editorimages/FontSize.gif" class="buttonStyle"
        onclick="showFontSizeDiv()" title="Font Size" />
    <img src="../App_Themes/Default/images/editorimages/FormatCodeBlock.gif" class="buttonStyle"
        onclick="showHeadingDiv()" title="Heading" />
    <img src="../App_Themes/Default/images/editorimages/BackColor.gif" class="buttonStyle"
        onclick="showColorDiv('BackColor')" title="Backcolor" />
    <img src="../App_Themes/Default/images/editorimages/ForeColor.gif" class="buttonStyle"
        onclick="showColorDiv('ForeColor')" title="Forecolor" />
    <img src="../App_Themes/Default/images/editorimages/InsertOrderedList.gif" class="buttonStyle"
        onclick="doAction('InsertOrderedList')" title="Ordered List" />
    <img src="../App_Themes/Default/images/editorimages/InsertUnorderedList.gif" class="buttonStyle"
        onclick="doAction('InsertUnorderedList')" title="Unordered List" />
    <img src="../App_Themes/Default/images/editorimages/JustifyLeft.gif" class="buttonStyle"
        onclick="doAction('JustifyLeft')" title="Left Align" />
    <img src="../App_Themes/Default/images/editorimages/JustifyCenter.gif" class="buttonStyle"
        onclick="doAction('JustifyCenter')" title="Center Align" />
    <img src="../App_Themes/Default/images/editorimages/JustifyRight.gif" class="buttonStyle"
        onclick="doAction('JustifyRight')" title="Right Align" />
    <img src="../App_Themes/Default/images/editorimages/JustifyFull.gif" class="buttonStyle"
        onclick="doAction('JustifyFull')" title="Justify" />
    <img src="../App_Themes/Default/images/editorimages/Indent.gif" class="buttonStyle"
        onclick="doAction('Indent')" title="Indent" />
    <img src="../App_Themes/Default/images/editorimages/Outdent.gif" class="buttonStyle"
        onclick="doAction('Outdent')" title="Outdent" />
</div>
<div id="FrameDiv" style="display: block">
    <iframe id="editFrame" frameborder="0" name="editFrame" class="frameStyle" onload="setText(this, '<%=hdnEditorContent.ClientID%>')">
    </iframe>
</div>
<asp:HiddenField ID="hdnEditorContent" runat="server" />
