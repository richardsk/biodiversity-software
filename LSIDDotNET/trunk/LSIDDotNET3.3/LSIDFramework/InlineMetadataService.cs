using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net;

using LSIDClient;

namespace LSIDFramework
{
/**
 * 
 * This metadata service returns metadata specied inline in an XML document Metadata Service Description Language
 * (MSDL). A simple example document follows. The default format of entries is 'application/xml+rdf'.  
 * <p>
 * <p>
 * <pre>
 * 
 * &ltmetadata xmlns="http://www.ibm.com/LSID/Standard/msdl"&gt
 *	&ltlsid uri="urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:predicates:locus" location="inline" format="application/xml+rdf"&gt
 *		&ltrdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
 *				 xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#"&gt
 *			&ltrdf:Description rdf:about="urn:lsid:ncbi.nlm.nih.gov.lsid.i3c.org:predicates:locus"&gt
 *				 &ltrdfs:label&gtGenbank Locus&lt/rdfs:label&gt
 *			&lt/rdf:Description&gt
 *		&lt/rdf:RDF&gt
 *	&lt/lsid&gt
 *&lt/metadata&gt
 * 
 * </pre>
 * 
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 */
    public class InlineMetadataService : LSIDMetadataService 
    {
	
        public static String NAMESPACE = "http://www.ibm.com/LSID/Standard/msdl";
        public static String PREFIX = "msdl";
        public static String ROOT = "metadata";
	
        //private static SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
	
        // elements
        private static String LSID_ELT = "lsid";
	
        // attributes
        private static String LOCATION_ATTR = "location";
        private static String URI_ATTR = "uri";
        private static String FORMAT_ATTR = "format";
        private static String EXPIRES_ATTR = "expires";
		
	
        private Hashtable metadata = new Hashtable();

        /**
         * Construct a new service.  The service will be empty until load() is called. 
         */
        public InlineMetadataService() 
        {
        }
	
        /**
         * Load the MSDL from the given URI
         * @param URL the URI of the MSDL
         */
        public void loadFromUri(String uri) 
        {
            try 
            {
				WebRequest req = WebRequest.Create(uri);
				if (HTTPUtils.WebProxy != null) req.Proxy = new WebProxy(HTTPUtils.WebProxy);
				loadFromStream(req.GetRequestStream());				
            } 
            catch (IOException e) 
            {
                throw new LSIDServerException(e, "Error getting MSDL at: " + uri);
            }
        }
	
        /**
         * Load the MSDL from the given file location
         * @param File the file that contains the MSDL
         */
        public void loadFromFile(String file) 
        {
            try 
            {
                loadFromStream(File.OpenRead(file));
            } 
            catch (FileNotFoundException e) 
            {
                throw new LSIDServerException(e, "MSDL file not found: " + file);
            }
        }
	
        /**
         * Load the MSDL from the given XML String
         * @param String the XML String that contains the MSDL
         */
        public void loadFromXml(String xml) 
        {
            loadFromStream(new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(xml)));
        }
	
        /**
         * Load the MSDL from the given InputStream, close it on exit.
         * @param InputStream the input stream of XML that contains the MSDL
         */
        public void loadFromStream(Stream xml) 
        {
            try 
            {
                XmlDocument doc = new XmlDocument();
                StreamReader rdr = new StreamReader(xml);
                doc.LoadXml(rdr.ReadToEnd());
            	XmlNode root = doc.FirstChild;
            	while (!(root is XmlElement))
            		root = root.NextSibling;
            	load((XmlElement)root);
            } 
			catch (IOException e) 
			{
            	throw new LSIDServerException(e, "Error parsing MSDL");
            } 
			finally 
			{
            	try 
				{
            		if (xml != null)
            			xml.Close();
            	} 
				catch (IOException e)
				{
            		LSIDException.PrintStackTrace(e);
            	}	
            }
        }
	
