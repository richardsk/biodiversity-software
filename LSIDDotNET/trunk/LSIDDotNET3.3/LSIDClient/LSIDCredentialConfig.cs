using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace LSIDClient
{
	
	/**
	 * 
	 * Represents the System LSID Credential config. The configuration is loaded from the file "lsid-client.credentials" which
	 * must be located in the file specified by the system property "LSID_CLIENT_HOME". An example document is include below.
	 * 
	 * <pre>
	 * &ltlsid-credentials&gt
	 *	&ltlsid&gt
	 *		&ltmap&gturn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org&lt/map&gt
	 *		&ltbasic&gt
	 *			&ltusername&gtusername&lt/username&gt
	 *			&ltpassword&gtpass&lt/password&gt
	 *		&lt/basic&gt
	 *	&lt/lsid&gt
	 *	&ltlsid&gt
	 *		&ltmap&gturn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:omim&lt/map&gt
	 *		&ltbasic&gt
	 *			&ltusername&gtusername&lt/username&gt
	 *			&ltpassword&gtpass&lt/password&gt
	 *		&lt/basic&gt
	 *	&lt/lsid&gt
	 *	&ltlsid&gt
	 *		&ltmap&gturn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:omim:601077&lt/map&gt
	 *		&ltbasic&gt
	 *			&ltusername&gtusername&lt/username&gt
	 *			&ltpassword&gtpass&lt/password&gt
	 *		&lt/basic&gt
	 *	&lt/lsid&gt
	 *	&ltport&gt
	 *		&ltmap&gtservicename&lt/map&gt
	 *		&ltbasic&gt
	 *			&ltusername&gtusername&lt/username&gt
	 *			&ltpassword&gtpassw&lt/password&gt
	 *		&lt/basic&gt
	 *	&lt/port&gt
	 *	&ltport&gt
	 *		&ltmap&gtservicename:portname&lt/map&gt
	 *		&ltbasic&gt
	 *			&ltusername&gtusername&lt/username&gt
	 *			&ltpassword&gtpassw&lt/password&gt
	 *		&lt/basic&gt
	 *	&lt/port&gt
	 * &lt/lsid-credentials&gt
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * </pre>
	 */
    public class LSIDCredentialConfig 
    {

        public static readonly String LSID_CLIENT_HOME = "LSID_CLIENT_HOME";

        private static readonly String CREDENTIAL_FILE_NAME = "lsid-credentials.xml";
        private static readonly String LSID_CLIENT_HOME_DEFAULT = "\\lsid-client";

        private static Hashtable knownPorts = new Hashtable();
        private static Hashtable knownLSIDs = new Hashtable();

        //private string lsidHome = null;
	
        private static LSIDCredentialConfig instance = new LSIDCredentialConfig();

        /**
          * Get the singleton instance of this class
          * @return LSIDCredentialConfig the instance
          */
        public static LSIDCredentialConfig getInstance() 
        {
            return instance;
        }

        /**
          * Lookup the credentials to use for a given LSID
          * @param LSID the lsid in question
          * @return Hashtable the properties bag that should be used with an LSID credentials object
          */
        public static Hashtable getCredentials(LSID lsid) 
        {

            // service name and name
            String lsidkey = lsid.ToString();
            if (knownLSIDs.ContainsKey(lsidkey)) 
            {
                return addBasicAuth((String) knownLSIDs[lsidkey]);
            }

            lsidkey = "urn:lsid:" + lsid.Authority.Authority + ":" + lsid.Namespace + ":" + lsid.Object;
            if (knownLSIDs.ContainsKey(lsidkey)) 
            {
                return addBasicAuth((String) knownLSIDs[lsidkey]);
            }

            lsidkey = "urn:lsid:" + lsid.Authority.Authority + ":" + lsid.Namespace;
            if (knownLSIDs.ContainsKey(lsidkey)) 
            {
                return addBasicAuth((String) knownLSIDs[lsidkey]);
            }

            lsidkey = "urn:lsid:" + lsid.Authority.Authority;
            if (knownLSIDs.ContainsKey(lsidkey)) 
            {
                return addBasicAuth((String) knownLSIDs[lsidkey]);
            }
            return new Hashtable();
        }
	
        /**
          * Lookup the credentials to use for a given Authority
          * @param LSIDAuthority the lsid authority in question
          * @return Hashtable the properties bag that should be used with an LSID credentials object
          */
        public static Hashtable getCredentials(LSIDAuthority authority) 
        {
            String lsidkey = "urn:lsid:" + authority;
            if (knownLSIDs.ContainsKey(lsidkey)) 
            {
                return addBasicAuth((String) knownLSIDs[lsidkey]);
            }
            return new Hashtable();
        }

        /**
          * Lookup the credentials to use for a given port
          * @param LSIDPort the port in question
          * @return Hashtable the properties bag that should be used with an LSID credentials object
          */
        public static Hashtable getCredentials(LSIDPort port) 
        {

            // service name and name
            String portkey = port.getServiceName() + ":" + port.getName();
            if (!knownPorts.ContainsKey(portkey)) 
            {
                // name
                if (port.getServiceName() != null)
                    portkey = (String) knownPorts[port.getServiceName()];
            }
            if (portkey != null && knownPorts.ContainsKey(portkey)) 
            {
                String auth = (String) knownPorts[portkey];
                return addBasicAuth(auth);
            }
            return new Hashtable();
        }
	
        /**
          * Construct a new instance, singleton so private.
          */
        private LSIDCredentialConfig() 
        {
            String lsidHome = System.Configuration.ConfigurationSettings.AppSettings[LSIDResolverConfig.LSID_CLIENT_HOME];
            if (lsidHome == null) 
            {       
                lsidHome = LSID_CLIENT_HOME_DEFAULT;
                LSIDException.WriteError("No LSID_CLIENT_HOME specified, using default: " + lsidHome);
            }
            
            if (!Directory.Exists(lsidHome))
                Directory.CreateDirectory(lsidHome);

            String credXml = lsidHome + "\\" + CREDENTIAL_FILE_NAME;
            if (File.Exists(credXml)) 
            {
                StreamReader _in = null;
                try 
                {
                    StreamReader sr = File.OpenText(credXml);
                    LSIDCredentials credentials = new LSIDCredentials();
					
					//todo check

					XmlDocument doc = new XmlDocument();
					doc.LoadXml(sr.ReadToEnd());
					sr.Close();

					//read lsids & ports					
					XPathNavigator nav = doc.CreateNavigator();
					XPathNodeIterator itr = nav.SelectDescendants("lsid", "", false);
					while (itr.MoveNext()) 
					{
						XPathNavigator cNav = itr.Current;
						if (cNav != null)
						{
							CredentialElement l = new CredentialElement();
							l.XmlDeserialise(cNav);
							knownLSIDs.Add(l.map, l.basicUserName + ":" + l.basicPassword);
						}
					}

					itr = nav.SelectDescendants("port", "", false);
					while (itr.MoveNext()) 
					{
						XPathNavigator cNav = itr.Current;
						if (cNav != null)
						{
							CredentialElement l = new CredentialElement();
							l.XmlDeserialise(cNav);
							knownPorts.Add(l.map, l.basicUserName + ":" + l.basicPassword);
						}
					}

//                    Port[] ports = credentials.getPort();
//                    for (int i = 0; i < ports.length; ++i) 
//                    {
//                        Basic basic = ports[i].getBasic();
//                        knownPorts.put(ports[i].getMap(), basic.getUsername() + ":" + basic.getPassword());
//                    }
//
//                    Lsid[] lsids = credentials.getLsid();
//                    for (int i = 0; i < lsids.length; ++i) 
//                    {
//                        Basic basic = lsids[i].getBasic();
//                        knownLSIDs.put(lsids[i].getMap(), basic.getUsername() + ":" + basic.getPassword());
//                    }

                } 
                catch (Exception e) 
                {
                    LSIDException.WriteError("Error loading credential file, using default settings, exception trace follows");
                    LSIDException.PrintStackTrace(e);
                    writeDefaultCredentials();
                } 
                finally 
                {
                    if (_in != null) 
                    {
                        try 
                        {
                            _in.Close();
                        } 
                        catch (IOException e) 
                        {
                            LSIDException.PrintStackTrace(e);
                        }
                    }
                }
            } 
            else 
            {
                LSIDException.WriteError("Credential file: " + credXml + " does not exist, using default settings");
                writeDefaultCredentials();
            }

        }

        /**
          * create a properites bag for an auth string
          */
        private static Hashtable addBasicAuth(String auth) 
        {

            Hashtable bag = new Hashtable();
            if (auth != null) 
            {
                string[] toks = auth.Split(':');
                if (toks.Length > 1) 
                {
                    bag[LSIDCredentials.BASICUSERNAME] = toks[0];
                    bag[LSIDCredentials.BASICPASSWORD] = toks[1];
                }
            }
            return bag;

        }

        /**
          * writeout a default credential file
          */
        private void writeDefaultCredentials() 
        {
            //TODO embedded resource config
//            InputStream _in = null;
//            OutputStream _out = null;
//            try 
//            {
//                _in = getClass().getResourceAsStream(LSIDCredentialConfig.CREDENTIAL_FILE_NAME);
//                if (_in == null) 
//                {
//                    System.err.println("Non fatal error: no default credential exists in client installation");
//                    return;
//                }
//                _out = new FileOutputStream(new File(lsidHome, LSIDCredentialConfig.CREDENTIAL_FILE_NAME));
//                byte[] bytes = new byte[1024];
//                int numbytes = _in.read(bytes);
//                while (numbytes != -1) 
//                {
//                    _out.write(bytes, 0, numbytes);
//                    numbytes = _in.read(bytes);
//                }
//                _out.flush();
//            } 
//            catch (IOException e) 
//            {
//                System.err.println("No fatal error: could not write default credentials file, stack trace follows");
//                e.printStackTrace();
//            } 
//            finally 
//            {
//                if (_in != null) 
//                {
//                    try 
//                    {
//                        _in.close();
//                    } 
//                    catch (IOException e) 
//                    {
//                        e.printStackTrace();
//                    }
//                }
//                if (_out != null) 
//                {
//                    try 
//                    {
//                        _out.close();
//                    } 
//                    catch (IOException e) 
//                    {
//                        e.printStackTrace();
//                    }
//                }
//            }
        }

    }	

	public class CredentialElement
	{
		public string map = "";
		public string basicUserName = "";
		public string basicPassword = "";

		public void XmlDeserialise(XPathNavigator nav)
		{            
			XPathNodeIterator itr = nav.SelectDescendants("map", nav.NamespaceURI, false);
			if (itr.MoveNext())
			{
				map = itr.Current.Value;
			}

			itr = nav.SelectDescendants("basic", nav.NamespaceURI, false);
			if (itr.MoveNext())
			{
				XPathNodeIterator uitr = nav.SelectDescendants("username", nav.NamespaceURI, false);
				if (uitr.MoveNext())
				{
					basicUserName = uitr.Current.Value;
				}
				
				uitr = nav.SelectDescendants("password", nav.NamespaceURI, false);
				if (uitr.MoveNext())
				{
					basicPassword = uitr.Current.Value;
				}
			}
		}
	}
	
}
