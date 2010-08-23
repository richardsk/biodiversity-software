using System;
using System.Xml;
using System.Collections;

namespace LSIDFramework
{
/**
 * 
 * Handles metadata service components of type "msdl".  Instantiates an InlineMetadataService with provided MSDL document.
 * 
 * 
 */
    public class MSDLComponentHandler : ServiceComponentHandler
    {

        /**
         * @see ServiceComponentHandler#loadComponent(Element, LSIDServiceConfig)
         */
        public LSIDService loadComponent(XmlElement compElt, LSIDServiceConfig config) 
        {
            XmlAttribute location = compElt.Attributes[ServiceConfigurationConstants.LOCATION_ATTR]; 
            if (location == null)
                throw new LSIDConfigurationException("Must specify location of MSDL document");
            // setup XML namespace 
            XmlElement envNS = compElt.OwnerDocument.CreateElement("nsmappings");
            XmlNamespaceManager mgr = new XmlNamespaceManager(compElt.OwnerDocument.NameTable);
            try 
            {
                InlineMetadataService imds = new InlineMetadataService();
                if (location.Value.Equals(ServiceConfigurationConstants.FILE)) 
                {
                    String xpathStr = "child::text()";
                    String fileStr = compElt.SelectSingleNode(xpathStr,mgr).Value;
                    imds.loadFromFile(fileStr ); 
                } 
                else if (location.Value.Equals(ServiceConfigurationConstants.URL)) 
                {
                    String xpathStr = "child::text()";
                    String urlStr = compElt.SelectSingleNode(xpathStr,mgr).Value;
                    imds.loadFromUri(urlStr);
                } 
                else if (location.Value.Equals(ServiceConfigurationConstants.INLINE)) 
                {
                    mgr.AddNamespace(InlineMetadataService.PREFIX, InlineMetadataService.NAMESPACE);
                    String xpathStr = InlineMetadataService.PREFIX + ":" + InlineMetadataService.ROOT;
                    XmlElement asdlElt = (XmlElement)compElt.SelectSingleNode(xpathStr,mgr);
                    imds.load(asdlElt);	
                } 
                else 
                    throw new LSIDConfigurationException("Bad MSDL location: " + location);
                return imds;
            } 
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e,"Error handling MSDL component");
            }
        }
	
        public ArrayList knownServices()
        {
            ArrayList l = new ArrayList();
            l.Add("meta");	
            return l;
        }
    }
}
