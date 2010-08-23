using System;

namespace LSIDFramework
{
	/**
	 * 
	 * Encapsulates an exception during LSID service configuration.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDConfigurationException : LSIDServerException 
    {


        /**
        * Constructor for LSIDConfigurationException.
        * @param Exception the root cause
        * @param String the description
        */
        public LSIDConfigurationException(Exception e, String description) : 
            base(e,SERVER_CONFIGURATION_ERROR,description)
        {
        }

        /**
         * Constructor for LSIDConfigurationException.
         * @param String the description
         */
        public LSIDConfigurationException(String description) :
            base(SERVER_CONFIGURATION_ERROR,description)
        {
        }

    }
}
