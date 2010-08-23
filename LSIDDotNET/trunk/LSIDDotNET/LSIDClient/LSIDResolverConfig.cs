using System;
using System.Collections;
using System.Web.Services.Description;
using System.IO;
using System.Xml;


namespace LSIDClient
{
	
	/**	  
	Represents the System LSID Resolver config. The configuration is loaded from the file "lsid-client.xml" which
	must be located in the file specified by the app.config or web.config property "LSID_CLIENT_HOME". 
	 
	<p>The file should be an XML file of the following format: <br>
	 
	<pre>
	<lsidClient xmlns="http://www.ibm.com/LSID/Standard/ClientConfiguration">
	
	<!-- caching configuration -->
	<caching>
		<!-- whether or not to perform caching, default false. -->
		<useCache>true</useCache>
		<!-- The location of the cache dir. By default, the cache
             is placed in LSID_CLIENT_HOME/cache. If use-cache == false, this
			 argument will be ignored. LSID_CLENT_HOME location is defined in the app.config or web.config file for the active application-->
		<lsidCacheDir>/mycachedir</lsidCacheDir>
	</caching>
	
	<!-- authority mappings which short-cut the DNS lookup for an authority name.
		 If no mappings are present, all authority names will be resolved by DNS.
		 -->
	<hostMappings>
		<hostMapping>
			<authority>lsidsample.org</authority>
			<endpoint>http://localhost/authority/</endpoint>
		</hostMapping>
	</hostMappings>
	
	<!-- foreign authorities are additional authorities that should be asked about a given LSID or set of LSIDs in addition
	     to the actual authority of those LSIDs -->
	<foreignAuthorities>
		<!-- foreign authorities may be specified for a set of LSIDs with given authorities or namespace.  The auth and ns
		     attributes may each be simple wild cards (*) -->
		<pattern auth="myauthorityname" ns="ns">
			<authority>myotherauthority</authority>
			<authority>myotherauthority</authority>
		</pattern>
		<!-- foreign authorities may be specified for a given LSID.  -->
		<lsid lsid="urn:lsid:myauthority:ns:myobj">
			<authority>myotherauthority</authority>
			<authority>myotherauthority</authority>
		</lsid>
		<pattern auth="myauthorityname" ns="ns">
			<authority>myotherauthority</authority>
		</pattern>
		<!-- foreign authorities may be specified for a given LSID.  -->
		<lsid lsid="urn:lsid:myauthority:ns:myobj">
			<authority>myotherauthority</authority>
		</lsid>
	</foreignAuthorities>
	
	<!-- accepted metadata formats, application/xml+rdf is the default, but we include it as an example
	any formats listed here override the default format -->
	<acceptedFormats>
		<format>application/xml+rdf</format>
	</acceptedFormats>
	
	
	<!-- WSDL extensions, used for handling custom port types and imports -->
	<wsdlExtensions>
		<importMap>
			<location>http://www.ibm.com/WSDL/newports.wsdl</location>
			<resource>/com/ibm/lsid/NewPortType.wsdl</resource>
		</importMap>
		<portMap>
			<portType>http://www.ibm.com/WSDL:NewPortType</portType>
			<portFactory classname="LSIDFramework.NewPortFactory" assemblyname="LSIDFramework.dll" />
		</portMap>
	</wsdlExtensions>

	<webSettings>
		<!--proxy>proxy.org.com:8080</proxy--> 
	</webSettings>
	</lsidClient>
	</pre>
	 
	If no cache file is present, default values are used, and LSID_CLIENT_HOME is assumed to be
	"/lsid-client".	
	*/
    public class LSIDResolverConfig 
    {

        public static String LSID_CLIENT_HOME = "LSID_CLIENT_HOME";

        private static String CONFIG_FILE_NAME = "lsid-client.xml";
        private static String LSID_CLIENT_HOME_DEFAULT = "lsid-client";

