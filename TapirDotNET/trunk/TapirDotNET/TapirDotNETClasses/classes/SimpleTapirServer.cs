using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections;

namespace TapirDotNET
{
    /// <summary>
    /// Simple Tapir Dot NET API Server implementation.  Intended to be overriden and extended for specific implementations.
    /// This implementation assumes that there is no mappings and DB configured through the standard admin configurator.  The databases is accessed directly, 
    /// through custom functionality.  The ping and metadata operations can work as normal (ie no db required).  
    /// </summary>
    public abstract class SimpleTapirServer : ITapirServer
    {
        internal class SimpleMapping
        {
            public string schema = "";
            public string schema_ns = "";
            public TpConcept concept;

            public SimpleMapping(string sch, string ns, string conc, bool searchable, string sqlfield)
            {
                schema = sch;
                schema_ns = ns;
                concept = new TpConcept();
                concept.SetId(conc);
                concept.SetSearchable(searchable);
                SingleColumnMapping scm = new SingleColumnMapping();
                scm.SetConcept(concept);
                scm.SetField(sqlfield);
                concept.SetMapping(scm);
            }
        }

        private ArrayList mMappings = new ArrayList();

        protected void AddConcept(string schema, string schema_namespace, string concept_id, bool isSearchable, string sql_feild_name)
        {
            if (GetMapping(schema, schema_namespace, concept_id) == null)
            {
                mMappings.Add(new SimpleMapping(schema, schema_namespace, concept_id, isSearchable, sql_feild_name));
            }
        }

        internal SimpleMapping GetMapping(string schema, string schema_namespace, string concept_id)
        {
            foreach (SimpleMapping sm in mMappings)
            {
                if (sm.schema == schema && sm.concept.GetId() == concept_id && sm.schema_ns == schema_namespace) return sm;
            }
            return null; 
        }

        internal SimpleMapping GetMapping(string concept_id)
        {
            foreach (SimpleMapping sm in mMappings)
            {
                if (sm.concept.GetId() == concept_id) return sm;
            }
            return null; 
        }

        /// <summary>
        /// Override to add the concept mappings from concept_id -> sql field name
        /// </summary>
        protected abstract void AddConcepts();

        /// <summary>
        /// Override to get the dataset itself from the DB, using the sql_fields passed in and the start, limit params.
        /// Number of records returned should equal "limit", or less if there are no more.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sql_fields"></param>
        /// <returns></returns>
        protected abstract DataSet GetInventoryResults(int start, int limit, bool doCount, System.Collections.ArrayList sql_concepts);

        /// <summary>
        /// Override to get the total count of all inventory records, using (grouped by) the sql_fields passed in.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sql_fields"></param>
        /// <returns></returns>
        protected abstract int GetInventoryCount(System.Collections.ArrayList sql_concepts);


        #region ITapirServer Members

        public TapirDotNET.TpResponse ProcessRequest(TapirDotNET.TpRequest req)
        {
            TpResponse resp = null;
            
            try
            {
                AddConcepts();
               
                //standard configurator execution
                string operation = req.GetOperation().ToLower();

                if (operation == "ping")
                {
                    //standard response
                    resp = new TpPingResponse(req);
                    resp.Process();
                }
                else if (operation == "capabilities")
                {
                    //custom response
                    string capXml = "\t<concepts>\n";

                    string lastSchema = "";

                    foreach (SimpleMapping sm in this.mMappings)
                    {
                        if (sm.schema != lastSchema)
                        {
                            if (lastSchema != "") capXml += "\t\t</schema>\n";
                            
                            capXml += "\t\t";
                            capXml += "<schema namespace=\"" + TpUtils.EscapeXmlSpecialChars(sm.schema_ns) + "\" " + "location=\"" + TpUtils.EscapeXmlSpecialChars(sm.schema) + "\">" + "\n";

                            lastSchema = sm.schema;
                        }

                        string searchable = (sm.concept.IsSearchable()) ? "true" : "false";

                        capXml += "\t\t\t";
                        capXml += "<mappedConcept searchable=\"" + searchable + "\">" + TpUtils.EscapeXmlSpecialChars(sm.concept.GetId()) + "</mappedConcept>\n";
                    }
                    capXml += "\t\t</schema>\n";

                    capXml += "\t</concepts>\n";

                    string capResp = req.GetResource().GetCapabilitiesXml(capXml);

                    TpUtils.WriteUTF8(HttpContext.Current.Response, capResp);
                }
                else if (operation == "metadata")
                {
                    //standard response
                    resp = new TpMetadataResponse(req);
                    resp.Process();
                }
                else if (operation == "inventory")
                {
                    //custom, optimised function
                    string inv = GetInventory(req);

                    if (inv.Length > 0) TpUtils.WriteUTF8(HttpContext.Current.Response, inv);
                }
                else if (operation == "search")
                {
                    //custom, optimised function
                }
                else
                {
                    // Unknown operation 
                    resp = new TpResponse(req);
                    resp.Init();
                    resp.ReturnError("Unknown operation \"" + operation + "\"");
                }

            }
            catch (Exception ex)
            {
                new TpDiagnostics().Append(TpConfigManager.DC_GENERAL_ERROR, "Error processing request : " + ex.Message, TpConfigManager.DIAG_ERROR);
            }

            return resp;
        }

