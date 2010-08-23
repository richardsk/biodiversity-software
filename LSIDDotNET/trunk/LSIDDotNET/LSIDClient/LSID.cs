using System;

namespace LSIDClient
{
	
	/**
	 * Encapsulates an LSID.  The meta data fields retrieved by <code> Abstract, Instances, Format,
	 * Type will be null until they are set during a meta data query invocation.
	 * @see <a href="LSIDMetadata.html">LSIDMetadata</a>
	 * 
	 * * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
    public class LSID 
    {
	
        // the lsid string
        public String Lsid;
	
        // the raw components of the lsid
        public LSIDAuthority Authority;
        public String Namespace;
        public String Object;
        public String Revision;

        // basic meta data fields, null until populated by LSIDMetaDataQuery
        public LSID Abstract;
        public String Format;
        public String Type;
        public LSID[] Instances;

        public LSID()
        {
        }

        /**
         * Construct a new LSID with the String representation.
         * @param String The lsid String respresentation
         */
        public LSID(String lsid) 
        {
            //try {
            //this.lsid = lsid.toLowerCase();
            if (lsid.EndsWith(":")) 
            {
                lsid = lsid.Substring(0, lsid.Length - 1);
            }
            string[] toks = lsid.Split(':');
            // check for urn and lsid
            try 
            {
                String urn = toks[0].ToLower();
                String l = toks[1].ToLower();
                if (!urn.Equals("urn") || !l.Equals("lsid")) 
                {
                    throw new MalformedLSIDException("urn:lsid: not found: [" + lsid + "]");
                }
            }
            catch (Exception e) 
            {
                throw new MalformedLSIDException(e, "urn:lsid: not found: [" + lsid + "]");
            }

            try 
            {
                Authority = new LSIDAuthority(toks[2]);
            }
            catch (MalformedLSIDException e) 
            {
                throw new MalformedLSIDException(e, "invalid authority found: [" + lsid + "]");
            }
            catch (Exception e) 
            {
                throw new MalformedLSIDException(e, "authority not found: [" + lsid + "]");
            }

            try 
            {
                Namespace = toks[3];
            }
            catch (Exception e) 
            {
                throw new MalformedLSIDException(e, "namespace not found: [" + lsid + "]");
            }

            try 
            {
                Object = toks[4];
            }
            catch (Exception e) 
            {
                throw new MalformedLSIDException(e, "object not found: [" + lsid + "]");
            }
            if (toks.Length > 5)
            {
                Revision = toks[5];
            }
		
            this.Lsid = "urn:lsid:" + this.Authority.Authority + ":" + this.Namespace + ":" + this.Object + (this.Revision != null ? ":" + this.Revision : "");
        }
	
        /**
         * Construct a new LSID with the given components
         * @param String the authority
         * @param String the namespace
         * @param String the object
         * @param String the revision, can be null
         */
        public LSID(String authority, String ns, String o, String revision) 
        {
            this.Authority = new LSIDAuthority(authority.ToLower());
            this.Namespace = ns;//.toLowerCase();
            this.Object = o;//.toLowerCase();
            if (revision != null)
                this.Revision = revision;//.toLowerCase();
            Lsid = "urn:lsid:" + this.Authority.Authority + ":" + this.Namespace + ":" + this.Object + (this.Revision != null ? ":" + this.Revision : "");
        }

        /**
        * Returns the lsid 
        * @return String The lsid String representation
        */
        public override String ToString() 
        {
            return Lsid;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /**
        * Two LSIDs are equal their string representations are equal disregarding case.
        */
        public override Boolean Equals(Object lsid) 
        {
            if (lsid is LSID) 
            {
                LSID theLSID = (LSID)lsid;
                return theLSID.ToString().Equals(ToString());
            } 
            else
                return false;
        }
    }

}
