using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.IO;

using LSIDClient;

namespace LSIDFramework
{
	/**
 * 
 * Utility class to manage LSID service configuration. The configuration is defined in XML document with 
 * two main elements: services and maps. Each mapping is a collection of authority:namespace pairs that match
 * a subset of LSIDs.  
 * <p>
 * <p>
 * Services are made up of parameters and components which have a type and a body. A service 
 * component "type" refers to how the component is defined.  The most basic type is "class", 
 * whose body contains only the classname of an impl of LSIDService. Other types such as "asdl" and "msdl" load components 
 * from XML in the body, or a referenced location.  A given registry can be configured to load all 
 * component elements or only certain components. 
 * <p>
 * <p>
 * The map of a component specifies the map element that defines which LSIDs are handled by the component. Note that if
 * two components specify the same map AND a ServiceRegistry is configured to load BOTH of these components, it is left
 * unspecified which service an LSID that matches the map will be handled by.  However, it is OK to specify the same map
 * for two components that the same ServiceRegistry will not load. For example &ltauth&gt and &ltmeta&gt components will
 * in general not be loaded by the same registry and two such components that belong to the same service will quite often
 * share the same LSID map. (&ltmeta&gt elements will be loaded by a ServiceRegistry owned by the MetadataServlet, and 
 * &ltauth&gt elements will be loaded by a ServiceRegistry owned by the AuthorityServlet).
 * <p>
 * Below is an exmple document.  Documents which include further features can be found with authority implmentations in the
 * lsid-authorities CVS module.
 * <p>
 * <pre>
 * &ltdeployment-descriptor xmlns="http://www.ibm.com/LSID/Standard/rsdl"&gt
 * 		&ltcomponent-handlers&gt
 *			&lt!-- These handlers (along with "class") are added by default, we include these as examples --&gt
 *			&ltcomponent-handler type="asdl" classname="ASDLComponentHandler" /&gt
 *			&ltcomponent-handler type="msdl" classname="MSDLComponentHandler" /&gt
 *		&lt/component-handlers&gt
 * 		&ltmaps&gt
 *			&ltmap name="all"&gt
 *				&ltpattern auth="*" ns="*" /&gt
 *			&lt/map&gt
 *		&lt/maps&gt
 *		&ltservices&gt
 *			&ltservice name="CachingProxyResolutionService" &gt
 *				&ltparams&gt
 *					&ltparam name="proxyHome"&gt/lsid/java/proxy&lt/param&gt
 *				&lt/params&gt
 *				&ltcomponents&gt
 *					&ltauth map="all" type="class"&gtCachingProxyAuthority&lt/auth&gt
 *					&ltmeta map="all" type="class"&gtCachingProxyAuthority&lt/meta&gt 	
 *					&ltdata map="all" type="class"&gtCachingProxyAuthority&lt/data&gt
 *				&lt/components&gt
 *			&lt/service&gt	
 *		&lt/services&gt
 * &lt/deployment-descriptor&gt
 * 
 * </pre>
 * 
 */
    public class ServiceRegistry : ServiceConfigurationConstants 
    {
        // default service component handlers by type
        private Hashtable defaultHandlers = new Hashtable();

        // default service component handlers by type
        private Hashtable handlers = new Hashtable();

        // authority:namespaces maps by name
        private Hashtable maps = new Hashtable();

        // service configs by service name
        private Hashtable serviceConfigs = new Hashtable();

        // an array of all the service instances
        private ArrayList instances = new ArrayList();

        // the main registry, stores all pattern mappings.
        private Hashtable registry = new Hashtable();


        /**
         * Construct a new service registry that loads only authority services
         * @param File the directory to load from. If null, loads from directory "services" in the current dir.
         * @return ServiceRegistry the registry
         */
        public static ServiceRegistry getAuthorityServiceRegistry(String location) 
        {
            ServiceRegistry reg = new ServiceRegistry(); 
            reg.setComponentHandler(TYPE_ASDL, new ASDLComponentHandler());
            reg.setComponentHandler(TYPE_CACHING, new CachingComponentHandler());
            reg.load(location, new String[] { AUTH_COMP });
            return reg;
        }

