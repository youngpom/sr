<%@ Page Language="C#" AutoEventWireup="true" CodeFile="writeReqeuestBy.aspx.cs" Inherits="writeReqeuestBy" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>요청자 관리</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <script language='javascript' type="text/javascript">
        function btnEdit_Click() {
            f = document.form1;
            if (f.txtReqempSeq.value.length > 0) return confirm("정말 '수정' 하시겠습니까?");
            else { alert("요청자SEQ를 확인하세요!"); return false; }
        }
        function btnDelete_Click() {
            f = document.form1;
            if (f.txtReqempSeq.value.length > 0) return confirm("정말 '삭제' 하시겠습니까?");
            else { alert("요청자SEQ를 확인하세요!"); return false; }
        }
        function btnSave_Click() {
            f = document.form1;
            if (f.txtReqempSeq.value.length > 0) { alert("요청자SEQ가 이미 존재합니다."); return false; }
            return confirm("정말 '등록' 하시겠습니까?");
        }
        function btnReset_Click() {
        }
        function ChangeJwcd() {
            f = document.form1;
        }
        function ShowRow(ReqempSeq, DeptseqNm, ReqempNm, JwcdNm, Oftel, Mbtel, Email, Jwcd, Deptseq) {
            f = document.form1;

            f.txtReqempSeq.value = ReqempSeq;
            f.txtDeptseqNm.value = DeptseqNm;
            f.txtReqempNm.value = ReqempNm;
            var i;
            for (i = 0; i < f.cboJwcdNm.length; i++) 
            {
                if (f.cboJwcdNm.options[i].value == Jwcd) {
                    document.form1.cboJwcdNm.options[i].selected = true;
                }
            }
            f.txtOftel.value = Oftel;
            f.txtMbtel.value = Mbtel;
            f.txtEmail.value = Email;
            f.txtDeptseq.value = Deptseq;
        }
     
    </script>
    <link href="common.css" type="text/css" rel="stylesheet"/>    
</head>
<body>
<br/>
    <form id="form1" runat="server">
    <div  class="bullet_title" >
    요청자 관리
    </div>
    <table class="action_table" >
        <tr>
            <td class="action_td_lbl" width="120px" >
                <asp:Label ID="lblReqempSeq" runat="server" Text="요청자SEQ" ></asp:Label>
            </td>
            <td class="action_td_in"  width="200px" >
                <asp:TextBox ID="txtReqempSeq" runat="server" CssClass="text" readonly="true" 
                    BackColor="#EAEAEA" Width="120px"></asp:TextBox>
            </td>
            <td  class="action_td_lbl_base" width="120px"></td>
            <td  class="action_td_in"  width="200px" ></td>
        </tr>
        <tr>
            <td  class="action_td_lbl" >
                <asp:Label ID="lblReqempNm" runat="server" Text="요청자명"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtReqempNm" runat="server" CssClass="text" Width="120px" ></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblJwcd" runat="server" Text="직위"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:DropDownList ID="cboJwcdNm" runat="server" CssClass="text"  onChange="javascript:ChangeJwcd();" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblDeptseq" runat="server" Text="부서코드" ></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtDeptseq" runat="server" CssClass="text" 
                    
                    onClick="javascript:SearchDept('form1.txtDeptseq.value','form1.txtDeptseqNm.value');" 
                    ReadOnly="false" Width="120px" ></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblDeptseqNm" runat="server" Text="부서명" ></asp:Label>
            </td>
            <td  class="action_td_in" >
                <asp:TextBox ID="txtDeptseqNm" runat="server" CssClass="text" 
                    
                    onClick="javascript:SearchDept('form1.txtDeptseq.value','form1.txtDeptseqNm.value');" 
                    ReadOnly="false" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblOftel" runat="server" Text="직통전화"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtOftel" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblMbtel" runat="server" Text="휴대전화"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtMbtel" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblEmail" runat="server" Text="이메일"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtEmail" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl_base" >
            </td>
            <td class="action_td_in" >
            </td>
        </tr>
       </table>
        <table class="button_table" width="709px">
            <tr>
                <td style="text-align:center;" >
                <asp:Button ID="btnEdit" runat="server" Text="수정"  CssClass="btn_search" onClientClick="javascript:return btnEdit_Click();" onclick="btnEdit_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="삭제"  CssClass="btn_search" onClientClick="javascript:return btnDelete_Click();" onclick="btnDelete_Click" />
                <asp:Button ID="btnSave" runat="server" Text="등록"  CssClass="btn_search" onClientClick="javascript:return btnSave_Click();" onclick="btnSave_Click" />
                <asp:Button ID="btnReset" runat="server" Text="초기화"  CssClass="btn_search" onClientClick="javascript:return btnReset_Click();" onclick="btnReset_Click" />
                </td>
            </tr>
            <tr>
            <td height="20px"></td></tr>
         </table>
         <asp:GridView 
                    ID="listReqemp" 
                    runat="server" 
                    onrowcreated="listReqemp_RowCreated" 
                    CssClass="listTable" 
                    AllowPaging="True" 
                    onpageindexchanging="listReqemp_PageIndexChanging" 
                    onrowdatabound="listReqemp_RowDataBound"
                    style="width:709px !important">
         </asp:GridView>

        <table class="button_table" width="709px">
            <tr>
                <td align="center">
                <asp:DropDownList ID="cboSearch" runat="server">
                    <asp:ListItem Value="ReqempNm">요청자명</asp:ListItem>
                    <asp:ListItem Value="DeptseqNM">요청부서명</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="검색" CssClass="btn_search"  onclick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
