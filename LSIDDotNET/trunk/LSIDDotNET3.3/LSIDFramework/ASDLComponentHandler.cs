using System;
using System.Xml;
using System.Collections;

namespace LSIDFramework
{
/**
 * 
 * Handles authority service components of type "asdl".  Instantiates a GatewayAuthority with provided ASDL document.
 * 
 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
 * 
 */
    public class ASDLComponentHandler : ServiceComponentHandler 
    {
        /**
         * @see ServiceComponentHandler#loadComponent(Element, LSIDServiceConfig)
         */
        public LSIDService loadComponent(XmlElement compElt, LSIDServiceConfig config) 
        {
            XmlAttribute location = compElt.Attributes[ServiceConfigurationConstants.LOCATION_ATTR];
            if (location == null)
                throw new LSIDConfigurationException("Must specify location of ASDL document");
            // setup XML namespace 
            XmlElement envNS = compElt.OwnerDocument.CreateElement("nsmappings", ServiceConfigurationConstants.RSDL_NS_URI);
            XmlNamespaceManager mgr = new XmlNamespaceManager(compElt.OwnerDocument.NameTable);
            try 
            {
                GatewayAuthority ga = new GatewayAuthority();
                if (location.Value.Equals(ServiceConfigurationConstants.FILE)) 
                {
                    String xpathStr = "child::text()";
                    String fileStr = compElt.SelectSingleNode(xpathStr,mgr).Value;
                    ga.loadByFile(fileStr);
                } 
                else if (location.Value.Equals(ServiceConfigurationConstants.URL)) 
                {
                    String xpathStr = "child::text()";
                    String urlStr = compElt.SelectSingleNode(xpathStr,mgr).Value;
                    ga.loadByURL(urlStr);	
				} 
                else if (location.Value.Equals(ServiceConfigurationConstants.INLINE)) 
                {
                    envNS = compElt.OwnerDocument.CreateElement(GatewayAuthority.NAMESPACE);
                    String xpathStr = GatewayAuthority.PREFIX + ":" + GatewayAuthority.ROOT;
                    XmlElement asdlElt = (XmlElement)compElt.SelectSingleNode(xpathStr,mgr);
                    ga.load(asdlElt);	
                } 
                else 
                    throw new LSIDConfigurationException("Bad ASDL location: " + location);
                return ga;
            } 
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e,"Error handling ASDL component");
            }
        }

        public ArrayList knownServices()
        {	
            ArrayList l = new ArrayList();
            l.Add("auth");	
            return l;
        }
    }
}
