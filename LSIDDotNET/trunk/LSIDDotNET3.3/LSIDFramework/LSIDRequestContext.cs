using System;
using System.Collections;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 * 
	 * bag for request
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDRequestContext 
    {
        public LSID Lsid;
        public String Hint;
        public LSIDCredentials Credentials;
        public AuthenticationResponse AuthResponse;
        public String ReqUrl;
        
        private Hashtable headersTable = new Hashtable();
        private SerializableDictionary headers;
        	
        public LSIDRequestContext()
        {
            headers = new SerializableDictionary(headersTable);
        }

        /**
         * Method addProtocolHeader.
         * @param name
         * @param value
         */
        public void addProtocolHeader(String name, String value)
        {   
            headersTable.Add(name,value);
        }
	
        /**
         * Method getProtocolHeaders.
         * @return Map
         */
        public Hashtable getProtocalHeaders() 
        {
            return headersTable;
        }	
	
    }
}
