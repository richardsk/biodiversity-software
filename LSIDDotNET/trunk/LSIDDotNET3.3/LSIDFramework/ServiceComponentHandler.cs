using System;
using System.Xml;
using System.Collections;

namespace LSIDFramework
{
    /**
     * Defines an interface for classes that can read a type of service component in an LSID service config file.
     * 
     */
    public interface ServiceComponentHandler 
    {

        /**
         * Process a service component element and return a fully instantiated and configured LSIDService
         * @param Element the service component elemnt
         * @param LSIDServiceConfig configuration parameters for the service
         * @return LSIDService the service.  If service cannot be loaded, should not return null, 
         * but instead should throw an LSIDConfigurationException
         */
        LSIDService loadComponent(XmlElement compElt, LSIDServiceConfig config);
	
        /**
         * Declares which services a component type can implement
         * @return String[]
         */
        ArrayList knownServices();
    }
}