        private static Boolean USE_CACHE_DEFAULT = false;
        private static String LSID_CACHE_DIR_DEFAULT = "cache"; // relative to client home 
	
        private static Hashtable DEFAULT_METADATA_FACTORIES = new Hashtable();
			
        private static String[] DEFAULT_METADATA_FORMATS = new String[] {MetadataResponse.RDF_FORMAT};
        	

        private string lsidHome = null;
        private Boolean useCache = false;
        private String lsidCacheDir = null;
        private LSIDCache cache = null;
        private string[] acceptedFormats = null;
        private Hashtable hostMappings = new Hashtable();
        private Hashtable metadataFactories = new Hashtable();
        private Hashtable foreignAuthorities = new Hashtable();
        private Hashtable portFactories = new Hashtable();
        private Hashtable importMaps = new Hashtable();
        private WebSettings webSettings = new WebSettings();

        /**
         * Create and load a new configuration.  Note, the load stack swallows all exceptions.  However, important error
         * messages are printed to standard error, default configuration is used as a way to recover from most
         * exceptions during configuration.
         */

        static LSIDResolverConfig()
        {
            DEFAULT_METADATA_FACTORIES.Add(MetadataResponse.RDF_FORMAT,new XSLTMetadataFactory());
        }

        public LSIDResolverConfig() 
        {
            String lsidHomeStr = System.Configuration.ConfigurationSettings.AppSettings[LSID_CLIENT_HOME];
            if (lsidHomeStr == null) 
            {
                lsidHomeStr = Global.BinDirectory + "\\" + LSID_CLIENT_HOME_DEFAULT;
            }
            lsidHome = lsidHomeStr;
            if (!Directory.Exists(lsidHome))
                Directory.CreateDirectory(lsidHome);

            String config = lsidHome + "\\" + CONFIG_FILE_NAME;
            if (File.Exists(config)) 
            {
                try 
                {
                    LSIDClient conf = new LSIDClient();
                    conf.ReadConfig(config);
                    loadConfig(conf);
                } 
                catch (Exception e) 
				{
					string msg = e.Message + " : " + e.Source + " : " + e.StackTrace;
					LSIDException.WriteError(msg);

                    loadConfig(null);
                    writeDefaultConfig();
                } 
            } 
            else 
            {
                loadConfig(null);
                writeDefaultConfig();
            }
        }

        /**
         * Returns the cache instance.
         * @return LSIDCache the cache instance
         */
        public LSIDCache getCache() 
        {
            return cache;
        }

        /**
         * Returns the hostMappings.
         * @return Hashtable the mappings
         */
        public Hashtable getHostMappings() 
        {
            return hostMappings;
        }

        /**
         * Looks up the array of foreign authorities configure for a given LSID.
         * @param LSID the lsid to lookup
         * @return LSIDAuthority[] the set of authorities.  A null response indicates no authorities.
         */
        public LSIDAuthority[] getForeignAuthorities(LSID lsid) 
        {
            String[] lookup = new String[] { lsid.ToString(), lsid.Authority/*.toString()*/ + ":" + lsid.Namespace, lsid.Authority/*.toString()*/ + ":*", "*:" + lsid.Namespace, "*:*" };
            LSIDAuthority[] auths = null;
            for (int i = 0; i < lookup.Length; i++) 
            {
                auths = (LSIDAuthority[]) foreignAuthorities[lookup[i]];
                if (auths != null)
                    return auths;
            }
            return null;
        }

        /**
         * Returns the root working directory of the client.
         * @return File
         */
        public string getLsidHome() 
        {
            return lsidHome;
        }
	
        /**
         * Returns the accepted formats for this client
         * @return String[] the format strings
         */
        public string[] getAcceptedFormats() 
        {
            if (acceptedFormats == null)
                return DEFAULT_METADATA_FORMATS;
            else
                return acceptedFormats;	
        }

