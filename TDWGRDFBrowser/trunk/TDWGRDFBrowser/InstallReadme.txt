To install, run the stup file, RDFBrowserSetup.msi.

If you do not install the application in the folder c:\program files\landcare research\TDWGRDFBrowser, then the following will need to be done:

In the install directory, the file TDWGRDFBrowser.exe.config contains the configuration details in it.
- Change the Home dir to point to the directory where the application was installed
- Change the ConnectionString DataSource path to point to MS Access database in the directory where the application was installed 
- Change the LSID_CLIENT_HOME path to point to the lsid-client directory in the install directory

If you are behind a firewall and nee to set the proxy, then set the WebProxy config setting to your proxy
eg proxy.example.org:8080

The LSID_CLIENT_HOME directory contains the LSID config settings file.  This should not need to be edited, unless you need to alter the way LSIDs are resolved.  
For example if an LSID resolver does not have a DNS record but you know the authority url, then a HostMapping can be added to the lsid-client config file
to point directly to a resolver for that LSID format.

