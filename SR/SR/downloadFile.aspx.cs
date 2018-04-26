using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

public partial class downloadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string regdt = Request.QueryString["regdt"];
        string serverpath = ConfigurationManager.AppSettings["filepath"] + "\\" + regdt;
        string originname = HttpUtility.UrlDecode(Request.QueryString["originname"]);
        string savename = HttpUtility.UrlDecode(Request.QueryString["savename"]);

        // 버퍼를 비운다.
        Response.Clear();
        // 내려보낼 데이터의 형식을 지정
        Response.ContentType = "Application/UnKnown"; // 파일의 타입에 상관없이 강제적으로 다운로드 창이 뜬다.
        // aspx파일이 아닌 원래의 파일 이름으로 다운로드 하도록 한다.
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(originname));

        // 버퍼에 바이너리 데이터를 모두 채운다.
        Response.WriteFile(serverpath + "\\" + savename);
        // 그 데이터를 브라우저로 내려보낸다.
        Response.End();
    }
}
