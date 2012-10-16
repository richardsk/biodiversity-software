using System;
using System.Collections;
using System.Xml.XPath;
using System.Xml;

namespace LSIDClient
{
    [Serializable()]
    public class HostMappings 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field hostMappingList
         */
        public ArrayList hostMappingList = new ArrayList();


        //----------------/
        //- Constructors -/
        //----------------/

        public HostMappings() 
        {
            hostMappingList = new ArrayList();
        }


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addHostMapping
         * 
         * @param vHostMapping
         */
        public void addHostMapping(HostMapping vHostMapping)
        {
            hostMappingList.Add(vHostMapping);
        }

        /**
         * Method addHostMapping
         * 
         * @param index
         * @param vHostMapping
         */
        public void addHostMapping(int index, HostMapping vHostMapping)
        {
            hostMappingList.Insert(index, vHostMapping);
        }

        /**
         * Method clearHostMapping
         */
        public void clearHostMapping()
        {
            hostMappingList.Clear();
        }

        /**
         * Method enumerateHostMapping
         */
        public IEnumerator enumerateHostMapping()
        {
            return hostMappingList.GetEnumerator();
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
        
            if (obj is HostMappings)
            {
        
                HostMappings temp = (HostMappings)obj;
                if (this.hostMappingList != null) 
                {
                    if (temp.hostMappingList == null) return false;
                    else if (!(this.hostMappingList.Equals(temp.hostMappingList))) 
                        return false;
                }
                else if (temp.hostMappingList != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Method getHostMapping
         * 
         * @param index
         */
        public HostMapping getHostMapping(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > hostMappingList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (HostMapping) hostMappingList[index];
        }

        /**
         * Method getHostMapping
         */
        public HostMapping[] getHostMapping()
        {
            int size = hostMappingList.Count;
            HostMapping[] mArray = new HostMapping[size];
            for (int index = 0; index < size; index++) 
            {
                mArray[index] = (HostMapping) hostMappingList[index];
            }
            return mArray;
        }

        /**
         * Method getHostMappingAsReferenceReturns a reference to
         * 'hostMapping'. No type checking is performed on any
         * modications to the Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getHostMappingAsReference()
        {
            return hostMappingList;
        }

        /**
         * Method getHostMappingCount
         */
        public int getHostMappingCount()
        {
            return hostMappingList.Count;
        }


        public void XmlDeserialise(XPathNavigator nav)
        {
            hostMappingList.Clear();
            
            XPathNodeIterator itr = nav.SelectDescendants("hostMapping", nav.NamespaceURI, false);
            while (itr.MoveNext())
            {
                if (itr.Current != null)
                {
                    HostMapping hm = new HostMapping();
                    hm.XmlDeserialise(itr.Current);

                    hostMappingList.Add(hm);
                }
            }
        }

        /**
         * Method removeHostMapping
         * 
         * @param vHostMapping
         */
        public Boolean removeHostMapping(HostMapping vHostMapping)
        {
            hostMappingList.Remove(vHostMapping);
            return true;
        }

        /**
         * Method setHostMapping
         * 
         * @param index
         * @param vHostMapping
         */
        public void setHostMapping(int index, HostMapping vHostMapping)
        {
            //-- check bounds for index
            if ((index < 0) || (index > hostMappingList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            hostMappingList[index] = vHostMapping;
        }

        /**
         * Method setHostMapping
         * 
         * @param hostMappingArray
         */
        public void setHostMapping(HostMapping[] hostMappingArray)
        {
            //-- copy array
            hostMappingList.Clear();
            for (int i = 0; i < hostMappingArray.Length; i++) 
            {
                hostMappingList.Add(hostMappingArray[i]);
            }
        }

        /**
         * Method setHostMappingSets the value of 'hostMapping' by
         * copying the given ArrayList.
         * 
         * @param hostMappingCollection the Vector to copy.
         */
        public void setHostMapping(ArrayList hostMappingCollection)
        {
            //-- copy collection
            hostMappingList.Clear();
            for (int i = 0; i < hostMappingCollection.Count; i++) 
            {
                hostMappingList.Add((HostMapping)hostMappingCollection[i]);
            }
        }

        /**
         * Method setHostMappingAsReferenceSets the value of
         * 'hostMapping' by setting it to the given ArrayList. No type
         * checking is performed.
         * 
         * @param hostMappingCollection the ArrayList to copy.
         */
        public void setHostMappingAsReference(ArrayList hostMappingCollection)
        {
            hostMappingList = hostMappingCollection;
        }


    }

}
