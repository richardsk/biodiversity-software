using System;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Web;

using Microsoft.Web.Services2;

using LSIDClient;


namespace LSIDFramework
{
	/**
	 * 
	 * Web service front end for a collection of LSID Authority Services.  Uses the service registry to determine which
	 * service implementation to invoke.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class AuthorityWebService : LSIDWebService 
	{
		public AuthorityWebService(HttpContext context, LSIDCredentials creds)
		{
			CurrentHTTPContext = context;
			Credentials = creds;
		}

		/**
		 * get the services WSDL for the the given lsid
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements
		 * returned as an attachment.
		 */
		public void getAvailableServices(XmlNodeList bodyElements) 
		{
			LSID theLsid = getLSID(bodyElements[0]);
			LSIDRequestContext ctx = GetRequestContext(theLsid);
			LSIDAuthorityService service = (LSIDAuthorityService) LSIDFramework.Global.TheServiceRegistry.lookupService(theLsid);
			if (service == null)
				throw new LSIDServerException(LSIDException.UNKNOWN_LSID, "Unknown lsid: " + theLsid);
			ExpiringResponse resp = service.getAvailableServices(ctx);
			if (resp.getExpires() != DateTime.MinValue)
				addExpirationHeader(resp.getExpires());

			//http mime response
			SoapEnvelope respEnv = new SoapEnvelope();
			respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_WSDL_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			createMimeMessage(respEnv, resp.getValue().ToString());
		}

		/**
		 * register a foreign authority for the given LSID
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements
		 * returned as an attachment.
		 */
		public void notifyForeignAuthority(XmlNodeList bodyElements) 
		{
			LSID theLsid = getLSID(bodyElements[0]);
			LSIDAuthority auth = getAuthority(bodyElements[0]);
			LSIDRequestContext ctx = GetRequestContext(theLsid);
			LSIDAuthorityService service = (LSIDAuthorityService) LSIDFramework.Global.TheServiceRegistry.lookupService(theLsid);				
			if (service == null)
				throw new LSIDServerException(LSIDException.UNKNOWN_LSID, "Unknown lsid: " + theLsid);
			service.notifyForeignAuthority(ctx, auth);

			SoapEnvelope env = new SoapEnvelope();
			env.CreateBody().AppendChild(env.CreateElement(SoapConstants.NOTIFY_FOREIGN_AUTHORITY_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			CurrentHTTPContext.Response.Write(env.OuterXml);

		}

		/**
		 * register a foreign authority for the given LSID
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements
		 * returned as an attachment.
		 */
		public void revokeNotificationForeignAuthority(XmlNodeList bodyElements) 
		{
			LSID theLsid = getLSID(bodyElements[0]);
			LSIDAuthority auth = getAuthority(bodyElements[0]);
			LSIDRequestContext ctx = GetRequestContext(theLsid);
			LSIDAuthorityService service = (LSIDAuthorityService) LSIDFramework.Global.TheServiceRegistry.lookupService(theLsid);
			if (service == null)
				throw new LSIDServerException(LSIDException.UNKNOWN_LSID, "Unknown lsid: " + theLsid);
			service.revokeNotificationForeignAuthority(ctx, auth);

			SoapEnvelope env = new SoapEnvelope();
			env.CreateBody().AppendChild(env.CreateElement(SoapConstants.REVOKE_NOTIFICATION_FOREIGN_AUTHORITY_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			CurrentHTTPContext.Response.Write(env.OuterXml);

		}

		/**
			 * get the lsid from the given body element
			 */
		protected LSIDAuthority getAuthority(XmlNode bodyElt) 
		{
			XmlNodeList nodes = bodyElt.SelectNodes(WSDLConstants.AUTHORITY_NAME_PART);
				
			if (nodes.Count == 0)
			{
				throw new LSIDException(LSIDException.INVALID_METHOD_CALL, "Must specify LSID parameter");
			}
			return new LSIDAuthority(nodes[0].InnerText);

		}

	}
}
