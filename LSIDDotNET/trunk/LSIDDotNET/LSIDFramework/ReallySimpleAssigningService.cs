using System;
using System.Collections;

using LSIDClient;

namespace LSIDFramework
{
	/**
	 * 
	 * This is a simple assigning service
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
	public class ReallySimpleAssigningService : LSIDAssigningService 
	{
		/* 
		 * @see LSIDAssigningService#assignLSID(String, String, Properties)
		 */
		public LSID assignLSID(String authority, String ns, Properties properties) 
		{
			String lsid = "urn:lsid:" + authority + ":" + ns + ":" + new System.Random().Next().ToString();
			IEnumerator e = properties.enumerateProperty(); 
			while(e.MoveNext())
			{
				Property p = (Property)e.Current;
				lsid += "-" + p.getName() + "_" + p.getValue();
			}
			return new LSID(lsid);
		}

		/* 
		 * @see LSIDAssigningService#assignLSIDFromList(Properties, LSID[])
		 */
		public LSID assignLSIDFromList(Properties properties, LSID[] suggested) 
		{
			if(suggested.Length > 0) 
			{
				return suggested[0];
			} 
			return new LSID("urn:lsid:unknown:unknown:" + new System.Random().Next().ToString());
		}

		/* 
		 * @see LSIDAssigningService#getLSIDPattern(String, String, Properties)
		 */
		public String getLSIDPattern(String authority, String ns, Properties properties) 
		{
			return "prefix";
		}

		/* 
		 * @see LSIDAssigningService#getLSIDPatternFromList(Properties, String[])
		 */
		public String getLSIDPatternFromList(Properties properties, String[] suggested) 
		{
			String pref = "prefix";
			IEnumerator e = properties.enumerateProperty(); 
			while(e.MoveNext())
			{
				Property p = (Property)e.Current;
				pref += "-" + p.getName() + "_" + p.getValue();
			}
			if(suggested.Length > 0) 
			{
				return suggested[0] + pref;
			}
			return pref;
		}

		/* 
		 * @see LSIDAssigningService#assignLSIDForNewRevision(LSID)
		 */
		public LSID assignLSIDForNewRevision(LSID previousIdentifier) 
		{
			return new LSID("urn:lsid:" + previousIdentifier.Authority + 
				":" + previousIdentifier.Namespace+ ":" 
				+ previousIdentifier.Object+ new Random().Next().ToString());
		}

		/* 
		 * @see LSIDAssigningService#getAllowedPropertyNames()
		 */
		public String[] getAllowedPropertyNames() 
		{
			String[] s = {"really","simple","properties"};
			return s;
		}

		/* 
		 * @see LSIDAssigningService#getAuthoritiesAndNamespaces()
		 */
		public String[][] getAuthoritiesAndNamespaces() 
		{
			String[][] s = new String[][]{new String[]{"authority","ns"}, new String[]{"authority2","ns1"}};
			return s;
		}

		/* 
		 * @see LSIDService#initService(LSIDServiceConfig)
		 */
		public void initService(LSIDServiceConfig config) 
		{
		}

	}
}