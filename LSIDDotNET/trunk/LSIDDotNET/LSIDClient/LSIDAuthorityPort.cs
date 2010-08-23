using System;

namespace LSIDClient
{
	
	/**
	 * 
	 * This interface provides the details of an LSID authority port defined in a WSDL.
	 * Note, the values returned by the methods are NOT the same as the components of a URL for the data. For example, 
	 * if the protocol is "soap", "http" could be part of the URL.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 * */
	public interface LSIDAuthorityPort : LSIDStandardPort
    {
    }
}
