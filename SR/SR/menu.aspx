<%@ Page Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="./common.css?ver=1" rel="stylesheet" type="text/css" />
</head>
<body bgcolor=#D8D8D8 ><!--   bgcolor="Black" -->
    <form id="form1" runat="server">
    <table class="menuTable">
        <tr><td width="300px" >
        <br />
        <asp:Label ID="lblIP" runat="server" Font-Bold="True"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblCookieUserID" runat="server" Font-Bold="true"></asp:Label>
        <br /><br/>
        </td></tr>

         <tr><td>
        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/notice.aspx" 
            Target="main">■  공지사항</asp:HyperLink>
          <br /><br/><br/>
        </td></tr> <tr><td >
        <br />
        </td></tr>

        <tr><td>
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/writeTask.aspx" 
            Target="main">■  단위업무 관리</asp:HyperLink>
          <br /><br/><br/>
        </td></tr> <tr><td >
        <br />
        </td></tr>

        <tr><td >
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/readTask.aspx" 
            Target="main">■  단위업무 목록</asp:HyperLink>
        <br /><br/><br/>
        </td></tr> <tr><td >
        <br />
        </td></tr>
        <tr><td >
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/writeDeveloper.aspx" 
            Target="main">■  멤버정보 관리</asp:HyperLink>
        <br /><br/><br/>
        </td></tr> <tr><td >
        <br />
        </td></tr>
        <tr><td >
     <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/writeReqeuestBy.aspx" 
            Target="main">■  요청자 관리</asp:HyperLink>
        <br /><br/><br/>
        </td></tr> <tr><td >
        <br />
        </td></tr>
        <tr><td >
       <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/writeCommon.aspx" 
            Target="main">■  공통코드 관리</asp:HyperLink>
        <br />
        </td></tr> <tr><td >
        <br /><br/>
        </td></tr>
        <tr><td >
      <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/readWBS.aspx" 
            Target="main">■  단위업무 스케줄</asp:HyperLink>
 
        <br /><br/>
        </td></tr> <tr><td >
        <br />
        </td></tr>
        <tr><td >
        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/readStat.aspx" 
            Target="main">■  집계</asp:HyperLink>
        <br /></br>
        </td></tr> <tr><td >
        <br />
        </td></tr>
        <tr><td >
        <br />
        </td></tr>
        <tr><td >
        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Default.aspx?type=logout" 
            Target="_parent">■  로그아웃</asp:HyperLink>
        <br />
        <br />
            </td></tr></table>
<br />
    </form>
</body>
</html>