        /**
         * Returns the metaDataFactory the factory to use to create metadata stores.  
         * @param String the format
         * @return LSIDMetadataFactory the factory
         */
        public LSIDMetadataFactory getMetadataFactory(String format) 
        {
            LSIDMetadataFactory factory = (LSIDMetadataFactory)metadataFactories[format];
            if (factory == null)
                factory = (LSIDMetadataFactory) DEFAULT_METADATA_FACTORIES[format];
            return factory;
        }
	
        /**
         * Get the meta data factory to use for a given port type
         * @param PortType the WSDL port type
         * @return LSIDPortFactory factory
         */
        public LSIDPortFactory getLSIDPortFactory(XmlQualifiedName portName) 
        {
            String key =  portName.Namespace + ":" + portName.Name;
            Object fact = portFactories[key];
            if (fact == null)
                return null;
            if (fact is LSIDPortFactory) return (LSIDPortFactory)fact;
            try 
            {
                //todo check
				PortFactory pf = (PortFactory)fact;
				                        
				LSIDPortFactory newFact = (LSIDPortFactory)
					System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(pf.getClassname());

                Hashtable ps = new Hashtable();
                for (int i=0;i<pf.getParamCount();i++) 
                {
                    Param param = pf.getParam(i);
                    ps.Add(param.getName(),param.getValue());
                }
                newFact.init(ps);
                portFactories.Add(key,newFact);
                return newFact;
            } 
            catch (Exception) 
			{                       

                throw new LSIDException("Error loading lsid port factory for port: " + portName.ToString());
            }
        }
	
        /**
         * Get the local copy of a WSDL import document with the given location url
         * @param String the location url from the document containing the import
         * @return ImportMapChoice the file containing the import.
         */
        public ImportMapChoice getImportMap(String url) 
        {
            return (ImportMapChoice)importMaps[url];	
        }

        /**
         * Returns whether or not resolvers should use caching
         * @return boolean
         */
        public Boolean UseCache() 
        {
            return useCache;
        }

        /**
         * enable caching.  
         * @param boolean whether to useCache or not.
         */
        public void setUseCache(Boolean useCache) 
        {
            if (this.useCache == useCache)
                return;
            if (useCache == false)
                this.useCache = false;
            else 
            {
                if (cache == null) 
                {
                    string cacheDir = null;
                    String cacheDirStr = lsidCacheDir;
                    if (cacheDirStr != null)
                        cacheDir = cacheDirStr;
                    else
                        cacheDir = lsidHome + "\\" + LSID_CACHE_DIR_DEFAULT;

                    try 
                    {
                        //System.Configuration.ConfigurationSettings.AppSettings[LSIDCache.CACHE_DIR_PROPERTY] = cacheDir;
                    } 
                    catch (IOException e) 
                    {
						LSIDException.WriteError(
							"Error setting cache dir path: " + cacheDir + ", stack trace follows, will use default");
                        
						LSIDException.PrintStackTrace(e);
                    }
                    try 
                    {
                        cache = LSIDCache.load();
                    } 
                    catch (Exception e) 
                    {                        
						LSIDException.WriteError(
							"Error loading LSID Cache, will continue without caching, stack trace follows");
                        
						LSIDException.PrintStackTrace(e);
                    }
                }
                this.useCache = true;
            }
        }

