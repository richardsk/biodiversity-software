using System;
using System.Collections;
using System.Xml.XPath;

namespace LSIDClient
{
    [Serializable()]
    public class ForeignAuthorities 
    {


        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field foreignAuthorities
         */
        private ArrayList _foreignAuthorities;


        //----------------/
        //- Constructors -/
        //----------------/

        public ForeignAuthorities() 
        {
            _foreignAuthorities = new ArrayList();
        } 


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addForeignAuthoritiesItem
         * 
         * @param vForeignAuthoritiesItem
         */
		public void addForeignAuthoritiesItem(ForeignAuthoritiesItem vForeignAuthoritiesItem)
		{
			_foreignAuthorities.Add(vForeignAuthoritiesItem);
		}

        /**
         * Method addForeignAuthoritiesItem
         * 
         * @param index
         * @param vForeignAuthoritiesItem
         */
		public void addForeignAuthoritiesItem(int index, ForeignAuthoritiesItem vForeignAuthoritiesItem)
		{
			_foreignAuthorities.Insert(index, vForeignAuthoritiesItem);
		}

        /**
         * Method clearForeignAuthoritiesItem
         */
		public void clearForeignAuthoritiesItem()
		{
			_foreignAuthorities.Clear();
		}

        /**
         * Method enumerateForeignAuthoritiesItem
         */
        public IEnumerator enumerateForeignAuthoritiesItem()
        {
            return _foreignAuthorities.GetEnumerator();
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
        
			if (obj is ForeignAuthorities) 
			{        
				ForeignAuthorities temp = (ForeignAuthorities)obj;
				if (this._foreignAuthorities != null) 
				{
					if (temp._foreignAuthorities == null) return false;
					else if (!(this._foreignAuthorities.Equals(temp._foreignAuthorities))) 
						return false;
				}
				else if (temp._foreignAuthorities != null)
					return false;
				return true;
			}
			return false;
		}

        /**
         * Method getForeignAuthoritiesItem
         * 
         * @param index
         */
		public ForeignAuthoritiesItem getForeignAuthoritiesItem(int index)
		{
			//-- check bounds for index
			if ((index < 0) || (index > _foreignAuthorities.Count)) 
			{
				throw new IndexOutOfRangeException();
			}
        
			return (ForeignAuthoritiesItem) _foreignAuthorities[index];
		}

        /**
         * Method getForeignAuthoritiesItem
         */
        public ForeignAuthoritiesItem[] getForeignAuthoritiesItem()
        {
            int size = _foreignAuthorities.Count;
            ForeignAuthoritiesItem[] mArray = new ForeignAuthoritiesItem[size];
            for (int index = 0; index < size; index++) 
            {
                mArray[index] = (ForeignAuthoritiesItem) _foreignAuthorities[index];
            }
            return mArray;
        }

        /**
         * Method getForeignAuthoritiesItemAsReferenceReturns a
         * reference to 'foreignAuthoritiesItem'. No type checking is
         * performed on any modications to the Collection.
         * 
         * @return returns a reference to the Collection.
         */
		public ArrayList getForeignAuthoritiesItemAsReference()
		{
			return _foreignAuthorities;
		}

        /**
         * Method getForeignAuthoritiesItemCount
         */
		public int getForeignAuthoritiesItemCount()
		{
			return _foreignAuthorities.Count;
		}

        /**
         * Method removeForeignAuthoritiesItem
         * 
         * @param vForeignAuthoritiesItem
         */
		public Boolean removeForeignAuthoritiesItem(ForeignAuthoritiesItem vForeignAuthoritiesItem)
		{
			_foreignAuthorities.Remove(vForeignAuthoritiesItem);
			return true;
		}

        /**
         * Method setForeignAuthoritiesItem
         * 
         * @param index
         * @param vForeignAuthoritiesItem
         */
		public void setForeignAuthoritiesItem(int index, ForeignAuthoritiesItem vForeignAuthoritiesItem)        
		{
			//-- check bounds for index
			if ((index < 0) || (index > _foreignAuthorities.Count)) 
			{
				throw new IndexOutOfRangeException();
			}
			_foreignAuthorities[index] = vForeignAuthoritiesItem;
		}

        /**
         * Method setForeignAuthoritiesItem
         * 
         * @param foreignAuthoritiesItemArray
         */
		public void setForeignAuthoritiesItem(ForeignAuthoritiesItem[] foreignAuthoritiesItemArray)
		{
			//-- copy array
			_foreignAuthorities.Clear();
			for (int i = 0; i < foreignAuthoritiesItemArray.Length; i++) 
			{
				_foreignAuthorities.Add(foreignAuthoritiesItemArray[i]);
			}
		}

        /**
         * Method setForeignAuthoritiesItemSets the value of
         * 'foreignAuthoritiesItem' by copying the given ArrayList.
         * 
         * @param foreignAuthoritiesItemCollection the Vector to copy.
         */
		public void setForeignAuthoritiesItem(ArrayList foreignAuthoritiesItemCollection)
		{
			//-- copy collection
			_foreignAuthorities.Clear();
			for (int i = 0; i < foreignAuthoritiesItemCollection.Count; i++) 
			{
				_foreignAuthorities.Add((ForeignAuthoritiesItem)foreignAuthoritiesItemCollection[i]);
			}
		}

        /**
         * Method setForeignAuthoritiesItemAsReferenceSets the value of
         * 'foreignAuthoritiesItem' by setting it to the given
         * ArrayList. No type checking is performed.
         * 
         * @param foreignAuthoritiesItemCollection the ArrayList to copy
         */
		public void setForeignAuthoritiesItemAsReference(ArrayList foreignAuthoritiesItemCollection)
		{
			_foreignAuthorities = foreignAuthoritiesItemCollection;
		}

        
        public void XmlDeserialise(XPathNavigator nav)
        {
            _foreignAuthorities.Clear();
            
            if (nav.MoveToFirstChild())
            {
                ForeignAuthoritiesItem fi = new ForeignAuthoritiesItem();

                while (nav.Name == "pattern" || nav.Name == "lsid" || nav.NodeType == XPathNodeType.Comment)
                {
                    if (nav.Name == "pattern")
                    {
                        Pattern p = new Pattern();
                    
                        if (nav.MoveToFirstAttribute())
                        {
                            p.auth = nav.Value;
                            nav.MoveToNextAttribute();
                            p.ns = nav.Value;

                            nav.MoveToParent();
                            if (nav.MoveToFirstChild())
                            {
                                while (nav.Name == "authority" || nav.NodeType == XPathNodeType.Comment)
                                {
                                    if (nav.NodeType != XPathNodeType.Comment)
                                    {
                                        p.authorityList.Add(nav.Value);
                                    }
                                    if (!nav.MoveToNext()) break;
                                }
                                nav.MoveToParent();
                            }
                        }

                        fi.addPattern(p);
                    }
                    
                    if (nav.Name == "lsid")
                    {
                        FALSID fa = new FALSID();

                        nav.MoveToFirstAttribute();
                        fa.lsidVal = nav.Value;

                        nav.MoveToParent();

                        if (nav.MoveToFirstChild())
                        {
                            while (nav.Name == "authority" || nav.NodeType == XPathNodeType.Comment)
                            {
                                if (nav.NodeType != XPathNodeType.Comment) fa.addAuthority(nav.Value);

                                if (!nav.MoveToNext()) break;
                            }
                            nav.MoveToParent();
                        }
                    
                        fi.addLsid(fa);
                    }


                    if (!nav.MoveToNext()) break;
                }

				_foreignAuthorities.Add(fi);
            }
        }

    }
}
