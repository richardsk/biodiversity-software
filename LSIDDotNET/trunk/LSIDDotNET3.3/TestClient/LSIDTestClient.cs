using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using LSIDClient;

namespace TestClient
{
    public class LSIDTestClient : ResolutionListener
	{
		public delegate void UpdateResults(string res);
		public UpdateResults UpdateDelegate  = null;

        // NCBI

        // Genbank
//        public static String lsid10 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank_gi:30350027";
//        public static String lsid11 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank:bm872070";
//        public static String lsid12 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank:u34074";
//        public static String lsid13 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank:l17325.1";
//        public static String lsid14 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank:x00353";
//        public static String lsid15 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank:nm_002165";
//        public static String lsid16 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank_gi:30407099";
//        //public static String lsid16 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:genbank:nm_bad";
//
//        // Proteins 
//        public static String lsid21 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:proteins:aah52812";
//        public static String lsid22 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:proteins_gi:871485";
//
//        // Pubmed
//        public static String lsid30 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:pubmed:12441807";
//
//        // OMIM 
//        public static String lsid40 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:omim:601077";
//        public static String lsid41 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:omim:605956";
//        public static String lsid42 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:omim:601077-text";
//        public static String lsid43 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:omim:606518";
//        public static String lsid44 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:omim:158050";
//
//        // Locus link
//        public static String lsid50 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:locuslink:3397";
//        public static String lsid51 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:predicates:transvar";
//
//        // predicates
//        public static String lsid60 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:predicates:lsid_xref";
//        public static String lsid61 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:predicates:locus";
//        public static String lsid62 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:types:mrna";
//
//        // Ensembl
//        public static String lsid70 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_gene:ensg00000002016";
//        public static String lsid71 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_gene:ensg00000002016-fasta";
//        public static String lsid72 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_exon:ense00001155775";
//        public static String lsid73 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_exon:ense00001155775-fasta";
//        public static String lsid74 = "urn:lsid:ensembl.org.lsid.biopathways.org:predicates:confidence";
//        public static String lsid75 = "urn:lsid:ensembl.org.lsid.biopathways.org:predicates:exon";
//        public static String lsid76 = "urn:lsid:ensembl.org.lsid.biopathways.org:types:gene";
//        public static String lsid77 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_ref:12153";
//        public static String lsid78 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_analysis:61";
//        public static String lsid79 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_translation:18099";
//        public static String lsid80 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_transcript:18099";
//        //public static String lsid81 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_clone:ab015355";
//        //public static String lsid82 = "urn:lsid:ensembl.org.lsid.biopathways.org:homosapiens_clonefragment:24";
//
//        // UCSC
//        public static String lsid90 = "urn:lsid:genome.ucsc.edu.lsid.biopathways.org:hg13:chr2_1-1000";
//        public static String lsid91 = "urn:lsid:genome.ucsc.edu.lsid.biopathways.org:types:segment";
//        public static String lsid92 = "urn:lsid:genome.ucsc.edu.lsid.biopathways.org:formats:dasgff";
//        public static String lsid93 = "urn:lsid:genome.ucsc.edu.lsid.biopathways.org:predicates:type_reference";
//
//        // Swissprot
//
//        public static String lsid100 = "urn:lsid:ebi.ac.uk.lsid.biopathways.org:swissprot-proteins:p01879";
//        public static String lsid101 = "urn:lsid:ebi.ac.uk.lsid.biopathways.org:swissprot-data:p01879-sprot";
//        public static String lsid102 = "urn:lsid:ebi.ac.uk.lsid.biopathways.org:swissprot-proteins:q9hc29";
//
//        // Gene Ontology
//
//        public static String lsid110 = "urn:lsid:geneontology.org.lsid.biopathways.org:go.id:0003673";
//        public static String lsid111 = "urn:lsid:geneontology.org.lsid.biopathways.org:types:gene_ontology";
//        public static String lsid112 = "urn:lsid:geneontology.org.lsid.biopathways.org:go.id:0003674";
//        public static String lsid113 = "urn:lsid:geneontology.org.lsid.biopathways.org:types:molecular_function";
//
//        // Hugo
//        public static String lsid120 = "urn:lsid:gene.ucl.ac.uk.lsid.biopathways.org:hugo:MVP";
//
//        public static String lsid200 = "urn:lsid:lsid.biopathways.org:formats:fasta";

