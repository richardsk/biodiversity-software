using System;

namespace LSIDClient
{
	[Serializable()]
	public class Param 
	{

		//--------------------------/
		//- Class/Member Variables -/
		//--------------------------/

		/**
		* Field _name
		*/
		private String _name;

		/**
		 * Field _value
		 */
		private String _value;


		//----------------/
		//- Constructors -/
		//----------------/

		public Param() 
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
		 * @param obj
		 */
		public override Boolean Equals(Object obj)
		{
			if ( this == obj )
				return true;
        
			if (obj is Param) 
			{
        
				Param temp = (Param)obj;
				if (this._name != null) 
				{
					if (temp._name == null) return false;
					else if (!(this._name.Equals(temp._name))) 
						return false;
				}
				else if (temp._name != null)
					return false;
				if (this._value != null) 
				{
					if (temp._value == null) return false;
					else if (!(this._value.Equals(temp._value))) 
						return false;
				}
				else if (temp._value != null)
					return false;
				return true;
			}
			return false;
		}

		/**
		 * Returns the value of field 'name'.
		 * 
		 * @return the value of field 'name'.
		 */
		public String getName()
		{
			return this._name;
		}

		/**
		 * Returns the value of field 'value'.
		 * 
		 * @return the value of field 'value'.
		 */
		public String getValue()
		{
			return this._value;
		}
 

		/**
		 * Sets the value of field 'name'.
		 * 
		 * @param name the value of field 'name'.
		 */
		public void setName(String name)
		{
			this._name = name;
		}

		/**
		 * Sets the value of field 'value'.
		 * 
		 * @param value the value of field 'value'.
		 */
		public void setValue(String value)
		{
			this._value = value;
		}

	}
}
