using System;
using System.IO;

namespace LSIDClient
{
	/**
	 * 
	 * Simple object to hold the response to an HTTP get
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class HTTPResponse 
    {
	
        private Stream data;
        private String contentType;
        private DateTime expiration;
	
        /**
         * construct a new HTTPResponse
         * @param InputStream the data
         * @param String the contentType
         * @param Date the value of header Expires
         */
        public HTTPResponse(Stream data, String contentType, DateTime expiration) 
        {
            this.data = data;
            this.contentType = contentType;
            this.expiration = expiration;	
        }
	
        /**
         * Returns the contentType.
         * @return String
         */
        public String getContentType() 
        {
            return contentType;
        }

        /**
         * Returns the data.
         * @return InputStream
         */
        public Stream getData() 
        {
            return data;
        }

        /**
         * Returns the expiration.
         * @return Date
         */
        public DateTime getExpiration() 
        {
            return expiration;
        }

    }
}
