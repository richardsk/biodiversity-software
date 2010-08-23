using System.Web;

namespace TapirDotNET 
{

	public class TpExpression
	{
		public int mType = -1;
		public object mReference; // mixed: a value (for literals) or a parameter name or a concept id
								  //        or a TpTransparentConcept object (base concept of local 
								  //        filters)
		public bool mHasWildcard = false;
		public object mBaseConcept; // Type of Expression (see constants defined in TpFilter.cs)
		public bool XmlEncode = true;
        public bool mRequired = false;
		
		public TpExpression(int type, object reference, bool required)
		{
			this.mType = type;
			this.mReference = reference;
            this.mRequired = required;
		}

        public bool IsRequired()
        {
            return mRequired;
        }
		
		public new int GetType()
		{
			return this.mType;
		}// end of member function GetType
		
		public virtual void  SetReference(object reference)
		{
			this.mReference = reference;
		}// end of member function SetReference
		
		public virtual object GetReference()
		{
			return this.mReference;
		}// end of member function GetReference
		
		public virtual string GetValue(TpResource rResource, string localType, bool caseSensitive, bool isLike, string conceptDatatype)
		{
			object val = null;
			string msg;
			TpLocalMapping r_local_mapping;
			TpConcept concept;
			TpConceptMapping mapping;
			TpDataSource r_data_source;
			string r_adodb_connection;
			
			if (this.mType == TpFilter.EXP_LITERAL)
			{
                val = _PrepareValue( this.mReference, rResource, localType, caseSensitive, isLike, conceptDatatype );
                if (val == null) return "";
			}
			else if (this.mType == TpFilter.EXP_PARAMETER)
			{
				if (HttpContext.Current.Request[this.mReference.ToString()] == null)
				{
					msg = "Parameter \"" + this.mReference + "\" is missing";
						
					new TpDiagnostics().Append(TpConfigManager.DC_MISSING_PARAMETER, msg, TpConfigManager.DIAG_WARN);
					return "";
				}
					
				val = HttpContext.Current.Request[this.mReference.ToString()];

                _PrepareValue(val, rResource, localType, caseSensitive, isLike, conceptDatatype);
                if (val == null) return "";
			}
			else if (this.mType == TpFilter.EXP_CONCEPT)
			{
				r_local_mapping = rResource.GetLocalMapping();
						
				concept = r_local_mapping.GetConcept(this.mReference);
						
				if (concept == null || !concept.IsMapped())
				{
					msg = "Concept \"" + this.mReference + "\" is not mapped";
							
					new TpDiagnostics().Append(TpConfigManager.DC_UNMAPPED_CONCEPT, msg, TpConfigManager.DIAG_WARN);
					return "";
				}
						
				if (!concept.IsSearchable())
				{
					msg = "Concept \"" + this.mReference + "\" is not searchable";
							
					new TpDiagnostics().Append(TpConfigManager.DC_UNSEARCHABLE_CONCEPT, msg, TpConfigManager.DIAG_WARN);
					return "";
				}
						
				mapping = concept.GetMapping();
						
				val = mapping.GetSqlTarget();
						
				if (localType == TpUtils.TYPE_TEXT && !caseSensitive)
				{
					r_data_source = rResource.GetDataSource();
							
					r_adodb_connection = r_data_source.GetConnection().ConnectionString;
							
					val = r_adodb_connection.ToUpper() + "(" + val + ")";
				}
			}
			else if (this.mType == TpFilter.EXP_COLUMN)
				// only for local filters
			{
				val = this.mReference.ToString();// table.column
			}
			else if (this.mType == TpFilter.EXP_VARIABLE)
			{
				if (!rResource.HasVariable(this.mReference.ToString()))
				{
					msg = "Unknown variable \"" + this.mReference + "\"";
									
					new TpDiagnostics().Append(TpConfigManager.DC_UNKNOWN_VARIABLE, msg, TpConfigManager.DIAG_WARN);
					return "";
				}
								
				val = Utility.TypeSupport.ToString(rResource.GetVariable(this.mReference.ToString()));
			}
			
			if (this.mHasWildcard && val != null)
			{
				val += " ESCAPE '&'";// SQL92
			}

            if (val == null) return "";

			return val.ToString();
		}// end of member function GetValue
		
