using System;
using System.Xml;

namespace LSIDClient
{
	/**
	 * 
	 * String contants for LSID Resolution Service WSDL documents
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class WSDLConstants 
    {
	
        // protocols/prefixes
        public static readonly String HTTP = "http";
        public static readonly String FTP = "ftp";
        public static readonly String SOAP = "soap";
        public static readonly String FILE = "file";
	
        // prefixes
        public static readonly String TNS = "tns"; // target namespace
        public static readonly String DSB = "dsb"; // data soap binding
        public static readonly String DHB = "dhb"; // data http binding
        public static readonly String DFB = "dfb"; // data ftp binding
        public static readonly String DLB = "dlb"; // data file binding
        public static readonly String ASB = "asb"; // authority soap binding
        public static readonly String AHB = "ahb"; // authority http binding
        public static readonly String ANB = "anb"; // assign service soap binding
	
        //static readonly String OMG = "omg";
	
        // standard namespaces	
        protected static readonly String HTTP_NS_URI = "http://schemas.xmlsoap.org/wsdl/http/";
        public static readonly String FTP_NS_URI = "http://www.ibm.com/wsdl/ftp/";
        public static readonly String FILE_NS_URI = "http://www.ibm.com/wsdl/file/";
        protected static readonly String SOAP_NS_URI = "http://schemas.xmlsoap.org/wsdl/soap/";
        protected static readonly String WSDL_NS_URI = "http://schemas.xmlsoap.org/wsdl/";
        protected static readonly String MIME_NS_URI = "http://schemas.xmlsoap.org/wsdl/mime/";

        public static readonly String CLIENT_CONFIG_NS_URI = "http://www.ibm.com/LSID/Standard/ClientConfiguration";
	
        // omg namespaces, the soap binding namespaces are the namespaces of the top level body elements in soap request and responses
        public static readonly String OMG_ASSIGNING_PORT_TYPES_WSDL_SCHEMA_NS_URI = "http://www.omg.org/LSID/2003/Standard/Assigning/WSDL/SchemaTypes";
        public static readonly String OMG_ASSIGNING_PORT_TYPES_WSDL_NS_URI = "http://www.omg.org/LSID/2003/Standard/Assigning/WSDL";
        public static readonly String OMG_ASSIGNING_SOAP_BINDINGS_WSDL_NS_URI ="http://www.omg.org/LSID/2003/Assigning/StandardSOAPBinding";
        public static readonly String OMG_LSID_PORT_TYPES_WSDL_NS_URI = "http://www.omg.org/LSID/2003/Standard/WSDL";
        public static readonly String OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI = "http://www.omg.org/LSID/2003/DataServiceSOAPBindings";
        public static readonly String OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI = "http://www.omg.org/LSID/2003/DataServiceHTTPBindings";
        public static readonly String OMG_DATA_FTP_BINDINGS_WSDL_NS_URI = "http://www.omg.org/LSID/2003/DataServiceFTPBindings";
        public static readonly String OMG_DATA_FILE_BINDINGS_WSDL_NS_URI = "http://www.omg.org/LSID/2003/DataServiceFileBindings";
        public static readonly String OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI = "http://www.omg.org/LSID/2003/AuthorityServiceSOAPBindings";
        public static readonly String OMG_AUTHORITY_HTTP_BINDINGS_WSDL_NS_URI = "http://www.omg.org/LSID/2003/AuthorityServiceHTTPBindings";

        // port types
        public static readonly String DATA_PORT_TYPE = "LSIDDataServicePortType";
        public static readonly String METADATA_PORT_TYPE = "LSIDMetadataServicePortType";
        public static readonly String AUTHORITY_PORT_TYPE = "LSIDAuthorityServicePortType";
    
        // binding XmlQualifiedNames
        public static readonly XmlQualifiedName DATA_SOAP_BINDING = new XmlQualifiedName("LSIDDataSOAPBinding", OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName DATA_HTTP_BINDING = new XmlQualifiedName("LSIDDataHTTPBinding", OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName DATA_HTTP_BINDING_DIRECT = new XmlQualifiedName("LSIDDataHTTPBindingDirect", OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName DATA_FTP_BINDING = new XmlQualifiedName("LSIDDataFTPBinding", OMG_DATA_FTP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName DATA_FILE_BINDING = new XmlQualifiedName("LSIDDataFileBinding", OMG_DATA_FILE_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName METADATA_SOAP_BINDING = new XmlQualifiedName("LSIDMetadataSOAPBinding", OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI); 
        public static readonly XmlQualifiedName METADATA_HTTP_BINDING = new XmlQualifiedName("LSIDMetadataHTTPBinding", OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName METADATA_HTTP_BINDING_DIRECT = new XmlQualifiedName("LSIDMetadataHTTPBindingDirect", OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName METADATA_FTP_BINDING = new XmlQualifiedName("LSIDMetadataFTPBinding", OMG_DATA_FTP_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName METADATA_FILE_BINDING = new XmlQualifiedName("LSIDMetadataFileBinding", OMG_DATA_FILE_BINDINGS_WSDL_NS_URI);
        public static readonly XmlQualifiedName AUTHORITY_SOAP_BINDING = new XmlQualifiedName("LSIDAuthoritySOAPBinding", OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI); 
        public static readonly XmlQualifiedName AUTHORITY_HTTP_BINDING = new XmlQualifiedName("LSIDAuthorityHTTPBinding", OMG_AUTHORITY_HTTP_BINDINGS_WSDL_NS_URI); 
    
        // mime extension constants
        /* no longer need (we think)
        static readonly String MIME_PREFIX = "mime";
        static readonly String BODY_MIME_PART = "body";
        static readonly String OCTET_STREAM_MIME_TYPE = "application/octet-stream";
        */
   	
