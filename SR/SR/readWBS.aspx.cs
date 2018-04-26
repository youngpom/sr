using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.IO;

public partial class tesklistb : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        chkUserId();

        string developer = string.Empty;
        string reqdept = string.Empty;
        string taskstep = string.Empty;
        string requser = string.Empty;
        string startdate = string.Empty;
        string enddate = string.Empty;
        DateTime calDate ;
        DateTime lastDate;
        DateTime firstDate;
        bool mydata = false;
        bool ingdata = false;
        int paging = 0;

        Cache["CellWidth"] = "19"; // 스케줄 출력시 셀가로길이 px


        if (txtStartTime.Text == "") txtStartTime.Text = string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(-2));
        if (txtEndTime.Text == "") txtEndTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse( string.Format("{0:yyyy-MM-01}", DateTime.Now)).AddMonths(3).AddDays(-1));


        //시작조회일(startdate) 결정
        calDate = DateTime.Parse( txtStartTime.Text );
        lastDate = DateTime.Parse( string.Format("{0:yyyy-MM-01}", calDate ) ).AddDays(-1); //전월말일
        firstDate =  DateTime.Parse( string.Format("{0:yyyy-MM-01}", DateTime.Parse( string.Format("{0:yyyy-MM-01}", calDate ) ).AddDays(-1))); //전월초일
        if( calDate.AddDays( 7 - (int)calDate.DayOfWeek ) == lastDate.AddDays( 7 - (int)lastDate.DayOfWeek ) ) // '시작조회일 주말일요일' == '전월말의 주말일요일' // 즉 같은 주이면
        {
            startdate = string.Format( "{0:yyyy-MM-dd}", firstDate.AddDays( 7 - (int)firstDate.DayOfWeek   ));
        }else{
            startdate = string.Format("{0:yyyy-MM-dd}", lastDate.AddDays(7 - (int)lastDate.DayOfWeek));
        }
        
        //종료조회일 결정
        calDate = DateTime.Parse( txtEndTime.Text );
        lastDate = DateTime.Parse( string.Format("{0:yyyy-MM-01}", calDate.AddMonths(2) ) ).AddDays(-1); //익월말일
        firstDate = DateTime.Parse( string.Format("{0:yyyy-MM-01}", calDate.AddMonths(1) ) ); //익월초일
        if( calDate.AddDays( 7 - (int)calDate.DayOfWeek ) ==  firstDate.AddDays( 7 - (int)firstDate.DayOfWeek ) ) // '종료조회일 주말일요일' == '익월초일의 주말일요일 // 즉 같은 주이면
        {
            enddate = string.Format("{0:yyyy-MM-dd}", lastDate.AddDays( 7 - (int)lastDate.DayOfWeek ));
        }else{
            enddate = string.Format("{0:yyyy-MM-dd}", firstDate.AddDays(7 - (int)firstDate.DayOfWeek));
        }


        if (!IsPostBack)
        {

            string usersql = @"select devempid, devempnm from pjt_devemp order by devempid";

            DataTable userdt = GetDataTable(usersql);
            if (userdt.Rows.Count > 0)
            {
                makeSelectList(cboDeveloper, userdt, "devempnm", "devempid");
            }
            userdt.Dispose();

            ///진행단계list를 만들어준다.
            string taskstepsql = @"select comcd, comcdnm from pjt_comcd where upcd = '40' order by comcd";
            DataTable taskstepdt = GetDataTable(taskstepsql);
            if (taskstepdt.Rows.Count > 0)
            {
                makeSelectList(cboTaskStep, taskstepdt, "comcdnm", "comcd");
            }
            taskstepdt.Dispose();

            //writeTask넘어온 parameter들의 값을 넣어준다.
            if(!String.IsNullOrEmpty(Request["taskID"]) && Request["taskID"] != "")
            {
                cboTaskStep.SelectedValue = Request["hdnTaskStep"];
                cboDeveloper.SelectedValue = Request["hdnDeveloper"];
                cboGnlReqTool.SelectedValue = Request["hdnReqTool"];
                txtStartTime.Text = Request["hdnStartTime"];
                txtEndTime.Text = Request["hdnEndTime"];
                hdnRequestDept.Value = Request["hdnRequestDept"];
                txtRequestDept.Text = Request["hdnRequestDeptName"];
                hdnRequestBy.Value = Request["hdnRequestBy"];
                txtRequestBy.Text = Request["hdnRequestByName"];
                chkMyData.Checked = bool.Parse(Request["hdnMyData"]);
                chkIngData.Checked = bool.Parse(Request["hdnIngData"]);
                paging = int.Parse(Request["hdnPaging"]);
            }
            
            developer = cboDeveloper.Items[cboDeveloper.SelectedIndex].Value;
            reqdept = hdnRequestDept.Value;
            taskstep = cboTaskStep.Items[cboTaskStep.SelectedIndex].Value;
            requser = hdnRequestBy.Value;

        }
        else {}

        //startdate = txtStartTime.Text;
        //enddate = txtEndTime.Text;
        mydata = chkMyData.Checked == true ? true : false;
        ingdata = chkIngData.Checked == true ? true : false;

        listCalendar.PageIndex = paging;
        MakeGridview(developer, reqdept, taskstep, requser, startdate, enddate, mydata, ingdata);
    }



    private void MakeGridview(string developer, string reqdept, string taskstep, string requser, string startdate, string enddate, bool mydata, bool ingdata)
    {
        string sql = @"select taskseq,  '[' || to_char(REQDT,'mm-dd') || '][' || substr(NVL(DOCNO,''),6,10) || ']  ' || REQRMK as REQRMK, SYSNM_COMCD.COMCDNM as SYSNM, REQDEPT_COMCD.comcdnm as REQDEPTNM, devempnm, TASKSTEP_COMCD.comcdnm as taskstep, taskprog, 
                              stexdt, stdt, spexdt, spdt
                         from pjt_task left join 
	                          pjt_devemp on pjt_task.devemp = pjt_devemp.devempid left join
			                  pjt_comcd as REQDEPT_COMCD on pjt_task.reqdeptseq = REQDEPT_COMCD.comcd and REQDEPT_COMCD.upcd = '30' left join
                              PJT_COMCD AS SYSNM_COMCD on PJT_TASK.SYSNM = SYSNM_COMCD.COMCD and SYSNM_COMCD.UPCD = '50' left join
                              PJT_COMCD AS TASKSTEP_COMCD on PJT_TASK.TASKSTEP = TASKSTEP_COMCD.COMCD and TASKSTEP_COMCD.UPCD = '40'
                        where taskseq is not null and PJT_TASK.REQTOOL = '602' ";
        if (developer != "" && developer != "전체")
            sql = sql + " and pjt_task.devemp = '" + developer + "'";
        if (reqdept != "")
            sql = sql + " and pjt_task.REQDEPTSEQ = '" + reqdept + "'";
        if (taskstep != "" && taskstep != "전체")
            sql = sql + " and pjt_task.taskstep = '" + taskstep + "'";
        if (requser != "")
            sql = sql + " and pjt_task.reqemp = '" + requser + "'";
        if (startdate != "")
            sql = sql + " and (PJT_TASK.STDT >= '" + startdate + "' or PJT_TASK.STEXDT >= '" + startdate + "' or REQDT >= '" + startdate + "' or REQDUEDT >= '" + startdate + "')";
        if (enddate != "")
            sql = sql + " and (PJT_TASK.SPDT <= '" + enddate + "' or PJT_TASK.SPEXDT <= '" + enddate + "' or REQDT <= '" + enddate + "' or REQDUEDT <= '" + enddate + "')";
        if (mydata == true)//
            sql = sql + " and (PJT_TASK.RCPTEMP = '" + Request.Cookies["UserSettings"]["USERID"] + "' or PJT_TASK.DEVEMP = '" + Request.Cookies["UserSettings"]["USERID"] + "')";
        if (ingdata == true)//
            sql = sql + " and TASKSTEP_COMCD.COMCDNM != '처리완료'";
        sql = sql + "   order by taskid desc";
        //Response.Write(sql);
        DataSet ds = GetData(sql);


        ///해당기간의 dataset을 가져와서 달력을 그려준다.
        ds = makeWBS(ds, startdate, enddate);


        if (ds.Tables.Count > 0)
        {
            listCalendar.EmptyDataText = "조회하신 데이터가 없습니다.";
            listCalendar.Width = Unit.Parse((260 + ((ds.Tables[0].Columns.Count - 9) * 20)).ToString());
            listCalendar.DataSource = ds;
            listCalendar.DataBind();
        }
        else
        {
            //Message.Text = "Unable to connect to the database.";
        }
        ds.Dispose();
    }



    /// <summary>
    /// 해당 기간에 맞게 달력을 그려준다.
    /// </summary>
    /// <param name="ds">해당 데이터테이블</param>
    /// <param name="startdate">시작일</param>
    /// <param name="enddate">종료일</param>
    private DataSet makeWBS(DataSet ds, string startdate, string enddate)
    {

        DateTime sDate = DateTime.Parse(startdate);
        DateTime eDate = DateTime.Parse(enddate);
        DateTime tDate = sDate; //해당 주의 첫날을 setting
        int weeknum = (eDate - sDate).Days / 7 + 1; ///조회하는 기간의 주수를 계산한다.
        Cache["NowWeek"] = (int)((DateTime.Now.Date - sDate).Days / 7) + 11;

        DateTime stexdt = new DateTime(); //시작예정일
        DateTime stdt = new DateTime(); //시작일
        DateTime spexdt = new DateTime(); //종료예정일
        DateTime spdt = new DateTime();//종료일

        string mrkflg = ""; //마크플래그 
        if (ds.Tables.Count > 0)
        {
            ///Header를 추가
            for (int cnt = 0; cnt < weeknum; cnt++)
            {
                ds.Tables[0].Columns.Add(string.Format("{0:yyyy-MM}", tDate) + " " + cnt.ToString());
                tDate = tDate.AddDays(7);
            }

            ///content 추가
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    stexdt = DateTime.Now.Date;
                    stdt = DateTime.Now.Date;
                    spexdt = DateTime.Now.Date;
                    spdt = DateTime.Now.Date;// 종료일이 없으면 현재일(현재까지 진행표기를 위해)

                    object[] aryRow = oRow.ItemArray;
                    if (!string.IsNullOrEmpty(aryRow[7].ToString()) && aryRow[7].ToString() != "") stexdt = DateTime.Parse(aryRow[7].ToString());
                    if (!string.IsNullOrEmpty(aryRow[8].ToString()) && aryRow[8].ToString() != "") stdt = DateTime.Parse(aryRow[8].ToString());
                    if (!string.IsNullOrEmpty(aryRow[9].ToString()) && aryRow[9].ToString() != "") spexdt = DateTime.Parse(aryRow[9].ToString());
                    if (!string.IsNullOrEmpty(aryRow[10].ToString()) && aryRow[10].ToString() != "") spdt = DateTime.Parse(aryRow[10].ToString());

                    int diff_stexdt = (stexdt - sDate).Days / 7 < 0 ? 0 : (stexdt - sDate).Days / 7; //시작예정일의 시작점
                    int diff_stdt = (stdt - sDate).Days / 7 < 0 ? 0 : (stdt - sDate).Days / 7; //시작일의 시작점
                    int diff_spexdt = (spexdt - sDate).Days / 7 < 0 ? 0 : (spexdt - sDate).Days / 7; //종료예정일의 시작점
                    int diff_spdt = (spdt - sDate).Days / 7 < 0 ? 0 : (spdt - sDate).Days / 7; //종료일의 시작점

                    for (int i = 0; i < weeknum; i++)
                    {
                        mrkflg = "";
                        if (diff_stexdt <= i && i <= diff_spexdt) mrkflg = "PLAN";
                        if (diff_stdt <= i && i <= diff_spdt) mrkflg = mrkflg + "ACTUAL";

                        // 시작예정일 / 종료예정일 둘다 있으면 계획스케줄 표시
                        if (mrkflg == "PLAN" && !string.IsNullOrEmpty(aryRow[7].ToString()) && aryRow[7].ToString() != "" && !string.IsNullOrEmpty(aryRow[9].ToString()) && aryRow[9].ToString() != "") oRow[i + 11] = "image/dot_plan_line.GIF";

                        // 시작일 / 종료일(진행중인 경우는 현재일로봄) 있으면 실행스케줄 표시
                        if (mrkflg == "ACTUAL" && !string.IsNullOrEmpty(aryRow[8].ToString()) && aryRow[7].ToString() != "") oRow[i + 11] = "image/dot_actual_line.GIF";

                        // 시작예정일 / 종료예정일 / 시작일 / 종료일 전부 있으면 계획/실행 스케줄 표시 
                        if (mrkflg == "PLANACTUAL" && !string.IsNullOrEmpty(aryRow[7].ToString()) && aryRow[7].ToString() != "" && !string.IsNullOrEmpty(aryRow[9].ToString()) && aryRow[9].ToString() != "" && !string.IsNullOrEmpty(aryRow[8].ToString()) && aryRow[7].ToString() != "") oRow[i + 11] = "image/dot_both_line.GIF";

                        // 시작예정일만 있으면 시작예정일부터 끝까지 계획표시
                        if (diff_stexdt <= i &&
                            (string.IsNullOrEmpty(aryRow[7].ToString()) || aryRow[7].ToString() == ""
                            || string.IsNullOrEmpty(aryRow[9].ToString()) || aryRow[9].ToString() == ""
                            )
                           ) oRow[i + 11] = "image/dot_plan_line.GIF";

                    }

                }
            }
        } //if (ds.Tables.Count > 0)
        else
        {
            DataTable oTable = new DataTable();

            DataColumn taskColumn = new DataColumn("작업명", typeof(string));
            DataColumn requestdeptColumn = new DataColumn("요청부서", typeof(string));
            DataColumn developerColumn = new DataColumn("담당자", typeof(string));
            oTable.Columns.Add(taskColumn);
            oTable.Columns.Add(requestdeptColumn);
            oTable.Columns.Add(developerColumn);
            for (int cnt = 0; cnt <= weeknum; cnt++)
            {
                tDate = tDate.AddDays(6);
                oTable.Columns.Add(string.Format("{0:yyyy-MM}", tDate) + " " + cnt.ToString());
            }
            oTable.Rows.Add("", "", "");
            ds.Tables.Add(oTable);
        }
        return ds;
    }




    protected void listCalendar_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            listCalendarHeader_RowCreated(sender, e);
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Style["height"] = "40px";
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
            int cnt = 11;
            for (cnt = 11; cnt < e.Row.Cells.Count; cnt++)
            {
                e.Row.Cells[cnt].Width = Unit.Parse(Cache["CellWidth"].ToString());
                e.Row.Cells[cnt].Attributes.Add("style", "padding: 0 0 0 0 !important;");
            }
            e.Row.Cells[cnt - 1].Visible = false;
        }
    }



    protected void listCalendarHeader_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Text = "제목";
            e.Row.Cells[1].Width = Unit.Parse("300");
            e.Row.Cells[2].Visible = false;//작업명
            e.Row.Cells[3].Visible = false;//요청부서
            e.Row.Cells[4].Text = "담당자";
            e.Row.Cells[4].Width = Unit.Parse("45");
            e.Row.Cells[5].Text = "진행단계";
            e.Row.Cells[5].Width = Unit.Parse("55");
            e.Row.Cells[6].Visible = false; //진척률
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;

            ///월의 경우 해당 월은 합쳐져 보여야하므로 ColumnSpan처리
            if (e.Row.Cells.Count > 11)
            {
                string tempstr = string.Empty;
                int tempcnt = 0;
                int cnt = 11;
                for (cnt = 11; cnt < e.Row.Cells.Count; cnt++)
                {
                    if (e.Row.Cells[cnt].Text.Substring(0, 7) == tempstr)
                    {
                        tempcnt = tempcnt + 1;
                        e.Row.Cells[cnt].Visible = false;
                    }
                    else
                    {
                        e.Row.Cells[cnt].Text = e.Row.Cells[cnt].Text.Substring(0, 7);
                        if (tempcnt > 0)
                            e.Row.Cells[cnt - tempcnt - 1].ColumnSpan = tempcnt + 1;
                        for (int hdncnt = 0; hdncnt < tempcnt; hdncnt++)
                        {

                        }
                        tempcnt = 0;
                    }
                    tempstr = e.Row.Cells[cnt].Text.Substring(0, 7);
                    e.Row.Cells[cnt].Width = Unit.Parse(Cache["CellWidth"].ToString());
                }
                e.Row.Cells[cnt - 1].Visible = false;
            }
        }
    }


    protected void listCalendar_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string stexdt = string.Empty;//시작예정일
        string stdt = string.Empty;//시작일
        string spexdt = string.Empty;//종료예정일
        string spdt = string.Empty;//종료일

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onClick"] = "javascript: openTask(''," + e.Row.Cells[0].Text + ");";
            e.Row.Attributes["onMouseover"] = "this.className='trMouseOver';";
            e.Row.Attributes["onMouseout"] = "this.className='trMouseOut';";
            for (int cnt = 0; cnt < e.Row.Cells.Count; cnt++)
            {
                if (e.Row.Cells[cnt].Text.Contains("image/"))
                {

                    stexdt = "";
                    stdt = "";
                    spexdt = "";
                    spdt = "";

                    if (!string.IsNullOrEmpty(e.Row.Cells[7].Text) && e.Row.Cells[7].Text.Trim() != "&nbsp;") stexdt = e.Row.Cells[7].Text.Substring(5,5).Replace("-","/");
                    if (!string.IsNullOrEmpty(e.Row.Cells[8].Text) && e.Row.Cells[8].Text.Trim() != "&nbsp;") stdt = e.Row.Cells[8].Text.Substring(5, 5).Replace("-", "/");
                    if (!string.IsNullOrEmpty(e.Row.Cells[9].Text) && e.Row.Cells[9].Text.Trim() != "&nbsp;") spexdt = e.Row.Cells[9].Text.Substring(5, 5).Replace("-", "/");
                    if (!string.IsNullOrEmpty(e.Row.Cells[10].Text) && e.Row.Cells[10].Text.Trim() != "&nbsp;") spdt = e.Row.Cells[10].Text.Substring(5, 5).Replace("-", "/");

                    Image oImage = new Image();
                    oImage.ImageUrl = e.Row.Cells[cnt].Text;
                    oImage.Attributes["width"] = "100%";
                    oImage.Attributes["height"] = "200%";
                    oImage.Attributes["alt"] = "[예정] : " + stexdt + " ~ " + spexdt + "\n" + "[실행] : " + stdt + " ~ " + spdt;
                    e.Row.Cells[cnt].Controls.Add(oImage);
                }
            }

            if (e.Row.Cells[9].Text.Trim() != "" && e.Row.Cells[9].Text.Length > 10)
            {
                if (DateTime.Parse(e.Row.Cells[9].Text) < DateTime.Now && e.Row.Cells[5].Text.Trim() != "처리완료")
                {
                    e.Row.Cells[5].Text = "<font color=\"red\">지연</font>";
                }
            }
            //제목내용길이 제한
            if (e.Row.Cells[1].Text.Trim() != "" && e.Row.Cells[1].Text.Length > 60)
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 60) + "~~~~~";

            // 2017-11-13 오류로 일단 막음
            //e.Row.Cells[ Int32.Parse(Cache["NowWeek"].ToString() ) ].Attributes["onMouseout"] = "this.className='trNow';";
            //e.Row.Cells[ Int32.Parse(Cache["NowWeek"].ToString() ) ].Attributes["onMouseover"] = "this.className='trNow';";
            //e.Row.Cells[ Int32.Parse(Cache["NowWeek"].ToString() ) ].Attributes["class"] = "trNow";

        }
    }



    protected void listCalendar_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        listCalendar.PageIndex = e.NewPageIndex;
        pagingNo.Value = e.NewPageIndex.ToString();

        btnSearch_Click( sender, e);
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string developer = string.Empty;
        string reqdept = string.Empty;
        string taskstep = string.Empty;
        string requser = string.Empty;
        string startdate = string.Empty;
        string enddate = string.Empty;
        bool mydata = false;
        bool ingdata = false;

        developer = cboDeveloper.Items[cboDeveloper.SelectedIndex].Value;
        reqdept = hdnRequestDept.Value;
        taskstep = cboTaskStep.Items[cboTaskStep.SelectedIndex].Value;
        requser = hdnRequestBy.Value;
        startdate = txtStartTime.Text;
        enddate = txtEndTime.Text;
        mydata = chkMyData.Checked == true ? true : false;
        ingdata = chkIngData.Checked == true ? true : false;

        Page_Load(sender, e);
        //MakeGridview(developer, reqdept, taskstep, requser, startdate, enddate, mydata, ingdata);

    }


    protected void listCalendar_DataBinding(object sender, EventArgs e)
    {

    }


}
