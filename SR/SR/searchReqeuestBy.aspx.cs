using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

/*
 * 요청자검색 요청페이지에서 자바스크립트 사용예
 * HTML 본문 : onClick="javascript:SearchReqemp('form1.txtReqempseq.value','form1.txtReqempNm.value');"
   'form1.txtReqempseq.value'   => 요청자코드가 샛팅될 Item을 정의
   'form1.txtReqempNm.value' => 요청자명이  샛팅될 Item을 정의
 */

public partial class searchReqeuestBy : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        chkUserId();

        //cboSearch.SelectedValue = Request["cboSearch"];
        //txtSearch.Text = Request["txtSearch"];
        // !IsPostBack 안으로 넘김
        hdnReqempseqQuery.Value = Request["ReqempseqQuery"];
        hdnReqempNmQuery.Value = Request["ReqempNmQuery"];

        if (!IsPostBack)
        {
            ///상위코드list를 만들어준다.
            string upcdsql = @"select comcd, ' ( ' || comcd || ' ) ' ||  comcdnm  as comcdnm from pjt_comcd where upcd = '10' order by UPcd";
            DataTable upcdt = GetDataTable(upcdsql);
            if (upcdt.Rows.Count > 0)
            {
                makeSelectList(cboJwcdNm, upcdt, "comcdnm", "comcd");
            }
            upcdt.Clear();
            upcdt.Dispose();


            if (Request["cboSearch"] != null && !string.IsNullOrEmpty(Request["cboSearch"]))
                cboSearch.SelectedValue = Request["cboSearch"];
            if (!string.IsNullOrEmpty(Request["txtSearch"]) && Request["txtSearch"] != "undefined")
            {
                txtSearch.Text = Request["txtSearch"];
            }

            ListReqempseqBind();
        }

    }

    protected void listReqempseq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > -1)
        {
            string Reqempseq = ""; // Comcd
            string ReqempNm = ""; //ComcdNm
            string ReqempseqQuery = "";
            string ReqempNmQuery = "";

            Reqempseq = DataBinder.Eval(e.Row.DataItem, "Reqempseq").ToString();
            ReqempNm = DataBinder.Eval(e.Row.DataItem, "ReqempNm").ToString();

            if (hdnReqempseqQuery.Value.Length > 0) ReqempseqQuery = "opener.document." + hdnReqempseqQuery.Value + "='" + Reqempseq + "';";
            if (hdnReqempNmQuery.Value.Length > 0) ReqempNmQuery = "opener.document." + hdnReqempNmQuery.Value + "='" + ReqempNm + "';";

            e.Row.Attributes["ondblClick"] = "javascript:" + ReqempseqQuery + ReqempNmQuery + "self.close();";
            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";
            e.Row.Attributes["alt"] = "해당요청자를 더블클릭하세요";
            //this.style.cursor='pointer'; this.style.cursor='hand';

        }
    }

    protected void listReqempseq_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = "SEQ"; //Reqempseq
            e.Row.Cells[0].Width = Unit.Parse("80");
            e.Row.Cells[1].Text = "부서명"; //DeptseqNm
            e.Row.Cells[1].Width = Unit.Parse("360");
            e.Row.Cells[2].Text = "성명"; //ReqempNm
            e.Row.Cells[2].Width = Unit.Parse("80");
            e.Row.Cells[3].Text = "직위"; //JwcdNm
            e.Row.Cells[3].Width = Unit.Parse("150");
            e.Row.Cells[4].Text = "직통전화"; //Oftel
            e.Row.Cells[4].Width = Unit.Parse("200");
            e.Row.Cells[5].Text = "휴대전화"; //Mbtel
            e.Row.Cells[5].Width = Unit.Parse("200");
            e.Row.Cells[6].Text = "이메일"; //Email
            e.Row.Cells[6].Width = Unit.Parse("200");
        }
    }

    /// <summary>
    /// 지정된 sql을 받아서 Dataset을 넘겨줍니다.
    /// </summary>
    /// <param name="sql">조회를 원하는 쿼리를 입력합니다.</param>
    /// <returns></returns>

    private void ListReqempseqBind()
    {
        //DB에서 공통코드 정보를 가져와서 listComcd에 입력한다.

        string where = "";

        if (cboSearch.SelectedItem.Value.ToString() == "ReqempNm")
            where = "where re.ReqempNm like '%" + txtSearch.Text + "%'";
        else if (cboSearch.SelectedItem.Value.ToString() == "Reqempseq")
            where = "where re.Reqempseq like '%" + txtSearch.Text + "%'";
        else if (cboSearch.SelectedItem.Value.ToString() == "DeptseqNm")
            where = "where de.comcdnm like '%" + txtSearch.Text + "%'";

        string sql = @"select re.Reqempseq
                            , de.comcdnm as DeptseqNm
                            , re.REQEMPNM
                            , jw.COMCDNM as JwcdNm
                            , re.OFTEL 
                            , re.MBTEL
                            , re.EMAIL
                         from pjt_reqemp re Left outer join PJT_COMCD jw on re.JWCD = jw.COMCD and jw.upcd = '10'
                                            Left outer join PJT_COMCD de on re.DEPTSEQ = de.COMCD and de.UPCD = '30'"
                        + where;

        //HttpContext.Current.Response.Write(sql);
        // Run the query and bind the resulting DataSet
        // to the GridView control.
        DataSet ds = GetData(sql);
        if (ds.Tables.Count > 0)
        {
            listReqempseq.DataSource = ds;
            listReqempseq.DataBind();
            listReqempseq.AllowPaging = true;
        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        ds.Dispose();
    }

    protected void listReqempseq_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listReqempseq.PageIndex = e.NewPageIndex;
        ListReqempseqBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ListReqempseqBind();
    }
    protected void btnReqempseqEmpty_Click(object sender, EventArgs e)
    {
        string ReqempseqQuery = "";
        string ReqempNmQuery = "";
        if (hdnReqempseqQuery.Value.Length > 0) ReqempseqQuery = "opener.document." + hdnReqempseqQuery.Value + "='';";
        if (hdnReqempNmQuery.Value.Length > 0) ReqempNmQuery = "opener.document." + hdnReqempNmQuery.Value + "='';";

        string Query = "<script language='javascript' type='text/javascript'>" + ReqempseqQuery + ReqempNmQuery + "self.close();</script>";
        HttpContext.Current.Response.Write(Query);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>" + Query + "</script>");

    }
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
        ReqempNm = txtReqempNm.Text;
        Jwcd = cboJwcdNm.SelectedValue;

        Deptseq = txtDeptseq.Value;
        DeptseqNm = txtDeptseqNm.Text;
        Oftel = txtOftel.Text;
        Mbtel = txtMbtel.Text;
        Email = txtEmail.Text;


        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;
        querystring = @"insert into pjt_reqemp ( reqempnm          , jwcd          , oftel          , mbtel          , email          , deptseq          , regemp          , chgemp          , regdt            , chgdt) 
                                        values ( '" + ReqempNm + "', '" + Jwcd + "', '" + Oftel + "', '" + Mbtel + "', '" + Email + "', '" + Deptseq + "', '" + UserId + "', '" + UserId + "', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)";


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

        ListReqempseqBind();
    }
}
