<%@ Page Language="C#" AutoEventWireup="true" CodeFile="readTask.aspx.cs" Inherits="tasklista" %>

<%@ Register Assembly="AjaxControlToolkit, Version=3.5.51116.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e"
    Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="common.css?ver=1" type="text/css" rel="stylesheet" />
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <script language='javascript' type="text/javascript">
        function callSearchDept() {
            if (event.keyCode == 13) {
                SearchDeptCheckOne('form1.hdnRequestDept.value', 'form1.txtRequestDept.value', form1.txtRequestDept.value);
                //form1.txtRequestDept.focus();
            }
        }
        function callSearchReqemp() {
            if (event.keyCode == 13) {
                SearchReqempCheckOne('form1.hdnRequestBy.value', 'form1.txtRequestBy.value', form1.txtRequestBy.value);
            }

        }
        function openTask(status, idx) {
            document.forms["writeform"].hdnTaskStep.value = document.forms["form1"].cboTaskStep.value;
            document.forms["writeform"].hdnDeveloper.value = document.forms["form1"].cboDeveloper.value;
            document.forms["writeform"].hdnGubun_1.value = document.forms["form1"].cboGubun_1.value;
            document.forms["writeform"].hdnStartTime.value = document.forms["form1"].txtStartTime.value;
            document.forms["writeform"].hdnEndTime.value = document.forms["form1"].txtEndTime.value;
            document.forms["writeform"].hdnDocNo.value = document.forms["form1"].txtDocNo.value;
            document.forms["writeform"].hdnRequestDept.value = document.forms["form1"].hdnRequestDept.value;
            document.forms["writeform"].hdnRequestDeptName.value = document.forms["form1"].txtRequestDept.value;
            document.forms["writeform"].hdnRequestBy.value = document.forms["form1"].hdnRequestBy.value;
            document.forms["writeform"].hdnRequestByName.value = document.forms["form1"].txtRequestBy.value;

            if (document.forms["form1"].chkMyData.checked == true)
                document.forms["writeform"].hdnMyData.value = "true";
            else
                document.forms["writeform"].hdnMyData.value = "false";

            document.forms["writeform"].hdnRcptEmp.value = document.forms["form1"].txtRcptEmp.value;
            document.forms["writeform"].hdnDevrmk.value = document.forms["form1"].txtDevrmk.value;

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
    <title>단위업무 조회(목록)</title>
</head>
<body>


    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <br />
        <div class="bullet_title">
            단위업무 목록
        </div>
        <table class="action_table">
            <tr>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lblTaskStep" runat="server" Text="진행단계"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:DropDownList ID="cboTaskStep" runat="server" Width="100px">
                        <asp:ListItem>전체</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lblDeveloper" runat="server" Text="처리자"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:DropDownList ID="cboDeveloper" runat="server" Width="100px">
                        <asp:ListItem>전체</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lbnReqTool" runat="server" Text="구 분"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:DropDownList ID="cboGubun_1" runat="server" Width="100px">
                        <asp:ListItem>전체</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblStartTime" runat="server" Text="기간(From)"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtStartTime" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="btnStartTime" runat="server" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle" />
                    <asp:CalendarExtender ID="calStartTime" runat="server" TargetControlID="txtStartTime" PopupButtonID="btnStartTime" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender>
                </td>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblEndTime" runat="server" Text="기간(To)"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtEndTime" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="btnEndTime" runat="server" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle" />
                    <asp:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndTime" PopupButtonID="btnEndTime" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender>
                </td>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblDocNo" runat="server" Text="문서번호"></asp:Label></td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtDocNo" runat="server" Width="100px"></asp:TextBox>
                    ex) 0123 </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblRuquestDept" runat="server" Text="요청부서"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtRequestDept" runat="server" Width="100px" onKeyPress="javascript:callSearchDept();"></asp:TextBox><!--  -->
                    <asp:ImageButton ID="btnRequestDept" runat="server" ImageUrl="image/serach_.jpg" ImageAlign="AbsMiddle" OnClientClick="javascript:SearchDept('form1.hdnRequestDept.value','form1.txtRequestDept.value', form1.txtRequestDept.value);" />
                    <!--   -->
                    <asp:HiddenField ID="hdnRequestDept" runat="server" />
                </td>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblRcptEmp" runat="server" Text="접수자"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtRcptEmp" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lbnMyData" runat="server" Text="내 것만 보기"></asp:Label></td>
                <td class="action_td_in" width="200px">
                    <asp:CheckBox ID="chkMyData" runat="server" Text="" /></td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblRequestBy" runat="server" Text="요청자"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtRequestBy" runat="server" Width="100px" onKeyPress="javascript:callSearchReqemp();"></asp:TextBox>
                    <asp:ImageButton ID="btnRequestBy" runat="server" ImageUrl="image/serach_.jpg" ImageAlign="AbsMiddle" OnClientClick="javascript:SearchReqemp('form1.hdnRequestBy.value','form1.txtRequestBy.value', form1.txtRequestBy.value);" />
                    <asp:HiddenField ID="hdnRequestBy" runat="server" />
                </td>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblDevrmk" runat="server" Text="처리내용"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtDevrmk" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="action_td_lbl" width="100px">
                    <asp:Label ID="lblIngData" runat="server" Text="미처리내역"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:CheckBox ID="chkIngData" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lblTaskStep1" runat="server" Text="조  건"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:RadioButton ID="AllReq" runat="server" GroupName="select" Width="140px" Text="전&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;체"></asp:RadioButton>
                    <asp:RadioButton ID="FirstReq" runat="server" GroupName="select" Width="140px" Text="최초요구"></asp:RadioButton>
                    <asp:RadioButton ID="ComReq" runat="server" GroupName="select" Width="140px" Text="전산요청"></asp:RadioButton>
                    <asp:RadioButton ID="AddReq" runat="server" GroupName="select" Width="140px" Text="추가요구"></asp:RadioButton>
                </td>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lbnReqTool2" runat="server" Text="적용 프로그램"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:TextBox ID="txtProgramcopy" runat="server" Text="" Width="170px"></asp:TextBox>
                </td>

                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lbnReqTool1" runat="server" Style="text-align: center" ForeColor="Red" Text="긴&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp급<br/>(환자에게<br/>직접영향)"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:CheckBox ID="chkImPorTant" runat="server" Text="" Width="50"></asp:CheckBox>
                    <asp:HiddenField ID="hdnImt" runat="server" />
                </td>
            </tr>
            <tr>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lblSYSNM" runat="server" Text="시스템명"></asp:Label>
                </td>
                <td width="200px" class="action_td_in">
                    <asp:DropDownList ID="cboGnlSystemName" runat="server" Width="100px">
                        <asp:ListItem>전체</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lblSearchReqRmk" runat="server" Text="제목"></asp:Label>
                </td>
                <td class="action_td_in">
                    <asp:TextBox ID="txtSearchReqRmk" runat="server" Width="170px"></asp:TextBox>
                </td>
                <td width="100px" class="action_td_lbl">
                    <asp:Label ID="lblReqContent" runat="server" Text="요청내용"></asp:Label>
                </td>
                <td class="action_td_in">
                    <asp:TextBox ID="txtSearchReqContent" runat="server" Width="170px"></asp:TextBox>
                </td>
            </tr>

        </table>
        <table class="button_table" width="900px">
            <tr>
                <td colspan="6" style="text-align: center;">
                    <asp:Button ID="btnSearch" runat="server" Text="검색" OnClick="btnSearch_Click" CssClass="btn_search" /></td>
                <td colspan="1" style="width: 100px; text-align: center;">
                    <asp:DropDownList ID="cboPageSize" runat="server"
                        OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged" Visible="False">
                        <asp:ListItem>PageSize</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />

        총 건수 : <%=kkk%>
        <div name="GridviewDiv" id="GridviewDiv" class="result_table" style="overflow-y: scroll; width: 1580px; height: 500px">
            <asp:GridView
                ID="listTask"
                runat="server"
                ShowHeader="true"
                CssClass="listTable"
                AllowPaging="True"
                I
                OnPageIndexChanged="listTask_PageIndexChanged"
                OnRowCreated="listTask_RowCreated"
                OnRowDataBound="listTask_RowDataBound"
                Style="width: 1550px !important"
                OnPageIndexChanging="listTask_PageIndexChanging"
                CellPadding="3" PageSize="30">
            </asp:GridView>
            <asp:HiddenField ID="pagingNo" runat="server" Value="0" />
        </div>
    </form>
    <form id="writeform" name="writeform">
        <input type="hidden" name="hdnTaskStep" value="" />
        <input type="hidden" name="hdnDeveloper" value="" />
        <input type="hidden" name="hdnGubun_1" value="" />
        <input type="hidden" name="hdnStartTime" value="" />
        <input type="hidden" name="hdnEndTime" value="" />
        <input type="hidden" name="hdnDocNo" value="" />
        <input type="hidden" name="hdnRequestDept" value="" />
        <input type="hidden" name="hdnRequestDeptName" value="" />
        <input type="hidden" name="hdnRequestBy" value="" />
        <input type="hidden" name="hdnRequestByName" value="" />
        <input type="hidden" name="hdnMyData" value="" />
        <input type="hidden" name="hdnRcptEmp" value="" />
        <input type="hidden" name="hdnDevrmk" value="" />
        <input type="hidden" name="hdnIngData" value="" />
        <input type="hidden" name="hdnPaging" value="" />
        <input type="hidden" name="taskID" value="" />
        <input type="hidden" name="MODE" value="" />
        <input type="hidden" name="hdnTargetPage" value="readTask.aspx" />
    </form>
</body>
</html>
