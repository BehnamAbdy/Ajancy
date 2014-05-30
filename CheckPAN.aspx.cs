using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class CheckPAN : System.Web.UI.Page
{
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Public.ConnectionString);
        SqlCommand cmd = new SqlCommand("Check_PAN", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@PAN", SqlDbType.VarChar, 20)).Value = this.txtPAN.Text.Trim();
        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.TinyInt)).Direction = ParameterDirection.Output;
        con.Open();
        cmd.ExecuteScalar();
        byte result = (byte)cmd.Parameters["@Result"].Value;
        con.Close();

        switch (result)
        {
            case 0:
                this.lblMessage.InnerHtml = "کارت سوختی با این شماره PAN یافت نشد";
                break;

            case 1:
                this.lblMessage.InnerHtml = "کارت سوخت مورد نظر نرمال شده چنانچه مورد تاييد اتحاديه ها است در سايت پارس ناوگان قسممت کارت سوخت های مسدود شده ثبت اطلاعات نمايند";
                break;

            case 2:
                this.lblMessage.InnerHtml = "خواهشمندیم برای صدورالمثني این کارت سوخت به دفاتر پليس +10 مراجعه نمايید";
                break;
        }
    }
}