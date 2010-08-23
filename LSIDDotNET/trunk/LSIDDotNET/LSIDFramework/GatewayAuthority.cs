using System;
using System.Xml;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

using LSIDClient;

namespace LSIDFramework
{
/**
 *
 * The Gateway Authority is an authority that defines a set of LSIDs that map to a datasource that is not LSID
 * enabled.  These mappings are defined in an Authority Service Description Language (ASDL) document.
 * 
 * <p>
 * Example
 * <p>
 * 
 * <pre>
 * &ltlsid-authority name="LifeSciencesGateway" xmlns="http://www.ibm.com/LSID/Standard/ASDL"&gt
 *	&ltservices&gt
 *		&ltservice name="proxy-data"&gt
 *			&ltprotocol&gthttp&lt/protocol&gt
 *			&lthostname&gtlocalhost&lt/hostname&gt
 *			&ltpath &gt/LSID/proxy/dataService&lt/path&gt
 *			&ltport&gt9080&lt/port&gt
 *		&lt/service&gt
 *		&ltservice name="pdb-soap-data"&gt
 *			&ltsoap-endpoint&gthttp://localhost:9080/LSID/proxy/dataService&lt/soap-endpoint&gt
 *		&lt/service&gt
 *		&ltservice name="proxy-meta-data"&gt
 *			&ltsoap-endpoint insert-lsid="path"&gthttp://localhost:9080/LSID/proxy/metaDataService&lt/soap-endpoint&gt
 *		&lt/service&gt
 *		&ltservice name="proxy-meta-data-http"&gt
 *			&ltprotocol&gthttp&lt/protocol&gt
 *			&lthostname&gtlocalhost&lt/hostname&gt
 *			&ltpath &gt/LSID/proxy/metaDataService/data&lt/path&gt
 *			&ltport&gt9080&lt/port&gt
 *		&lt/service&gt
 *	&lt/services&gt
 *	&ltlsids&gt
 *		&ltlsid urn="URN:LSID:pdb.org:PDB:1AFT-MMCIF:"&gt
 *			&ltdata-services&gt
 *				&ltservice name="ftp-pdb"&gt
 *					&ltprotocol&gtftp&lt/protocol&gt
 *					&lthostname&gtbeta.rcsb.org&lt/hostname&gt
 *					&ltpath&gt/pub/pdb/uniformity/data/mmCIF/all/1aft.cif.Z&lt/path&gt
 *				&lt/service&gt
 *				&ltservice-ptr name="proxy-data"/&gt
 *				&ltservice-ptr name="pdb-soap-data"/&gt
 *			&lt/data-services&gt
 *			&ltmeta-data-services&gt
 *				&ltservice-ptr name="proxy-meta-data"/&gt
 *			&lt/meta-data-services&gt
 *		&lt/lsid&gt
 *		&ltlsid urn="URN:LSID:pdb.org:PDB:1AFT-PDB:"&gt
 *			&ltdata-services&gt
 *				&ltservice-ptr name="proxy-data"/&gt
 *			&lt/data-services&gt
 *			&ltmeta-data-services&gt
 *				&ltservice-ptr name="proxy-meta-data"/&gt
 *				&ltservice-ptr name="proxy-meta-data-http"/&gt
 *			&lt/meta-data-services&gt
 *		&lt/lsid&gt
 *	&lt/lsids&gt
 * &lt/lsid-authority&gt
 * </pre>
 * 				
 * The location of such a file should be stored in the LSID service config property "asdl-file" or "asdl-uri".
 * (For example, the Http Servlet Config Properties if this authority is deployed against the AuthorityServlet) 
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 * 
 */
	public class GatewayAuthority : SimpleAuthority 
	{
        	
		/**
			 * The LSIDServiceConfig property that specifies the URI where the ASDL document is located
			 */
		public static String ASDL_URI = "asdl-uri";
        	
		/**
			 * The LSIDServiceConfig property that specifies the File where the ASDL document is located
			 */
		public static String ASDL_FILE = "asdl-file";
        	
		/**
			 * The LSIDServiceConfig property that specifies the DOM element where the ASDL document is located
			 */
		public static String ASDL_ELEMENT = "asdl-element";
        	
		/**
			 * The LSIDServiceConfig property that specifies the String where the ASDL document is located
			 */
		public static String ASDL_STRING = "asdl-string";
        	
