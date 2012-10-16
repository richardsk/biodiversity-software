using System;

using LSIDFramework;
using LSIDClient;

namespace LSIDAuthoritySample
{
	/// <summary>
	/// Summary description for SampleAuthentication.
	/// </summary>
	public class SampleAuthentication : LSIDSecurityService
	{
	
		#region LSIDSecurityService Members

		public AuthenticationResponse authenticate(LSIDRequestContext req)
		{
			if (req.Credentials != null && 
				req.Credentials.getProperty(LSIDCredentials.BASICUSERNAME).ToString() == "sampleUser" &&
				req.Credentials.getProperty(LSIDCredentials.BASICPASSWORD).ToString() == "samplePass")
			{
				return new AuthenticationResponse(true);				
			}

			return new AuthenticationResponse(false);
		}

		#endregion

		#region LSIDService Members

		public void initService(LSIDServiceConfig config)
		{
			// TODO:  Add SampleAuthentication.initService implementation
		}

		#endregion
	}
}