        // service constants
        protected static readonly String SERVICE_NAME = "LSIDService";
   	
        // WSDL import locations and resource filenames. The location is the location string in documents that will import it.
        protected static readonly String STANDARD_PORT_TYPES_LOCATION = "LSIDPortTypes.wsdl";
        protected static readonly String STANDARD_PORT_TYPES_FILE = "LSIDPortTypes.wsdl";
        protected static readonly String DATA_SOAP_BINDINGS_LOCATION = "LSIDDataServiceSOAPBindings.wsdl";
        protected static readonly String DATA_SOAP_BINDINGS_FILE = "LSIDDataServiceSOAPBindings.wsdl";
        protected static readonly String DATA_HTTP_BINDINGS_LOCATION = "LSIDDataServiceHTTPBindings.wsdl";
        protected static readonly String DATA_HTTP_BINDINGS_FILE = "LSIDDataServiceHTTPBindings.wsdl";
        protected static readonly String DATA_FTP_BINDINGS_LOCATION = "LSIDDataServiceFTPBindings.wsdl";
        protected static readonly String DATA_FTP_BINDINGS_FILE = "LSIDDataServiceFTPBindings.wsdl";
        protected static readonly String DATA_FILE_BINDINGS_LOCATION = "LSIDDataServiceFileBindings.wsdl";
        protected static readonly String DATA_FILE_BINDINGS_FILE = "LSIDDataServiceFileBindings.wsdl";
        protected static readonly String AUTHORITY_SOAP_BINDINGS_LOCATION = "LSIDAuthorityServiceSOAPBindings.wsdl";
        protected static readonly String AUTHORITY_SOAP_BINDINGS_FILE = "LSIDAuthorityServiceSOAPBindings.wsdl";
        protected static readonly String AUTHORITY_HTTP_BINDINGS_LOCATION = "LSIDAuthorityServiceHTTPBindings.wsdl";
        protected static readonly String AUTHORITY_HTTP_BINDINGS_FILE = "LSIDAuthorityServiceHTTPBindings.wsdl";

        // part names, these will be http get parameter name and soap parameter element names
        public static readonly String EXPIRATION_PART = "expiration";
        public static readonly String FORMAT_PART = "format";
        public static readonly String LSID_PART = "lsid";
        public static readonly String START_PART = "start";
        public static readonly String LENGTH_PART = "length";
        public static readonly String ACCEPTED_FORMATS_PART = "acceptedFormats";
        public static readonly String AUTHORITY_NAME_PART = "authorityName";
        public static readonly String AUTHORITY_PART = "authority";
        public static readonly String NAMESPACE_PART = "namespace";
        public static readonly String PROPERTY_LIST_PART = "propertyList";
        public static readonly String LSID_PATTERN_PART = "LSIDPattern";
        public static readonly String PREVIOUS_LSID_PART = "previousLSID";
        public static readonly String SUGGESTED_LSIDS_PART = "suggestedLSIDs";
        public static readonly String SUGGESTED_LSID_PATTERNS_PART = "suggestedLSIDPatterns";
        public static readonly String PROPERTY_NAMES_PART = "propertyNames";
        public static readonly String AUTHORITY_AND_NAMESPACES_PART = "authorityAndNamespaces";
	
        // element names
        public static readonly String PROPERTY_ELT = "property";
        public static readonly String NAME_ELT = "name";
        public static readonly String VALUE_ELT = "value";
        public static readonly String LSID_ELT = "LSID";
        public static readonly String LSID_PATTERN_ELT = "LSIDPattern";
        public static readonly String AUTHORITY_NAMESPACE_ELT = "authorityNamespace";
        public static readonly String AUTHORITY_ELT = "authority";
        public static readonly String NAMESPACE_ELT = "namespace";
        public static readonly String PROPERTY_NAME_ELT = "propertyName";
	

    }
}
