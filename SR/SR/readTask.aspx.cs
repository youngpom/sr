using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;

public partial class tasklista : common
{
	
	public int kkk= 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        /*
        if( cboPageSize.SelectedValue.ToString() != "" ) Session["PAGESIZE"] = cboPageSize.SelectedValue.ToString();
        if (Request.Cookies["UserSettings"] != null && Request.Cookies["UserSettings"]["PAGESIZE"] != null)
        {
            cboPageSize.SelectedValue = Request.Cookies["UserSettings"]["PAGESIZE"];
            listTask.PageSize = int.Parse(Request.Cookies["UserSettings"]["PAGESIZE"]);
        }*/
        chkUserId();
        string userid = Request.Cookies["UserSettings"]["USERID"];

        string developer = string.Empty;
        string reqdept = string.Empty;
        string taskstep = string.Empty;
        string reqtool = string.Empty;
        string requser = string.Empty;
        string startdate = string.Empty;
        string enddate = string.Empty;
		string programcopy = string.Empty;
		string gubun_1 = string.Empty;
		
		
        bool mydata = false;
        bool ingdata = false;
		bool imtdata = false;
		//bool noticedata = false;

        bool radioall = false;
        bool radiofirst = false;
		bool radiocom = false;
		bool radioadd = false;
        string rcptemp = string.Empty;
        string reqrmk1 = string.Empty;
        string devrmk = string.Empty;
        string docno = string.Empty;
        string sysnm = string.Empty;
        string search_reqcontent = string.Empty;
        string search_reqrmk = string.Empty;
        string reqemps = string.Empty;

