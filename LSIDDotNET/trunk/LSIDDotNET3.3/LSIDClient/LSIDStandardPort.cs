using System;
using System.Collections;

namespace LSIDClient
{
    public class LSIDStandardPortTypes
    {
        /**
        * HTTP path type: indicates that the location should be used as a URL as is.
        */
        public static readonly String PATH_TYPE_DIRECT = "direct";
	
        /**
             * HTTP path type: indicates that input message parts must be added to the URL as HTTP GET parameters.
             */
        public static readonly String PATH_TYPE_URL_ENCODED = "urlEncoded"; 
    }

    public interface LSIDStandardPort : LSIDPort 
    {
        /**
         * Returns the location of the port.
         * @return String for HTTP and SOAP, this returns "http://host:port/path", for FTP, this returns the server name.
         */
        String getLocation();
	
        /**
         * Returns the path or path type depending on the protocol.
         * @return String for FTP this returns the directory and file path, for HTTP this returns the path type, for SOAP, this returns
         * null.
         */
        String getPath();
	
        /**
         * Returns the protocol
         * @return String "ftp", "http", or "soap"
         */
        String getProtocol();

    }


    /**
         * Implements the public interface LSIDDataPort so that
         * instances of this class may be used to retrieve data. 
         */
    public class LSIDStandardPortImpl : LSIDDataPort, LSIDMetadataPort, LSIDAuthorityPort 
    {
        public String name = null;
        public String serviceName = null;
        public String location;
        public String protocol;
        public String path;
        public LSIDCredentials lsidCredentials = null;
        public Hashtable headers = new Hashtable();
        /**
             * Method addProtocolHeader.
             * @param name
             * @param value
             */
        public void addProtocolHeader(String name, String value) 
        {
            headers.Add(name, value);
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
             * Return the name of the port
             */
        public String getName() 
        {
            return name;
        }

        /**
             * @see LSIDDataPort#getServiceName()
             */
        public String getServiceName() 
        {
            return serviceName;
        }

        /**
             * Return the host name of the data, includes the protocol for http (http://hostname), 
             * return entire URI for soap
             */
        public String getLocation() 
        {
            return location;
        }

        /**
             * Return the path of the data
             */
        public String getPath() 
        {
            return path;
        }

        /**
             * Return the protocol that must be used to retrieve the data
             */
        public String getProtocol() 
        {
            return protocol;
        }

        /**
             * get the key to store this port by
             */
        public String getKey() 
        {
            return serviceName + ":" + name;
        }

    }
}
