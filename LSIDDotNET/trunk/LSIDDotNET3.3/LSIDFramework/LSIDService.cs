using System;

namespace LSIDFramework
{
	/**
	 *
	 * Base interface for all LSID services.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public interface LSIDService
	{
        void initService(LSIDServiceConfig config); 
	}
}
