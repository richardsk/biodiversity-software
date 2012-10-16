using System;

namespace LSIDClient
{
    public interface LSIDMetadata 
    {

        /**
         * Import metadata from the given input stream
         * @param MetadataResponse the metadata to add
         */
        void addMetadata(MetadataResponse metadata); 
	
        /**
         * Get the format of data for the given lsid.
         * @param LSID the lsid whose format to retrieve.  Implementations should populate
         * the format field of the lsid.
         * @return String the format of the lsid.  Returns null if the given lsid has no format
         */
        String getFormat(LSID lsid); 
	
        /**
         * Get the concrete instances for the given abstract lsid
         * @param LSID the abstract lsid whose instances to get.  Implementations should populate
         * the instances field of the lsid
         * @return LSID[] the concrete lsids. Implementations should populate the abstract field of each of these
         * lsids with the parameter lsid.
         */
        LSID[] getInstances(LSID lsid); 
	
        /**
         * Get the abstract lsid for the given concrete lsid
         * @param LSID the concrete lsid. Implementations should populate the abstract field of this lsid.
         * @return LSID the abstract lsid, null if the given lsid has no abstract 
         */
        LSID getAbstract(LSID lsid); 
	
        /**
         * Get the type of the given lsid
         * @param LSID the lsid whose type to get.  Implementations should populate the type field of this lsid.
         * @return String the type
         */
        String getType(LSID lsid); 
	
        /**
         * Get a list of foreign authorities
         * @param LSID the lsid whose foreign authorities to get.
         * @return LSIDAuthority[] the foreign authorities.
         */
        LSIDAuthority[] getForeignAuthorities(LSID lsid); 

    }
}
