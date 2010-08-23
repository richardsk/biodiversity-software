using System;

namespace LSIDClient
{
    /**
     * 
     * This exception gets thrown when an internal cache error occurs
     *
     * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
     *  
     */
    public class LSIDCacheException : LSIDException 
    {

        /**
        * Create a new exception.
        * @param Exception the original exception
        * @param String the message
        */
        public LSIDCacheException(Exception e, String msg)  : 
            base(e, INTERNAL_PROCESSING_ERROR, "Internal cache error: " + msg)
        {
        }
	
        /**
         * Create a new exception
         * @param String the message
         */
        public LSIDCacheException(String msg) : 
            base(INTERNAL_PROCESSING_ERROR, "Internal cache error: " + msg)
        {
        }

    }

}