        /**
         * Load the MSDL from the given DOM element
         * @param Element the DOM that contains the MSDL
         */
        public void load(XmlElement MSDL) 
        {
            // setup the parent expiration date.
            XmlAttribute dateAttr = MSDL.Attributes[EXPIRES_ATTR]; 
            DateTime expr = DateTime.MinValue;
            if (dateAttr != null && !dateAttr.Value.Equals("")) 
            {
                try 
                {
                    expr = SOAPUtils.parseDate(dateAttr.Value);
                } 
                catch (Exception e) 
                {
                    throw new LSIDServerException(e, "Error reading expiration date in MSDL: " + dateAttr.Value);
                }
            }
            // setup XML namespace 
            XmlElement envNS = MSDL.OwnerDocument.CreateElement("nsmappings", NAMESPACE);
            XmlNamespaceManager mgr = new XmlNamespaceManager(MSDL.OwnerDocument.NameTable);
            mgr.AddNamespace(PREFIX, NAMESPACE);
            String xpathStr = PREFIX + ":" + LSID_ELT;
            try 
            {
                XmlNodeList lsids = MSDL.SelectNodes(xpathStr, mgr);
                for (int i=0;i<lsids.Count;i++) 
                {
                    XmlElement lsidElt = (XmlElement)lsids.Item(i);
	    		
                    // compare this LSIDs expiration with the parent expiration and set the tighter one
	    		
                    DateTime lsidExp = expr;
                    XmlAttribute expAttr = null;
                    try 
                    {
                        expAttr = lsidElt.Attributes[EXPIRES_ATTR];
                        if (expAttr != null && expAttr.Value != "") 
                        {
                            DateTime d = SOAPUtils.parseDate(expAttr.Value);
                            if (lsidExp == DateTime.MinValue)
                                lsidExp = d;
                            else 
                            {
                                if (d < lsidExp)
                                    lsidExp = d;
                            }
                        }
                    } 
                    catch (Exception e) 
                    {
                        throw new LSIDServerException(e, "Error reading expiration date in MSDL: " + expAttr.Value);
                    }
	    		
                    String location = lsidElt.Attributes[LOCATION_ATTR].Value;
                    LSID lsid = null;
                    try 
                    {
                        lsid = new LSID(lsidElt.Attributes[URI_ATTR].Value);
                    } 
                    catch (MalformedLSIDException e) 
                    {
                        throw new LSIDServerException(e,"Bad LSID in MSDL");	
                    }
                    if (location.Equals(ServiceConfigurationConstants.INLINE)) 
                    {
                        XmlElement elt = null;
                        XmlNodeList kids = lsidElt.ChildNodes;
                        for (int j=0;j<kids.Count;j++) 
                        {
                            XmlNode node = kids.Item(j);
                            if (node is XmlElement) 
                            {
                                elt = (XmlElement)node;
                                break;
                            }	
                        }
						String fmt = null;
						if (lsidElt.Attributes[FORMAT_ATTR] != null) fmt = lsidElt.Attributes[FORMAT_ATTR].Value;
                        loadMetaData(lsid, elt, lsidExp, fmt);
                    } 
                    else  
                    {
                        String loc = null;
                        try 
                        {
                            xpathStr = "child::text()";
                            loc = lsidElt.SelectSingleNode(xpathStr).Value;
                            if (location.Equals(ServiceConfigurationConstants.FILE)) 
                            {
								String fmt = null;
								if (lsidElt.Attributes[FORMAT_ATTR] != null) fmt = lsidElt.Attributes[FORMAT_ATTR].Value;
                                loadMetaData(lsid, File.OpenRead(loc), lsidExp, fmt);
                            } 
                            else if (location.Equals(ServiceConfigurationConstants.URL)) 
                            {
                                Uri u = new Uri(loc);
								System.Net.WebRequest r = System.Net.WebRequest.Create(u);
								if (HTTPUtils.WebProxy != null) r.Proxy = new WebProxy(HTTPUtils.WebProxy);
								String fmt = null;
								if (lsidElt.Attributes[FORMAT_ATTR] != null) fmt = lsidElt.Attributes[FORMAT_ATTR].Value;
                                loadMetaData(lsid, r.GetResponse().GetResponseStream(), lsidExp, fmt);
                            }
                        } 
                        catch (Exception e) 
                        {
                            throw new LSIDServerException(e,"Error reading metadata file specified in MSDL:  " + loc);
                        }
                    }
                }
            } 
            catch (Exception e) 
            {
                throw new LSIDServerException(e,"Error reading metadata file specified in MSDL");
            }			
        }

        public MetadataResponse getMetadata(LSIDRequestContext ctx, String[] formats) 
        {
            LSID lsid = ctx.Lsid;
            Entry entry = (Entry)metadata[lsid.ToString()];
            if (entry == null)
                throw new LSIDServerException(LSIDServerException.NO_METADATA_AVAILABLE,"No metadata in msdl for " + lsid);
            byte[] bytes = entry.metadata;
            if (formats != null) 
            {
                Boolean found = false;
                for (int i=0;i<formats.Length; ) //i++) 
                {
                    if (formats[i].Equals(entry.format))
                        found = true;
                    break;				
                }
                if (!found)
                    throw new LSIDServerException(LSIDServerException.NO_METADATA_AVAILABLE_FOR_FORMATS,"No metadata found for given format"); 
            }
            if (bytes == null)
                throw new LSIDServerException(LSIDServerException.NO_METADATA_AVAILABLE,"No metadata in msdl for " + lsid);
            return new MetadataResponse(new MemoryStream(bytes),entry.expires,entry.format);
        }

        /**
         * @see LSIDService#initService(LSIDServiceConfig)
         */
        public void initService(LSIDServiceConfig config) 
        {
        }
	
        /**
         * load metadata from an element, store the data in the table keyed by lsid
         */
        private void loadMetaData(LSID lsid, XmlElement md, DateTime expr, String format) 
        {
            try 
			{
				Entry entry = new Entry(System.Text.UTF8Encoding.UTF8.GetBytes(md.OuterXml), expr);
            	if (format != null && !format.Equals(""))
            		entry.format = format;
            	metadata[lsid.ToString()] = entry;
            } 
			catch (IOException e) 
			{
            	throw new LSIDServerException(e, "Error serializing RDF Document");	
            }
        }
	
        /**
         * load meta data from an input stream.
         */
        private void loadMetaData(LSID lsid, Stream xml, DateTime expr, String format) 
        {
            XmlDocument doc = new XmlDocument();
            try 
            {
                StreamReader rdr = new StreamReader(xml);
                doc.LoadXml(rdr.ReadToEnd());
            	loadMetaData(lsid,(XmlElement)doc.FirstChild, expr, format);
            } 
			catch (IOException e) 
			{
            	throw new LSIDServerException(e, "Error parsing metadata in MSDL");
			} 
			finally 
			{
            	try 
				{
            		if (xml != null)
            			xml.Close();
            	} 
				catch (IOException e)
				{
            		LSIDException.PrintStackTrace(e);
            	}	
            }	
        }

        /**
         * private struct to store a meta data entry: a byte array and an optional Date
         */
        private class Entry 
        {
		
            public byte[] metadata;
            public String format = MetadataResponse.RDF_FORMAT;
            public DateTime expires;	
		
            public Entry (byte[] metadata, DateTime expires) 
            {
                this.metadata = metadata;
                this.expires = expires;
            }
	
        }
	
	
    }

}
