using System;
using System.Collections;
using System.Web.Services.Description;

namespace LSIDClient
{
	/**
	 * 
	 * This port object is created for any ports of unknown port type found in a WSDL received in response to 
	 * getAvailableOperations.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
    public class DefaultLSIDPort : LSIDPort 
    {
	
        private String portName = null;
        private String serviceName = null;	
        private LSIDCredentials credentials;
        private Hashtable protocolHeaders = new Hashtable();
        private Port port;
	
        /**
         * create a new port
         * @param String the service name
         * @param String the port name
         * @param Port the WSDL port
         */
        public DefaultLSIDPort(String serviceName, String portName, Port port) 
        {
            this.serviceName = serviceName;
            this.portName = portName;
			this.port = port;
        }

        /**
         * @see LSIDPort#getLsidCredentials()
         */
        public LSIDCredentials getLsidCredentials() 
        {
            return credentials;
        }

        /**
         * @see LSIDPort#setLsidCredentials(LSIDCredentials)
         */
        public void setLsidCredentials(LSIDCredentials lsidCredentials) 
        {
            this.credentials = lsidCredentials;
        }

        /**
         * @see LSIDPort#getName()
         */
        public String getName() 
        {
            return portName;
        }

        /**
         * @see LSIDPort#getServiceName()
         */
        public String getServiceName() 
        {
            return serviceName;
        }

        /**
         * @see LSIDPort#addProtocolHeader(String, String)
         */
        public void addProtocolHeader(String name, String value) 
        {
            protocolHeaders[name] = value;
        }

        /**
         * @see LSIDPort#getProtocalHeaders()
         */
        public Hashtable getProtocolHeaders() 
        {
            return protocolHeaders;
        }
	
        /**
         * Get the WSDL port
         * @return Port the WSDL port
         */
        public Port getPort() 
        {
            return port;
        }

    }

}
