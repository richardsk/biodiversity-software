using System;
using System.Collections;

namespace LSIDClient
{
	/**
	 * LSIDMetadataFactory implementation that simply returns a metadata store that queries metadata by
	 * applying an XSLT transform on the RDF and executes simple XPATH on the result.  The property "one-store" determines
	 * if the same instance is returned each time createInstance is called.  This should be enabled if users want to
	 * build up metadata over several calls.  The default behavior is to have a single store.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class XSLTMetadataFactory : LSIDMetadataFactory 
	{	
		private static String ONE_STORE = "one-store";
	
		private Boolean oneStore = true;
		private XSLTMetadata store;

		/**
		 * @see LSIDMetadataFactory#createInstance()
		 */
		public LSIDMetadata createInstance() 
		{
			if (!oneStore) 
			{
				XSLTMetadata data = new XSLTMetadata();
				data.init();
				return data;
			} 
			else 
			{
				if (store == null) 
				{
					store = new XSLTMetadata();
					store.init();
				}
				return store;
			}
		}

		/**
		 * @see LSIDMetadataFactory#setProperties(Hashtable)
		 */
		public void setProperties(Hashtable properties) 
		{
			String oneStoreStr = (String)properties[ONE_STORE];
			if (oneStoreStr == null)
				oneStore = true;
			else
				oneStore = Boolean.Parse(oneStoreStr);
		}

	}
}
