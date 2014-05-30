using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;


public partial class UC_Date : System.Web.UI.UserControl
{
    public string Text
    {
        get
        {
            if (txtDate.Text.Contains('_'))
                return "";
            return txtDate.Text;
        }
        set { txtDate.Text = value; }
    }

    public bool Required
    {
        get { return rfvDate.Enabled; }
        set { rfvDate.Enabled = value; }
    }

    public string CssClass
    {
        get
        {
            return txtDate.CssClass;
        }
        set
        {
            txtDate.CssClass = value;
        }
    }

    public override void Focus()
    {
        rfvDate.Focus();
    }

    public bool Enabled
    {
        get
        {
            return txtDate.Enabled;
        }
        set
        {
            txtDate.Enabled = value;
            btnCalander.Enabled = value;
        }
    }

    public Unit Width
    {
        get { return txtDate.Width; }
        set { txtDate.Width = value; }
    }

    public void Clear()
    {
        txtDate.Text = string.Format("____{0}__{0}__", Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator);
    }

    public bool HasDate
    {
        get
        {
            Regex regex = new Regex(@"\d{4}(/|_)\d{2}(/|_)\d{2}");
            return regex.IsMatch(txtDate.Text);
        }
    }

    public DateTime? GeorgianDate
    {
        get
        {
            if (HasDate)
            {
                short year = Convert.ToInt16(txtDate.Text.Substring(0, 4));
                short month = Convert.ToInt16(txtDate.Text.Substring(5, 2));
                short day = Convert.ToInt16(txtDate.Text.Substring(8, 2));
                PersianCalendar pc = new PersianCalendar();
                return pc.ToDateTime(year, month, day, 0, 0, 0, 0).Date;
            }
            else
            {
                return null;
            }
        }
    }

    public DateTime? GeorgianDateTime
    {
        get
        {
            if (HasDate)
            {
                short year = Convert.ToInt16(txtDate.Text.Substring(0, 4));
                short month = Convert.ToInt16(txtDate.Text.Substring(5, 2));
                short day = Convert.ToInt16(txtDate.Text.Substring(8, 2));
                PersianCalendar pc = new PersianCalendar();
                return pc.ToDateTime(year, month, day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            }
            else
            {
                return null;
            }
        }
        set
        {
            txtDate.Text = Public.ToPersianDate(value);
        }
    }

    public void SetDate(DateTime georgianDate)
    {
        if (georgianDate.Year == 1)
        {
            this.txtDate.Text = null;
        }
        else
        {
            this.txtDate.Text = Public.ToPersianDate(georgianDate);
        }
    }

    public void SetDate(DateTime? georgianDate)
    {
        if (georgianDate == null)
        {
            this.txtDate.Text = null;
        }
        else
        {
            this.txtDate.Text = Public.ToPersianDate(georgianDate);
        }
    }
}




