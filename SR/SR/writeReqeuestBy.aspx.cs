
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
// Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('" + Request["cboSearch"] +Request["txtSearch"] + "','writeDeveloper.aspx')</script>");

public partial class writeReqeuestBy : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ///사용자 로그인여부를 확인합니다.
        chkUserId();

        if (IsAdmin() == false)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('권한이 없습니다.','readTask.aspx')</script>");
        }

        txtReqempSeq.Text = Request["txtReqempSeq"];
        txtReqempNm.Text = Request["txtReqempNm"];
        cboJwcdNm.SelectedValue = Request["cboJwcdNm"];

        txtDeptseq.Text = Request["txtDeptseq"];
        txtDeptseqNm.Text = Request["txtDeptseqNm"];
        txtOftel.Text = Request["txtOftel"];
        txtMbtel.Text = Request["txtMbtel"];
        txtEmail.Text = Request["txtEmail"];

        cboSearch.SelectedValue = Request["cboSearch"];
        txtSearch.Text = Request["txtSearch"];

        if (!IsPostBack)
        {
            ///상위코드list를 만들어준다.
            string upcdsql = @"select comcd, ' ( ' + isnull(comcd, '') + ' ) ' +  isnull(comcdnm, '')  as comcdnm from pjt_comcd where upcd = '10' order by UPcd";
            DataTable upcdt = GetDataTable(upcdsql);
            if (upcdt.Rows.Count > 0)
            {
                makeSelectList(cboJwcdNm, upcdt, "comcdnm", "comcd");
            }
            upcdt.Clear();
            upcdt.Dispose();

            //ListComcd GridView 출력
            ListReqempBind();
        }
    }

    //등록 버튼 클릭시
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string ReqempSeq = string.Empty;
        string ReqempNm = string.Empty;
        string Jwcd = string.Empty;
        string Deptseq = string.Empty;
        string DeptseqNm = string.Empty;
        string Oftel = string.Empty;
        string Mbtel = string.Empty;
        string Email = string.Empty;

        string UserId = Request.Cookies["UserSettings"]["USERID"];
        ReqempSeq = txtReqempSeq.Text;
        ReqempNm = txtReqempNm.Text;
        Jwcd = cboJwcdNm.SelectedValue;

        Deptseq = txtDeptseq.Text;
        DeptseqNm = txtDeptseqNm.Text;
        Oftel = txtOftel.Text;
        Mbtel = txtMbtel.Text;
        Email = txtEmail.Text;


        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;
        querystring = @"insert into pjt_reqemp ( reqempnm          , jwcd          , oftel          , mbtel          , email          , deptseq          , regemp          , chgemp          , regdt            , chgdt) 
                                        values ( '" + ReqempNm + "', '" + Jwcd + "', '" + Oftel + "', '" + Mbtel + "', '" + Email + "', '" + Deptseq + "', '" + UserId + "', '" + UserId + "', GETDATE(), GETDATE())";


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

        ListReqempBind();
    }

    //수정버튼 클릭시
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string ReqempSeq = string.Empty;
        string ReqempNm = string.Empty;
        string Jwcd = string.Empty;
        string Deptseq = string.Empty;
        string DeptseqNm = string.Empty;
        string Oftel = string.Empty;
        string Mbtel = string.Empty;
        string Email = string.Empty;
        string UserId = Request.Cookies["UserSettings"]["USERID"];

        ReqempSeq = txtReqempSeq.Text;
        ReqempNm = txtReqempNm.Text;
        Jwcd = cboJwcdNm.Items[cboJwcdNm.SelectedIndex].Value;
        Deptseq = txtDeptseq.Text;
        DeptseqNm = txtDeptseqNm.Text;
        Oftel = txtOftel.Text;
        Mbtel = txtMbtel.Text;
        Email = txtEmail.Text;

        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        //정보수정
        querystring = @"update pjt_reqemp
                               set ReqempNm = '" + ReqempNm + @"'
                                 , Jwcd = '" + Jwcd + @"'
                                 , Oftel = '" + Oftel + @"'
                                 , Mbtel = '" + Mbtel + @"'
                                 , Email = '" + Email + @"'
                                 , Deptseq = '" + Deptseq + @"'
                                 , chgemp = '" + UserId + @"'
                                 , chgdt = GETDATE()
                             where ReqempSeq = '" + ReqempSeq + "'";

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

        ListReqempBind();

    }


    // 삭제버튼 클릭시
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string ReqempSeq = string.Empty;
        ReqempSeq = txtReqempSeq.Text;


        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        //개인정보 수정
        querystring = @"delete from pjt_reqemp
                         where ReqempSeq = " + ReqempSeq;

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

        ListReqempBind();

        btnReset_Click(sender, e);
        /*
        txtReqempSeq.Text = "";
        txtReqempNm.Text = "";
        txtDeptseq.Text = "";
        txtDeptseqNm.Text = "";
        txtOftel.Text = "";
        txtMbtel.Text = "";
        txtEmail.Text = "";
        */
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.btnDelete_Click','writeDeveloper.aspx')</script>");
    }


    private void ListReqempBind()
    {
        //DB에서 공통코드 정보를 가져와서 listComcd에 입력한다.

        string where = "";

        if (cboSearch.SelectedItem.Value.ToString() == "DeptseqNM")
            where = "de.comcdNm like '%" + txtSearch.Text + "%'";
        else if (cboSearch.SelectedItem.Value.ToString() == "ReqempNm")
            where = "ReqempNm like '%" + txtSearch.Text + "%'";

        string sql = @"select re.ReqempSeq
                            , de.comcdNm as DeptSeqNm
                            , re.ReqEmpNm
                            , jw.comcdNm as JwcdNm
                            , re.Oftel
                            , re.Mbtel
                            , re.Email
                            , re.deptseq
                            , re.jwcd
                         from pjt_reqemp re Left outer join pjt_comcd  de on re.deptseq = de.comcd
                                            Left outer join pjt_comcd  jw on re.Jwcd    = jw.comcd
                        where " + where;
        //HttpContext.Current.Response.Write(sql);
        // Run the query and bind the resulting DataSet
        // to the GridView control.
        DataSet ds = GetData(sql);
        if (ds.Tables.Count > 0)
        {
            listReqemp.DataSource = ds;
            listReqemp.DataBind();
            listReqemp.AllowPaging = true;

        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        ds.Dispose();
    }

    /// <summary>
    /// row가 생성될때마다 타게되는 event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void listReqemp_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = "SEQ"; //ReqempSeq
            e.Row.Cells[0].Width = Unit.Parse("50");
            e.Row.Cells[1].Text = "부서"; //DeptSeqNm
            e.Row.Cells[1].Width = Unit.Parse("150");
            e.Row.Cells[2].Text = "성명"; //ReqEmpNm
            e.Row.Cells[2].Width = Unit.Parse("80");
            e.Row.Cells[3].Text = "직급"; //JwcdNm
            e.Row.Cells[3].Width = Unit.Parse("80");
            e.Row.Cells[4].Text = "직통"; //Oftel
            e.Row.Cells[4].Width = Unit.Parse("120");
            e.Row.Cells[5].Text = "휴대폰"; //Mbtel
            e.Row.Cells[5].Width = Unit.Parse("120");
            e.Row.Cells[6].Text = "이메일"; //Email
            //e.Row.Cells[6].Width = Unit.Parse("120");
            e.Row.Cells[7].Visible = false; // deptseq
            e.Row.Cells[8].Visible = false; // jwcd
        }

    }


    protected void listReqemp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > -1)
        {
            string ReqempSeq = string.Empty;
            string DeptseqNm = string.Empty;
            string ReqempNm = string.Empty;
            string JwcdNm = string.Empty;
            string Oftel = string.Empty;
            string Mbtel = string.Empty;
            string Email = string.Empty;
            string Jwcd = string.Empty;
            string Deptseq = string.Empty;


            ReqempSeq = DataBinder.Eval(e.Row.DataItem, "ReqempSeq").ToString();
            DeptseqNm = DataBinder.Eval(e.Row.DataItem, "DeptseqNm").ToString();
            ReqempNm = DataBinder.Eval(e.Row.DataItem, "ReqempNm").ToString();
            JwcdNm = DataBinder.Eval(e.Row.DataItem, "JwcdNm").ToString();
            Oftel = DataBinder.Eval(e.Row.DataItem, "Oftel").ToString();
            Mbtel = DataBinder.Eval(e.Row.DataItem, "Mbtel").ToString();
            Email = DataBinder.Eval(e.Row.DataItem, "Email").ToString();
            Jwcd = DataBinder.Eval(e.Row.DataItem, "Jwcd").ToString();
            Deptseq = DataBinder.Eval(e.Row.DataItem, "Deptseq").ToString();


            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";
            e.Row.Attributes["onClick"] = "ShowRow('" + ReqempSeq + "','" + DeptseqNm + "','" + ReqempNm + "','" + JwcdNm + "','" + Oftel + "','" + Mbtel + "','" + Email + "','" + Jwcd + "','" + Deptseq + "');";
            //this.style.cursor='pointer'; this.style.cursor='hand';

            e.Row.Cells[7].Visible = false; // deptseq
            e.Row.Cells[8].Visible = false; // jwcd

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ListReqempBind();
    }



    protected void listReqemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listReqemp.PageIndex = e.NewPageIndex;
        ListReqempBind();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtReqempSeq.Text = "";
        txtReqempNm.Text = "";
        txtDeptseq.Text = "";
        txtDeptseqNm.Text = "";
        txtOftel.Text = "";
        txtMbtel.Text = "";
        txtEmail.Text = "";
    }
}