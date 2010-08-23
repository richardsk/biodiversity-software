using System;

namespace LSIDClient
{
	/**
	 * 
	 * This interface is implemented by classes who want to know about the completion of asynchonous calls to
	 * the LSID resolver API.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public interface ResolutionListener 
    {
	
        /**
         * Notification that a call to ResolverAuthority has completed
         * @param Integer the request ID
         * @param LSIDAuthority the authority that was resolved
         */
        void resolveAuthorityComplete(int requestID, LSIDAuthority authority);
	
        /**
         * Notification that the WSDL wrapper is availble
         * @param Integer the request ID
         * @param LSIDWSDLWrapper the WSDL wrapper
         */
        void getWSDLWrapperComplete(int requestID,LSIDWSDLWrapper wrapper);
	
        /**
         * Notification that data is available
         * @param Integer the request ID
         * @param Stream the stream of data, the stream must be closed.
         */
        void getDataComplete(int requestID, System.IO.Stream data);
	
        /**
         * Notification that metadata is available
         * @param Integer the request ID
         * @param MetadataResponse the metadata, the stream must be closed
         */
        void getMetadataComplete(int requestID, MetadataResponse metadata);
	
        /**
         * Notification that the metadata store is available
         * @param Integer the request ID
         * @param LSIDMetadata the the metadata store
         */
		//todo implement
        //void getMetadataStoreComplete(int requestID, LSIDMetadata metadata);
	
        /**
         * Notification that a call has failed
         * @param Integer the request ID 
         * @param LSIDException the exception for the failure
         */
        void requestFailed(int requestID, LSIDException cause);
	
    }
}
