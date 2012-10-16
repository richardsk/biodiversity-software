using System;
using System.Xml;
using System.Xml.XPath;

namespace LSIDClient
{
    [Serializable()]
    public class HostMapping 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
        * Field authority
        */
        public String authority;

        /**
         * Field endpoint
         */
        public String endpoint;


        //----------------/
        //- Constructors -/
        //----------------/

        public HostMapping() 
        {
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //-----------/
        //- Methods -/
        //-----------/

        /**
         * 
         * @param obj
         */
        public override Boolean Equals(Object obj)
        {
            if ( this == obj )
                return true;
        
            if (obj is HostMapping) 
            {
        
                HostMapping temp = (HostMapping)obj;
                if (this.authority != null) 
                {
                    if (temp.authority == null) return false;
                    else if (!(this.authority.Equals(temp.authority))) 
                        return false;
                }
                else if (temp.authority != null)
                    return false;
                if (this.endpoint != null) 
                {
                    if (temp.endpoint == null) return false;
                    else if (!(this.endpoint.Equals(temp.endpoint))) 
                        return false;
                }
                else if (temp.endpoint != null)
                    return false;
                return true;
            }
            return false;
        }
    

        /**
         * Returns the value of field 'authority'.
         * 
         * @return the value of field 'authority'.
         */
        public String getAuthority()
        {
            return this.authority;
        }

        /**
         * Returns the value of field 'endpoint'.
         * 
         * @return the value of field 'endpoint'.
         */
        public String getEndpoint()
        {
            return this.endpoint;
        }


        
        public void XmlDeserialise(XPathNavigator nav)
        {
            authority = null;
            XPathNodeIterator itr = nav.SelectChildren("authority", nav.NamespaceURI);
            if (itr.MoveNext())
            {
                authority = itr.Current.Value;
            }
            endpoint = null;
            itr = nav.SelectChildren("endpoint", nav.NamespaceURI);
            if (itr.MoveNext())
            {
                endpoint = itr.Current.Value;
            }
        }


        /**
         * Sets the value of field 'authority'.
         * 
         * @param authority the value of field 'authority'.
         */
        public void setAuthority(String authority)
        {
            this.authority = authority;
        }

        /**
         * Sets the value of field 'endpoint'.
         * 
         * @param endpoint the value of field 'endpoint'.
         */
        public void setEndpoint(String endpoint)
        {
            this.endpoint = endpoint;
        }


    }

}
