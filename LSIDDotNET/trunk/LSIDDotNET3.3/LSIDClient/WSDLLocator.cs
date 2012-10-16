using System;
using System.IO;
using System.Reflection;

namespace LSIDClient
{
	public class WSDLLocator
	{
		public WSDLLocator()
		{
		}

		public static Stream GetWSDLFile(string location)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			//string[] n = asm.GetManifestResourceNames();
			return asm.GetManifestResourceStream("LSIDClient.WSDL." + location);
		}
	}
}
