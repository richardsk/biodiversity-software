using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace LSIDClient
{
    [Serializable()]
    public class LSIDClient 
    {
        //--------------------------/
        //- Class/Member Variables -/
        //--------------------------/

        /**
            * Field caching
            */
        public Caching caching;

        /**
         * Field hostMappings
         */
        public HostMappings hostMappings;

        /**
         * Field foreignAuthorities
         */
        private ForeignAuthorities foreignAuthorities;

        /**
         * Field metadataStores
         */
		//todo implement
        //private MetadataStores metadataStores;

        /**
         * Field wsdlExtensions
         */
        private WsdlExtensions wsdlExtensions;

        /**
         * Field acceptedFormats
         */
        private AcceptedFormats acceptedFormats;

        private WebSettings webSettings;

        //----------------/
        //- Constructors -/
        //----------------/

        public LSIDClient() 
        {
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //-----------/
        //- Methods -/
        //-----------/

        /**
         * 
         * @param obj
         */
        public override Boolean Equals(Object obj)
        {
            if ( this == obj )
                return true;
        
            if (obj is LSIDClient) 
            {

                LSIDClient temp = (LSIDClient)obj;
                if (this.caching != null) 
                {
                    if (temp.caching == null) return false;
                    else if (!(this.caching.Equals(temp.caching))) 
                        return false;
                }
                else if (temp.caching != null)
                    return false;
                if (this.hostMappings != null) 
                {
                    if (temp.hostMappings == null) return false;
                    else if (!(this.hostMappings.Equals(temp.hostMappings))) 
                        return false;
                }
                else if (temp.hostMappings != null)
                    return false;
                if (this.foreignAuthorities != null) 
                {
                    if (temp.foreignAuthorities == null) return false;
                    else if (!(this.foreignAuthorities.Equals(temp.foreignAuthorities))) 
                        return false;
                }
                else if (temp.foreignAuthorities != null)
                    return false;
				//todo implemenet
//                if (this.metadataStores != null) 
//                {
//                    if (temp.metadataStores == null) return false;
//                    else if (!(this.metadataStores.Equals(temp.metadataStores))) 
//                        return false;
//                }
//                else if (temp.metadataStores != null)
//                    return false;
                if (this.wsdlExtensions != null) 
                {
                    if (temp.wsdlExtensions == null) return false;
                    else if (!(this.wsdlExtensions.Equals(temp.wsdlExtensions))) 
                        return false;
                }
                else if (temp.wsdlExtensions != null)
                    return false;
                if (this.acceptedFormats != null) 
                {
                    if (temp.acceptedFormats == null) return false;
                    else if (!(this.acceptedFormats.Equals(temp.acceptedFormats))) 
                        return false;
                }
                else if (temp.acceptedFormats != null)
                    return false;
                return true;
            }
            return false;
        } 

        /**
         * Returns the value of field 'acceptedFormats'.
         * 
         * @return the value of field 'acceptedFormats'.
         */
        public AcceptedFormats getAcceptedFormats()
        {
            return this.acceptedFormats;
        } 

        /**
         * Returns the value of field 'caching'.
         * 
         * @return the value of field 'caching'.
         */
        public Caching getCaching()
        {
            return this.caching;
        } 

        /**
         * Returns the value of field 'foreignAuthorities'.
         * 
         * @return the value of field 'foreignAuthorities'.
         */
        public ForeignAuthorities getForeignAuthorities()
        {
            return this.foreignAuthorities;
        } 

        /**
         * Returns the value of field 'hostMappings'.
         * 
         * @return the value of field 'hostMappings'.
         */
        public HostMappings getHostMappings()
        {
            return this.hostMappings;
        } 

        /**
         * Returns the value of field 'metadataStores'.
         * 
         * @return the value of field 'metadataStores'.
         */
		//todo implement
//        public MetadataStores getMetadataStores()
//        {
//            return this.metadataStores;
//        } 

        /**
         * Returns the value of field 'wsdlExtensions'.
         * 
         * @return the value of field 'wsdlExtensions'.
         */
        public WsdlExtensions getWsdlExtensions()
        {
            return this.wsdlExtensions;
        } 

        public WebSettings getWebSettings()
        {
            return this.webSettings;
        }



        /**
         * Sets the value of field 'acceptedFormats'.
         * 
         * @param acceptedFormats the value of field 'acceptedFormats'.
         */
        public void setAcceptedFormats(AcceptedFormats acceptedFormats)
        {
            this.acceptedFormats = acceptedFormats;
        }

        /**
         * Sets the value of field 'caching'.
         * 
         * @param caching the value of field 'caching'.
         */
        public void setCaching(Caching caching)
        {
            this.caching = caching;
        }

        /**
         * Sets the value of field 'foreignAuthorities'.
         * 
         * @param foreignAuthorities the value of field
         * 'foreignAuthorities'.
         */
        public void setForeignAuthorities(ForeignAuthorities foreignAuthorities)
        {
            this.foreignAuthorities = foreignAuthorities;
        }

        /**
         * Sets the value of field 'hostMappings'.
         * 
         * @param hostMappings the value of field 'hostMappings'.
         */
        public void setHostMappings(HostMappings hostMappings)
        {
            this.hostMappings = hostMappings;
        }

        /**
         * Sets the value of field 'metadataStores'.
         * 
         * @param metadataStores the value of field 'metadataStores'.
         */
		//todo implement
//        public void setMetadataStores(MetadataStores metadataStores)
//        {
//            this.metadataStores = metadataStores;
//        }

        /**
         * Sets the value of field 'wsdlExtensions'.
         * 
         * @param wsdlExtensions the value of field 'wsdlExtensions'.
         */
        public void setWsdlExtensions(WsdlExtensions wsdlExtensions)
        {
            this.wsdlExtensions = wsdlExtensions;
        }

        public void setWebSettings(WebSettings settings)
        {
            this.webSettings = settings;
        }

        public void ReadConfig(string configFileName)
        {
            StreamReader rdr = null;
            
            try
            {
                rdr = new StreamReader(configFileName);
                XmlDocument doc = new XmlDocument();
                doc.Load(rdr);

                XPathNavigator nav = doc.CreateNavigator();
                
                caching = null;
                XPathNodeIterator itr = nav.SelectDescendants("caching", WSDLConstants.CLIENT_CONFIG_NS_URI, false);
                if (itr.MoveNext())
                {
                    XPathNavigator cNav = itr.Current;
                    if (cNav != null)
                    {
                        caching = new Caching();
                        caching.XmlDeserialise(cNav);
                    }
                }

                itr = nav.SelectDescendants("hostMappings", WSDLConstants.CLIENT_CONFIG_NS_URI, false);
                if (itr.MoveNext()) 
                {
                    XPathNavigator cNav = itr.Current;
                    if (cNav != null)
                    {
                        hostMappings = new HostMappings();
                        hostMappings.XmlDeserialise(cNav);
                    }
                }

				//todo implement
//				itr = nav.SelectDescendants("metadataStores", WSDLConstants.CLIENT_CONFIG_NS_URI, false);
//				if (itr.MoveNext()) 
//				{
//					XPathNavigator cNav = itr.Current;
//					if (cNav != null)
//					{
//						metadataStores = new MetadataStores();
//						metadataStores.XmlDeserialise(cNav);
//					}
//				}
                
                itr = nav.SelectDescendants("foreignAuthorities", WSDLConstants.CLIENT_CONFIG_NS_URI, false);
                if (itr.MoveNext()) 
                {
                    XPathNavigator cNav = itr.Current;
                    if (cNav != null)
                    {
                        foreignAuthorities = new ForeignAuthorities();
                        foreignAuthorities.XmlDeserialise(cNav);
                    }
                }

                itr = nav.SelectDescendants("acceptedFormats", WSDLConstants.CLIENT_CONFIG_NS_URI, false);
                if (itr.MoveNext()) 
                {
                    XPathNavigator cNav = itr.Current;
                    if (cNav != null)
                    {
                        acceptedFormats = new AcceptedFormats();
                        acceptedFormats.XmlDeserialise(cNav);
                    }
                }

                itr = nav.SelectDescendants("webSettings", WSDLConstants.CLIENT_CONFIG_NS_URI, false);
                if (itr.MoveNext()) 
                {
                    XPathNavigator cNav = itr.Current;
                    if (cNav != null)
                    {
                        webSettings = new WebSettings();
                        webSettings.XmlDeserialise(cNav);
                    }
                }
                                
            }
            finally
            {                
            }
        }
    }
}
