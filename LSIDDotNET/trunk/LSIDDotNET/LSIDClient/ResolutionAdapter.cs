using System;
using System.IO;

namespace LSIDClient
{
	/**
	 * 
	 * A stub implementation of ResolutionListener.  Can be extended by classes that implement only a subset
	 * of the ResolutionListener methods.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class ResolutionAdapter : ResolutionListener 
	{

		public void getDataComplete(int requestID, Stream data) 
		{
		}

		public void getMetadataComplete(int requestID, MetadataResponse metadata) 
		{
		}

		//todo implement
//		public void getMetadataStoreComplete(int requestID, LSIDMetadata metadata) 
//		{
//		}

		public void getWSDLWrapperComplete(int requestID, LSIDWSDLWrapper wrapper) 
		{
		}

		public void requestFailed(int requestID, LSIDException cause) 
		{
		}

		public void resolveAuthorityComplete(int requestID, LSIDAuthority authority) 
		{
		}

	}
}
