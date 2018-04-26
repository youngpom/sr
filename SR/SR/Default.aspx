<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>전산부 업무 관리</title>
    <link href="./common.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #form1
        {
            width: 546px;
        }
        .style1
        {
            width: 150px;
        }
    </style>

</head>
<body align="center">
    <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <br />
        <br />
        <table align="center" style="margin-right: 75px"><tr><td colspan="2" align="center">
				<img alt="로고" src="http://www.hallahosp.co.kr/han/kr/img/main2011/logo.gif" 
                    width="150" height="39" id="logo" align="middle" /> 
                <br />
                <br />
                의료정보실&nbsp;&nbsp; A-Pro<br />
                <br />
                    </td></tr>
        <tr><td width="100px" align="right"><asp:Label ID="Label1" runat="server" Text="아이디 : "></asp:Label></td>
            <td class="style1" ><asp:TextBox ID="TB_user" runat="server" Width="112px"></asp:TextBox></td></tr>
            <tr><td width="100px" align="right"><asp:Label ID="Label2" runat="server" Text="암  호 : "></asp:Label></td>
                   <td class="style1" >
                       <asp:TextBox ID="TB_password" runat="server" Width="112px" 
                       TextMode="Password"></asp:TextBox></td></tr>
            <tr><td colspan=2 align="center">
               <asp:Button ID="B_login" runat="server" Text="로그인" onclick="B_login_Click" />
               </td></tr>
            <tr><td colspan=2 align="center">
               <asp:Label ID="L_login" runat="server"></asp:Label>
               </td></tr>
        </table>
   </form>
</body>
</html>
