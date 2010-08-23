using System;
using System.Web;
using System.Xml;
using System.IO;

using Microsoft.Web.Services2;

using LSIDClient;

namespace LSIDFramework
{

	/**
	 * 
	 *  Web service front end for a collection of LSID Metadata Services.  Uses the service registry to determine which
	 * service implementation to invoke.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class MetadataWebService : LSIDWebService 
	{
		public MetadataWebService(HttpContext context, LSIDCredentials creds)
		{
			CurrentHTTPContext = context;
			Credentials = creds;
		}
	
		/**
		 * get the metadata for the the given lsid
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void getMetadata(XmlNodeList bodyElements) 
		{
			LSID theLsid = getLSID(bodyElements[0]);
			LSIDRequestContext ctx = GetRequestContext(theLsid);
			String[] formats = null;
			formats = getAcceptedFormats(bodyElements[0]);
			LSIDMetadataService service = (LSIDMetadataService)LSIDFramework.Global.MetadataRegistry.lookupService(theLsid);
			if (service == null)
				throw new HttpException(LSIDException.UNKNOWN_LSID,"Unknown lsid: " + theLsid);
			MetadataResponse response = service.getMetadata(ctx, formats);

			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_METADATA_OP_NAME+ SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			XmlElement fp = respEnv.CreateElement(WSDLConstants.FORMAT_PART);
			fp.AppendChild(respEnv.CreateTextNode(response.getFormat()));
			ret.AppendChild(fp);
		
			if (response.getExpires() != DateTime.MinValue)
			{
				XmlElement dt = respEnv.CreateElement(WSDLConstants.EXPIRATION_PART);
				dt.AppendChild(respEnv.CreateTextNode(SOAPUtils.writeDate(response.getExpires())));
			}

			createMimeMessage(respEnv, (Stream)response.getValue());			
		}
	
	
		/**
		 * parse the accepted formats from the string
		 */
		private String[] getAcceptedFormats(XmlNode bodyElt) 
		{
			XmlNodeList n = bodyElt.SelectNodes(WSDLConstants.ACCEPTED_FORMATS_PART);
			if (n.Count > 0)
			{
				XmlNode elt = n[0];
				string formats = elt.InnerText;
				return formats.Split(',');
			}

			return null;
		}
		
		/**
		 * extend the base impl of getRequestContext to extract the hint from the URL
		 */
		protected override LSIDRequestContext GetRequestContext(LSID lsid) 
		{
			LSIDRequestContext rc = base.GetRequestContext(lsid);
			String hint = CurrentHTTPContext.Request.PathInfo;
			if (hint != null && hint.Length > 0 && hint[0] == '/')
				hint = hint.Substring(1);
			rc.Hint = hint;
			return rc;		
		}
	}
}
