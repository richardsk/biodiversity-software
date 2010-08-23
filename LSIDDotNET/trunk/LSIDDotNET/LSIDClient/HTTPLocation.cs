using System;
using System.Collections;


namespace LSIDClient
{
    public class HTTPLocation : LSIDDataPort, LSIDMetadataPort, LSIDAuthorityPort 
    {
        private String hostname = null;
        private String dataPath = null;
        private int port = -1;
        private String serviceName = null;
        private String portName = null;
        private LSIDCredentials lsidCredentials = null;	
        private Hashtable headers = new Hashtable();
        private String pathType = LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED;
	
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
         * Create an HTTP endpoint with default portName and serviceName
         * 
         * @param String the host name of the service
         * @param int the port number of the service, -1 for default
         * @param String the data path of the service getData
         */
        public HTTPLocation(String hostname, int port, String dataPath) 
        {
            this.hostname = hostname;
            this.port = port;
            this.dataPath = dataPath;
            if (this.dataPath == null)
                this.dataPath = "";
            else if (!this.dataPath.StartsWith("/"))
                this.dataPath = "/" + this.dataPath;
        }

        /**
         * Create an HTTP endpoint with default serviceNmae
         * 
         * @param String the name of the WSDL port
         * @param String the host name of the service
         * @param int the port number of the service, -1 for default
         * @param String the data path of the service
         */
        public HTTPLocation(String portName, String hostname, int port, String dataPath) 
        {
            this.portName = portName;
            this.hostname = hostname;
            this.port = port;
            this.dataPath = dataPath;
            if (this.dataPath == null)
                this.dataPath = "";
            else if (!this.dataPath.StartsWith("/"))
                this.dataPath = "/" + this.dataPath;
        }

        /**
         * Create an HTTP endpoint with default serviceNmae
         * 
         * @param String the name of the WSDL service
         * @param String the name of the WSDL port
         * @param String the host name of the service 
         * @param int the port number of the service, -1 for default
         * @param String the data path of the service
         */
        public HTTPLocation(String serviceName, String portName, String hostname, int port, String dataPath) 
        {
            this.serviceName = serviceName;
            this.portName = portName;
            this.hostname = hostname;
            this.port = port;
            this.dataPath = dataPath;
            if (this.dataPath == null)
                this.dataPath = "";
            else if (!this.dataPath.StartsWith("/"))
                this.dataPath = "/" + this.dataPath;
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
            if (port < 0)
                return "http://" + hostname + dataPath;
            else
                return "http://" + hostname + ":" + port + dataPath;
        }

        public String getPath() 
        {
            return pathType;
        }

        public String getProtocol() 
        {
            return "http";
        }

        /**
         * Sets the pathType.
         * @param pathType The pathType to set
         */
        public void setPathType(String pathType) 
        {
            this.pathType = pathType;
        }

    }

}