        // PDB
//        public static String lsid01 = "URN:lsid:PDB.org:PDB:1AFT";
//        public static String lsid02 = "urn:lsid:pdb.org:PDB:1AFT-MMCIF";
//        public static String lsid03 = "urn:lsid:pdb.org:PDB:1AFT-PDB";
//        public static String lsid04 = "urn:lsid:pdb.org:PDB:1AFT-FASTA";
//        //		 */
//        public static String lsid999 = "urn:lsid:ncbi.nlm.nih.gov.lsid.biopathways.org:bad:object";
//
//        public static String lsid1000 = "urn:lsid:mygrid.org.uk:test:1";
//	
//        public static String lsid1001 = "urn:lsid:gdb.org:GenomicSegment:GDB132938";

//		public static String lsid2000 = "urn:lsid:indexfungorum.org:Names:213645";
		public static String lsid2001 = "urn:lsid:indexfungorum.org:Names:213649";
		public static String lsid2002 = "urn:lsid:lsidsample.org:sample:12345";

        public void RunTests()
        {
            ///////

			string res = "";
			
			System.Reflection.FieldInfo[] fields = typeof(LSIDTestClient).GetFields(); //(System.Reflection.BindingFlags.Static);
			for (int i = 0; i < fields.Length; ++i) 
			{
				if (fields[i].IsStatic)
				{
					string s = (string)fields[i].GetValue(this);
					res = "Test authority : " + testAuthority(s, LSIDWSDLWrapper.HTTP, null) + "\r\n";
					UpdateDelegate(res);
					res = "Test authority : " + testAuthority(s, LSIDWSDLWrapper.SOAP, null) + "\r\n";
					UpdateDelegate(res);
				}
			}
			
            //testAggregateMetadata(new LSID(lsid40));
            res = "Test file : " + testFile(new LSID(lsid2002)) + "\r\n"; //lsid11
			UpdateDelegate(res);
            //res = testAuthorityAsync(new LSID(lsid2002)) + "\r\n"; //lsid42
			//UpdateDelegate(res);
			res = "Test data by range : " + testDataByRange(lsid2002,5,4, LSIDWSDLWrapper.SOAP) + "\r\n"; //lsid10
			UpdateDelegate(res);
			//todo implement metadata stores?
            res = "Test metadata store : " + testMetadataStore(lsid2002, LSIDWSDLWrapper.SOAP) + "\r\n"; //lsid40
			UpdateDelegate(res);

            // test FAN
            res = "Test FAN : " + testFANNotify(new LSID(lsid2002),new LSIDAuthority("smithdansauthority.org")) + "\r\n"; //lsid30
			UpdateDelegate(res);
            testFANNotify(new LSID(lsid2002),new LSIDAuthority("joesauthority.org")); //lsid30
            res = "Test FAN revoke : " + testFANRevoke(new LSID(lsid2002),new LSIDAuthority("joesauthority.org")); //lsid40
			UpdateDelegate(res);
            res = "Test FAN lookup : " + testFANLookup(new LSID(lsid2002)) + "\r\n";
			UpdateDelegate(res);

            // test assigning service
            res = "Test assigning : " + testAssigning("http://localhost:80/authority/assigning") + "\r\n";
			UpdateDelegate(res);
            //res = testAssigning("http://lsid.biopathways.org:9090/authority/assigning") + "\r\n";
			//UpdateDelegate(res);
            // maintain the cache
            LSIDCache.load().maintainCache();	
        }

		public static String getMetadata(string lsidString, LSIDCredentials cred)
		{
			LSID lsid = new LSID(lsidString);
			LSIDResolver resolver = new LSIDResolver(lsid, cred);
			String output = lsid.ToString() + "\r\n";
			try 
			{				
				HTTPLocation p = new HTTPLocation("myres.org", 80, "");
				MetadataResponse resp = resolver.getMetadata();
				System.IO.StreamReader inp = new System.IO.StreamReader(resp.getMetadata());
				output += inp.ReadToEnd();
				inp.Close();
			}
			catch(Exception ex)
			{
				output += "Error: " + ex.Message + "\r\n";      
			}
				
			return output;
		}

