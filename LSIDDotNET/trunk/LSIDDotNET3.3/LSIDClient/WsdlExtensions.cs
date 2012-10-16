using System;
using System.Collections;
using System.Xml.Serialization;

namespace LSIDClient
{
    [Serializable()]
    public class WsdlExtensions 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field importMapList
         */
        public ArrayList importMapList;

        /**
         * Field portMapList
         */
        public ArrayList portMapList;


        //----------------/
        //- Constructors -/
        //----------------/

        public WsdlExtensions() 
        {
            importMapList = new ArrayList();
            portMapList = new ArrayList();
        }


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addImportMap
         * 
         * @param vImportMap
         */
        public void addImportMap(ImportMap vImportMap)
        {
            importMapList.Add(vImportMap);
        }

        /**
         * Method addImportMap
         * 
         * @param index
         * @param vImportMap
         */
        public void addImportMap(int index, ImportMap vImportMap)
        {
            importMapList.Insert(index, vImportMap);
        }

        /**
         * Method addPortMap
         * 
         * @param vPortMap
         */
        public void addPortMap(PortMap vPortMap)
        {
            portMapList.Add(vPortMap);
        }

        /**
         * Method addPortMap
         * 
         * @param index
         * @param vPortMap
         */
        public void addPortMap(int index, PortMap vPortMap)
        {
            portMapList.Insert(index, vPortMap);
        }

        /**
         * Method clearImportMap
         */
        public void clearImportMap()
        {
            importMapList.Clear();
        }

        /**
         * Method clearPortMap
         */
        public void clearPortMap()
        {
            portMapList.Clear();
        }

        /**
         * Method enumerateImportMap
         */
        public IEnumerator enumerateImportMap()
        {
            return importMapList.GetEnumerator();
        }

        /**
         * Method enumeratePortMap
         */
        public IEnumerator enumeratePortMap()
        {
            return portMapList.GetEnumerator();
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
        
            if (obj is WsdlExtensions) 
            {        
                WsdlExtensions temp = (WsdlExtensions)obj;
                if (this.importMapList != null) 
                {
                    if (temp.importMapList == null) return false;
                    else if (!(this.importMapList.Equals(temp.importMapList))) 
                        return false;
                }
                else if (temp.importMapList != null)
                    return false;
                if (this.portMapList != null) 
                {
                    if (temp.portMapList == null) return false;
                    else if (!(this.portMapList.Equals(temp.portMapList))) 
                        return false;
                }
                else if (temp.portMapList != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Method getImportMap
         * 
         * @param index
         */
        public ImportMap getImportMap(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > importMapList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (ImportMap) importMapList[index];
        }

        /**
         * Method getImportMap
         */
        public ImportMap[] getImportMap()
        {
            int size = importMapList.Count;
            ImportMap[] mArray = new ImportMap[size];
            for (int index = 0; index < size; index++) 
            {
                mArray[index] = (ImportMap) importMapList[index];
            }
            return mArray;
        }

        /**
         * Method getImportMapAsReferenceReturns a reference to
         * 'importMap'. No type checking is performed on any
         * modications to the Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getImportMapAsReference()
        {
            return importMapList;
        }

        /**
         * Method getImportMapCount
         */
        public int getImportMapCount()
        {
            return importMapList.Count;
        }

        /**
         * Method getPortMap
         * 
         * @param index
         */
        public PortMap getPortMap(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > portMapList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (PortMap) portMapList[index];
        }

        /**
         * Method getPortMap
         */
        public PortMap[] getPortMap()
        {
            return (PortMap[])portMapList.ToArray(typeof(PortMap));
        }

        /**
         * Method getPortMapAsReferenceReturns a reference to
         * 'portMap'. No type checking is performed on any modications
         * to the Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getPortMapAsReference()
        {
            return portMapList;
        }

        /**
         * Method getPortMapCount
         */
        public int getPortMapCount()
        {
            return portMapList.Count;
        }


        /**
         * Method removeImportMap
         * 
         * @param vImportMap
         */
        public Boolean removeImportMap(ImportMap vImportMap)
        {
            importMapList.Remove(vImportMap);
            return true;
        }

        /**
         * Method removePortMap
         * 
         * @param vPortMap
         */
        public Boolean removePortMap(PortMap vPortMap)
        {
            portMapList.Remove(vPortMap);
            return true;
        }

        /**
         * Method setImportMap
         * 
         * @param index
         * @param vImportMap
         */
        public void setImportMap(int index, ImportMap vImportMap)
        {
            //-- check bounds for index
            if ((index < 0) || (index > importMapList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            importMapList[index] = vImportMap;
        }

        /**
         * Method setImportMap
         * 
         * @param importMapArray
         */
        public void setImportMap(ImportMap[] importMapArray)
        {
            //-- copy array
            importMapList.Clear();
            for (int i = 0; i < importMapArray.Length; i++) 
            {
                importMapList.Add(importMapArray[i]);
            }
        }

        /**
         * Method setImportMapSets the value of 'importMap' by copying
         * the given ArrayList.
         * 
         * @param importMapCollection the Vector to copy.
         */
        public void setImportMap(ArrayList importMapCollection)
        {
            //-- copy collection
            importMapList.Clear();
            for (int i = 0; i < importMapCollection.Count; i++) 
            {
                importMapList.Add((ImportMap)importMapCollection[i]);
            }
        }

        /**
         * Method setImportMapAsReferenceSets the value of 'importMap'
         * by setting it to the given ArrayList. No type checking is
         * performed.
         * 
         * @param importMapCollection the ArrayList to copy.
         */
        public void setImportMapAsReference(ArrayList importMapCollection)
        {
            importMapList = importMapCollection;
        }

        /**
         * Method setPortMap
         * 
         * @param index
         * @param vPortMap
         */
        public void setPortMap(int index, PortMap vPortMap)
        {
            //-- check bounds for index
            if ((index < 0) || (index > portMapList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            portMapList[index] = vPortMap;
        }

        /**
         * Method setPortMap
         * 
         * @param portMapArray
         */
        public void setPortMap(PortMap[] portMapArray)
        {
            //-- copy array
            portMapList.Clear();
            for (int i = 0; i < portMapArray.Length; i++) 
            {
                portMapList.Add(portMapArray[i]);
            }
        }

        /**
         * Method setPortMapSets the value of 'portMap' by copying the
         * given ArrayList.
         * 
         * @param portMapCollection the Vector to copy.
         */
        public void setPortMap(ArrayList portMapCollection)
        {
            //-- copy collection
            portMapList.Clear();
            for (int i = 0; i < portMapCollection.Count; i++) 
            {
                portMapList.Add((PortMap)portMapCollection[i]);
            }
        }

        /**
         * Method setPortMapAsReferenceSets the value of 'portMap' by
         * setting it to the given ArrayList. No type checking is
         * performed.
         * 
         * @param portMapCollection the ArrayList to copy.
         */
        public void setPortMapAsReference(ArrayList portMapCollection)
        {
            portMapList = portMapCollection;
        }

    }

}