        #endregion

        protected virtual void Error(string msg)
        {
            // Error tag
            TpUtils.WriteUTF8(HttpContext.Current.Response, "<error level=\"error\">" + TpUtils.EscapeXmlSpecialChars(msg) + "</error>");

            TpLog.debug(">> Returned Error: " + msg);
        }// end of member function Error

        protected virtual string GetInventory(TpRequest req)
        {
            string invResp = "";

			TpInventoryParameters inventory_parameters = (TpInventoryParameters)req.GetOperationParameters();
			
			if (inventory_parameters == null)
			{
				this.Error("No parameters specified");
				return "";
			}
			
            Utility.OrderedMap concepts = inventory_parameters.GetConcepts();
			
			if (concepts.Count == 0)
			{
                this.Error("No concepts specified");
				return "";
			}
            
			string concepts_xml = "";
            System.Collections.ArrayList sql_concepts = new System.Collections.ArrayList();

			foreach ( string concept_id in concepts.Keys ) 
			{       
                SimpleMapping sm = GetMapping(concept_id);
				if (sm == null)
				{
                    this.Error("Concept \"" + concept_id + "\" is not mapped");
					return "";
				}
								
				concepts_xml += "\n" + "<concept id=\"" + concept_id + "\" />";

                sql_concepts.Add(sm.concept);
			}

            int limit = req.GetLimit();

            if (limit == -1) limit = req.GetResource().GetSettings().GetMaxElementRepetitions();
            int start = req.GetStart();

            int matched = 0;            
            if (req.GetCount()) matched = GetInventoryCount(sql_concepts);

            DataSet dsResults = GetInventoryResults(start, limit, req.GetCount(), sql_concepts);

            // Inventory Header
		    invResp += "\n<inventory>";
		    invResp += "\n<concepts>";
		    invResp += concepts_xml;
		    invResp += "\n</concepts>";
			
		    // Inventory Records
		    int num_recs = 0;
		    int num_concepts = concepts.Count;
            
		    Utility.OrderedMap tag_names = concepts.GetValuesOrderedMap();
		
		    while (num_recs < dsResults.Tables[0].Rows.Count && (num_recs < limit))
		    {				
			    invResp += "\n<record";
				
			    if (req.GetCount())
			    {
				    invResp += " count=\"" + dsResults.Tables[0].Rows[num_recs][num_concepts] + "\"";
			    }
				
			    invResp += ">";
				
			    for (int i = 0; i < num_concepts; ++i)
			    {
				    invResp += "\n<" + tag_names[i].ToString() + ">";
                    SingleColumnMapping scm = (SingleColumnMapping)GetMapping(concepts.GetKeyAt(i)).concept.GetMapping();
                    string val = dsResults.Tables[0].Rows[num_recs][scm.GetField()].ToString();
				    invResp += TpServiceUtils.EncodeData(val, "UTF-8");
										
				    invResp += "</" + tag_names[i].ToString() + ">";
			    }
				
			    invResp += "\n</record>";	
				
			    num_recs++;
		    }
			
		    // Inventory Summary
			
		    invResp += "\n" + "<summary start=\"" + start.ToString() + "\"";
			
		    if (num_recs < dsResults.Tables[0].Rows.Count)
		    {
			    int next = start + limit;
				
			    invResp += " next=\"" + next.ToString() + "\"";
		    }
			    			
		    invResp += " totalReturned=\"" + num_recs.ToString() + "\"";
			
		    if (req.GetCount())
		    {
			    invResp += " totalMatched=\"" + matched.ToString() + "\"";
		    }
			
		    invResp += " />";
			
		    invResp += "\n</inventory>";

            return invResp;
            
        }

    }
}