        public static String testAuthority(String lsidString, String protocol, LSIDCredentials cred) 
        {
            LSID lsid = new LSID(lsidString);
            LSIDResolver resolver = new LSIDResolver(lsid, cred);
            String output = lsid.ToString() + "\r\n";
            try 
            {
                LSIDWSDLWrapper wrapper = resolver.getWSDLWrapper();

				Enumeration en = wrapper.getMetadataPortNamesForProtocol(protocol);
				while (en.hasMoreElements())
				{
					output += "Port Name : " + en.nextElement().ToString() + "\r\n";
				}

                // get metadata
                LSIDMetadataPort port = wrapper.getMetadataPortForProtocol(protocol);
                MetadataResponse resp = resolver.getMetadata(port);
                System.IO.StreamReader inp = new System.IO.StreamReader(resp.getMetadata());
                
				output += "Metadata: " + inp.ReadToEnd() + "\r\n";
                inp.Close();

                // test getting the data over SOAP if available
                LSIDDataPort dataport = resolver.getWSDLWrapper().getDataPortForProtocol(protocol);
                if (dataport == null) 
                {
                    output += "No data \r\n";
                } 
                else 
                {
                    inp = null;

                    // tell the server not to cache anything
                    //dataport.addProtocolHeader("Pragma","no-cache");
                    //dataport.addProtocolHeader("Cache-Control","no-cache");

                    inp = new System.IO.StreamReader(resolver.getData(dataport));

                    output += "Data: " + inp.ReadToEnd() + "\r\n";
                    inp.Close();
                }


            } 
            catch (Exception e) 
            {
                output += "Error: " + e.Message + "\r\n";      
            }

            return output;

        }

        public static string testDataByRange(String lsidString, int start, int length, String protocol) 
        {
			string res = "";
			try
			{
				LSID lsid = new LSID(lsidString);
				LSIDResolver resolver = new LSIDResolver(lsid);
				System.IO.StreamReader inp = new System.IO.StreamReader(resolver.getData(start, length));
				res = inp.ReadToEnd();
				//System.err.println(inp.available());
				inp.Close();
			}
			catch(LSIDException e)
			{
				res += e.Message;
			}
			return res;
        }

        public static string testMetadataStore(String lsidString, String protocol)
        {
			string res = "";
			try
			{
				//todo implement
//				LSID lsid = new LSID(lsidString);
//				LSIDResolver resolver = new LSIDResolver(lsid);			
//				LSIDMetadata metadata = resolver.getMetadataStore(resolver.getWSDLWrapper().getMetadataPortForProtocol(protocol));
//				LSID[] instances = metadata.getInstances(lsid);
//				for (int i = 0; i < instances.Length; i++) 
//				{
//					res += "inst: " + instances[i].ToString();
//					res += "fmt: " + instances[i].Format;
//					res += "abs: " + instances[i].Abstr;
//				}
			}
			catch(Exception ex)
			{
				res = ex.Message + "\r\n";
			}
			return res;
        }

        public static string testAuthorityAsync(LSID lsid) 
        {
			string res = "";
			//todo
            LSIDTestClient testAsync = new LSIDTestClient();
            AsyncLSIDResolver alr = new AsyncLSIDResolver(testAsync, lsid);
            testAsync.storeLSID(alr.getWSDLWrapper(), lsid);
            testAsync.storeLSID(alr.getMetadata(), lsid);
            testAsync.storeLSID(alr.getData(), lsid);
            alr.destroy();

			return res;
        }

        // instance methods and fields for testing the asynchronous API

        private Hashtable lsids = new Hashtable();

        public void storeLSID(int requestID, LSID lsid) 
        {
            lsids.Add(requestID, lsid);
        }

        public LSID getLSID(int requestID) 
        {
            return (LSID) lsids[requestID];
        }

        /**
         * @see com.ibm.lsid.client.async.ResolutionListener#getDataInputStreamComplete(InputStream, int)
         */
        public void getDataComplete(int requestID, System.IO.Stream data) 
		{
			StreamReader rdr = new StreamReader(data);
            try 
            {
                if (UpdateDelegate != null) UpdateDelegate(rdr.ReadToEnd() + "\r\n");
            } 
            finally 
            {
                try 
                {
                    data.Close();
                } 
                catch (Exception e) 
                {			
					LSIDException.WriteError(e.Message);
                }
            }
        }

