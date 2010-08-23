using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.IO;

namespace TapirDotNET 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			
			
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
            //LSID?
            Regex lsidRex = new Regex(@"^(.+).aspx\/authority\/(.+){0,}", RegexOptions.IgnoreCase);
            Match lsidMatch = lsidRex.Match(Request.RawUrl);
            if (lsidMatch.Success)
            {
                if (Request["lsid"] != null)
                {
                    // an lsid has been passed so they are calling
                    // the getAvailableServices(lsid) method and we
                    // should return the appropriate WSDL.

                    TpLsidResolver res = new TpLsidResolver(Request["lsid"]);

                    string url = res.GetTemplateUrl();

                    if (url == "")
                    {
                        Response.Status = "404 Not found.";
                        Response.StatusCode = 404;
                        Response.End();
                    }
                    else
                    {
                        Response.ContentType = "text/xml; charset=\"utf-8\"";

                        // Metadata will be a direct TAPIR call
                        //data will return nothing

                        string resp = File.ReadAllText(TpConfigManager.TP_WSDL_DIR + "\\" + "LsidDataServices.wsdl");
                        resp = resp.Replace("[LSID_METADATA_ADDRESS]", url);
                        resp = resp.Replace("[LSID_DATA_ADDRESS]", "");
                        TpUtils.WriteUTF8(Response, resp);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    // no lsid has been passed so they are asking
                    // how they should call the getAvailableServices(lsid) method.
                    // we should return an WSDL with the right location in it.

                    Response.ContentType = "text/xml";

                    string resp = File.ReadAllText(TpConfigManager.TP_WSDL_DIR + "\\" + "LsidAuthority.wsdl");
                    resp = resp.Replace("[LSID_AUTHORITY_ADDRESS]", "http://" + Request.Params["SERVER_NAME"] + Request.Path);
                    TpUtils.WriteUTF8(Response, resp);
                    Response.Flush();
                    Response.End();                    
                }
                return;
            }

			Regex rex = new Regex(@"^(.+\/)tapir\.aspx\/(.+)[\?](.+){0,}", RegexOptions.IgnoreCase);
			Match m = rex.Match(Request.RawUrl);
			if (!m.Success)
			{
				rex = new Regex(@"^(.+\/)tapir\.aspx\/(.+)", RegexOptions.IgnoreCase);
				m = rex.Match(Request.RawUrl);
			}
			if (m.Success)
			{
				string dsa = m.Groups[2].Value.Replace("/", "");
				string end = "";
				if (m.Groups.Count > 3) end = m.Groups[3].Value.Replace("?", "");

				Context.RewritePath(m.Groups[1].Value + "tapir.aspx?dsa=" + dsa + "&" + end);
			}
			
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			
			
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{
			
			
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

