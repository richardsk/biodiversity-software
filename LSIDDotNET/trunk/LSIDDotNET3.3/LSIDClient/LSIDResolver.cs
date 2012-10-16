using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Messaging;
using Microsoft.Web.Services2.Addressing;
using Microsoft.Web.Services2.Configuration;


namespace LSIDClient
{
	/**
	 *
	 * Resolves the authority, WSDL, Meta Data, and the actual data of the LSID.
	 * Configuration for the resolver is contained in the configuration file. 
	 * @see <a href="LSIDResolverConfig.html">LSIDResolverConfig</a>
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDResolver 
    {
        public static String ANY_AUTHORITY = "*";

        private LSIDAuthority authority;
        private LSID lsid;
        private LSIDWSDLWrapper wsdlWrapper;
        private LSIDCache cache;
        private Boolean useCache;
        private LSIDCredentials runtimeCredentials; // credentials set on this resolver dynamically
        private LSIDCredentials lsidCredentials; // credentials to use for the given lsid, specified in config file

        private static LSIDResolverConfig config = new LSIDResolverConfig();
		
        private static int NOTIFY = 0;
        private static int REVOKE = 1;

        /**
         * Set the home directory of the resolver and load the configuration from there. This 
         * call will override default configuration if no system property was specified at startup but
         * it will not overrite configuration that was already loaded from a file specified by a previously
         * set system property. 
         * @param String the file location of the home.
         */
        public static void setResolverHome(String location) 
        {
            String home = System.Configuration.ConfigurationSettings.AppSettings[LSIDResolverConfig.LSID_CLIENT_HOME];
            if (home == null) 
            {
                System.Configuration.ConfigurationManager.AppSettings.Add(LSIDResolverConfig.LSID_CLIENT_HOME, location);
                config = new LSIDResolverConfig();
            }
        }

        /**
         * get the configuration for LSID client configuration.  This is statkc because there is only a single configuration
         * per runtime.
         */
        public static LSIDResolverConfig getConfig() 
        {
            return config;
        }

        /**
         * Resolve the given authority against the host mappings table.
         * If it is not found there, we defer to DNS.
         * @param LSIDAuthority the authority to resolve
         * @return LSIDAuthority the authority resolved.
         */
        public static LSIDAuthority resolveAuthority(LSIDAuthority authority) 
        {
            if (authority.isResolved())
                return authority;
            String url = (String) config.getHostMappings()[authority.Authority];
            if (url == null)
                url = (String) config.getHostMappings()[ANY_AUTHORITY];
            if (url != null) 
            {
                authority.Url = url;
            } 
            else 
            {
                //create the DNS query
                //Record[] records = dns.getRecords("_lsid._tcp." + authority, Type.SRV);
				DNSResolver.Resolver.DNSRecord[] recs =  DNSResolver.Resolver.Query("_lsid._tcp." + authority.Authority, DNSResolver.Resolver.RecordTypes.DNS_TYPE_SRV);
                if (recs != null && recs.Length > 0)
                {
                    foreach (DNSResolver.Resolver.DNSRecord rec in recs)
                    {
                        if (rec.Type == DNSResolver.Resolver.RecordTypes.DNS_TYPE_SRV)
                        {
                            DNSResolver.Resolver.SRVRecord srv = (DNSResolver.Resolver.SRVRecord)rec;
                            
                            authority.Port = srv.Port;
                            authority.Server = srv.Target;
                            
                            break;
                        }
                    }
                }
                else 
                {
                    // developer convenience 
                    authority.Port = 80;
                    authority.Server = authority.Authority;
                    //throw new LSIDException(LSIDException.INTERNAL_PROCESSING_ERROR, "DNS Lookup for authority failed " + authority);	
                }
            }

            // try to get the WSDL for the authority...fail quietly but warn if it doesn't exist
            try 
            {
                LSIDWSDLWrapper wrapper = null;
                if (config.UseCache())
                    wrapper = config.getCache().readWSDL(authority, null);
                if (wrapper == null) 
                {
                    HTTPResponse resp = HTTPUtils.doGet(authority.getUrl(), null, null, null);
                    wrapper = new LSIDWSDLWrapper(resp.getData());
                    wrapper.setExpiration(resp.getExpiration());
                    if (config.UseCache())
                        config.getCache().writeWSDL(authority, null, wrapper);
                }
                authority.AuthorityWSDL = wrapper;
            } 
            catch (LSIDException) 
            {
				LSIDException.WriteError(
					"Error getting WSDL for authority: " + url + ", will attempt default bindings");
            }
            return authority;
        }

