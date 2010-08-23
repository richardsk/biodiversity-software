using System;
using System.IO;

using Microsoft.Web.Services2.Configuration;

namespace LSIDClient
{
	/**
	 *
	 * This class provides an asynchronous (callback-based) LSID Client API.  Callback classes must implement
	 * <code>ResolutionListener</code> or extend <code>ResolutionAdapter</code>.  Static configuration methods are still 
	 * invoked on <code>LSIDResolver</code>.  
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class AsyncLSIDResolver 
	{	
		private WorkPool pool = new WorkPool();
		private ResolutionListener listener;
		private LSIDResolver resolver;
	
		private static int nextRequestID = 1;
	
		private static int getNextRequestID() 
		{
			return nextRequestID++;	
		}
	
		static AsyncLSIDResolver()
		{
			WebServicesConfiguration.MessagingConfiguration.AddTransport("http", SOAPUtils.LSIDTransport);	
		}

		/**
		 * Construct a new async resolver for doing ops not specific to a single LSID
		 * @param ResolutionListener the listener for callbacks
		 */
		public AsyncLSIDResolver(ResolutionListener listener) 
		{
			this.listener = listener;
		}
	
		/**
		 * Construct a new async resolver using the given LSID.
		 * @param ResolutionListener the listener for callbacks
		 * @param LSID the LSID to invoke operations on
		 */
		public AsyncLSIDResolver(ResolutionListener listener, LSID lsid) 
		{
			this.listener = listener;
			resolver = new LSIDResolver(lsid);
		}
	
		/**
		 * Construct a new async resolver using the given LSID and authority.
		 * @param ResolutionListener the listener for callbacks
		 * @param LSIDAuthority the authority to use instead of the one specified by the LSID
		 * @param LSID the LSID to invoke operations on
		 */
		public AsyncLSIDResolver(ResolutionListener listener, LSIDAuthority authority, LSID lsid) 
		{
			this.listener = listener;
			resolver = new LSIDResolver(authority,lsid);
		}

		/**
		 * Construct a new resolver using the given LSID.
		 * @param ResolutionListener the listener for callbacks
		 * @param LSID the LSID to resolve the authority of and to invoke operations on.
		 * @param LSIDCredentials the credentials to use to resolve the LSID
		 */
		public AsyncLSIDResolver(ResolutionListener listener, LSID lsid, LSIDCredentials credentials) 
		{
			this.listener = listener;
			resolver = new LSIDResolver(lsid,credentials);
		}
	
		/**
		 * Construct a new resolver using the given LSID and authority.
		 * @param ResolutionListener the listener for callbacks
		 * @param LSID the authority. 
		 * @param LSID the LSID to invoke operations on
		 * @param LSIDCredentials the credentials to use to resolve the LSID
		 */
		public AsyncLSIDResolver(ResolutionListener listener, LSIDAuthority authority, LSID lsid, LSIDCredentials credentials) 
		{
			this.listener = listener;
			resolver = new LSIDResolver(authority,lsid,credentials);
		}
	
		/**
		 * Construct a new resolver using the given synchronous resolver
		 * @param ResolutionListener the listener for callbacks
		 * @param LSIDResolver the fully configured resolver to use
		 */
		public AsyncLSIDResolver(ResolutionListener listener, LSIDResolver resolver) 
		{
			this.listener = listener;
			this.resolver = resolver;
		}
	
		/**
		 * Must be called in order to cleanup all the threads that will wait forever otherwise
		 */
		public void destroy() 
		{
			//pool.destroy();
		}
	
		/**
		 * get the synchronous resolver that this resolver is using
		 * @return LSIDResolver the resolver
		 */
		public LSIDResolver getLSIDResolver() 
		{
			return resolver;	
		}
	
