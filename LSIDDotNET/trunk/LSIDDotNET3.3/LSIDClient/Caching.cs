using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace LSIDClient
{
    [Serializable()]
    public class Caching 
    {
        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
        * Field useCache
        */
        public String useCache;

        /**
         * Field lsidCacheDir
         */
        public String lsidCacheDir;


        //----------------/
        //- Constructors -/
        //----------------/

        public Caching() 
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
        
			if (obj is Caching) 
			{
        
				Caching temp = (Caching)obj;
				if (this.useCache != null) 
				{
					if (temp.useCache == null) return false;
					else if (!(this.useCache.Equals(temp.useCache))) 
						return false;
				}
				else if (temp.useCache != null)
					return false;
				if (this.lsidCacheDir != null) 
				{
					if (temp.lsidCacheDir == null) return false;
					else if (!(this.lsidCacheDir.Equals(temp.lsidCacheDir))) 
						return false;
				}
				else if (temp.lsidCacheDir != null)
					return false;
				return true;
			}
			return false;
		}

        /**
         * Returns the value of field 'lsidCacheDir'.
         * 
         * @return the value of field 'lsidCacheDir'.
         */
        public String getLsidCacheDir()
        {
            return this.lsidCacheDir;
        } 

        /**
         * Returns the value of field 'useCache'. 
         * 
         * @return the value of field 'useCache'.
         */
        public String getUseCache()
        {
            return this.useCache;
        } 

        public void XmlDeserialise(XPathNavigator nav)
        {
            useCache = null;
            XPathNodeIterator itr = nav.SelectChildren("useCache", nav.NamespaceURI);
            if (itr.MoveNext())
            {
                useCache = itr.Current.Value;
            }
            lsidCacheDir = null;
            itr = nav.SelectChildren("lsidCacheDir", nav.NamespaceURI);
            if (itr.MoveNext())
            {
                lsidCacheDir = itr.Current.Value;
            }
        }

        /**
         * Sets the value of field 'lsidCacheDir'.
         * 
         * @param lsidCacheDir the value of field 'lsidCacheDir'.
         */
        public void setLsidCacheDir(String lsidCacheDir)
        {
            this.lsidCacheDir = lsidCacheDir;
        }

        /**
         * Sets the value of field 'useCache'.
         * 
         * @param useCache the value of field 'useCache'.
         */
        public void setUseCache(String useCache)
        {
            this.useCache = useCache;
        }


    }
}
