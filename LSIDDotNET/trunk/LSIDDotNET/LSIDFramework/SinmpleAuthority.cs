using System;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 *
	 * This class provides an additional layer of abstraction from the LSIDAuthorityService interface.  The developer
	 * still must implement methods for providing the authority version and the available LSIDs.  The 
	 * getAvailableOperations method is broken down so that the developer need only provide the core 
	 * pieces of information that define the service
	 *
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */	 
    public abstract class SimpleAuthority : LSIDAuthorityService
    {
        public void initService(LSIDServiceConfig config)
        {
        }

        public ExpiringResponse getAvailableServices(LSIDRequestContext ctx)
        {
            try 
            {
                //return new ExpiringResponse("val", getExpiration());
                LSIDWSDLWrapper wsdl = new LSIDWSDLWrapper(ctx.Lsid);
                			
                LSIDDataPort[] dataLocs = getDataLocations(ctx.Lsid,ctx.ReqUrl);
                if (dataLocs == null)
                    throw new LSIDServerException(LSIDException.UNKNOWN_LSID,"Unknown LSID");
                else 
                    for (int i = 0; i< dataLocs.Length; ++i)
                        wsdl.setDataLocation(dataLocs[i]);
                
                LSIDMetadataPort[] metaLocs = getMetadataLocations(ctx.Lsid,ctx.ReqUrl);
                if (metaLocs != null)
                    for (int i = 0; i < metaLocs.Length; ++i)
                        wsdl.setMetadataLocation(metaLocs[i]);
                			
                return new ExpiringResponse(wsdl.ToString(),getExpiration());
            }
            catch (LSIDException e) 
            {
                throw new LSIDServerException(e, e.getErrorCode(), "Simple Authority Error in getAvailableOperations(" + ctx.Lsid + "): ");
            }
        }
	
        /**
         * Get the expiration date/time of the available operations.  By default, returns null, indicating no expiration.
         * Implementing classes should override this method if they want to include expiration information.
         * @return Date the date/time at which the available operations will expire.
         */
        protected DateTime getExpiration() 
        {
            return DateTime.MinValue;
        }
	
        /**
         * Get an array of the data sources at which the data may be retrieved
         * @param LSID the LSID
         * @param String the url over which the request for available operations came in on
         * @return LSIDStandardPort[] an array of data locations, an array of length 0 if no locations exist, null if the LSID is bad
         * @see HTTPLocation
         * @see FTPLocation
         * @see SOAPLocation
         */
        protected abstract LSIDDataPort[] getDataLocations(LSID lsid, String url); 

        /**
        * Get an array of sources at which the metadata may be retrieved and/or queried
        * @param LSID the LSID
        * @param String the url over which the request for available operations came in on
        * @return LSIDStandardPort[] an array of meta data sources, an array of length 0 if no locations exist, null if the LSID is bad
        * @see HTTPLocation
        * @see FTPLocation
        * @see SOAPLocation
        */
        protected abstract LSIDMetadataPort[] getMetadataLocations(LSID lsid, String url);  

        /**
        * @see LSIDAuthorityService#notifyForeignAuthority(LSIDRequestContext, LSIDAuthority)
        */
        public void notifyForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            throw new LSIDServerException(LSIDServerException.METHOD_NOT_IMPLEMENTED, "FAN service not available");
        }

        /** 
         * @see LSIDAuthorityService#revokeNotificationForeignAuthority(LSIDRequestContext, LSIDAuthority)
         */
        public void revokeNotificationForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName) 
        {
            throw new LSIDServerException(LSIDServerException.METHOD_NOT_IMPLEMENTED, "FAN service not available");
        }
    }
}
