using System;

namespace LSIDClient
{
    [Serializable()]
    public class ImportMap 
    {


        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
        * Field location
        */
        public String location;

        /**
         * Field importMapChoice
         */
        public ImportMapChoice importMapChoice;


        //----------------/
        //- Constructors -/
        //----------------/

        public ImportMap() 
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
        
            if (obj is ImportMap) 
            {
        
                ImportMap temp = (ImportMap)obj;
                if (this.location != null) 
                {
                    if (temp.location == null) return false;
                    else if (!(this.location.Equals(temp.location))) 
                        return false;
                }
                else if (temp.location != null)
                    return false;
                if (this.importMapChoice != null) 
                {
                    if (temp.importMapChoice == null) return false;
                    else if (!(this.importMapChoice.Equals(temp.importMapChoice))) 
                        return false;
                }
                else if (temp.importMapChoice != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Returns the value of field 'importMapChoice'.
         * 
         * @return the value of field 'importMapChoice'.
         */
        public ImportMapChoice getImportMapChoice()
        {
            return this.importMapChoice;
        }

        /**
         * Returns the value of field 'location'.
         * 
         * @return the value of field 'location'.
         */
        public String getLocation()
        {
            return this.location;
        }


        /**
         * Sets the value of field 'importMapChoice'.
         * 
         * @param importMapChoice the value of field 'importMapChoice'.
         */
        public void setImportMapChoice(ImportMapChoice importMapChoice)
        {
            this.importMapChoice = importMapChoice;
        }

        /**
         * Sets the value of field 'location'.
         * 
         * @param location the value of field 'location'.
         */
        public void setLocation(String location)
        {
            this.location = location;
        }


    }

}
