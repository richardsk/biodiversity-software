using System;

namespace LSIDFramework
{
	/**
	 *
	 * Constants pertaining to LSID service configuration, (Resolution Service Description Language, RSDL)
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class ServiceConfigurationConstants 
    {
	
        // servlet paramater to specify location of configuration
        public static  String RSDL_LOCATION = "rsdl-location";
        public static  String LSID_CLIENT_HOME = "LSID_CLIENT_HOME";
	
        // servlet context parameters for storing service registries.
        public static  String AUTHENTICATION_SERVICE_REGISTRY = "authenticationServiceRegistry";
        public static  String AUTHORITY_SERVICE_IMPLEMENTATION_REGISTRY = "authserviceImplRegistry";
        public static  String DATA_SERVICE_IMPLEMENTATION_REGISTRY = "dataserviceImplRegistry";
        public static  String METADATA_SERVICE_IMPLEMENTATION_REGISTRY = "metaserviceImplRegistry";
        public static  String ASSIGNING_SERVICE_IMPLEMENTATION_REGISTRY = "assnserviceImplRegistry";

        // namespaces
        public static  String RSDL_NS_URI = "http://www.ibm.com/LSID/Standard/rsdl";
	
        // prefixes
        public static  String RSDL_PREFIX = "rsdl";
	
        // elements
        public static  String COMPONENT_HANDLERS_ELT = "component-handlers";
        public static  String COMPONENT_HANDLER_ELT = "component-handler";
        public static  String MAPS_ELT = "maps";
        public static  String MAP_ELT = "map";
        public static  String PATTERN_ELT = "pattern";
        public static  String SERVICES_ELT = "services";
        public static  String SERVICE_ELT = "service";
        public static  String COMPONENTS_ELT = "components";
        public static  String PARAMS_ELT = "params";
        public static  String PARAM_ELT = "param";
	
        // attributes
        public static  String NAME_ATTR = "name";
        public static  String AUTH_ATTR = "auth";
        public static  String NS_ATTR = "ns";
        public static  String MAP_ATTR = "map";
        public static  String TYPE_ATTR = "type";
        public static  String CLASSNAME_ATTR = "classname";
        public static  String LOCATION_ATTR = "location";
        public static  String ASSEMBLY_NAME = "assemblyname";
	
        // component elements
        public static  String AUTH_COMP = "auth";
        public static  String META_COMP = "meta";
        public static  String DATA_COMP = "data";
        public static  String AUTHENTICATION_COMP = "authentication";
        public static  String ASSIGNING_COMP = "assn";
	
        // component types
        public static  String TYPE_CLASS = "class";
        public static  String TYPE_ASDL = "asdl";
        public static  String TYPE_MSDL = "msdl";
        public static  String TYPE_CACHING = "caching";
	
        // locations
        public static  String INLINE = "inline";
        public static  String FILE = "file";
        public static  String URL = "url";
	

    }

}
