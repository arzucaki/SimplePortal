using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace BasitPortal
{
    public partial class Login : System.Web.UI.Page
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();

        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            con.ConnectionString = "data source =2.200.163.33; initial catalog = JDAQuality; persist security info = True; user id = sa; password = dhl@1234; MultipleActiveResultSets = True; App = EntityFramework";
            con.Open();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "Select * from [User] where userName='"+txtUserName.Text+"'";
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            sda.Fill(ds, "User");
            if (ds.Tables[0].Rows.Count>0)
            {
                Response.Redirect("Home.aspx?User="+ txtUserName.Text);
            }
            else
            {
                lblError.Text = "Kullanıcı mecvut değil!";
            }
        }
    }
}