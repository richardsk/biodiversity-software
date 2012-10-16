using System;
using System.Collections;
using System.IO;

using LSIDClient;

namespace LSIDFramework
{
/**
 * 
 * This class provides a metadata caching service implementation for authorities.
 * Services that this wraps may only using the LSID and the HINT to determine which
 * metadata to return otherwise caching will not work properly. 
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 */
    public class CachingMetadataService : LSIDMetadataService 
    {	
        private LSIDMetadataService service;
        private LSIDCache cache;

        public CachingMetadataService(LSIDMetadataService service) 
        {
            this.service = service;
        }

        public MetadataResponse getMetadata(LSIDRequestContext req, String[] formats)
        {
            LSID lsid = req.Lsid;
            Hashtable headers = req.getProtocalHeaders();
            // make sure aren't caching foreign metadata
            if (!(req.Hint != null && req.Hint.Equals("foreignAuthorities")) && cache != null && !headers.ContainsValue(HTTPConstants.NOCACHE)) 
            {
                try 
                {
                    MetadataResponse response = cache.readMetadata(lsid.Authority,lsid,"servercache",req.Hint,formats);
                    if (response != null) 
                    {
                        return response;	
                    }
				
                    // wasn't in the cache, retrieve it and place it in the cache
                    response = service.getMetadata(req,formats);
                    if (response == null) 
                    {
                        throw new LSIDServerException(LSIDServerException.INTERNAL_PROCESSING_ERROR,"getMetaData should not return null");	
                    }
                    Stream str = cache.writeMetadata(lsid.Authority,lsid,"servercache",req.Hint,response.getMetadata(),response.getExpires(),response.getFormat());
                    return new MetadataResponse(str, response.getExpires(),response.getFormat() );
                }
                catch (LSIDCacheException e) 
                {
                    throw new LSIDServerException(e,e.getErrorCode(),"cache exception in metadata lookup");	
                }			
            }
            // just use normal service.  is this proper for a caching service?
            return service.getMetadata(req,formats);
        }

        /**
         * @see LSIDService#initService(LSIDServiceConfig)
         */
        public void initService(LSIDServiceConfig config) 
        {
            cache = LSIDResolver.getCache();
            if (cache == null) 
            {
                throw new LSIDServerException(LSIDServerException.SERVER_CONFIGURATION_ERROR,"Unable to load Authority Cache - environmental variable not set");	
            }
            if (service == null) 
            {
                throw new LSIDServerException(LSIDServerException.SERVER_CONFIGURATION_ERROR,"Unable to load Authority Service");	
            }		
        }

    }
}