		/**
		 * Resolve the given authority against the host mappings table.
		 * If it is not found there, we defer to DNS.
		 * @param LSIDAuthority the authority to resolve
		 * @return Integer the requestID generated for this request
		 */
		public int resolveAuthority(LSIDAuthority authority) 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runResAuth), new object[]{authority, reqID});
			return reqID;		
		}
		public void runResAuth(object[] args) 
		{
			LSIDAuthority auth = (LSIDAuthority) args[0];
			try 
			{
				if (auth.isResolved())
					listener.resolveAuthorityComplete((int)args[1],auth);
				else {
					LSIDAuthority result = LSIDResolver.resolveAuthority(auth);
					listener.resolveAuthorityComplete((int)args[1],result);
				}
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[1],e);
			}
		}
	
		/**
		 * Get the wrapped WSDL.
		 * @return Integer the requestID generated for this request
		 */
		public int getWSDLWrapper() 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetWSDL), new object[]{reqID});
			return reqID;		
		}
		public void runGetWSDL(object[] args) 
		{
			try {
				LSIDWSDLWrapper result = resolver.getWSDLWrapper();
				listener.getWSDLWrapperComplete((int)args[0],result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[0],e);
			}
		}
	
		/**
		 * Open a connection to the data of the associated LSID, and open an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled in the callback 
		 * @return Integer the requestID generated for this request
		 */
		public int getData() 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetData), new object[]{reqID});
			return reqID;
		}
		public void runGetData(object[] args) 
		{
			try {
				Stream result = resolver.getData();
				listener.getDataComplete((int)args[0],result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[0],e);
			}
		}
	
		/**
		 * Open a connection to the data of the associated LSID, and open an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled in the callback 
		 * @param int start where to start getting data.
		 * @param int length the number of bytes to retrieve. The stream returned my have more or fewer bytes, but if length
		 * @return Integer the requestID generated for this request
		 * bytes are available, the stream will contain at least length bytes.
		 */
		public int getData(int start, int length) 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetDataRange), new object[]{start, length, reqID});
			return reqID;
		}
		public void runGetDataRange(object[] args) 
		{
			try 
			{
				Stream result = resolver.getData((int)args[0], (int)args[1]);
				listener.getDataComplete((int)args[2],result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[2],e);
			}
		}
	
		/**
		 * Open a connection to the data of the associated LSID, and return an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled in the callback.  
		 * @param LSIDDataPort the port to use to get the data.  Instances of LSIDDataPort can be obtained 
		 * by methods in this resolver's LSIDWSDLWrapper.  
		 * @return Integer the requestID generated for this request
		 * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
		 */
		public int getData(LSIDDataPort port) 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetDataPort), new object[]{port, reqID});
			return reqID;	
		}
		public void runGetDataPort(object[] args) 
		{
			try 
			{
				Stream result = resolver.getData((LSIDDataPort)args[0]);
				listener.getDataComplete((int)args[1], result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[1], e);
			}
		}
	
		/**
		 * Open a connection to the data of the associated LSID, and return an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled in the callback.  
		 * @param LSIDDataPort the port to use to get the data.  Instances of LSIDDataPort can be obtained 
		 * by methods in this resolver's LSIDWSDLWrapper.  
		 * @param int start where to start getting data.
		 * @param int length the number of bytes to retrieve. The stream returned my have more or fewer bytes, but if length
		 * @return Integer the requestID generated for this request
		 * bytes are available, the stream will contain at least length bytes.
		 * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
		 */
		public int getData(LSIDDataPort port, int start, int length) 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetDataPortRange), new object[]{port, start, length, reqID});
			return reqID;
		}
		public void runGetDataPortRange(object[] args) 
		{
			try 
			{
				Stream result = resolver.getData((LSIDDataPort)args[0], (int)args[1], (int)args[2]);
				listener.getDataComplete((int)args[3], result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[2],e);
			}
		}
	
		
	
	
		/**
		 * Open a connection to the meta data of the associated LSID, and return an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled by the callback
		 * @return Integer the requestID generated for this request
		 */
		public int getMetadata() 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetMetadata), new object[]{reqID});
			return reqID;
		}
		public void runGetMetadata(object[] args) 
		{
			try 
			{
				MetadataResponse result = resolver.getMetadata();
				listener.getMetadataComplete((int)args[0], result);	
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[0], e);
			}
		}
	
		/**
		 * Open a connection to the meta data of the associated LSID, and return an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled by the callback
		 * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
		 * accepted formats string indicating that we don't care about format.
		 * @return Integer the requestID generated for this request
		 */
		public int getMetadata(String[] acceptedFormats) 
		{
			int reqID = getNextRequestID();			
			pool.AddJob(new WorkPool.Runnable(runGetMetadataAcc), new object[]{acceptedFormats, reqID});
			return reqID;
		}
		public void runGetMetadataAcc(object[] args) 
		{
			try 
			{
				MetadataResponse result = resolver.getMetadata((String[])args[0]);
				listener.getMetadataComplete((int)args[1], result);	
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[1],e);
			}
		}
	
		/**
		 * Open a connection to the meta data of the associated LSID, and return an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled in the callback  
		 * @param LSIDMetadataPort the port to use to get the meta data.  Instances of LSIDMetadataPort can be obtained 
		 * by methods in this resolver's LSIDWSDLWrapper.  
		 * @return Integer the requestID generated for this request
		 * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
		 */
		public int getMetadata(LSIDMetadataPort port) 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetMetadataPort), new object[]{port, reqID});
			return reqID;	
		}
		public void runGetMetadataPort(object[] args) 
		{
			try 
			{
				MetadataResponse result = resolver.getMetadata((LSIDMetadataPort)args[0]);
				listener.getMetadataComplete((int)args[1],result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[1],e);
			}
		}
	
		/**
		 * Open a connection to the meta data of the associated LSID, and return an InputStream.
		 * NOTE: the caller MUST close the stream after it is handled in the callback  
		 * @param LSIDMetadataPort the port to use to get the meta data.  Instances of LSIDMetadataPort can be obtained 
		 * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
		 * accepted formats string indicating that we don't care about format.
		 * by methods in this resolver's LSIDWSDLWrapper.  
		 * @return Integer the requestID generated for this request
		 * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
		 */
		public int getMetadata(LSIDMetadataPort port, String[] acceptedFormats) 
		{
			int reqID = getNextRequestID();
			pool.AddJob(new WorkPool.Runnable(runGetMetadataPortAcc), new object[]{port, acceptedFormats, reqID});
			return reqID;
		
		}
		public void runGetMetadataPortAcc(object[] args) 
		{
			try 
			{
				MetadataResponse result = resolver.getMetadata((LSIDMetadataPort)args[0], (String[])args[1]);
				listener.getMetadataComplete((int)args[2],result);
			} 
			catch (LSIDException e) 
			{
				listener.requestFailed((int)args[2],e);
			}
		}
	
		/**
		 * Open a connection to the metadata of the associated LSID, and put it in the store.
		 * @return Integer the requestID generated for this request
		 */
		//todo implement
