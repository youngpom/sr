using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

/*
 * 부서검색 요청페이지에서 자바스크립트 사용예
 * HTML 본문 : onClick="javascript:SearchDept('form1.txtDeptseq.value','form1.txtDeptseqNm.value');"
   'form1.txtDeptseq.value'   => 부서코드가 샛팅될 Item을 정의
   'form1.txtDeptseqNm.value' => 부서명이  샛팅될 Item을 정의
 */


public partial class searchDept : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        chkUserId();

        hdnDeptseqQuery.Value = Request["DeptseqQuery"];
        hdnDeptseqNmQuery.Value = Request["DeptseqNmQuery"];

        if (!IsPostBack)
        {
            if (Request["cboSearch"] != null && !string.IsNullOrEmpty(Request["cboSearch"]))
                cboSearch.SelectedValue = Request["cboSearch"];
            if (!string.IsNullOrEmpty(Request["txtSearch"]) && Request["txtSearch"] != "undefined")
            {
                txtSearch.Text = Request["txtSearch"];
            }
            ListDeptBind();
        }

    }
    
    protected void listDept_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ( e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > -1 )
        {
            string Deptseq = ""; // Comcd
            string DeptseqNm = ""; //ComcdNm
            string DeptseqQuery = "";
            string DeptseqNmQuery = "";

            Deptseq = DataBinder.Eval(e.Row.DataItem, "Deptseq").ToString();
            DeptseqNm = DataBinder.Eval(e.Row.DataItem, "DeptseqNm").ToString();

            if (hdnDeptseqQuery.Value.Length > 0) DeptseqQuery = "opener.document." + hdnDeptseqQuery.Value + "='" + Deptseq + "';";
            if (hdnDeptseqNmQuery.Value.Length > 0) DeptseqNmQuery = "opener.document." + hdnDeptseqNmQuery.Value + "='" + DeptseqNm + "';";

            e.Row.Attributes["ondblClick"] = "javascript:" + DeptseqQuery + DeptseqNmQuery + "self.close();";
            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";
            e.Row.Attributes["alt"] = "해당부서를 더블클릭하세요";
            //this.style.cursor='pointer'; this.style.cursor='hand';
            //e.Row.Attributes["onClick"] = "setDept('" + Deptseq + "','" + DeptseqNm + "');";

        }
    }

    protected void listDept_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = "SEQ"; //comcd
            e.Row.Cells[0].Width = Unit.Parse("80");
            e.Row.Cells[1].Text = "파트"; //optA
            e.Row.Cells[1].Width = Unit.Parse("80");
            e.Row.Cells[2].Text = "부서명"; //comcdnm
            e.Row.Cells[2].Width = Unit.Parse("360");
            e.Row.Cells[3].Text = "약어"; //optB
            e.Row.Cells[3].Width = Unit.Parse("80");
            e.Row.Cells[4].Text = "위치"; //optE
            e.Row.Cells[4].Width = Unit.Parse("200");
        }
        //e.Row.Cells[7].Visible = false; // deptseq
        //e.Row.Cells[8].Visible = false; // jwcd
    }
    
    /// <summary>
    /// 지정된 sql을 받아서 Dataset을 넘겨줍니다.
    /// </summary>
    /// <param name="sql">조회를 원하는 쿼리를 입력합니다.</param>
    /// <returns></returns>

    private void ListDeptBind()
    {
        //DB에서 공통코드 정보를 가져와서 listComcd에 입력한다.

        string where = "";

        string onlyone = string.Empty;
        string Query = string.Empty;

        onlyone = Request["OnlyOne"];

        if (cboSearch.SelectedItem.Value.ToString() == "Deptseq")
            where = " and de.comcd like '%" + txtSearch.Text + "%'";
        else if (cboSearch.SelectedItem.Value.ToString() == "DeptseqNm")
            where = " and de.comcdnm like '%" + txtSearch.Text + "%'";
        else if (cboSearch.SelectedItem.Value.ToString() == "Deptpart")
            where = " and de.optb like '%" + txtSearch.Text + "%'";

        string sql = @"select de.comcd as DeptSeq
                            , de.optB  -- 파트
                            , de.comcdNm as DeptseqNm
                            , de.opta    -- 약어
                            , de.optE    -- 위치
                         from pjt_comcd de
                        where de.upcd = '30' and de.use01=1 " + where;
        
        //HttpContext.Current.Response.Write(sql);
        // Run the query and bind the resulting DataSet
        // to the GridView control.
        DataSet ds = GetData(sql);
        if (ds.Tables.Count > 0)
        {
            listDept.DataSource = ds;
            listDept.DataBind();
            listDept.AllowPaging = true;

            if (onlyone == "Y" && ds.Tables[0].Rows.Count == 1)
            {
                string Deptseq = ""; // Comcd
                string DeptseqNm = ""; //ComcdNm
                string DeptseqQuery = "";
                string DeptseqNmQuery = "";

                Deptseq = ds.Tables[0].Rows[0]["DeptSeq"].ToString();
                DeptseqNm = ds.Tables[0].Rows[0]["DeptseqNm"].ToString();

                if (hdnDeptseqQuery.Value.Length > 0) DeptseqQuery = "opener.document." + hdnDeptseqQuery.Value + "='" + Deptseq + "';";
                if (hdnDeptseqNmQuery.Value.Length > 0)
                {
                    DeptseqNmQuery = "opener.document." + hdnDeptseqNmQuery.Value + "='" + DeptseqNm + "';";
                    DeptseqNmQuery = DeptseqNmQuery + "opener.document." + hdnDeptseqNmQuery.Value.Split('.')[0] + "." + hdnDeptseqNmQuery.Value.Split('.')[1] + ".className='textbold';";
                }
                Query = "<script language='javascript' type='text/javascript'>" + DeptseqQuery + DeptseqNmQuery + "self.close();</script>";
                //Response.Write(Query);
            }
        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        
        ds.Dispose();

        if (!string.IsNullOrEmpty(Query) && Query != "")
            HttpContext.Current.Response.Write(Query);
    }

    protected void listDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listDept.PageIndex = e.NewPageIndex;
        ListDeptBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ListDeptBind();
    }
    protected void btnDeptEmpty_Click(object sender, EventArgs e)
    {
        string DeptseqQuery = "";
        string DeptseqNmQuery = "";
        if (hdnDeptseqQuery.Value.Length > 0) DeptseqQuery = "opener.document." + hdnDeptseqQuery.Value + "='';";
        if (hdnDeptseqNmQuery.Value.Length > 0) DeptseqNmQuery = "opener.document." + hdnDeptseqNmQuery.Value + "='';";
        string Query = "<script language='javascript' type='text/javascript'>" + DeptseqQuery + "'';" + DeptseqNmQuery + "'';" + "self.close();</script>";
        HttpContext.Current.Response.Write(Query);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>" + Query + "</script>");
    }
}
