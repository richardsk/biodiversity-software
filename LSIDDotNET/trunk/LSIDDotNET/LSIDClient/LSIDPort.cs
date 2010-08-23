using System;

namespace LSIDClient
{
	/**
	 * This interface provides the details of all LSID port types defined in a WSDL.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public interface LSIDPort 
    {

        /**
         * Returns the lsidCredentials.
         * @return LSIDCredentials
         */
        LSIDCredentials getLsidCredentials();

        /**
         * Sets the lsidCredentials.
         * @param lsidCredentials The lsidCredentials to set
         */
        void setLsidCredentials(LSIDCredentials lsidCredentials);
	
        /**
         * Return the name of the port
         * @return String then name of the port
         */
        String getName();
	
        /**
         * Return the name of the service that contains this port
         * @return String the name of the service.  a value of null implies that a default service name should be used.
         */
        String getServiceName();


        /**
         * Method addProtocolHeader.
         * @param name
         * @param value
         */
        void addProtocolHeader(String name, String value);
	
        /**
         * Method getProtocolHeaders.
         * @return Map
         */
        System.Collections.Hashtable getProtocolHeaders();
    }
}