		/**
			 * The value that indicates the TCP/IP default port
			 */
		private static int DEFAULT_PORT = -1;
        	
		public static String NAMESPACE = "http://www.ibm.com/LSID/Standard/ASDL";
		public static String PREFIX = "asdl";
		public static String ROOT = "lsid-authority";
        	
		private static String SERVICE = "service";
		private static String SERVICES = "services";
		private static String SERVICE_PTR = "service-ptr";
		private static String DATA_SERVICES = "data-services";
		private static String META_DATA_SERVICES = "meta-data-services";
		private static String NAME_ATTR = "name";
		private static String PROTOCOL = "protocol";
		private static String HOSTNAME = "hostname";
		private static String PATH = "path";
		private static String TYPE_ATTR = "type";
		private static String PORT = "port";
		private static String INSERT_LSID_ATTR = "insert-lsid";
		private static String SOAP_ENDPOINT = "soap-endpoint";
		private static String LSID_ELT = "lsid";
		private static String LSIDS = "lsids";
		private static String URN_ATTR = "urn";
		private static String SOAP = "soap";
		private static String HTTP = "http";
		private static String FTP = "ftp";
		private static String LOCALHOST = "localhost";
        	
		private static String MASK = "*";
        	
		// namespace node
		private XmlElement envNS;
        	
		private static String gatewayName = null;
		private LSID[] lsids;
		private Hashtable lsidEntries = new Hashtable();
		private static Hashtable serviceEntries = new Hashtable();
        	
        	
		public static void main(String[] args) 
		{        		
//			GatewayAuthority ga = new GatewayAuthority();
//			ga.loadByFile("c:/wsad5/workspace/NCBIAuthority/com/ibm/lsid/server/impl/ncbi/mdasdl.xml");
//			LSIDRequestContext ctx = new LSIDRequestContext();
//			ctx.Lsid = new LSID("urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:types:mrna");
//			ctx.ReqUrl = "http://3.56.78.9:1000/foo/bar";
//			String str = ga.getAvailableServices(ctx).getValue().ToString();
			//	;
			//System.out.println(str);
		}
        	 
        	
		/**
			 * Construct a new authority.  The authority will be empty until load() is called. 
			 */
		public GatewayAuthority() 
		{
		}
        	
		/**
			 * Load the authority ASDL from the given URI
			 * @param URL the URI of the ASDL
			 */
		public void loadByURL(String uri) 
		{
			try 
			{
				Uri u = new Uri(uri);
				WebRequest r = WebRequest.Create(u);
				if (HTTPUtils.WebProxy != null) r.Proxy = new WebProxy(HTTPUtils.WebProxy);
				load(r.GetResponse().GetResponseStream());
			} 
			catch (IOException e) 
			{
				throw new LSIDServerException(e, "Error getting Gateway Authority ASDL at: " + uri);
			}
		}
        	
		/**
			 * Load the authority ASDL from the given file location
			 * @param File the file that contains the ASDL
			 */
		public void loadByFile(String file) 
		{
			try 
			{
				load(System.IO.File.OpenRead(file));
			} 
			catch (FileNotFoundException e) 
			{
				throw new LSIDServerException(e, "Gateway Authority ASDL file not found: " + file);
			}
		}
        	
		/**
			 * Load the authority from the given XML String
			 * @param String the XML String that contains the ASDL
			 */
		public void loadByXml(String xml) 
		{                
			load(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(xml)));
		}
        	
		/**
			 * Load the authority from the given InputStream, close it on exit.
			 * @param InputStream the input stream of XML that contains the ASDL
			 */
		public void load(Stream xml) 
		{
			//DOMParser parser = new DOMParser();
			try 
			{
				StreamReader rdr = new StreamReader(xml);
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(rdr.ReadToEnd());
				//parser.parse(new InputSource(xml));
				XmlNode root = doc.FirstChild;
				while (!(root is XmlElement))
					root = root.NextSibling;
				load((XmlElement)root);
			} 
			catch (IOException e) 
			{
				throw new LSIDServerException(e, "Error parsing Gateway Authority ASDL");
			} 
			finally 
			{
				try 
				{
					if (xml != null)
						xml.Close();
				} 
				catch (IOException e)
				{
					LSIDException.PrintStackTrace(e);
				}	
			}
		}
        	
