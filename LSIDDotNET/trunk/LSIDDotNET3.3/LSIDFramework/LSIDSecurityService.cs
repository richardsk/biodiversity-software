using System;

namespace LSIDFramework
{
    /**
     * 
     * This class provides an interface for an LSID Security Service.  The implementor must implement 
     * this interface if they want access control for their data
     * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
     * 
     */

    public interface LSIDSecurityService : LSIDService 
    {

        /**
        * authenticate the current requests.  Note, an exception should only be raised if an error
        * occurs during an authentication.  Failed authentication caused by bad credentials should not raise an exception.
        * An example use exception case would be if the service could not connect to the LDAP server for authentication.
        * @param LSIDRequestContext the request
        * @return AuthenticationResponse the response.
        */
        AuthenticationResponse authenticate(LSIDRequestContext req) ;

    }
}
