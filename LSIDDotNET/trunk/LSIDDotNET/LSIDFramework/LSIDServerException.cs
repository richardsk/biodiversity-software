using System;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 *
	 * Encapsulates an exception in an LSID Service.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDServerException : LSIDException 
    {

        /**
        * Construct a new LSIDServerException; store the parent exception
        * @param Exception The Exception which caused the LSIDServerException to be raised.
        * @param int The error code
        * @param String The error description.
        */
        public LSIDServerException(Exception e, int errorCode, String description) :
            base(errorCode,description)
        {
        }
	
        /**
         * Construct a new exception
         * @param int the error code
         * @param String The exception message.
         */
        public LSIDServerException(int errorCode, String description) :
            base(errorCode,description)
        {
        }
	
        /**
         * Construct a new LSIDServerException; store the parent exception, set error code to INTERNAL_PROCESSING_ERROR
         * @param Exception The Exception which caused the LSIDServerException to be raised.
         * @param String The error description.
         */
        public LSIDServerException(Exception e, String description) :
            base(e,description)
        {
        }
	
        /**
         * Construct a new exception, set error code to INTERNAL_PROCESSING_ERROR
         * @param String The exception message.
         */
        public LSIDServerException(String description) :
            base(description)
        {
        }

	
	}
}