		/**
			 * Load the authority ASDL from the given DOM element
			 * @param Element the DOM that contains the ASDL
			 */
		public void load(XmlElement ASDL) 
		{
			envNS = ASDL.OwnerDocument.CreateElement("nsmappings", "http://www.w3.org/2000/xmlns/");
			gatewayName = ASDL.GetAttribute(NAME_ATTR);		
			// load the service entries
			String xpathStr = PREFIX + ":" + SERVICES + "/" + PREFIX + ":" + SERVICE;
			XmlNamespaceManager mgr = new XmlNamespaceManager(ASDL.OwnerDocument.NameTable);
			mgr.AddNamespace(PREFIX, NAMESPACE);

			try 
			{
				XmlNodeList services = ASDL.SelectNodes(xpathStr, mgr);
				if (services != null) 
				{
					for (int i=0;i<services.Count;i++) 
					{
						XmlElement elt = (XmlElement)services.Item(i);
						ServiceEntry service = new ServiceEntry(elt);
						serviceEntries.Add(service.name,service);
					}		
				}
			} 
			catch (Exception e) 
			{
				throw new LSIDServerException(e, "Error loading services from Gateway Authority ASDL");
			}
        		
			// load the lsids
			xpathStr = PREFIX + ":" + LSIDS + "/" + PREFIX + ":" + LSID_ELT;
			try 
			{
				XmlNodeList lsids = ASDL.SelectNodes(xpathStr, mgr);
				if (lsids != null) 
				{
					this.lsids = new LSID[lsids.Count]; 
					for (int i=0;i<lsids.Count;i++) 
					{
						XmlElement elt = (XmlElement)lsids.Item(i);
						LSIDEntry lsid = new LSIDEntry(elt);
						this.lsids[i] = lsid.lsid;
						lsidEntries.Add(lsid.lsid.ToString(),lsid);
					}		
				}
			} 
			catch (Exception e) 
			{
				throw new LSIDServerException(e, "Error loading meta data services from Gateway Authority ASDL");
			}
        				
		}
        
		/**
			 * Load the Gateway Authority configuration. <code>LSIDServerConfig</code> properties are checked
			 * for the location of the ASDL in the following order<br>
			 * <code>CONFIG_URI</code> The url String where the doc is located<br>
			 * <code>CONFIG_FILE</code> The file string on the local system where the doc is located<br>
			 * <code>CONFIG_ELEMENT</code> The DOM element that contains the doc<br>
			 * <code>CONFIG_STRING</code> The XML string that contains the doc
			 * 
			 * @see LSIDService#initService(LSIDServiceConfig)
			 */
		public new void initService(LSIDServiceConfig config)
		{
			String asdlUri = config.getProperty(ASDL_URI);
			String asdlFile = config.getProperty(ASDL_FILE);
			String asdlElement = config.getProperty(ASDL_ELEMENT);
			String asdlString = config.getProperty(ASDL_STRING);
			if (asdlUri != null) 
			{
				try 
				{
					loadByURL(asdlUri);
				} 
				catch (Exception e) 
				{
					throw new LSIDServerException(e, "Bad location for XML authority ASDL: " + asdlUri);	
				}
			} 
			else if (asdlFile != null) 
			{
				loadByFile(asdlFile);
			} 
			else if (asdlElement != null) 
			{
				loadByXml(asdlElement);
			} 
			else if (asdlString != null) 
			{
				loadByXml(asdlString);
			}
		}
        	
