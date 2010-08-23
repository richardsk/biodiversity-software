using System;

namespace TapirDotNET 
{

	public class TpSqlBuilder
	{
		public bool mDistinct = false;
		public bool mCount = false;
		public bool mGroupAll = false;
		public Utility.OrderedMap mOrderBy;
		public Utility.OrderedMap mSelect = new Utility.OrderedMap();
		public Utility.OrderedMap mTables = new Utility.OrderedMap();
		public Utility.OrderedMap mConditions = new Utility.OrderedMap();// null = no order ||
		// empty array = order by all (ascending) || 
		// array of concept ids => descend (boolean)
		
		// table name => TpTable object
        private System.Data.OleDb.OleDbConnection cnn;
		
		
		public TpSqlBuilder(System.Data.OleDb.OleDbConnection cnn)
		{
            this.cnn = cnn;
		}
		
		
		public virtual void  SetDistinct(bool bool_Renamed)
		{
			this.mDistinct = bool_Renamed;
		}// end of member function SetDistinct
		
		public virtual void  AddCountColumn(bool doCount)
		{
			this.mCount = doCount;
		}// end of member function AddCountColumn
		
		public virtual void  GroupAll()
		{
			this.mGroupAll = true;
		}// end of member function GroupAll
		
		public virtual void  OrderBy(Utility.OrderedMap orderBy)
		{
			this.mOrderBy = orderBy;// target id => descend
		}// end of member function OrderBy
		
		public virtual void  AddTargetColumn(string column)
		{
			this.mSelect[column] = column;
		}// end of member function AddTargetColumn
		
		 /** $concept here should always correspond to a property
		*  In the future, $concept should also indicate to which
		*  class it belongs. And the class must also have a mapping.
		*/
		public virtual bool AddTargetConcept(TpConcept concept)
		{
			TpConceptMapping mapping = concept.GetMapping();
		
	        string sql_target = mapping.GetSqlTarget();
            string concept_id = concept.GetId();

            if (sql_target == null || sql_target.Length == 0)
            {
                string msg = "Could not find a valid local mapping for concept '" + concept_id + "'. It will be ignored.";
                new TpDiagnostics().Append(TpConfigManager.DC_UNMAPPED_CONCEPT, msg, TpConfigManager.DIAG_ERROR);
                return false;
            }

			this.mSelect[concept.GetId()] = mapping.GetSqlTarget();
            
            return true;
		}// end of member function AddTargetConcept
		
		public virtual int GetTargetIndex(string targetId)
		{
			int i;
			i = - 1;
			
			foreach ( string target_id in this.mSelect.Keys ) 
			{
				object sql = this.mSelect[target_id];
				++i;
				
				if (Utility.StringSupport.StringCompare(targetId, target_id, false) == 0)
				{
					break;
				}
			}
			
			
			return i;
		}// end of member function GetTargetIndex
		
		public virtual string GetTargetSql(string targetId)
		{
			return this.mSelect[targetId].ToString();
		}// end of member function GetTargetSql
		
		 /** In the future this method should receive as a parameter
		*  a class that maps to one or more linked tables. It could
		*  then be called a single time, and then subsequent calls
		*  to AddLinkToClass() could be made.
		*/
		public virtual void  AddRecordSource(Utility.OrderedMap tables)
		{
			this.mTables = tables;
		}// end of member function AddRecordSource
		
		public virtual void  AddCondition(string sql)
		{
			this.mConditions.Push(sql);
		}// end of member function AddCondition
		    
        /** Return a table or column name ready to be used in a SQL statement.
         *  The SQL syntax is not case sensitive by default. However, names can be 
         *  case sensitive and contain spaces and other delimiters and can use keywords, 
         *  if sourrounded with square brackets ( [] ).  
         */
        public static string GetSqlName( string name ) 
        {
            if ( ! TpConfigManager.TP_SQL_DELIMIT_NAMES )
            {
                return name;
            }
        
            string[] names = name.Split('.');

            string res = "[";
            foreach (string s in names)
            {
                if (res.Length > 1) res += "].[";
                res += s;
            }
            res += "]";
            return res;

        } // end of member function GetSqlName

