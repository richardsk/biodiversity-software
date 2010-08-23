using System;
using System.Collections;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

using LSIDClient;

namespace LSIDFramework
{
    //[XmlSchemaProvider("DictSchema")]
//    [XmlRoot("dictionary", Namespace =
//         "http://thinktecture.com/demos/dictionary", IsNullable = true)]
    public class SerializableDictionary : IXmlSerializable
    {
        string NS = WSDLConstants.OMG_LSID_PORT_TYPES_WSDL_NS_URI;  //"http://landcareresearch.co.nz/xml/serialization";
        private IDictionary dictionary;

        public SerializableDictionary()
        {
            dictionary = new Hashtable();
        }
        public SerializableDictionary(IDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        void IXmlSerializable.WriteXml(XmlWriter w)
        {
            w.WriteStartElement("dictionary", NS);
            foreach(object key in dictionary.Keys)
            {
                object value = dictionary[key];
                w.WriteStartElement("item", NS);
                w.WriteElementString("key", NS, key.ToString());
                w.WriteElementString("value", NS, value.ToString());
                w.WriteEndElement();
            }
            w.WriteEndElement();
        }

        void IXmlSerializable.ReadXml(XmlReader r)
        {
            r.Read();
            r.ReadStartElement("dictionary");
            while(r.NodeType != XmlNodeType.EndElement)
            {
                r.ReadStartElement("item", NS);
                string key = r.ReadElementString("key", NS);
                string value = r.ReadElementString("value", NS);
                r.ReadEndElement();
                r.MoveToContent();
                dictionary.Add(key, value);
            }
        }

        XmlSchema IXmlSerializable.GetSchema() { return null; }

//        public static XmlQualifiedName DictSchema(XmlSchemaSet xss)
//        {
//            XmlSchema xs = XmlSchema.Read(new StringReader(
//                "<xs:schema id='DictionarySchema' targetNamespace='" + NS +
//                "' elementFormDefault='qualified' xmlns='" + NS + "' xmlns:mstns='" +
//                NS + "' xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType " +
//                "name='DictionaryType'><xs:sequence><xs:element name='item' type='ItemType' " +
//                "maxOccurs='unbounded' /></xs:sequence></xs:complexType><xs:complexType " +
//                "name='ItemType'><xs:sequence><xs:element name='key' type='xs:string' />" +
//                "<xs:element name='value' type='xs:string' /></xs:sequence></xs:complexType>" +
//                "<xs:element name='dictionary' type='mstns:DictionaryType'></xs:element></xs:schema>"),
//                null);
//            xss.XmlResolver = new XmlUrlResolver();
//            xss.Add(xs);
//            return new XmlQualifiedName("DictionaryType", NS);
//        }
    }

}
