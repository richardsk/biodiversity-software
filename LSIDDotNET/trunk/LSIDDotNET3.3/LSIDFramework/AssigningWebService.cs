using System;
using System.Web;
using System.Xml;
using System.IO;
using System.Collections;

using Microsoft.Web.Services2;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 * 
	 *  Web service front end for a collection of LSID Assigning Services.  Uses the service registry to determine which
	 * service implementation to invoke.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */

	public class AssigningWebService : LSIDWebService 
	{
		public AssigningWebService(HttpContext context, LSIDCredentials creds)
		{
			CurrentHTTPContext = context;
			Credentials = creds;
		}

		/**
		 * assign an LSID for the given parameters
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void assignLSID(XmlNodeList bodyElements) 
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");
			String authority = getStringValue(bodyElements[0], WSDLConstants.AUTHORITY_PART);
			String ns = getStringValue(bodyElements[0], WSDLConstants.NAMESPACE_PART);
			
			XmlNodeList n = bodyElements[0].SelectNodes(WSDLConstants.PROPERTY_LIST_PART);
			if (n.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "must specify property list");
        
			Properties props = extractPropertyList(n[0]);
			LSID lsid = service.assignLSID(authority, ns, props);
		
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.ASSIGN_LSID_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			addStringValue(ret, WSDLConstants.LSID_PART, lsid.ToString());

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);
		}

		/**
		 * assign an LSID for the given parameters from a list of LSIDs
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void assignLSIDFromList(XmlNodeList bodyElements)
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");

			XmlNodeList pl = bodyElements[0].SelectNodes(WSDLConstants.PROPERTY_LIST_PART);
			if (pl.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "must specify property list");

			Properties props = extractPropertyList(pl[0]);

			XmlNodeList sl = bodyElements[0].SelectNodes(WSDLConstants.SUGGESTED_LSIDS_PART);
			if (sl.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "must specify suggested lsids");

			LSID[] lsids = extractLSIDList(sl[0]);

			LSID lsid = service.assignLSIDFromList(props, lsids);

		
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.ASSIGN_LSID_FROM_LIST_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			addStringValue(ret, WSDLConstants.LSID_PART, lsid.ToString());

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);

		}

		/**
		 * get an LSID pattern for assigning LSIDs
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void getLSIDPattern(XmlNodeList bodyElements) 
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");
			String authority = getStringValue(bodyElements[0], WSDLConstants.AUTHORITY_PART);
			String ns = getStringValue(bodyElements[0], WSDLConstants.NAMESPACE_PART);

			XmlNodeList pl = bodyElements[0].SelectNodes(WSDLConstants.PROPERTY_LIST_PART);
			if (pl.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "must specify property list");
			

			Properties props = extractPropertyList(pl[0]);
			String pattern = service.getLSIDPattern(authority, ns, props);
			
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_LSID_PATTERN_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			addStringValue(ret, WSDLConstants.LSID_PATTERN_PART, pattern);

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);
		}

		/**
		 * assign an LSID pattern from the given list of patterns
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void getLSIDPatternFromList(XmlNodeList bodyElements) 
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");

			XmlNodeList pl = bodyElements[0].SelectNodes(WSDLConstants.PROPERTY_LIST_PART);
			if (pl.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "must specify property list");

			Properties props = extractPropertyList(pl[0]);

			XmlNodeList sl = bodyElements[0].SelectNodes(WSDLConstants.SUGGESTED_LSID_PATTERNS_PART);
			if (sl.Count < 1) throw new HttpException(LSIDException.INVALID_METHOD_CALL, "must specify suggested lsids");

			String[] patterns = extractPatternList(sl[0]);

			String pattern = service.getLSIDPatternFromList(props, patterns);
			
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_LSID_PATTERN_FROM_LIST_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			addStringValue(ret, WSDLConstants.LSID_PATTERN_PART, pattern);

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);
		}

		/**
		 * assign a new revision of the given LSID
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void assignLSIDForNewRevision(XmlNodeList bodyElements) 
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");
			LSID prevLsid = new LSID(getStringValue(bodyElements[0], WSDLConstants.PREVIOUS_LSID_PART));
			LSID lsid = service.assignLSIDForNewRevision(prevLsid);
			
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.ASSIGN_LSID_FOR_NEW_REVISION_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));
			addStringValue(ret, WSDLConstants.LSID_PART, lsid.ToString());			

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);

		}

		/**
		 * get the allowed property names
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void getAllowedPropertyNames(XmlNodeList bodyElements) 
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");
			String[] names = service.getAllowedPropertyNames();
						
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_ALLOWED_PROPERTY_NAMES_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));

			XmlElement elt = respEnv.CreateElement(WSDLConstants.PROPERTY_NAMES_PART);
			addPropertyNameList(elt, names);

			try 
			{
				ret.AppendChild(elt);
			} 
			catch (Exception e) 
			{
				throw new HttpException("error building response", e);
			}

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);

			//return new SOAPBodyElement[] { ret };
		}

		/**
		 * assign an LSID for the given parameters
		 * @param SOAPBodyElement[] the body elements
		 * @return SOAPBodyElement[] the return body elements 
		 */
		public void getAuthoritiesAndNamespaces(XmlNodeList bodyElements) 
		{
			// for assigning service, no map is specified so we just give a bogus LSID for registry lookups
			LSID bogusLsid = new LSID("urn:lsid:foo:bar:row");
			GetRequestContext(bogusLsid); //do authentication
			LSIDAssigningService service = (LSIDAssigningService) LSIDFramework.Global.AssigningRegistry.lookupService(bogusLsid);
			if (service == null)
				throw new HttpException(LSIDException.SERVER_CONFIGURATION_ERROR, "no assigning service available");
			String[][] names = service.getAuthoritiesAndNamespaces();
						
			SoapEnvelope respEnv = new SoapEnvelope();
			XmlNode ret = respEnv.CreateBody().AppendChild(respEnv.CreateElement(SoapConstants.GET_AUTHORITIES_AND_NAMESPACES_OP_NAME + SoapConstants.OPERATION_RESPONSE_SUFFIX, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI));

			XmlElement elt = respEnv.CreateElement(WSDLConstants.AUTHORITY_AND_NAMESPACES_PART);
			addAuthoritiesAndNamespaces(elt, names);

			try 
			{
				ret.AppendChild(elt);
			} 
			catch (Exception e) 
			{
				throw new HttpException("error building response", e);
			}

			CurrentHTTPContext.Response.Write(respEnv.OuterXml);

		}

		/**
		 * Utility method to extract a property list from the given body element that is the part 
		 * containing the list.
		 * @param elt
		 * @return
		 */
		private static Properties extractPropertyList(XmlNode elt) 
		{
			Properties props = new Properties();

			String xpathStr = WSDLConstants.ANB + ":" + WSDLConstants.PROPERTY_ELT;                
			XmlNamespaceManager mgr = new XmlNamespaceManager(elt.OwnerDocument.NameTable);
			mgr.AddNamespace(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);

			try
			{				
				XmlNodeList nl = elt.SelectNodes(xpathStr, mgr);
				foreach (XmlNode n in nl)
				{
					Property p = new Property();
				
					xpathStr = WSDLConstants.NAME_ELT;   
					XmlNodeList nn = n.SelectNodes(xpathStr, mgr);
					p.setName(nn[0].InnerText);

					xpathStr = WSDLConstants.VALUE_ELT;   
					XmlNodeList vn = n.SelectNodes(xpathStr, mgr);
					p.setValue(vn[0].InnerText);

					props.addProperty(p);
				}
			}
			catch(Exception ex)
			{
				throw new LSIDException(ex, "Invalid properties");
			}

			return props;
		}

		private static LSID[] extractLSIDList(XmlNode elt)
		{			
			String xpathStr = WSDLConstants.ANB + ":" + WSDLConstants.LSID_ELT;                
			XmlNamespaceManager mgr = new XmlNamespaceManager(elt.OwnerDocument.NameTable);
			mgr.AddNamespace(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			
			ArrayList lsids = new ArrayList();

			XmlNodeList nodes = elt.SelectNodes(xpathStr, mgr);
			foreach (XmlNode n in nodes)
			{
				lsids.Add(new LSID(n.InnerText));
			}

			return (LSID[])lsids.ToArray(typeof(LSID));

		}

		private static String[] extractPatternList(XmlNode elt) 
		{
			String xpathStr = WSDLConstants.ANB + ":" + WSDLConstants.LSID_PATTERN_ELT;                
			XmlNamespaceManager mgr = new XmlNamespaceManager(elt.OwnerDocument.NameTable);
			mgr.AddNamespace(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			
			ArrayList patterns = new ArrayList();

			XmlNodeList nodes = elt.SelectNodes(xpathStr, mgr);
			foreach (XmlNode n in nodes)
			{
				patterns.Add(n.InnerText);
			}

			return (String[])patterns.ToArray(typeof(String));
		}

		private static void addPropertyNameList(XmlElement part, String[] propertyNames) 
		{
			for (int i = 0; i < propertyNames.Length; i++) 
			{
                XmlElement propelt = part.OwnerDocument.CreateElement(WSDLConstants.ANB, WSDLConstants.PROPERTY_NAME_ELT, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);

				try 
				{
					propelt.InnerText = propertyNames[i]; 
					part.AppendChild(propelt);
				} 
				catch (Exception e) 
				{
					throw new HttpException("Error building property list", e);
				}
			}
		}

		private static void addAuthoritiesAndNamespaces(XmlElement part, String[][] auths) 
		{
			for (int i = 0; i < auths.Length; i++) 
			{
				try 
				{
					XmlElement anelt = part.OwnerDocument.CreateElement(WSDLConstants.ANB, WSDLConstants.AUTHORITY_NAMESPACE_ELT, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
					XmlElement authelt = part.OwnerDocument.CreateElement(WSDLConstants.ANB, WSDLConstants.AUTHORITY_ELT, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
					XmlElement nselt = part.OwnerDocument.CreateElement(WSDLConstants.ANB, WSDLConstants.NAMESPACE_ELT, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);

					anelt.AppendChild(authelt);
					anelt.AppendChild(nselt);
					authelt.AppendChild(part.OwnerDocument.CreateTextNode(auths[i][0]));
					nselt.AppendChild(part.OwnerDocument.CreateTextNode(auths[i][1]));
					part.AppendChild(anelt);
				} 
				catch (Exception e) 
				{
					throw new HttpException("Error building property list", e);
				}
			}
		}

	}
}
