<%@ Page Language="C#" AutoEventWireup="true" CodeFile="writeTask.aspx.cs" Inherits="teskedit" %>

<%@ Register Assembly="AjaxControlToolkit, Version=3.5.51116.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e"
    Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>단위업무 등록/수정</title>
    <script type="text/javascript" language='javascript' src="common.js"></script>
    <link href="./common.css?ver=1" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript">
        function setDate(obj) {
            var today = new Date();

            if (obj.value == "403") //처리완료이면 시작예정일과 종료예정일, 시작일, 종료일이 없는경우 오늘로 셋팅
            {
                if (document.getElementById("txtStartExpectDate").value == "")
                    document.getElementById("txtStartExpectDate").value = today.format("yyyy-MM-dd");
                if (document.getElementById("txtStartDate").value == "")
                    document.getElementById("txtStartDate").value = today.format("yyyy-MM-dd");
                if (document.getElementById("txtEndExpectDate").value == "")
                    document.getElementById("txtEndExpectDate").value = today.format("yyyy-MM-dd");
                if (document.getElementById("txtEndDate").value == "")
                    document.getElementById("txtEndDate").value = today.format("yyyy-MM-dd");
            }
        }

        function setComplete(obj) {
            if (obj.value == "403") //처리완료이면 빈값들 셋팅
            {
                // 시작예정일과 종료예정일, 시작일, 종료일이 없는경우 오늘로 셋팅
                var today = new Date();
                if (document.getElementById("txtStartExpectDate").value == "")
                    document.getElementById("txtStartExpectDate").value = today.format("yyyy-MM-dd");
                if (document.getElementById("txtStartDate").value == "")
                    document.getElementById("txtStartDate").value = today.format("yyyy-MM-dd");
                if (document.getElementById("txtEndExpectDate").value == "")
                    document.getElementById("txtEndExpectDate").value = today.format("yyyy-MM-dd");
                if (document.getElementById("txtEndDate").value == "")
                    document.getElementById("txtEndDate").value = today.format("yyyy-MM-dd");

                //처리자 빈값이면 현재 세션직원으로 셋팅
                if (document.getElementById("txtProcessBy").value == "") {
                    document.getElementById("txtProcessBy").value = document.getElementById("hdnLoginNm").value
                    document.getElementById("hdnProcessBy").value = document.getElementById("hdnLoginId").value
                }
                
                //진척률 빈값이면 100%로 셋팅
                if (document.getElementById("txtTaskProgress").value == "")
                    document.getElementById("txtTaskProgress").value = "100%";
            }
        }

        // 부서검색
        function callSearchDept() {
            if (event.keyCode == 13) {
                SearchDeptCheckOne('form1.hdnRequestDept.value', 'form1.txtRequestDept.value', form1.txtRequestDept.value);
            }
        }

        // 요청자 검색
        function callSearchReqemp() {
            if (event.keyCode == 13) {
                SearchDevempCheckOne('form1.hdnRequestBy.value', 'form1.txtRequestBy.value', form1.txtRequestBy.value);
            }
        }

        // 처리자 검색
        function callSearchDevemp() {
            if (event.keyCode == 13) {
                SearchDevempCheckOne('form1.hdnProcessBy.value', 'form1.txtProcessBy.value', form1.txtProcessBy.value);
            }
        }

        // 접수자 검색
        function callSearchRcptemp() {
            if (event.keyCode == 13) {
                SearchDevempCheckOne('form1.hdnReceiptBy.value', 'form1.txtReceiptBy.value', form1.txtReceiptBy.value);
            }
        }


        function openTask() {
            document.forms["writeform"].hdnTaskStep.value = document.forms["form1"].searchTaskStep.value;
            document.forms["writeform"].hdnDeveloper.value = document.forms["form1"].searchDeveloper.value;
            document.forms["writeform"].hdnReqTool.value = document.forms["form1"].searchReqTool.value;
            document.forms["writeform"].hdnGubun_1.value = document.forms["form1"].searchGubun_1Tool.value;
            document.forms["writeform"].hdnStartTime.value = document.forms["form1"].searchStartTime.value;
            document.forms["writeform"].hdnEndTime.value = document.forms["form1"].searchEndTime.value;
            document.forms["writeform"].hdnDocNo.value = document.forms["form1"].searchDocNo.value;
            document.forms["writeform"].hdnRequestDept.value = document.forms["form1"].searchRequestDept.value;
            document.forms["writeform"].hdnRequestDeptName.value = document.forms["form1"].searchRequestDeptName.value;
            document.forms["writeform"].hdnRequestBy.value = document.forms["form1"].searchRequestBy.value;
            document.forms["writeform"].hdnRequestByName.value = document.forms["form1"].searchRequestByName.value;
            document.forms["writeform"].hdnMyData.value = document.forms["form1"].searchMyData.value;
            document.forms["writeform"].hdnReqrmk.value = document.forms["form1"].searchReqrmk.value;
            document.forms["writeform"].hdnDevrmk.value = document.forms["form1"].searchDevrmk.value;
            document.forms["writeform"].hdnIngData.value = document.forms["form1"].searchIngData.value;
            document.forms["writeform"].hdnPaging.value = document.forms["form1"].searchPaging.value;
            document.forms["writeform"].taskID.value = document.forms["form1"].searchtaskID.value;
            document.forms["writeform"].MODE.value = document.forms["form1"].searchMODE.value;
            document.forms["writeform"].hdnTargetPage.value = document.forms["form1"].searchTargetPage.value;
            document.forms["writeform"].hdnReqrmkCopy.value = document.forms["form1"].searchReqrmkCopy.value;
            document.forms["writeform"].hdnIpt.value = document.forms["form1"].searchIpt.value;
            document.forms["writeform"].hdnnotice.value = document.forms["form1"].searchnotice.value;

            if (document.forms["writeform"].hdnTargetPage.value == "") {
                document.forms["writeform"].hdnTargetPage.value = "readTask.aspx";
            }

            document.forms["writeform"].action = document.forms["writeform"].hdnTargetPage.value;
            document.forms["writeform"].method = "post";
            document.forms["writeform"].submit();
            return false;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <br />
        <div class="bullet_title">단위업무 관리</div>

        <table class="action_table" id="TABLE1" width="900px">
            <tr>
                <asp:HiddenField ID="hdnTaskId" runat="server" />
                <asp:HiddenField ID="hdnTaskSeq" runat="server" />
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblGnlSystemName" runat="server" Text="시스템명"></asp:Label>
                </td>
                <td class="action_td_in" id="XXX" width="285px">
                    <asp:DropDownList ID="cboGnlSystemName" runat="server"></asp:DropDownList>
                    <asp:Label ID="viewGnlSystemName" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblGnlReqType" runat="server" Text="요청유형"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:DropDownList ID="cboGnlReqType" runat="server" Width="55px"></asp:DropDownList>
                    <asp:RadioButton ID="FirstReq" runat="server" GroupName="select" Width="70px" Text="최초요구"></asp:RadioButton>
                    <asp:RadioButton ID="ComReq" runat="server" GroupName="select" Width="70px" Text="전산요청"></asp:RadioButton>
                    <asp:RadioButton ID="AddReq" runat="server" GroupName="select" Width="70px" Text="추가요구"></asp:RadioButton>
                    <asp:Label ID="viewGnlReqType" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblGnlReqTool" runat="server" Text="요청방식"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:DropDownList ID="cboGnlReqTool" runat="server" Width="100px"></asp:DropDownList>
                    <asp:Label ID="viewGnlReqTool" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblDocNo" runat="server" Text="문서번호"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtDocNo" runat="server" Width="200px"></asp:TextBox><br />
                    <asp:Label ID="lblDocNoEx" runat="server" Text="ex) 2012-5610-0000"></asp:Label>
                    <asp:Label ID="viewDocNo" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestDate" runat="server" Text="요청일"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtRequestDate" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton ID="btnRequestDate" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle" runat="server" />
                    <asp:CalendarExtender ID="calRequestDate" runat="server" TargetControlID="txtRequestDate" PopupButtonID="btnRequestDate" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender>
                    <asp:Label ID="viewRequestDate" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestDept" runat="server" Text="요청부서"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtRequestDept" runat="server" onKeyPress="javascript:callSearchDept();"></asp:TextBox>
                    <asp:ImageButton ID="btnRequestDept" runat="server" ImageUrl="image/serach_.jpg" ImageAlign="AbsMiddle" OnClientClick="javascript:SearchDept('form1.hdnRequestDept.value','form1.txtRequestDept.value', form1.txtRequestDept.value);" />
                    <asp:HiddenField ID="hdnRequestDept" runat="server" />
                    <asp:Label ID="viewRequestDept" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestBy" runat="server" Text="요청자"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtRequestBy" runat="server" onKeyPress="javascript:callSearchReqemp();"></asp:TextBox>
                    <asp:ImageButton ID="btnRequestBy" runat="server" ImageUrl="image/serach_.jpg" ImageAlign="AbsMiddle" OnClientClick="javascript:SearchDevemp('form1.hdnRequestBy.value','form1.txtRequestBy.value');" />
                    <asp:HiddenField ID="hdnRequestBy" runat="server" />
                    <asp:Label ID="viewRequestBy" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblReceiptBy" runat="server" Text="접수자"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtReceiptBy" runat="server" onKeyPress="javascript:callSearchRcptemp();"></asp:TextBox>
                    <asp:ImageButton ID="btnReceiptBy" runat="server" ImageUrl="image/serach_.jpg" ImageAlign="AbsMiddle" OnClientClick="javascript:SearchDevemp('form1.hdnReceiptBy.value','form1.txtReceiptBy.value');" />
                    <asp:HiddenField ID="hdnReceiptBy" runat="server" />
                    <asp:Label ID="viewReceiptBy" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestBys" runat="server" Text="업무담당자"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtRequestBys" runat="server"></asp:TextBox>
                    <asp:Label ID="viewRequestBys" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestDueDate" runat="server" Text="완료요청일"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtRequestDueDate" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton ID="btnRequestDuedate" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle" runat="server" />
                    <asp:CalendarExtender ID="calRequestDueDate" runat="server" TargetControlID="txtRequestDueDate" PopupButtonID="btnRequestDuedate" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd">
                    </asp:CalendarExtender>
                    <asp:Label ID="viewRequestDueDate" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestContent" runat="server" Text="제목"></asp:Label>
                </td>
                <td class="action_td_in" width="735" colspan="3">
                    <asp:TextBox ID="txtRequestContent" runat="server" Width="500" MaxLength="200"></asp:TextBox>
                    <asp:Label ID="viewRequestContent" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:CheckBox ID="chknotice" runat="server" Text="공지사항" Width="70"></asp:CheckBox>
                    <asp:CheckBox ID="chkImPorTant" runat="server" Style="text-align: center" ForeColor="Red" Text="긴급(환자직접영향)" Width="150"></asp:CheckBox>

                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestContent2" runat="server" Text="적용 프로그램"></asp:Label>
                </td>
                <td class="action_td_in" width="735" colspan="3">
                    <asp:TextBox ID="txtRequestContent2" runat="server" Width="670" MaxLength="30"></asp:TextBox>
                    <asp:Label ID="viewRequestContent2" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:HiddenField ID="hdnIpt" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblRequestContent1" runat="server" Text="요청사항"></asp:Label>
                </td>
                <td class="action_td_in" width="735" colspan="3">
                    <asp:TextBox ID="txtRequestContent1" runat="server" TextMode="MultiLine" Width="720" Rows="10" MaxLength="1000"></asp:TextBox>
                    <asp:Label ID="viewRequestContent1" runat="server" Text="" Visible="false"></asp:Label>
                    <br />
                    <asp:FileUpload ID="uploadRequestContent" runat="server" Width="670" />
                    <br />
                    <asp:Label ID="lblRequestFileName" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hdnRequestFileRealName" runat="server" />
                    <asp:CheckBox ID="chkRequestFileName" runat="server" Text="삭제" />
                </td>
            </tr>

        </table>
        <br />
        <br />
        <table class="action_table">
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblTaskStep" runat="server" Text="진행단계"></asp:Label>
                </td>
                <td class="action_td_in" width="285" colspan="3">
                    <asp:DropDownList ID="cboTaskStep" runat="server" onChange="javascript:setComplete(this);">
                        <asp:ListItem Text="-" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="viewTaskStep" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblGubun" runat="server" Text="구분"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:DropDownList ID="cboGubun" runat="server">
                        <asp:ListItem Text="-" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="viewGubun" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblTaskStep1" runat="server" Text="개발 구분"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:DropDownList ID="cboGubun_1" runat="server" onChange="javascript:setComplete(this);">
                        <asp:ListItem Text="-" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="viewGubun_1" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblProcessBy" runat="server" Text="처리자"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtProcessBy" runat="server" onKeyPress="javascript:callSearchDevemp();"></asp:TextBox>
                    <asp:ImageButton ID="btnProcessBy"
                        runat="server" ImageUrl="image/serach_.jpg" ImageAlign="AbsMiddle" OnClientClick="javascript:SearchDevemp('form1.hdnProcessBy.value','form1.txtProcessBy.value');" />
                    <asp:Label ID="viewProcessBy"
                        runat="server" Text="" Visible="false"></asp:Label>
                    <asp:HiddenField ID="hdnProcessBy" runat="server" />
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblTaskProgress" runat="server" Text="진척률"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtTaskProgress" runat="server"></asp:TextBox>
                    <asp:Label ID="viewTaskProgress"
                        runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblStartExpectDate" runat="server" Text="시작예정일"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtStartExpectDate" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton ID="btnStartExpectDate" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle"
                        runat="server" /><asp:Label ID="viewStartExpectDate"
                            runat="server" Text="" Visible="false"></asp:Label>
                    <asp:CalendarExtender ID="calartExpectDate" runat="server" TargetControlID="txtStartExpectDate" PopupButtonID="btnStartExpectDate" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd">
                    </asp:CalendarExtender>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblEndExpectDate" runat="server" Text="종료예정일"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtEndExpectDate" runat="server" TabIndex="10"></asp:TextBox>
                    <asp:ImageButton ID="btnEndExpectDate" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle"
                        runat="server" /><asp:Label ID="viewEndExpectDate"
                            runat="server" Text="" Visible="false"></asp:Label>
                    <asp:CalendarExtender ID="calEndExpectDate" runat="server" TargetControlID="txtEndExpectDate" PopupButtonID="btnEndExpectDate" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblStartDate" runat="server" Text="시작일"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtStartDate" runat="server" TabIndex="10"></asp:TextBox>
                    <asp:ImageButton ID="btnStartDate" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle"
                        runat="server" /><asp:Label ID="viewStartDate"
                            runat="server" Text="" Visible="false"></asp:Label>
                    <asp:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate" PopupButtonID="btnStartDate" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd">
                    </asp:CalendarExtender>
                </td>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblEndDate" runat="server" Text="종료일"></asp:Label>
                </td>
                <td class="action_td_in" width="285">
                    <asp:TextBox ID="txtEndDate" runat="server" TabIndex="10"></asp:TextBox>
                    <asp:ImageButton ID="btnEndDate" ImageUrl="image/unnamed.png" ImageAlign="AbsMiddle"
                        runat="server" /><asp:Label ID="viewEndDate"
                            runat="server" Text="" Visible="false"></asp:Label>
                    <asp:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDate" PopupButtonID="btnEndDate" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblPrcoessContent" runat="server" Text="처리사항"></asp:Label>
                </td>
                <td colspan="3" class="action_td_in" width="735">
                    <asp:TextBox ID="txtPrcoessContent" runat="server" TextMode="MultiLine" Rows="10" Width="720"></asp:TextBox>
                    <asp:Label ID="viewPrcoessContent" runat="server" Text="" Visible="false"></asp:Label>
                    <br />
                    <asp:FileUpload ID="uploadPrcoessContent" runat="server" Width="670" />
                    <br />
                    <asp:Label ID="lblProcessFileName" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hdnResponseFileRealName" runat="server" />
                    <asp:CheckBox ID="chkProcessFileName" runat="server" Text="삭제" />
                </td>
            </tr>
            <tr>
                <td class="action_td_lbl" width="135">
                    <asp:Label ID="lblProcessBys" runat="server" Text="처리관련자"></asp:Label>
                </td>
                <td colspan="3" class="action_td_in" width="735px">
                    <asp:TextBox ID="txtProcessBys" runat="server" Width="720" MaxLength="30"></asp:TextBox>
                    <asp:Label ID="viewProcessBys" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>

        <br />
        <br />

        <table class="button_table" width="900px">
            <tr style="border: 1pt solid white;">
                <td style="text-align: center;">
                    <asp:Button ID="btnSave" runat="server" Text="등록" OnClick="btnSave_Click" CssClass="btn_search" Style="cursor: pointer !important;" />
                    <asp:Button ID="btnDelete" runat="server" Text="삭제" OnClick="btnDelete_Click" CssClass="btn_search" Style="cursor: pointer !important;" />
                    <!--<asp:Button ID="btnWbs" runat="server" Text="WBS"  onclick="btnWbs_Click" CssClass="btn_search" />-->
                    <asp:Button ID="btnList" runat="server" Text="목록" OnClick="btnList_Click" CssClass="btn_search" />

                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Style="cursor: pointer !important;" CssClass="btn_search" />
                    <asp:HiddenField ID="hdnLoginId" runat="server" />
                    <asp:HiddenField ID="hdnLoginNm" runat="server" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="searchTaskStep" runat="server" />
        <asp:HiddenField ID="searchDeveloper" runat="server" />
        <asp:HiddenField ID="searchReqTool" runat="server" />
        <asp:HiddenField ID="searchStartTime" runat="server" />
        <asp:HiddenField ID="searchEndTime" runat="server" />
        <asp:HiddenField ID="searchDocNo" runat="server" />
        <asp:HiddenField ID="searchRequestDept" runat="server" />
        <asp:HiddenField ID="searchRequestDeptName" runat="server" />
        <asp:HiddenField ID="searchRequestBy" runat="server" />
        <asp:HiddenField ID="searchRequestByName" runat="server" />
        <asp:HiddenField ID="searchMyData" runat="server" />
        <asp:HiddenField ID="searchReqrmk" runat="server" />
        <asp:HiddenField ID="searchDevrmk" runat="server" />
        <asp:HiddenField ID="searchIngData" runat="server" />
        <asp:HiddenField ID="searchPaging" runat="server" />
        <asp:HiddenField ID="searchtaskID" runat="server" />
        <asp:HiddenField ID="searchMODE" runat="server" />
        <asp:HiddenField ID="searchTargetPage" runat="server" />
    </form>
    <form id="writeform" name="writeform">
        <input type="hidden" name="hdnTaskStep" value="<%=Request["hdnTaskStep"] %>" />
        <input type="hidden" name="hdnDeveloper" value="<%=Request["hdnDeveloper"] %>" />
        <input type="hidden" name="hdnReqTool" value="<%=Request["hdnReqTool"] %>" />
        <input type="hidden" name="hdnStartTime" value="<%=Request["hdnStartTime"] %>" />
        <input type="hidden" name="hdnEndTime" value="<%=Request["hdnEndTime"] %>" />
        <input type="hidden" name="hdnDocNo" value="<%=Request["hdnDocNo"] %>" />
        <input type="hidden" name="hdnRequestDept" value="<%=Request["hdnRequestDept"] %>" />
        <input type="hidden" name="hdnRequestDeptName" value="<%=Request["hdnRequestDeptName"] %>" />
        <input type="hidden" name="hdnRequestBy" value="<%=Request["hdnRequestBy"] %>" />
        <input type="hidden" name="hdnRequestByName" value="<%=Request["hdnRequestByName"] %>" />
        <input type="hidden" name="hdnMyData" value="<%=Request["hdnMyData"] %>" />
        <input type="hidden" name="hdnReqrmk" value="<%=Request["hdnReqrmk"] %>" />
        <input type="hidden" name="hdnDevrmk" value="<%=Request["hdnDevrmk"] %>" />
        <input type="hidden" name="hdnIngData" value="<%=Request["hdnIngData"] %>" />
        <input type="hidden" name="hdnPaging" value="<%=Request["hdnPaging"] %>" />
        <input type="hidden" name="taskID" value="<%=Request["taskID"] %>" />
        <input type="hidden" name="MODE" value="<%=Request["MODE"] %>" />
        <input type="hidden" name="hdnTargetPage" value="<%=Request["hdnTargetPage"] %>" />
    </form>
</body>
</html>