//		public int getMetadataStore() 
//		{
//			int reqID = getNextRequestID();
//			pool.AddJob(new WorkPool.Runnable(runGetMetadataStore), new object[]{reqID});
//			return reqID;
//		}
//		public void runGetMetadataStore(object[] args) 
//		{
//			try 
//			{
//				LSIDMetadata result = resolver.getMetadataStore();
//				listener.getMetadataStoreComplete((int)args[0],result);	
//			} 
//			catch (LSIDException e) 
//			{
//				listener.requestFailed((int)args[0],e);
//			}
//		}
	
		/**
		 * Open a connection to the metadata of the associated LSID, and put it in the store.
		 * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
		 * accepted formats string indicating that we don't care about format.
		 * @return Integer the requestID generated for this request
		 */
		//todo implement
//		public int getMetadataStore(String[] acceptedFormats) 
//		{
//			int reqID = getNextRequestID();
//			pool.AddJob(new WorkPool.Runnable(runGetMetadataStoreAcc), new object[]{acceptedFormats, reqID});
//			return reqID;
//		}
//		public void runGetMetadataStoreAcc(object[] args) 
//		{
//			try 
//			{
//				LSIDMetadata result = resolver.getMetadataStore((String[])args[0]);
//				listener.getMetadataStoreComplete((int)args[1],result);	
//			} 
//			catch (LSIDException e) 
//			{
//				listener.requestFailed((int)args[1],e);
//			}
//		}
	
		/**
		 * Open a connection to the metadata of the associated LSID, and put it in the store
		 * @param LSIDMetadataPort the port to use to get the meta data.  Instances of LSIDMetadataPort can be obtained 
		 * by methods in this resolver's LSIDWSDLWrapper.  
		 * @return Integer the requestID generated for this request
		 * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a> 
		 */
		//todo implement
//		public int getMetadataStore(LSIDMetadataPort port) 
//		{
//			int reqID = getNextRequestID();
//			pool.AddJob(new WorkPool.Runnable(runGetMetadataStorePort), new object[]{port, reqID});
//			return reqID;	
//		}
//		public void runGetMetadataStorePort(object[] args) 
//		{
//			try 
//			{
//				LSIDMetadata result = resolver.getMetadataStore((LSIDMetadataPort)args[0]);
//				listener.getMetadataStoreComplete((int)args[1],result);
//			} 
//			catch (LSIDException e) 
//			{
//				listener.requestFailed((int)args[0],e);
//			}
//		}
	
		/**
		 * Open a connection to the meta data of the associated LSID, and put it in the store.
		 * @param LSIDMetadataPort the port to use to get the meta data.  Instances of LSIDMetadataPort can be obtained 
		 * @param String[] the accepted formats. If length 0, will use default formats from config.  If null, will send no
		 * accepted formats string indicating that we don't care about format.
		 * by methods in this resolver's LSIDWSDLWrapper.  
		 * @return Integer the requestID generated for this request
		 * @see <a href="LSIDWSDLWrapper.html">LSIDWSDLWrapper</a>
		 */
		//todo implement
//		public int getMetadataStore(LSIDMetadataPort port, String[] acceptedFormats) 
//		{
//			int reqID = getNextRequestID();
//			pool.AddJob(new WorkPool.Runnable(runGetMetadataStorePortAcc), new object[]{port, acceptedFormats, reqID});
//			return reqID;	
//		}
//		public void runGetMetadataStorePortAcc(object[] args) 
//		{
//			try 
//			{
//				LSIDMetadata result = resolver.getMetadataStore((LSIDMetadataPort)args[0],(String[])args[1]);
//				listener.getMetadataStoreComplete((int)args[2],result);
//			} 
//			catch (LSIDException e) 
//			{
//				listener.requestFailed((int)args[2],e);
//			}
//		}
	
	
	}
}