        if (txtStartTime.Text == "")
            //txtStartTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(DateTime.Now.Year + "-01-01"));
            txtStartTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-3));

        if (txtEndTime.Text == "")
            txtEndTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

        /*
        if (IsAdmin() == false)
        {
            chkMyData.Checked = true;
            chkMyData.Enabled = false;
        }
        */
		
        if (!IsPostBack)
        {
            //처리자콤보
            string usersql = @"select devempid, devempnm from pjt_devemp where devempid like 'mcc%' or devempnm in ('박창근','김종길','양진영','박혜경','김현이','서경배','김효찬') or devempid in ('5117160','5115198') order by devempid asc";
            // Retrieve the connection string stored in the Web.config file.
            DataTable userdt = GetDataTable(usersql);
            if (userdt.Rows.Count > 0)
            {
                makeSelectList(cboDeveloper, userdt, "devempnm", "devempid");
            }
            userdt.Clear();
            userdt.Dispose();

            //진행단계콤보
            string taskstepsql = @"select comcd, comcdnm from pjt_comcd where upcd = '40' order by comcd";
            DataTable taskstepdt = GetDataTable(taskstepsql);
            if (taskstepdt.Rows.Count > 0)
            {
                makeSelectList(cboTaskStep, taskstepdt, "comcdnm", "comcd");
            }
          //  ListItem oItem = new ListItem("지연", "지연");
          //  cboTaskStep.Items.Add(oItem);
            taskstepdt.Clear();
            taskstepdt.Dispose();

            //요청방식콤보
            /*     string ReqToolSql = @"select comcd, comcdnm from pjt_comcd where upcd = '60' order by comcd";
                 DataTable ReqTooldt = GetDataTable(ReqToolSql);
                 if (ReqTooldt.Rows.Count > 0)
                 {
                     makeSelectList(cboReqTool, ReqTooldt, "comcdnm", "comcd");
                 }
                 ReqTooldt.Clear();
                 ReqTooldt.Dispose();*/
            
            ///시스템명 콤보
            string systemnamesql = @"
                               select comcd, comcdnm from pjt_comcd where upcd = '50' order by comcd";
            DataTable systemnamedt = GetDataTable(systemnamesql);
            if (systemnamedt.Rows.Count > 0)
            {
                makeSelectList(cboGnlSystemName, systemnamedt, "comcdnm", "comcd");
            }
            systemnamedt.Clear();
            systemnamedt.Dispose();

            string ReqToolSql_1 = @"select comcd, comcdnm from pjt_comcd where upcd = '80' order by comcd";
            DataTable ReqTooldt_1 = GetDataTable(ReqToolSql_1);
            if (ReqTooldt_1.Rows.Count > 0)
            {
                makeSelectList(cboGubun_1, ReqTooldt_1, "comcdnm", "comcd");
            }
            ReqTooldt_1.Clear();
            ReqTooldt_1.Dispose();


            int paging = 0;
            //writeTask넘어온 parameter들의 값을 넣어준다.
            if (!String.IsNullOrEmpty(Request["taskID"]) && Request["taskID"] != "")
            {
                cboTaskStep.SelectedValue = Request["hdnTaskStep"];
                cboDeveloper.SelectedValue = Request["hdnDeveloper"];
                cboGubun_1.SelectedValue = Request["hdnGubun_1Tool"];
                txtStartTime.Text = Request["hdnStartTime"];
                txtEndTime.Text = Request["hdnEndTime"];
                hdnRequestDept.Value = Request["hdnRequestDept"];
                txtRequestDept.Text = Request["hdnRequestDeptName"];
                hdnRequestBy.Value = Request["hdnRequestBy"];
                //txtRequestBys.Text = Request["hdnRequestBys"];
                txtRequestBy.Text = Request["hdnRequestByName"];
                chkMyData.Checked = bool.Parse(Request["hdnMyData"]);
                chkIngData.Checked = bool.Parse(Request["hdnIngData"]);
				FirstReq.Checked = bool.Parse(Request["hdnRadFirstReq"]);
				ComReq.Checked = bool.Parse(Request["hdnRadComReq"]);
				AddReq.Checked = bool.Parse(Request["hdnRadAddReq"]);
				chkImPorTant.Checked = bool.Parse(Request["hdnImt"]);
                txtRcptEmp.Text = Request["hdnRcptEmp"];
                txtDevrmk.Text = Request["hdnDevrmk"];
                txtDocNo.Text = Request["hdnDocNo"];
				//noticedata = reader["NOTICE"];
				//programcopy = Request["hdnIpt"];
				
                paging = int.Parse(Request["hdnPaging"]);
            }

            //GridView 출력
            developer = cboDeveloper.Items[cboDeveloper.SelectedIndex].Value;
            reqdept = hdnRequestDept.Value;
            taskstep = cboTaskStep.Items[cboTaskStep.SelectedIndex].Value;
            requser = hdnRequestBy.Value;
            startdate = txtStartTime.Text;
            enddate = txtEndTime.Text;
            docno = txtDocNo.Text;
            gubun_1 = cboGubun_1.Items[cboGubun_1.SelectedIndex].Value;
            rcptemp = txtRcptEmp.Text;
            devrmk = txtDevrmk.Text;
            sysnm = cboGnlSystemName.Items[cboGnlSystemName.SelectedIndex].Value;
            search_reqcontent = txtSearchReqContent.Text;
            search_reqrmk = txtSearchReqRmk.Text;
            reqemps = txtRequestBys.Text;

            //Response.Write(Request["PROGRAMCOPY"]);
            programcopy = Request["PROGRAMCOPY"];
            //programcopy = 
            mydata = chkMyData.Checked == true ? true : false;
            ingdata = chkIngData.Checked == true ? true : false;
            radioall = AllReq.Checked == true ? true : false;
			radiofirst =  FirstReq.Checked == true ? true : false;
		    radiocom = ComReq.Checked == true ? true : false;
		    radioadd =  AddReq.Checked == true ? true : false;
		    imtdata = chkImPorTant.Checked == true ? true : false;
			
            listTask.PageIndex = paging;

            MakeGridview(developer, reqdept, taskstep, requser, startdate, enddate, mydata, ingdata, rcptemp, devrmk, gubun_1, docno,imtdata,programcopy,radioall,radiofirst,radiocom,radioadd,userid,sysnm,search_reqrmk, search_reqcontent,reqemps);
        }
    }

    public void MakeGridview(string developer, string reqdept, string taskstep, string requser, string startdate, string enddate, bool mydata, bool ingdata, string rcptemp, string devrmk, string gubun_1, string docno,bool imtdata,string programcopy,bool radioall,bool radiofirst,bool radiocom, bool radioadd,string userid,string sysnm,string search_reqrmk,string search_reqcontent, string reqemps)
    {
        string sql = @"SELECT TASKSEQ , TASKID,docno as DOCNO, TASKSTEP_COMCD.COMCDNM as TASKSTEP, REQTYPE_COMCD.COMCDNM as REQTYPE, REQTOOL_COMCD.COMCDNM as REQTOOL
                            , REQDT, REQDEPT_COMCD.COMCDNM as REQDEPTNM,  REQRMK, REQEMP, RCPEMP_DEVEMP.DEVEMPNM as RCPEMPNM
		                    , REQEMPS, REQDUEDT
		                    , TASKPROG, DOTYPE_COMCD.COMCDNM as DOTYPE, STEXDT, STDT, SPEXDT, SPDT
		                    , DEVEMP_DEVEMP.DEVEMPNM as DEVEMPNM, DEVEMPS, DEVRMK
		                    , PJT_TASK.REGEMP, PJT_TASK.CHGEMP, PJT_TASK.REGDT, PJT_TASK.CHGDT, RCPTEMP, DEVEMP
                            , CONSEQ ,case when CONSEQ is null then '' else 'FILE' end  
                            , CheckIpt , RadFirstReq,RadComReq,RadAddReq,PROGRAMCOPY							
                            , SYSNM_COMCD.COMCDNM as SYSNM
                        FROM PJT_TASK left join
                             PJT_COMCD AS SYSNM_COMCD on PJT_TASK.SYSNM = SYSNM_COMCD.COMCD and SYSNM_COMCD.UPCD = '50' left join
		                     PJT_COMCD AS REQDEPT_COMCD on PJT_TASK.REQDEPTSEQ = REQDEPT_COMCD.COMCD and REQDEPT_COMCD.UPCD = '30' left join
		                     PJT_COMCD AS REQTYPE_COMCD on PJT_TASK.REQTYPE = REQTYPE_COMCD.COMCD and REQTYPE_COMCD.UPCD = '70' left join
		                     PJT_COMCD AS REQTOOL_COMCD on PJT_TASK.REQTOOL = REQTOOL_COMCD.COMCD and REQTOOL_COMCD.UPCD = '60' left join
		                     PJT_COMCD AS DOTYPE_COMCD on PJT_TASK.DOTYPE = DOTYPE_COMCD.COMCD and DOTYPE_COMCD.UPCD = '80' left join
		                     PJT_COMCD AS TASKSTEP_COMCD on PJT_TASK.TASKSTEP = TASKSTEP_COMCD.COMCD and TASKSTEP_COMCD.UPCD = '40' left join
		                     PJT_DEVEMP AS RCPEMP_DEVEMP on PJT_TASK.RCPTEMP = RCPEMP_DEVEMP.DEVEMPID left join
                             PJT_ATTFILE AS ATTFILE_CONSEQ on PJT_TASK.TASKSEQ = ATTFILE_CONSEQ.CONSEQ left join
		                     PJT_DEVEMP AS DEVEMP_DEVEMP on PJT_TASK.DEVEMP = DEVEMP_DEVEMP.DEVEMPID
                       WHERE PJT_TASK.TASKSEQ is not NULL";			   
					   
        if (developer != "" && developer != "전체")
            sql = sql + " and PJT_TASK.DEVEMP = '" + developer + "'";
        if (reqdept != "")
            sql = sql + " and PJT_TASK.REQDEPTSEQ = '" + reqdept + "'";
        
       // if (taskstep == "지연")
           // sql = sql + " and SYSNM !=  403 ";
	   //if(taskstep == "처리완료")
		  // sql = sql  + " and PJT_TASK_SYSNM = '403' ";
	   
	 //  if(taskstep!="" && taskstep !="전체")
		//   sql = sql + " and SYSNM 
        //else if (taskstep != "" && taskstep != "전체")

	 // sql = sql + " and PJT_TASK.TASKSTEP = '" + taskstep + "'";
		
		 //(기존소스)
	 
		  if (taskstep == "지연")
            sql = sql + " and (TASKSTEP_COMCD.COMCDNM != '처리완료' AND PJT_TASK.SPEXDT < '" + DateTime.Now.ToShortDateString() + "' AND SPDT is NULL)";
        else if (taskstep != "" && taskstep != "전체")
            sql = sql + " and PJT_TASK.TASKSTEP = '" + taskstep + "'";

          // 시스템명
          if(sysnm != "전체")
            sql = sql + " and PJT_TASK.SYSNM = '" + sysnm + "'";
        // 제목
        if (search_reqrmk != "")
            sql = sql + " and PJT_TASK.REQRMK LIKE '%" + search_reqrmk + "%'";
        // 요청내용
        if (search_reqcontent != "")
            sql = sql + " and PJT_TASK.REQRMKCOPY LIKE '%" + search_reqcontent + "%'";
        //if(taskstep !="" && taskstep!="지연")
        //	 sql = sql + " and PJT_TASK.TASKSTEP = '" + taskstep + "'";
        // else
        //	  sql = sql + " and PJT_TASK.TASKSTEP = '" + taskstep + "'";

        // 요청자
        if (requser != "")
            sql = sql + " and PJT_TASK.REQEMP = '" + requser + "'";
      //  if (startdate != "")
        //    sql = sql + " and (PJT_TASK.STDT >= '" + startdate + "' or PJT_TASK.STEXDT >= '" + startdate + "' or REQDT >= '" + startdate + "' or REQDUEDT >= '" + startdate + "')";
       // if (enddate != "")
          //  sql = sql + " and (PJT_TASK.SPDT <= '" + enddate + "' or PJT_TASK.SPEXDT <= '" + enddate + "' or REQDT <= '" + enddate + "' or REQDUEDT <= '" + enddate + "')";
	  
	 // REQDT >= '2017-09-07' AND REQDT <= '2017-09-20' ;
	 
	    if(startdate !="" && enddate !="")
		  sql = sql +  " and (PJT_TASK.REQDT >= '" + startdate + "' and PJT_TASK.REQDT <= '" + enddate + "' )";
        //Response.Write(sql);
        // 내 것만 보기
        if (mydata == true)
            sql = sql + " and (PJT_TASK.RCPTEMP = '" + Request.Cookies["UserSettings"]["USERID"] + "' or PJT_TASK.DEVEMP = '" + Request.Cookies["UserSettings"]["USERID"] + "')";
        // 미처리내역
        if (ingdata == true)
            sql = sql + " and (TASKSTEP_COMCD.COMCDNM != '처리완료' or TASKSTEP_COMCD.COMCDNM is NULL)";

        // 요청자 이름으로 검색
        if (rcptemp != "")
            sql = sql + " and PJT_TASK.RCPTEMP IN (SELECT A.DEVEMPID FROM PJT_DEVEMP A WHERE DEVEMPNM='" + rcptemp + "')";

        if (devrmk != "")
            sql = sql + " and PJT_TASK.DEVRMK like '%" + devrmk + "%'";
        if (gubun_1 != "" && gubun_1 != "전체")
            sql = sql + " and PJT_TASK.DOTYPE = '" + gubun_1 + "'";

        // 전산담당자 이름으로 검색
        if (reqemps != "")
            sql = sql + " and PJT_TASK.REQEMPS like '%" + reqemps + "%'";

        // 문서번호
        if (docno != "")
            sql = sql + " and PJT_TASK.DOCNO like '%" + docno + "'";

        // 내 것만 보기
        if (imtdata==true)
		    sql = sql + " and PJT_TASK.CheckIpt =  '1'";

        // 전체 요청 유형 선택 시 요청유형 조건 없음
        //if (radioall==true)
        if (radiofirst==true)
            sql = sql + " and PJT_TASK.RadFirstReq =  '1' ";
		if (radiocom==true)
            sql = sql + " and PJT_TASK.RadComReq =  '1' ";
		if (radioadd==true)
            sql = sql + " and PJT_TASK.RadAddReq =  '1' ";
		if (programcopy != "")
            sql = sql + " and PJT_TASK.PROGRAMCOPY like '%" + programcopy+ "%'";

        // 관리자가 아니면(MCC 직원) OCS/EMR 항목만 표시
        if (IsAdmin() == false)
            sql = sql + "and SYSNM_COMCD.COMCD = '501'";

        sql = sql + "  ORDER BY REGDT DESC";
		
		//string sql1 = 
        //Response.Write(sql);
		
        DataTable dt = GetDataTable(sql);
        if (dt.Rows.Count >= 0)
        {
            listTask.EmptyDataText = "조회하신 데이터가 없습니다.";
            listTask.DataSource = dt;
            listTask.DataBind();
        }
		kkk=dt.Rows.Count;
        dt.Clear();
        dt.Dispose();
    }

    protected void listTask_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //listTask.Attributes.Add("style", "table-layout:fixed");

        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell HeaderCell = new TableHeaderCell();

          //GridView HeaderGrid = (GridView)sender;
           // GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //TableHeaderCell HeaderCell = new TableHeaderCell();

            //HeaderCell.Text = "기본사항";
            //HeaderCell.ColumnSpan = 4;
            //HeaderGridRow.Cells.Add(HeaderCell);

            //HeaderCell = new TableHeaderCell();
           // HeaderCell.Text = "요청사항";
            //HeaderCell.ColumnSpan = 5;
           // HeaderGridRow.Cells.Add(HeaderCell);

          //  HeaderCell = new TableHeaderCell();
         //   HeaderCell.Text = "처리사항";
           // HeaderCell.ColumnSpan = 3; //6
          //  HeaderGridRow.Cells.Add(HeaderCell);

            listTask.Controls[0].Controls.AddAt(0, HeaderGridRow);
            e.Row.Cells[0].Text = "시퀀스No";
            e.Row.Cells[0].Visible = false;  //TASKSEQ
			e.Row.Cells[1].Visible = false;
          //  e.Row.Cells[1].Text = "일련번호";
          //  e.Row.Cells[1].Width = Unit.Parse("120");
            
			e.Row.Cells[2].Text = "문서번호";
            e.Row.Cells[2].Width = Unit.Parse("100");
			//e.Row.Cells[3].Visible = false;
            e.Row.Cells[3].Text = "진행단계";
            e.Row.Cells[3].Width = Unit.Parse("80");
			
            e.Row.Cells[4].Visible = false;//요청유형
            e.Row.Cells[5].Visible = false;//요청방식
			
			
            e.Row.Cells[6].Text = "요청일";
            e.Row.Cells[6].Width = Unit.Parse("100");
            

            e.Row.Cells[7].Text = "요청부서";
            e.Row.Cells[7].Width = Unit.Parse("160");
			
            e.Row.Cells[8].Text = "제 목";
            e.Row.Cells[8].Width = Unit.Parse("600");
			
			
            e.Row.Cells[9].Visible = false; //요청자
			
			
            e.Row.Cells[10].Text = "접수자";
            e.Row.Cells[10].Width = Unit.Parse("60");
            

            e.Row.Cells[11].Text = "전산담당자"; //전산 담당자
            e.Row.Cells[11].Width = Unit.Parse("100");

            e.Row.Cells[12].Visible = false; //완료요청일
            e.Row.Cells[13].Visible = false; //진척률
			
			
			
            e.Row.Cells[14].Text = "구분";
			e.Row.Cells[14].Width = Unit.Parse("80");
			
            e.Row.Cells[15].Visible = false; //시작예정일
		    e.Row.Cells[16].Visible = false; //시작일
			
			
            e.Row.Cells[17].Text = "완료예정일"; //종료예정일
            e.Row.Cells[17].Width = Unit.Parse("100");
            

            e.Row.Cells[18].Visible = false;//완료일
			
            //e.Row.Cells[18].Text = "완료일";
            //e.Row.Cells[18].Width = Unit.Parse("200");
			
            e.Row.Cells[19].Text = "처리자";
			e.Row.Cells[19].Width = Unit.Parse("60");
            

            e.Row.Cells[20].Visible = false; //처리관련자
            //e.Row.Cells[20].Text = "처리관련자";
            //e.Row.Cells[20].Width = Unit.Parse("100");
			
			
            e.Row.Cells[21].Text = "처리사항";
            e.Row.Cells[21].Width = Unit.Parse("250");
			
            e.Row.Cells[22].Visible = false; //등록자
            e.Row.Cells[23].Visible = false; //수정자
            e.Row.Cells[24].Visible = false; //등록일
            e.Row.Cells[25].Visible = false; //수정일
            e.Row.Cells[26].Visible = false; //접수자
            e.Row.Cells[27].Visible = false; //개발자
            e.Row.Cells[28].Visible = false; //파일
            e.Row.Cells[29].Visible = false; //시스템
						
			e.Row.Cells[30].Visible = false; // 긴급사항
			e.Row.Cells[31].Visible = false;
			e.Row.Cells[32].Visible = false;
			e.Row.Cells[33].Visible = false;
			e.Row.Cells[34].Text = " 적용프로그램";
			e.Row.Cells[34].Width = Unit.Parse("200");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;//TASKSEQ
			e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;//요청유형
            e.Row.Cells[5].Visible = false;//요청방식
            e.Row.Cells[9].Visible = false; //요청자
