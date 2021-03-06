﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;

public partial class teskedit : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        chkUserId();

        if (!IsPostBack)
        {
            searchTaskStep.Value = Request["hdnTaskStep"];
            searchDeveloper.Value = Request["hdnDeveloper"];
            searchReqTool.Value = Request["hdnReqTool"];
            searchStartTime.Value = Request["hdnStartTime"];
            searchEndTime.Value = Request["hdnEndTime"];
            searchDocNo.Value = Request["hdnDocNo"];
            searchRequestDept.Value = Request["hdnRequestDept"];
            searchRequestDeptName.Value = Request["hdnRequestDeptName"];
            searchRequestBy.Value = Request["hdnRequestBy"];
            searchRequestByName.Value = Request["hdnRequestByName"];
            searchMyData.Value = Request["hdnMyData"];
            searchReqrmk.Value = Request["hdnReqrmk"];
            searchDevrmk.Value = Request["hdnDevrmk"];
            searchIngData.Value = Request["hdnIngData"];
            searchPaging.Value = Request["hdnPaging"];
            searchtaskID.Value = Request["taskID"];
            searchMODE.Value = Request["MODE"];
            searchTargetPage.Value = Request["hdnTargetPage"];

            //get방식으로 받아서 입력하기
            txtDocNo.Text = Request["txtDocNo"];
            txtRequestDate.Text = Request["txtRequestDate"];
            txtRequestDept.Text = Request["txtRequestDept"];
            txtRequestBy.Text = Request["txtRequestBy"];
            txtReceiptBy.Text = Request["txtReceiptBy"];
            txtRequestBys.Text = Request["txtRequestBys"];
            txtRequestDueDate.Text = Request["txtRequestDueDate"];
            txtRequestContent.Text = Request["txtRequestContent"];
            String tempSystemName = Request["cboGnlSystemName"];
            String tempReqTool = Request["cboGnlReqTool"];
            String tempReqType = Request["cboGnlReqType"];


            if (txtRequestDate.Text == "")
            {
                txtRequestDate.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            txtReceiptBy.Text = Request.Cookies["UserSettings"]["USERNM"];
            hdnReceiptBy.Value = Request.Cookies["UserSettings"]["USERID"];
            if (txtRequestDueDate.Text == "")
            {
                txtRequestDueDate.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }

            //처리완료시 처리자에 현재세션사용자 자동입력위한 기초값 히든셋팅
            hdnLoginNm.Value = Request.Cookies["UserSettings"]["USERNM"];
            hdnLoginId.Value = Request.Cookies["UserSettings"]["USERID"];

            //uploadRequestContent.FileName = Request["uploadRequestContent"].ToString();

            ///시스템명 list를 만들어준다.
            string systemnamesql = @"
                               select comcd, comcdnm from pjt_comcd where upcd = '50' order by comcd";
            DataTable systemnamedt = GetDataTable(systemnamesql);
            if (systemnamedt.Rows.Count > 0)
            {
                makeSelectList(cboGnlSystemName, systemnamedt, "comcdnm", "comcd");
            }
            systemnamedt.Clear();
            systemnamedt.Dispose();

            ///요청유형 list를 만들어준다.
            string reqtypesql = @"
                               select comcd, comcdnm from pjt_comcd where upcd = '70' order by comcd";
            DataTable reqtypedt = GetDataTable(reqtypesql);
            if (reqtypedt.Rows.Count > 0)
            {
                makeSelectList(cboGnlReqType, reqtypedt, "comcdnm", "comcd");
            }
            reqtypedt.Clear();
            reqtypedt.Dispose();

            ///요청방식 list를 만들어준다.
            string reqtoolsql = @"
                               select comcd, comcdnm from pjt_comcd where upcd = '60' order by comcd";
            DataTable reqtooldt = GetDataTable(reqtoolsql);
            if (reqtooldt.Rows.Count > 0)
            {
                makeSelectList(cboGnlReqTool, reqtooldt, "comcdnm", "comcd");
            }
            reqtypedt.Clear();
            reqtypedt.Dispose();

            ///구분list를 만들어준다.
            string gubunsql = @"
                               select comcd, comcdnm from pjt_comcd where upcd = '80' order by comcd";
            DataTable gubundt = GetDataTable(gubunsql);
            if (gubundt.Rows.Count > 0)
            {
                makeSelectList(cboGubun, gubundt, "comcdnm", "comcd");
            }
            reqtypedt.Clear();
            reqtypedt.Dispose();

            ///진행단계list를 만들어준다.
            string taskstepsql = @"
                               select comcd, comcdnm from pjt_comcd where upcd = '40' order by comcd";
            DataTable taskstepdt = GetDataTable(taskstepsql);
            if (taskstepdt.Rows.Count > 0)
            {
                makeSelectList(cboTaskStep, taskstepdt, "comcdnm", "comcd");
            }
            reqtypedt.Clear();
            reqtypedt.Dispose();

            if (!string.IsNullOrEmpty(Request["taskID"]) && Request["taskID"] != "")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                // Connect to the database and run the query.
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    string tasksql = @"SELECT TASKSEQ, TASKID, SYSNM, REQTYPE, REQTOOL, REQDT, REQDEPTSEQ, REQDEPT_COMCD.COMCDNM as REQDEPTNM
                                            , REQEMP, PJT_REQEMP.REQEMPNM, RCPTEMP, RCPEMP_DEVEMP.DEVEMPNM as RCPEMPNM
                                            , REQEMPS, REQDUEDT, REQRMK
                                            , DOTYPE, DEVEMP, DEVEMP_DEVEMP.DEVEMPNM AS DEVEMPNM, DEVEMPS, TASKSTEP, TASKPROG, STEXDT, STDT, SPEXDT, SPDT, DEVRMK
		                                    , PJT_TASK.REGEMP, PJT_TASK.CHGEMP, PJT_TASK.REGDT, PJT_TASK.CHGDT 
                                            , DOCNO 
                                         FROM PJT_TASK left join
		                                      PJT_REQEMP on PJT_TASK.REQEMP = PJT_REQEMP.REQEMPSEQ left join
		                                      PJT_DEVEMP AS RCPEMP_DEVEMP on PJT_TASK.RCPTEMP = RCPEMP_DEVEMP.DEVEMPID left join
		                                      PJT_DEVEMP AS DEVEMP_DEVEMP on PJT_TASK.DEVEMP = DEVEMP_DEVEMP.DEVEMPID left join
		                                      PJT_COMCD AS REQDEPT_COMCD on PJT_TASK.REQDEPTSEQ = REQDEPT_COMCD.COMCD and REQDEPT_COMCD.UPCD = '30'
                                    WHERE TASKSEQ = '" + Request["taskID"].ToString() + "'";

                    //Response.Write(tasksql);
                    //Response.End();
                    cmd.CommandText = tasksql;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        hdnTaskSeq.Value = reader["TASKSEQ"].ToString();
                        hdnTaskId.Value = reader["TASKID"].ToString();
                        cboGnlSystemName.SelectedValue = reader["SYSNM"].ToString(); //시스템명
                        viewGnlSystemName.Text = cboGnlSystemName.Items[cboGnlSystemName.SelectedIndex].Text; //시스템명
                        cboGnlReqType.SelectedValue = reader["REQTYPE"].ToString(); //요청유형
                        viewGnlReqType.Text = cboGnlReqType.Items[cboGnlReqType.SelectedIndex].Text; //요청유형
                        cboGnlReqTool.SelectedValue = reader["REQTOOL"].ToString(); //요청방식
                        viewGnlReqTool.Text = cboGnlReqTool.Items[cboGnlReqTool.SelectedIndex].Text; //요청방식
                        txtRequestDate.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["REQDT"].ToString())); //요청일
                        viewRequestDate.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["REQDT"].ToString())); //요청일
                        hdnRequestDept.Value = reader["REQDEPTSEQ"].ToString(); //요청부서
                        txtRequestDept.Text = reader["REQDEPTNM"].ToString(); //요청부서
                        viewRequestDept.Text = reader["REQDEPTNM"].ToString(); //요청부서
                        hdnRequestBy.Value = reader["REQEMP"].ToString(); //요청자
                        txtRequestBy.Text = reader["REQEMPNM"].ToString(); //요청자
                        viewRequestBy.Text = reader["REQEMPNM"].ToString(); //요청자
                        hdnReceiptBy.Value = reader["RCPTEMP"].ToString(); //접수자
                        txtReceiptBy.Text = reader["RCPEMPNM"].ToString(); //접수자
                        viewReceiptBy.Text = reader["RCPEMPNM"].ToString(); //접수자
                        txtRequestBys.Text = reader["REQEMPS"].ToString(); //요청관련자
                        viewRequestBys.Text = reader["REQEMPS"].ToString(); //요청관련자
                        txtRequestDueDate.Text = reader["REQDUEDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["REQDUEDT"].ToString())); //완료요청일
                        viewRequestDueDate.Text = reader["REQDUEDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["REQDUEDT"].ToString())); //완료요청일
                        txtRequestContent.Text = reader["REQRMK"].ToString().ToString().Replace("™", "'"); //요청사항
                        viewRequestContent.Text = reader["REQRMK"].ToString().ToString().Replace("™", "'"); //요청사항
                        txtDocNo.Text = reader["DOCNO"].ToString(); //문서번호
                        viewDocNo.Text = reader["DOCNO"].ToString(); //문서번호

                        cboGubun.SelectedValue = reader["DOTYPE"].ToString(); //구분
                        viewGubun.Text = cboGubun.Items[cboGubun.SelectedIndex].Text; //구분
                        cboTaskStep.SelectedValue = reader["TASKSTEP"].ToString(); //진행단계
                        viewTaskStep.Text = cboTaskStep.Items[cboTaskStep.SelectedIndex].Text; //진행단계
                        hdnProcessBy.Value = reader["DEVEMP"].ToString(); //처리자
                        txtProcessBy.Text = reader["DEVEMPNM"].ToString(); //처리자
                        viewProcessBy.Text = reader["DEVEMPNM"].ToString(); //처리자
                        txtTaskProgress.Text = reader["TASKPROG"].ToString(); //진척률
                        viewTaskProgress.Text = reader["TASKPROG"].ToString(); //진척률
                        txtStartExpectDate.Text = reader["STEXDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["STEXDT"].ToString()));//시작예정일
                        viewStartExpectDate.Text = reader["STEXDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["STEXDT"].ToString()));//시작예정일
                        txtEndExpectDate.Text = reader["SPEXDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["SPEXDT"].ToString()));//종료예정일
                        viewEndExpectDate.Text = reader["SPEXDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["SPEXDT"].ToString()));//종료예정일
                        txtStartDate.Text = reader["STDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["STDT"].ToString()));//시작일
                        viewStartDate.Text = reader["STDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["STDT"].ToString()));//시작일
                        txtEndDate.Text = reader["SPDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["SPDT"].ToString()));//종료일
                        viewEndDate.Text = reader["SPDT"].ToString() == "" ? "" : string.Format("{0:yyyy-MM-dd}", DateTime.Parse(reader["SPDT"].ToString()));//종료일
                        txtPrcoessContent.Text = reader["DEVRMK"].ToString().Replace("™", "'"); //처리사항
                        viewPrcoessContent.Text = reader["DEVRMK"].ToString().Replace("™", "'"); //처리사항
                        txtProcessBys.Text = reader["DEVEMPS"].ToString(); //처리관련자
                        viewProcessBys.Text = reader["DEVEMPS"].ToString(); //처리관련자

                         //if (!string.IsNullOrEmpty(Request["MODE"]) && Request["MODE"] == "view")
                        if (!string.IsNullOrEmpty(Request["MODE"]) && Request["MODE"] == "view" && 1==2)
                        {
                            cboGnlSystemName.Visible = false;
                            viewGnlSystemName.Visible = true; //시스템명
                            cboGnlReqType.Visible = false; //요청유형
                            viewGnlReqType.Visible = true; //요청유형
                            cboGnlReqTool.Visible = false; //요청방식
                            viewGnlReqTool.Visible = true; //요청방식
                            txtRequestDate.Visible = false; //요청일
                            viewRequestDate.Visible = true; //요청일
                            txtRequestDept.Visible = false; //요청부서
                            viewRequestDept.Visible = true; //요청부서
                            txtRequestBy.Visible = false; //요청자
                            viewRequestBy.Visible = true; //요청자
                            txtReceiptBy.Visible = false; //접수자
                            viewReceiptBy.Visible = true; //접수자
                            txtRequestBys.Visible = false; //요청관련자
                            viewRequestBys.Visible = true; //요청관련자
                            txtRequestDueDate.Visible = false; //완료요청일
                            viewRequestDueDate.Visible = true; //완료요청일
                            txtRequestContent.Visible = false; //요청사항
                            viewRequestContent.Visible = true; //요청사항

                            cboGubun.Visible = false; //구분
                            viewGubun.Visible = true; //구분
                            cboTaskStep.Visible = false; //진행단계
                            viewTaskStep.Visible = true; //진행단계
                            txtProcessBy.Visible = false; //처리자
                            viewProcessBy.Visible = true; //처리자
                            txtTaskProgress.Visible = false; //진척률
                            viewTaskProgress.Visible = true; //진척률
                            txtStartExpectDate.Visible = false;//시작예정일
                            viewStartExpectDate.Visible = true;//시작예정일
                            txtEndExpectDate.Visible = false;//종료예정일
                            viewEndExpectDate.Visible = true;//종료예정일
                            txtStartDate.Visible = false;//시작일
                            viewStartDate.Visible = true;//시작일
                            txtEndDate.Visible = false;//종료일
                            viewEndDate.Visible = true;//종료일
                            txtPrcoessContent.Visible = false; //처리사항
                            viewPrcoessContent.Visible = true; //처리사항
                            txtProcessBys.Visible = false; //처리관련자
                            viewProcessBys.Visible = true; //처리관련자
                            txtDocNo.Visible = false; //문서번호
                            viewDocNo.Visible = true; //문서번호
                            lblDocNoEx.Visible = false; //문서번호예

                            btnDelete.Visible = false;
                            btnEndDate.Visible = false;
                            btnEndExpectDate.Visible = false;
                            btnProcessBy.Visible = false;
                            btnReceiptBy.Visible = false;
                            btnRequestBy.Visible = false;
                            btnRequestDate.Visible = false;
                            btnRequestDept.Visible = false;
                            btnRequestDuedate.Visible = false;
                            btnSave.Visible = false;
                            btnStartDate.Visible = false;
                            btnStartExpectDate.Visible = false;
                            btnDelete.Visible = false;

                            uploadPrcoessContent.Visible = false;
                            uploadRequestContent.Visible = false;
                            chkProcessFileName.Visible = false;
                            chkRequestFileName.Visible = false;

                        }
			else if(!string.IsNullOrEmpty(Request["MODE"]) && Request["MODE"] == "view" )
                        //else if(USERLVL.Equals('1'))
                        { // MCCC
                             
                            btnSave.Text = "수정";
                            btnDelete.Visible = false;
                        }
			else if(!string.IsNullOrEmpty(Request["MODE"]) && Request["MODE"] != "view" )
                        //else if(USERLVL.Equals('1'))
                        {  // 관리자
                             
                            btnSave.Text = "수정";
                            btnDelete.Visible = true;
                        }
                        else
                        {
                            btnSave.Text = "수정";
                            btnDelete.Visible = false;
                        }
                    }
                    reader.Dispose();
                    reader.Close();
                    cmd.Dispose();

                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    string attachsql = @"SELECT CONTABLE, CONSEQ, ATTSEQ, ORFILENM, SAFILENM, REGEMP, CHGEMP, REGDT, CHGDT , PROFILENM , PPROFILENM
                                           FROM PJT_ATTFILE
                                          WHERE CONTABLE = 'PJT_TASK' AND CONSEQ = '" + Request["taskID"].ToString() + "'";
                    cmd.CommandText = attachsql;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["SAFILENM"].ToString() != "" && reader["SAFILENM"].ToString().Substring(0, 2) == "R_")
                            {
                                lblRequestFileName.Text = "<a href=\"downloadFile.aspx?regdt=" + reader["REGDT"].ToString().Substring(0, 4) + "&originname=" + HttpUtility.UrlEncode(reader["ORFILENM"].ToString()) + "&savename=" + HttpUtility.UrlEncode(reader["SAFILENM"].ToString()) + "\" target=\"_new\" class=\"link\">" + reader["PROFILENM"].ToString() + "</a>";
                                hdnRequestFileRealName.Value = reader["SAFILENM"].ToString();
                            }
                            if (reader["PROFILENM"].ToString() != "" && reader["PPROFILENM"].ToString().Substring(0, 2) == "P_")
                            {
                                lblProcessFileName.Text = "<a href=\"downloadFile.aspx?regdt=" + reader["REGDT"].ToString().Substring(0, 4) + "&originname=" + HttpUtility.UrlEncode(reader["ORFILENM"].ToString()) + "&savename=" + HttpUtility.UrlEncode(reader["PPROFILENM"].ToString()) + "\" target=\"_new\" class=\"link\">" + reader["PROFILENM"].ToString() + "</a>";
                                hdnResponseFileRealName.Value = reader["PPROFILENM"].ToString();
                            }
                        }
                    }
                    reader.Dispose();
                    reader.Close();
                    cmd.Dispose();
                }
            }
            else
            {
                if (tempSystemName != "")
                {
                    cboGnlSystemName.SelectedIndex = Convert.ToInt32(tempSystemName);
                }
                else
                {
                    cboGnlSystemName.SelectedIndex = 0;
                }
                if (tempReqTool != "")
                {
                    cboGnlReqTool.SelectedIndex = Convert.ToInt32(tempReqTool);
                }
                else
                {
                    cboGnlReqTool.SelectedIndex = 0;
                }
                if (tempReqType != "")
                {
                    cboGnlReqType.SelectedIndex = Convert.ToInt32(tempReqType);
                }
                else
                {
                    cboGnlReqType.SelectedIndex = 0;
                }
                cboGubun.SelectedIndex = 0;
                cboTaskStep.SelectedIndex = 0;

                //lblRequestFileName.Visible = false;
                //chkRequestFileName.Visible = false;
                //lblProcessFileName.Visible = false;
                //chkProcessFileName.Visible = false;
            }
        }


    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        saveData();
        if (hdnTaskSeq.Value != "") //수정이면
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alert('수정되었습니다.')</script>");
        else //등록
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('저장되었습니다.','writeTask.aspx')</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alert('등록되었습니다.')</script>");
    }

    private void saveData()
    {
        string taskID = string.Empty;
        string system = string.Empty;
        string requestType = string.Empty;
        string requestTool = string.Empty;
        string requestDate = string.Empty;
        string requestDept = string.Empty;
        string requestBy = string.Empty;
        string receiptBy = string.Empty;
        string requestBys = string.Empty;
        string requestDueDate = string.Empty;
        string requestContent = string.Empty;
        string requestFile = string.Empty;
        string requestRealFile = string.Empty;
        string gubun = string.Empty;
        string taskstep = string.Empty;
        string processBy = string.Empty;
        string taskProgress = string.Empty;
        string startExpectDate = string.Empty;
        string endExpectDate = string.Empty;
        string startDate = string.Empty;
        string endDate = string.Empty;
        string prcessContent = string.Empty;
        string prcessFile = string.Empty;
        string prcessRealFile = string.Empty;
        string processBys = string.Empty;
        string remark = string.Empty;
        string docno = string.Empty;

        taskID = hdnTaskId.Value;
        system = cboGnlSystemName.Items[cboGnlSystemName.SelectedIndex].Value; //시스템명
        requestType = cboGnlReqType.Items[cboGnlReqType.SelectedIndex].Value; //요청유형
        requestTool = cboGnlReqTool.Items[cboGnlReqTool.SelectedIndex].Value; //요청방식
        requestDate = txtRequestDate.Text; //요청일
        requestDept = hdnRequestDept.Value; //요청부서
        requestBy = hdnRequestBy.Value; //요청자
        receiptBy = hdnReceiptBy.Value; //접수자
        requestBys = txtRequestBys.Text; //요청관련자
        requestDueDate = txtRequestDueDate.Text; //완료요청일
        requestContent = txtRequestContent.Text; //요청사항
        requestFile = uploadRequestContent.FileName; //원본파일명(요청사항)
        requestRealFile = saveFile(uploadRequestContent, "R"); //저장파일명(요청사항)

        gubun = cboGubun.Items[cboGubun.SelectedIndex].Value; //구분
        taskstep = cboTaskStep.Items[cboTaskStep.SelectedIndex].Value; //진행단계
        processBy = hdnProcessBy.Value; //처리자
        taskProgress = txtTaskProgress.Text; //진척률
        startExpectDate = txtStartExpectDate.Text; //시작예정일
        endExpectDate = txtEndExpectDate.Text; //종료예정일
        startDate = txtStartDate.Text; //시작일
        endDate = txtEndDate.Text; //종료일
        prcessContent = txtPrcoessContent.Text; //처리사항
        prcessFile = uploadPrcoessContent.FileName; //원본파일명(처리사항)
        prcessRealFile = saveFile(uploadPrcoessContent, "P"); //저장파일명(처리사항)
        processBys = txtProcessBys.Text; //처리관련자
        docno = txtDocNo.Text; //문서번호

        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string sql = string.Empty;
            string attachsql = string.Empty;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            if (!string.IsNullOrEmpty(taskID) && taskID.Trim() != "")
            {
                //Update
                sql = @"UPDATE PJT_TASK 
                           SET SYSNM = '" + system + @"'
		                         , REQTYPE = '" + requestType + @"'
		                         , REQTOOL = '" + requestTool + "'";
                if (requestDate == "")
                    sql = sql + ", REQDT = NULL";
                else
                    sql = sql + ", REQDT = '" + requestDate + @"'";

                sql = sql + " , REQDEPTSEQ = '" + requestDept + @"'";
                if (requestBy == "")
                    sql = sql + ", REQEMP = NULL";
                else
                    sql = sql + ", REQEMP = '" + requestBy + @"'";
                if (receiptBy == "")
                    sql = sql + ", RCPTEMP = NULL";
                else
                    sql = sql + ", RCPTEMP = '" + receiptBy + @"'";
                sql = sql + @"   , REQEMPS = '" + requestBys + @"'";
                if (requestDueDate == "")
                    sql = sql + ", REQDUEDT = NULL";
                else
                    sql = sql + ", REQDUEDT = '" + requestDueDate + @"'";
                sql = sql + @"
		                         , REQRMK = '" + requestContent.Trim().Replace("'", "™") + @"'
		                         , DOTYPE = '" + gubun + @"'";
                if (processBy == "")
                    sql = sql + ", DEVEMP = NULL";
                else
                    sql = sql + ", DEVEMP = '" + processBy + @"'";
                sql = sql + @"
		                         , DEVEMPS = '" + processBys + @"'
		                         , TASKSTEP = '" + taskstep + @"'
		                         , TASKPROG = '" + taskProgress + @"'";
                if (startExpectDate == "")
                    sql = sql + ", STEXDT = NULL";
                else
                    sql = sql + ", STEXDT = '" + startExpectDate + @"'";

                if (startDate == "")
                    sql = sql + ", STDT = NULL";
                else
                    sql = sql + ", STDT = '" + startDate + @"'";

                if (endExpectDate == "")
                    sql = sql + ", SPEXDT = NULL";
                else
                    sql = sql + ", SPEXDT = '" + endExpectDate + @"'";

                if (endDate == "")
                    sql = sql + ", SPDT = NULL";
                else
                    sql = sql + ", SPDT = '" + endDate + @"'";
                sql = sql + @"
		                         , DEVRMK = '" + prcessContent.Trim().Replace("'", "™") + @"'
                                 , DOCNO  = '" + docno + @"'
		                         , CHGEMP = '" + Request.Cookies["UserSettings"]["USERID"] + @"'
		                         , CHGDT = GETDATE()
                         WHERE TASKID = '" + taskID + @"';";
                //Response.Write(sql);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

           /*     //첨부파일목록을 수정해준다.
                if (chkRequestFileName.Checked == true || !string.IsNullOrEmpty(requestFile) && requestFile != "")
                {
                    deleteFile(hdnRequestFileRealName.Value);
                    cmd = new SqlCommand();
                    string deletesql = "DELETE FROM PJT_ATTFILE WHERE CONTABLE = 'PJT_TASK' AND CONSEQ = '" + hdnTaskSeq.Value + "' AND SAFILENM like 'R_%'";
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = deletesql;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
             */

            
                //첨부파일목록을 수정해준다.
                if (chkProcessFileName.Checked == true || !string.IsNullOrEmpty(prcessFile) && prcessFile != "")
                {
                    deleteFile(hdnResponseFileRealName.Value);
                    cmd = new SqlCommand();
                    string deletesql = "DELETE FROM PJT_ATTFILE WHERE CONTABLE = 'PJT_TASK' AND CONSEQ = '" + hdnTaskSeq.Value + "' AND SAFILENM like 'P_%'";
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = deletesql;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                //해당 TASKSEQ를 가져와서 첨부테이블에 넣어준다.
                if (!string.IsNullOrEmpty(requestFile) && requestFile != "")
                {
                    attachsql = @"INSERT INTO PJT_ATTFILE
                                     (CONTABLE, CONSEQ, ATTSEQ, ORFILENM, SAFILENM, REGEMP, CHGEMP, REGDT, CHGDT) 
                              SELECT 'PJT_TASK', TASKSEQ, 0, '" + requestFile + @"', '" + requestRealFile + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', GETDATE(), GETDATE() FROM PJT_TASK WHERE TASKSEQ = '" + hdnTaskSeq.Value + @"';";
                    cmd.Dispose();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = attachsql;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (!string.IsNullOrEmpty(prcessFile) && prcessFile != "")
                {
                   attachsql = @"INSERT INTO PJT_ATTFILE
                                     (CONTABLE, CONSEQ, ATTSEQ, ORFILENM, SAFILENM, REGEMP, CHGEMP, REGDT, CHGDT) 
                              SELECT 'PJT_TASK', TASKSEQ, 1, '" + requestFile + @"', '" + requestRealFile + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', GETDATE(), GETDATE() FROM PJT_TASK WHERE TASKSEQ = '" + hdnTaskSeq.Value + @"';";
                    
                    cmd.Dispose();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = attachsql;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

            }
            else
            {
                //Insert
                //TASKID의 max값을 가져온다.
                sql = @"INSERT INTO PJT_TASK (TASKID 
                                                   , SYSNM, REQTYPE, REQTOOL, REQDT, REQDEPTSEQ, REQEMP, RCPTEMP, REQEMPS, REQDUEDT, REQRMK 
                                                   , DOTYPE, DEVEMP, DEVEMPS, TASKSTEP, TASKPROG, STEXDT, STDT, SPEXDT, SPDT, DOCNO, DEVRMK
                                                   , REGEMP, CHGEMP, REGDT, CHGDT) 
                      SELECT '" + DateTime.Now.Year.ToString() + @"-' +RIGHT(RTRIM(LTRIM('000'+COALESCE(RIGHT(MAX(TASKID),4) + 1,1))),4)
                               , '" + system + @"', '" + requestType + @"', '" + requestTool + @"'";
                if (requestDate == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + requestDate + "'";

                sql = sql + ", '" + requestDept + @"'";

                if (requestBy == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + requestBy + "'";

                if (receiptBy == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + receiptBy + "'";

                sql = sql + " , '" + requestBys + "'";
                if (requestDueDate == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + requestDueDate + "'";

                sql = sql + ", '" + requestContent.Trim().Replace("'", "™") + @"'
                               , '" + gubun + @"', '" + processBy + @"', '" + processBys + @"', '" + taskstep + @"', '" + taskProgress + "'";
                if (startExpectDate == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + startExpectDate + "'";

                if (startDate == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + startDate + "'";

                if (endExpectDate == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + endExpectDate + "'";

                if (endDate == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + endDate + "'";

                if (docno == "")
                    sql = sql + " , " + "NULL";
                else
                    sql = sql + " , " + "'" + docno + "'";

                sql = sql + " , '" + prcessContent.Trim().Replace("'", "™") + @"'
                               , '" + Request.Cookies["UserSettings"]["USERID"] + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', GETDATE(), GETDATE() FROM PJT_TASK;";
                cmd.CommandType = CommandType.Text;
                //Response.Write(sql);
                //Response.End();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();


                //해당 TASKSEQ를 가져와서 첨부테이블에 넣어준다.
                if (!string.IsNullOrEmpty(requestFile) && requestFile != "")
                {
                    attachsql = @"INSERT INTO PJT_ATTFILE
                                     (CONTABLE, CONSEQ, ATTSEQ, ORFILENM, SAFILENM, REGEMP, CHGEMP, REGDT, CHGDT) 
                              SELECT 'PJT_TASK', MAX(TASKSEQ), 0, '" + requestFile + @"', '" + requestRealFile + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', GETDATE(), GETDATE() FROM PJT_TASK WHERE REGEMP = '" + Request.Cookies["UserSettings"]["USERID"] + @"' AND REQRMK = '" + requestContent.Trim() + @"';";
                    cmd.Dispose();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = attachsql;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (!string.IsNullOrEmpty(prcessFile) && prcessFile != "")
                {
                    attachsql = @"INSERT INTO PJT_ATTFILE
                                     (CONTABLE, CONSEQ, ATTSEQ, ORFILENM, SAFILENM, REGEMP, CHGEMP, REGDT, CHGDT) 
                              SELECT 'PJT_TASK', MAX(TASKSEQ), 1, '" + prcessFile + @"', '" + prcessRealFile + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', '" + Request.Cookies["UserSettings"]["USERID"] + @"', GETDATE(), GETDATE() FROM PJT_TASK WHERE REGEMP = '" + Request.Cookies["UserSettings"]["USERID"] + @"' AND REQRMK = '" + requestContent.Trim() + @"';";
                    cmd.Dispose();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = attachsql;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
        }

    }

    private string saveFile(FileUpload requestFile, string type)
    {
        string realname = string.Empty;
        try
        {
            string targetPath = string.Empty;
            targetPath = ConfigurationManager.AppSettings["filepath"];

            //해당 위치에 파일이 있는지 확인한다.
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(targetPath + "\\" + DateTime.Now.Year);
            if (!oDirectoryInfo.Exists)
                oDirectoryInfo.Create();

            FileInfo oFileInfo = new FileInfo(oDirectoryInfo.FullName + "\\" + type + "_" + requestFile.FileName);
            int cnt = 1;
            if (requestFile.HasFile)
            {
                if (!oFileInfo.Exists)
                {
                    requestFile.SaveAs(oDirectoryInfo.FullName + "\\" + type + "_" + requestFile.FileName);
                    realname = type + "_" + requestFile.FileName;
                }
                else
                {
                    while (oFileInfo.Exists)
                    {
                        oFileInfo = new FileInfo(oDirectoryInfo.FullName + "\\" + type + "_" + requestFile.FileName + "(" + cnt.ToString() + ")");
                        cnt = cnt + 1;
                    }
                    requestFile.SaveAs(oDirectoryInfo.FullName + "\\" + type + "_" + requestFile.FileName + "(" + (cnt - 1).ToString() + ")");
                    realname = type + "_" + requestFile.FileName + "(" + (cnt - 1).ToString() + ")";
                }
            }
            return realname;
        }
        catch (Exception ex)
        {
            return "error:" + ex.Message;
        }
    }

    private bool deleteFile(string filename)
    {
        try
        {
            string targetPath = string.Empty;
            targetPath = ConfigurationManager.AppSettings["filepath"];

            //해당 위치에 파일이 있는지 확인한다.
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(targetPath + "\\" + DateTime.Now.Year);
            FileInfo oFileInfo = new FileInfo(oDirectoryInfo.FullName + "\\" + filename);

            if (oFileInfo.Exists)
                oFileInfo.Delete();

            return true;
        }
        catch (Exception e)
        {
            return true;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string taskID = string.Empty;
        string taskSeq = string.Empty;

        taskID = hdnTaskId.Value;
        taskSeq = hdnTaskSeq.Value;

        // Retrieve the connection string stored in the Web.config file.
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string querystring = string.Empty;

        //개인정보 수정
        querystring = @"delete from pjt_task
                         where TASKSEQ = '" + taskSeq + "'";

        //이게 insert인지 update건인지 확인해서 처리
        SqlConnection conn = new SqlConnection(connectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = querystring;
        cmd.ExecuteNonQuery();
        cmd.Dispose();

        string requestFileRealName = hdnRequestFileRealName.Value;
        string responseFileRealName = hdnResponseFileRealName.Value;

        if (!string.IsNullOrEmpty(requestFileRealName) && requestFileRealName != "")
            deleteFile(requestFileRealName);
        if (!string.IsNullOrEmpty(responseFileRealName) && responseFileRealName != "")
            deleteFile(responseFileRealName);

        string attachsql = "delete from PJT_ATTFILE where CONTABLE = 'PJT_TASK' AND CONSEQ = '" + taskSeq + "'";
        cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = querystring;
        cmd.ExecuteNonQuery();
        cmd.Dispose();

        conn.Dispose();
        conn.Close();

        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alert('저장되었습니다.');openTask();</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>alertMessage('삭제되었습니다.','writeTask.aspx')</script>");

    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("readTask.aspx");
    }
    protected void btnWbs_Click(object sender, EventArgs e)
    {
        Response.Redirect("readWBS.aspx");
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyScript", "<script language='javascript'>document.location.href('writeTask.aspx');</script>");
    }
}
