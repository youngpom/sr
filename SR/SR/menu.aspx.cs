using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class menu : common
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Request.Cookies["UserSettings"] != null &&
             Request.Cookies["UserSettings"]["USERID"] != null && 
             Request.Cookies["UserSettings"]["ExpiresDate"] != null 
             && Request.Cookies["UserSettings"].Expires != null
           )
        {
            lblCookieUserID.Text = Request.Cookies["UserSettings"]["USERID"].ToString() + " 님";
            HyperLink6.NavigateUrl = HyperLink6.NavigateUrl + "?devempid=" + Request.Cookies["UserSettings"]["USERID"].ToString();
        }
        else
        {
            //back처리
        }
 HyperLink2.Visible = false;
HyperLink8.Visible = false;
        if (IsAdmin() == false)
        {
            HyperLink3.Visible = false;
            HyperLink2.Visible = false;
            HyperLink4.Visible = false;
            HyperLink5.Visible = false;
            HyperLink8.Visible = false;
        }

        //lblIP.Text = HttpContext.Current.Request.UserHostAddress.ToString();
        string hostname = System.Net.Dns.GetHostName();
        System.Net.IPAddress[] ipaddresses = System.Net.Dns.Resolve(hostname).AddressList;
        string SVRIP = null;


      //  lblIP.Text = "서버 : ";
        foreach (System.Net.IPAddress ipaddress in ipaddresses)
        { SVRIP = ipaddress.ToString(); 
        }

       // lblIP.Text = lblIP.Text +   hostname.ToString();
        //if ( SVRIP == "192.168.106.23" ) { lblIP.Text = "Live"; }
        //else { lblIP.Text = "Test"; }
    }
}