		public virtual string GetLogRepresentation()
		{
			string txt;
			string value_Renamed;
			txt = "";
			
			if (this.mType == TpFilter.EXP_LITERAL)
			{
				txt = "\"" + this.mReference.ToString().Replace("\"", "\\\"") + "\"";
			}
			else
			{
				if (this.mType == TpFilter.EXP_PARAMETER)
				{
					value_Renamed = "";
					
					if (HttpContext.Current.Request[this.mReference.ToString()] != null)
					{
						value_Renamed = Utility.TypeSupport.ToString(HttpContext.Current.Request[this.mReference.ToString()]);
					}
					
					txt = "Parameter(" + this.mReference + "=>" + value_Renamed + ")";
				}
				else
				{
					if (this.mType == TpFilter.EXP_CONCEPT)
					{
						txt = this.mReference.ToString();
					}
					else
					{
						if (this.mType == TpFilter.EXP_VARIABLE)
						{
							txt = "Variable(" + this.mReference + ")";
						}
					}
				}
			}
			
			return txt;
		}// end of member function GetLogRepresentation
		
		
		public virtual string GetLikeTerm(string val)
		{
			bool no_wildcard;
			int i;
			no_wildcard = false;
			
			if (val.IndexOf("*") == -1)
			{
				no_wildcard = true;
			}
			
			if (no_wildcard)
			{
				// No wildcard means adding one in the beginning and 
				// another in the end
				val = "*" + val + "*";
			}
			
			// Escape DB wildcard character in term
			if (val.IndexOf(TpConfigManager.TP_SQL_WILDCARD) != -1)
			{
				this.mHasWildcard = true;
				val = val.Replace(TpConfigManager.TP_SQL_WILDCARD, "&" + TpConfigManager.TP_SQL_WILDCARD);
			}
						
			// Replace wildcards if DB uses a different character
			// Note: don't replace escaped wildcards!!
			string[] parts = val.Split("*".ToCharArray());
				
			if (parts.Length > 1)
			{
				val = "";
				
				for (i = 0; i < parts.Length; ++i)
				{
					if (i > 0)
					{
						//If last character of last piece is "_"
						if (parts[i - 1].Length > 0 && parts[i - 1].Substring(parts[i - 1].Length - 1, 1) == "_")
						{
							val = val.Substring(0, val.Length - 1) + "*";
						}
						else
						{
							val += TpConfigManager.TP_SQL_WILDCARD;
						}
					}
					
					val = val + parts[i];
				}				
			}
			
			return val;
		}// end of member function GetLikeTerm
		
		public override string ToString()
		{
			string ret;
			ret = "";
			
			if (this.mType == TpFilter.EXP_LITERAL)
			{
				ret += "literal";
			}
			else
			{
				if (this.mType == TpFilter.EXP_CONCEPT)
				{
					ret += "concept";
				}
				else
				{
					if (this.mType == TpFilter.EXP_PARAMETER)
					{
						ret += "parameter";
					}
					else
					{
						if (this.mType == TpFilter.EXP_VARIABLE)
						{
							ret += "variable";
						}
						else
						{
							ret += "expression?";
						}
					}
				}
			}
			
			return ret + "[" + this.mReference + "]";
		}// end of member function ToString
		
		public virtual string GetXml()
		{
			string xml;
			TpConcept concept;
			xml = "";
			
			if (this.mType == TpFilter.EXP_LITERAL)
			{
				xml = "<literal value=\"" + this.mReference.ToString() + "\"/>";
			}
			else if (this.mType == TpFilter.EXP_PARAMETER)
			{
				xml = "<parameter name=\"" + this.mReference.ToString() + "\"/>";
			}
			else if (this.mType == TpFilter.EXP_CONCEPT)
			{
				xml = "<concept id=\"" + this.mReference.ToString() + "\"/>";
			}
			else if (this.mType == TpFilter.EXP_COLUMN) // only for local filters
			{
				// local filters
				concept = (TpConcept)this.mBaseConcept;
							
				SingleColumnMapping scm = (SingleColumnMapping)concept.GetMapping();
							
				xml = "<t_concept table=\"" + scm.GetTable() + "\" " + "field=\"" + scm.GetField() + "\" " + "type=\"" + scm.GetLocalType() + "\"/>";
			}
			else if (this.mType == TpFilter.EXP_VARIABLE)
			{
				xml = "<variable name=\"" + this.mReference.ToString() + "\"/>";
			}
			
			return xml;
		}// end of member function GetXml
		
        
        private object _PrepareValue( object value, TpResource rResource, string localType, bool caseSensitive, bool isLike, string conceptDatatype )
        {
            bool add_delimiter = true;

