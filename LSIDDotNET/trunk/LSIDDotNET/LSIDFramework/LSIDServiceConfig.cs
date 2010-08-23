using System;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 * 
	 * This interface provides configuration to LSID services
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDServiceConfig 
    {
        public LSIDServiceConfig()
        {
        }

        /**
         * Get the names of all configuration properties
         * @return Enumeration the property names
         */
        public Enumeration getPropertyNames()
        {
            return null;
        }
	
        /**
         * Get the value of a given property
         * @param String the name of the property
         * @return String the value of the property
         */
        public String getProperty(String property)
        {
            return null;
        }
    }
}