        public static void notifyForeignAuthority(LSIDAuthority foreignAuthority, LSID lsid, LSIDCredentials credentials) 
        {
            notifyForeignAuthority(NOTIFY, foreignAuthority, lsid, credentials);
        }

        public static void revokeNotificationForeignAuthority(LSIDAuthority foreignAuthority, LSID lsid, LSIDCredentials credentials) 
        {
            notifyForeignAuthority(REVOKE, foreignAuthority, lsid, credentials);
        }

        private static void notifyForeignAuthority(int op, LSIDAuthority foreignAuthority, LSID lsid, LSIDCredentials credentials) 
        {
            LSIDAuthority authority = lsid.Authority;
            if (!authority.isResolved())
                resolveAuthority(authority);
            LSIDWSDLWrapper wrapper = authority.AuthorityWSDL;
            LSIDAuthorityPort authorityPort = wrapper.getAuthorityPortForProtocol(WSDLConstants.SOAP);
            String protocol = authorityPort.getProtocol();
            if (credentials == null)
                credentials = authorityPort.getLsidCredentials();
            if (protocol.Equals(WSDLConstants.SOAP)) 
            {
                try 
				{
					string opName = SoapConstants.NOTIFY_FOREIGN_AUTHORITY_OP_NAME;
					if (op == REVOKE)
						opName = SoapConstants.REVOKE_NOTIFICATION_FOREIGN_AUTHORITY_OP_NAME;
					Hashtable ps = new Hashtable();
					ps.Add(WSDLConstants.LSID_PART, lsid.ToString());
					ps.Add(WSDLConstants.AUTHORITY_NAME_PART, foreignAuthority.ToString());

					SoapEnvelope respEnv = SOAPUtils.MakeSoapCall(authorityPort.getLocation(), opName, WSDLConstants.OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI, null, ps, credentials);
                } 
                catch (Exception e) 
                {
                    throw new LSIDException(e, "Error notifying authority");
                }
            } 
            else if (protocol.Equals(WSDLConstants.HTTP)) 
            {
                string authloc = authorityPort.getLocation();
                if (authloc.EndsWith("/"))
                	authloc = authloc.Substring(0, authloc.Length - 1);
                if (op == NOTIFY)
                	authloc += HTTPConstants.HTTP_AUTHORITY_SERVICE_NOTIFY_PATH;
                else
                	authloc += HTTPConstants.HTTP_AUTHORITY_SERVICE_REVOKE_PATH;
                Hashtable ps = new Hashtable();
                ps.Add(WSDLConstants.LSID_PART, lsid);
                ps.Add(WSDLConstants.AUTHORITY_NAME_PART,foreignAuthority.ToString());
                HTTPResponse resp = HTTPUtils.doGet(authloc, authorityPort.getProtocolHeaders(), ps, credentials);
            } 
            else
                throw new LSIDException("Cannot notify authority over protocol: " + protocol);

        }

        /**
         * Return the cache instance used be resolvers in our system
         * @return LSIDCache the cache, null if caching is disabled.
         */
        public static LSIDCache getCache() 
        { 
            return config.getCache();
        }

		static LSIDResolver()
		{			
			WebServicesConfiguration.MessagingConfiguration.AddTransport("http", SOAPUtils.LSIDTransport);	
		}

        // instance LSID methods

        /**
         * Construct a new resolver using the given LSID. 
         * @param LSID the LSID to resolve the authority of and to invoke operations on. 
         */
        public LSIDResolver(LSID lsid)
        {
            this.lsid = lsid;
            this.authority = lsid.Authority;
            this.cache = config.getCache();
            this.useCache = config.UseCache();
            this.lsidCredentials = new LSIDCredentials(lsid);
        }

