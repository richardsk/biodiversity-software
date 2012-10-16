using System;

namespace LSIDClient
{
    public class ExpiringResponse 
    {
	
        public Object value;
        public DateTime expires;	

        public ExpiringResponse()
        {
        }
           
        /**
         * Construct a new response
         * @param Object the value of the response
         * @param Date the expiration date/time of the response
         */
        public ExpiringResponse(Object value, DateTime expires) 
        {
            this.value = value;
            this.expires = expires;
        }
	
        /**
         * Construct a new response
         * @param Object the value of the response
         * @param long the expiration date/time in millis of the response
         */
        public ExpiringResponse(Object value, long expires) 
        {
            this.value = value;
            if (expires > 0)
                this.expires = new DateTime(expires);
        }


        /**
         * Returns the expires.
         * @return Date a value of null indicates that this response never expires
         */
        public DateTime getExpires() 
        {
            return expires;
        }

        /**
         * Returns the value.
         * @return Object
         */
        public Object getValue() 
        {
            return value;
        }

        /**
         * Sets the value.
         * @param value The value to set
         */
        public void setValue(Object value) 
        {
            this.value = value;
        }

    }

}
