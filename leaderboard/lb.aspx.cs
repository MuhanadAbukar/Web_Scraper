using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace leaderboard
{ 
    public partial class lb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string connstr = "Data Source=DESKTOP-QJBSGQ4\\MSSQLSERVER01;Initial Catalog=mslearnEXP;Persist Security Info=True;User ID=sa;Password=muhanad123";
                SqlConnection conn = new SqlConnection(connstr);
                var s = new SqlCommand("select * from users order by levels desc ,[exp] desc, expoutof asc");
                s.Connection = conn;
                conn.Open();
                var s2 = new DataTable();
                s2.Load(s.ExecuteReader());
                conn.Close();
                GridView1.DataSource = s2;
                GridView1.DataBind();
            }
        }
    }
}