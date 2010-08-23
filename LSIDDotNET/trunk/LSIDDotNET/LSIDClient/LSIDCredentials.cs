using System;
using System.Collections;


namespace LSIDClient
{
	/**
	 * 
	 * This class provides an LSID Security Service property bag.  
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDCredentials 
    {

        public readonly static String X509CERTIFICATE = "X509Certificate";

        public readonly static String BASICUSERNAME = "basicUsername";
        public readonly static String BASICPASSWORD = "basicPassword";

        public readonly static String WSSEPASSWORD = "wssePassword";
        public readonly static String WSSEUSERNAME = "wsseUsername";

        private Hashtable bag;

        /**
         * Create a new credentials object to set properties dynamically.
         */
        public LSIDCredentials() 
        {
            bag = new Hashtable();
        }

        /**
         * Create a new credentials object using the configured credentials for the given port
         * @param LSIDPort the port to use
         */
        public LSIDCredentials(LSIDPort port) 
        {
            this.bag = LSIDCredentialConfig.getCredentials(port);
        }

        /**
         * Create a new credentials object using the configured credentials for the given LSID
         * @param LSID the lsid to use
         */
        public LSIDCredentials(LSID lsid) 
        {
            this.bag = LSIDCredentialConfig.getCredentials(lsid);
        }
	
        /**
         * Create a new credentials object using the configured credentials for the given LSIDAuthority
         * @param LSIDAuthority the lsid authority to use
         */
        public LSIDCredentials(LSIDAuthority lsidauth) 
        {
            this.bag = LSIDCredentialConfig.getCredentials(lsidauth);
        }

        /**
         * Get a property from the credentials bag.  Valid properties are the String constants in this class
         * @param propertyName
         * @return Object
         */
        public Object getProperty(String propertyName) 
        {
            return bag[propertyName];
        }

        /**
         * Enumerate the property names
         * @return Enumerations the names
         */
        public Enumeration keys() 
        {
            return new IColEnumeration(bag.Keys);
        }

        /**
         * Set a property in the credentials bag. Valid properties are the String constants in this class
         * @param propertyName
         * @param value
         */
        public void setProperty(String propertyName, Object value) 
        {
            bag[propertyName] = value;
        }

        /**
         * Format the credentials object to a human-readable string
         * @return String the formatted credentials.
         */
        public override String ToString() 
        {
            String contains = "";
            Enumeration keys = new IColEnumeration(bag.Keys);
            while (keys.hasMoreElements()) 
            {
                String key = (String) keys.nextElement();
                contains += key + ":" + bag[key] + " ";

            }
            return contains;
        }

    }
}
