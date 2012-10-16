using System;
using System.Collections;
using System.Xml.Serialization;

namespace LSIDClient
{
    [Serializable()]
    public class Pattern 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field auth
         */
        public String auth;

        /**
         * Field ns
         */
        public String ns;

        /**
         * Field authorityList
         */
        public ArrayList authorityList;


        //----------------/
        //- Constructors -/
        //----------------/

        public Pattern() 
        {
            authorityList = new ArrayList();
        }


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addAuthority
         * 
         * @param vAuthority
         */
        public void addAuthority(String vAuthority)
        {
            authorityList.Add(vAuthority);
        }

        /**
         * Method addAuthority
         * 
         * @param index
         * @param vAuthority
         */
        public void addAuthority(int index, String vAuthority)
        {
            authorityList.Insert(index, vAuthority);
        }

        /**
         * Method clearAuthority
         */
        public void clearAuthority()
        {
            authorityList.Clear();
        }

        /**
         * Method enumerateAuthority
         */
        public IEnumerator enumerateAuthority()
        {
            return authorityList.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /**
         * 
         * @param obj
         */
        public override Boolean Equals(Object obj)
        {
            if ( this == obj )
                return true;
        
            if (obj is Pattern) 
            {        
                Pattern temp = (Pattern)obj;
                if (this.auth != null) 
                {
                    if (temp.auth == null) return false;
                    else if (!(this.auth.Equals(temp.auth))) 
                        return false;
                }
                else if (temp.auth != null)
                    return false;
                if (this.ns != null) 
                {
                    if (temp.ns == null) return false;
                    else if (!(this.ns.Equals(temp.ns))) 
                        return false;
                }
                else if (temp.ns != null)
                    return false;
                if (this.authorityList != null) 
                {
                    if (temp.authorityList == null) return false;
                    else if (!(this.authorityList.Equals(temp.authorityList))) 
                        return false;
                }
                else if (temp.authorityList != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Returns the value of field 'auth'.
         * 
         * @return the value of field 'auth'.
         */
        public String getAuth()
        {
            return this.auth;
        }

        /**
         * Method getAuthority
         * 
         * @param index
         */
        public String getAuthority(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > authorityList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (String)authorityList[index];
        } 

        /**
         * Method getAuthority
         */
        public String[] getAuthority()
        {
            return (String[])authorityList.ToArray(typeof(string));
        }

        /**
         * Method getAuthorityAsReferenceReturns a reference to
         * 'authority'. No type checking is performed on any
         * modications to the Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getAuthorityAsReference()
        {
            return authorityList;
        }

        /**
         * Method getAuthorityCount
         */
        public int getAuthorityCount()
        {
            return authorityList.Count;
        }

        /**
         * Returns the value of field 'ns'.
         * 
         * @return the value of field 'ns'.
         */
        public String getNs()
        {
            return this.ns;
        }


        /**
         * Method removeAuthority
         * 
         * @param vAuthority
         */
        public Boolean removeAuthority(String vAuthority)
        {
            authorityList.Remove(vAuthority);
            return true;
        }

        /**
         * Sets the value of field 'auth'.
         * 
         * @param auth the value of field 'auth'.
         */
        public void setAuth(String auth)
        {
            this.auth = auth;
        }

        /**
         * Method setAuthority
         * 
         * @param index
         * @param vAuthority
         */
        public void setAuthority(int index, String vAuthority)
        {
            //-- check bounds for index
            if ((index < 0) || (index > authorityList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            authorityList[index] = vAuthority;
        }

        /**
         * Method setAuthority
         * 
         * @param authorityArray
         */
        public void setAuthority(String[] authorityArray)
        {
            //-- copy array
            authorityList.Clear();
            for (int i = 0; i < authorityArray.Length; i++) 
            {
                authorityList.Add(authorityArray[i]);
            }
        }

        /**
         * Method setAuthoritySets the value of 'authority' by copying
         * the given ArrayList.
         * 
         * @param authorityCollection the Vector to copy.
         */
        public void setAuthority(ArrayList authorityCollection)
        {
            //-- copy collection
            authorityList.Clear();
            for (int i = 0; i < authorityCollection.Count; i++) 
            {
                authorityList.Add((String)authorityCollection[i]);
            }
        }

        /**
         * Method setAuthorityAsReferenceSets the value of 'authority'
         * by setting it to the given ArrayList. No type checking is
         * performed.
         * 
         * @param authorityCollection the ArrayList to copy.
         */
        public void setAuthorityAsReference(ArrayList authorityCollection)
        {
            authorityList = authorityCollection;
        }

        /**
         * Sets the value of field 'ns'.
         * 
         * @param ns the value of field 'ns'.
         */
        public void setNs(String ns)
        {
            this.ns = ns;
        }

    }
}
