using System;

namespace LSIDClient
{
	/**
	 *
	 * Encapsulates an exception in the LSID stack.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class LSIDException : Exception 
    {
		public static readonly string EXCEPTION_EVENT_SOURCE = "LSIDFramework";

        /**
            * STANDARD ERROR CODE: Malformed LSID - a syntactically invalid LSID was provided.
            */
        public static readonly int MALFORMED_LSID = 200;

        /**
         * STANDARD ERROR CODE: Unknown LSID - this error should be raised when a service invoked with an LSID for which it
         * will never have an answer.
         */
        public static readonly int UNKNOWN_LSID = 201;
	
        /**
         * STANDARD ERROR CODE: No Data Available - no data exists for the current LSID at the moment
         */
        public static readonly int NO_DATA_AVAILABLE = 300;
	
        /**
         * STANDARD ERROR CODE: The requested starting position of data range is not valid.
         */
        public static readonly int INVALID_RANGE = 301;
	
        /**
         * STANDARD ERROR CODE: No meta data is available for the given LSID at the time of the request
         */
        public static readonly int NO_METADATA_AVAILABLE = 400;
	
        /**
         * STANDARD ERROR CODE: No meta data is available for the given LSID in any of the client's accepted formats. 
         */
        public static readonly int NO_METADATA_AVAILABLE_FOR_FORMATS = 401;
	
        /**
         * STANDARD ERROR CODE: Internal processing error - a generic catch-all for unexpected errors.
         */
        public static readonly int INTERNAL_PROCESSING_ERROR = 500;
	
        /**
         * STANDARD ERROR CODE: Method not implemented - a method the service should implement hasn't been implemented yet.
         */
        public static readonly int METHOD_NOT_IMPLEMENTED = 501;
	
        /**
         * WS PLATFORM ERROR CODE: Server configuration error - the server has not been configured properly.
         */
        public static readonly int SERVER_CONFIGURATION_ERROR = 521;
	
        /**
         * WS PLATFORM ERROR CODE: Authentication error - used to indicate that an auth error has occured
         */
        public static readonly int AUTHENTICATION_ERROR = 700;
	
        /**
         * WS PLATFORM ERROR CODE:  Invalid message format - for example, the body of the message contained invalid XML.
         */
        public static readonly int INVALID_MESSAGE_FORMAT = 710;
	
        /**
         * WS PLATFORM ERROR CODE: Unknown method - the body contains valid XML, but the method element does not name a known method, or belongs to an unrecognized namespace.
         */
        public static readonly int UNKNOWN_METHOD = 711;
	
        /**
         * WS PLATFORM ERROR CODE: Invalid method call - a method call has the wrong number of parameters, has incorrectly named parameters, etc.
         */
        public static readonly int INVALID_METHOD_CALL = 712;
	
        private Exception innerException;
        private int errorCode;
        private String description;

        /**
         * Construct a new LSIDException; store the parent exception
         * @param Exception The Exception which caused the LSIDException to be raised.
         * @param int the error code
         * @param String The error description.
         */
        public LSIDException(Exception e, int errorCode, String description) : base(description)
        {
            this.description = description;
            this.errorCode = errorCode;
            this.innerException = e;
        }
	
        /**
         * Construct a new exception
         * @param int the error code
         * @param String the error description
         */
        public LSIDException(int errorCode, String description) : base(description)
        {
            this.description = description;	
            this.errorCode = errorCode;
        }
	
        /**
         * Construct a new LSIDException; store the parent exception, error code defaults to INTERNAL_PROCESSING_ERROR
         * @param Exception The Exception which caused the LSIDException to be raised.
         * @param String The error description.
         */
        public LSIDException(Exception e, String description) : base(description)
        {
            this.description = description;
            this.errorCode = INTERNAL_PROCESSING_ERROR;
            this.innerException = e;
        }
	
        /**
         * Construct a new exception, error code defaults to INTERNAL_PROCESSING_ERROR
         * @param int the error code
         * @param String the error description
         */
        public LSIDException(String description) : base(description)
        {
            this.description = description;	
            this.errorCode = INTERNAL_PROCESSING_ERROR;
        }
	
        /**
         * Retrieve the exception that was caught when this exception was thrown.
         * @return Exception the inner Exception
         */ 
        public Exception getInnerException() 
        {
            return innerException;
        }
	
        /**
         * Returns a string containing the errorcode and the description
         * @return the exception message
         */
        public String getMessage() 
        {
            return errorCode + " : " + description  + (innerException != null ? " Root Cause: " + innerException.GetType().ToString() + " " +  innerException.Message : ""); 
        }
	
        /**
         * Print the stack trace of the inner exception to event log
         */
        public void PrintStackTrace() 
		{
			string msg = Message + " : " + Source + " : " + StackTrace;
			WriteError(msg);
			
			//System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, msg); 		
        }
		
		public static void PrintStackTrace(Exception e) 
		{
			string msg = e.Message + " : " + e.Source + " : " + e.StackTrace;
			WriteError(msg);
		}

		public static void WriteError(string msg)
		{
			try
			{
				msg = DateTime.Now.ToString() + " : " + msg;
				String f = Global.BinDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LogFile"];
				System.IO.StreamWriter wr = System.IO.File.AppendText(f);
				wr.WriteLine(msg);
				wr.Close();
			}
			catch(Exception ex)
			{
				try
				{
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, ex.Message	); 
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, msg); 
				}
				catch(Exception )				
				{
				}
			}
		}
		
		public static void WriteError(LSIDException ex)
		{
			string msg = DateTime.Now.ToString() + " : " + ex.Message + " : " + ex.StackTrace;
			if (ex.innerException != null)
			{
				msg += " : Inner Exception : " + ex.innerException.Message + " : " + ex.innerException.StackTrace; 
			}

			try
			{
				String f = Global.BinDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LogFile"];
				System.IO.StreamWriter wr = System.IO.File.AppendText(f);
				wr.WriteLine(msg);
				wr.Close();
			}
			catch(Exception e)
			{
				try
				{
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, e.Message	); 
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, msg); 
				}
				catch(Exception )				
				{
				}
			}
		}
		
		public static void WriteError(Exception ex)
		{
			string msg = DateTime.Now.ToString() + " : " + ex.Message + " : " + ex.StackTrace;
			if (ex.InnerException != null)
			{
				msg += " : Inner Exception : " + ex.InnerException.Message + " : " + ex.InnerException.StackTrace; 
			}

			try
			{
				String f = Global.BinDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LogFile"];
				System.IO.StreamWriter wr = System.IO.File.AppendText(f);
				wr.WriteLine(msg);
				wr.Close();
			}
			catch(Exception e)
			{
				try
				{
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, e.Message	); 
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, msg); 
				}
				catch(Exception )				
				{
				}
			}
		}
	

        /**
         * Returns the description.
         * @return String
         */
        public String getDescription() 
        {
            return description;
        }

        /**
         * Returns the errorCode.
         * @return int
         */
        public int getErrorCode() 
        {
            return errorCode;
        }

    }
}
