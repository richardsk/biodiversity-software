using System;
using System.Collections;

namespace LSIDClient
{
	/**
	 * Implements the LSIDStandardPort interfaces for FTP endpoints
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 **/

	public class FTPLocation : LSIDDataPort, LSIDMetadataPort 
	{
		private String hostname = null;
		private String dataPath = null;
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
			* Create an FTP endpoint with default portName and serviceName
			* 
			* @param String the host name of the service providing getData
			* @param String the file path of the service providing getData
			*/
		public FTPLocation(String hostname, String dataPath) 
		{
			this.hostname = hostname;
			this.dataPath = dataPath;
		}

		/**
			* Create an FTP endpoint with default serviceName
			* 
			* @param String the name of the WSDL port
			* @param String the host name of the service providing getData
			* @param String the file path of the service providing getData
			*/
		public FTPLocation(String portName, String hostname, String dataPath) 
		{
			this.portName = portName;
			this.hostname = hostname;
			this.dataPath = dataPath;
		}

		/**
			* Create an FTP endpoint
			* 
			* @param String the name of the WSDL service
			* @param String the name of the WSDL port
			* @param String the host name of the service
			* @param String the file path of the service
			*/
		public FTPLocation(String serviceName, String portName, String hostname, String dataPath) 
		{
			this.serviceName = serviceName;
			this.portName = portName;
			this.hostname = hostname;
			this.dataPath = dataPath;
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
			return hostname;
		}

		public String getPath() 
		{
			return dataPath;
		}

		public String getProtocol() 
		{
			return "ftp";
		}

	}

}
