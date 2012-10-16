using System;
using System.Xml;
using System.Collections;

namespace LSIDFramework
{
/**
 * 
 * Handler to load service component of type="caching".  The body of components
 * 
 * 
 */
    public class CachingComponentHandler : ServiceComponentHandler
    {
        /**
         * @see ServiceComponentHandler#loadComponent(Element, LSIDServiceConfig)
         */
        public LSIDService loadComponent(XmlElement compElt, LSIDServiceConfig config) 
        {
            try 
            {
                String asmName = compElt.Attributes["assemblyname"].Value;
                String className = compElt.SelectSingleNode("child::text()").Value;
                LSIDService service = null;
                try 
                {
                    service = (LSIDService)System.Reflection.Assembly.LoadFrom(asmName).CreateInstance(className);

                    LSIDServiceComponentHandler.checkComponent(compElt.LocalName, service);
                    service.initService(config);
                }
                catch (Exception e) 
                {
                    throw new LSIDServerException(e,LSIDServerException.INTERNAL_PROCESSING_ERROR,"Could not load class: "+className);
                }
//                catch (IllegalAccessException e) 
//                {
//                    throw new LSIDServerException(e,LSIDServerException.INTERNAL_PROCESSING_ERROR,"Could not load class: "+className);
//                }
//                catch (ClassNotFoundException e) 
//                {
//                    throw new LSIDServerException(e,LSIDServerException.INTERNAL_PROCESSING_ERROR,"Could not find class: "+className);					
//                }
    		
                LSIDService cacheService = null;
                String type = compElt.Name;
                if (type.Equals(ServiceConfigurationConstants.AUTH_COMP)) 
                {    			
                    cacheService = new CachingAuthorityService((LSIDAuthorityService)service);
                } 
                else if (type.Equals(ServiceConfigurationConstants.META_COMP)) 
                {
                    cacheService = new CachingMetadataService((LSIDMetadataService)service);    			
                } 
                else if (type.Equals(ServiceConfigurationConstants.DATA_COMP)) 
                {
                    cacheService = new CachingDataService((LSIDDataService)service);    			
                } 
                cacheService.initService(config);
                return cacheService;
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
            return l;
        }
    }
}
