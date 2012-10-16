using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Configuration;

namespace LSIDClient
{
	/**
	 *
	 * Assign an LSID based on hints given to the assigning service
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
	public class LSIDAssigner 
	{
		LSIDStandardPort port = null;
		LSIDCredentials creds = null;

		static LSIDAssigner()
		{
			WebServicesConfiguration.MessagingConfiguration.AddTransport("http", SOAPUtils.LSIDTransport);	
		}

		public LSIDAssigner(LSIDStandardPort port) 
		{
			this.port = port;
		}

		public LSIDAssigner(LSIDStandardPort port, LSIDCredentials creds) 
		{
			this.port = port;
			this.creds = creds;
		}

		/**
		 * returns a full LSID for a data entity that has the properties passed in the 
		 * properties (like an object type and the attributes belonging to the data entity).
		 * This LSID has authority and namespace as requested by caller. Service returns null 
		 * if it cannot or does not want to name the object. properties is an array of 
		 * name/value pairs (both of type string).
		 * 
		 * @param authority
		 * @param namespace
		 * @param properies
		 * @return
		 * @throws LSIDException
		 */
		public LSID assignLSID(String authority, String ns, Properties properties) 
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}

			Hashtable ps = new Hashtable();
			ps.Add(WSDLConstants.AUTHORITY_PART, authority);
			ps.Add(WSDLConstants.NAMESPACE_PART, ns);
			ps.Add(WSDLConstants.PROPERTY_LIST_PART, properties);
		
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.ASSIGN_LSID_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);

			string lsid = resp.Body.ChildNodes[0].InnerText;
			return new LSID(lsid);
		}

		/**
		 * similar to the assignLSID, only the caller is suggesting the identifier itself. 
		 * "authority" and "namespace" parts for all suggestions should be the same. The 
		 * service can return one LSID from the list, something different (but with the same
		 * authority and namespace) or raise an exception.
		 * 
		 * @param properties
		 * @param suggested
		 * @return
		 * @throws LSIDException
		 */
		public LSID assignLSIDFromList(Properties properties, LSID[] suggested) 
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}

			Hashtable ps = new Hashtable();
			ps.Add(WSDLConstants.PROPERTY_LIST_PART, properties);
			ps.Add(WSDLConstants.SUGGESTED_LSIDS_PART, getSuggestedLSIDsXml(suggested));
		
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.ASSIGN_LSID_FROM_LIST_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);

			string lsid = resp.Body.ChildNodes[0].InnerText;
			return new LSID(lsid);
		}

		/**
		 * returns a prefix of an LSID such that the caller can use that as a template for
		 * constructing LSIDs and it is still guaranteed that these will be globally unique
		 * as far as the caller takes care not to reuse the same LSID twice locally.
		 * 
		 * @param authority
		 * @param namespace
		 * @param properties
		 * @return
		 * @throws LSIDException
		 */
		public String getLSIDPattern(String authority, String ns, Properties properties) 
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}
		
			Hashtable ps = new Hashtable();
			ps.Add(WSDLConstants.AUTHORITY_PART, authority);
			ps.Add(WSDLConstants.NAMESPACE_PART, ns);
			ps.Add(WSDLConstants.PROPERTY_LIST_PART, properties);
		
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.GET_LSID_PATTERN_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);

			return resp.Body.ChildNodes[0].InnerText;
		}

		/**
		 * combines assignLSIDFromList and getLSIDPattern.
		 * 
		 * @param properties
		 * @param suggested
		 * @return
		 * @throws LSIDException
		 */
		public String getLSIDPatternFromList(Properties properties, String[] suggested) 
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}

			Hashtable ps = new Hashtable();
			ps.Add(WSDLConstants.PROPERTY_LIST_PART, properties);
			ps.Add(WSDLConstants.SUGGESTED_LSID_PATTERNS_PART, getSuggestedPatternsXml(suggested)); 
		
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.GET_LSID_PATTERN_FROM_LIST_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);
			
			return resp.Body.ChildNodes[0].InnerText;
			
		}

		/**
		 * The caller sends in an identifier for an existing object and expects a new
		 * identifier with a different revision (for naming a new version of some object).
		 * 
		 * @param previousIdentifier
		 * @return
		 * @throws LSIDException
		 */
		public LSID assignLSIDForNewRevision(LSID previousIdentifier) 
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}
		
			Hashtable ps = new Hashtable();
			ps.Add(WSDLConstants.PREVIOUS_LSID_PART, previousIdentifier); 
		
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.ASSIGN_LSID_FOR_NEW_REVISION_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);

			string lsid = resp.Body.ChildNodes[0].InnerText;
			return new LSID(lsid);
		}

		/**
		 * returns an array of names of properties that the LSID Assigning Service can use
		 * when passed in methods assignLSID, assignLSIDFromList, and getLSIDPattern.
		 * @return
		 */
		public String[] getAllowedPropertyNames()
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}

			Hashtable ps = new Hashtable();		
			
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);

			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.GET_ALLOWED_PROPERTY_NAMES_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);

			XmlNamespaceManager mgr = new XmlNamespaceManager(resp.NameTable);
			mgr.AddNamespace(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			XmlNodeList n = resp.Body.ChildNodes[0].SelectNodes(WSDLConstants.PROPERTY_NAMES_PART, mgr);
			if (n.Count > 0)
			{
				return extractPropertyNames(n[0], mgr);
			}
			return null;
		
		}

		private static String getSuggestedPatternsXml(String[] suggested)
		{
			String sug = "";

			foreach( String sp in suggested)
			{
				sug += "<" + WSDLConstants.ANB + ":" + WSDLConstants.LSID_PATTERN_ELT + ">" + sp + "</" + WSDLConstants.ANB + ":" + WSDLConstants.LSID_PATTERN_ELT + ">";
			}

			return sug;
		}

		private static String getSuggestedLSIDsXml(LSID[] suggested)
		{
			String sug = "";

			foreach( LSID ls in suggested)
			{
				sug += "<" + WSDLConstants.ANB + ":" + WSDLConstants.LSID_PART + ">" + ls.ToString() + "</" + WSDLConstants.ANB + ":" + WSDLConstants.LSID_PART + ">";
			}

			return sug;
		}

		private static String[] extractPropertyNames(XmlNode elt, XmlNamespaceManager mgr) 
		{
			ArrayList res = new ArrayList();

			XmlNodeList nl = elt.SelectNodes(WSDLConstants.ANB + ":" + WSDLConstants.PROPERTY_NAME_ELT, mgr);			
			foreach (XmlNode n in nl)
			{
				res.Add(n.ChildNodes[0].InnerText);
			}
			return (String[])res.ToArray(typeof(String));
		}

		/**
		 * returns an array of pairs of Strings where the first item is the authority part
		 * that the service can assign names for, and the second String is valid namespace
		 * in that authority.
		 * 
		 * @return
		 */
		public String[][] getAuthoritiesAndNamespaces() 
		{
			if (port == null) 
			{
				throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unknown assigning endpoint");
			}

			Hashtable ps = new Hashtable();		
			
			Hashtable nsl = new Hashtable();
			nsl.Add(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);

			SoapEnvelope resp = SOAPUtils.MakeSoapCall(port.getLocation(), SoapConstants.GET_AUTHORITIES_AND_NAMESPACES_OP_NAME, WSDLConstants.OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI, nsl, ps, creds);

			XmlNamespaceManager mgr = new XmlNamespaceManager(resp.NameTable);
			mgr.AddNamespace(WSDLConstants.ANB, WSDLConstants.OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI);
			XmlNodeList n = resp.Body.ChildNodes[0].SelectNodes(WSDLConstants.AUTHORITY_AND_NAMESPACES_PART, mgr);
			if (n.Count > 0)
			{
				return extractAuthorityNSList(n[0], mgr);
			}
			return null;
		}
	
		private static String[][] extractAuthorityNSList(XmlNode elt, XmlNamespaceManager mgr)
		{
			ArrayList res = new ArrayList();

			XmlNodeList nl = elt.SelectNodes(WSDLConstants.ANB + ":" + WSDLConstants.AUTHORITY_NAMESPACE_ELT, mgr);

			foreach (XmlNode n in nl)
			{
				String[] entry = new String[2];

				XmlNodeList aqn = n.SelectNodes(WSDLConstants.ANB + ":" + WSDLConstants.AUTHORITY_ELT, mgr);

				if (aqn.Count < 1)
				{
					throw new LSIDException(LSIDException.INVALID_MESSAGE_FORMAT,"invalid authority list format");
				}
				entry[0] = aqn[0].InnerText;
			
				XmlNodeList nqn = n.SelectNodes(WSDLConstants.ANB + ":" + WSDLConstants.NAMESPACE_ELT, mgr);

				if (nqn.Count < 1)
				{
					throw new LSIDException(LSIDException.INVALID_MESSAGE_FORMAT,"invalid authority list format");
				}
				entry[0] = nqn[0].InnerText;

				res.Add(entry);
			}
		
			String[][] authns = new String[res.Count][];
			return (String[][])res.ToArray(typeof(String[]));
		
		}

	}
}
