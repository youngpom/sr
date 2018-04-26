<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchReqeuestBy.aspx.cs" Inherits="searchReqeuestBy" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>요청자조회</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <link href="common.css" type="text/css" rel="stylesheet"/>  
    <script language='javascript' type="text/javascript">
        window.resizeTo(700, 550);
        //window.moveTo(300, 350);
        this.focus();
        function btnReqempseqEmpty_Click()
        {
        }
        
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
            if (f.txtReqempSeq.value.length > 0) return confirm("요청자SEQ가 존재합니다.\n현재 데이터로 '복제 신규등록' 하시겠습니까?");
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
</head>
<body>
    <form id="form1" runat="server">
    <div  class="bullet_title" >
    요청자 조회
    </div>
    <table width="610" style="margin-right: 0px">
        <tr>
            <td colspan="4" align="center">
                <asp:DropDownList ID="cboSearch" runat="server">
                    <asp:ListItem Value="ReqempNm">요청자명</asp:ListItem>
                    <asp:ListItem Value="Reqempseq">요청자코드</asp:ListItem>
                    <asp:ListItem Value="DeptseqNm">부서명</asp:ListItem>
                </asp:DropDownList>
&nbsp;
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
&nbsp;&nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="검색" onclick="btnSearch_Click" />
            &nbsp;&nbsp;
                <asp:Button ID="btnReqempseqEmpty" runat="server"  
                    onClientclick="btnReqempseqEmpty_Click" onclick="btnReqempseqEmpty_Click" 
                    Text="요청자 미지정" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" height="20">
                <asp:GridView ID="listReqempseq" runat="server" 
                    onrowcreated="listReqempseq_RowCreated" CssClass="listTable" 
                    AllowPaging="True" onpageindexchanging="listReqempseq_PageIndexChanging" 
                    onrowdatabound="listReqempseq_RowDataBound">
                </asp:GridView>
                <table>
                    <tr>
                        <td><asp:Button ID="btnSave" runat="server" Text="저장" onclick="btnSave_Click" /></td>
                        <td width="170">
                            <asp:HiddenField ID="txtDeptseq" runat="server" />
                            <asp:TextBox ID="txtDeptseqNm" runat="server" CssClass="text" 
                    
                    onClick="javascript:SearchDept('form1.txtDeptseq.value','form1.txtDeptseqNm.value');" 
                    ReadOnly="false" style="width:170px !important;"></asp:TextBox></td>
                        <td width="45">
                            <asp:TextBox ID="txtReqempNm" runat="server" CssClass="text" style="width:40px !important;"></asp:TextBox>
                        </td>
                        <td width="70">
                            <asp:DropDownList ID="cboJwcdNm" runat="server" CssClass="text"  onChange="javascript:ChangeJwcd();"  style="width:65px !important;">
                            </asp:DropDownList>
                        </td>
                        <td width="95"><asp:TextBox ID="txtOftel" runat="server" CssClass="text" style="width:90px !important;"></asp:TextBox></td>
                        <td width="88"><asp:TextBox ID="txtMbtel" runat="server" CssClass="text" style="width:83px !important;"></asp:TextBox></td>
                        <td width="88"><asp:TextBox ID="txtEmail" runat="server" CssClass="text" style="width:83px !important;"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:HiddenField ID="hdnReqempseqQuery" runat="server" />
                <asp:HiddenField ID="hdnReqempNmQuery" runat="server" />
                <asp:Label ID="Label1" runat="server" Text="* 해당요청자를 더블클릭하세요"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
