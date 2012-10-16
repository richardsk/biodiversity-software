using System;
using System.Xml;

namespace LSIDClient
{
	
	public interface IFTPLocation 
	{		
		/**
		 * Get the FTP server name
		 * @return String the server name
		 */
		String getServer();
	
		/**
		 * Set the FTP server name
		 * @param String the server name
		 */
		void setServer(String server);
	
		/**
		 * Get the FTP file path
		 * @return String the file path
		 */
		String getFilePath();
	
		/**
		 * Set the FTP file pith
		 * @param String the filepath
		 */
		void setFilePath(String filePath);

	}
}
