using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;

/// <summary>
/// common의 요약 설명입니다.
/// </summary>
public class common : System.Web.UI.Page
{
    public common()
    {
        //
        // TODO: 여기에 생성자 논리를 추가합니다.
        //

    }

    /// <summary>
    /// 해당 사용자가 정상 사용자인지 확인한다.
    /// </summary>
    public void chkUserId()
    {
        if (Request.Cookies["UserSettings"] == null || Request.Cookies["UserSettings"]["USERID"] == "" || Request.Cookies["UserSettings"]["ExpiresDate"] == null || DateTime.Parse(Request.Cookies["UserSettings"]["ExpiresDate"]) < DateTime.Now)
        // DateTime.Parse(Request.Cookies["UserSettings"]["ExpiresDate"]) < DateTime.Now
        {
            Request.Cookies.Remove("UserSettings");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('잘못된 경로로 접근하였거나 로그인정보를 잃어버렸습니다. 로그인페이지로 이동합니다.','default.aspx?redirectpage=" + Request.Path + "')</script>");
        }
        else
        {
            //현재 접속세션이 유효하면 쿠기를 업데이트하여 세션연장 
            string USERID = Request.Cookies["UserSettings"]["USERID"];

            //Response.Write(USERID);
            string USERNM = string.Empty;
            string USERLVL = string.Empty;

            String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"SELECT DEVEMPNM, EMPLVL
                                  FROM PJT_DEVEMP
                                 WHERE DEVEMPID = '" + USERID + "'";
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    USERNM = reader["DEVEMPNM"].ToString();
                    USERLVL = reader["EMPLVL"].ToString();
                }
                reader.Dispose();
                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            Request.Cookies.Remove("UserSettings");
            setCookie(USERID, USERNM, USERLVL);
        }
    }

    public bool IsAdmin()
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string emplvl = string.Empty;
        emplvl = "0";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string userID = string.Empty;


            userID = Request.Cookies["UserSettings"]["USERID"];


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT EMPLVL
                                  FROM PJT_DEVEMP
                                 WHERE DEVEMPID = '" + userID + "'";
            emplvl = cmd.ExecuteScalar().ToString();
            cmd.Dispose();
        }

        if (emplvl == "1")
            return true;
        else
            return false;
    }

    public bool IsCImember()
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string deptseq = string.Empty;
        deptseq = "0";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string userID = string.Empty;
            userID = Request.Cookies["UserSettings"]["USERID"];
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT DEPTSEQ
                                  FROM PJT_DEVEMP
                                 WHERE DEVEMPID = '" + userID + "'";
            deptseq = cmd.ExecuteScalar().ToString();
            cmd.Dispose();
        }

        if (deptseq == "20713")
            return true;
        else
            return false;
    }

    public bool chk_notice()
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string emplvl = string.Empty;
        emplvl = "0";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string userID = string.Empty;

            userID = Request.Cookies["UserSettings"]["USERID"];
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT EMPLVL
                                  FROM PJT_DEVEMP
                                 WHERE DEVEMPID = '" + userID + "'";
            emplvl = cmd.ExecuteScalar().ToString();
            cmd.Dispose();
        }

        if (emplvl == "1")
            return true;
        else
            return false;
    }

    /// <summary>
    /// 로그인 쿠키를 생성한다.
    /// </summary>
    /// <param name="USERID">USERID</param>
    /// <param name="USERNM">USERNM</param>
    /// <param name="USERLVL">USERLVL</param>
    /// <param name="ExpiresDate">ExpiresDate</param>
    /// 쿠기사제 : Request.Cookies.Remove("UserSettings");
    public void setCookie(String USERID, String USERNM, String USERLVL)
    {
        DateTime ExpiresDate = DateTime.Now.AddDays(2d);
        HttpCookie oCookie = new HttpCookie("UserSettings");
        oCookie["USERID"] = USERID;
        oCookie["USERNM"] = USERNM;
        oCookie["USERLVL"] = USERLVL;

        if (Session["PAGESIZE"] != null)
            oCookie["PAGESIZE"] = Session["PAGESIZE"].ToString();
        oCookie["ExpiresDate"] = string.Format("{0:u}", ExpiresDate);
        oCookie.Expires = ExpiresDate;
        Response.Cookies.Add(oCookie);
    }
    /*
    public void setCookiePageSize(String PageSize)
    {

        if (Request.Cookies["C1"] != null && Request.Cookies["C1"]["PAGESIZE"] != null)
        {
            Request.Cookies.Remove("C1");
        }
        HttpCookie oCookie = new HttpCookie("C1");
        oCookie["PAGESIZE"] = PageSize;
        oCookie.Expires =  DateTime.Now.AddMonths(1);
        Response.Cookies.Add(oCookie);
    }
    */
    public void getUserInfo(string USERID, string USERPW, out string USERNM, out string USERLVL)
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string emplvl = string.Empty;
        emplvl = "0";
        USERNM = "";
        USERLVL = "";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = @"SELECT DEVEMPNM, EMPLVL
                                  FROM PJT_DEVEMP
                                 WHERE DEVEMPID = '" + USERID + "' AND DEVEMPPW = '" + USERPW + "'";
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                USERNM = reader["DEVEMPNM"].ToString();
                USERLVL = reader["EMPLVL"].ToString();
            }
            reader.Dispose();
            reader.Close();
            cmd.Dispose();
            conn.Close();
        }
    }

    /// <summary>
    /// DropDown list를 받은 dataset으로 만들어줍니다.
    /// </summary>
    /// <param name="cboList">대상 Dropdown list</param>
    /// <param name="ds">dataset</param>
    /// <param name="textField">text column명</param>
    /// <param name="valueField">value column명</param>
    public void makeSelectList(DropDownList cboList, DataSet ds, string textField, string valueField)
    {
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ListItem oListItem = new ListItem(ds.Tables[0].Rows[i][textField].ToString(), ds.Tables[0].Rows[i][valueField].ToString());
            cboList.Items.Add(oListItem);
        }
    }

    /// <summary>
    /// DropDown list를 받은 datatable으로 만들어줍니다
    /// </summary>
    /// <param name="cboList">대상 Dropdown list</param>
    /// <param name="dt">DataTable</param>
    /// <param name="textField">text column명</param>
    /// <param name="valueField">value column명</param>
    public void makeSelectList(DropDownList cboList, DataTable dt, string textField, string valueField)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ListItem oListItem = new ListItem(dt.Rows[i][textField].ToString(), dt.Rows[i][valueField].ToString());
            cboList.Items.Add(oListItem);
        }
    }


    /// <summary>
    /// 지정된 sql을 받아서 Dataset을 넘겨줍니다.
    /// </summary>
    /// <param name="sql">조회를 원하는 쿼리를 입력합니다.</param>
    /// <returns></returns>
    public DataSet GetData(string sql)
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        DataSet ds = new DataSet();

        //Response.Write( sql );

        try
        {
            // Connect to the database and run the query.
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            // Fill the DataSet.
            adapter.Fill(ds);
            adapter.Dispose();
            conn.Dispose();
            conn.Close();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
            // The connection failed. Display an error message.
            //Message.Text = "Unable to connect to the database.";
        }

        return ds;
    }

    /// <summary>
    /// 지정된 sql을 받아서 Dataset을 넘겨줍니다.
    /// </summary>
    /// <param name="sql">조회를 원하는 쿼리를 입력합니다.</param>
    /// <returns></returns>
    public DataTable GetDataTable(string sql)
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        //DataTable dt = new DataTable("dt");
        DataTable dt = new DataTable();
        try
        {
            // Connect to the database and run the query.
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            SqlDataReader reader = cmd.ExecuteReader();

            DataRow oRow;
            DataColumn oColumn;

            if (reader.HasRows == true)
            {
                for (int cnt = 0; cnt < reader.FieldCount; cnt++)
                {
                    oColumn = new DataColumn();
                    oColumn.ColumnName = reader.GetName(cnt);
                    oColumn.DataType = reader.GetFieldType(cnt);
                    dt.Columns.Add(oColumn);
                }

                while (reader.Read())
                {
                    oRow = dt.NewRow();
                    for (int cnt = 0; cnt < reader.FieldCount; cnt++)
                    {
                        oRow[cnt] = reader[cnt];
                    }
                    dt.Rows.Add(oRow);
                }
            }
            reader.Dispose();
            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();
        }
        catch (Exception ex)
        {
            // The connection failed. Display an error message.
            //Message.Text = "Unable to connect to the database.";
            //Response.Write(ex.StackTrace + "<BR>");
            //Response.Write(ex.Message + "<BR>");
        }

        return dt;
    }





}