        /**
         * @see com.ibm.lsid.client.async.ResolutionListener#getMetaDataComplete(LSIDMetadata, int)
         */
        public void getMetadataStoreComplete(int requestID, LSIDMetadata metadata) 
        {
            try 
            {
                System.Windows.Forms.MessageBox.Show(
                    "REQ: " + requestID + " Metadata Type: " + metadata.getType(getLSID(requestID)));
            } 
            catch (Exception e) 
            {
                LSIDException.WriteError(e.Message);
            }
        }

        /**
         * @see com.ibm.lsid.client.async.ResolutionListener#getMetaDataInputStreamComplete(ExpiringResponse, int)
         */
        public void getMetadataComplete(int requestID, MetadataResponse metadata) 
        {
            System.IO.StreamReader mdata = new System.IO.StreamReader( metadata.getMetadata());
            try 
            {
                System.Windows.Forms.MessageBox.Show(mdata.ReadToEnd());
            } 
            catch (Exception e) 
            {
                LSIDException.WriteError(e.Message);
            } 
            finally 
            {
                try 
                {
                    mdata.Close();
                } 
                catch (Exception e) 
                {
                    LSIDException.WriteError(e.Message);
                }
            }
        }

        /**
         * @see com.ibm.lsid.client.async.ResolutionListener#getResolveAuthorityComplete(LSIDAuthority, int)
         */
        public void resolveAuthorityComplete(int requestID, LSIDAuthority authority) 
        {
            System.Windows.Forms.MessageBox.Show(
                "REQ: " + requestID + " Authority URL: " + authority.getUrl());
        }

        /**
         * @see com.ibm.lsid.client.async.ResolutionListener#getWSDLWrapperComplete(LSIDWSDLWrapper, int)
         */
        public void getWSDLWrapperComplete(int requestID, LSIDWSDLWrapper wrapper) 
        {
            System.Windows.Forms.MessageBox.Show(
                "REQ: " + requestID + " WSDL Length: " + wrapper.getWSDL().Length);
        }

        /**
         * @see com.ibm.lsid.client.async.ResolutionListener#requestFailed(LSIDException, int)
         */
        public void requestFailed(int requestID, LSIDException cause) 
        {
            System.Windows.Forms.MessageBox.Show(
                "REQ: " + requestID + " failed");
            cause.PrintStackTrace();
        }

        public static string testFile(LSID lsid) 
        {
			string res = "";

        	try 
			{
        		LSIDResolver resolver = new LSIDResolver(lsid);
				FileLocation loc = new FileLocation("Test.txt");
    
        		Stream data = resolver.getData(loc);
				res += data.Length.ToString() + ": ";
				byte[] bytes = new byte[1024];
				StreamReader rdr = new StreamReader(data);
        		res += rdr.ReadToEnd() + "\r\n";
        		rdr.Close();
    
        		Stream mdata = resolver.getMetadata(loc, null).getMetadata();
				res += mdata.Length.ToString() + ": ";
        		
				rdr = new StreamReader(mdata);
				res += rdr.ReadToEnd() + "\r\n";
				rdr.Close();
        	} 
			catch (Exception e) 
			{
        		res = "Error : " + e.Message;
        	}
			return res;
        }

        public static string testFANNotify(LSID lsid, LSIDAuthority foreignAuthority) 
        {
			string res = "ok";
			try
			{
				LSIDResolver.notifyForeignAuthority(foreignAuthority, lsid, null);
			}
			catch(Exception ex)
			{
				res = "Error: " + ex.Message;
			}
			return res;
        }

        public static string testFANRevoke(LSID lsid, LSIDAuthority foreignAuthority) 
        {
			string res = "ok";
			try
			{
				LSIDResolver.revokeNotificationForeignAuthority(foreignAuthority, lsid, null);
			}
			catch(Exception e)
			{
				res = "Error : " + e.Message;
			}

			return res;
        }

