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
	 *  Web service front end for a collection of LSID Data Services.  Uses the service registry to determine which
	 * service implementation to invoke.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class DataWebService : LSIDWebService 
	{
		public DataWebService(HttpContext context, LSIDCredentials creds)
		{
			CurrentHTTPContext = context;
			Credentials = creds;
		}
	
		/**
		 * get the data for the the given lsid
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements.
		 */
		public void getData(XmlNodeList bodyElements) 
		{
			Stream data = null;
		
			LSID theLsid = getLSID(bodyElements[0]);
			LSIDRequestContext ctx = GetRequestContext(theLsid);
			//ServiceRegistry reg = getServiceRegistry(DATA_SERVICE_IMPLEMENTATION_REGISTRY);
			LSIDDataService service = (LSIDDataService)LSIDFramework.Global.DataRegistry.lookupService(theLsid);
			if (service == null)
				throw new LSIDServerException(LSIDException.UNKNOWN_LSID,"Unknown lsid: " + theLsid);
			data = service.getData(ctx);
		
			//http response	
			SoapEnvelope respEnv = new SoapEnvelope();
			respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_DATA_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			createMimeMessage(respEnv, data);		
		}
	
		/**
		 * get the data for the the given lsid and range
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements.
		 */
		public void getDataByRange(XmlNodeList bodyElements) 
		{
			Stream data = null;
		
			LSID theLsid = getLSID(bodyElements[0]);
			LSIDRequestContext ctx = GetRequestContext(theLsid);
			int start = getRangeValue(bodyElements[0], WSDLConstants.START_PART);
			int length = getRangeValue(bodyElements[0],WSDLConstants.LENGTH_PART);
			
			//ServiceRegistry reg = getServiceRegistry(DATA_SERVICE_IMPLEMENTATION_REGISTRY);
			LSIDDataService service = (LSIDDataService)LSIDFramework.Global.DataRegistry.lookupService(theLsid);
			data = service.getDataByRange(ctx, start, length);
		
			SoapEnvelope respEnv = new SoapEnvelope();
			respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_DATA_BY_RANGE_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			createMimeMessage(respEnv, data);
	
		}
	
		private int getRangeValue(XmlNode bodyElt, String rangePart) 
		{
			XmlNodeList n = bodyElt.SelectNodes(rangePart);
			if (n == null || n.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL,"Must specify " + rangePart + " parameter");
		
			String eltValue = n[0].Value;
			try 
			{
				return int.Parse(eltValue);
			} 
			catch (FormatException) 
			{
				throw new HttpException(LSIDException.INVALID_METHOD_CALL,"Bad range parameter: " + eltValue);
			}
		}

	}
}