            // TODO: add specific blocks for the other concept types

            if ( conceptDatatype == "http://www.w3.org/2001/XMLSchema#dateTime" && 
                      ! isLike )
            {
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(value.ToString(), "^([\\-]?\\d{4})\\-(\\d{2})\\-(\\d{2})T(\\d{2}):(\\d{2}):(\\d{2})(\\.\\d+)?(Z|(\\+|\\-)(\\d{2}):(\\d{2}))?$");

                if (m.Success && int.Parse(m.Groups[2].Value) >  0 && int.Parse(m.Groups[2].Value) < 13 && 
                     int.Parse(m.Groups[3].Value) >  0 && int.Parse(m.Groups[3].Value) < 32 && 
                     int.Parse(m.Groups[4].Value) >= 0 && int.Parse(m.Groups[4].Value) < 24 && 
                     int.Parse(m.Groups[5].Value) >= 0 && int.Parse(m.Groups[5].Value) < 61 && 
                     int.Parse(m.Groups[6].Value) >= 0 && int.Parse(m.Groups[6].Value) < 61 )
                {
                    int year = int.Parse(m.Groups[1].Value);
                    int month = int.Parse(m.Groups[2].Value);
                    int day = int.Parse(m.Groups[3].Value);
                    int hr = int.Parse(m.Groups[4].Value);
                    int min = int.Parse(m.Groups[5].Value);
                    int secs = int.Parse(m.Groups[6].Value);

                    // Note: second decimals and time zone are being ignored

                    TpDataSource r_data_source = rResource.GetDataSource();

                    System.Data.OleDb.OleDbConnection r_adodb_connection = r_data_source.GetConnection();

                    if ( localType == TpUtils.TYPE_DATETIME )
                    {
                       value = new System.Data.SqlTypes.SqlDateTime(year, month, day, hr, min, secs ).ToString();
                       add_delimiter = false;
                    }
                    else if ( localType == TpUtils.TYPE_DATE )
                    {
                       value = new System.Data.SqlTypes.SqlDateTime(year, month, day).ToString();

                       add_delimiter = false;
                    }
                    else if ( localType == TpUtils.TYPE_TEXT )
                    {
                        // Assume that the text follows the same pattern?
                    }
                    else if ( localType == TpUtils.TYPE_NUMERIC )
                    {
                        string msg = "Expression " + this.ToString() + " has a local datatype (numeric) incompatible with the corresponding concept datatype (xsd:dateTime)";

                        TpDiagnostics td = new TpDiagnostics();
                        td.Append( TpConfigManager.DC_INVALID_FILTER, msg, TpConfigManager.DIAG_WARN );

                        return null;
                    }
                }
                else
                {
                    string msg = "Value '" + value + "' does not match the expected xsd:dateTime pattern";

                    new TpDiagnostics().Append( TpConfigManager.DC_INVALID_FILTER, msg, TpConfigManager.DIAG_WARN );
                }
            }

            if ( localType != TpUtils.TYPE_NUMERIC )
            {
                if ( localType == TpUtils.TYPE_TEXT && !caseSensitive )
                {
                    value = value.ToString().ToUpper();
                }

                if ( add_delimiter )
                {
                    value = value.ToString().Replace(TpConfigManager.TP_SQL_QUOTE, TpConfigManager.TP_SQL_QUOTE_ESCAPE);
                }

                if ( isLike )
                {
                    value = this.GetLikeTerm( value.ToString() );
                }

                if ( add_delimiter )
                {
                    value = TpConfigManager.TP_SQL_QUOTE + value + TpConfigManager.TP_SQL_QUOTE;
                }
            }

            return value;

        } // end of member function _PrepareValue

		public virtual object Accept(object visitor, object args)
		{
			return ((TpFilterVisitor)visitor).VisitExpression(this, args);
		}// end of member function Accept
		
		 /**
		* Internal method called before serialization
		*
		* @return array Properties that should be considered during serialization
		*/
		public virtual Utility.OrderedMap __sleep()
		{
			return new Utility.OrderedMap("mType", "mReference");
		}// end of member function __sleep
	}
}
