using System;
using System.IO;

namespace LSIDClient
{
	/**
	 * This implementation queries meta data by applying an XSLT transform on the 
	 * RDF and executes simple XPATH on the result.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class XSLTMetadata : LSIDMetadata 
	{
	
		//private static String RDF_STYLE_SHEET = "rdf-canonicalize.xsl";
	
		// XML namespaces
		//private static String RDF_NS_URI = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
		//private static String I3CP_NS_URI = "urn:lsid:i3c.org:predicates:";
		//private static String DC_NS_URI = "http://purl.org/dc/elements/1.1/";
	
		// XML prefixes
		private static String RDF_PREFIX = "rdf";
		//private static String I3CP_PREFIX = "i3cp";
		//private static String DC_PREFIX = "dc";
	
		// XML tag names
		//private static String RDF_DOCS_ELT = "RDF-DOCS";
		private static String RDF_ELT = "RDF";
		private static String DESCRIPTION_ELT = "Description";
	
		// XML attr names
		//private static String EXPIRES_ATTR = "expires";
	
		// XPath helpers
		private static String PATH_TO_DESCRIPTION = RDF_PREFIX + ":" + RDF_ELT + "/" + RDF_PREFIX + ":" + DESCRIPTION_ELT;
	
		//private TransformerFactory factory = TransformerFactory.newInstance();
		//private Document doc;
		//private Element rdfDocs;
		//private Element envNS;
	
		/**
		 * initialize the meta data store.
		 */
		public void init() 
		{
			try 
			{
				//			DocumentBuilder builder = new DocumentBuilderFactoryImpl().newDocumentBuilder();
				//			doc = builder.newDocument();
				//			rdfDocs = doc.createElement(RDF_DOCS_ELT);
				//			doc.appendChild(rdfDocs);
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e, "Internal error preparing rdf store");
			}
			//		envNS = doc.createElement("nsmappings");
			//    	envNS.setAttributeNS("http://www.w3.org/2000/xmlns/", "xmlns:" + RDF_PREFIX ,RDF_NS_URI);
			//    	envNS.setAttributeNS("http://www.w3.org/2000/xmlns/", "xmlns:" + I3CP_PREFIX ,I3CP_NS_URI);
			//    	envNS.setAttributeNS("http://www.w3.org/2000/xmlns/", "xmlns:" + DC_PREFIX ,DC_NS_URI);
		}
	
		/**
		 * get the DOM to perform queries on
		 */
		//	public Document getDocument() 
		//	{
		//		return doc;
		//	}

		/**
		 * @see LSIDMetadata#addMetadata(InputStream, Date)
		 */
		public void addMetadata(MetadataResponse metadata) 
		{
			//byte[] result = null;
			Stream inp = null;
			try 
			{
				inp = metadata.getMetadata();
				//todo
				// canonicalize the rdf
				//			Stream styleSheet = getClass().getResourceAsStream(RDF_STYLE_SHEET);
				//			Transformer transformer = factory.newTransformer(new StreamSource(styleSheet));
				//			ByteArrayOutputStream out = new ByteArrayOutputStream();
				//			transformer.transform(new StreamSource(in), new StreamResult(out));
				//			result = out.toByteArray();
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e, "Error applying transform to RDF");	
			} 
			finally 
			{
				try 
				{
					if (inp != null)
						inp.Close();
				} 
				catch (System.IO.IOException e) 
				{
					LSIDException.PrintStackTrace(e);
				}	
			}
			// parse the rdf and add it to our XML store
			try 
			{
				//			DOMParser parser = new DOMParser();
				//			parser.parse(new InputSource(new ByteArrayInputStream(result)));
				//			Document formattedDoc = parser.getDocument();
				//			Element root = (Element)formattedDoc.getFirstChild();
				//			Element improot = (Element)doc.importNode(root,true);
				//			rdfDocs.appendChild(improot);
				//			Date expiration = metadata.getExpires();
				//			if (expiration != null)
				//				improot.setAttribute(EXPIRES_ATTR,String.valueOf(expiration.getTime()));
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e,"Error parsing formatted RDF");
			} 
		}

		/**
		 * @see LSIDMetadata#getAbstract(LSID)
		 */
		public LSID getAbstract(LSID lsid) 
		{
			try 
			{
				String xpathStr = PATH_TO_DESCRIPTION + "/i3cp:storedas[@rdf:resource=\"" + lsid + "\"]/parent::node()/@rdf:about";
				//			Attr node = (Attr)XPathAPI.selectSingleNode(rdfDocs,xpathStr,envNS);
				//			if (node == null)
				//				return null;
				//			LSID newlsid = new LSID(node.getNodeValue());
				//			lsid.Abstr = newlsid;
				return null; //newlsid;
			} 
			catch (MalformedLSIDException e) 
			{
				throw new LSIDMetadataException(e,"bad lsid in meta data, getAbstract()");
			}
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e,"Error doing Xpath for getAbstract()");
			} 
		}

		/**
		 * @see LSIDMetadata#getFormat(LSID)
		 */
		public String getFormat(LSID lsid) 
		{
			try 
			{
				//	    	String xpathStr = PATH_TO_DESCRIPTION + "[@rdf:about=\"" + lsid + "\"]" + "/dc:format/@rdf:resource";
				//			Attr node = (Attr)XPathAPI.selectSingleNode(rdfDocs,xpathStr,envNS);
				//			if (node == null)
				//				return null;
				//			String format = node.getNodeValue();
				//			lsid.setFormat(format);
				return null; //format;
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e,"Error doing Xpath for getFormat()");
			}
		}

		/**
		 * @see LSIDMetadata#getInstances(LSID)
		 */
		public LSID[] getInstances(LSID lsid) 
		{
			try 
			{
				//	    	String xpathStr = PATH_TO_DESCRIPTION + "[@rdf:about=\"" + lsid + "\"]" + "/i3cp:storedas/@rdf:resource";
				//			NodeList attrs = XPathAPI.selectNodeList(rdfDocs,xpathStr,envNS);
				//			if (attrs == null)
				//				return null;
				//			LSID[] instances = new LSID[attrs.getLength()];
				//			for (int i=0;i<attrs.getLength();i++) {
				//				Attr attr = (Attr)attrs.item(i);
				//				LSID newlsid = new LSID(attr.getNodeValue());
				//				getFormat(newlsid);
				//				newlsid.setAbstr(lsid);
				//				instances[i] = newlsid;
				//			}
				return null; //instances;
			} 
			catch (MalformedLSIDException e) 
			{
				throw new LSIDMetadataException(e,"bad lsid in meta data, getInstances()");
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e,"Error doing Xpath for getInstances()");
			}
		}

		/**
		 * @see LSIDMetadata#getType(LSID)
		 */
		public String getType(LSID lsid) 
		{
			try 
			{
				//	    	String xpathStr = PATH_TO_DESCRIPTION + "[@rdf:about=\"" + lsid + "\"]" + "/rdf:type/@rdf:resource";
				//			Attr node = (Attr)XPathAPI.selectSingleNode(rdfDocs,xpathStr,envNS);
				//			if (node == null)
				//				return null;
				//			String type = node.getNodeValue();
				//			lsid.setType(type);
				return null; //type;
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e,"Error doing Xpath for getType()");
			}
		}
	
	

		/**
		 * @see LSIDMetadata#getForeignAuthorities(LSID)
		 */
		public LSIDAuthority[] getForeignAuthorities(LSID lsid) 
		{
			try 
			{
				//			String xpathStr = PATH_TO_DESCRIPTION + "[@rdf:about=\"" + lsid + "\"]" + "/i3cp:foreignauthority/child::text()";
				//			NodeList nodes = XPathAPI.selectNodeList(rdfDocs,xpathStr,envNS);
				//			if (nodes == null)
				//				return null;
				//			LSIDAuthority[] auths = new LSIDAuthority[nodes.getLength()];
				//			for (int i=0;i<nodes.getLength();i++) {
				//				String nodeStr = nodes.item(i).getNodeValue();
				//				auths[i] = new LSIDAuthority(nodeStr);
				//			}
				return null; //auths;
			} 
			catch (MalformedLSIDException e) 
			{
				throw new LSIDMetadataException(e,"bad lsid in meta data, getInstances()");
			} 
			catch (Exception e) 
			{
				throw new LSIDMetadataException(e,"Error doing Xpath for getInstances()");
			}
		}

	}
}
