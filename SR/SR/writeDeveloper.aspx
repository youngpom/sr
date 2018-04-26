<%@ Page Language="C#" AutoEventWireup="true" CodeFile="writeDeveloper.aspx.cs" Inherits="writeDeveloper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>회원정보 관리</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <link href="common.css" type="text/css" rel="stylesheet"/>    
</head>
<body>
<br/>
    <form id="form1" runat="server">
    <div  class="bullet_title" >
        멤버정보 관리
    </div>
    <table   class="action_table" >
        <tr>
            <td class="action_td_lbl"  width="120px">
                <asp:Label ID="lblUserID" runat="server" Text="멤버 ID"></asp:Label>
            </td>
            <td class="action_td_in" width="200px">
                <asp:TextBox ID="txtUserID" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" width="120px">
                <asp:Label ID="lblUserName" runat="server" Text="성명"></asp:Label>
            </td>
            <td  class="action_td_in" width="200px">
                <asp:TextBox ID="txtUserName" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblUserPassword" runat="server" Text="멤버 암호"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtUserPassword" runat="server" TextMode="Password" 
                    CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblUserPosition" runat="server" Text="직위"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:DropDownList ID="cboUserPosition" runat="server" CssClass="text" 
                    Width="120px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblUserPasswordConfirm" runat="server" Text="암호확인"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtUserPasswordConfirm" runat="server" TextMode="Password" 
                    CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblUserEmail" runat="server" Text="이메일"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtUserEmail" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblUserTel" runat="server" Text="일반전화"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtUserTel" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblUserMobile" runat="server" Text="휴대전화"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtUserMobile" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblIsAdmin" runat="server" Text="관리자여부"></asp:Label>
            </td>
            <td colspan="3" class="action_td_in">
                <asp:CheckBox ID="chkIsAdmin" runat="server" />
            </td>
        </tr>
       </table>
        <table class="button_table" width="709px">
            <tr>
                <td style="text-align:center;" >
                <asp:Button ID="btnSave" runat="server" Text="등록"  CssClass="btn_search" onclick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="삭제"  CssClass="btn_search" onclick="btnDelete_Click" />
                <asp:Button ID="btnReset" runat="server" Text="초기화"  CssClass="btn_search" onclick="btnReset_Click" />
                <asp:HiddenField ID="hdnUserID" runat="server" />
                </td>
            </tr>
            <tr>
            <td height="20px"></td></tr>
         </table>
    <asp:Panel ID="listPanel" runat="server">
       <asp:GridView 
                    ID="listDeveloper" 
                    runat="server" 
                    onrowcreated="listDeveloper_RowCreated" 
                    CssClass="listTable" 
                    AllowPaging="True" 
                    onpageindexchanging="listDeveloper_PageIndexChanging" 
                    onselectedindexchanging="listDeveloper_SelectedIndexChanging"
                    style="width:709px !important" >
       </asp:GridView>
        <table class="button_table" width="709px">
            <tr>
                <td align="center">
                <asp:DropDownList ID="cboSearch" runat="server">
                    <asp:ListItem Value="ReqempNm">개발자명</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="검색"  CssClass="btn_search" onclick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
