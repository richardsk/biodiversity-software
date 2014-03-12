using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace TaxonBlaster
{
    public partial class TaxonBlasterForm : Form
    {
        public TaxonBlasterForm()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            var req = WebRequest.Create("http://www.ncbi.nlm.nih.gov/blast/Blast.cgi");
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            var wr = new StreamWriter(req.GetRequestStream());
            wr.Write("CMD=Put&QUERY=CTTGGTCATTTAGAGGAAGTAAAAGTCGTAACAAGGTTTCCGTAGGTGAACCTGCGGAAGGATCATTATTGAATACGATTGGTACTGATGCTGGCCCTTTACTGGGCATGTGCTCGTCCATCTATTTATCTTCTCTTGTGCACATCTTGTAGTCTTGGATGAACCCTTCGCATTCGTGCGGTCTGGGAGTTTGCGATTAAACCCGCTTCTCCTGCTTGTCCAAGGCTATGTTTTCATATACACTATAAAGTTACAGAATGTCTATTAACGACTTGTGCTAGTCACGGTCA");
            wr.Flush();

            var response = req.GetResponse();

        }
    }
}
