using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace LSIDFramework
{
    /**
     * 
     * Handler to load service component of type="class".  The body of components 
     * 
     */
    public class LSIDServiceComponentHandler : ServiceComponentHandler
    {	
        private static String[] COMPONENT_NAMES = {"auth","data","meta","authentication","assn"};
        private static Type[] COMPONENT_INTERFACES = {
                                                         typeof(LSIDAuthorityService),
                                                         typeof(LSIDDataService),
                                                         typeof(LSIDMetadataService),
                                                         typeof(LSIDSecurityService),
                                                         typeof(LSIDAssigningService) };
                
        private static Hashtable NAME_TO_INTERFACE = new Hashtable();
    
        static LSIDServiceComponentHandler()
        {
            for (int i=0;i<COMPONENT_NAMES.Length;i++) 
            {
                NAME_TO_INTERFACE.Add(COMPONENT_NAMES[i],COMPONENT_INTERFACES[i]);	
            }
        }         
        
	
        public static void checkComponent(String elementLocalName, LSIDService service) 
        {
            if (service.GetType().GetInterface((NAME_TO_INTERFACE[elementLocalName]).ToString()) == null)
                throw new LSIDConfigurationException("Elements of " + elementLocalName +" must implement " + NAME_TO_INTERFACE[elementLocalName]);
        }

        /**
            * @see ServiceComponentHandler#loadComponent(Element, LSIDServiceConfig)
            */
        public LSIDService loadComponent(XmlElement compElt, LSIDServiceConfig config) 
        {
            try 
            {
                XmlElement envNS = compElt.OwnerDocument.CreateElement("nsmappings"); 
                
                String xpathStr = "child::text()";
                String className = compElt.SelectSingleNode(xpathStr).Value;
                String asmName = compElt.Attributes["assemblyname"].Value;
                asmName = LSIDClient.Global.BinDirectory + "\\" + asmName;
                                 
                LSIDService service = (LSIDService)System.Reflection.Assembly.LoadFrom(asmName).CreateInstance(className);
                checkComponent(compElt.LocalName,service);
                service.initService(config);
                return service;
            }
            catch (Exception e) 
            {
                throw new LSIDConfigurationException(e,"Error loading service");
            }
        }

        public ArrayList knownServices()
        {
            ArrayList l = new ArrayList();
            l.Add("meta");
            l.Add("auth");
            l.Add("data");
            l.Add("assn");
            l.Add("authentication");	
            return l;
        }
    }
}