        /**
         * Construct a new resolver using the given LSID and authority.
         * @param LSID the authority. 
         * @param LSID the LSID to invoke operations on
         */
        public LSIDResolver(LSIDAuthority authority, LSID lsid) 
        {
            this.authority = authority;
            this.lsid = lsid;
            this.cache = config.getCache();
            this.useCache = config.UseCache();
            this.lsidCredentials = new LSIDCredentials(lsid);
        }

        /**
         * Construct a new resolver using the given LSID.
         * @param LSID the LSID to resolve the authority of and to invoke operations on.
         * @param LSIDCredentials the credentials to use to resolve the LSID
         */
        public LSIDResolver(LSID lsid, LSIDCredentials credentials) 
        {
            this.lsid = lsid;
            this.authority = lsid.Authority;
            this.cache = config.getCache();
            this.useCache = config.UseCache();
            this.runtimeCredentials = credentials;
            this.lsidCredentials = new LSIDCredentials(lsid);
        }

        /**
         * Construct a new resolver using the given LSID and authority.  Caching is disabled by default.
         * @param LSID the authority. 
         * @param LSID the LSID to invoke operations on
         * @param LSIDCredentials the credentials to use to resolve the LSID
         */
        public LSIDResolver(LSIDAuthority authority, LSID lsid, LSIDCredentials credentials) 
        {
            this.authority = authority;
            this.lsid = lsid;
            this.cache = config.getCache();
            this.useCache = config.UseCache();
            this.runtimeCredentials = credentials;
            this.lsidCredentials = new LSIDCredentials(lsid);
        }

        /**
         * disable/enable cache for this resolver.  Can only be enabled only if caching is enabled in global config.
         * @param boolean whether or not to use cache
         */
        public void setUseLocalCache(Boolean use) 
        {
            if (config.UseCache() && use) 
            {
                useCache = true;
            } 
            else 
            {
                useCache = false;
            }
        }

        /**
         * Get the LSID for this resolver
         * @return LSID the lsid
         */
        public LSID getLSID() 
        {
            return lsid;
        }

        /**
         * Get the authority
         * @return LSIDAuthority the authority
         */
        public LSIDAuthority getAuthority() 
        {
            return authority;
        }

        /**
         * Get the wrapped WSDL returned as a response to getAvailableServices().
         * @return LSIDWSDLWrapper the wsdl
         */
		public LSIDWSDLWrapper getWSDLWrapper() 
		{
			if (wsdlWrapper != null)
				return wsdlWrapper;
		
			if (wsdlWrapper == null)
				fetchWSDL();
			return wsdlWrapper;		
		}

