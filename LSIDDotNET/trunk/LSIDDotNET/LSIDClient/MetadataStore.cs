using System;
using System.Xml.XPath;

namespace LSIDClient
{
	//todo implement metadata stores
	//add to lsid-client config file:
	//
	//<!-- the meta data store is used to collect meta data as it is downloaded. Formats included here must be
	//listed in the acceptedMetadataFormats tag above.
	//-->
	//<metadataStores>
	//	<metadataStore format="application/xml+rdf" metadata-factory="LSIDFramework.XSLTMetadataFactory" assemblyname="LSIDFramework.dll">
	//		<properties>
	//			<property name="one-store">
	//				<value>true</value>
	//			</property>
	//			<property name="other">
	//				<value>my value</value>
	//			</property>
	//		</properties>
	//	</metadataStore>
	//</metadataStores>

//    [Serializable()]
//    public class MetadataStore 
//    {
//
//
//        //--------------------------/
//        //- Class/Member Variables -/
//        //--------------------------/
//
//        /**
//        * Field metadataFactory
//        */
//        public String metadataFactory;
//
//		public String metadataFactoryAssembly;
//
//        /**
//         * Field format
//         */
//        public  String format;
//
//        /**
//         * Field properties
//         */
//        public Properties properties;
//
//
//        //----------------/
//        //- Constructors -/
//        //----------------/
//
//        public MetadataStore() 
//        {
//        }
//
//
//        //-----------/
//        //- Methods -/
//        //-----------/
//
//        /**
//         * Note: hashCode() has not been overriden
//         * 
//         * @param obj
//         */
//        public Boolean equals(Object obj)
//        {
//            if ( this == obj )
//                return true;
//        
//            if (obj is MetadataStore) 
//            {
//        
//                MetadataStore temp = (MetadataStore)obj;
//                if (this.metadataFactory != null) 
//                {
//                    if (temp.metadataFactory == null) return false;
//                    else if (!(this.metadataFactory.Equals(temp.metadataFactory))) 
//                        return false;
//                }
//                else if (temp.metadataFactory != null)
//                    return false;
//                if (this.format != null) 
//                {
//                    if (temp.format == null) return false;
//                    else if (!(this.format.Equals(temp.format))) 
//                        return false;
//                }
//                else if (temp.format != null)
//                    return false;
//                if (this.properties != null) 
//                {
//                    if (temp.properties == null) return false;
//                    else if (!(this.properties.equals(temp.properties))) 
//                        return false;
//                }
//                else if (temp.properties != null)
//                    return false;
//                return true;
//            }
//            return false;
//        }
//
//        /**
//         * Returns the value of field 'format'.
//         * 
//         * @return the value of field 'format'.
//         */
//        public String getFormat()
//        {
//            return this.format;
//        }
//
//        /**
//         * Returns the value of field 'metadataFactory'.
//         * 
//         * @return the value of field 'metadataFactory'.
//         */
//        public String getMetadataFactory()
//        {
//            return this.metadataFactory;
//        }
//
//        /**
//         * Returns the value of field 'properties'.
//         * 
//         * @return the value of field 'properties'.
//         */
//        public Properties getProperties()
//        {
//            return this.properties;
//        }
//
//
//
//		public void XmlDeserialise(XPathNavigator nav)
//		{
//			metadataFactory = null;
//			metadataFactoryAssembly = null;
//			format = null;
//			properties = new Properties();
//			
//			bool next = nav.MoveToFirstAttribute();
//			while (next)
//			{
//				if (nav.Name == "metadata-factory")
//				{
//					metadataFactory = nav.Value;
//				}
//				else if (nav.Name == "format")
//				{
//					format = nav.Value;
//				}
//				else if (nav.Name == "assemblyname")
//				{
//					metadataFactoryAssembly = nav.Value;
//				}
//				next = nav.MoveToNextAttribute();
//			}
//			
//			nav.MoveToParent();
//			XPathNodeIterator itr = nav.SelectChildren("properties", nav.NamespaceURI);
//			if (itr.MoveNext())
//			{
//				nav.MoveToFirstChild();
//				itr = nav.SelectChildren("property", nav.NamespaceURI);
//								
//				while (itr.MoveNext())
//				{
//					if (properties.getPropertyCount() == 0) nav.MoveToFirstChild();
//					else nav.MoveToNext();
//
//					XPathNavigator pNav = nav.Clone();
//					if (pNav.MoveToFirstAttribute()) 
//					{
//						Property p = new Property();
//						p.setName(pNav.Value);
//						pNav.MoveToParent();
//						pNav.MoveToFirstChild();
//						p.setValue(pNav.Value);
//						
//						properties.addProperty(p);
//					}
//				}
//
//				nav.MoveToParent();
//			}
//		}
//
//        /**
//         * Sets the value of field 'format'.
//         * 
//         * @param format the value of field 'format'.
//         */
//        public void setFormat(String format)
//        {
//            this.format = format;
//        }
//        
//        /**
//         * Sets the value of field 'metadataFactory'.
//         * 
//         * @param metadataFactory the value of field 'metadataFactory'.
//         */
//        public void setMetadataFactory(String metadataFactory)
//        {
//            this.metadataFactory = metadataFactory;
//        }
//
//        /**
//         * Sets the value of field 'properties'.
//         * 
//         * @param properties the value of field 'properties'.
//         */
//        public void setProperties(Properties properties)
//        {
//            this.properties = properties;
//        }
//
//
//    }

}
