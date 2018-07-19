using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using System.Configuration;

public partial class _Default : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //로그아웃을 클릭해서 들어오면 쿠키만료
            if (Request["type"] == "logout")
            {
                L_login.Text = Request["type"];
                Response.Cookies["UserSettings"].Value = null;
                Request.Cookies.Remove("UserSettings");
            }

            //default페이지를 새로 열었을때 쿠키가 살아 있으면 다음 페이지로 자동 이동한다.
          //  if (Request.Cookies["UserSettings"] != null && Request.Cookies["UserSettings"]["USERID"] != null)
          //  {
            //    Response.Redirect("frame.aspx");
             //  // HttpContext.Current.Response.Write("<script>document.location ='./frame.aspx'</script>");
           // }
        }
    }

    protected void B_login_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        string sql = string.Empty;
        string USERID = string.Empty;
        string USERNM = string.Empty;
        string USERLVL = string.Empty;

        USERID = TB_user.Text;

        getUserInfo(TB_user.Text, TB_password.Text, out USERNM, out USERLVL);

        if (!string.IsNullOrEmpty(USERNM) && USERNM != "")
        {
            setCookie(USERID, USERNM, USERLVL);
            Response.Redirect("frame.aspx");
            //HttpContext.Current.Response.Write("<script>document.location ='./frame.aspx'</script>");
        }
        else
        {
            L_login.Text = "ID 또는 비밀번호 불일치";
        }
    }
}
