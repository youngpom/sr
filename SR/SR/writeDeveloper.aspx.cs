using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Drawing;
using System.Data.SqlClient;

public partial class writeDeveloper : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ///사용자 로그인여부를 확인합니다.
        chkUserId();

        if (!IsPostBack)
        {
            ///직위list를 만들어준다.
            string jwcdsql = @"select comcd, comcdnm from pjt_comcd where upcd = '10' order by comcd";
            DataTable jwcddt = GetDataTable(jwcdsql);
            if (jwcddt.Rows.Count > 0)
            {
                makeSelectList(cboUserPosition, jwcddt, "comcdnm", "comcd");
            }
            jwcddt.Clear();
            jwcddt.Dispose();

            ///선택된 사용자가 있는 경우 해당 사용자를 조회해서 data를 display
            if (!string.IsNullOrEmpty(Request.QueryString["devempid"]) && Request.QueryString["devempid"].ToString() != "")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string empsql = @"select devempid, devempnm, devemppw, jwcd, email, oftel, hptel, emplvl
                                from pjt_devemp
                               where devempid = '" + Request.QueryString["devempid"].ToString() + "'";
                // Connect to the database and run the query.
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = empsql;
                cmd.Parameters.Add("@devempid", SqlDbType.VarChar, 10).Value = Request.QueryString["devempid"].ToString();
                SqlDataReader reader = cmd.ExecuteReader();

                TextBoxMode passMode = txtUserPassword.TextMode;
                TextBoxMode textMode = txtUserID.TextMode;

                if (reader.Read())
                {
                    txtUserID.Text = reader["devempid"].ToString();
                    hdnUserID.Value = reader["devempid"].ToString();
                    txtUserName.Text = reader["devempnm"].ToString();

                    txtUserPassword.TextMode = textMode;
                    txtUserPasswordConfirm.TextMode = textMode;

                    txtUserPassword.Text = reader["devemppw"].ToString();
                    txtUserPasswordConfirm.Text = reader["devemppw"].ToString();

                    txtUserPassword.TextMode = passMode;
                    txtUserPasswordConfirm.TextMode = passMode;

                    txtUserEmail.Text = reader["email"].ToString();
                    txtUserTel.Text = reader["oftel"].ToString();
                    txtUserMobile.Text = reader["hptel"].ToString();
                    if (reader["emplvl"].ToString() == "1")
                    {
                        chkIsAdmin.Checked = true;
                    }
                    cboUserPosition.SelectedValue = reader["jwcd"].ToString();
                    btnSave.Text = "수정";
                    btnDelete.Visible = true;
                }
                cmd.Dispose();
                reader.Dispose();
                reader.Close();
                conn.Dispose();
                conn.Close();
            }
            else
            {
                btnDelete.Visible = false;
            }
            //DB에서 개발자 정보를 가져와서 listDeveloper에 입력한다.
            listQuery();


            //관리자가 아니였을때의 처리
            if (Request.Cookies["UserSettings"] != null && Request.Cookies["UserSettings"]["USERLVL"] != "1")
            {
                //listDeveloper.Visible = false; // 개발자목록 비노출
                listPanel.Visible = false; // 개발자목록 비노출
                txtUserID.ReadOnly = true; // UserID ReadOnly
                lblIsAdmin.Visible = false;// 관리자체크항목 비노출
                chkIsAdmin.Visible = false;// 관리자체크항목 비노출
                btnDelete.Visible = false;// 삭제버튼 비노출
                btnReset.Visible = false; //초기화버튼 비노출
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string userID = string.Empty;
        string updateUserID = string.Empty;
        string userName = string.Empty;
        string userPassword = string.Empty;
        string userPosition = string.Empty;
        string userEmail = string.Empty;
        string userTel = string.Empty;
        string userMobile = string.Empty;
        string isAdmin = string.Empty;

        userID = txtUserID.Text;
        updateUserID = hdnUserID.Value;
        userName = txtUserName.Text;
        userPassword = txtUserPassword.Text;
        userPosition = cboUserPosition.Items[cboUserPosition.SelectedIndex].Value;
        userEmail = txtUserEmail.Text;
        userTel = txtUserTel.Text;
        userMobile = txtUserMobile.Text;
        isAdmin = chkIsAdmin.Checked == true ? "1" : "0";

        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        if (string.IsNullOrEmpty(updateUserID) || updateUserID == "")
        {
            //신규등록
            querystring = @"insert into pjt_devemp (devempid, devempnm, devemppw, jwcd, email, oftel, hptel, emplvl, regdt, chgdt) 
                            values ('" + userID + "', '" + userName + "', '" + userPassword + "', '" + userPosition + "', '" + userEmail + "', '" + userTel + "', '" + userMobile + "', '" + isAdmin + "', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";
        }
        else
        {
            if (updateUserID == Request.Cookies["UserSettings"]["USERID"])
            {
                //개인정보 수정
                querystring = @"update pjt_devemp 
                               set devempnm = '" + userName + @"'
                                 , devemppw = '" + userPassword + @"'
                                 , jwcd = '" + userPosition + @"'
                                 , email = '" + userEmail + @"'
                                 , oftel = '" + userTel + @"'
                                 , hptel = '" + userMobile + @"'
                                 , emplvl = '" + isAdmin + @"'
                                 , chgdt = CURRENT_TIMESTAMP
                             where devempid = '" + userID + "'";
            }
            else
            {
                //개인정보 수정
                querystring = @"update pjt_devemp 
                               set devempnm = '" + userName + @"'
                                 , devemppw = '" + userPassword + @"'
                                 , jwcd = '" + userPosition + @"'
                                 , email = '" + userEmail + @"'
                                 , oftel = '" + userTel + @"'
                                 , hptel = '" + userMobile + @"'
                                 , emplvl = '" + isAdmin + @"'
                                 , chgdt = CURRENT_TIMESTAMP
                             where devempid = '" + updateUserID + "'";
            }
        }

        //HttpContext.Current.Response.Write(querystring);
        //이게 insert인지 update건인지 확인해서 처리
        SqlConnection conn = new SqlConnection(connectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = querystring;
        cmd.ExecuteNonQuery();
        cmd.Dispose();
        conn.Dispose();
        conn.Close();

        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.','writeDeveloper.aspx?devempid=" + userID + "')</script>");

    }

    /// <summary>
    /// row가 생성될때마다 타게되는 event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void listDeveloper_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = "개발자ID";
            e.Row.Cells[0].Width = Unit.Parse("80");
            e.Row.Cells[1].Text = "성명";
            e.Row.Cells[1].Width = Unit.Parse("80");
            e.Row.Cells[2].Text = "직급";
            e.Row.Cells[2].Width = Unit.Parse("50");
            e.Row.Cells[3].Text = "일반전화";
            e.Row.Cells[3].Width = Unit.Parse("100");
            e.Row.Cells[4].Text = "휴대전화";
            e.Row.Cells[4].Width = Unit.Parse("100");
            e.Row.Cells[5].Text = "이메일";
            e.Row.Cells[5].Width = Unit.Parse("150");
            e.Row.Cells[6].Text = "관리";
            e.Row.Cells[6].Width = Unit.Parse("50");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onClick"] = "location.href='writeDeveloper.aspx?devempid=" + DataBinder.Eval(e.Row.DataItem, "devempid") + "'";
            e.Row.Attributes["onmouseover"] = "this.className='trMouseOver'";
            e.Row.Attributes["onmouseout"] = "this.className='trMouseOut'";
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string userID = string.Empty;
        string updateUserID = string.Empty;

        userID = txtUserID.Text;
        updateUserID = hdnUserID.Value;

        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        //개인정보 수정
        querystring = @"delete from pjt_devemp 
                         where devempid = '" + userID + "'";

        //이게 insert인지 update건인지 확인해서 처리
        SqlConnection conn = new SqlConnection(connectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = querystring;
        cmd.ExecuteNonQuery();
        cmd.Dispose();
        conn.Dispose();
        conn.Close();

        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.','writeDeveloper.aspx')</script>");

    }

    private void listQuery()
    {
        //DB에서 개발자 정보를 가져와서 listDeveloper에 입력한다.
        string sql = @"SELECT devempid, devempnm, comcdnm, oftel, hptel, email, emplvl, case when emplvl = '1' then '관리자' else '' end 
                         FROM pjt_devemp, pjt_comcd
                        WHERE pjt_devemp.jwcd = pjt_comcd.comcd
                          AND pjt_comcd.upcd = '10'
                          AND devempnm like '%" + txtSearch.Text + "%'";
        // Run the query and bind the resulting DataSet
        // to the GridView control.
        DataSet ds = GetData(sql);
        if (ds.Tables.Count > 0)
        {
            listDeveloper.DataSource = ds;
            listDeveloper.DataBind();

        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        ds.Dispose();
    }

    protected void listDeveloper_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    protected void listDeveloper_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listDeveloper.PageIndex = e.NewPageIndex;
        listQuery();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        listQuery();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {

        txtUserID.Text = "";
        hdnUserID.Value = "";
        txtUserName.Text = "";
        txtUserPassword.Text = "";
        //cboUserPosition.Items[cboUserPosition.SelectedIndex].Value;
        txtUserEmail.Text = "";
        txtUserTel.Text = "";
        txtUserMobile.Text = "";
        chkIsAdmin.Checked = false;
        btnSave.Text = "등록";
    }
}
