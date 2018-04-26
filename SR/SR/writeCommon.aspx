<%@ Page Language="C#" AutoEventWireup="true" CodeFile="writeCommon.aspx.cs" Inherits="writeCommon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>공통코드 관리</title>
    <script language='javascript' src="common.js" type="text/javascript"></script>
    <script language='javascript' type="text/javascript">
        function btnEdit_Click() {
            f = document.form1;
            if (f.hdnComcd.value.length <= 0) {
                alert("신규등록 하셔야합니다!");
                return false;
            }else  if (f.txtComcd.value.length > 0) return confirm("정말 '수정' 하시겠습니까?");
            else { alert("공통코드를 확인하세요!"); return false; }
        }
        function btnDelete_Click() {
            f = document.form1;
            if (f.txtComcd.value.length > 0) return confirm("정말 '삭제' 하시겠습니까?");
            else { alert("공통코드를SEQ를 확인하세요!"); return false; }
        }
        function btnSave_Click() {
            f = document.form1;
            if ( f.txtComcd.value.length <= 0)  {
                alert("공통코드는 필수 항목입니다!");
                return false;
            }else if (f.txtComcd.value == f.hdnComcd.value)
            {
                alert("이미 존재하는 공통코드는 등록 할 수 없습니다!");
                return false;
            }else return confirm("정말 '등록' 하시겠습니까?");
        }
        function btnReset_Click() {
            f = document.form1;

            //f.txtUpcd.value = "";
            f.txtComcd.value = "";
            f.hdnComcd.value = "";
            f.txtComcdNm.value = "";
            f.txtSrt.value = "";
            f.chkIsUse.checked = false; 
            f.txtOptE.value = "";
            f.txtOptA.value = "";
            f.txtOptB.value = "";
            f.txtOptC.value = "";
            f.txtOptD.value = "";
            f.txtComcd.readOnly = false;
            f.txtComcd.style.backgroundColor = "#FFFFFF";
            //f.txtUpcd.value = cboUpcdNm.options[cboUpcdNm.selectedIndex].Value;
        }
        /*
        function btnSave_Click() {
        return confirm("정말 저장하시겠습니까?");
        }

        function btnDelete_Click() {
        confirm("정말 삭제하시겠습니까?");
        }

        function btnEdit_Click() {
        confirm("정말 수정하시겠습니까?");
        }
        */
        function ChangeComcd() {
          //  f = document.form1;
          //  if (f.txtComcd.value != f.hdnComcd.value)
          //  { f.txtComcd.style.color = "red"; }
          //  else { f.txtComcd.style.color = "black"; } 
        }
        function ChangeUpcd() {
            f = document.form1;
            f.txtSearch.value = f.cboUpcdNm.options[f.cboUpcdNm.selectedIndex].value;

            f.txtUpcd.value = f.cboUpcdNm.options[f.cboUpcdNm.selectedIndex].value;
            f.btnSearch.focus();
        }
        function ShowRow(rowUpcd, rowComcd, rowComcdNm, rowSrt, rowUse01, rowOptE, rowOptA, rowOptB, rowOptC, rowOptD) {
            f = document.form1;

            f.txtUpcd.value = rowUpcd;
            var i;
            for (i = 0; i < f.cboUpcdNm.length; i++) 
            {
                if ( f.cboUpcdNm.options[i].value == rowUpcd ){
                    document.form1.cboUpcdNm.options[i].selected = true;
                }
            }


            f.txtComcd.value = rowComcd;
            f.hdnComcd.value = rowComcd;
            f.txtComcdNm.value = rowComcdNm;
            f.txtSrt.value = rowSrt;

            if (rowUse01 == "1")
            { f.chkIsUse.checked = true; }
            else { f.chkIsUse.checked = false; }

            f.txtOptE.value = rowOptE;
            f.txtOptA.value = rowOptA;
            f.txtOptB.value = rowOptB;
            f.txtOptC.value = rowOptC;
            f.txtOptD.value = rowOptD;

            f.txtComcd.readOnly = true;
            f.txtComcd.style.backgroundColor = "#EAEAEA";
        }
     
    </script>
    <link href="common.css" type="text/css" rel="stylesheet"/>    
