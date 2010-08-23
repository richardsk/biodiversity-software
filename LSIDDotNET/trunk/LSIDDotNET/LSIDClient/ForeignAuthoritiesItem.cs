using System;
using System.Collections;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace LSIDClient
{
    [Serializable()]
    public class ForeignAuthoritiesItem 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field _patternList
         */
        public ArrayList patternList;

        /**
         * Field _lsidList
         */
        public ArrayList lsidList;


        //----------------/
        //- Constructors -/
        //----------------/

        public ForeignAuthoritiesItem() 
        {
            patternList = new ArrayList();
            lsidList = new ArrayList();
        }


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addLsid
         * 
         * @param vLsid
         */
        public void addLsid(FALSID vLsid)
        {
            lsidList.Add(vLsid);
        }

        /**
         * Method addLsid
         * 
         * @param index
         * @param vLsid
         */
        public void addLsid(int index, FALSID vLsid)
        {
            lsidList.Insert(index, vLsid);
        }

        /**
         * Method addPattern
         * 
         * @param vPattern
         */
        public void addPattern(Pattern vPattern)
        {
            patternList.Add(vPattern);
        }

        /**
         * Method addPattern
         * 
         * @param index
         * @param vPattern
         */
        public void addPattern(int index, Pattern vPattern)
        {
            patternList.Insert(index, vPattern);
        }

        /**
         * Method clearLsid
         */
        public void clearLsid()
        {
            lsidList.Clear();
        }

        /**
         * Method clearPattern
         */
        public void clearPattern()
        {
            patternList.Clear();
        }

        /**
         * Method enumerateLsid
         */
        public IEnumerator enumerateLsid()
        {
            return lsidList.GetEnumerator();
        }

        /**
         * Method enumeratePattern
         */
        public IEnumerator enumeratePattern()
        {
            return patternList.GetEnumerator();
        }

        /**
         * Method getLsid
         * 
         * @param index
         */
        public FALSID getLsid(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > lsidList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (FALSID) lsidList[index];
        }

        /**
         * Method getLsid
         */
        public FALSID[] getLsid()
        {
            return (FALSID[])lsidList.ToArray(typeof(FALSID));
        }

        /**
         * Method getLsidAsReferenceReturns a reference to 'lsid'. No
         * type checking is performed on any modications to the
         * Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getLsidAsReference()
        {
            return lsidList;
        }

        /**
         * Method getLsidCount
         */
        public int getLsidCount()
        {
            return lsidList.Count;
        }

        /**
         * Method getPattern
         * 
         * @param index
         */
        public Pattern getPattern(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > patternList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (Pattern) patternList[index];
        }

        /**
         * Method getPattern
         */
        public Pattern[] getPattern()
        {
            return (Pattern[])patternList.ToArray(typeof(Pattern));
        }

        /**
         * Method getPatternAsReferenceReturns a reference to
         * 'pattern'. No type checking is performed on any modications
         * to the Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getPatternAsReference()
        {
            return patternList;
        }

        /**
         * Method getPatternCount
         */
        public int getPatternCount()
        {
            return patternList.Count;
        }

        /**
         * Method removeLsid
         * 
         * @param vLsid
         */
        public Boolean removeLsid(FALSID vLsid)
        {
            lsidList.Remove(vLsid);
            return true;
        }

        /**
         * Method removePattern
         * 
         * @param vPattern
         */
        public Boolean removePattern(Pattern vPattern)
        {
            patternList.Remove(vPattern);
            return true;
        }

        /**
         * Method setLsid
         * 
         * @param index
         * @param vLsid
         */
        public void setLsid(int index, FALSID vLsid)
        {
            //-- check bounds for index
            if ((index < 0) || (index > lsidList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            lsidList[index] = vLsid;
        }

        /**
         * Method setLsid
         * 
         * @param lsidArray
         */
        public void setLsid(FALSID[] lsidArray)
        {
            //-- copy array
            lsidList.Clear();
            for (int i = 0; i < lsidArray.Length; i++) 
            {
                lsidList.Add(lsidArray[i]);
            }
        }

        /**
         * Method setLsidSets the value of 'lsid' by copying the given
         * ArrayList.
         * 
         * @param lsidCollection the Vector to copy.
         */
        public void setLsid(ArrayList lsidCollection)
        {
            //-- copy collection
            lsidList.Clear();
            for (int i = 0; i < lsidCollection.Count; i++) 
            {
                lsidList.Add((FALSID)lsidCollection[i]);
            }
        }

        /**
         * Method setLsidAsReferenceSets the value of 'lsid' by setting
         * it to the given ArrayList. No type checking is performed.
         * 
         * @param lsidCollection the ArrayList to copy.
         */
        public void setLsidAsReference(ArrayList lsidCollection)
        {
            lsidList = lsidCollection;
        }

        /**
         * Method setPattern
         * 
         * @param index
         * @param vPattern
         */
        public void setPattern(int index, Pattern vPattern)
        {
            //-- check bounds for index
            if ((index < 0) || (index > patternList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            patternList[index] = vPattern;
        }

        /**
         * Method setPattern
         * 
         * @param patternArray
         */
        public void setPattern(Pattern[] patternArray)
        {
            //-- copy array
            patternList.Clear();
            for (int i = 0; i < patternArray.Length; i++) 
            {
                patternList.Add(patternArray[i]);
            }
        }

        /**
         * Method setPatternSets the value of 'pattern' by copying the
         * given ArrayList.
         * 
         * @param patternCollection the Vector to copy.
         */
        public void setPattern(ArrayList patternCollection)
        {
            //-- copy collection
            patternList.Clear();
            for (int i = 0; i < patternCollection.Count; i++) 
            {
                patternList.Add((Pattern)patternCollection[i]);
            }
        }

        /**
         * Method setPatternAsReferenceSets the value of 'pattern' by
         * setting it to the given ArrayList. No type checking is
         * performed.
         * 
         * @param patternCollection the ArrayList to copy.
         */
        public void setPatternAsReference(ArrayList patternCollection)
        {
            patternList = patternCollection;
        }


    }

}
