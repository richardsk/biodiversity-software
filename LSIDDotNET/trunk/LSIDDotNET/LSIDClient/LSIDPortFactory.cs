using System;
using System.Collections;
using System.Web.Services.Description;

namespace LSIDClient
{
	/**
	 * 
	 * The customizable port factory architecture allows client applications to consume ports and bindings 
	 * found in the available operations WSDL.  Implementations of this interface must implement construction of 
	 * an instance of LSIDPort specific to the WSDL port type with which this factory is registered.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public interface LSIDPortFactory 
    {
	
        /**
         * initialize the factory
         * @param Hashtable configuration properties for the factory, loaded from lsid-client.xml config.
         */
        void init(Hashtable properties);
	
        /**
         * Create a new lsid port object.
         * @param String serviceName the service name of the port
         * @param Port the WSDL port object
         */
        LSIDPort createPort(String serviceName, Port port); 

    }
}
