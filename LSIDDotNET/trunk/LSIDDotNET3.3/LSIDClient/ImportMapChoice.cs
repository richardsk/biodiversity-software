using System;

namespace LSIDClient
{
    [Serializable()]
    public class ImportMapChoice 
    {


        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
        * Field _resource
        */
        private String _resource;

        /**
         * Field _file
         */
        private String _file;


        //----------------/
        //- Constructors -/
        //----------------/

        public ImportMapChoice() 
        {
        }


        //-----------/
        //- Methods -/
        //-----------/

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
        
            if (obj is ImportMapChoice) 
            {
        
                ImportMapChoice temp = (ImportMapChoice)obj;
                if (this._resource != null) 
                {
                    if (temp._resource == null) return false;
                    else if (!(this._resource.Equals(temp._resource))) 
                        return false;
                }
                else if (temp._resource != null)
                    return false;
                if (this._file != null) 
                {
                    if (temp._file == null) return false;
                    else if (!(this._file.Equals(temp._file))) 
                        return false;
                }
                else if (temp._file != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Returns the value of field 'file'.
         * 
         * @return the value of field 'file'.
         */
        public String getFile()
        {
            return this._file;
        }

        /**
         * Returns the value of field 'resource'.
         * 
         * @return the value of field 'resource'.
         */
        public String getResource()
        {
            return this._resource;
        }

        /**
         * Sets the value of field 'file'.
         * 
         * @param file the value of field 'file'.
         */
        public void setFile(String file)
        {
            this._file = file;
        }

        /**
         * Sets the value of field 'resource'.
         * 
         * @param resource the value of field 'resource'.
         */
        public void setResource(String resource)
        {
            this._resource = resource;
        }

    }
}
