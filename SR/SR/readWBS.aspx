<%@ Page Language="C#" AutoEventWireup="true" CodeFile="readWBS.aspx.cs" Inherits="tesklistb" %>

<%@ Register Assembly="AjaxControlToolkit, Version=3.5.51116.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e"
    Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>단위업무 조회(스캐줄)</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <script language='javascript' type="text/javascript">
        function callSearchDept()
        {
            if(event.keyCode == 13) {
                SearchDeptCheckOne('form1.hdnRequestDept.value','form1.txtRequestDept.value', form1.txtRequestDept.value);
                //form1.txtRequestDept.focus();
            }
        }
        function callSearchReqemp()
        {
            if(event.keyCode == 13) {
                SearchReqempCheckOne('form1.hdnRequestBy.value','form1.txtRequestBy.value', form1.txtRequestBy.value);
            }

        }
        function openTask(status, idx) {
            document.forms["writeform"].hdnTaskStep.value = document.forms["form1"].cboTaskStep.value;
            document.forms["writeform"].hdnDeveloper.value = document.forms["form1"].cboDeveloper.value;
            document.forms["writeform"].hdnStartTime.value = document.forms["form1"].txtStartTime.value;
            document.forms["writeform"].hdnEndTime.value = document.forms["form1"].txtEndTime.value;
            document.forms["writeform"].hdnRequestDept.value = document.forms["form1"].hdnRequestDept.value;
            document.forms["writeform"].hdnRequestDeptName.value = document.forms["form1"].txtRequestDept.value;
            document.forms["writeform"].hdnRequestBy.value = document.forms["form1"].hdnRequestBy.value;
            document.forms["writeform"].hdnRequestByName.value = document.forms["form1"].txtRequestBy.value;
            if (document.forms["form1"].chkMyData.checked == true)
                document.forms["writeform"].hdnMyData.value = "true";
            else
                document.forms["writeform"].hdnMyData.value = "false";
            if (document.forms["form1"].chkIngData.checked == true)
                document.forms["writeform"].hdnIngData.value = "true";
            else
                document.forms["writeform"].hdnIngData.value = "false";
            document.forms["writeform"].hdnPaging.value = document.forms["form1"].pagingNo.value;
            document.forms["writeform"].taskID.value = idx;
            document.forms["writeform"].MODE.value = status;

            document.forms["writeform"].action = "writeTask.aspx";
            document.forms["writeform"].method = "post";
            document.forms["writeform"].submit();

        }
        
    </script>
    <link href="common.css" type="text/css" rel="stylesheet"/>  
    <style type="text/css">
        .style1
        {
            color: BLUE;
        }
        .style2
        {
            color: ORANGE;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
        <div  class="bullet_title" >
        단위업무 조회(스캐줄)
        </div>
        <table  class="action_table" >
            <tr>
                <td  class="action_td_lbl" >
                    <asp:Label ID="lblTaskStep" runat="server" Text="진행단계"></asp:Label>
                </td>
                <td  class="action_td_in">
                    <asp:DropDownList ID="cboTaskStep" runat="server" Width="100px">
                        <asp:ListItem>전체</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="120px" class="action_td_lbl">
                    <asp:Label ID="lblDeveloper" runat="server" Text="처리자"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:DropDownList ID="cboDeveloper" runat="server" Width="100px">
                        <asp:ListItem>전체</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="action_td_lbl" width="120px" >
                    <asp:Label ID="lblGnlReqTool" runat="server" Text="요청방식"></asp:Label>
                </td><td class="action_td_in" width="200px" > 
                    <asp:DropDownList ID="cboGnlReqTool" runat="server" Width="100px" Enabled="false">
                        <asp:ListItem>문서</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td  class="action_td_lbl">
                    <asp:Label ID="lblFrom" runat="server" Text="기간(From)"></asp:Label>
                </td>
                <td class="action_td_in">
                    <asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnStartTime" runat="server" ImageUrl="image/calendar.gif" ImageAlign="AbsMiddle" />
                    <asp:CalendarExtender ID="calStartTime" runat="server" TargetControlID="txtStartTime" PopupButtonID="btnStartTime" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender> 
                </td>
                <td class="action_td_lbl">
                    <asp:Label ID="lblTo" runat="server" Text="기간(To)"></asp:Label>
                </td>
                <td class="action_td_in"  >
                    <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnEndTime" runat="server" ImageUrl="image/calendar.gif" ImageAlign="AbsMiddle"/>
                    <asp:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndTime" PopupButtonID="btnEndTime" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender>
                </td>
                <td class="action_td_lbl"><asp:Label ID="lbnMyData" runat="server" Text="내 것만 보기"></asp:Label></td>
                <td class="action_td_in"><asp:CheckBox ID="chkMyData" runat="server" Text="" /></td>
            </tr>
            <tr>
                <td width="120px" class="action_td_lbl">
                    <asp:Label ID="lblRuquestDept" runat="server" Text="요청부서"></asp:Label>
                </td>
                <td width="200px"  class="action_td_in">
                    <asp:TextBox ID="txtRequestDept" runat="server" onKeyPress="javascript:callSearchDept();"></asp:TextBox><!--  -->
                    <asp:ImageButton ID="btnRequestDept" runat="server" ImageUrl="image/search.gif" ImageAlign="AbsMiddle" onClientClick="javascript:SearchDept('form1.hdnRequestDept.value','form1.txtRequestDept.value', form1.txtRequestDept.value);"/> <!--   -->
                    <asp:HiddenField ID="hdnRequestDept" runat="server" />
                </td>
                <td  class="action_td_lbl">
                    <asp:Label ID="lblRequestBy" runat="server" Text="요청자"></asp:Label>
                </td>
                <td  class="action_td_in">
                    <asp:TextBox ID="txtRequestBy" runat="server" onKeyPress="javascript:callSearchReqemp();"></asp:TextBox>
                    <asp:ImageButton ID="btnRequestBy" runat="server" ImageUrl="image/search.gif" ImageAlign="AbsMiddle" onClientClick="javascript:SearchReqemp('form1.hdnRequestBy.value','form1.txtRequestBy.value', form1.txtRequestBy.value);"/>
                    <asp:HiddenField ID="hdnRequestBy" runat="server" />
                </td>
                <td class="action_td_lbl"> <asp:Label ID="lblIngData" runat="server" Text="미처리내역"></asp:Label></td>
                <td class="action_td_in"><asp:CheckBox ID="chkIngData" runat="server" Text="" /></td>
            </tr>
        </table>
        <table class="button_table" width="1063px">
            <tr>
                <td   style="text-align:center;" ><asp:Button ID="btnSearch" runat="server" 
                        Text="검색" CssClass="btn_search" onclick="btnSearch_Click"  /></td>
            </tr>
         </table>
        <!--
        <div name="HeaderDiv" id="HeaderDiv" class="result_table" style="overflow-y:scroll;width:1061px;height:25px" >
        <asp:GridView ID="listCalendarHeader"
            runat="server" 
            onrowcreated="listCalendarHeader_RowCreated" ShowHeader="true" 
            CssClass="listTable"
            style="width:1044px !important"  >
        </asp:GridView>
        </div>
        -->
        <div name="GridviewDiv" id="GridviewDiv" class="result_table" style="overflow-y:scroll;width:1061px;height:520px" >
        <asp:GridView ID="listCalendar" runat="server" 
            onrowcreated="listCalendar_RowCreated" ShowHeader="true" 
            CssClass="listTable" AllowPaging="True" 
            ondatabinding="listCalendar_DataBinding" 
            onrowdatabound="listCalendar_RowDataBound" 
            style="width:1044px !important" 
            onpageindexchanging="listCalendar_PageIndexChanging" PageSize="100">
        </asp:GridView>
            <asp:HiddenField ID="pagingNo" runat="server" Value="0" />
        </div>
        <br />* <span class="style1">■■■</span> : 계획을 의미합니다. "시작예정일 ~ 종료예정일"에 의해서 표시됩니다.
        <br />* <span class="style2">■■■</span> : 실행을 의미합니다. "시작일 ~ 종료일"에 의해서 표시됩니다.
        <br />* 현재일에 <span class="style1">■</span>만 나타나면 시작지연, 현재일에 <span class="style2">■</span>만 나타나면 종료지연
    </form>
    <form id="writeform" name="writeform">
        <input type="hidden" name="hdnTaskStep" value="" />
        <input type="hidden" name="hdnDeveloper" value="" />
        <input type="hidden" name="hdnStartTime" value="" />
        <input type="hidden" name="hdnEndTime" value="" />
        <input type="hidden" name="hdnRequestDept" value="" />
        <input type="hidden" name="hdnRequestDeptName" value="" />
        <input type="hidden" name="hdnRequestBy" value="" />
        <input type="hidden" name="hdnRequestByName" value="" />
        <input type="hidden" name="hdnMyData" value="" />
        <input type="hidden" name="hdnIngData" value="" />
        <input type="hidden" name="hdnPaging" value="" />
        <input type="hidden" name="taskID" value="" />
        <input type="hidden" name="MODE" value="" />
        <input type="hidden" name="hdnTargetPage" value="readWBS.aspx" />
    </form>    
</body>
</html>
