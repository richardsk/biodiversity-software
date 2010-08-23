using System;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 * 
	 * LSIDAuthorityService provides an interface to an LSID authority implementation.  Implementations of LSIDAuthorityService
	 * must be threadsafe. 
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public interface LSIDAuthorityService : LSIDService 
    {
	
        /**
        * Get the WSDL document that describes the methods that can be called on the given LSID
        * @param LSID the LSID to query
        * @param String the url over which the request came in.  Implementation should feel free to use
        * this url as a reference for building the operations WSDL.
        * @return ExpiringResponse contains a String of the WSDL document
        */
        ExpiringResponse getAvailableServices(LSIDRequestContext req); 
	
        /**
         * Add a known foreign authority to the metadata of an lsid
         * @param req
         * @param authorityName
         * @throws LSIDServerException
         */
        void notifyForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName); 
	
	
        /**
         * Remove a foreign authority registration from a specific lsid
         * @param req
         * @param authorityName
         * @throws LSIDServerException
         */
        void revokeNotificationForeignAuthority(LSIDRequestContext req, LSIDAuthority authorityName); 
    }
}