        /**
         * Open a connection to the data of the associated LSID, and return an InputStream.
         * NOTE: the caller MUST close the stream.  
         * @return InputStream an InputStream to the data.
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
        public Stream getData()
        {
            getWSDLWrapper();
            LSIDDataPort port = wsdlWrapper.getDataPort();
            return getData(port, -1, -1);
        }

        /**
         * Open a connection to the data of the associated LSID, and return an InputStream.
         * NOTE: the caller MUST close the stream.  
         * @param int start where to start getting data.
         * @param int length the number of bytes to retrieve. The stream returned my have more or fewer bytes, but if length
         * bytes are available, the stream will contain at least length bytes.
         * @return InputStream an InputStream to the data.
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
        public Stream getData(int start, int length) 
        {
            getWSDLWrapper();
            LSIDDataPort port = wsdlWrapper.getDataPort();
            return getData(port, start, length);
        }

        /**
         * Open a connection to the data of the associated LSID, and return an InputStream.
         * NOTE: the caller MUST close the stream.  
         * @param LSIDDataPort the port to use to get the data.  Instances of LSIDDataPort can be obtained 
         * by methods in this resolver's LSIDWSDLWrapper. 
         * bytes are available, the stream will contain at least length bytes.
         * @return InputStream an InputStream to the data.
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
        public Stream getData(LSIDDataPort port) 
        {
            if (port == null)
                throw new LSIDException("Null data port provided");
            return getData(port, -1, -1);
        }

        /**
         * Open a connection to the data of the associated LSID, and return an InputStream.
         * NOTE: the caller MUST close the stream.  
         * @param LSIDDataPort the port to use to get the data.  Instances of LSIDDataPort can be obtained 
         * by methods in this resolver's LSIDWSDLWrapper. 
         * @param int start where to start getting data.
         * @param int length the number of bytes to retrieve. The stream returned my have more or fewer bytes, but if length
         * bytes are available, the stream will contain at least length bytes.
         * @return InputStream an InputStream to the data.
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
        public Stream getData(LSIDDataPort port, int start, int length) 
        {
            if (port == null)
                throw new LSIDException("Null data port provided");
            Stream inp = null;
            if (useCache)
                inp = cache.readData(lsid, start, length);
            if (inp == null) 
            {
                getWSDLWrapper();
                inp = getDataFromPort(port, start, length);
                if (useCache)
                    return cache.writeData(lsid, inp, start, length);
            }
            return inp;
        }

        /**
         * Retrieve the metadata and load it into a store.
         * This method will send the default accepted formats configured for this client. This method is equivalent to calling
         * <code>getMetadata(new String[0])</code>
         * @return LSIDMetadata a reference to a store containing the retrieved metadata
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
		//todo implement
//        public LSIDMetadata getMetadataStore() 
//        {
//            MetadataResponse metadata = getMetadata();
//            String format = metadata.getFormat();
//            if (format == null)
//                throw new LSIDException("Can't create store, no metadata format received");
//            LSIDMetadata store = config.getMetadataFactory(format).createInstance();
//            store.addMetadata(metadata);
//            return store;
//        }

        /**
         * Retrieve the metadata and load it into a store.
         * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
         * accepted formats string indicating that we don't care about format.
         * @return LSIDMetadata a reference to a store containing the retrieved metadata
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
		//todo implement
//        public LSIDMetadata getMetadataStore(String[] acceptedFormats) 
//        {
//            MetadataResponse metadata = getMetadata(acceptedFormats);
//            String format = metadata.getFormat();
//            if (format == null)
//                throw new LSIDException("Can't create store, no metadata format received");
//            LSIDMetadata store = config.getMetadataFactory(format).createInstance();
//            store.addMetadata(metadata);
//            return store;
//        }

        /**
         * Retrieve the metadata and load it into a store.
         * This method will send the default accepted formats configured for this client. This method is equivalent to calling
         * <code>getMetadata(new String[0])</code>
         * @param LSIDMetadataPort the port to use to get the meta data.  Instances of LSIDMetadataPort can be obtained 
         * by methods in this resolver's LSIDWSDLWrapper.  
         * @return LSIDMetadata a reference to a store containing the retrieved metadata
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
		//todo implement
//        public LSIDMetadata getMetadataStore(LSIDMetadataPort port) 
//        {
//            MetadataResponse metadata = getMetadata(port);
//            String format = metadata.getFormat();
//            if (format == null || format.Length == 0)
//                throw new LSIDException("Can't create store, no metadata format received");
//            LSIDMetadata store = config.getMetadataFactory(format).createInstance();
//            store.addMetadata(metadata);
//            return store;
//        }

        /**
         * Retrieve the metadata and load it into a store.
         * @param LSIDMetadataPort the port to use to get the metadata.  Instances of LSIDMetadataPort can be obtained 
         * by methods in this resolver's LSIDWSDLWrapper.  
         * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
         * accepted formats string indicating that we don't care about format.
         * @return LSIDMetadata a reference to a store containing the retrieved metadata
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
		//todo implement
//        public LSIDMetadata getMetadataStore(LSIDMetadataPort port, String[] acceptedFormats) 
//        {
//            MetadataResponse metadata = getMetadata(port, acceptedFormats);
//            String format = metadata.getFormat();
//            if (format == null)
//                throw new LSIDException("Can't create store, no metadata format received");
//            LSIDMetadata store = config.getMetadataFactory(format).createInstance();
//            store.addMetadata(metadata);
//            return store;
//        }

        /**
         * Open a connection to the metadata of the associated LSID.
         * NOTE: the caller MUST close the stream contained in the MetadataResponse object.
         * This method will send the default accepted formats configured for this client. This method is equivalent to calling
         * <code>getMetadata(new String[0])</code>
         * @return MetadataResponse contains an InputStream to the metadata.
         */
        public MetadataResponse getMetadata() 
        {
            getWSDLWrapper();
            LSIDMetadataPort port = wsdlWrapper.getMetadataPort();
            if (port == null)
                throw new LSIDException(LSIDException.NO_METADATA_AVAILABLE, "No meta data available for lsid: " + lsid);
            return getMetadata(port, new String[0]);
        }

