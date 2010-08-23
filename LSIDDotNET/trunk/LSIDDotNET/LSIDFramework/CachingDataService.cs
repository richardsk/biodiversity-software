using System;
using System.IO;
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
    public class CachingDataService : LSIDDataService 
    {
        private LSIDDataService service;
        private LSIDCache cache;

        /**
         * Method CachingDataService.
         * @param service
         */
        public CachingDataService(LSIDDataService service) 
        {
            this.service = service;	
        }


        public Stream getData(LSIDRequestContext ctx) 
        {
            LSID lsid = ctx.Lsid;
            Hashtable headers = ctx.getProtocalHeaders();
            if (cache != null && !headers.ContainsValue(HTTPConstants.NOCACHE)) 
            {
                try 
                {
                    Stream strm = cache.readData(lsid,-1,-1);
                    if (strm != null) 
                    {
                        return strm;	
                    }
				
                    // wasn't in the cache, retrieve it and place it in the cache
                    strm = service.getData(ctx);
                    if (strm == null) 
                    {
                        throw new LSIDServerException(LSIDServerException.INTERNAL_PROCESSING_ERROR,"getData should not return null");	
                    }
                    return cache.writeData(lsid,strm,-1,-1);
                }
                catch (LSIDCacheException e) 
                {
                    throw new LSIDServerException(e,e.getErrorCode(),"cache exception in data lookup");	
                }			
            }
            // just use normal service.  is this proper for a caching service?
            return service.getData(ctx);
        }
	
        public Stream getDataByRange(LSIDRequestContext ctx, int start, int length) 
        {
            LSID lsid = ctx.Lsid;
            Hashtable headers = ctx.getProtocalHeaders();
            //System.err.println(cache);
            //System.err.println(headers.containsValue(HTTPConstants.NOCACHE));
            if (cache != null && !headers.ContainsValue(HTTPConstants.NOCACHE)) 
            {
                try 
                {
                    Stream strm = cache.readData(lsid,start,length);
                    if (strm != null) 
                    {
                        return strm;	
                    }
				
                    // wasn't in the cache, retrieve it and place it in the cache
                    strm = service.getDataByRange(ctx,start,length);
                    if (strm == null) 
                    {
                        throw new LSIDServerException(LSIDServerException.INTERNAL_PROCESSING_ERROR,"getData should not return null");	
                    }
                    return cache.writeData(lsid,strm,start,length);
                }
                catch (LSIDCacheException e) 
                {
                    throw new LSIDServerException(e,e.getErrorCode(),"cache exception in data lookup");	
                }			
            }
            // just use normal service.  is this proper for a caching service?
            return service.getDataByRange(ctx,start,length);
		
            //throw new LSIDServerException(LSIDException.NOT_IMPLEMENTED,"method getDataByRange not yet implemented");
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
