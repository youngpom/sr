using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

public partial class writeCommon : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ///사용자 로그인여부를 확인합니다.
        chkUserId();

        if (IsAdmin() == false)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('권한이 없습니다.','readTask.aspx')</script>");
        }

        // Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('" + Request["cboSearch"] +Request["txtSearch"] + "','writeDeveloper.aspx')</script>");
        txtUpcd.Text = Request["txtUpcd"];

        cboUpcdNm.SelectedValue = Request["cboUpcdNm"];
        txtComcdNm.Text = Request["txtComcdNm"];
        txtComcd.Text = Request["txtComcd"];
        if (Request["chkIsUse"] == "1")
        { chkIsUse.Checked = true; }
        txtOptE.Text = Request["txtOptE"];
        txtOptA.Text = Request["txtOptA"];
        txtOptB.Text = Request["txtOptB"];
        txtOptC.Text = Request["txtOptC"];
        txtOptD.Text = Request["txtOptD"];
        cboSearch.SelectedValue = Request["cboSearch"];
        txtSearch.Text = Request["txtSearch"];

        //btnSave.Text = "수정";
        //hdnSaveType.Value = "update";
        //btnDelete.Visible = true;


        if (!IsPostBack)
        {
            ///상위코드list를 만들어준다.
            string upcdsql = @"select comcd, ' ( ' + isnull(comcd, '') + ' ) ' +  isnull(comcdnm, '')  as comcdnm from pjt_comcd where upcd = 'A' order by UPcd";
            DataSet upcds = GetData(upcdsql);
            if (upcds.Tables.Count > 0)
            {
                cboUpcdNm.DataSource = upcds;
                cboUpcdNm.DataTextField = "comcdnm";
                cboUpcdNm.DataValueField = "comcd";
                cboUpcdNm.DataBind();
            }
            upcds.Dispose();
            txtUpcd.Text = cboUpcdNm.Items[cboUpcdNm.SelectedIndex].Value;

            //ListComcd GridView 출력
            ListComcdBind();
        }
    }


    /// <summary>
    /// 지정된 sql을 받아서 Dataset을 넘겨줍니다.
    /// </summary>
    /// <param name="sql">조회를 원하는 쿼리를 입력합니다.</param>
    /// <returns></returns>
    private DataSet GetData(string sql)
    {
        // Retrieve the connection string stored in the Web.config file.
        String connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        DataSet ds = new DataSet();

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

            //return ds.Tables[0].DefaultView;//추가함
        }
        catch (Exception ex)
        {
            // The connection failed. Display an error message.
            //Message.Text = "Unable to connect to the database.";
        }

        return ds;
    }


    //등록 버튼 클릭시
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Upcd = string.Empty;
        string Comcd = string.Empty;
        string ComcdNm = string.Empty;
        string Srt = string.Empty;
        string Use01 = string.Empty;
        string OptE = string.Empty;
        string OptA = string.Empty;
        string OptB = string.Empty;
        string OptC = string.Empty;
        string OptD = string.Empty;
        string UserId = Request.Cookies["UserSettings"]["USERID"];

        Upcd = txtUpcd.Text;
        //Upcd = cboUpcdNm.SelectedValue;
        Comcd = txtComcd.Text;
        ComcdNm = txtComcdNm.Text;
        Srt = txtSrt.Text;
        Use01 = chkIsUse.Checked == true ? "1" : "0";
        OptA = txtOptA.Text;
        OptB = txtOptB.Text;
        OptC = txtOptC.Text;
        OptD = txtOptD.Text;
        OptE = txtOptE.Text;


        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;
        querystring = @"insert into pjt_comcd ( upcd          , comcd          , comcdnm          , srt          , use01          , opte          , opta          , optb          , optc          , optd          , regemp          , chgemp          , regdt            , chgdt) 
                                       values ( '" + Upcd + "', '" + Comcd + "', '" + ComcdNm + "', '" + Srt + "', '" + Use01 + "', '" + OptE + "', '" + OptA + "', '" + OptB + "', '" + OptC + "', '" + OptD + "', '" + UserId + "', '" + UserId + "', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";

        try
        {
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
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("<script language='javascript' type='text/javascript'>alert('DataBase처리중 오류가 있습니다.');history.back(-1);</script>");
        }
        ListComcdBind();
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.btnSave_Click','writeDeveloper.aspx')</script>");
    }

    //수정버튼 클릭시
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string Upcd = string.Empty;
        string Comcd = string.Empty;
        string ComcdNm = string.Empty;
        string Srt = string.Empty;
        string Use01 = string.Empty;
        string OptE = string.Empty;
        string OptA = string.Empty;
        string OptB = string.Empty;
        string OptC = string.Empty;
        string OptD = string.Empty;
        string UserId = Request.Cookies["UserSettings"]["USERID"];

        Upcd = txtUpcd.Text;
        //Upcd = cboUpcdNm.SelectedValue;
        Comcd = txtComcd.Text;
        ComcdNm = txtComcdNm.Text;
        Srt = txtSrt.Text;
        Use01 = chkIsUse.Checked == true ? "1" : "0";
        OptA = txtOptA.Text;
        OptB = txtOptB.Text;
        OptC = txtOptC.Text;
        OptD = txtOptD.Text;
        OptE = txtOptE.Text;


        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        //개인정보 수정
        querystring = @"update pjt_comcd 
                               set upcd = '" + Upcd + @"'
                                 , comcdnm = '" + ComcdNm + @"'
                                 , srt = '" + Srt + @"'
                                 , use01 = '" + Use01 + @"'
                                 , opte = '" + OptE + @"'
                                 , opta = '" + OptA + @"'
                                 , optb = '" + OptB + @"'
                                 , optc = '" + OptC + @"'
                                 , optd = '" + OptD + @"'
                                 , chgemp = '" + UserId + @"'
                                 , chgdt = GETDATE()
                             where comcd = '" + Comcd + "' and upcd = '" + Upcd + "'";

        try
        {

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
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(querystring);
        }

        ListComcdBind();

        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.btnSave_Click','writeDeveloper.aspx')</script>");
    }


    // 삭제버튼 클릭시
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string Comcd = string.Empty;
        string updateComcd = string.Empty;

        Comcd = txtComcd.Text;
        //updateComcd = hdnUserID.Value;

        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        //개인정보 수정
        querystring = @"delete from pjt_comcd
                         where comcd = '" + Comcd + "'";

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

        ListComcdBind();

        txtUpcd.Text = "";
        txtComcdNm.Text = "";
        txtComcd.Text = "";
        txtOptE.Text = "";
        txtOptA.Text = "";
        txtOptB.Text = "";
        txtOptC.Text = "";
        txtOptD.Text = "";
        txtSrt.Text = "";

        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.btnDelete_Click','writeDeveloper.aspx')</script>");
    }

    /// <summary>
    /// row가 생성될때마다 타게되는 event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void listComcd_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {

            e.Row.Cells[0].Text = "UPCD";
            e.Row.Cells[0].Width = Unit.Parse("50");
            e.Row.Cells[1].Text = "COMCD";
            e.Row.Cells[1].Width = Unit.Parse("50");
            e.Row.Cells[2].Text = "VALUE";
            e.Row.Cells[2].Width = Unit.Parse("100");
            e.Row.Cells[3].Text = "순서";
            e.Row.Cells[3].Width = Unit.Parse("50");
            e.Row.Cells[4].Text = "사용";
            e.Row.Cells[4].Width = Unit.Parse("50");
            e.Row.Cells[5].Text = "옵션E";
            e.Row.Cells[5].Width = Unit.Parse("190");
            e.Row.Cells[6].Text = "옵션A";
            e.Row.Cells[6].Width = Unit.Parse("100");
            e.Row.Cells[7].Text = "옵션B";
            e.Row.Cells[7].Width = Unit.Parse("100");
            e.Row.Cells[8].Text = "옵션C";
            e.Row.Cells[8].Width = Unit.Parse("100");
            e.Row.Cells[9].Text = "옵션D";
            e.Row.Cells[9].Width = Unit.Parse("100");

            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;

        }
    }


    protected void listComcd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > -1)
        {
            string rowUpcd = string.Empty;
            string rowComcd = string.Empty;
            string rowComcdNm = string.Empty;
            string rowSrt = string.Empty;
            string rowUse01 = string.Empty;
            string rowOptE = string.Empty;
            string rowOptA = string.Empty;
            string rowOptB = string.Empty;
            string rowOptC = string.Empty;
            string rowOptD = string.Empty;

            rowComcd = DataBinder.Eval(e.Row.DataItem, "Comcd").ToString();
            rowUpcd = DataBinder.Eval(e.Row.DataItem, "Upcd").ToString();
            rowComcdNm = DataBinder.Eval(e.Row.DataItem, "ComcdNm").ToString();
            rowSrt = DataBinder.Eval(e.Row.DataItem, "Srt").ToString();
            rowUse01 = DataBinder.Eval(e.Row.DataItem, "Use01").ToString();
            rowOptE = DataBinder.Eval(e.Row.DataItem, "OptE").ToString();
            rowOptA = DataBinder.Eval(e.Row.DataItem, "OptA").ToString();
            rowOptB = DataBinder.Eval(e.Row.DataItem, "OptB").ToString();
            rowOptC = DataBinder.Eval(e.Row.DataItem, "OptC").ToString();
            rowOptD = DataBinder.Eval(e.Row.DataItem, "OptD").ToString();

            e.Row.Attributes["onClick"] = "ShowRow('" + rowUpcd + "','" + rowComcd + "','" + rowComcdNm + "','" + rowSrt + "','" + rowUse01 + "','" + rowOptE + "','" + rowOptA + "','" + rowOptB + "','" + rowOptC + "','" + rowOptD + "');";
            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";
            //this.style.cursor='pointer'; this.style.cursor='hand';

            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            if (e.Row.Cells[4].Text == "1") e.Row.Cells[4].Text = "Y"; else e.Row.Cells[4].Text = "N";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ListComcdBind();
    }

    private void ListComcdBind()
    {
        //DB에서 공통코드 정보를 가져와서 listComcd에 입력한다.
        string sql = @"select upcd, comcd, comcdnm, srt, use01, opte, opta, optb, optc, optd
                                from pjt_comcd where " + cboSearch.SelectedItem.Value.ToString() + " like '%" + txtSearch.Text + "%'";

        //HttpContext.Current.Response.Write(sql);
        // Run the query and bind the resulting DataSet
        // to the GridView control.
        DataSet ds = GetData(sql);
        if (ds.Tables.Count > 0)
        {
            listComcd.DataSource = ds;
            listComcd.DataBind();

            listComcd.AllowPaging = true;

        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        ds.Dispose();
    }

    protected void listComcd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listComcd.PageIndex = e.NewPageIndex;
        ListComcdBind();
    }

    protected void listComcd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}