		protected override LSIDMetadataPort[] getMetadataLocations(LSID lsid, String url) 
		{
        		
			LSIDEntry entry = (LSIDEntry) lsidEntries[lsid.ToString()];		
        		
			if (entry == null) 
			{
				String maskedUrn = "urn:lsid:" + lsid.Authority.ToString() + ":" + lsid.Namespace + ":" + MASK;
				entry = (LSIDEntry) lsidEntries[maskedUrn];	
				if (entry == null)
				{
					return new LSIDMetadataPort[0];
				}
			}		
        		
			ServiceEntry[] entries = entry.metaDataServices;
			ServiceEntry[] retEntries = new ServiceEntry[entries.Length];
			if (entries == null)
			{
				return new LSIDMetadataPort[0];
			}
        		
			try 
			{
				Uri r = new Uri(url);
				// check to see if any entries had "localhost" in them.  If so,
				// change the host and port to that of the inbound request.
				// also must create a new entry for any entries with a "*".  This new entry will have the actual
				// LSID in it.
				for (int i=0;i<entries.Length;i++) 
				{
					if (entries[i].lsid.Object.Equals("*"))
						retEntries[i] = new ServiceEntry(entries[i],lsid);
					else
						retEntries[i] = entries[i];
					if (retEntries[i].getProtocol().Equals(SOAP)) 
					{
						if (retEntries[i].getLocation().IndexOf(LOCALHOST) != -1) 
						{
							Uri u = new Uri(retEntries[i].getLocation());
							//URL u = new URL(retEntries[i].getLocation());
							Uri l = new Uri("http:" + r.Port + "//" + r.Host + "/" + u.AbsolutePath);
							//URL l = new URL(HTTP,r.getHost(),r.getPort(),u.getPath());
							String loc = retEntries[i].path.location;
							retEntries[i].path = new LSIDPath(l.ToString());
							retEntries[i].path.location = loc;
						}
					} 
					else if (retEntries[i].getProtocol().Equals(HTTP)) 
					{
						if (retEntries[i].hostname.Equals(LOCALHOST)) 
						{
							retEntries[i].hostname = r.Host;
							retEntries[i].port = r.Port;
						}
					}			
				}
			} 
			catch (Exception e) 
			{
				LSIDException.PrintStackTrace(e);
				return new LSIDMetadataPort[0];
			}
        		
			return retEntries;
		}
        	
		protected override LSIDDataPort[] getDataLocations(LSID lsid, String url) 
		{
			LSIDEntry entry = (LSIDEntry) lsidEntries[lsid.ToString()];
			if (entry == null)
			{
				return new LSIDDataPort[0];
			}
			ServiceEntry[] entries = entry.dataServices;
			ServiceEntry[] retEntries = new ServiceEntry[entries.Length];
			if (entries == null)
			{
				return new LSIDDataPort[0];
			}
			try 
			{
				Uri r = new Uri(url);
				//URL r = new URL(url);
				// check to see if any entries had "localhost" in them.  If so,
				// change the host and port to that of the inbound request.
				// also must create a new entry for any entries with a "*".  This new entry will have the actual
				// LSID in it.
				for (int i=0;i<entries.Length;i++) 
				{
					if (entries[i].lsid.Object.Equals("*"))
						retEntries[i] = new ServiceEntry(entries[i],lsid);
					else
						retEntries[i] = entries[i];
					if (retEntries[i].getProtocol().Equals(SOAP)) 
					{
						if (retEntries[i].getLocation().IndexOf(LOCALHOST) != -1) 
						{
							Uri u = new Uri(retEntries[i].getLocation());
							Uri l = new Uri("http:" + r.Port + "//" + r.Host + "/" + u.AbsolutePath);
							String loc = retEntries[i].path.location;
							retEntries[i].path = new LSIDPath(l.ToString());
							retEntries[i].path.location = loc;
						}
					} 
					else if (retEntries[i].getProtocol().Equals(HTTP)) 
					{
						if (retEntries[i].hostname.Equals(LOCALHOST)) 
						{
							retEntries[i].hostname = r.Host;
							retEntries[i].port = r.Port;
						}
					}				
				}
			} 
			catch (Exception e) 
			{
				LSIDException.PrintStackTrace(e);
				return new LSIDDataPort[0];
			}
			return entries;
		}
        	
		/**
			 * private data structure to maintain the lsids that are registered with this authority
			 */
		internal class LSIDEntry 
		{
			public LSID lsid;
			public ServiceEntry[] dataServices;
			public ServiceEntry[] metaDataServices;
        		
			public LSIDEntry(XmlElement elt) 
			{
				String urn = elt.GetAttribute(URN_ATTR);
				try 
				{
					lsid = new LSID(urn);
				} 
				catch (MalformedLSIDException e) 
				{
					throw new LSIDServerException(e, "Bad LSID in Gateway Authority ASDL.");
				}			
        	
				dataServices = loadServices(elt,DATA_SERVICES);
				metaDataServices = loadServices(elt,META_DATA_SERVICES);
			}
        		