        /**
         * write a default config file to the lsid-home directory
         */
        private void writeDefaultConfig() 
        {
			//todo embedded resource config
            //		InputStream in = null;
            //		OutputStream out = null;
            //		try {
            //			in = getClass().getResourceAsStream(CONFIG_FILE_NAME);
            //			if (in == null) {
            //				System.err.println("Non fatal error: no default config exists in client installation");
            //				return;
            //			}
            //			out = new FileOutputStream(new File(lsidHome, CONFIG_FILE_NAME));
            //			byte[] bytes = new byte[1024];
            //			int numbytes = in.read(bytes);
            //			while (numbytes != -1) {
            //				out.write(bytes, 0, numbytes);
            //				numbytes = in.read(bytes);
            //			}
            //			out.flush();
            //		} catch (IOException e) {
            //			System.err.println("No fatal error: could not write default config file, stack trace follows");
            //			e.printStackTrace();
            //		} finally {
            //			if (in != null) {
            //				try {
            //					in.close();
            //				} catch (IOException e) {
            //					e.printStackTrace();
            //				}
            //			}
            //			if (out != null) {
            //				try {
            //					out.close();
            //				} catch (IOException e) {
            //					e.printStackTrace();
            //				}
            //			}
            //		}
        }

        /**
         * load the configuration from a Hashtable.  If the table is null, use default config.  Use default
         * config for any property that does not exist in a non-null table.
         */
        private void loadConfig(LSIDClient conf) 
        {
            useCache = USE_CACHE_DEFAULT;
            string cacheDir = lsidHome + "\\" + LSID_CACHE_DIR_DEFAULT;
            if (conf != null) 
            {
			
                // read cache config
                Caching c = conf.getCaching();
                if (c != null) 
                {
                    String useCacheStr = c.getUseCache();
                    if (useCacheStr != null)
                        useCache = Boolean.Parse(useCacheStr);
                    String cacheDirStr = c.getLsidCacheDir();
                    if (cacheDirStr != null)
                        cacheDir = cacheDirStr;
                }
			
                // read the accepted formats
                AcceptedFormats formats = conf.getAcceptedFormats();
                if (formats != null)
                    acceptedFormats = formats.getFormat();
			
                // read the metadata factories info
				//todo implement
//                MetadataStores mds = conf.getMetadataStores();
//                if (mds != null) 
//                {
//                    int count = mds.getMetadataStoreCount();
//                    for (int i=0;i<count;i++) 
//                    {
//                        MetadataStore ms = mds.getMetadataStore(i);
//                        if (ms != null) 
//                        {
//                            String mdfc = ms.getMetadataFactory();
//                            String type = ms.getFormat();
//							String asmName = ms.metadataFactoryAssembly;
//                            try 
//                            {
//                                LSIDMetadataFactory metadataFactory = (LSIDMetadataFactory) 
//								    System.Reflection.Assembly.LoadFrom(asmName).CreateInstance(mdfc);
//
//                                Hashtable factoryProps = new Hashtable();
//                                IEnumerator props = ms.getProperties().enumerateProperty();
//                                while (props.MoveNext()) 
//                                {
//                                    Property prop = (Property) props.Current;
//                                    String propname = prop.getName();
//                                    String propvalue = prop.getValue();
//                                    factoryProps.Add(propname, propvalue);
//                                }
//                                try 
//                                {
//                                    metadataFactory.setProperties(factoryProps);
//                                } 
//                                catch (Exception e) 
//                                {                                    
//									System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE,
//										"Error setting properties on meta data factory, stack trace follows: " + e.Message);
//                                 
//									LSIDException.PrintStackTrace(e);
//                                }
//                                metadataFactories.Add(type,metadataFactory);
//                            } 
//                            catch (Exception e) 
//                            {                                
//								System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE,
//									"Error instantiating class: " + mdfc + ", using default");
//                                
//								LSIDException.PrintStackTrace(e);
//                            }
//                        }
//                    }
//                }
			
                // read host mappings
                hostMappings = new Hashtable();
                HostMappings hMaps = conf.getHostMappings();
                
                if (hMaps != null)
                {
                    IEnumerator en = hMaps.enumerateHostMapping();

                    while (en.MoveNext())
                    {                   
                        HostMapping hm = (HostMapping)en.Current;
                        String auth = hm.getAuthority();
                        String endpoint = hm.getEndpoint();
                        hostMappings.Add(auth, endpoint);
                    }
                }

			    webSettings = new WebSettings();
                WebSettings ws = conf.getWebSettings();
                if (ws != null) 
                {
                    webSettings = ws;
                    HTTPUtils.WebProxy = ws.getProxy();
                }

                //read foreign authorities
				foreignAuthorities = new Hashtable();
                ForeignAuthorities fAuths= conf.getForeignAuthorities();
                if (fAuths != null)
                {
                    IEnumerator fen = fAuths.enumerateForeignAuthoritiesItem();

                    while(fen.MoveNext())
                    {
                        ForeignAuthoritiesItem fai = (ForeignAuthoritiesItem)fen.Current;
                        IEnumerator lsidEn = fai.enumerateLsid();
                        while (lsidEn.MoveNext())
                        {
                            FALSID l = (FALSID)lsidEn.Current;
                            LSID theLsid = null;
                            try
                            {
                                theLsid = new LSID(l.lsidVal);
                            }
                            catch(MalformedLSIDException ex)
                            {
                                ex.PrintStackTrace();
                                continue;
                            }

                            LSIDAuthority[] lsidAuths = new LSIDAuthority[l.authorityList.Count];
                            for (int i = 0; i < lsidAuths.Length; i++) 
                            {
                                try 
                                {
                                    lsidAuths[i] = new LSIDAuthority(l.authorityList[i].ToString());
                                } 
                                catch (MalformedLSIDException e) 
                                {
                                    e.PrintStackTrace();
                                }
                            }
						
                            try
                            {
                                //if it fails just dont add this fi
                                foreignAuthorities.Add(theLsid.Lsid, lsidAuths);
                            }
                            catch(Exception){}
                        }

                        IEnumerator pen = fai.enumeratePattern();
                        while(pen.MoveNext())
                        {
                            Pattern pat = (Pattern)pen.Current;
                            String patStr = pat.auth + ":" + pat.ns;
                            LSIDAuthority[] lsidAuths = new LSIDAuthority[pat.authorityList.Count];
                            for (int i = 0; i < pat.authorityList.Count; i++) 
                            {
                                try 
                                {
                                    lsidAuths[i] = new LSIDAuthority(pat.authorityList[i].ToString());
                                } 
                                catch (MalformedLSIDException e) 
                                {
                                    e.PrintStackTrace();
                                }
                            }
                            try
                            {
                                //if it fails just dont add this fi
                                foreignAuthorities.Add(patStr, lsidAuths);
                            }
                            catch(Exception){}
                        }
                    }
                }

                // read WSDL extension config
                WsdlExtensions wsdlExts = conf.getWsdlExtensions();
			
                // read import maps
                if (wsdlExts != null) 
                {
                    ImportMap[] ims = wsdlExts.getImportMap();
                    for (int i=0;i<ims.Length;i++) 
                    {
                        ImportMap im = ims[i];
                        importMaps.Add(im.getLocation(),im.getImportMapChoice());
                    }
				
                    // read port type factories
                    PortMap[] pms = wsdlExts.getPortMap();
                    for (int i=0;i<pms.Length;i++) 
                    {
                        PortMap pm = pms[i];
                        portFactories.Add(pm.getPortType(),pm.getPortFactory());	
                    }
                }
			
            }

            if (useCache) 
            {
                try 
                {
                    //System.Configuration.ConfigurationSettings.AppSettings[LSIDCache.CACHE_DIR_PROPERTY] = cacheDir;
                } 
                catch (IOException e) 
				{
					LSIDException.WriteError(
						"Error setting cache dir path: " + cacheDir + ", stack trace follows, will use default");
                                
					LSIDException.PrintStackTrace(e);                    
                }
                try 
                {
                    cache = LSIDCache.load();
                } 
                catch (Exception e) 
				{
					LSIDException.WriteError(
						"Error loading LSID Cache, will continue without caching, stack trace follows");
                                
					LSIDException.PrintStackTrace(e);                                        
                }
            }

        }

    }
}
