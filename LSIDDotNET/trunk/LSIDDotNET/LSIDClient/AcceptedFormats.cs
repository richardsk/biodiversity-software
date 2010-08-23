using System;
using System.Collections;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace LSIDClient
{
    [Serializable()]
    public class AcceptedFormats 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field formatList
         */
        private ArrayList formatList;


        //----------------/
        //- Constructors -/
        //----------------/

        public AcceptedFormats() 
        {
            formatList = new ArrayList();
        }


        //-----------/
        //- Methods -/
        //-----------/

        /**
         * Method addFormat
         * 
         * @param vFormat
         */
        public void addFormat(String vFormat)
        {
            formatList.Add(vFormat);
        }

        /**
         * Method addFormat
         * 
         * @param index
         * @param vFormat
         */
        public void addFormat(int index, String vFormat)
        {
            formatList.Insert(index, vFormat);
        }

        /**
         * Method clearFormat
         */
        public void clearFormat()
        {
            formatList.Clear();
        }

        /**
         * Method enumerateFormat
         */
        public IEnumerator enumerateFormat()
        {
            return formatList.GetEnumerator();
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
        
            if (obj is AcceptedFormats) 
            {        
                AcceptedFormats temp = (AcceptedFormats)obj;
                if (this.formatList != null) 
                {
                    if (temp.formatList == null) return false;
                    else if (!(this.formatList.Equals(temp.formatList))) 
                        return false;
                }
                else if (temp.formatList != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Method getFormat
         * 
         * @param index
         */
        public String getFormat(int index)
        {
            //-- check bounds for index
            if ((index < 0) || (index > formatList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
        
            return (String)formatList[index];
        }

        /**
         * Method getFormat
         */
        public String[] getFormat()
        {
            return (String[])formatList.ToArray(typeof(string));
        }

        /**
         * Method getFormatAsReferenceReturns a reference to 'format'.
         * No type checking is performed on any modications to the
         * Collection.
         * 
         * @return returns a reference to the Collection.
         */
        public ArrayList getFormatAsReference()
        {
            return formatList;
        }

        /**
         * Method getFormatCount
         */
        public int getFormatCount()
        {
            return formatList.Count;
        }


        /**
         * Method removeFormat
         * 
         * @param vFormat
         */
        public Boolean removeFormat(String vFormat)
        {
            formatList.Remove(vFormat);
            return true;
        }

        /**
         * Method setFormat
         * 
         * @param index
         * @param vFormat
         */
        public void setFormat(int index, String vFormat)
        {
            //-- check bounds for index
            if ((index < 0) || (index > formatList.Count)) 
            {
                throw new IndexOutOfRangeException();
            }
            formatList[index] = vFormat;
        }

        /**
         * Method setFormat
         * 
         * @param formatArray
         */
        public void setFormat(String[] formatArray)
        {
            //-- copy array
            formatList.Clear();
            for (int i = 0; i < formatArray.Length; i++) 
            {
                formatList.Add(formatArray[i]);
            }
        }

        /**
         * Method setFormatSets the value of 'format' by copying the
         * given ArrayList.
         * 
         * @param formatCollection the Vector to copy.
         */
        public void setFormat(ArrayList formatCollection)
        {
            //-- copy collection
            formatList.Clear();
            for (int i = 0; i < formatCollection.Count; i++) 
            {
                formatList.Add((String)formatCollection[i]);
            }
        }

        /**
         * Method setFormatAsReferenceSets the value of 'format' by
         * setting it to the given ArrayList. No type checking is
         * performed.
         * 
         * @param formatCollection the ArrayList to copy.
         */
        public void setFormatAsReference(ArrayList formatCollection)
        {
            formatList = formatCollection;
        }

        public void XmlDeserialise(XPathNavigator nav)
        {
            formatList.Clear();
            
            if (nav.MoveToFirstChild())
            {
                while (nav.Name == "format" || nav.NodeType == XPathNodeType.Comment)
                {
                    if (nav.Name == "format")
                    {
                        formatList.Add(nav.Value);
                    }

                    if (!nav.MoveToNext()) break;
                }

            }
        }

    }

}
