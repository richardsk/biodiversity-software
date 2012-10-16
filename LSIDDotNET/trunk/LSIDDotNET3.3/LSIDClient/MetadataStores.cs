using System;
using System.Collections;
using System.Xml.XPath;

namespace LSIDClient
{
	//todo implement

//    [Serializable()]
//    public class MetadataStores 
//    {
//
//        //--------------------------/
//        //- Class/Member Variables -/
//        //--------------------------/
//
//        /**
//         * Field _metadataStoreList
//         */
//        private ArrayList _metadataStoreList;
//
//
//        //----------------/
//        //- Constructors -/
//        //----------------/
//
//        public MetadataStores() 
//        {
//            _metadataStoreList = new ArrayList();
//        }
//
//
//        //-----------/
//        //- Methods -/
//        //-----------/
//
//        /**
//         * Method addMetadataStore
//         * 
//         * @param vMetadataStore
//         */
//        public void addMetadataStore(MetadataStore vMetadataStore)
//        {
//            _metadataStoreList.Add(vMetadataStore);
//        }
//
//        /**
//         * Method addMetadataStore
//         * 
//         * @param index
//         * @param vMetadataStore
//         */
//        public void addMetadataStore(int index, MetadataStore vMetadataStore)
//        {
//            _metadataStoreList.Insert(index, vMetadataStore);
//        }
//
//        /**
//         * Method clearMetadataStore
//         */
//        public void clearMetadataStore()
//        {
//            _metadataStoreList.Clear();
//        } //-- void clearMetadataStore() 
//
//        /**
//         * Method enumerateMetadataStore
//         */
//        public IEnumerator enumerateMetadataStore()
//        {
//            return _metadataStoreList.GetEnumerator();
//        }
//
//        /**
//         * Note: hashCode() has not been overriden
//         * 
//         * @param obj
//         */
//        public Boolean equals(Object obj)
//        {
//            if ( this == obj )
//                return true;
//        
//            if (obj is MetadataStores) 
//            {
//        
//                MetadataStores temp = (MetadataStores)obj;
//                if (this._metadataStoreList != null) 
//                {
//                    if (temp._metadataStoreList == null) return false;
//                    else if (!(this._metadataStoreList.Equals(temp._metadataStoreList))) 
//                        return false;
//                }
//                else if (temp._metadataStoreList != null)
//                    return false;
//                return true;
//            }
//            return false;
//        }
//
//        /**
//         * Method getMetadataStore
//         * 
//         * @param index
//         */
//        public MetadataStore getMetadataStore(int index)
//        {
//            //-- check bounds for index
//            if ((index < 0) || (index > _metadataStoreList.Count)) 
//            {
//                throw new IndexOutOfRangeException();
//            }
//        
//            return (MetadataStore) _metadataStoreList[index];
//        }
//
//        /**
//         * Method getMetadataStore
//         */
//        public MetadataStore[] getMetadataStore()
//        {
//            int size = _metadataStoreList.Count;
//            MetadataStore[] mArray = new MetadataStore[size];
//            for (int index = 0; index < size; index++) 
//            {
//                mArray[index] = (MetadataStore) _metadataStoreList[index];
//            }
//            return mArray;
//        }
//
//        /**
//         * Method getMetadataStoreAsReferenceReturns a reference to
//         * 'metadataStore'. No type checking is performed on any
//         * modications to the Collection.
//         * 
//         * @return returns a reference to the Collection.
//         */
//        public ArrayList getMetadataStoreAsReference()
//        {
//            return _metadataStoreList;
//        }
//
//        /**
//         * Method getMetadataStoreCount
//         */
//        public int getMetadataStoreCount()
//        {
//            return _metadataStoreList.Count;
//        }
//
//
//		public void XmlDeserialise(XPathNavigator nav)
//		{
//			_metadataStoreList.Clear();
//            
//			XPathNodeIterator itr = nav.SelectDescendants("metadataStore", nav.NamespaceURI, false);
//			while (itr.MoveNext())
//			{
//				if (itr.Current != null)
//				{
//					MetadataStore ms = new MetadataStore();
//					ms.XmlDeserialise(itr.Current);
//
//					_metadataStoreList.Add(ms);
//				}
//			}
//		}
//
//        /**
//         * Method removeMetadataStore
//         * 
//         * @param vMetadataStore
//         */
//        public Boolean removeMetadataStore(MetadataStore vMetadataStore)
//        {
//            _metadataStoreList.Remove(vMetadataStore);
//            return true;
//        }
//
//        /**
//         * Method setMetadataStore
//         * 
//         * @param index
//         * @param vMetadataStore
//         */
//        public void setMetadataStore(int index, MetadataStore vMetadataStore)        
//        {
//            //-- check bounds for index
//            if ((index < 0) || (index > _metadataStoreList.Count)) 
//            {
//                throw new IndexOutOfRangeException();
//            }
//            _metadataStoreList[index] = vMetadataStore;
//        }
//
//        /**
//         * Method setMetadataStore
//         * 
//         * @param metadataStoreArray
//         */
//        public void setMetadataStore(MetadataStore[] metadataStoreArray)
//        {
//            //-- copy array
//            _metadataStoreList.Clear();
//            for (int i = 0; i < metadataStoreArray.Length; i++) 
//            {
//                _metadataStoreList.Add(metadataStoreArray[i]);
//            }
//        }
//
//        /**
//         * Method setMetadataStoreSets the value of 'metadataStore' by
//         * copying the given ArrayList.
//         * 
//         * @param metadataStoreCollection the Vector to copy.
//         */
//        public void setMetadataStore(ArrayList metadataStoreCollection)
//        {
//            //-- copy collection
//            _metadataStoreList.Clear();
//            for (int i = 0; i < metadataStoreCollection.Count; i++) 
//            {
//                _metadataStoreList.Add((MetadataStore)metadataStoreCollection[i]);
//            }
//        }
//
//        /**
//         * Method setMetadataStoreAsReferenceSets the value of
//         * 'metadataStore' by setting it to the given ArrayList. No
//         * type checking is performed.
//         * 
//         * @param metadataStoreCollection the ArrayList to copy.
//         */
//        public void setMetadataStoreAsReference(ArrayList metadataStoreCollection)
//        {
//            _metadataStoreList = metadataStoreCollection;
//        }
//
//    }

}
