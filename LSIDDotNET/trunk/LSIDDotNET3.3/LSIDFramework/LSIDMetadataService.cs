using System;

using LSIDClient;

namespace LSIDFramework
{/**
 *
 * This interface provides acces to an LSID metadata service.  Implementor must provide 
 * functionality for retrieving meta data
 */
    public interface LSIDMetadataService : LSIDService 
    {
	
        /**
        * Get the meta data associated with the LSID
        * @param LSIDRequestContext req
        * @param String[] a list of accepted metadata formats
        * @return MetadataResponse value contains an InputStream to the data, null if there is no meta data
        */
        MetadataResponse getMetadata(LSIDRequestContext req, String[] acceptedFormats);

    }

}
