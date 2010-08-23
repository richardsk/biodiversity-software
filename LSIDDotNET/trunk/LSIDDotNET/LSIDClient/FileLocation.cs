using System;
using System.Collections;

namespace LSIDClient
{

	/**
	 * Implements the LSIDStandardPort interfaces for files
	 * 
	 * * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
	public class FileLocation : LSIDDataPort,LSIDMetadataPort 
	{
		private String filename = null;
		private String serviceName = null;
		private String portName = null;
		private LSIDCredentials lsidCredentials = null;	
		private Hashtable headers = new Hashtable();
		/**
		 * Method addProtocolHeader.
		 * @param name
		 * @param value
		 */
		public void addProtocolHeader(String name, String value)
		{
			headers.Add(name,value);
		}
	
		/**
		 * Method getProtocolHeaders.
		 * @return Map
		 */
		public Hashtable getProtocolHeaders() 
		{
			return headers;
		}	
		/**
		 * Returns the lsidCredentials.
		 * @return LSIDCredentials
		 */
		public LSIDCredentials getLsidCredentials() 
		{
			if (lsidCredentials == null) 
			{
				LSIDCredentials portCreds = new LSIDCredentials(this);
				if (portCreds.keys().hasMoreElements())
					lsidCredentials = portCreds;
			}
			return lsidCredentials;
		}

		/**
		 * Sets the lsidCredentials.
		 * @param lsidCredentials The lsidCredentials to set
		 */
		public void setLsidCredentials(LSIDCredentials lsidCredentials) 
		{
			this.lsidCredentials = lsidCredentials;
		}

		/**
		 * Create a File endpoint with default portName and serviceName
		 * 
		 * @param String the filename
		 * @param String the file path of the service providing getData
		 */
		public FileLocation(String filename) 
		{
			this.filename = filename;
		}

		/**
		 * Create a File endpoint with default serviceName
		 * 
		 * @param String the name of the WSDL port
		 * @param String the filename
		 */
		public FileLocation(String portName, String filename) 
		{
			this.portName = portName;
			this.filename = filename;
		}

		/**
		 * Create a File endpoint
		 * 
		 * @param String the name of the WSDL service
		 * @param String the name of the WSDL port
		 * @param String the filename
		 */
		public FileLocation(String serviceName, String portName, String filename) 
		{
			this.serviceName = serviceName;
			this.portName = portName;
			this.filename = filename;
		}

		public String getServiceName() 
		{
			return serviceName;
		}

		public String getName() 
		{
			return portName;
		}

		public String getLocation() 
		{
			return filename;
		}

		public String getPath() 
		{
			return filename;
		}

		public String getProtocol() 
		{
			return "file";
		}

	}
}