using System;
using System.Web.Services.Description;
using System.Web.Services.Configuration;
using System.Xml.Serialization;
using System.Xml;

namespace LSIDClient
{
	/**
	* Implementation of .NET SOAP Extension for the FTP location of a port
	* 
	* @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	*/
	

	[XmlFormatExtensionPrefix("ftp", "http://www.ibm.com/wsdl/ftp/")]	
	[XmlFormatExtension("location", "http://www.ibm.com/wsdl/ftp/", typeof(Port))]
	public class FTPLocationImpl : ServiceDescriptionFormatExtension, IFTPLocation//, IXmlSerializable
	{			
		[XmlAttribute("server")]
		public String server;
		
		[XmlAttribute("filepath")]
		public String filePath;
		
		public FTPLocationImpl()
		{
		}

		/**
		 * Construct a new FTPLocation
		 * @param String the FTP server
		 * @param String the filePath
		 */
		public FTPLocationImpl(String server, String filePath) 
		{
			this.server = server;
			this.filePath = filePath;
		}	

		public String getServer()
		{	
			return server;			
		}

		public void setServer(String s)
		{
			server = s;
		}

		public String getFilePath()
		{
			return filePath;		
		}

		public void setFilePath(String f)
		{
			filePath = f;
		}

	
		#region IXmlSerializable Members

//		public void WriteXml(XmlWriter writer)
//		{
//			writer.WriteStartElement("ftp", "location", WSDLConstants.FTP_NS_URI);
//			writer.WriteAttributeString("Server", server);
//			writer.WriteAttributeString("FilePath", filePath);
//			writer.WriteEndElement();
//
//			//XmlSerializer s = new XmlSerializer(this.GetType());
//			//s.Serialize(writer, server);
//			//s.Serialize(writer, filePath);
//		}
//
//		public System.Xml.Schema.XmlSchema GetSchema()
//		{
//			return null;
//		}
//
//		public void ReadXml(XmlReader reader)
//		{
//			XmlSerializer s = new XmlSerializer(this.GetType());
//			server = s.Deserialize(reader).ToString();
//			filePath = s.Deserialize(reader).ToString();
//		}

		#endregion
	}
}