        /**
         * Open a connection to the metadata of the associated LSID.
         * NOTE: the caller MUST close the stream contained in the MetadataResponse object.
         * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
         * accepted formats string indicating that we don't care about format.
         * @return MetadataResponse contains an InputStream to the metadata.
         */
        public MetadataResponse getMetadata(String[] acceptedFormats) 
        {
            getWSDLWrapper();
            LSIDMetadataPort port = wsdlWrapper.getMetadataPort();
            if (port == null)
                throw new LSIDException(LSIDException.NO_METADATA_AVAILABLE, "No meta data available for lsid: " + lsid);
            return getMetadata(port, acceptedFormats);
        }

        /**
         * Open a connection to the metadata of the associated LSID.
         * NOTE: the caller MUST close the stream contained in the MetadataResponse object.
         * This method will send the default accepted formats configured for this client. This method is equivalent to calling
         * <code>getMetadata(new String[0])</code>* 
         * @param LSIDMetadataPort the port to use to get the metadata.  Instances of LSIDMetadataPort can be obtained 
         * by methods in this resolver's LSIDWSDLWrapper.  
         * @return MetadataResponse contains an InputStream to the metadata.
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
        public MetadataResponse getMetadata(LSIDMetadataPort port) 
        {
            if (port == null)
                throw new LSIDException(LSIDException.NO_METADATA_AVAILABLE, "No meta data available for lsid: " + lsid.ToString());
            return getMetadata(port, new String[0]);
        }

        /**
         * Open a connection to the metadata of the associated LSID.
         * NOTE: the caller MUST close the stream contained in the MetadataResponse object.
         * @param LSIDMetadataPort the port to use to get the metadata.  Instances of LSIDMetadataPort can be obtained 
         * by methods in this resolver's LSIDWSDLWrapper.  
         * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
         * accepted formats string indicating that we don't care about format.
         * @return MetadataResponse contains an InputStream to the metadata.
         * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
         */
        public MetadataResponse getMetadata(LSIDMetadataPort port, String[] acceptedFormats) 
        {
            if (port == null)
                throw new LSIDException("Null meta data port specified");
            MetadataResponse response = null;
           
            if (acceptedFormats != null && acceptedFormats.Length < 1)
                acceptedFormats = config.getAcceptedFormats();

            if (useCache)
                response = cache.readMetadata(authority, lsid, port.getServiceName(), port.getName(), acceptedFormats);
            if (response == null) 
            {
                getWSDLWrapper();
                String formatsStr = null;
                if (acceptedFormats != null) 
                {
                    formatsStr = acceptedFormats[0];
                    for (int i = 1; i < acceptedFormats.Length; i++)
                        formatsStr += "," + acceptedFormats[i];
                }
                response = getMetadataFromPort(port, formatsStr);
                if (useCache) 
                {
                    Stream cachingStream = cache.writeMetadata(authority, lsid, port.getServiceName(), port.getName(), (Stream) response.getValue(), response.getExpires(), response.getFormat());
                    response.setValue(cachingStream);
                }
            }
            return response;
        }

