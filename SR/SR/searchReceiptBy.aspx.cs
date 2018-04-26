

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
 * 개발자검색 요청페이지에서 자바스크립트 사용예
 * HTML 본문 : onClick="javascript:SearchDevemp('form1.txtDevempId.value','form1.txtDevempNm.value');"
   'form1.txtDevempId.value'   => 요청자코드가 샛팅될 Item을 정의
   'form1.txtDevempNm.value' => 요청자명이  샛팅될 Item을 정의
 */

public partial class searchReceiptBy : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        chkUserId();

        cboSearch.SelectedValue = Request["cboSearch"];
        txtSearch.Text = Request["txtSearch"];
        hdnDevempIdQuery.Value = Request["DevempIdQuery"];
        hdnDevempNmQuery.Value = Request["DevempNmQuery"];

        if (!IsPostBack)
        {
            listDevempBind();
        }

    }
    
    protected void listDevemp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ( e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > -1 )
        {
            string DevempId = ""; // Comcd
            string DevempNm = ""; //ComcdNm
            string DevempIdQuery = "";
            string DevempNmQuery = "";

            DevempId = DataBinder.Eval(e.Row.DataItem, "DevempId").ToString();
            DevempNm = DataBinder.Eval(e.Row.DataItem, "DevempNm").ToString();

            if (hdnDevempIdQuery.Value.Length > 0) DevempIdQuery = "opener.document." + hdnDevempIdQuery.Value + "='" + DevempId + "';";
            if (hdnDevempNmQuery.Value.Length > 0) DevempNmQuery = "opener.document." + hdnDevempNmQuery.Value + "='" + DevempNm + "';";

            e.Row.Attributes["ondblClick"] = "javascript:" + DevempIdQuery + DevempNmQuery + "self.close();";
            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";
        }
    }

    protected void listDevemp_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = "성명"; //ReqempNm
            e.Row.Cells[0].Width = Unit.Parse("80");
            e.Row.Cells[1].Text = "ID"; //ReqempNm
            e.Row.Cells[1].Width = Unit.Parse("80");
            e.Row.Cells[2].Text = "직위"; //JwcdNm
            e.Row.Cells[2].Width = Unit.Parse("80");
            e.Row.Cells[3].Text = "직통전화"; //Oftel
            e.Row.Cells[3].Width = Unit.Parse("200");
            e.Row.Cells[4].Text = "휴대전화"; //Mbtel
            e.Row.Cells[4].Width = Unit.Parse("200");
            e.Row.Cells[5].Text = "이메일"; //Email
            e.Row.Cells[5].Width = Unit.Parse("200");
        }
    }
    
    /// <summary>
    /// 지정된 sql을 받아서 Dataset을 넘겨줍니다.
    /// </summary>
    /// <param name="sql">조회를 원하는 쿼리를 입력합니다.</param>
    /// <returns></returns>

    private void listDevempBind()
    {
        //DB에서 공통코드 정보를 가져와서 listComcd에 입력한다.

        string where = "";

        if (cboSearch.SelectedItem.Value.ToString() == "DevempNm")
            where = "where de.DevempNm like '%" + txtSearch.Text + "%'";

        string sql = @"select de.DevempNm
                            , de.DevempId
                            , jw.COMCDNM as JwcdNm
                            , de.OFTEL 
                            , de.HPTEL as MBTEL
                            , de.EMAIL
                         from pjt_Devemp de Left outer join PJT_COMCD jw on de.JWCD = jw.COMCD and jw.upcd = '10'
                        " + where;

        //HttpContext.Current.Response.Write(sql);
        // Run the query and bind the resulting DataSet
        // to the GridView control.
        DataSet ds = GetData(sql);
        if (ds.Tables.Count > 0)
        {
            listDevemp.DataSource = ds;
            listDevemp.DataBind();
            listDevemp.AllowPaging = true;
        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        ds.Dispose();
    }

    protected void listDevemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listDevemp.PageIndex = e.NewPageIndex;
        listDevempBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        listDevempBind();
    }
    protected void btnDevempIdEmpty_Click(object sender, EventArgs e)
    {
        string DevempIdQuery = "";
        string DevempNmQuery = "";
        if (hdnDevempIdQuery.Value.Length > 0) DevempIdQuery = "opener.document." + hdnDevempIdQuery.Value + "='';";
        if (hdnDevempNmQuery.Value.Length > 0) DevempNmQuery = "opener.document." + hdnDevempNmQuery.Value + "='';";

        string Query = "<script language='javascript' type='text/javascript'>" + DevempIdQuery + DevempNmQuery + "self.close();</script>";
        HttpContext.Current.Response.Write(Query);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>" + Query + "</script>");
    }
}
