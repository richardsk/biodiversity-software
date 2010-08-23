using System;
using System.Collections;

namespace LSIDClient
{
    public interface LSIDMetadataFactory 
    {

        /**
         * Create a new instance.  May make use of properties set by <code>setProperties()</code>.  
         * @return LSIDMetadata the new instance.
         */
        LSIDMetadata createInstance(); 
	
        /**
         * Set the properties that the factory should use to create instances.
         */
        void setProperties(Hashtable properties); 

    }
}