			private ServiceEntry[] loadServices(XmlElement elt, String tagName)
			{
				try 
				{
					String xpathServices = PREFIX + ":" + tagName + "/" + PREFIX + ":" + SERVICE;
					String xpathPtrs = PREFIX + ":" + tagName + "/" + PREFIX + ":" + SERVICE_PTR;
					XmlNamespaceManager mgr = new XmlNamespaceManager(elt.OwnerDocument.NameTable);
					mgr.AddNamespace(PREFIX, NAMESPACE);

					int totalNodes = 0;
					int numServiceNodes = 0;
					XmlNodeList serviceNodes = elt.SelectNodes(xpathServices,mgr);
					XmlNodeList ptrNodes = elt.SelectNodes(xpathPtrs,mgr);
					if (serviceNodes != null) 
					{
						numServiceNodes = serviceNodes.Count;
						totalNodes += numServiceNodes;
					}
					if (ptrNodes != null) 
					{
						totalNodes += ptrNodes.Count;
					}
					ServiceEntry[] services = new ServiceEntry[totalNodes];
					if (serviceNodes != null) 
					{
						for (int i=0;i<serviceNodes.Count;i++) 
						{
							XmlElement serviceelt = (XmlElement)serviceNodes.Item(i);
							services[i] = new ServiceEntry(serviceelt);
							services[i].lsid = lsid;
						}		
					}
					if (ptrNodes != null) 
					{
						for (int i=0;i<ptrNodes.Count;i++) 
						{
							XmlElement ptrelt = (XmlElement)ptrNodes.Item(i);
							String ptr = ptrelt.GetAttribute(NAME_ATTR);
							if (ptr == null)
								throw new LSIDServerException("'service-ptr' element must have 'name' attribute");
							ServiceEntry target = (ServiceEntry)serviceEntries[ptr];
							if (target == null)
								throw new LSIDServerException("Bad data service name link in ASDL: " + ptr);
							services[numServiceNodes + i] = new ServiceEntry(target,lsid);	
						}
					}
					return services;
				} 
				catch (Exception e) 
				{
					throw new LSIDServerException(e, "Error loading " + tagName + " from Gateway Authority ASDL");
				}
        			
			}
		}
        	
		/**
			 * private data structure to maintain data services.
			 */
		internal class ServiceEntry : LSIDDataPort, LSIDMetadataPort 
		{
			public LSID lsid;
			public String name;
			public String protocol;
			public String hostname;
			public LSIDPath path;
			public int port = DEFAULT_PORT;
			private LSIDCredentials lsidCredentials = null;	
			private Hashtable headers = new Hashtable();
			/**
				 * Method addProtocalHeader.
				 * @param name
				 * @param value
				 */
			public void addProtocolHeader(String name, String value)
			{
				headers.Add(name,value);
			}
        		
			/**
				 * Method getProtocalHeaders.
				 * @return Map
				 */
			public Hashtable getProtocolHeaders() 
			{
				return headers;
			}		
			/**
				 * Returns the lsidCredentials.
				 * @return LSIDCredentials
				 */
			public LSIDCredentials getLsidCredentials() 
			{
				return lsidCredentials;
			}
        	
			/**
				 * Sets the lsidCredentials.
				 * @param lsidCredentials The lsidCredentials to set
				 */
			public void setLsidCredentials(LSIDCredentials lsidCredentials) 
			{
				this.lsidCredentials = lsidCredentials;
			}
        			
			/**
				 * Create a new entry with values from another entry, and attach it to an LSID
				 */
			public ServiceEntry(ServiceEntry entry, LSID lsid) 
			{
				this.hostname = entry.hostname;
				this.path = entry.path;
				this.protocol = entry.protocol;
				this.port = entry.port;
				this.lsid = lsid;
				this.name = entry.name;
			}
        		
