using System;

namespace LSIDFramework
{
	/**
	 * 
	 * Response from authentication request
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class AuthenticationResponse 
    {
        public Object ResponseData;
        public Boolean Success;
        public String Realm;
	
        public AuthenticationResponse()
        {
        }

        /**
         * Create a new response
         * @param boolean if authentication succeeded
         * @param Object any data associated with the response
         * @param String the authentication realm
         */
        public AuthenticationResponse(Boolean success, Object response, String realm) 
        {
            this.Success = success;
            this.ResponseData = response;	
            this.Realm = realm;
        }

        /**
         * Create a new response
         * @param boolean if authentication succeeded
         * @param Object any data associated with the response
         */
        public AuthenticationResponse(Boolean success, Object response) 
        {
            this.Success = success;
            this.ResponseData = response;	
        }
	
        /**
         * Create a new response
         * @param boolean if authentication succeeded
         * @param String the authentication realm
         */
        public AuthenticationResponse(Boolean success, String realm) 
        {
            this.Success = success;
            this.Realm = realm;
        }

        /**
         * Create a new response
         * @param Object any data associated with the response
         * @param String the authentication realm
         */
        public AuthenticationResponse(Boolean success) 
        {
            this.Success = success;
            this.ResponseData = null;	
        }
	
    }

}