        /**
         * get the data input stream for the given location
         */
        private Stream getDataFromPort(LSIDDataPort ldp, int start, int length) 
        {
            if (ldp == null)
                throw new LSIDException(LSIDException.NO_DATA_AVAILABLE, "No data available for LSID  " + lsid);
            String protocol = ldp.getProtocol();
            String location = ldp.getLocation();
            String path = ldp.getPath();
            LSIDCredentials credentials = null;
            credentials = runtimeCredentials;
            if (credentials == null)
                credentials = ldp.getLsidCredentials();
            if (credentials == null)
                credentials = lsidCredentials;
            if (protocol == null || location == null)
                throw new LSIDException(LSIDException.NO_DATA_AVAILABLE, "No Data Available for LSID: " + lsid);
			if (protocol.Equals(WSDLConstants.HTTP)) 
			{
				Hashtable ps = null;
				if (ldp.getPath().Equals(LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED)) 
				{
					ps = new Hashtable();
					if (start != -1 && length != -1) 
					{
						ps.Add(WSDLConstants.START_PART, start.ToString());
						ps.Add(WSDLConstants.LENGTH_PART, length.ToString());
					}
					ps.Add(WSDLConstants.LSID_PART, lsid);
				}
				HTTPResponse resp = HTTPUtils.doGet(location, ldp.getProtocolHeaders(), ps, credentials);
				return resp.getData();
			} 
			else if (protocol.Equals(WSDLConstants.FTP)) 
			{
				if (start != -1 && length != -1) 
				{
					throw new LSIDException(LSIDException.METHOD_NOT_IMPLEMENTED, "getDataByRange not supported over FTP protocol, use Stream API for chunking.");
				}
				return FTPUtils.doGet(location, path, credentials);
			} 
			else if (protocol.Equals(WSDLConstants.FILE)) 
			{
				if (start != -1 && length != -1) 
				{
					throw new LSIDException(LSIDException.METHOD_NOT_IMPLEMENTED, "getDataByRange not supported over File protocol, use Stream API for chunking.");
				}
				try 
				{
					return System.IO.File.OpenRead(location);
				} 
				catch (FileNotFoundException e) 
				{
					throw new LSIDException(e, LSIDException.NO_DATA_AVAILABLE, "Could not open data file " + location);
				}
			} 
			else if (protocol.Equals(WSDLConstants.SOAP)) 
			{
				try 
				{
					Hashtable ps = new Hashtable();
					ps.Add(WSDLConstants.LSID_PART, lsid.ToString());

					if (start != -1 && length != -1)
					{
						ps.Add(WSDLConstants.START_PART, start.ToString());
						ps.Add(WSDLConstants.LENGTH_PART, length.ToString());
					}

					SoapEnvelope respEnv = SOAPUtils.MakeSoapCall(ldp.getLocation(), SoapConstants.GET_DATA_OP_NAME, WSDLConstants.OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI, null, ps, credentials);

					return respEnv.Context.Attachments[0].Stream;
				}
				catch (Exception ex) 
				{
					throw new LSIDException(ex, "Error processing response");
				} 
			}
			else 
			{
				throw new LSIDException(LSIDException.NO_DATA_AVAILABLE, "No Data Available for LSID: " + lsid + " using port: " + ldp.getName());
			}
        }

