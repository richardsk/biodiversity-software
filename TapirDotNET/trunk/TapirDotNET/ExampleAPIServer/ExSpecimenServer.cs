using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

using TapirDotNET;

namespace ExampleAPIServer
{
    /// <summary>
    /// Example Tapir Dot NET API Server implementation.
    /// This implementation assumes that there is no mappings and DB configured through the standard admin configurator.  The databases is accessed directly, 
    /// through custom functionality.  The ping and metadata operations can work as normal (ie no db required).  The capabilities returns a fixed document of 
    /// mappings that match what the code functionality can provide (in this case just DwC mappings).
    /// </summary>
    public class ExSpecimenServer : SimpleTapirServer
    {
        protected override void AddConcepts()
        {
            AddConcept("http://rs.tdwg.org/dwc/tdwg_dw_core.xsd", "http://rs.tdwg.org/dwc/dwcore", "http://rs.tdwg.org/dwc/dwcore/GlobalUniqueIdentifier", true, "SpecimenLSID");
            AddConcept("http://rs.tdwg.org/dwc/tdwg_dw_core.xsd", "http://rs.tdwg.org/dwc/dwcore", "http://rs.tdwg.org/dwc/dwcore/BasisOfRecord", true, "SpecimenType");
            AddConcept("http://rs.tdwg.org/dwc/tdwg_dw_core.xsd", "http://rs.tdwg.org/dwc/dwcore", "http://rs.tdwg.org/dwc/dwcore/CatalogNumber", true, "AccessionNumber");
            AddConcept("http://rs.tdwg.org/dwc/tdwg_dw_core.xsd", "http://rs.tdwg.org/dwc/dwcore", "http://rs.tdwg.org/dwc/dwcore/ScientificName", true, "FullName");
            AddConcept("http://rs.tdwg.org/dwc/tdwg_dw_core.xsd", "http://rs.tdwg.org/dwc/dwcore", "http://rs.tdwg.org/dwc/dwcore/Collector", true, "VerbatimCollector");
        }

        protected override DataSet GetInventoryResults(int start, int limit, bool doCount, System.Collections.ArrayList sql_concepts)
        {
            DataSet ds = null;
            
            int total = start + limit + 1;
            string select = "";

            foreach (TpConcept conc in sql_concepts)
            {
                SingleColumnMapping scm = (SingleColumnMapping)conc.GetMapping(); //always a single column mapping
                if (select.Length > 0) select += ", ";
                select += scm.GetField();
            }

            using (SqlConnection cnn = new SqlConnection("data source=devserver04\\sql2005;initial catalog=cis_dev;user id=cis_user;password=[C15_Us3r];persist security info=True;"))
            {
                string sql = "select top " + total.ToString() + " " + select;
                if (doCount) 
                {
                    sql += ", count(*)";
                }
                sql += " from vwDarwinCore group by " + select + " order by " + select;

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = sql;
                cnn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < start; i++) ds.Tables[0].Rows.RemoveAt(0);
            }

            return ds;
        }

        protected override int GetInventoryCount(System.Collections.ArrayList sql_concepts)
        {
            int count = 0;

            string select = "";

            foreach (TpConcept conc in sql_concepts)
            {
                SingleColumnMapping scm = (SingleColumnMapping)conc.GetMapping();
                if (select.Length > 0) select += ", ";
                select += scm.GetField();
            }

            using (SqlConnection cnn = new SqlConnection("data source=devserver04\\sql2005;initial catalog=cis_dev;user id=cis_user;password=[C15_Us3r];persist security info=True;"))
            {
                string sql = "select count(*) from vwDarwinCore group by " + select;

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = sql;
                cnn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0) count = ds.Tables[0].Rows.Count;
            }

            return count;
        }

    }
}
