<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchDept.aspx.cs" Inherits="searchDept" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>부서조회</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <link href="common.css" type="text/css" rel="stylesheet"/>  
    <script language='javascript' type="text/javascript">
        window.resizeTo(700, 550);
        //window.moveTo(300, 350);
        this.focus();
        function btnDeptEmpty_Click()
        {
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div  class="bullet_title" >
    부서조회
    </div>
    <table width="610" style="margin-right: 0px">
        <tr>
            <td colspan="4" align="center">
                <asp:DropDownList ID="cboSearch" runat="server">
                    <asp:ListItem Value="DeptseqNm">부서명</asp:ListItem>
                    <asp:ListItem Value="Deptseq">부서코드</asp:ListItem>
                    <asp:ListItem Value="Deptpart">파트</asp:ListItem>
                </asp:DropDownList>
&nbsp;
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
&nbsp;&nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="검색" onclick="btnSearch_Click" />
            &nbsp;&nbsp;
                <asp:Button ID="btnDeptEmpty" runat="server"  
                    onClientclick="btnDeptEmpty_Click" onclick="btnDeptEmpty_Click" 
                    Text="부서 미지정" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" height="20">
                <asp:GridView ID="listDept" runat="server" 
                    onrowcreated="listDept_RowCreated" CssClass="listTable" 
                    AllowPaging="True" onpageindexchanging="listDept_PageIndexChanging" 
                    onrowdatabound="listDept_RowDataBound">
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:HiddenField ID="hdnDeptseqQuery" runat="server" />
                <asp:HiddenField ID="hdnDeptseqNmQuery" runat="server" />
                <asp:Label ID="Label1" runat="server" Text="* 해당부서를 더블클릭하세요"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
