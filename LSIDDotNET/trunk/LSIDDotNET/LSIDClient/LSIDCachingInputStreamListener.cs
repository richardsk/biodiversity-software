using System;

namespace LSIDClient
{
	/**
	*
	* Classes should implement this interface who need to be notified when 
	* the input stream has closed.
	
	* @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	*/
	interface LSIDCachingInputStreamListener 
	{
	
		/**
		 * Notify this listener that the input stream has closed.
		 * @param LSIDAuthority the authority associated with the data in the stream that was closed.
		 * @param LSID the LSID associated with the data in the stream that was closed.
		 */
		void inputStreamClosed(LSIDAuthority authority, LSID lsid);

	}
}