        /**
         * Construct a new service registry that loads only meta data services
         * @param File the directory to load from. If null, loads from directory "services" in the current dir.
         * @return ServiceRegistry the registry
         */
        public static ServiceRegistry getMetaDataServiceRegistry(String location) 
        {
            ServiceRegistry reg = new ServiceRegistry();
            reg.setComponentHandler(TYPE_MSDL, new MSDLComponentHandler());
            reg.setComponentHandler(TYPE_CACHING, new CachingComponentHandler());
            reg.load(location, new String[] { META_COMP });
            return reg;
        }

        /**
         * Construct a new service registry that loads only data services
         * @param File the directory to load from. If null, loads from directory "services" in the current dir.
         * @return ServiceRegistry the registry
         */
        public static ServiceRegistry getDataServiceRegistry(String location) 
        {
            ServiceRegistry reg = new ServiceRegistry();
            reg.setComponentHandler(TYPE_CACHING, new CachingComponentHandler());
            reg.load(location, new String[] { DATA_COMP });
            return reg;
        }

        /**
         * Construct a new service registry that loads only authentication services
         * @param File the directory to load from. If null, loads from directory "services" in the current dir.
         * @return ServiceRegistry the registry
         */
        public static ServiceRegistry getAuthenticationServiceRegistry(String location) 
        {
            ServiceRegistry reg = new ServiceRegistry();
            reg.load(location, new String[] { AUTHENTICATION_COMP });
            return reg;
        }

        /**
         * Construct a new service registry that loads only assigning services
         * @param File the directory to load from. If null, loads from directory "services" in the current dir.
         * @return ServiceRegistry the registry
         */
        public static ServiceRegistry getAssigningServiceRegistry(String location) 
        {
            ServiceRegistry reg = new ServiceRegistry();
            reg.load(location, new String[] { ASSIGNING_COMP });
            return reg;
        }

