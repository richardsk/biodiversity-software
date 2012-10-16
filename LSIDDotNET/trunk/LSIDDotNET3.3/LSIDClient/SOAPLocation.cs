using System;
using System.Collections;

namespace LSIDClient
{

    public class SOAPLocation : LSIDDataPort, LSIDMetadataPort, LSIDAuthorityPort 
    {
        private String endpoint = null;
        private String serviceName = null;
        private String portName = null;
        private LSIDCredentials lsidCredentials = null;	
        private Hashtable headers = new Hashtable();
        /**
         * Method addProtocolHeader.
         * @param name
         * @param value
         */
        public void addProtocolHeader(String name, String value)
        {
            headers.Add(name,value);
        }
	
        /**
         * Method getProtocolHeaders.
         * @return Map
         */
        public Hashtable getProtocolHeaders() 
        {
            return headers;
        }	
        /**
         * Returns the lsidCredentials.
         * @return LSIDCredentials
         */
        public LSIDCredentials getLsidCredentials() 
        {
            if (lsidCredentials == null) 
            {
                LSIDCredentials portCreds = new LSIDCredentials(this);
                if (portCreds.keys().hasMoreElements())
                    lsidCredentials = portCreds;
            }
            return lsidCredentials;
        }

        /**
         * Sets the lsidCredentials.
         * @param lsidCredentials The lsidCredentials to set
         */
        public void setLsidCredentials(LSIDCredentials lsidCredentials) 
        {
            this.lsidCredentials = lsidCredentials;
        }
	
        /**
         * Create a SOAP endpoint with default portName and serviceName
         * 
         * @param String the full endpoint url
         */
        public SOAPLocation(String url) 
        {
            endpoint = url;
        }

        /**
         * Create a SOAP endpoint with default serviceName
         * 
         * @param String the name of this port
         * @param String the full endpoint url
         */
        public SOAPLocation(String portName, String url) 
        {
            this.portName = portName;
            endpoint = url;
        }
	
        /**
         * Create a SOAP endpoint with default serviceName
         * 
         * @param String the name of the service that contains this port
         * @param String the name of this port
         * @param String the full endpoint url of the service
         */
        public SOAPLocation(String serviceName, String portName, String url) 
        {
            this.serviceName = serviceName;
            this.portName = portName;
            endpoint = url;
        }
	
        public String getServiceName() 
        {
            return serviceName;
        }

        public String getName() 
        {
            return portName;
        }

        public String getLocation() 
        {
            return endpoint;
        }

        public String getPath() 
        {
            return null;
        }

        public String getProtocol() 
        {
            return "soap";
        }

    }
}
