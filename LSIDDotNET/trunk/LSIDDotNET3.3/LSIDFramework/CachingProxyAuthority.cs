using System;
using System.Collections;
using System.IO;

using LSIDClient;

namespace LSIDFramework
{/**
 * 
 * This simple LSIDAuthority implementation forwards requests to the authority resolved by DNS.
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 */
    public class CachingProxyAuthority : LSIDAuthorityService, LSIDMetadataService, LSIDDataService 
    {

        // config properties
        private static String PROXY_HOME = "proxyHome";

        // a pool of LSIDResolvers, stored one per LSID.
        private static Hashtable resolverPool = new Hashtable();

        // service configurations
        LSIDServiceConfig config;

        // Authority Service

        /**
         * 
         * Get the available methods WSDL.
         * 
         */
        public ExpiringResponse getAvailableServices(LSIDRequestContext req) 
        {
            LSID lsid = req.Lsid;
            String reqUrl = req.ReqUrl;
            try 
            {
			
                // create a new SDL and return it
                LSIDWSDLWrapper wsdl = new LSIDWSDLWrapper(lsid);
			
                Boolean usecache = !req.getProtocalHeaders().ContainsValue(HTTPConstants.NOCACHE);
                LSIDResolver resolver = getResolver(lsid,usecache);
                LSIDWSDLWrapper wrapper = resolver.getWSDLWrapper();
			
                // determine which services we should include in the WSDL that we return to the caller
                LSIDDataPort dataPort = wrapper.getDataPort();
                if (dataPort != null) 
                {
                    // set a soap port
                    String dataUrl = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) 
                        + "/data";
                    wsdl.setDataLocation(new SOAPLocation("CachingProxySOAP","SOAPData",dataUrl));
                    // set an http port 
                    dataUrl = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) + "/data";
                    try 
                    {
                        Uri dataURL = new Uri(dataUrl);
                        wsdl.setDataLocation(new HTTPLocation("CachingProxyHTTP","HTTPData",dataURL.Host, dataURL.Port, dataURL.AbsolutePath));
                    } 
                    catch (Exception e) 
                    {
                        throw new LSIDServerException(e, "Internal error parsing URL: " + dataUrl);
                    }				
                }
			
