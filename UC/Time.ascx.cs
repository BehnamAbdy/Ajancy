using System;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class UserControl_Time : System.Web.UI.UserControl
{
    public bool Enabled
    {
        set
        {
            this.txtTime.Enabled = value;
            rfvTime.Enabled = value;
        }
        get { return this.txtTime.Enabled; }
    }

    public bool Required
    {
        get
        {
            return rfvTime.Enabled;
        }
        set
        {
            rfvTime.Enabled = value;
            lblStar.Visible = value;
        }
    }

    public string Text
    {
        get { return txtTime.Text.Trim(); }
        set { txtTime.Text = value; }
    }

    public Unit Width
    {
        set { this.txtTime.Width = value; }
        get { return this.txtTime.Width; }
    }

    public bool EnableValidation
    {
        set { this.rfvTime.Enabled = value; }
        get { return this.rfvTime.Enabled; }
    }

    public string ValidationGroupRequiredFieldValidator
    {
        get { return this.rfvTime.ValidationGroup; }
        set { this.rfvTime.ValidationGroup = value; }
    }

    public string ValidationGroupTextBox
    {
        get { return this.txtTime.ValidationGroup; }
        set { this.txtTime.ValidationGroup = value; }
    }

    public TimeSpan Time
    {
        get
        {
            if (IsTime)
                return TimeSpan.Parse(string.Format("{0}:00", txtTime.Text));
            return TimeSpan.Zero;
        }
        set
        {
            txtTime.Text = value.ToString().Substring(0, 5);
        }
    }

    public TimeSpan? NullableTime
    {
        get
        {
            if (IsTime)
                return TimeSpan.Parse(string.Format("{0}:00", txtTime.Text));
            return null;
        }
        set
        {
            txtTime.Text = value.ToString().Substring(0, 5);
        }
    }

    public override void Focus()
    {
        txtTime.Focus();
    }

    public string ClientOnChange
    {
        set { txtTime.Attributes.Add("onchange", value); }
    }

    public bool IsTime
    {
        get
        {
            Regex regex = new Regex(@"\d{2}(:|.)\d{2}");
            return regex.IsMatch(txtTime.Text);
        }
    }
}
