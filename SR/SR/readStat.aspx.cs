using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

public partial class readStat : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        chkUserId();

        string developer = string.Empty;
        string reqdept = string.Empty;
        string taskstep = string.Empty;
        string reqtool = string.Empty;
        string requser = string.Empty;
        string startdate = string.Empty;
        string enddate = string.Empty;
        bool mydata = false;
        bool ingdata = false;
        string reqrmk = string.Empty;
        string devrmk = string.Empty;
        string docno = string.Empty;

        if (txtStartTime.Text == "")
            txtStartTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(DateTime.Now.Year + "-01-01"));
            //txtStartTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMonths(-1));
        if (txtEndTime.Text == "")
            txtEndTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(DateTime.Now.Year + "-12-31"));
            //txtEndTime.Text = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

        if (!IsPostBack)
        {
            //처리자콤보
            string usersql = @"select devempid, devempnm from pjt_devemp order by devempid";
            // Retrieve the connection string stored in the Web.config file.
            DataTable userdt = GetDataTable(usersql);
            if (userdt.Rows.Count > 0)
            {
               // makeSelectList(cboDeveloper, userdt, "devempnm", "devempid");
            }
            userdt.Clear();
            userdt.Dispose();
           
            //GridView 출력

            startdate = txtStartTime.Text;
            enddate = txtEndTime.Text;

            MakeGridview( startdate, enddate );
            MakeGridview2(startdate, enddate);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string startdate = string.Empty;
        string enddate = string.Empty;

        startdate = txtStartTime.Text;
        enddate = txtEndTime.Text;

        MakeGridview(startdate, enddate);
        MakeGridview2(startdate, enddate);
    }


    private void MakeGridview( string startdate, string enddate)
    {
        //주간작업
        string sql = @"
select '(' +
        isnull(cast( sum( aa.완료건 ) as varchar(3)), '') + '/' + 
        isnull(cast( sum( aa.접수건 ) as varchar(3)), '') + ')' as 총계
      , bb.요청형태
      , '(' + isnull(rtrim(ltrim( cast( sum(case when aa.요청형태= bb.요청형태 then aa.완료건 else 0 end ) as varchar(3)))), '')  + '/' + 
               isnull(rtrim(ltrim( cast( sum(case when aa.요청형태= bb.요청형태 then aa.접수건 else 0 end ) as varchar(3)))), '') + ')'  as 형태별소계
      , bb.파트 AS 업무파트
      , isnull(rtrim(ltrim( cast( sum(case when aa.파트= bb.파트 and aa.요청형태= bb.요청형태 then aa.완료건 else 0 end ) as varchar(3)))), '')  + '/' + 
        isnull(rtrim(ltrim( cast( sum(case when aa.파트= bb.파트 and aa.요청형태= bb.요청형태 then aa.접수건 else 0 end ) as varchar(3)))), '') + '건' as 파트별소계
from (
        select ty.comcdnm as 요청형태
             , case when de.optb in ('원무', '보험') then '1.원무/보험'
                    when de.optb in ('외래', '병동') then '2.외래/병동'
                    when de.optb in ('진료', '진료지원') then '3.진료/지원' 
                    when de.optb in ('사무국' ) then '4.사무국' 
                    else '5.그외'  end   as 파트
             , sum( case  when pj.taskstep = '403' then 1 else 0 end ) as 완료건
             , count(*) as 접수건
         from PJT_TASK pj left outer join PJT_COMCD de on pj.REQDEPTSEQ = de.COMCD
                          left outer join PJT_COMCD st on pj.taskstep = st.comcd
                          left outer join PJT_COMCD ty on pj.reqtool = ty.comcd ";
        sql = sql + "  where reqdt between  '" + startdate + "' and '" + enddate + "'";
        sql = sql + @"
      group by ty.comcdnm 
             , case when de.optb in ('원무', '보험') then '1.원무/보험'
                    when de.optb in ('외래', '병동') then '2.외래/병동'
                    when de.optb in ('진료', '진료지원') then '3.진료/지원' 
                    when de.optb in ('사무국' ) then '4.사무국' 
                    else '5.그외'  end
      ) aa ,
      (
      select 요청형태, 파트, 0 as 완료건, 0 as 접수건  from
          ( select '문서' as 요청형태 from sysibm.sysdummy1 union all
            select '전화' as 요청형태 
          ) a,
          ( select '1.원무/보험' as 파트 from sysibm.sysdummy1 union all
            select '2.외래/병동' as 파트 all
            select '3.진료/지원' as 파트 all
            select '4.사무국'    as 파트 all
            select '5.그외'      as 파트
          ) b
      ) bb
group by  bb.요청형태, bb.파트 ";


        //Response.Write(sql);
        DataTable dt = GetDataTable(sql);
        if (dt.Rows.Count >= 0)
        {
            listTask.EmptyDataText = "조회하신 데이터가 없습니다.";
            listTask.DataSource = dt;
            listTask.DataBind();
        }
        dt.Clear();
        dt.Dispose();
    }

    protected void listTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Style["text-align"] = "right";
            e.Row.Cells[2].Style["text-align"] = "right";
            e.Row.Cells[4].Style["text-align"] = "right";
        }
    }

    private void MakeGridview2(string startdate, string enddate)
    {
        //월간 작업
        string sql = @"
select '(' ||
        cast( sum( aa.완료건 ) as varchar(3)) || '/' || 
        cast( sum( aa.접수건 ) as varchar(3)) || ')' as 총계
      , bb.요청형태
      , '(' || trim( cast( sum(case when aa.요청형태= bb.요청형태 then aa.완료건 else 0 end ) as varchar(3)))  || '/' || 
               trim( cast( sum(case when aa.요청형태= bb.요청형태 then aa.접수건 else 0 end ) as varchar(3))) || ')'  as 형태별소계
      , bb.처리사항
      , trim( cast( sum(case when aa.처리사항= bb.처리사항 and aa.요청형태= bb.요청형태 then aa.완료건 else 0 end ) as varchar(3)))  || '/' || 
        trim( cast( sum(case when aa.처리사항= bb.처리사항 and aa.요청형태= bb.요청형태 then aa.접수건 else 0 end ) as varchar(3))) || '건' as 소계
from (
        select ty.comcdnm as 요청형태
             , DECODE(dt.comcdnm, '자료정정', '1.자료정정'
                                ,'장비점검','2.장비점검'
                                ,'자료확인','3.자료확인'
                                , '프로그램수정(루틴포함)','4.PG루틴'
                                , '5.기타'  ) as 처리사항
             , sum( case  when pj.taskstep = '403' then 1 else 0 end ) as 완료건
             , count(*) as 접수건
         from PJT_TASK pj left outer join PJT_COMCD de on pj.REQDEPTSEQ = de.COMCD
                          left outer join PJT_COMCD st on pj.taskstep = st.comcd
                          left outer join PJT_COMCD ty on pj.reqtool = ty.comcd
                          left outer join PJT_COMCD dt on pj.dotype = dt.comcd ";
         sql = sql + "where reqdt between '" + startdate + "' and '" + enddate + "'";
      sql = sql + @" group by ty.comcdnm, DECODE(dt.comcdnm, '자료정정', '1.자료정정'
                                ,'장비점검','2.장비점검'
                                ,'자료확인','3.자료확인'
                                , '프로그램수정(루틴포함)','4.PG루틴'
                                , '5.기타'  )
      ) aa ,
     (
        select 요청형태, 처리사항 from 
          ( select '문서' as 요청형태 from sysibm.sysdummy1 union all
            select '전화' as 요청형태 from sysibm.sysdummy1 
          ) a,
          ( select '1.자료정정' as 처리사항 from sysibm.sysdummy1 union all
            select '2.장비점검' as 처리사항 from sysibm.sysdummy1 union all
            select '3.자료확인' as 처리사항 from sysibm.sysdummy1 union all
            select '4.PG루틴'    as 처리사항 from sysibm.sysdummy1 union all
            select '5.기타'      as 처리사항 from sysibm.sysdummy1
          ) b
     ) bb
group by  bb.요청형태, bb.처리사항";


        //Response.Write(sql);
        DataTable dt = GetDataTable(sql);
        if (dt.Rows.Count >= 0)
        {
            monStat.EmptyDataText = "조회하신 데이터가 없습니다.";
            monStat.DataSource = dt;
            monStat.DataBind();
        }
        dt.Clear();
        dt.Dispose();
    }


    protected void monStat_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Style["text-align"] = "right";
            e.Row.Cells[2].Style["text-align"] = "right";
            e.Row.Cells[4].Style["text-align"] = "right";
        }
    }

}