//            e.Row.Cells[11].Visible = false; //요청관련자
            e.Row.Cells[12].Visible = false; //완료요청일
            e.Row.Cells[13].Visible = false; //진척률
            e.Row.Cells[15].Visible = false; //시작예정일
            e.Row.Cells[16].Visible = false; //시작일
            e.Row.Cells[18].Visible = false;//완료일
            e.Row.Cells[20].Visible = false; //처리관련자
            e.Row.Cells[22].Visible = false; //등록자
            e.Row.Cells[23].Visible = false; //수정자
            e.Row.Cells[24].Visible = false; //등록일
            e.Row.Cells[25].Visible = false; //수정일
            e.Row.Cells[26].Visible = false; //접수자
            e.Row.Cells[27].Visible = false; //개발자
            e.Row.Cells[28].Visible = false; //파일
            e.Row.Cells[29].Visible = false; //시스템
			
			e.Row.Cells[30].Visible = false; // 긴급사항
			e.Row.Cells[31].Visible = false;
			e.Row.Cells[32].Visible = false;
			e.Row.Cells[33].Visible = false;
        }
    }
    protected void listTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string userID = string.Empty;

        userID = Request.Cookies["UserSettings"]["USERID"];

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Style["height"] = "40px";

            e.Row.Cells[6].Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(e.Row.Cells[6].Text));

            if (e.Row.Cells[18].Text.Trim() != "" && e.Row.Cells[18].Text.Length > 10)
                e.Row.Cells[18].Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(e.Row.Cells[18].Text));

            if (e.Row.Cells[17].Text.Trim() != "" && e.Row.Cells[17].Text.Length > 10)
            {
                e.Row.Cells[17].Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(e.Row.Cells[17].Text));
                if (DateTime.Parse(e.Row.Cells[17].Text) < DateTime.Now.Date && e.Row.Cells[3].Text.Trim() != "처리완료")
                {
                   // e.Row.Cells[3].Text = "<font color=\"red\">지연</font>";
                }
            }

            if (e.Row.Cells[3].Text.Trim() != "처리완료")
            {
                e.Row.Cells[3].Text = "<font color=\"blue\">" + e.Row.Cells[3].Text + "</font>";
            }
			else
			{
				e.Row.Cells[3].Text = "<font color=\"red\">" + e.Row.Cells[3].Text + "</font>";
			}
            
            if (e.Row.Cells[8].Text.Trim() != "")
                e.Row.Cells[8].Text = e.Row.Cells[8].Text.Replace("™", "'");
            if (e.Row.Cells[8].Text.Trim() != "" && e.Row.Cells[8].Text.Length > 50)
                e.Row.Cells[8].Text = e.Row.Cells[8].Text.Substring(0, 50) + "~~~~~";
            if (e.Row.Cells[28].Text.Trim() == "FILE") e.Row.Cells[8].Text = "<font color='blue'>[파일]</font> " + e.Row.Cells[8].Text;
            if (e.Row.Cells[21].Text.Trim() != "")
                e.Row.Cells[21].Text = e.Row.Cells[21].Text.Replace("™", "'");
            if (e.Row.Cells[21].Text.Trim() != "" && e.Row.Cells[21].Text.Length > 50)
                e.Row.Cells[21].Text = e.Row.Cells[21].Text.Substring(0, 50) + "~~~~~";
            if (e.Row.Cells[6].Text.Trim() != "" && e.Row.Cells[6].Text == string.Format("{0:yyyy-MM-dd}", DateTime.Now))
            {// 요청일자가 오늘이면 요청일자를 굵게함
                e.Row.Cells[6].Style["font-weight"] = "bold";
                e.Row.Cells[6].Style["color"] = "black";
            }
            if (e.Row.Cells[24].Text.Trim() != "" && string.Format("{0:yyyy-MM-dd}", DateTime.Parse(e.Row.Cells[24].Text)) == string.Format("{0:yyyy-MM-dd}", DateTime.Now))
            {// 등록일자가 오늘이면 접수번호를 굵게함
                e.Row.Cells[1].Style["font-weight"] = "bold";
                e.Row.Cells[1].Style["color"] = "black";
            }

            //내가 전산담당자이거나 접수자이거나 처리자인경우 해당 셀 스타일 조정
            if (e.Row.Cells[11].Text == userID)
            {
                e.Row.Cells[11].Style["font-weight"] = "bold";
                e.Row.Cells[11].Style["color"] = "black";
            }
            if (e.Row.Cells[26].Text == userID)
            {
                //e.Row.Cells[10].Style["background-color"] = "LightGray";
                e.Row.Cells[10].Style["font-weight"] = "bold";
                e.Row.Cells[10].Style["color"] = "black";
            }
            if (e.Row.Cells[27].Text == userID)
            {
                //e.Row.Cells[19].Style["background-color"] = "LightGray";
                e.Row.Cells[19].Style["font-weight"] = "bold";
                e.Row.Cells[19].Style["color"] = "black";
            }

            //내가 관리자거나 접수자나 처리자인 경우 수정이 가능하다.
        //    if (IsAdmin() == true || e.Row.Cells[26].Text == userID || e.Row.Cells[27].Text == userID || e.Row.Cells[22].Text == userID || e.Row.Cells[23].Text == userID)
				  if (IsAdmin() == true || e.Row.Cells[26].Text == userID || e.Row.Cells[27].Text == userID || e.Row.Cells[22].Text == userID )
            {
                //e.Row.Attributes["onClick"] = "document.location.href=\"writeTask.aspx?taskID=" + e.Row.Cells[0].Text + "\"";
                e.Row.Attributes["onClick"] = "javascript: openTask('edit'," + e.Row.Cells[0].Text + ");";
            }
            else
            {
                //e.Row.Attributes["onClick"] = "document.location.href=\"writeTask.aspx?MODE=view&taskID=" + e.Row.Cells[0].Text + "\"";
                e.Row.Attributes["onClick"] = "javascript: openTask('view'," + e.Row.Cells[0].Text + ");";
            }
            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";

            e.Row.Cells[2].Text = "<center>" + e.Row.Cells[2].Text + "</center>";
            e.Row.Cells[3].Text = "<center>" + e.Row.Cells[3].Text + "</center>";
            e.Row.Cells[6].Text = "<center>" + e.Row.Cells[6].Text + "</center>";
            e.Row.Cells[10].Text = "<center>" + e.Row.Cells[10].Text + "</center>";
            e.Row.Cells[11].Text = "<center>" + e.Row.Cells[11].Text + "</center>";
            e.Row.Cells[14].Text = "<center>" + e.Row.Cells[14].Text + "</center>";
            e.Row.Cells[17].Text = "<center>" + e.Row.Cells[17].Text + "</center>";
            e.Row.Cells[19].Text = "<center>" + e.Row.Cells[19].Text + "</center>";
        }
    }
    protected void listTask_PageIndexChanged(object sender, EventArgs e)
    {

    }
    protected void listTask_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string userid = Request.Cookies["UserSettings"]["USERID"];
        string developer = string.Empty;
        string reqdept = string.Empty;
        string taskstep = string.Empty;
        string reqtool = string.Empty;
        string requser = string.Empty;
        string startdate = string.Empty;
        string enddate = string.Empty;
		string programcopy = string.Empty;
		string gubun_1 = string.Empty;
        string sysnm = string.Empty;
        string search_reqrmk = string.Empty;
        string search_reqcontent = string.Empty;
        string reqemps = string.Empty;

        bool mydata = false;
        bool ingdata = false;
		bool imtdata = false;

        bool radioall = false;
        bool radiofirst = false;
		bool radiocom = false;
		bool radioadd = false;
		
		
        //검색항목 추가
        string rcptemp = string.Empty;
        string devrmk = string.Empty;
        string docno = string.Empty;
		//programcopy = Request["hdnIpt"];

        listTask.PageIndex = e.NewPageIndex;
        pagingNo.Value = e.NewPageIndex.ToString();

        developer = cboDeveloper.Items[cboDeveloper.SelectedIndex].Value;
        reqdept = hdnRequestDept.Value;
        taskstep = cboTaskStep.Items[cboTaskStep.SelectedIndex].Value;
        //reqtool = cboReqTool.Items[cboReqTool.SelectedIndex].Value;
		gubun_1 = cboGubun_1.Items[cboGubun_1.SelectedIndex].Value;
        requser = hdnRequestBy.Value;
        startdate = txtStartTime.Text;
        enddate = txtEndTime.Text;
        //programcopy = 
        if (AllReq.Checked)
        {
            radioall = true;
            radiofirst = false;
            radiocom = false;
            radioadd = false;
        }
        if (FirstReq.Checked)
		{
            radioall = false;
			radiofirst = true;
			radiocom = false;
			radioadd = false;
		}
		if(ComReq.Checked)
		{
            radioall = false;
			radiofirst = false;
			radiocom = true;
			radioadd = false;
		}
		if(AddReq.Checked)
		{
            radioall = false;
			radiofirst = false;
			radiocom = false;
			radioadd = true;
		}

		programcopy = txtProgramcopy.Text;
        mydata = chkMyData.Checked == true ? true : false;
        ingdata = chkIngData.Checked == true ? true : false;
	    //	adiofirst =  FirstReq.Checked == true ? true : false;
		//radiocom = ComReq.Checked == true ? true : false;
		//radioadd =  AddReq.Checked == true ? true : false;
		imtdata = chkImPorTant.Checked == true ? true : false;

        rcptemp = txtRcptEmp.Text;
        devrmk = txtDevrmk.Text;
        docno = txtDocNo.Text;
        sysnm = cboGnlSystemName.Items[cboGnlSystemName.SelectedIndex].Value;
        search_reqrmk = txtSearchReqRmk.Text;
        search_reqcontent = txtSearchReqContent.Text;
        reqemps = txtRequestBys.Text;

        MakeGridview(developer, reqdept, taskstep, requser, startdate, enddate, mydata, ingdata, rcptemp, devrmk, gubun_1, docno,imtdata,programcopy,radioall,radiofirst,radiocom,radioadd,userid,sysnm,search_reqrmk,search_reqcontent,reqemps);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string userid = Request.Cookies["UserSettings"]["USERID"];

        string developer = string.Empty;
        string reqdept = string.Empty;
        string taskstep = string.Empty;
        string reqtool = string.Empty;
        string requser = string.Empty;
        string startdate = string.Empty;
        string enddate = string.Empty;
        bool mydata = false;
        bool ingdata = false;
		bool imtdata = false;
		string gubun_1 = string.Empty;

        bool radioall = false;
        bool radiofirst = false;
		bool radiocom = false;
		bool radioadd = false;
        //검색항목 추가
        string rcptemp = string.Empty;
        string devrmk = string.Empty;
        string docno = string.Empty;
		string programcopy = string.Empty;
        string sysnm = string.Empty;
        string search_reqrmk = string.Empty;
        string search_reqcontent = string.Empty;
        string reqemps = string.Empty;

        developer = cboDeveloper.Items[cboDeveloper.SelectedIndex].Value;
        reqdept = hdnRequestDept.Value;
        taskstep = cboTaskStep.Items[cboTaskStep.SelectedIndex].Value;
        sysnm = cboGnlSystemName.Items[cboGnlSystemName.SelectedIndex].Value;
        search_reqrmk = txtSearchReqRmk.Text;
        search_reqcontent = txtSearchReqContent.Text;
        reqemps = txtRequestBys.Text;

        //   reqtool = cboReqTool.Items[cboReqTool.SelectedIndex].Value;
        gubun_1 = cboGubun_1.Items[cboGubun_1.SelectedIndex].Value;
        requser = hdnRequestBy.Value;
        startdate = txtStartTime.Text;
        enddate = txtEndTime.Text;

        mydata = chkMyData.Checked == true ? true : false;
        ingdata = chkIngData.Checked == true ? true : false;

        if (AllReq.Checked)
        {
            radioall = true;
            radiofirst = false;
            radiocom = false;
            radioadd = false;
        }
        if (FirstReq.Checked)
        {
            radioall = false;
            radiofirst = true;
            radiocom = false;
            radioadd = false;
        }
        if (ComReq.Checked)
        {
            radioall = false;
            radiofirst = false;
            radiocom = true;
            radioadd = false;
        }
        if (AddReq.Checked)
        {
            radioall = false;
            radiofirst = false;
            radiocom = false;
            radioadd = true;
        }

        //mydata = chkMyData.Checked == true ? true : false;
        // ingdata = chkIngData.Checked == true ? true : false;
        //radiofirst =  FirstReq.Checked == true ? true : false;
        //radiocom = ComReq.Checked == true ? true : false;
        //radioadd =  AddReq.Checked == true ? true : false;
        imtdata = chkImPorTant.Checked == true ? true : false;

        rcptemp = txtRcptEmp.Text;
        devrmk = txtDevrmk.Text;
        docno = txtDocNo.Text;
		
		programcopy = txtProgramcopy.Text;
		//Response.Write("Qewqwqew" + programcopy);
		//programcopy =Request["PROGRAMCOPY"];  //Request["hdnIpt"];
		
        MakeGridview(developer, reqdept, taskstep, requser, startdate, enddate, mydata, ingdata, rcptemp, devrmk, gubun_1, docno,imtdata,programcopy,radioall,radiofirst,radiocom,radioadd,userid,sysnm,search_reqrmk,search_reqcontent,reqemps);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Session["PAGESIZE"] = cboPageSize.Items[cboPageSize.SelectedIndex].Value;
    }
}


