<%@ Page Language="C#" AutoEventWireup="true" CodeFile="readStat.aspx.cs" Inherits="readStat" %>

<%@ Register Assembly="AjaxControlToolkit, Version=3.5.51116.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e"
    Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="common.css" type="text/css" rel="stylesheet" /> 
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <script language='javascript' type="text/javascript">
    </script>
    <title>전산요청 조회(목록)</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div  class="bullet_title" >
        집계
    </div>
        <table class="action_table" >
             <tr>
                <td  class="action_td_lbl" width="120px">
                    <asp:Label ID="lblStartTime" runat="server" Text="기간(From)"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnStartTime" runat="server" ImageUrl="image/calendar.gif" ImageAlign="AbsMiddle" />
                    <asp:CalendarExtender ID="calStartTime" runat="server" TargetControlID="txtStartTime" PopupButtonID="btnStartTime" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender> 
                 </td>
                <td  class="action_td_lbl" width="120px">
                    <asp:Label ID="lblEndTime" runat="server" Text="기간(To)"></asp:Label>
                </td>
                <td class="action_td_in" width="200px">
                    <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnEndTime" runat="server" ImageUrl="image/calendar.gif" ImageAlign="AbsMiddle"/>
                    <asp:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndTime" PopupButtonID="btnEndTime" Format="yyyy-MM-dd" TodaysDateFormat="yyyy-MM-dd"></asp:CalendarExtender>
                </td>
            </tr>
        </table>
        <table class="button_table" width="709px">
            <tr>
                <td style="text-align:center;" ><asp:Button ID="btnSearch" runat="server" Text="검색" onclick="btnSearch_Click" CssClass="btn_search"  /></td>
            </tr>
        </table>
        <p></p>
        <p>1.주간집계용</p>
        <asp:GridView 
            ID="listTask" 
            runat="server" 
            ShowHeader="true" 
            CssClass="listTable" 
            style="width:709px !important" 
            CellPadding="0" PageSize="100">
        </asp:GridView>
        <p></p>
        <p>2.월간집계용</p>
        <asp:GridView 
            ID="monStat" 
            runat="server" 
            ShowHeader="true" 
            CssClass="listTable" 
            style="width:709px !important" 
            CellPadding="0" PageSize="100">
        </asp:GridView>
    </form>    
</body>
</html>
