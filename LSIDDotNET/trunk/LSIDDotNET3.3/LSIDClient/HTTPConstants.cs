using System;

namespace LSIDClient
{
	/**
	 * 
	 * Constants for http related operations
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class HTTPConstants 
    {
	
        public static readonly String HTTP_DATE_FORMAT = "ddd, dd MMM yyyy HH:mm:ss Z";
        public static readonly String EXPIRES_HEADER = "Expires";
        public static readonly String HEADER_AUTHORIZATION = "Authorization";
        public static readonly String HEADER_PROGMA = "Pragma";
        public static readonly String HEADER_CACHECONTROL = "Cache-Control";
        public static readonly String NOCACHE = "no-cache";
        public static readonly String HEADER_LSID_ERROR_CODE = "LSID-Error-Code";
        public static readonly String HEADER_REQUEST_AUTHORIZATION = "WWW-Authenticate";
        public static readonly String HEADER_ACCEPT = "Accept";
        public static readonly String XML_CONTENT = "text/xml";
        public static readonly String HTML_CONTENT = "text/html";
        public static readonly String WSDL_CONTENT = "application/xml";
        public static readonly String RDF_CONTENT = "application/xml+rdf";
	
        // http get parameter names
        public static readonly String HINT = "hint";
	
        // path constants
        public static readonly String HTTP_AUTHORITY_SERVICE_PATH = "/authority";
        public static readonly String HTTP_AUTHORITY_SERVICE_NOTIFY_PATH = "/authority/notify";
        public static readonly String HTTP_AUTHORITY_SERVICE_REVOKE_PATH = "/authority/revoke";
	
    }
}