</head>
<body>
<br/>
    <form id="form1" runat="server">
        <DIV class="bullet_title" > 공통코드 관리</DIV>
    <table  class="action_table" >
        <tr>
            <td  class="action_td_lbl" width="120px" >
                <asp:Label ID="lblUpcd" runat="server" Text="상위코드" ></asp:Label>
            </td>
            <td  class="action_td_in" width="200px" >
                <asp:TextBox ID="txtUpcd" runat="server" CssClass="text" readonly="true" 
                    BackColor="#EAEAEA" Width="120px"></asp:TextBox>
            </td>
            <td  class="action_td_lbl" width="120px" >
                <asp:Label ID="lblUpcdNm" runat="server" Text="상위코드명"></asp:Label>
            </td>
            <td  class="action_td_in" width="200px" >
                <asp:DropDownList ID="cboUpcdNm" runat="server" CssClass="text"  
                    onChange="javascript:ChangeUpcd();" Width="120px" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblComcd" runat="server" Text="공통코드"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtComcd" runat="server"  CssClass="text" 
                    onChange="javascript:ChangeComcd();" Width="120px" ></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblComcdNm" runat="server" Text="공통코드명"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtComcdNm" runat="server"  CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblisUse" runat="server" Text="사용여부"></asp:Label>
            </td>
            <td  class="action_td_in" >
                <asp:CheckBox ID="chkIsUse" runat="server" />
            </td>
            <td  class="action_td_lbl" >
                <asp:Label ID="lblSrt" runat="server" Text="정렬순서"></asp:Label>
            </td>
            <td  class="action_td_in" >
                <asp:TextBox ID="txtSrt" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  class="action_td_lbl" >
                <asp:Label ID="lblOptE" runat="server" Text="옵션E설명"></asp:Label>
            </td>
            <td colspan=3  class="action_td_in" >
                <asp:TextBox ID="txtOptE" runat="server" CssClass="text" Width="380px" 
                    ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblOptA" runat="server" Text="옵션A(10)"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtOptA" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblOptB" runat="server" Text="옵션B(20)"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtOptB" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="action_td_lbl" >
                <asp:Label ID="lblOptC" runat="server" Text="옵션C(30)"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtOptC" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
            <td class="action_td_lbl" >
                <asp:Label ID="lblOptD" runat="server" Text="옵션D(40)"></asp:Label>
            </td>
            <td class="action_td_in" >
                <asp:TextBox ID="txtOptD" runat="server" CssClass="text" Width="120px"></asp:TextBox>
            </td>
        </tr>
       </table>
        <table class="button_table" width="709px">
            <tr>
                <td style="text-align:center;" >
                <asp:Button ID="btnEdit" runat="server" Text="수정"  CssClass="btn_search" onClientClick="javascript:return btnEdit_Click();" onclick="btnEdit_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="삭제"  CssClass="btn_search" onClientClick="javascript:return btnDelete_Click();" onclick="btnDelete_Click" />
                <asp:Button ID="btnSave" runat="server" Text="등록"  CssClass="btn_search" onClientClick="javascript:return btnSave_Click();" onclick="btnSave_Click" />
                <asp:Button ID="btnReset" runat="server" Text="초기화"   CssClass="btn_search" onClientClick="javascript:return btnReset_Click();"/>
                <asp:HiddenField ID="hdnComcd" runat="server" />
                </td>
            </tr>
            <tr>
            <td height="20px"></td></tr>
         </table>
        <div name="GridviewDiv" id="GridviewDiv" class="result_table" style="overflow-y:scroll;width:707px;height:400px" >
         <asp:GridView 
            ID="listComcd" 
            runat="server" 
            onrowcreated="listComcd_RowCreated"
            onrowdatabound="listComcd_RowDataBound"
            AllowPaging="True" 
            onpageindexchanging="listComcd_PageIndexChanging" 
            onselectedindexchanged="listComcd_SelectedIndexChanged" 
            PageSize="1000"
            CssClass="listTable"
            style="width:690px !important" >
          </asp:GridView>
<!--
-->
          </div>
          <table class="button_table" width="709px">
            <tr>
                <td style="text-align:center;" >
                <asp:DropDownList ID="cboSearch" runat="server">
                    <asp:ListItem Value="UpCD">UPCD</asp:ListItem>
                    <asp:ListItem Value="ComCD">COMCD</asp:ListItem>
                    <asp:ListItem Value="comcdnm">VALUE</asp:ListItem>
                    <asp:ListItem Value="OptE">옵션E</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="검색"  CssClass="btn_search"  onclick="btnSearch_Click" />
                </td>
            </tr>
            <tr>
            <td height="20px"></td></tr>
         </table>
    </form>
</body>
</html>
