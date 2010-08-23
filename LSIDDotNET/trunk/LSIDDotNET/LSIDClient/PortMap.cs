using System;

namespace LSIDClient
{
    [Serializable()]
    public class PortMap 
    {

        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
         * Field portType
         */
        public String portType;

        /**
         * Field portFactory
         */
        public PortFactory portFactory;


        //----------------/
        //- Constructors -/
        //----------------/

        public PortMap() 
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
        
            if (obj is PortMap) 
            {
        
                PortMap temp = (PortMap)obj;
                if (this.portType != null) 
                {
                    if (temp.portType == null) return false;
                    else if (!(this.portType.Equals(temp.portType))) 
                        return false;
                }
                else if (temp.portType != null)
                    return false;
                if (this.portFactory != null) 
                {
                    if (temp.portFactory == null) return false;
                    else if (!(this.portFactory.Equals(temp.portFactory))) 
                        return false;
                }
                else if (temp.portFactory != null)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * Returns the value of field 'portFactory'.
         * 
         * @return the value of field 'portFactory'.
         */
        public PortFactory getPortFactory()
        {
            return this.portFactory;
        }

        /**
         * Returns the value of field 'portType'.
         * 
         * @return the value of field 'portType'.
         */
        public String getPortType()
        {
            return this.portType;
        }

        /**
         * Sets the value of field 'portFactory'.
         * 
         * @param portFactory the value of field 'portFactory'.
         */
        public void setPortFactory(PortFactory portFactory)
        {
            this.portFactory = portFactory;
        }

        /**
         * Sets the value of field 'portType'.
         * 
         * @param portType the value of field 'portType'.
         */
        public void setPortType(String portType)
        {
            this.portType = portType;
        }


    }

}
