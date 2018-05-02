<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchReceiptBy.aspx.cs" Inherits="searchReceiptBy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>사용자 조회</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <link href="common.css" type="text/css" rel="stylesheet"/>  
    <script language='javascript' type="text/javascript">
        window.resizeTo(700, 550);
        //window.moveTo(300, 350);
        this.focus();
        function btnDevempIdEmpty_Click()
        {
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div  class="bullet_title" >
    사용자조회
    </div>
    <table width="610" style="margin-right: 0px">
        <tr>
            <td colspan="4" align="center">
                <asp:DropDownList ID="cboSearch" runat="server">
                    <asp:ListItem Value="DevempNm">사용자명</asp:ListItem>
                </asp:DropDownList>
&nbsp;
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
&nbsp;&nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="검색" onclick="btnSearch_Click" />
            &nbsp;&nbsp;
                <asp:Button ID="btnDevempIdEmpty" runat="server"  
                    onClientclick="btnDevempIdEmpty_Click" onclick="btnDevempIdEmpty_Click" 
                    Text="사용자 미지정" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" height="20">
                <asp:GridView ID="listDevemp" runat="server" 
                    onrowcreated="listDevemp_RowCreated" CssClass="listTable" 
                    AllowPaging="True" onpageindexchanging="listDevemp_PageIndexChanging" 
                    onrowdatabound="listDevemp_RowDataBound">
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:HiddenField ID="hdnDevempIdQuery" runat="server" />
                <asp:HiddenField ID="hdnDevempNmQuery" runat="server" />
                <asp:Label ID="Label1" runat="server" Text="* 해당사용자를 더블클릭하세요"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
