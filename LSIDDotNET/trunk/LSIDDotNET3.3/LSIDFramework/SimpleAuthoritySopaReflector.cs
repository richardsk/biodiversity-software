using System;
using System.Web.Services.Description;

namespace LSIDFramework
{
	/// <summary>
	/// Summary description for SimpleAuthoritySopaExtension.
	/// </summary>
	public class SimpleAuthoritySopaReflector : SoapExtensionReflector
	{
        public override void ReflectMethod()
        {
//            foreach (Port p in ReflectionContext.Service.Ports)
//            {
//                if (p.Name == "LSIDAuthoritySOAPBinding") p.Name = "LSIDAuthorityServicePortType";
//            }
        }
	}
}
