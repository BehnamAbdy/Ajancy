<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Time.ascx.cs" Inherits="UserControl_Time" %>
<script type="text/javascript">

    function getCorrectTime(t) {
        var result = '';
        for (var i = 0; i < 5; i++) {
            if (t.toString().charAt(i) == '_')
                result = result + '0';
            else
                result = result + t.toString().charAt(i);
        }
        return result;
    }

    function timeValidate(control) {
        if (control.value == '__' + control.value.charAt(2) + '__')
            return;
        control.value = getCorrectTime(control.value);

        var str = control.value.split(control.value.charAt(2));
        var hour = parseFloat(str[0]);
        var minute = parseFloat(str[1]);
        if (hour > 23 || minute > 59) {
            alert('ساعت معتبر نمی باشد');
            control.value = '__:__';
            control.focus();
        }
    }

</script>
<asp:TextBox ID="txtTime" runat="server" dir="ltr" onblur="javascript:timeValidate(this)"
    AutoCompleteType="Disabled" CssClass="textbox"></asp:TextBox>
<asp:Label ID="lblStar" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
<asp:CompareValidator ID="rfvTime" runat="server" ErrorMessage="*" ControlToValidate="txtTime"
    Operator="NotEqual" ValueToCompare="__:__"></asp:CompareValidator>
<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender" runat="server" Mask="99:99"
    MaskType="None" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" TargetControlID="txtTime">
</ajaxToolkit:MaskedEditExtender>