			/**
				 * Parse a new service entry from xml
				 */
			public ServiceEntry(XmlElement elt) 
			{
				XmlNamespaceManager mgr = new XmlNamespaceManager(elt.OwnerDocument.NameTable);
				mgr.AddNamespace(PREFIX, NAMESPACE);
				name = elt.GetAttribute(NAME_ATTR);
				XmlElement soapNode = (XmlElement)elt.SelectSingleNode(PREFIX + ":" + SOAP_ENDPOINT ,mgr);
				if (soapNode != null) 
				{
					this.path = new LSIDPath(soapNode);
					this.protocol = SOAP;
				} 
				else 
				{
					XmlNode protocolNode = elt.SelectSingleNode(PREFIX + ":" + PROTOCOL + "/text()",mgr);
					if (protocolNode != null)
						protocol = protocolNode.Value;
        				
					XmlNode hostnameNode = elt.SelectSingleNode(PREFIX + ":" + HOSTNAME + "/text()",mgr);
					if (hostnameNode != null)
						hostname = hostnameNode.Value;
        				 
					XmlElement pathNode = (XmlElement)elt.SelectSingleNode(PREFIX + ":" + PATH,mgr);
					if (pathNode != null) 
					{
						LSIDPath path = new LSIDPath(pathNode);
						this.path = path;
					}
					XmlNode portNode = elt.SelectSingleNode(PREFIX + ":" + PORT + "/text()",mgr);
					if (portNode != null)
						port = int.Parse(portNode.Value);
				}	
			}
        		
			/**
				 * @see LSIDDataPort#getName()
				 * @see LSIDMetadataPort#getName()
				 * @return String 
				 */
			public String getName() 
			{
				return name;
			}
        		
			/**
				 * return the name of the gateway that owns us.
				 * @see LSIDDataPort#getServiceName()
				 * @see LSIDMetadataPort#getServiceName()
				 */
			public String getServiceName() 
			{
				return gatewayName;
			}		
        
			/**
				 * @see LSIDDataPort#getLocation()
				 */
			public String getLocation() 
			{
				if (protocol.Equals(HTTP)) 
				{
					if (port < 0)
						return "http://" + hostname + "/" + path.getPath(lsid);
					else
						return "http://" + hostname + ":" + port + path.getPath(lsid);
				} 
				else if (protocol.Equals(FTP))
					return hostname;
				else if (protocol.Equals(SOAP))
					return path.getPath(lsid);
				else
					return null;
			}
        
			/**
				 * @see LSIDDataPort#getPath()
				 */
			public String getPath() 
			{
				if (protocol.Equals(FTP))
					return path.getPath(lsid);
				if (protocol.Equals(HTTP))
					return path.type;
				return null;
			}
        
			/**
				 * @see LSIDDataPort#getProtocol()
				 */
			public String getProtocol() 
			{
				return protocol;
			}
        
		}
        		
		/**
			 * private data structure to represent a path or url that might have the lsid inserted into it somewhere
			 */
		internal class LSIDPath 
		{
        		
			static String IN_QUERY = "query";
			static String IN_PATH = "path";
        	
			public String path;
			public String location;
			public String type = LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED; // for HTTP only
        		
			public LSIDPath(XmlElement elt) 
			{
				String location = elt.GetAttribute(INSERT_LSID_ATTR);
				String t = elt.GetAttribute(TYPE_ATTR);
				if (t != null && !t.Equals(""))
					type = t;
				this.location = location;
				this.path = elt.SelectSingleNode("text()").Value;
			}
        		
			public LSIDPath(String path) 
			{
				this.path = path;	
			}
        		
			// we should make this more complicated...can add special tokens in the path that get replaced with elements
			// from the LSID for example /pub/ftp/proteins/%object%.mcif
			public String getPath(LSID lsid) 
			{
				if (location != null) 
				{
					if (location.Equals(IN_QUERY)) 
					{
						if (path.IndexOf('?') != -1) // if a query string already exists, append this to the end.
							return path + "&lsid=" + lsid.ToString();
						else
							return path + "?lsid=" + lsid.ToString();
					}
					else if (location.Equals(IN_PATH)) 
					{
						if (path.EndsWith("/"))
							return path + lsid.ToString();
						else
							return path + "/" + lsid.ToString();
					} 
					else
						return path;						
				} 
				else 
				{
					return path;
				}	
			}
        		
		}
        
		/* 
		* @see LSIDAuthorityService#notifyForeignAuthority(String, String)
		*/
		public new void notifyForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
		{
			throw new LSIDServerException(LSIDServerException.METHOD_NOT_IMPLEMENTED,"Not Implemented in Gateway");
        		
		}
        
        
		/* 
		* @see LSIDAuthorityService#revokeNotifcationForeignAuthority(String, String)
		*/
		public new void revokeNotificationForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
		{
			throw new LSIDServerException(LSIDServerException.METHOD_NOT_IMPLEMENTED,"Not Implemented in Gateway");		
		}
	}
}
