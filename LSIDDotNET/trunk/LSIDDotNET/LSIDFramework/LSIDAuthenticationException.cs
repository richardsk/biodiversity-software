using System;

namespace LSIDFramework
{
    /**
     * 
     * Exception for errors during LSID authentication.  Note, this exception should only be raised if an error
     * occurs during an authentication.  Failed authentication caused by bad credentials should not raise this exception.
     * An example use of this exception would be if the service could not connect to the LDAP server for authentication.
     * 
     * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
     * 
     */
    public class LSIDAuthenticationException : LSIDServerException 
    {

        /**
        * Constructor for LSIDAuthenticationException.
        * @param e
        * @param description
        */
        public LSIDAuthenticationException(Exception e, String description) : base(e,AUTHENTICATION_ERROR,description)
        {
        }

        /**
         * Constructor for LSIDAuthenticationException.
         * @param description
         */
        public LSIDAuthenticationException(String description) : base(AUTHENTICATION_ERROR,description)
        {
        }

    }
}
