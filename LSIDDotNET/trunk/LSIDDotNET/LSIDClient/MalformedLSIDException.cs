using System;

namespace LSIDClient
{
	/**
	 * 
	 * This exception gets thrown when a malformed LSID is processed.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class MalformedLSIDException : LSIDException 
    {
        /**
        * Create a new exception
        * @param String the String that tried to be parsed as an LSID
        */
        public MalformedLSIDException(String lsid) :
            base(MALFORMED_LSID, "LSID must be of the form: urn:lsid:<AuthorityID>:<NamespaceID>:<ObjectID>:[RevisionID], got: " + lsid)
        {
        }
	
        /**
         * Create a new exception
         * @param Exception e the root cause exception
         * @param String the String that tried to be parsed as an LSID
         */
        public MalformedLSIDException(Exception e, String lsid) :
            base(e, MALFORMED_LSID, "LSID must be of the form: urn:lsid:<AuthorityID>:<NamespaceID>:<ObjectID>:[RevisionID], got: " + lsid)
        {
        }
	
    }
}