        /**
         * get the meta data input stream for the given location
         */
        private MetadataResponse getMetadataFromPort(LSIDMetadataPort lmdp, String formatsStr) 
        {
            if (lmdp == null)
                throw new LSIDException(LSIDException.NO_METADATA_AVAILABLE, "No meta data available for LSID  " + lsid);
            String protocol = lmdp.getProtocol();
            String location = lmdp.getLocation();
            String path = lmdp.getPath();
            if (protocol == null || location == null)
                throw new LSIDException(LSIDException.NO_METADATA_AVAILABLE, "No Meta Data Available for LSID: " + lsid + " using port: " + lmdp.getName());
            LSIDCredentials credentials = runtimeCredentials;

            if (credentials == null)
                credentials = lmdp.getLsidCredentials();
            if (credentials == null)
                credentials = lsidCredentials;
            if (protocol.Equals(WSDLConstants.HTTP)) 
            {
                Hashtable ps = null;
                if (lmdp.getPath().Equals(LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED)) 
                {
                    ps = new Hashtable();
                    ps.Add(WSDLConstants.LSID_PART, lsid.ToString());
                    if (formatsStr != null)
                        ps.Add(WSDLConstants.ACCEPTED_FORMATS_PART, formatsStr);
                }
                HTTPResponse resp = HTTPUtils.doGet(location, lmdp.getProtocolHeaders(), ps, credentials);
                String contentType = resp.getContentType();
                int ind = contentType.IndexOf(';');
                String format = null;
                if (ind != -1)
                    format = contentType.Substring(0, ind);
                else
                    format = contentType;
                return new MetadataResponse(resp.getData(), resp.getExpiration(), format);
            } 
            else if (protocol.Equals(WSDLConstants.FTP)) 
            {
                Stream data = FTPUtils.doGet(location, path, credentials);
                return new MetadataResponse(data, DateTime.MinValue, null);
            } 
            else if (protocol.Equals(WSDLConstants.FILE)) 
            {
                try 
                {
                    Stream data = File.OpenRead(location);
                    return new MetadataResponse(data, DateTime.MinValue, null);
                } 
                catch (FileNotFoundException e) 
                {
                    throw new LSIDException(e, LSIDException.NO_METADATA_AVAILABLE, "Could not open metadata file " + location);
                }
            } 
            else if (protocol.Equals(WSDLConstants.SOAP)) 
            {
				try 
				{
					Hashtable ps = new Hashtable();
					ps.Add(WSDLConstants.LSID_PART, lsid.ToString());
					ps.Add(WSDLConstants.ACCEPTED_FORMATS_PART, formatsStr);
					SoapEnvelope respEnv = SOAPUtils.MakeSoapCall(lmdp.getLocation(), SoapConstants.GET_METADATA_OP_NAME, WSDLConstants.OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI, null, ps, credentials);

					string fmt = "";
					DateTime exp = DateTime.MinValue;

					if (respEnv.Body != null)
					{						
						System.Xml.XmlNodeList expl = respEnv.Body.SelectNodes("//" + WSDLConstants.EXPIRATION_PART);
						if (expl != null && expl.Count > 0)
						{
							exp = SOAPUtils.parseDate(expl[0].InnerText);
						}

						System.Xml.XmlNodeList fmtel = respEnv.Body.SelectNodes("//" + WSDLConstants.FORMAT_PART);
						if (fmtel != null && fmtel.Count > 0)
						{
							fmt = fmtel[0].InnerText;
						}
					}

					return new MetadataResponse(respEnv.Context.Attachments[0].Stream, exp, fmt);

				} 
				catch (SoapException e) 
				{
					throw new LSIDException(e, "Error processing response");
				} 
				catch (Exception e) 
				{
					throw new LSIDException(e, "Error getting input stream from attachment");
				}
            }
            return null;
        }

        /**
         * Resolve the server and port of the authority.
         * This method populates the server and port fields of the associated
         * LSIDAuthority object.
         */
        private void resolveAuthority() 
        {
            authority = resolveAuthority(authority);
        }