		public virtual string GetSql()
		{
			string sql;
			int i;
			Utility.OrderedMap keys;
			int n_tab;
			string from;
			int num_conditions;
			sql = "SELECT ";
			
			if (this.mDistinct)
			{
				
				sql += "DISTINCT ";
			}
			
			// Targets clause
			
			i = 0;

			foreach ( string target_id in this.mSelect.Keys ) 
			{
				object target_sql = this.mSelect[target_id];
				++i;
				
				if (i > 1)
				{
					sql += ", ";
				}
				
				sql += Utility.TypeSupport.ToString(target_sql) + " AS c" + this.GetTargetIndex(target_id).ToString();
			}
			
			if (this.mCount)
			{
				
				if (i > 0)
				{
					sql += ", ";
				}
				
				sql += "count(*) as cnt";
			}
			else
			{
				//if (i == 0) sql += "*";
			}

			
			// FROM clause
			
			if (System.Convert.ToBoolean(Utility.OrderedMap.CountElements(this.mTables)))
			{
				keys = this.mTables.GetKeysOrderedMap(null);
				
				n_tab = 0;
				
				for (i = 0; i < Utility.OrderedMap.CountElements(keys); i++)
				{
					if (this.mTables[keys[i]] != null)
					{
						n_tab++;
					}
				}
				
				from = ((TpTable)this.mTables[keys[0]]).GetName();
				
				if (n_tab > 1)
				{
					from += "\n";
					
					for (i = 1; i < n_tab; i++)
					{
						from = "(" + from;
						from += " LEFT JOIN ";
						from = from + ((TpTable)this.mTables[keys[i]]).GetName();
						from += " ON ";
						from += ((TpTable)this.mTables[keys[i]]).GetParentName() + "." + ((TpTable)this.mTables[keys[i]]).GetJoin();
						from += " = ";
						from += ((TpTable)this.mTables[keys[i]]).GetName() + "." + ((TpTable)this.mTables[keys[i]]).GetKey();
						from += ")\n";
					}
				}
				
				sql += " FROM " + from;
			}
			
			// WHERE
			
			num_conditions = Utility.OrderedMap.CountElements(this.mConditions);
			
			if (num_conditions > 0)
			{
				sql += " WHERE ";
				
				for (i = 0; i < num_conditions; ++i)
				{
					if (i > 0)
					{
						sql += " AND ";
					}
					
					sql = sql + this.mConditions[i].ToString();
				}
			}
			
			// GROUP BY
			
			if (this.mGroupAll)
			{
				
				sql += " GROUP BY ";
				
				i = 0;
				
				foreach ( string target_id in this.mSelect.Keys ) 
				{
					object target_sql = this.mSelect[target_id];
					++i;
					
					if (i > 1)
					{
						sql += ", ";
					}
					
					sql = sql + Utility.TypeSupport.ToString(target_sql);
				}
				
			}
			
			// ORDER BY			
			if (this.mOrderBy != null) 
			{
				sql += " ORDER BY ";
				
				if (Utility.OrderedMap.CountElements(this.mOrderBy) == 0)
				// order by all
				{
					i = 0;
					
					foreach ( string target_id in this.mSelect.Keys ) 
					{
						object target_sql = this.mSelect[target_id];
						++i;
						
						if (i > 1)
						{
							sql += ", ";
						}
						
						sql = sql + Utility.TypeSupport.ToString(target_sql);
					}
					
				}
				else
				{
					i = 1;
					
					foreach ( string concept_id in this.mOrderBy.Keys ) 
					{
						object descend = this.mOrderBy[concept_id];
						if (i > 1)
						{
							sql += ", ";
						}
						
						sql += this.GetTargetSql(concept_id);
						
						if (Utility.TypeSupport.ToBoolean(descend))
						{
							sql += " DESC";
						}
						
						++i;
					}
					
				}
			}
			
			return sql;
		}// end of member function GetSql
	}
}
