using System;

namespace LSIDClient
{
	/**
	 * This class encapsulates an LSIDAuthority that can be resolved.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
    public class LSIDAuthority 
    {
	
        public static readonly String AUTHORITY_ID_PREFIX = "lsidauth:";
	
        public static readonly String AUTHORITY_PROTOCOL="http";
        public static readonly String AUTHORITY_PATH="/authority/";
	
        public String Authority;
	
        // the resolved components of the lsid
        public String Server;
        public int Port = -1;
        public String Url;
	
        // the wsdl describing how to invoke operations on the authority
        public LSIDWSDLWrapper AuthorityWSDL;
	
        public LSIDAuthority()
        {
        }

        /**
         * Construct an LSID authority object.
         * @param String LSID Authority must be valid authority string, or an ID of the form: lsidauth:<validauthoritystring>"
         */
        public LSIDAuthority(String authstr) 
        {
            try 
            {
                Authority = authstr.ToLower();
                if (Authority.StartsWith(AUTHORITY_ID_PREFIX))
                    Authority = Authority.Substring(AUTHORITY_ID_PREFIX.Length);
            } 
            catch (Exception) 
            {
                throw new MalformedLSIDException("LSID Authority must be valid authority string, or of form: lsidauth:<validauthoritystring>");
            }								
        }
	
        /**
         * Convenience constructor, construct an LSID authority object
         * @param LSID use this LSID's authority to construct this object
         */
        public LSIDAuthority(LSID lsid) 
        {
            Authority = lsid.Authority.Authority;						
        }		
	
      	
        /**
         * Returns the authority ID representation of this authority, lsidauth:authstr
         * @return String the ID representation
         */
        public String getAuthorityID() 
        {
            return AUTHORITY_ID_PREFIX + Authority;
        }
	
        /**
         * Returns the authority String
         * @return String the authority String
         */
        public override String ToString() 
        {
            return Authority;	
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /**
         * Tests equality on the authority string
         * @return
         */
        public override Boolean Equals(Object o) 
        {
            if (!(o is LSIDAuthority))
                return false;
            LSIDAuthority auth = (LSIDAuthority)o;
            return o.ToString().Equals(ToString());
        }
	
       	
        /**
         * Returns the url of the resolved Authority, invalid until resolved.  This overrides the 
         * server and port properties, and might contain a full path or even a different protocol.
         * @return String
         */
        public String getUrl() 
        {
            if (Url == null) 
            {
                if (Server != null)
                {
                    Url = "http://" + Server;
                    if (Port != -1) 
                    {
                        Url += ":" + Port;
                    }
                    Url += AUTHORITY_PATH;
                }
                else
                    return null;
            }
            return Url;
        }

       
        /**
         * @return boolean, whether or not the authority has been resolved.
         */
        public Boolean isResolved() 
        {
            return (getUrl() != null);
        }
	
        /**
         * get the url of an authority with the given server and port
         */
        public static String getAuthorityEnpointURL(String server, int port) 
        {
            return AUTHORITY_PROTOCOL + "://" + server + ":" + port + AUTHORITY_PATH;	
        }

    }

}
