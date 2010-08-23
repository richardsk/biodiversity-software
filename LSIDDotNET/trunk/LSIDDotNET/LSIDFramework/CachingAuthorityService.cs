using System;
using System.Collections;

using LSIDClient;

namespace LSIDFramework
{
/**
 * 
 * This class provides an authority caching service implementation for authorities.
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 */
    public class CachingAuthorityService : LSIDAuthorityService 
    {
        private LSIDAuthorityService service;
        private LSIDCache cache;
	
        /**
         * Method CachingAuthorityService.
         * @param service an LSIDAuthorityService impl
         */
        public CachingAuthorityService(LSIDAuthorityService service) 
        {
            this.service = service;	
        }


        public ExpiringResponse getAvailableServices(LSIDRequestContext ctx) 
        {
            // check the cache, then ask the authority
            LSID lsid = ctx.Lsid;
            Hashtable headers = ctx.getProtocalHeaders();
            if (cache != null && !headers.ContainsValue(HTTPConstants.NOCACHE)) 
            {
                try 
                {
                    LSIDWSDLWrapper response = cache.readWSDL(lsid.Authority,lsid);
                    if (response != null) 
                    {
                        return new ExpiringResponse(response.ToString(),response.getExpiration());	
                    }
				
                    // wasn't in the cache, retrieve it and place it in the cache
                    ExpiringResponse er = service.getAvailableServices(ctx);
                    if (er == null) 
                    {
                        throw new LSIDServerException(LSIDServerException.INTERNAL_PROCESSING_ERROR,"getAvailableServices should not return null");	
                    }
                    cache.writeWSDL(lsid.Authority,lsid,new LSIDWSDLWrapper((String)er.getValue()));
                    return er;
                } 
                catch (LSIDException e) 
                {
                    throw new LSIDServerException(e,e.getErrorCode(),"cache exception in wsdl lookup");	
                }			
            }
            // just use normal service.  is this proper for a caching service?
            return service.getAvailableServices(ctx);
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

        public void notifyForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            service.notifyForeignAuthority(req, authorityName);		
        }


        public void revokeNotificationForeignAuthority(LSIDRequestContext req,LSIDAuthority authorityName)
        {
            service.revokeNotificationForeignAuthority(req, authorityName);
		
        }

    }
}