                Enumeration portNames = wrapper.getMetadataPortNames();
                int portNum = 1;
                while (portNames.hasMoreElements()) 
                {
                    LSIDMetadataPort metaDataPort = wrapper.getMetadataPort((String)portNames.nextElement());
                    if (metaDataPort != null) 
                    {
                        // set a soap port
                        String metaDataUrl = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) 
                            + "/metadata/" + metaDataPort.getServiceName() + ";" + metaDataPort.getName();
                        wsdl.setMetadataLocation(new SOAPLocation("CachingProxySOAP","SOAPMetaData" + portNum,metaDataUrl));
                        // set an http port
                        metaDataUrl = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) + "/metadata";
                        try 
                        {
                            Uri dataURL = new Uri(metaDataUrl);
                            wsdl.setMetadataLocation(new HTTPLocation("CachingProxyHTTP","HTTPMetaData" + portNum, dataURL.Host, dataURL.Port, dataURL.AbsolutePath + 
                                "?hint=" + metaDataPort.getServiceName() + ";" + metaDataPort.getName()));
                        } 
                        catch (Exception e) 
                        {
                            throw new LSIDServerException(e, "Internal error parsing URL: " + metaDataUrl);
                        }
                    }
                    portNum++;
                }
                addCustomServices(req,wsdl);
                return new ExpiringResponse(wsdl.getWSDL(),wrapper.getExpiration());
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e, e.getErrorCode(), "Proxy Error in getAvailableOperations(" + lsid + "): " + e.getDescription());
            }
        }

        // meta data service
        public MetadataResponse getMetadata(LSIDRequestContext req, String[] formats) 
        {
            LSID lsid = req.Lsid;
            String hint = req.Hint;		
            try 
            {
                LSIDMetadataPort port = useAlternateMetadataPort(req,formats);
                if (hint == null)
                    throw new LSIDServerException("No hint specified in meta data service");
                Boolean usecache = !req.getProtocalHeaders().ContainsValue(HTTPConstants.NOCACHE);
                LSIDResolver resolver = getResolver(lsid,usecache);
                if (port == null) 
                {
                    hint = hint.Replace(';',':');
                    port = resolver.getWSDLWrapper().getMetadataPort(hint);
                }
                return resolver.getMetadata(port,formats);
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e, e.getErrorCode(), "Proxy Error retrieving meta data for " + lsid + " | " + e.getDescription());
            }
        }

        // Data service

        /**
         * 
         * Get the data for the given LSID
         * 
         */
        public Stream getData(LSIDRequestContext req) 
        {
            LSID lsid = req.Lsid;
            try 
            {
                Boolean usecache = !req.getProtocalHeaders().ContainsValue(HTTPConstants.NOCACHE);
                LSIDResolver resolver = getResolver(lsid,usecache);
                return resolver.getData();
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e, e.getErrorCode(), "Proxy Error retrieving data for " + lsid + " | " + e.getDescription());
            }
        }
	
        /**
         * 
         * Get the data for the given LSID
         * 
         */
        public Stream getDataByRange(LSIDRequestContext req, int start, int length) 
        {
            LSID lsid = req.Lsid;
            try 
            {
                Boolean usecache = !req.getProtocalHeaders().ContainsValue(HTTPConstants.NOCACHE);
                LSIDResolver resolver = getResolver(lsid,usecache);
                return resolver.getData(start,length);
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e, e.getErrorCode(), "Proxy Error retrieving data for " + lsid + " | " + e.getDescription());
            }
        }

        /**
         * @see LSIDService#initService(LSIDServiceConfig)
         */
        public void initService(LSIDServiceConfig config) 
        {
            
            String location = config.getProperty(PROXY_HOME);
            if (location != null)
                LSIDResolver.setResolverHome(location);
            
            this.config = config;
        }
	
        /**
         * This method can be overridden by authority services who want to add extra services to the WSDL that gets returned
         * for a given LSID.  This can be extra data/metadata ports.  However, they may be ports for additional services that are 
         * not part of the standard LSID spec.  An example might be a service for submitting metadata for a given LSID. 
         * This method does nothing be default. 
         * @param LSIDRequestContext the request context containing the LSID and other useful information
         * @param LSIDWSDLWrapper the object contain the WSDL.  The WSDL4J editable document can be retrieved via
         * <code>wrapper.getDefinition()</code>
         */ 	
        protected void addCustomServices(LSIDRequestContext ctx, LSIDWSDLWrapper wrapper) 
        {
            return;
        }	
	
        /**
         * 
         * This method can be overriden by caching authorities who want to specify a metadata port for a given inbound LSID
         * other than a port from the resolved getAvailableServices WSDL.  
         * @return LSIDMetadataPort should return null if no port is to be specified
         */
        protected LSIDMetadataPort useAlternateMetadataPort(LSIDRequestContext ctx, String[] formats) 
        {
            return null;
        }

        // private utility methods

        /**
         * Get a resolver instance for the given LSID. Look in the resolver pool before creating a new instance
         */
		private static LSIDResolver getResolver(LSID lsid, Boolean usecache) 
		{
			/* can't share resolvers because we will be toggling cache on the fly to respect
				 * cache headers
				LSIDResolver resolver = (LSIDResolver) resolverPool.get(lsid.toString());
				if (resolver == null) {
					resolver = new LSIDResolver(lsid);
					resolverPool.put(lsid.toString(), resolver);
				}
				return resolver;
				*/
			LSIDResolver resolver = new LSIDResolver(lsid);
			resolver.setUseLocalCache(usecache);
			return resolver;
		}

        /* 
         * @see LSIDAuthorityService#notifyForeignAuthority(String, String)
         */
        public void notifyForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            try 
            {
                LSIDResolver.notifyForeignAuthority(authorityName, req.Lsid, req.Credentials);
            } 
            catch (MalformedLSIDException e) 
            {
                throw new LSIDServerException(e,LSIDServerException.MALFORMED_LSID,"Malformed LSID");
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e,LSIDServerException.INTERNAL_PROCESSING_ERROR,"Error");
            }	
        }

        /* 
         * @see LSIDAuthorityService#revokeNotifcationForeignAuthority(String, String)
         */
        public void revokeNotificationForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            try 
            {
                LSIDResolver.revokeNotificationForeignAuthority(authorityName,req.Lsid, req.Credentials);
            } 
            catch (MalformedLSIDException e) 
            {
                throw new LSIDServerException(e,LSIDServerException.MALFORMED_LSID,"Malformed LSID");
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e,LSIDServerException.INTERNAL_PROCESSING_ERROR,"Error");
            }		
        }

    }

}
