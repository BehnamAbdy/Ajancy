<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Date.ascx.cs" Inherits="UC_Date" %>

<script type="text/javascript" language="javascript">

    function validate(control) {
        if (control.value == '____' + control.value.charAt(4) + '__' + control.value.charAt(4) + '__')
            return;
        control.value = control.value.replace(/\_/g, '0');
        var str = control.value.split(control.value.charAt(4));
        var day = parseFloat(str[2]);
        var month = parseFloat(str[1]);
        var year = parseFloat(str[0]);
        if (day > 31 || day < 1 || month > 12 || month < 1 || year > 1500 || year < 1300) {
            alert('تاریخ معتبر نمی باشد');
            control.value = '____' + control.value.charAt(4) + '__' + control.value.charAt(4) + '__';
            control.focus();
        }
    }

    function showCalander(control) {
        displayDatePicker(control.id.replace('btnCalander', 'txtDate'), control);
        return false;
    }    

</script>

<div style="line-height: 27px; vertical-align: middle; width: 150px; text-align: right;">
    <asp:TextBox ID="txtDate" runat="server" CssClass="textbox-date" AutoCompleteType="Disabled"
        onblur="javascript:validate(this);"></asp:TextBox>
    <asp:Button ID="btnCalander" runat="server" OnClientClick="javascript:return showCalander(this);"
        CssClass="button-calander" TabIndex="-1" />
    <asp:RegularExpressionValidator ID="rfvDate" runat="server" ErrorMessage="*" ControlToValidate="txtDate"
        ValidationExpression="\d{4}(/|-)\d{2}(/|-)\d{2}"></asp:RegularExpressionValidator>
    <ajaxToolkit:MaskedEditExtender ID="meeMaskEdit" runat="server" Mask="9999/99/99"
        MaskType="None" ClearMaskOnLostFocus="false" TargetControlID="txtDate" UserDateFormat="YearMonthDay"
        ClearTextOnInvalid="false">
    </ajaxToolkit:MaskedEditExtender>
</div>
