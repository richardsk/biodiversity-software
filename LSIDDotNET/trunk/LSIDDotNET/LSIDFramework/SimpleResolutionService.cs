using System;
using System.IO;
using System.Collections;

using LSIDClient;

namespace LSIDFramework
{
/**
 * 
 * This implementation provides a simple way to implement a complete resolution service (authority, data, and meta data)
 * thats runs on a single app server. The DataServlet, MetadataServlet and AuthorityServlet are hosted together.
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 */
    public abstract class SimpleResolutionService : LSIDAuthorityService, LSIDDataService, LSIDMetadataService 
    {
        private static String FOREIGN_HINT = "foreignAuthorities";

        private ArrayList validLSIDs = new ArrayList();

        public ExpiringResponse getAvailableServices(LSIDRequestContext req) 
        {
            if (!validLSIDs.Contains(req.Lsid)) 
            {
                validate(req);
                validLSIDs.Add(req.Lsid);
            }

            String reqUrl = req.ReqUrl;
            if(!reqUrl.EndsWith("/")) 
            {
                reqUrl = reqUrl + "/";
            }
            try 
            {
                LSIDWSDLWrapper wsdl = new LSIDWSDLWrapper(req.Lsid);
                if (hasData(req)) 
                {
                    String urlStr = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) + "/data/";
                    wsdl.setDataLocation(new SOAPLocation(getServiceName(), "SOAPData", urlStr));
                    try 
                    {
                        Uri dataURL = new Uri(urlStr);
                        LSIDDataPort p = new HTTPLocation(getServiceName(), "HTTPData", dataURL.Host, dataURL.Port, dataURL.AbsolutePath);
                        wsdl.setDataLocation(p);
                    } 
                    catch (UriFormatException e) 
                    {
                        LSIDException.WriteError(e);
                    }
                }
                if (hasMetadata(req)) 
                {
                    String urlStr = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) + "/metadata/";
                    wsdl.setMetadataLocation(new SOAPLocation(getServiceName(), "SOAPMetadata", urlStr));
                    try 
                    {
                        Uri metaDataURL = new Uri(urlStr);
                        LSIDMetadataPort p = new HTTPLocation(getServiceName(), "HTTPMetadata", metaDataURL.Host, metaDataURL.Port, metaDataURL.AbsolutePath);
                        wsdl.setMetadataLocation(p);
                    } 
                    catch (UriFormatException e) 
                    {
                        LSIDException.WriteError(e);
                    }
                }
                if (hasForeignAuthorities(req)) 
                {
                    String urlStr = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) + "/metadata/" + FOREIGN_HINT;
                    wsdl.setMetadataLocation(new SOAPLocation(getServiceName() + "FAN", "SOAPForeignAuthorities", urlStr));
                    try 
                    {
                        Uri metaDataURL = new Uri(urlStr);
                        LSIDMetadataPort p = new HTTPLocation(getServiceName() + "FAN", "HTTPForeignAuthorities", metaDataURL.Host, metaDataURL.Port, "/authority/metadata/?hint=" + FOREIGN_HINT);
                        wsdl.setMetadataLocation(p);
                    } 
                    catch (UriFormatException e) 
                    {
                        LSIDException.WriteError(e);
                    }
                }
                addCustomServices(req, wsdl);
                return new ExpiringResponse(wsdl.ToString(), getExpiration());
            } 
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e, "Error processing getAvailableServices for LSID " + req.Lsid);
            }

        }

        /**
         * utility method to check if the caller can take metadata
         */
        protected void checkFormats(String[] formats, String format) 
        {
            Boolean hasRDF = false;
            if (formats != null && formats.Length > 0) 
            {
                for (int i = 0; i < formats.Length; i++) 
                {
                    if (formats[i].Equals(format)) 
                    {
                        hasRDF = true;
                        break;
                    }
                }
                if (!hasRDF)
                    throw new LSIDServerException(LSIDException.NO_METADATA_AVAILABLE_FOR_FORMATS, "No acceptable metadata format");
            }
        }

        /**
         * utility method to verify that the inbound lsid uses only lower-case letters in the case-sensitive fields
         * (namespace, object, revision).  Many service implementations
         * whose back-ends are case insensitive will need to use this method to ensure that the only acceptable lsid
         * is the lower-case one.  
         */
        protected void checkLowerCase(LSID lsid) 
        {
            String check = lsid.Namespace + lsid.Object + (lsid.Revision != null ? lsid.Revision : null);
            for (int i = 0; i < check.Length; i++) 
            {
                if (check[i] >= 65 && check[i] <= 90)
                    throw new LSIDServerException(LSIDServerException.UNKNOWN_LSID, "namespace, object and revision must not contain lower-case chars");
            }
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
         * Get the expiration date/time of the available operations.  By default, returns null, indicating no expiration.
         * Implementing classes should override this method if they want to include expiration information.
         * @return Date the date/time at which the available operations will expire.
         */
        protected DateTime getExpiration() 
        {
            return DateTime.MinValue; //new Date(System.currentTimeMillis() + 1000000);
        }

        /**
         * Returns the name of this service, must not include spaces.
         * @return String the name
         */
        protected abstract String getServiceName();

        /**
         * Validate an LSID.  This method is called be SimpleResolutionService to determine if a WSDL should be returned for the
         * given LSID.  If the LSID is invalid, an LSIDServerException should be thrown.
         * @param LSID the LSID to validate
         */
        protected abstract void validate(LSIDRequestContext req);

        /**
         * Returns whether or not the given LSID has metadata.  The implementer may assume that the LSID is valid
         * @param LSID the LSID
         * @return boolean whether or not the given LSID has metadata
         */
        protected abstract Boolean hasMetadata(LSIDRequestContext req);

        /**
         * Returns whether or not the given LSID has data.  The implementer may assume that the LSID is valid
         * @param LSID the LSID
         * @return boolean whether or not the given LSID has data
         */
        protected abstract Boolean hasData(LSIDRequestContext req);

        public abstract MetadataResponse doGetMetadata(LSIDRequestContext ctx, String[] acceptedFormats);

        /**
         * Returns whether or not the given LSID has and registered foreign authorities
         * This function can be overridden to provide custom functionality
         * @param LSID the LSID
         * @return boolean whether or not the given LSID has metadata
         */
        protected Boolean hasForeignAuthorities(LSIDRequestContext req) 
        {
            try 
            {
                return SimpleFANStore.lookupFAPs(req.Lsid).Length >= 1;
            } 
            catch (LSIDServerException e) 
            {
                LSIDException.WriteError(e);
                return false;
            }
        }

        /**
         * Add a foreign authority registration for a specific lsid
         * This function can be overridden to provide custom functionality and authentication
         * @param req
         * @param authorityName
         * @throws LSIDServerException
         */
        public void notifyForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            SimpleFANStore.addFAP(authorityName, req.Lsid);
            // update the cache
            LSIDCache cache = LSIDResolver.getCache();
            if (cache != null) 
            {
                try 
                {
                    LSIDWSDLWrapper wrapper = cache.readWSDL(req.Lsid.Authority, req.Lsid);
                    if (wrapper != null) 
                    {
                        LSIDMetadataPort port = wrapper.getMetadataPort(getServiceName() + "SOAP");
                        if (port == null) 
                        {
                            String reqUrl = req.ReqUrl;
                            String urlStr = reqUrl.Substring(0, reqUrl.LastIndexOf('/')) + "/metadata/" + FOREIGN_HINT;
                            wrapper.setMetadataLocation(new SOAPLocation(getServiceName() + "SOAP", "SOAPForeignAuthorities", urlStr));
                            try 
                            {
                                Uri metaDataURL = new Uri(urlStr);
                                LSIDMetadataPort p = new HTTPLocation(getServiceName() + "HTTP", "HTTPForeignAuthorities", metaDataURL.Host, metaDataURL.Port, "/authority/metadata/?hint=" + FOREIGN_HINT);
                                wrapper.setMetadataLocation(p);
                            } 
                            catch (UriFormatException e) 
                            {
                                LSIDException.WriteError(e);
                            }
                            cache.writeWSDL(req.Lsid.Authority, req.Lsid, wrapper);
                        }
                    }
                } 
                catch (LSIDException e) 
                {
                    throw new LSIDServerException(e, "Error updating cache for foreign auth notification");
                }
            }
        }

        /**
         * Remove a foreign authority registration from a specific lsid
         * This function can be overridden to provide custom functionality
         * @param req
         * @param authorityName
         * @throws LSIDServerException
         */
        public void revokeNotificationForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            SimpleFANStore.removeFAP(authorityName, req.Lsid);
        }

        /**
         * 
         * by default, this will return a not-implemented error to the client
         * 
         * @see LSIDDataService#getDataByRange(LSIDRequestContext, int, int)
         */
        public virtual Stream getDataByRange(LSIDRequestContext req, int start, int length) 
        {
            throw new LSIDServerException(LSIDException.METHOD_NOT_IMPLEMENTED, "Method getDataByRange not implemented");
        }

        /* 
         * @see LSIDMetadataService#getMetadata(LSIDRequestContext, String[])
         */
        public MetadataResponse getMetadata(LSIDRequestContext req, String[] acceptedFormats) 
        {
            String hint = req.Hint;
            if (hint != null && hint.Equals(FOREIGN_HINT))
                return new SimpleFANStoreMetadataService().getMetadata(req, acceptedFormats);
            else
                return doGetMetadata(req, acceptedFormats);
        }

    
        public virtual Stream getData(LSIDRequestContext req)
        {
            return null;
        }
    
        public virtual void initService(LSIDServiceConfig config)
        {

        }
    }

}
