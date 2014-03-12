using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaxonBlaster
{
    public class Blaster
    {
        public static string SubmitTaxonMatches(List<string> geneSequences)
        {
            var req = WebRequest.Create("http://blast.ncbi.nlm.nih.gov/Blast.cgi?CMD=Put&QUERY=MKN&DATABASE=nr&PROGRAM=blastp&FILTER=L&HITLIST_SZE=500");
            //req.ContentType = "application/x-www-form-urlencoded";
            //req.Method = "POST";
            
            //var wr = new StreamWriter(req.GetRequestStream());
            //wr.Write("CMD=Put&QUERY=MKN&DATABASE=nr&PROGRAM=blastp&FILTER=L&HITLIST_SZE=500"); //CMD=Put&QUERY=" + string.Join(",", geneSequences));
            //wr.Flush();

            var response = req.GetResponse();

            var reader = new StreamReader(response.GetResponseStream());

            //todo return rid for submission
            return reader.ReadToEnd();
        }
    }
}
