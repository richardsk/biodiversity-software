using System;

using LSIDClient;

namespace LSIDFramework
{
    /**
     * 
     * LSIDAssingingService provides an interface to an LSID authority implementation.  
     * Implementations of LSIDAssigningService must be threadsafe. 
     * 
     * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
     */
    public interface LSIDAssigningService : LSIDService 
    {

        /**
        * returns a full LSID for a data entity that has the properties passed in the 
        * property_list (like an object type and the attributes belonging to the data entity).
        * This LSID has authority and namespace as requested by caller. Service returns null 
        * if it cannot or does not want to name the object. property_list is an array of 
        * name/value pairs (both of type string).
        * 
        * @param authority
        * @param namespace
        * @param properies
        * @return
        * @throws LSIDException
        */
        LSID assignLSID(String authority, String ns, Properties properties) ;

        /**
         * similar to the assignLSID, only the caller is suggesting the identifier itself. 
         * "authority" and "namespace" parts for all suggestions should be the same. The 
         * service can return one LSID from the list, something different (but with the same
         * authority and namespace) or raise an exception.
         * 
         * @param properties
         * @param suggested
         * @return
         * @throws LSIDException
         */
        LSID assignLSIDFromList(Properties properties, LSID[] suggested) ;

        /**
         * returns a prefix of an LSID such that the caller can use that as a template for
         * constructing LSIDs and it is still guaranteed that these will be globally unique
         * as far as the caller takes care not to reuse the same LSID twice locally.
         * 
         * @param authority
         * @param namespace
         * @param properties
         * @return
         * @throws LSIDException
         */
        String getLSIDPattern(String authority, String ns, Properties properties) ;

        /**
         * combines assignLSIDFromList and getLSIDPattern.
         * 
         * @param properties
         * @param suggested
         * @return
         * @throws LSIDException
         */
        String getLSIDPatternFromList(Properties properties, String[] suggested) ;

        /**
         * The caller sends in an identifier for an existing object and expects a new
         * identifier with a different revision (for naming a new version of some object).
         * 
         * @param previousIdentifier
         * @return
         * @throws LSIDException
         */
        LSID assignLSIDForNewRevision(LSID previousIdentifier) ;

        /**
         * returns an array of names of properties that the LSID Assigning Service can use
         * when passed in methods assignLSID, assignLSIDFromList, and getLSIDPattern.
         * @return
         */
        String[] getAllowedPropertyNames();

        /**
         * returns an array of pairs of Strings where the first item is the authority part
         * that the service can assign names for, and the second String is valid namespace
         * in that authority.
         * 
         * @return
         */
        String[][] getAuthoritiesAndNamespaces();

    }
}
