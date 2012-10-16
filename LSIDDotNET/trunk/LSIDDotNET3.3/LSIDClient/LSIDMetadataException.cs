using System;

namespace LSIDClient
{
	/**
	 * 
	 * Encapsulates an exception in an LSID meta data store
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class LSIDMetadataException : LSIDException 
	{

		/**
		 * Constructor for LSIDMetadataException.
		 * @param Exception the root cause
		 * @param int the errorCode
		 * @param String the description
		 */
		public LSIDMetadataException(Exception e, int errorCode, String description) : base(e, errorCode, description)
		{
		}

		/**
		 * Constructor for LSIDMetadataException.
		 * @param int the errorCode
		 * @param String the description
		 */
		public LSIDMetadataException(int errorCode, String description) : base (errorCode, description)
		{
		}

		/**
		 * Constructor for LSIDMetadataException.
		 * @param Exception the root cause
		 * @param String the description
		 */
		public LSIDMetadataException(Exception e, String description) : base (e, description)
		{
		}

		/**
		 * Constructor for LSIDMetadataException.
		 * @param String the description
		 */
		public LSIDMetadataException(String description) : base(description)
		{
		}

	}
}