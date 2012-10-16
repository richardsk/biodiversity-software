using System;
using System.Collections;

namespace LSIDClient
{

    [Serializable()]
    public class PortFactory 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field _classname
         */
        private String _classname;

        /**
         * Field _paramList
         */
        private ArrayList _paramList;


        //----------------/
        //- Constructors -/
        //----------------/

        public PortFactory() 
        {
            _paramList = new ArrayList();
        }


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addParam
         * 
         * @param vParam
         */
        public void addParam(Param vParam)
        {
            _paramList.Add(vParam);
        }

        /**
         * Method addParam
         * 
         * @param index
         * @param vParam
         */
        public void addParam(int index, Param vParam)
        {
            _paramList.Insert(index, vParam);
        }

        /**
         * Method clearParam
         */
        public void clearParam()
        {
            _paramList.Clear();
        }

        /**
         * Method enumerateParam
         */
        public IEnumerator enumerateParam()
        {
            return _paramList.GetEnumerator();
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
        
            if (obj is PortFactory) 
            {
        
                PortFactory temp = (PortFactory)obj;
                if (this._classname != null) 
                {
                    if (temp._classname == null) return false;
                    else if (!(this._classname.Equals(temp._classname))) 
                        return false;
                }
                else if (temp._classname != null)
                    return false;
                if (this._paramList != null) 
                {
                    if (temp._paramList == null) return false;
                    else if (!(this._paramList.Equals(temp._paramList))) 
                        return false;
                }
                else if (temp._paramList != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Returns the value of field 'classname'.
         * 
         * @return the value of field 'classname'.
         */
        public String getClassname()
        {
            return this._classname;
        }

        /**
         * Method getParam
         * 
         * @param index
         */
        public Param getParam(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > _paramList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (Param) _paramList[index];
        }

        /**
         * Method getParam
         */
        public Param[] getParam()
        {
            return (Param[])_paramList.ToArray(typeof(Param));
        }

        /**
         * Method getParamAsReferenceReturns a reference to 'param'. No
         * type checking is performed on any modications to the
         * Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getParamAsReference()
        {
            return _paramList;
        }

        /**
         * Method getParamCount
         */
        public int getParamCount()
        {
            return _paramList.Count;
        }


        /**
         * Method removeParam
         * 
         * @param vParam
         */
        public Boolean removeParam(Param vParam)
        {
            _paramList.Remove(vParam);
            return true;
        }

        /**
         * Sets the value of field 'classname'.
         * 
         * @param classname the value of field 'classname'.
         */
        public void setClassname(String classname)
        {
            this._classname = classname;
        }

        /**
         * Method setParam
         * 
         * @param index
         * @param vParam
         */
        public void setParam(int index, Param vParam)
        {
            //-- check bounds for index
            if ((index < 0) || (index > _paramList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            _paramList[index] = vParam;
        }

        /**
         * Method setParam
         * 
         * @param paramArray
         */
        public void setParam(Param[] paramArray)
        {
            //-- copy array
            _paramList.Clear();
            for (int i = 0; i < paramArray.Length; i++) 
            {
                _paramList.Add(paramArray[i]);
            }
        }

        /**
         * Method setParamSets the value of 'param' by copying the
         * given ArrayList.
         * 
         * @param paramCollection the Vector to copy.
         */
        public void setParam(ArrayList paramCollection)
        {
            //-- copy collection
            _paramList.Clear();
            for (int i = 0; i < paramCollection.Count; i++) 
            {
                _paramList.Add((Param)paramCollection[i]);
            }
        }

        /**
         * Method setParamAsReferenceSets the value of 'param' by
         * setting it to the given ArrayList. No type checking is
         * performed.
         * 
         * @param paramCollection the ArrayList to copy.
         */
        public void setParamAsReference(ArrayList paramCollection)
        {
            _paramList = paramCollection;
        }

    }
}
