using System;
using System.IO;

namespace LSIDFramework
{
    /**
     * 
     * This class provides an interface for an LSID Data Service.  The implementor must implement 
     * the methods which retrieves the data for an LSID
     * 
     * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
     * 
     */
    public interface LSIDDataService : LSIDService 
    {
	
        /**
        * Get the data associated with the LSID
        * @param LSIDRequestContext req
        * @return InputStream an input stream to the data, null if no data exists
        */
        Stream getData(LSIDRequestContext req) ;
	
        /**
         * Get the data range associated with the LSID
         * @param LSIDRequestContext req
         * @param int start the 0 offset of the first byte to retrieve
         * @param int length the number of bytes to retrieve
         * @return InputStream an input stream to the data, null if no data exists
         */
        Stream getDataByRange(LSIDRequestContext req, int start, int length) ;

    }
}
