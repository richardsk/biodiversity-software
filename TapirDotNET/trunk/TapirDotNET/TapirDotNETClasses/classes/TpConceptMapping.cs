using System.Web;

using TapirDotNET.Controls;

namespace TapirDotNET 
{

	public class TpConceptMapping
	{

		public string mMappingType = "AbstractMapping";
		public TpConcept mrConcept;
		public string mLocalType;
		
		public TpConceptMapping()
		{
			
		}
		
		
		public virtual string GetMappingType()
		{
			return this.mMappingType;
		}// end of member function GetMappingType
		
		public virtual void  SetConcept(TpConcept rConcept)
		{
			this.mrConcept = rConcept;
		}// end of member function SetConcept
		
		public virtual void  SetLocalType(string localType)
		{
			this.mLocalType = localType;
		}// end of member function SetLocalType
		
		public virtual bool IsMapped()
		{
			// Must be called by subclasses
			
			return !Utility.VariableSupport.Empty(this.mLocalType);
		}// end of member function IsMapped
		
		public virtual void  Refresh(Utility.OrderedMap tablesAndCols)
		{
			// Must be called by subclasses
			string input_name;
			
			input_name = this.GetLocalTypeInputName();
			
			if (HttpContext.Current.Request[input_name] != null)
			{
				this.mLocalType = HttpContext.Current.Request[input_name];
			}
		}// end of member function Refresh
		
		public virtual string GetLocalTypeInputName()
		{
			string error;
			if (this.mrConcept == null)
			{
				error = "Cannot produce an input name for local type without having " + "an associated concept!";
				new TpDiagnostics().Append(TpConfigManager.CFG_INTERNAL_ERROR, error, TpConfigManager.DIAG_ERROR);
				return "undefined_concept_value";
			}
			
			return Utility.StringSupport.StringReplace(Utility.TypeSupport.ToString(this.mrConcept.GetId()) + "_type", ".", "_");
		}// end of member function GetLocalTypeInputName
		
		public virtual string  GetHtml()
		{
			return null;
			// Must be called by subclasses after subclass implementaiton
		}// end of member function GetHtml
		
		public virtual string GetXml()
		{
			// Must be overwritten by subclasses
			return "<abstractMapping/>";
		}// end of member function GetXml
		
		public virtual string GetSqlTarget()
		{
			// Must be overwritten by subclasses
			return "";
		}// end of member function GetSqlTarget
		
		public virtual Utility.OrderedMap GetSqlFrom()
		{
			// Usually overwritten by subclasses
			return Utility.TypeSupport.ToArray(new Utility.OrderedMap());
		}// end of member function GetSqlFrom
		
		public virtual object GetLocalType()
		{
			return this.mLocalType;
		}// end of member function GetLocalType

        public string GetLocalXsdType()
        {
            if (this.mLocalType == TpUtils.TYPE_TEXT)
            {
                return "http://www.w3.org/2001/XMLSchema#string";
            }
            else if (this.mLocalType == TpUtils.TYPE_NUMERIC)
            {
                return "http://www.w3.org/2001/XMLSchema#decimal";
            }
            else if (this.mLocalType == TpUtils.TYPE_DATETIME)
            {
                return "http://www.w3.org/2001/XMLSchema#dateTime";
            }
            else if (this.mLocalType == TpUtils.TYPE_DATE)
            {
                return "http://www.w3.org/2001/XMLSchema#date";
            }

            return null;

        } // end of member function GetLocalXsdType

		public virtual Utility.OrderedMap GetLocalTypes()
		{
			Utility.OrderedMap types;
            types = new Utility.OrderedMap(new object[] { "", "-- type --" }, new object[] { "text", TpUtils.TYPE_TEXT }, new object[] { "xml", TpUtils.TYPE_XML }, new object[] { "numeric", TpUtils.TYPE_NUMERIC }, new object[] { "boolean", TpUtils.TYPE_BOOL }, new object[] { "date", TpUtils.TYPE_DATE }, new object[] { "datetime", TpUtils.TYPE_DATETIME });
			
			return types;
		}// end of member function GetLocalTypes
	}
}