        public static string testFANLookup(LSID lsid) 
        {
			string res = "";
			
			try
			{
				LSIDResolver resolver = new LSIDResolver(lsid);
				LSIDWSDLWrapper wrapper = resolver.getWSDLWrapper();
				String service = (String) wrapper.getServiceNames().nextElement();
				//System.err.println("Getting all metadata using service: " + service);
				Enumeration portnames = wrapper.getMetadataPortNamesForService(service);
				LSIDMetadata metadata = null;
				while (portnames.hasMoreElements()) 
				{
					String portname = (String) portnames.nextElement();
					LSIDMetadataPort port = wrapper.getMetadataPort(portname);
					
					//todo metadata = resolver.getMetadataStore(port);
				}
				//System.err.println(metadata.getClass().getName());
				//todo res += metadata.GetType().ToString();
				
//				LSIDAuthority[] auths = metadata.getForeignAuthorities(lsid);
//				for (int i = 0; i < auths.Length; i++) 
//				{
//					res += auths[i] + "\r\n";
//					//System.err.println(auths[i]);
//				}
			}
			catch(Exception ex)
			{
				res = "Error : " + ex.Message;
			}

			return res;
        }
        //
        //	public static void testAggregateMetadata(LSID lsid) 
        //    {
        //		JenaMetadataFactory factory = new JenaMetadataFactory();
        //		Hashtable properties = new Hashtable();
        //		properties.put(JenaMetadataFactory.MODEL_TYPE,JenaMetadataFactory.MEM_MODEL);
        //		factory.setProperties(properties);
        //		JenaMetadataStore store =  (JenaMetadataStore) factory.createInstance();
        //		store.addAllMetadata(lsid);
        //		Model model = store.getModel();
        //		model.write(System.out);		
        //	}
	
        public static string testAssigning(String endpoint)
        {
			string res = "";
			
			try
			{
				LSIDAssigner assigner = new LSIDAssigner(new SOAPLocation(endpoint));
				// assign an LSID with some props
				Properties props = new Properties();
				props.addProperty("smith","dan");
				props.addProperty("bh","szekel");
				LSID lsid = assigner.assignLSID("myauth", "myns", props);
				res += "assigned LSID: " + lsid + "\r\n";
				//System.err.println("assigned LSID: " + lsid);
		
		
				// assign an LSID for a new revision
				lsid = assigner.assignLSIDForNewRevision(lsid);
				res += "revised lsid: " + lsid + "\r\n";
				//System.err.println("revised lsid: " + lsid);
			
		
				// assign an LSID from a list
				LSID[] suggested = {new LSID("urn:lsid:myauth:myns:whos"),
									   new LSID("urn:lsid:myauth:myns:on"),
									   new LSID("urn:lsid:myauth:myns:first")};
				lsid = assigner.assignLSIDFromList(props,suggested);
				res += "From list LSID: " + lsid + "\r\n";
				//System.err.println("From list LSID: " + lsid);
		
				// get LSID pattern
				String pattern = assigner.getLSIDPattern("myauth", "myns", props);
				res += "assigned Pattern: " + pattern + "\r\n";
				//System.err.println("assigned Pattern: " + pattern);
		
				// get LSID pattern from a list
				String[] suggestedStrings = {"urn:lsid:myauth:myns",
												"urn:lsid:myauth:dansns",
												"urn:lsid:myauth:bensns"};
				pattern = assigner.getLSIDPatternFromList(props,suggestedStrings);
				res += "assigned Patter: " + pattern + "\r\n";
				//System.err.println("assigned Patter: " + pattern);
		
				// get the property names
				String[] names = assigner.getAllowedPropertyNames();
				for (int i=0;i<names.Length;i++)
					res += names[i] + " " + "\r\n";
				//System.err.print(names[i] + " ");

				//System.err.println();
			
				// get the list of authorities and namespaces
				String[][] authns = assigner.getAuthoritiesAndNamespaces();
				for (int i=0;i<authns.Length;i++) 
				{
					res += authns[i][0] + ":" + authns[i][1] + "\r\n";
					//System.err.println(authns[i][0] + ":" + authns[i][1]);
				}
			}
			catch(Exception ex)
			{
				res = "Error : " + ex.Message;
			}

			return res;
        }
    }
}