        /**
         * Create a new ServiceRegisty
         */
        public ServiceRegistry() 
        {
            try 
            {
                setComponentHandler(TYPE_CLASS, new LSIDServiceComponentHandler());
            } 
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e, "Error reading service mappings");
            }
        }

        /**
         * query the registry to find out which service implementation should be used to handle the given lsid.
         * @param LSID the lsid to lookup a service for.
         * @return LSIDService the service for the given LSID
         */
        public LSIDService lookupService(LSID lsid) 
        {
            Pattern lookup = new Pattern(lsid.Authority.ToString(), lsid.Namespace);
            LSIDService service = (LSIDService) registry[lookup.ToString()]; // try foo:bar
            if (service != null)
                return service;
            lookup.ns = "*";
            service = (LSIDService) registry[lookup.ToString()]; // try foo:*
            if (service != null) 
            {
                return service;
            }
            lookup.ns = lsid.Namespace;
            lookup.auth = "*";
            service = (LSIDService) registry[lookup.ToString()]; // try *:foo
            if (service != null) 
            {
                return service;
            }
            lookup.ns = "*";
            service = (LSIDService) registry[lookup.ToString()]; // try *:*
            if (service != null) 
            {
                return service;
            }
            return null;
        }

        /**
         * return a vector of all the services.
         */
        public ArrayList getAllServices() 
        {
            return instances;
        }

        /**
         * Load LSID service configuration from all XML files (*.xml) in the given directory. 
         * @param File the directory to load from. If null, loads from directory "services" in the current dir.
         * @param String[] the service components to load.
         */
        public void load(String location, String[] components) 
        {
			lock(this)
			{
				if (location != null) 
				{
					if (!Directory.Exists(location)) 
					{
						throw new LSIDConfigurationException("Directory " + location + " does not exist");
					}
				} 
				else 
				{
					location = "services";
				}
				String[] files = Directory.GetFiles(location);
				for (int i = 0; i < files.Length; i++) 
				{
					if (!Directory.Exists(files[i]) && files[i].ToString().EndsWith(".xml")) 
					{
						try 
						{
							String comps = "";
							for (int j = 0; j < components.Length; j++) 
							{
								comps += components[j] + " ";
							}
							//System.err.println("Loading " + comps + "components from " + files[i]);
							loadSingle(files[i], components);
						} 
						catch (Exception ex) 
						{
							LSIDException.WriteError("LSID config file: " + files[i] + " could not be loaded");
							LSIDException.PrintStackTrace(ex);
						}
					}
				}
			}
        }

        /**
         * Load LSID service configuration from the given XML file
         * @param File the file to load from.
         * @param String[] the service components to load.
         */
        public void loadSingle(String fileName, String[] components) 
        {
			lock(this)
			{
				try 
				{
					handlers.Clear();
					maps.Clear();
					serviceConfigs.Clear();
					System.IO.StreamReader inp = new System.IO.StreamReader(fileName);
					System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
					doc.Load(inp);
					System.Xml.XmlNode root = doc.FirstChild;
					while (!(root.NodeType == System.Xml.XmlNodeType.Element))
						root = root.NextSibling;
					System.Xml.XmlElement rootelt = (System.Xml.XmlElement) root;
					// setup XML namespace 
					XmlElement e = doc.CreateElement("nsmappings");
					// load component handlers
					loadComponentHandlers(rootelt);
					// load maps
					loadMaps(rootelt);
					// load service components
					for (int i = 0; i < components.Length; i++) 
					{
						loadComponents(rootelt, components[i]);
					}
				} 
				catch (IOException e) 
				{
					throw new LSIDConfigurationException(e, "");
				} 
			}
        }

        /**
         * Set a handler that can load a type of service component.
         * @param String the type of service this handler should handle
         * @param ServiceComponentHandler the handler
         */
        private void setComponentHandler(String type, ServiceComponentHandler handler) 
        {
            defaultHandlers.Add(type, handler); 
        }

        /**
         * Load all component handlers
         */
        private void loadComponentHandlers(XmlElement root) 
        {
            try 
            {
                String xpathStr = RSDL_PREFIX + ":" + COMPONENT_HANDLERS_ELT + "/" + RSDL_PREFIX + ":" + COMPONENT_HANDLER_ELT;
                //XPathNodeIterator itr = root.CreateNavigator().SelectDescendants(xpathStr,false);
                
                XmlNamespaceManager mgr = new XmlNamespaceManager(root.OwnerDocument.NameTable);
                mgr.AddNamespace(RSDL_PREFIX, RSDL_NS_URI);

                XmlNodeList nodes = root.SelectNodes(xpathStr, mgr);
                
                    foreach (XmlNode node in nodes)
                    {
                        String typeName = node.Attributes[TYPE_ATTR, ""].Value;
                        String className = node.Attributes[CLASSNAME_ATTR, ""].Value;
                        String asmName = node.Attributes[ASSEMBLY_NAME, ""].Value;

                        String asmFilename = LSIDClient.Global.BinDirectory + "\\" + asmName;
                        
                        ServiceComponentHandler sch = (ServiceComponentHandler)
                            System.Reflection.Assembly.LoadFrom(asmFilename).CreateInstance(className);
                        this.handlers.Add(typeName, sch);
                    }
                
            } 
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e, "Error loading component handlers");
            }
        }

        /**
         * Load all the maps as arrays of patterns and store them in the table
         */
        private void loadMaps(XmlElement root) 
        {
            try 
            {
                String xpathStr = RSDL_PREFIX + ":" + MAPS_ELT + "/" + RSDL_PREFIX + ":" + MAP_ELT;
                XmlNamespaceManager mgr = new XmlNamespaceManager(root.OwnerDocument.NameTable);
                mgr.AddNamespace(RSDL_PREFIX, RSDL_NS_URI);

                XmlNodeList maps = root.SelectNodes(xpathStr, mgr);
                if (maps != null) 
                {
                    for (int i = 0; i < maps.Count; i++) 
                    {
                        XmlElement map = (XmlElement) maps.Item(i);
                        String name = map.GetAttribute(NAME_ATTR);
                        xpathStr = RSDL_PREFIX + ":" + PATTERN_ELT;
                        XmlNodeList patterns = map.SelectNodes(xpathStr, mgr);
                        if (patterns != null) 
                        {
                            Pattern[] pats = new Pattern[patterns.Count];
                            for (int j = 0; j < patterns.Count; j++) 
                            {
                                XmlElement pattern = (XmlElement) patterns.Item(j);
                                pats[j] = new Pattern(pattern.GetAttribute(AUTH_ATTR), pattern.GetAttribute(NS_ATTR));
                            }
                            this.maps.Add(name, pats);
                        }
                    }
                }
            } 
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e, "Error loading maps");
            }
        }

        /**
         * Load all the service components with the given element name
         */
        private void loadComponents(XmlElement root, String component) 
        {
            try 
            {
                String xpathStr = RSDL_PREFIX + ":" + SERVICES_ELT + "/" + RSDL_PREFIX + ":" + SERVICE_ELT + "/" + RSDL_PREFIX + ":" + COMPONENTS_ELT + "/" + RSDL_PREFIX + ":" + component;
                XmlNamespaceManager mgr = new XmlNamespaceManager(root.OwnerDocument.NameTable);
                mgr.AddNamespace(RSDL_PREFIX, RSDL_NS_URI);

                XmlNodeList components = root.SelectNodes(xpathStr, mgr);
                if (components != null) 
                {
                    for (int i = 0; i < components.Count; i++) 
                    {
                        XmlElement comp = (XmlElement) components.Item(i);
                        String mapName = comp.GetAttribute(MAP_ATTR);
                        Pattern[] map = null;
                        if (mapName == null || mapName.Equals("")) 
                        {
                            map = new Pattern[]{new Pattern("*","*")};
                        } 
                        else 
                        {
                            map = (Pattern[]) maps[mapName];
                        }
                        if (map == null) 
                        {
                            throw new LSIDConfigurationException("Bad map link: " + mapName);
                        }
                        String typeName = comp.GetAttribute(TYPE_ATTR);
                        // locate the handler for the component
                        ServiceComponentHandler handler = (ServiceComponentHandler) handlers[typeName];
                        if (handler == null)
                            handler = (ServiceComponentHandler) defaultHandlers[typeName];
                        if (handler == null)
                            throw new LSIDConfigurationException("No handler for service component type " + typeName);
                        if (!handler.knownServices().Contains(component))  
                        {
                            throw new LSIDConfigurationException("Handler not valid for this component (" + component + ") and type (" + typeName + ") combination");
                        }
                        // get the service config for the service that we are part of.
                        LSIDServiceConfig config = getServiceConfig(comp);
                        // initialize the service
                        LSIDService service = handler.loadComponent(comp, config);
                        if (service == null)
                            throw new LSIDConfigurationException("Handler could not load service for comp " + typeName);
                        for (int j = 0; j < map.Length; j++) 
                        {
                            registry.Add(map[j].ToString(), service); 
                        }
                        instances.Add(service);
                    }
                }
            } 
            catch (Exception e) 
            {
				LSIDClient.LSIDLog.LogMessage(e.Message + ":" + e.StackTrace);
                throw new LSIDConfigurationException(e, "Error loading " + component + " components");
            }
        }

        /**
         * Get the service config from the service that the given component is part of
         */
        private LSIDServiceConfig getServiceConfig(XmlElement comp) 
        {
            try 
            {
                XmlElement service = (XmlElement) comp.ParentNode.ParentNode;
                String serviceName = service.Attributes[NAME_ATTR].Value;
                LSIDServiceConfigImpl config = (LSIDServiceConfigImpl) serviceConfigs[serviceName];
                if (config != null)
                    return config;
                config = new LSIDServiceConfigImpl();
                String xpathStr = RSDL_PREFIX + ":" + PARAMS_ELT + "/" + RSDL_PREFIX + ":" + PARAM_ELT;
                XmlNamespaceManager mgr = new XmlNamespaceManager(comp.OwnerDocument.NameTable);
                mgr.AddNamespace(RSDL_PREFIX, RSDL_NS_URI);
                XmlNodeList ps = service.SelectNodes(xpathStr, mgr);

                if (ps != null) 
                {
                    for (int i = 0; i < ps.Count; i++) 
                    {
                        XmlElement param = (XmlElement) ps.Item(i);
                        String name = param.Attributes[NAME_ATTR].Value;
                        xpathStr = "child::text()";
                        String value = param.SelectSingleNode(xpathStr, mgr).Value; 
                        config.setProperty(name, value);
                    }
                }
                serviceConfigs[serviceName] = config;
                return config;
            } 
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e, "Error loading service config, init params");
            }
        }

        /**
         * simple struct for patterns
         * <p>
         * <p>
         */
        internal class Pattern 
        {

            public Pattern(String auth, String ns) 
            {
                this.auth = auth.ToLower();
                this.ns = ns.ToLower();
            }

            public String auth;
            public String ns;

            public override String ToString() 
            {
                return auth + ":" + ns;
            }

        }

        /**
         * Simple simplementation of lsid service configuration
         */
        internal  class LSIDServiceConfigImpl : LSIDServiceConfig 
        {
            private Hashtable props = new Hashtable();

            /**
             * @see LSIDServiceConfig#getPropertyNames()
             */
            public new ICollection getPropertyNames() 
            {
                return props.Keys;
            }

            /**
             * @see LSIDServiceConfig#getProperty(String)
             */
            public new String getProperty(String property) 
            {
                return (String) props[property];
            }

            /**
             * Sets a property on this config
             */
            public void setProperty(String property, String value) 
            {
                props[property] = value;
            }

        }

        //	public static void main(String[] args) 
		//  {
        //		// PDB
        //		LSID lsid1 = new LSID("URN:LSID:pdb.org:PDB:1AFT");
        //		LSID lsid2 = new LSID("URN:LSID:pdb.org:PDB:1AFT-MMCIF");
        //		LSID lsid3 = new LSID("URN:LSID:pdb.org:PDB:1AFT-PDB");
        //		LSID lsid4 = new LSID("URN:LSID:pdb.org:PDB:1AFT-FASTA");
        //		
        //		// NCBI
        //		LSID lsid5 = new LSID("urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:genbank_gi:30350027");
        //		LSID lsid6 = new LSID("urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:genbank:bm872070");
        //		LSID lsid7 = new LSID("urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:pubmed:12441807");
        //		
        //		// Ensembl
        //		LSID lsid8 = new LSID("urn:lsid:ensembl.org.lsid.i3c.org:homosapiens_gene:ensg00000002016");
        //		LSID lsid9 = new LSID("urn:lsid:ensembl.org.lsid.i3c.org:homosapiens_gene:ensg00000002016-fasta");
        //		LSID lsid10 = new LSID("urn:lsid:ensembl.org.lsid.i3c.org:homosapiens_exon:ense00001160197");
        //		LSID lsid11 = new LSID("urn:lsid:ensembl.org.lsid.i3c.org:homosapiens_exon:ense00001160197-fasta");
        //		
        //		// UCSC
        //		LSID lsid12 = new LSID("urn:lsid:genome.ucsc.edu.lsid.i3c.org:hg13:chr2_1-1000");
        //		
        //		// Locus link
        //		LSID lsid13 = new LSID("urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:locuslink:9594");
        //		
        //		// IBM internal
        //		LSID lsid23 = new LSID("urn:lsid:pdb2.ls.adtech.internet.ibm.com:proteins:1aft");
        //		LSID lsid24 = new LSID("urn:lsid:pdb2.ls.adtech.internet.ibm.com:data:1afv-mmcif");
        //		LSID lsid25 = new LSID("urn:lsid:perl.ls.adtech.internet.ibm.com:swiss-id:hv20_mouse-sprot");
        //		LSID lsid26 = new LSID("urn:lsid:perl.ls.adtech.internet.ibm.com:swiss-id:hv20_mouse");
        //		LSID lsid27 = new LSID("urn:lsid:perl.ls.adtech.internet.ibm.com:swiss-ac:p35980-fasta");
        //		
        //		LSID lsid28 = new LSID("urn:lsid:lsid.gateway.ibm.com:pdb:1AFT-MMCIF");
        //		ServiceRegistry reg = getMetaDataServiceRegistry(new File("C:/wsad5/workspace/LSIDServerWeb/Web Content/services"));
        //		
        //		System.out.println(reg.lookupService(lsid8).getClass().getName());
        //	}

    }

}