        /**
         * fetch the WSDL and store it in the LSID associated with this resolver
         */
        private void fetchWSDL() 
        {
            wsdlWrapper = null;
            DateTime expires = DateTime.MinValue;
            int i = -1;
            LSIDAuthority currentAuthority = authority;
            LSIDAuthority[] foreignAuthorities = config.getForeignAuthorities(lsid);
            do 
            {
                LSIDWSDLWrapper newWrapper = null;

                if (useCache) 
                {
                    newWrapper = cache.readWSDL(currentAuthority, lsid);
                }
                if (newWrapper == null) 
                {
                    if (!currentAuthority.isResolved())
                        resolveAuthority(currentAuthority);
                    try 
                    {
                        LSIDCredentials credentials = runtimeCredentials;
                        if (credentials == null)
                        credentials = lsidCredentials;

                        LSIDWSDLWrapper authorityWSDL = authority.AuthorityWSDL;
                        LSIDAuthorityPort authorityPort = null;
                        if (authorityWSDL != null) 
                        {
                            authorityPort = authorityWSDL.getAuthorityPortForProtocol(WSDLConstants.HTTP);
                        }
                        if (authorityPort == null) // try default SOAP binding if the WSDL yields nothing
                            authorityPort = new SOAPLocation(currentAuthority.getUrl());
                        newWrapper = fetchWSDL(authorityPort);
                        if (useCache)
                            cache.writeWSDL(currentAuthority, lsid, newWrapper);
                    } 
                    catch (LSIDException e) 
                    {
                        // if we are dealing with foreign authorities and the error is simply that the
                        // the LSID is unknown, ignore the error.
                        if (i > -1 && e.getErrorCode() == LSIDException.UNKNOWN_LSID) 
                        {
                            i++; // update this to follow the length of the foreign keys, if we have any
                            if (foreignAuthorities != null && i < foreignAuthorities.Length) 
                            {
                                currentAuthority = foreignAuthorities[i];
                            } 
                            else 
                            {
                                currentAuthority = null;
                            }
                            continue;
                        } 
                        else
                            throw e;
                    }
                }

                if (i == -1) 
                {
                    wsdlWrapper = newWrapper;
                } 
                else 
                {
                    LSIDWSDLWrapper foreignWrapper = newWrapper;
                    // add all the data ports to the combined wrapper
                    Enumeration e = foreignWrapper.getDataPortNames();
                    while (e.hasMoreElements()) 
                    {
                        LSIDDataPort dataPort = foreignWrapper.getDataPort(e.nextElement().ToString());
                        wsdlWrapper.setDataLocation(dataPort);
                    }
                    // add all the metadata ports to the combined wrapper
                    Enumeration em = foreignWrapper.getMetadataPortNames();
                    while (em.hasMoreElements()) 
                    {
                        LSIDMetadataPort metadata = foreignWrapper.getMetadataPort(em.nextElement().ToString());
                        wsdlWrapper.setMetadataLocation(metadata);
                    }
                }

                i++; // update this to follow the length of the foreign keys, if we have any
                if (foreignAuthorities != null && i < foreignAuthorities.Length) 
                {
                    currentAuthority = foreignAuthorities[i];
                } 
                else 
                {
                    currentAuthority = null;
                }
            }
            while (currentAuthority != null);
            wsdlWrapper.setExpiration(expires);
        }

        /**
         * get the wsdl from the given authority port
         */
        private LSIDWSDLWrapper fetchWSDL(LSIDAuthorityPort authorityPort)
        {
            String protocol = authorityPort.getProtocol();
            LSIDCredentials credentials = runtimeCredentials;
            if (credentials == null)
                credentials = authorityPort.getLsidCredentials();
            if (credentials == null)
                credentials = lsidCredentials;

			if (protocol.Equals(WSDLConstants.SOAP)) 
			{
				LSIDWSDLWrapper wrapper = null;

				try 
				{   
					Hashtable ps = new Hashtable();
					ps.Add(WSDLConstants.LSID_PART, lsid.ToString());

					SoapEnvelope respEnv = SOAPUtils.MakeSoapCall(authorityPort.getLocation(), SoapConstants.GET_WSDL_OP_NAME, WSDLConstants.OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI, null, ps, credentials);

					DateTime exp = DateTime.MinValue; 
					if (respEnv.Header != null)
					{
						System.Xml.XmlNodeList expl = respEnv.Header.GetElementsByTagName(SoapConstants.EXPIRES_HEADER_ELT, null);
						if (expl != null && expl.Count > 0)
						{
							exp = SOAPUtils.parseDate(expl[0].InnerText);
						}
					}

					wrapper = new LSIDWSDLWrapper(respEnv.Context.Attachments[0].Stream);
					wrapper.setExpiration(exp);

				} 
				catch (Exception ex) 
				{
					throw new LSIDException(ex, "Error processing response");
				} 

                return wrapper;
            } 
            else if (protocol.Equals(WSDLConstants.HTTP)) 
            {
                String authloc = authorityPort.getLocation();
                if (authloc.EndsWith("/"))
                    authloc = authloc.Substring(0, authloc.Length - 1);
                authloc += HTTPConstants.HTTP_AUTHORITY_SERVICE_PATH + "/";
                Hashtable ps = new Hashtable();
                ps.Add(WSDLConstants.LSID_PART, lsid);
                HTTPResponse resp = HTTPUtils.doGet(authloc, authorityPort.getProtocolHeaders(), ps, credentials);
                LSIDWSDLWrapper wrapper = new LSIDWSDLWrapper(resp.getData());
                wrapper.setExpiration(resp.getExpiration());
                return wrapper;
            } 
            else
                throw new LSIDException("Cannot fetch wsdl over protocol: " + protocol);
        }

    }

}
