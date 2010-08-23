using System;

namespace LSIDClient
{
	/**
	 * 
	 * This class contains String constants used in building and parsing SOAP Envelopes
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class SoapConstants
    {	
            // operation names
            public static readonly String GET_METADATA_OP_NAME = "getMetadata";
            public static readonly String GET_WSDL_OP_NAME = "getAvailableServices";
            public static readonly String NOTIFY_FOREIGN_AUTHORITY_OP_NAME = "notifyForeignAuthority";
            public static readonly String REVOKE_NOTIFICATION_FOREIGN_AUTHORITY_OP_NAME = "revokeNotificationForeignAuthority";
            public static readonly String GET_DATA_OP_NAME = "getData";
            public static readonly String GET_DATA_BY_RANGE_OP_NAME = "getDataByRange";
            public static readonly String ASSIGN_LSID_OP_NAME = "assignLSID";
            public static readonly String ASSIGN_LSID_FROM_LIST_OP_NAME = "assignLSIDFromList";
            public static readonly String GET_LSID_PATTERN_OP_NAME = "getLSIDPattern";
            public static readonly String GET_LSID_PATTERN_FROM_LIST_OP_NAME = "getLSIDPatternFromList";
            public static readonly String ASSIGN_LSID_FOR_NEW_REVISION_OP_NAME = "assignLSIDForNewRevision";
            public static readonly String GET_ALLOWED_PROPERTY_NAMES_OP_NAME = "getAllowedPropertyNames";
            public static readonly String GET_AUTHORITIES_AND_NAMESPACES_OP_NAME = "getAuthoritiesAndNamespaces";
            public static readonly String OPERATION_RESPONSE_SUFFIX = "Response";
	
            // Envelope element names
	
            // fault detail elements
            public static readonly String ERROR_CODE_ELT = "errorcode";
            public static readonly String DESCRIPTION_ELT = "description";
	
            // header elements
            public static readonly String EXPIRES_HEADER_ELT = "expires";
	
            // soap fault codes
            public static readonly String SERVER = "Server";
            public static readonly String CLIENT = "Client";
	
	
	}
}
