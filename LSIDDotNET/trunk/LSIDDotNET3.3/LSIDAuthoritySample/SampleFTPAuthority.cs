using System;

using LSIDClient;
using LSIDFramework;

namespace LSIDAuthoritySample
{
	/// <summary>
	/// Summary description for SampleFTPAuthority.
	/// </summary>
	public class SampleFTPAuthority : SimpleAuthority
	{
	
		protected override LSIDMetadataPort[] getMetadataLocations(LSID lsid, string url)
		{			
			return new LSIDMetadataPort[]{new FTPLocation("kevin-richards", "temp/testFTP.txt")};
		}

		protected override LSIDDataPort[] getDataLocations(LSID lsid, string url)
		{
			return new LSIDDataPort[]{new FTPLocation("kevin-richards", "temp/testFTP.txt")};
		}

	}
}
