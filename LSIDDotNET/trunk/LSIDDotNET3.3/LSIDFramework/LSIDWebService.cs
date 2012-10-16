using System;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.IO;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Messaging;
using Microsoft.Web.Services2.Attachments;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 * 
	 * Base class for LSID web services.  Contains functionality for performing authentication and extracting request info
	 * from URL, headers, etc...
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDWebService : SoapService
    {
		public static HttpContext CurrentHTTPContext = null;
		public static LSIDCredentials Credentials = null;
		                
        /**
        * Creates the LSID request context given the LSID, and authenticates the request.
        * @param LSID the lsid from the soap parameters
        * @return LSIDRequestContext the authenticated and populated request context
        */
        protected virtual LSIDRequestContext GetRequestContext(LSID lsid) 
        {			
            LSIDRequestContext ctx = new LSIDRequestContext();
            
            ctx.Lsid = lsid;
            
            foreach ( String name in CurrentHTTPContext.Request.Headers.AllKeys )
            {
                ctx.addProtocolHeader(name, CurrentHTTPContext.Request.Headers[name].ToString());
            }

            ctx.Credentials = Credentials;            
            ctx.ReqUrl = CurrentHTTPContext.Request.Url.GetLeftPart(UriPartial.Path);
      
			string hint = CurrentHTTPContext.Request.PathInfo;
			if (hint != null && hint.Length > 0 && hint[0] == '/')
			{
                ctx.Hint = hint; 
            }

            AuthenticationResponse authresp = null;
            LSIDSecurityService service = (LSIDSecurityService) LSIDFramework.Global.AuthenticationRegistry.lookupService(lsid);
            if (service != null) 
            {
                try 
                {
                    authresp = service.authenticate(ctx);
                } 
                catch (LSIDAuthenticationException) 
                {
                    throw new LSIDException(LSIDException.AUTHENTICATION_ERROR, "Authentication failed");
                }
            }
            if (authresp != null && !authresp.Success) 
            {
                ctx.AuthResponse = authresp;
				throw new LSIDException(LSIDException.AUTHENTICATION_ERROR, "Authentication failed");
            }
            ctx.AuthResponse = authresp;
            return ctx;
        }

		protected void createMimeMessage(SoapEnvelope env, string attachmentStr)
		{
			string envGuid = Guid.NewGuid().ToString();
			string attGuid = Guid.NewGuid().ToString();
					
			CurrentHTTPContext.Response.ContentType = "Multipart/Related; type=\"text/xml\"; start=\"<" + envGuid + ">\"; boundary=\"--MIME_boundary\"";

			//soap envelope
			string mimeMsg = "--MIME_boundary\r\n";
			mimeMsg += "Content-Type: text/xml; charset=UTF-8\r\n";
			mimeMsg += "Content-Transfer-Encoding: binary\r\n";
			mimeMsg += "Content-ID: <" + envGuid + ">\r\n";

			CurrentHTTPContext.Response.Write(mimeMsg);
							
			CurrentHTTPContext.Response.Write(env.OuterXml + "\r\n");
		
			//attachments
			mimeMsg = "--MIME_boundary\r\n";
			mimeMsg += "Content-Type: text/xml; charset=UTF-8; \r\n"; //name="..."?
			mimeMsg += "Content-Transfer-Encoding: binary\r\n";
			mimeMsg += "Content-ID: <" + attGuid + ">\r\n";

			CurrentHTTPContext.Response.Write(mimeMsg);
		
			CurrentHTTPContext.Response.Write(attachmentStr);	
		}
		
		protected void createMimeMessage(SoapEnvelope env, Stream attachmentStr)
		{
			string envGuid = Guid.NewGuid().ToString();
			string attGuid = Guid.NewGuid().ToString();
					
			CurrentHTTPContext.Response.ContentType = "Multipart/Related; type=\"text/xml\"; start=\"<" + envGuid + ">\"; boundary=\"--MIME_boundary\"";

			//soap envelope
			string mimeMsg = "--MIME_boundary\r\n";
			mimeMsg += "Content-Type: text/xml; charset=UTF-8\r\n";
			mimeMsg += "Content-Transfer-Encoding: binary\r\n";
			mimeMsg += "Content-ID: <" + envGuid + ">\r\n";

			CurrentHTTPContext.Response.Write(mimeMsg);
							
			CurrentHTTPContext.Response.Write(env.OuterXml + "\r\n");
		
			//attachments
			mimeMsg = "--MIME_boundary\r\n";
			mimeMsg += "Content-Type: text/xml; charset=UTF-8; \r\n"; //name="..."?
			mimeMsg += "Content-Transfer-Encoding: binary\r\n";
			mimeMsg += "Content-ID: <" + attGuid + ">\r\n";

			CurrentHTTPContext.Response.Write(mimeMsg);
		
			StreamReader rdr = new StreamReader(attachmentStr);
			char[] b = new char[1];
			while (rdr.Read(b, 0, 1) != 0)
			{
				CurrentHTTPContext.Response.Write(b, 0, 1);	
			}
		}


        /**
         * a response header to the response message of the current message context
         */
        protected void addResponseHeader(String name, String val) 
        {
            try 
            {
                CurrentHTTPContext.Response.AddHeader(name, val);
            } 
            catch (Exception ) 
            {
                throw new Exception("Error adding response header");
            }
        }

        /**
         * add an expiration header
         */
        protected void addExpirationHeader(DateTime expires) 
        {
            addResponseHeader(SoapConstants.EXPIRES_HEADER_ELT, SOAPUtils.writeDate(expires));
        }

        /**
         * get the lsid from the given body element
         */
        protected LSID getLSID(XmlNode bodyElt) 
		{
			XmlNodeList nodes = bodyElt.SelectNodes(WSDLConstants.LSID_PART);
				
			if (nodes.Count == 0)
			{
				throw new LSIDException(LSIDException.INVALID_METHOD_CALL, "Must specify LSID parameter");
			}
			return new LSID(nodes[0].InnerText);

        }

        protected String getStringValue(XmlNode bodyElt, String partname) 
        {
			XmlNodeList n = bodyElt.SelectNodes(partname);
			if (n.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "Must specify " + partname);
			return n[0].InnerText;
        }

//
        protected void addStringValue(XmlNode bodyElt, String partname, String val) 
        {
            try 
            {
                XmlElement elt = bodyElt.OwnerDocument.CreateElement(partname);
				elt.AppendChild(bodyElt.OwnerDocument.CreateTextNode(val));
				bodyElt.AppendChild(elt);
            } 
            catch (Exception e) 
            {
                throw new HttpException("error building response", e);
            }
        }


    }
}
