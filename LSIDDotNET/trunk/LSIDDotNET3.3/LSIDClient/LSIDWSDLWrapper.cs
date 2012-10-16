using System;
using System.Collections;
using System.Web.Services.Description;
using System.Xml;
using System.IO;

namespace LSIDClient
{
	/**
	 *
	 * This class provides a simple API into the WSDL that an LSID resolves.  Most of its functionality is 
	 * available internally only to LSIDResolver. This class is meant to be used publicly as a way to generate
	 * a WSDL document.  Also, a few public methods exists for accessing the WSDL document in different formats. 
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class LSIDWSDLWrapper : WSDLConstants 
	{

		private static readonly Hashtable IMPORT_MAPS = new Hashtable();

		// WSDL representations
		private String wsdlStr = null;
		private ServiceDescriptionCollection wsdlDef; //array of service descriptions, first is the main wsdl, rest are imported wsdl

		// the metadata ports keyed by servicename:portname
		private Hashtable lsidMetadataPorts = new Hashtable();

		// the data ports keyed by servicename:portname
		private Hashtable lsidDataPorts = new Hashtable();

		// the authority ports keyed by servicename:portname
		private Hashtable lsidAuthorityPorts = new Hashtable();

		// the extension ports keyed by servicename:portname
		private Hashtable wsdlExtensionPorts = new Hashtable();

		// the expiration date/time of the WSDL
		private DateTime expiration = DateTime.MinValue;

		// maintain a counter of the number of ports and bindings that are added without give names so that we
		// can provide unique name for them.
		private int currPortNum = 1;

		private Hashtable ftpPorts = new Hashtable();

		public LSIDWSDLWrapper()
		{
			IMPORT_MAPS.Add(STANDARD_PORT_TYPES_LOCATION, STANDARD_PORT_TYPES_FILE);
			IMPORT_MAPS.Add(DATA_SOAP_BINDINGS_LOCATION, DATA_SOAP_BINDINGS_FILE);
			IMPORT_MAPS.Add(DATA_HTTP_BINDINGS_LOCATION, DATA_HTTP_BINDINGS_FILE);
			IMPORT_MAPS.Add(DATA_FTP_BINDINGS_LOCATION, DATA_FTP_BINDINGS_FILE);
			IMPORT_MAPS.Add(DATA_FILE_BINDINGS_LOCATION, DATA_FILE_BINDINGS_FILE);
			IMPORT_MAPS.Add(AUTHORITY_SOAP_BINDINGS_LOCATION, AUTHORITY_SOAP_BINDINGS_FILE);
			IMPORT_MAPS.Add(AUTHORITY_HTTP_BINDINGS_LOCATION, AUTHORITY_HTTP_BINDINGS_FILE);
		}

		/**
		 * Construct a new WSDL from scratch. This builds a generic LSID WSDL with not ports or bindings.  These will be
		 * created with <code>setDataLocation</code> and <code>setMetaDataLocation</code>.
		 * @param the LSID that this WSDL describes the methods for
		 */
		public LSIDWSDLWrapper(LSID lsid) 
		{
			wsdlDef = new ServiceDescriptionCollection();

			ServiceDescription wd = new ServiceDescription();
			wd.TargetNamespace = getTargetNamespace(lsid);
			wsdlDef.Add(wd); 
            
			//            wsdlDef = new DefinitionImpl();
			//            wsdlDef.setExtensionRegistry(new PopulatedExtensionRegistry());
			//            String tns = getTargetNamespace(lsid);
			//            wsdlDef.setTargetNamespace(tns);
			//            wsdlDef.addNamespace(TNS, tns);
		}

		/**
		 * Construct a wrapper from the String representation of the WSDL.
		 * @param String the wsdl in string form
		 */
		public LSIDWSDLWrapper(String wsdl) 
		{
			wsdlStr = wsdl;
			try 
			{
				wsdlDef = new ServiceDescriptionCollection();
				System.IO.StringReader sr = new System.IO.StringReader(wsdl);
				ServiceDescription wd = ServiceDescription.Read(sr);
				
				wsdlDef.Add(wd);

			} 
			catch (Exception e) 
			{
				throw new LSIDException(e, "Error reading wsdl file");
			}
			extractPorts();
		}

		/**
		 * Construct a wrapper from the String representation of the WSDL.
		 * @param InputStream the wsdl in stream form
		 */
		public LSIDWSDLWrapper(Stream wsdl) 
		{
			try 
			{
				StreamReader rdr = new StreamReader(wsdl, System.Text.Encoding.UTF8);
				string wsdlStr = rdr.ReadToEnd();

				wsdlDef = new ServiceDescriptionCollection();
				StringReader sr = new StringReader(wsdlStr);
								
				ServiceDescription wd = ServiceDescription.Read(sr);								
				
				wsdlDef.Add(wd);
			} 
			catch (Exception) 
			{
				throw new LSIDException("Error reading wsdl file");
			}
			extractPorts();
		}

		
		/**
		 * Get the WSDL String
		 * @return String get the String representation of the WSDL
		 */
		public String getWSDL() 
		{
			try 
			{
				if (wsdlStr == null) 
				{
					updateStringRepresentation();
				}
			} 
			catch (LSIDException e) 
			{
				e.PrintStackTrace();
				return null;
			}
			return wsdlStr;
		}

		/**
		 * Get the WSDL String
		 * @return String the String representation of the WSDL
		 */
		public override String ToString() 
		{
			return getWSDL();
		}

		/**
		 * Get the Defintion built by wsdl4j. 
		 * @return ServiceDescriptionCollection containing the WSDL Definition 
		 */
		public ServiceDescriptionCollection getDefinition() 
		{
			// the caller will most likely update the def, so we have invalidate the string rep
			wsdlStr = null;
			return wsdlDef;
		}

		/**
		 * Returns the expiration.
		 * @return Date null value 
		 */
		public DateTime getExpiration() 
		{
			return expiration;
		}

		/**
		 * Sets the expiration.
		 * @param expiration The expiration to set
		 */
		public void setExpiration(DateTime expiration) 
		{
			this.expiration = expiration;
		}

		/**
		 * Set the location at which data may be retrieved
		 * @param LSIDDataPort the the location of the data to set
		 */
		public void setDataLocation(LSIDDataPort dataPort) 
		{
			String serviceName = dataPort.getServiceName();
			if (serviceName == null)
				serviceName = SERVICE_NAME;
			Service service = getService(serviceName);
			if (service == null) 
			{
				//ServiceDescription sd = new ServiceDescription();
				service = new Service();
				service.Name = serviceName;
				wsdlDef[0].Services.Add(service);
				//sd.Services.Add(service);
				//wsdlDef.Add(sd);
			}

			// remove data port if it already exists.
			String portName = dataPort.getName();
			if (portName != null) 
			{
				System.Web.Services.Description.Port port = service.Ports[portName];
				if (port != null) 
				{
					Binding b = getBinding(port.Binding);
					if (b != null) 
					{
						service.Ports.Remove(port);
						b.ServiceDescription.Bindings.Remove(b);
					}
					//wsdlDef.Remove(.Bindings.Remove(wsdlDef.Bindings[port.Binding.Name]);
					//service.Ports.Remove(port);
				}
				lsidDataPorts.Remove(getPortKey(dataPort));
			}

			String protocol = dataPort.getProtocol();

			// we have to create a new port and possibly a binding
			// make sure we have the namespaces set in the defintion
			configureDataServiceDef(dataPort.getProtocol());

			//PortType dataPortType = wsdlDef.GetPortType(new XmlQualifiedName(DATA_PORT_TYPE, OMG_LSID_PORT_TYPES_WSDL_NS_URI));

			Binding binding = null;
			if (protocol.Equals(SOAP))
				binding = getBinding(DATA_SOAP_BINDING);
			else if (protocol.Equals(HTTP)) 
			{
				//System.err.println(dataPort.GetType().ToString());
				//System.err.println("path: " + dataPort.getPath());
				if (dataPort.getPath().Equals(LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED))
					binding = getBinding(DATA_HTTP_BINDING);
				else
					binding = getBinding(DATA_HTTP_BINDING_DIRECT);
			} 
			else if (protocol.Equals(FTP))
				binding = getBinding(DATA_FTP_BINDING);
			else if (protocol.Equals(FILE))
				binding = getBinding(DATA_FILE_BINDING);
			Port p = createPort(service, binding, dataPort);
			service.Ports.Add(p);
			lsidDataPorts[getPortKey(dataPort)] =  dataPort;

			// indicate that the WSDL has changed, so the string rep is no longer valid...
			wsdlStr = null;

		}

		/**
		 * Set the location at which metadata may be retrieved and queried
		 * @param LSIDMetadataPort the the location of the metadata to set
		 */
		public void setMetadataLocation(LSIDMetadataPort metadataPort) 
		{
			String serviceName = metadataPort.getServiceName();
			if (serviceName == null)
				serviceName = SERVICE_NAME;
			Service service = getService(serviceName);
			if (service == null) 
			{
				service = new Service();
				//ServiceDescription sd = new ServiceDescription();
				service.Name = serviceName;
				wsdlDef[0].Services.Add(service);
				//sd.Services.Add(service);
				//wsdlDef.Add(sd);
			}

			// remove data port if it already exists.
			String portName = metadataPort.getName();
			if (portName != null) 
			{
				Port port = service.Ports[portName];
				if (port != null) 
				{
					Binding b = getBinding(port.Binding);
					if (b != null) 
					{
						service.Ports.Remove(port);
						b.ServiceDescription.Bindings.Remove(b);
					}
					//                    wsdlDef.Bindings.Remove(wsdlDef.Bindings[port.Binding.Name]);
					//                    service.Ports.Remove(port);
				}
				lsidMetadataPorts.Remove(getPortKey(metadataPort));
			}

			String protocol = metadataPort.getProtocol();

			// we have to create a new port and possibly a binding
			// make sure we have the namespaces set in the defintion
			configureDataServiceDef(metadataPort.getProtocol());

			//PortType metadataPortType = wsdlDef.GetPortType(new XmlQualifiedName(METADATA_PORT_TYPE, OMG_LSID_PORT_TYPES_WSDL_NS_URI));

			Binding binding = null;
			if (protocol.Equals(SOAP))
				binding = getBinding(METADATA_SOAP_BINDING);
			else if (protocol.Equals(HTTP)) 
			{
				if (metadataPort.getPath().Equals(LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED))
					binding = getBinding(METADATA_HTTP_BINDING);
				else
					binding = getBinding(METADATA_HTTP_BINDING_DIRECT);
			} 
			else if (protocol.Equals(FTP))
				binding = getBinding(METADATA_FTP_BINDING);
			else if (protocol.Equals(FILE))
				binding = getBinding(METADATA_FILE_BINDING);
			Port p = createPort(service, binding, metadataPort);
			service.Ports.Add(p);
			lsidMetadataPorts[getPortKey(metadataPort)] =  metadataPort;

			// indicate that the WSDL has changed, so the string rep is no longer valid...
			wsdlStr = null;

		}

		/**
		 * Set the authority location.  
		 * @param LSIDAuthorityPort the the location of the metadata to set
		 */
		public void setAuthorityLocation(LSIDAuthorityPort authorityPort) 
		{
			String serviceName = authorityPort.getServiceName();
			if (serviceName == null)
				serviceName = SERVICE_NAME;
			Service service = getService(serviceName);
			if (service == null) 
			{
				service = new Service();
				//ServiceDescription sd = new ServiceDescription();
				service.Name = serviceName;
				wsdlDef[0].Services.Add(service);
				//sd.Services.Add(service);
				//wsdlDef.Add(sd);
			}

			// remove data port if it already exists.
			String portName = authorityPort.getName();
			if (portName != null) 
			{
				Port port = service.Ports[portName];
				if (port != null) 
				{
					Binding b = getBinding(port.Binding);
					if (b != null) 
					{
						service.Ports.Remove(port);
						b.ServiceDescription.Bindings.Remove(b);
					}
					//                    wsdlDef.Bindings.Remove(wsdlDef.Bindings[port.Binding.Name]);
					//                    service.Ports.Remove(port);
				}
				lsidAuthorityPorts.Remove(getPortKey(authorityPort));
			}

			String protocol = authorityPort.getProtocol();

			// we have to create a new port and possibly a binding
			// make sure we have the namespaces set in the defintion
			configureAuthorityServiceDef(protocol);

			//PortType authorityPortType = wsdlDef.GetPortType(new XmlQualifiedName(AUTHORITY_PORT_TYPE, OMG_LSID_PORT_TYPES_WSDL_NS_URI));

			Binding binding = null;
			if (protocol.Equals(SOAP))
				binding = getBinding(AUTHORITY_SOAP_BINDING);
			else if (protocol.Equals(HTTP))
				binding = getBinding(AUTHORITY_HTTP_BINDING);
			if (binding == null)
				throw new LSIDException("Unsuported protocol for authority port: " + protocol);
			Port authport = createPort(service, binding, authorityPort);

			service.Ports.Add(authport);
			lsidAuthorityPorts[getPortKey(authorityPort)] = authorityPort;
			// indicate that the WSDL has changed, so the string rep is no longer valid...
			wsdlStr = null;

		}
	
		private Binding getBinding(XmlQualifiedName name)
		{
			Binding b = null;

			try
			{				
				foreach (Import i in wsdlDef[0].Imports)
				{
					if (i.Namespace == name.Namespace)
					{										
						System.IO.Stream s = WSDLLocator.GetWSDLFile(i.Location);
						ServiceDescription bsd = ServiceDescription.Read(s); 

						b = bsd.Bindings[name.Name];
						break;
					}
				}

				//b = wsdlDef.GetBinding(name);
			}
			catch(Exception ex) 
			{                            
				LSIDException.PrintStackTrace(ex);                    
			}
			
			return b;
		}

		private Service getService(string name)
		{
			Service sd = null;
			try
			{
				XmlQualifiedName xqn = new XmlQualifiedName(name, wsdlDef[0].TargetNamespace);
				sd = wsdlDef.GetService(xqn);
			}
			catch(Exception ex) 
			{
				LSIDException.PrintStackTrace(ex);                    
			}

			return sd;
		}

		/**
		 * Get the names of the services.  All metadata ports in a given service must be the same.
		 * the client may assume that all the authorative metadata may be obtained by requesting
		 * one port from each service.
		 * @return Enumeration the service names
		 */
		public Enumeration getServiceNames() 
		{
			IColEnumeration names = new IColEnumeration();
            
			foreach (ServiceDescription sd in wsdlDef)
			{
				foreach (Service s in sd.Services)
				{
					names.Add(s.Name);	
				}
			}
			return names;
		}

		/**
		 * Get the keys of all the metadata ports
		 * @return Enumeration an Enumeration of Strings of the form "serviceName:portName"
		 */
		public Enumeration getMetadataPortNames() 
		{
			return new IColEnumeration( lsidMetadataPorts.Keys );
		}
	
		/**
		 * Get the keys of all the metadata ports in the given service
		 * @param String the service
		 * @return LSIDMetadataPort the port
		 */
		public Enumeration getMetadataPortNamesForService(String servicename) 
		{
			Service service = getService(servicename);
            			
			IColEnumeration names = new IColEnumeration();

			foreach (Port p in service.Ports)
			{
				XmlQualifiedName portTypeName = GetPortTypeName(service.ServiceDescription, p.Binding);

				if (portTypeName.Name.IndexOf(METADATA_PORT_TYPE) != -1) 
					names.Add(servicename + ":" + p.Name);	
			}
			return names;
		}

		/**
		 * Get the keys of all the metadata ports for the given protocol
		 * @param String the protocol
		 * @return Enumeration an Enumeration of Strings of the form "serviceName:portName"
		 */
		public Enumeration getMetadataPortNamesForProtocol(String protocol) 
		{
			IColEnumeration names = new IColEnumeration();
			IColEnumeration portNames = new IColEnumeration(lsidMetadataPorts.Keys);
			while (portNames.hasMoreElements()) 
			{
				String portName = (String) portNames.nextElement();
				LSIDMetadataPort lmdp = (LSIDMetadataPort) lsidMetadataPorts[portName];
				String prot = lmdp.getProtocol();
				if (prot == null)
					continue;
				if (prot.Equals(protocol))
					names.Add(portName);
			}
			return names;
		}

		/**
		 * Get an arbitrary metadata port if one exists. 
		 * @return LSIDMetadataPort a metadata port if one exits, null otherwise. Uses protocol preference order: HTTP, FTP, ANY
		 */
		public LSIDMetadataPort getMetadataPort() 
		{
			LSIDMetadataPort port = getMetadataPortForProtocol(HTTP);
			if (port != null)
				return port;
			port = getMetadataPortForProtocol(FTP);
			if (port != null)
				return port;
			IColEnumeration col = new IColEnumeration(lsidMetadataPorts.Keys);
			if (!col.hasMoreElements())
				return null;
			return (LSIDMetadataPort) lsidMetadataPorts[col.nextElement()];
		}

		/**
		 * Get the metadata port with the given key
		 * @param String the key of the port, of the form "serviceName:portName"
		 * @return LSIDMetadataPort, the metadata port
		 */
		public LSIDMetadataPort getMetadataPort(String name) 
		{
			return (LSIDMetadataPort) lsidMetadataPorts[name];
		}

		/**
		 * Get an arbitray metadata port for the given protocol
		 * @param String the protocol
		 * @return LSIDMetadataPort, the metadata port if one exists, null otherwise.
		 */
		public LSIDMetadataPort getMetadataPortForProtocol(String protocol) 
		{
            LSIDMetadataPort mp = null;

			IColEnumeration portNames = new IColEnumeration(lsidMetadataPorts.Keys);
			while (portNames.hasMoreElements()) 
			{
				LSIDMetadataPort lmdp = (LSIDMetadataPort) lsidMetadataPorts[portNames.nextElement()];
				String prot = lmdp.getProtocol();
				if (prot == null)
					continue;
                if (prot.Equals(protocol))
                {
                    if (mp == null || mp.getName() != METADATA_HTTP_BINDING_DIRECT.Name) //direct is preferred
                        mp = lmdp;
                }
			}
			return mp;
		}

		/**
		 * Get the keys of all the metadata ports
		 * @return Enumeration an Enumeration of Strings of the form "serviceName:portName"
		 */
		public Enumeration getDataPortNames() 
		{
			return new IColEnumeration(lsidDataPorts.Keys);
		}

		/**
		 * Get the keys of all the ports for the given protocol
		 * @param String the protocol
		 * @return Enumeration an Enumeration of Strings of the form "serviceName:portName"
		 */
		public Enumeration getDataPortNamesForProtocol(String protocol) 
		{
			IColEnumeration names = new IColEnumeration();
			IColEnumeration portNames = new IColEnumeration(lsidDataPorts.Keys);
			while (portNames.hasMoreElements()) 
			{
				String portName = (String) portNames.nextElement();
				LSIDDataPort ldp = (LSIDDataPort) lsidDataPorts[portName];
				if (ldp.getProtocol().Equals(protocol))
					names.Add(portName);
			}
			return names;
		}

		/**
		 * Get an arbitrary data port if one exists. 
		 * @return LSIDDataPort a data port if one exits, null otherwise. Uses protocol preference order: HTTP, FTP, ANY
		 */
		public LSIDDataPort getDataPort() 
		{
			LSIDDataPort port = getDataPortForProtocol(HTTP);
			if (port != null)
				return port;
			port = getDataPortForProtocol(FTP);
			if (port != null)
				return port;
			IColEnumeration col = new IColEnumeration(lsidDataPorts.Keys);
			if (!col.hasMoreElements())
				return null;
			return (LSIDDataPort) lsidDataPorts[col.nextElement()];
		}

		/**
		 * Get the data port with the given key
		 * @param String the key of the port, of the form "serviceName:portName"
		 * @return LSIDDataPort, the data port
		 */
		public LSIDDataPort getDataPort(String name) 
		{
			return (LSIDDataPort) lsidDataPorts[name];
		}

		/**
		 * Get an arbitray data port for the given protocol
		 * @param String the protocol
		 * @return LSIDDataPort, the data port if one exists, null otherwise.
		 */
		public LSIDDataPort getDataPortForProtocol(String protocol) 
		{
			IColEnumeration portNames = new IColEnumeration( lsidDataPorts.Keys);
			while (portNames.hasMoreElements()) 
			{
				LSIDDataPort ldp = (LSIDDataPort) lsidDataPorts[portNames.nextElement()];
				if (ldp.getProtocol().Equals(protocol))
					return ldp;
			}
			return null;
		}

		/**
		 * Get the keys of all the authority ports
		 * @return Enumeration an Enumeration of Strings of the form "serviceName:portName"
		 */
		public Enumeration getAuthorityPortNames() 
		{
			return new IColEnumeration( lsidAuthorityPorts.Keys );
		}

		/**
		 * Get the keys of all the authority ports for the given protocol
		 * @param String the protocol
		 * @return Enumeration an Enumeration of Strings of the form "serviceName:portName"
		 */
		public Enumeration getAuthorityPortNamesForProtocol(String protocol) 
		{
			IColEnumeration names = new IColEnumeration();
			IColEnumeration portNames = new IColEnumeration( lsidAuthorityPorts.Keys );
			while (portNames.hasMoreElements()) 
			{
				String portName = (String) portNames.nextElement();
				LSIDAuthorityPort lap = (LSIDAuthorityPort) lsidAuthorityPorts[portName];
				String prot = lap.getProtocol();
				if (prot == null)
					continue;
				if (prot.Equals(protocol))
					names.Add(portName);
			}
			return names;
		}

		/**
		 * Get an arbitrary authority port if one exists. 
		 * @return LSIDAuthorityPort an authority port if one exits, null otherwise. Uses protocol preference order: HTTP, SOAP
		 */
		public LSIDAuthorityPort getAuthorityPort() 
		{
			LSIDAuthorityPort port = getAuthorityPortForProtocol(HTTP);
			if (port != null)
				return port;
			port = getAuthorityPortForProtocol(SOAP);
			if (port != null)
				return port;
			IColEnumeration col = new IColEnumeration(lsidAuthorityPorts.Keys);
			if (!col.hasMoreElements())
				return null;
			return (LSIDAuthorityPort) lsidAuthorityPorts[col.nextElement()];
		}

		/**
		 * Get the authority port with the given key
		 * @param String the key of the port, of the form "serviceName:portName"
		 * @return LSIDAuthorityPort, the authority port
		 */
		public LSIDAuthorityPort getAuthorityPort(String name) 
		{
			return (LSIDAuthorityPort) lsidAuthorityPorts[name];
		}

		/**
		 * Get an arbitray authority port for the given protocol
		 * @param String the protocol
		 * @return LSIDAuthorityPort, the authority port if one exists, null otherwise.
		 */
		public LSIDAuthorityPort getAuthorityPortForProtocol(String protocol) 
		{
			IColEnumeration portNames = new IColEnumeration(lsidAuthorityPorts.Keys);
			while (portNames.hasMoreElements()) 
			{
				LSIDAuthorityPort lap = (LSIDAuthorityPort) lsidAuthorityPorts[portNames.nextElement()];
				String prot = lap.getProtocol();
				if (prot == null)
					continue;
				if (prot.Equals(protocol))
					return lap;
			}
			return null;
		}

		/**
		 * Get all the names of extension port names
		 * @return Enumeration an enum of strings, each string can be used as a key for <code>getExtensionPort()</code>
		 */
		public Enumeration getExtensionPortNames() 
		{
			return new IColEnumeration( wsdlExtensionPorts.Keys );
		}

		/**
		 * Get all the names of extension ports for a given LSIDPort implementation
		 * @param Class a specific implementation of LSIDPort for which we would like all the ports
		 * @return Enumeration an enum of strings, each string can be used as a key for <code>getExtensionPort()</code>
		 */
		//        public Enumeration getExtensionPortNamesByClass(Class portClass) 
		//        {
		//            Enumeration keys = wsdlExtensionPorts.keys();
		//            Vector ret = new Vector();
		//            while (keys.hasMoreElements()) 
		//            {
		//                String key = (String) keys.nextElement();
		//                LSIDPort port = (LSIDPort) wsdlExtensionPorts.get(key);
		//                if (port.getClass().equals(portClass))
		//                    ret.add(key);
		//            }
		//            return ret.elements();
		//        }

		/**
		 * Get the given extension port
		 * @param String the key of the port, of the form "serviceName:portName"
		 * @return LSIDPort the port
		 */
		public LSIDPort getExtensionPort(String name) 
		{
			return (LSIDPort) wsdlExtensionPorts[name];
		}

		/**
		 * configure the wsdldef with necessary imports and namespaces for the given protocol
		 * for supporting data serices
		 */
		private void configureDataServiceDef(String protocol) 
		{
			if (protocol.Equals(HTTP)) 
			{
				if (!ContainsNamespace(wsdlDef, OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI)) 
				{
					importNamespace(DHB, OMG_DATA_HTTP_BINDINGS_WSDL_NS_URI, DATA_HTTP_BINDINGS_LOCATION);
				}

			} 
			else if (protocol.Equals(SOAP)) 
			{
				if (!ContainsNamespace(wsdlDef, OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI)) 
				{
					importNamespace(DSB, OMG_DATA_SOAP_BINDINGS_WSDL_NS_URI, DATA_SOAP_BINDINGS_LOCATION);
				}
				
			}
			else if (protocol.Equals(FTP)) 
			{ 
				// ftp
				if (!ContainsNamespace(wsdlDef, OMG_DATA_FTP_BINDINGS_WSDL_NS_URI)) 
				{
					importNamespace(DFB, OMG_DATA_FTP_BINDINGS_WSDL_NS_URI, DATA_FTP_BINDINGS_LOCATION);
				}

			} 
			else if (protocol.Equals(FILE)) 
			{ 
				// file
				if (!ContainsNamespace(wsdlDef, OMG_DATA_FILE_BINDINGS_WSDL_NS_URI)) 
				{
					importNamespace(DFB, OMG_DATA_FILE_BINDINGS_WSDL_NS_URI, DATA_FILE_BINDINGS_LOCATION);
				}				
			} 
		}

		/**
		 * configure the wsdldef with necessary imports and namespaces for the given protocol
		 * for supporting authority serices
		 */
		private void configureAuthorityServiceDef(String protocol) 
		{
			if (protocol.Equals(HTTP)) 
			{
				if (!ContainsNamespace(wsdlDef, OMG_AUTHORITY_HTTP_BINDINGS_WSDL_NS_URI)) 
				{
					importNamespace(AHB, OMG_AUTHORITY_HTTP_BINDINGS_WSDL_NS_URI, AUTHORITY_HTTP_BINDINGS_LOCATION);
				}

			} 
			else if (protocol.Equals(SOAP)) 
			{
				if (!ContainsNamespace(wsdlDef, OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI)) 
				{
					importNamespace(ASB, OMG_AUTHORITY_SOAP_BINDINGS_WSDL_NS_URI, AUTHORITY_SOAP_BINDINGS_LOCATION);
				}
			}
		}

		private bool ContainsNamespace(ServiceDescriptionCollection wsdlDef, String ns)
		{
			bool found = false;

			foreach (ServiceDescription sd in wsdlDef)
			{
				if (sd.TargetNamespace == ns) 
				{
					found = true;
					break;
				}
				else
				{
					foreach (Import imp in sd.Imports)
					{
						if (imp.Namespace == ns) 
						{
							found = true;
							break;
						}
					}
				}
			}

			return found;
		}

		/**
		 * import the given namespace into the def.
		 */
		private void importNamespace(String prefix, String ns, String location) 
		{
			ServiceDescription importDef = null;
			try 
			{
				if (location.IndexOf("http:") == -1)
				{
					//import file from resources
					System.IO.Stream s = WSDLLocator.GetWSDLFile(location);
					importDef = ServiceDescription.Read(s); 
				}
				else
				{
					//import from url
					HTTPResponse resp = HTTPUtils.doGet(location, null, null, null);
					importDef = ServiceDescription.Read(resp.getData());
				}
			} 
			catch (Exception e) 
			{
				throw new LSIDException(e, "Error importing namespace: " + ns);
			}
			Import imp = new Import();
			imp.Location = Path.GetFileName(location);
			imp.Namespace = ns;
			wsdlDef[0].Imports.Add(imp);

			wsdlDef.Add(importDef);            
		}

		/**
		 * Updates the string representation of the WSDL. This should be called if the WSDL Definition changes
		 */
		private void updateStringRepresentation() 
		{
			MemoryStream ms = new MemoryStream();
			XmlTextWriter xw = new XmlTextWriter(ms, System.Text.UTF8Encoding.UTF8);
			try 
			{
				wsdlDef[0].Write(xw);
			} 
			catch (Exception e) 
			{
				throw new LSIDException(e, "Error writing WSDL def to string");
			}

			StreamReader rdr = new StreamReader(ms);
			ms.Position = 0;
			wsdlStr = rdr.ReadToEnd();

			//TODO use soap extensions to do this
			//ftp ports?
			if (wsdlStr.IndexOf(WSDLConstants.DATA_FTP_BINDING.Name) != -1)
			{
				//add ftp ports
				XmlDocument d = new XmlDocument();
				d.LoadXml(wsdlStr);
				
				String xpathStr = "//wsdl:port";
				
				XmlNamespaceManager mgr = new XmlNamespaceManager(d.NameTable);
				mgr.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
				mgr.AddNamespace("fb", "http://www.omg.org/LSID/2003/DataServiceFTPBindings/");

				XmlNodeList nodes = d.SelectNodes(xpathStr, mgr);
				foreach (XmlNode n in nodes)
				{
					try
					{							
						String binding = n.SelectSingleNode("@binding", mgr).Value;
						if (binding.EndsWith(WSDLConstants.DATA_FTP_BINDING.Name) ||
							binding.EndsWith(WSDLConstants.METADATA_FTP_BINDING.Name) )
						{
							XmlElement loc = d.CreateElement("ftp", "location", WSDLConstants.FTP_NS_URI);
							FTPLocationImpl port = (FTPLocationImpl)ftpPorts[n.Attributes["name"].Value];
						
							XmlAttribute att = d.CreateAttribute("server");
							att.Value = port.getServer();
							loc.Attributes.Append(att);
						
							att = d.CreateAttribute("filepath");
							att.Value = port.getFilePath();
							loc.Attributes.Append(att);
						
							n.AppendChild(loc);
						}
					}
					catch(Exception ex)
					{
						LSIDException.PrintStackTrace(ex);
					}
				}

				wsdlStr = d.OuterXml;
			}
		}

		private XmlQualifiedName GetPortTypeName(ServiceDescription sd, XmlQualifiedName bindingName)
		{
			XmlQualifiedName typeName = null;

			foreach (Import i in sd.Imports)
			{
				if (i.Namespace == bindingName.Namespace)
				{										
					System.IO.Stream s = WSDLLocator.GetWSDLFile(i.Location);
					ServiceDescription bsd = ServiceDescription.Read(s); 

					typeName = bsd.Bindings[bindingName.Name].Type;
					break;
				}
			}

			return typeName;
		}

		/**
		 * Extract the port info for data and meta data
		 */
		private void extractPorts() 
		{
			foreach (ServiceDescription sd in wsdlDef)
			{
				foreach (Service s in sd.Services)
				{
					// go through the ports, meta data and data
					foreach(Port p in s.Ports)
					{
						XmlQualifiedName portTypeName = GetPortTypeName(sd, p.Binding);

						//if (XmlQualifiedName.getLocalPart().equals(METADATA_PORT_TYPE) && XmlQualifiedName.getNamespaceURI().equals(OMG_LSID_PORT_TYPES_WSDL_NS_URI)) 
						if (portTypeName.Name.IndexOf(METADATA_PORT_TYPE) != -1 && portTypeName.Namespace.Equals(OMG_LSID_PORT_TYPES_WSDL_NS_URI))
						{
							if (!p.Binding.Equals(METADATA_SOAP_BINDING) && !p.Binding.Equals(METADATA_HTTP_BINDING) && !p.Binding.Equals(METADATA_FTP_BINDING) 
                                && !p.Binding.Equals(METADATA_FILE_BINDING) && !p.Binding.Equals(METADATA_HTTP_BINDING_DIRECT))
								throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unrecognized metadata binding: " + p.Binding.Name);
							LSIDStandardPortImpl portImpl = extractPort(s, p, portTypeName);
							lsidMetadataPorts[portImpl.getKey()] = portImpl;
						} 
							//else if (XmlQualifiedName.getLocalPart().equals(DATA_PORT_TYPE) && XmlQualifiedName.getNamespaceURI().equals(OMG_LSID_PORT_TYPES_WSDL_NS_URI)) 
						else if (portTypeName.Name.IndexOf(DATA_PORT_TYPE) != -1 && portTypeName.Namespace.Equals(OMG_LSID_PORT_TYPES_WSDL_NS_URI))
						{
							if (!p.Binding.Equals(DATA_SOAP_BINDING) && !p.Binding.Equals(DATA_HTTP_BINDING) && !p.Binding.Equals(DATA_FTP_BINDING)
                                && !p.Binding.Equals(DATA_FILE_BINDING) && !p.Binding.Equals(DATA_HTTP_BINDING_DIRECT))
								throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unrecognized data binding: " + p.Binding.Name);
							LSIDStandardPortImpl portImpl = extractPort(s, p, portTypeName);
							lsidDataPorts[portImpl.getKey()] = portImpl;
						} 
							//else if (XmlQualifiedName.getLocalPart().equals(AUTHORITY_PORT_TYPE) && XmlQualifiedName.getNamespaceURI().equals(OMG_LSID_PORT_TYPES_WSDL_NS_URI)) 
						else if (portTypeName.Name.IndexOf(AUTHORITY_PORT_TYPE) != -1 && portTypeName.Namespace.Equals(OMG_LSID_PORT_TYPES_WSDL_NS_URI))
						{
							if (!p.Binding.Equals(AUTHORITY_SOAP_BINDING) && !p.Binding.Equals(AUTHORITY_HTTP_BINDING))
								throw new LSIDException(LSIDException.UNKNOWN_METHOD, "Unrecognized authority binding: " + p.Binding.Name);
							LSIDStandardPortImpl portImpl = extractPort(s, p, portTypeName);
							lsidAuthorityPorts[portImpl.getKey()] = portImpl;
						} 
						else 
						{
							LSIDPortFactory lpf = LSIDResolver.getConfig().getLSIDPortFactory(portTypeName);
							LSIDPort newPort = null;
							if (lpf == null) 
							{
								//todo localpart of service name for following?
								//service.getXmlQualifiedName().getLocalPart()
								newPort = new DefaultLSIDPort(s.Name, p.Name, p);
							} 
							else 
							{
								// might have to use whole XmlQualifiedName, not sure for now ???
								newPort = lpf.createPort(s.Name, p); 
							}
							wsdlExtensionPorts[getPortKey(newPort)] = newPort;
						}
					}
				}
			}
		}
    

		private LSIDStandardPortImpl extractPort(Service service, Port port, System.Xml.XmlQualifiedName portTypeName) 
		{
			String portName = port.Name;
			LSIDStandardPortImpl portImpl = new LSIDStandardPortImpl();
			portImpl.name = portName;
			portImpl.serviceName = service.Name; 
			IColEnumeration portElts = new IColEnumeration(port.Extensions);
			while (portElts.hasMoreElements()) 
			{ // expecting only 1
				Object portElt = portElts.nextElement();
				if (portElt is SoapAddressBinding) 
				{
					SoapAddressBinding soapaddr = (SoapAddressBinding) portElt;
					portImpl.location = soapaddr.Location;
					portImpl.protocol = SOAP;

				} 
				else if (portElt is HttpAddressBinding) 
				{
					HttpAddressBinding httpaddr = (HttpAddressBinding) portElt;
					portImpl.location = httpaddr.Location;
					portImpl.protocol = HTTP;
					String bindingName = port.Binding.Name;
                    if (bindingName.Equals(DATA_HTTP_BINDING_DIRECT.Name) || bindingName.Equals(METADATA_HTTP_BINDING_DIRECT.Name))
						portImpl.path = LSIDStandardPortTypes.PATH_TYPE_DIRECT;
					else
						portImpl.path = LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED;
				} 
					//todo do these using soap extensions
				else if (portElt is XmlElement) 
				{
					XmlElement pe = (XmlElement)portElt;
					if (pe.LocalName == "address" && pe.NamespaceURI == WSDLConstants.OMG_AUTHORITY_HTTP_BINDINGS_WSDL_NS_URI)
					{
						portImpl.location = pe.Attributes["location"].InnerText;
						portImpl.protocol = HTTP;
						String bindingName = port.Binding.Name;
                        if (bindingName.Equals(DATA_HTTP_BINDING_DIRECT.Name) || bindingName.Equals(METADATA_HTTP_BINDING_DIRECT.Name))
							portImpl.path = LSIDStandardPortTypes.PATH_TYPE_DIRECT;
						else
							portImpl.path = LSIDStandardPortTypes.PATH_TYPE_URL_ENCODED;
					}
					else if (pe.LocalName == "location" && pe.NamespaceURI == WSDLConstants.FTP_NS_URI)
					{
						portImpl.location = pe.Attributes["server"].Value;
						portImpl.path = pe.Attributes["filepath"].Value;
						portImpl.protocol = FTP;
					}
					else if (pe.LocalName == "location" && pe.NamespaceURI == WSDLConstants.FILE_NS_URI)
					{
						portImpl.location = pe.Attributes["filename"].Value;
						portImpl.protocol = FILE;
					}
				} 
				else 
				{
					throw new LSIDException("Unknown Port impl");
				}
			}
			return portImpl;
		}

		/**
		 * Create a port for the given binding, protocol, hostname, port and name.
		 * In the case of some protocols, the hostname might be the entire endpoing, (for SOAP this is the case). 
		 * If the portname is null, then a default name for the given protocol is chosen.
		 */
		private Port createPort(Service serv, Binding binding, LSIDStandardPort port) 
		{
			Port newPort = new Port();
			newPort.Binding = new XmlQualifiedName(binding.Name, binding.ServiceDescription.TargetNamespace);
			String portName = port.getName();
			String protocol = port.getProtocol();
			if (portName == null)
				portName = newPortName(protocol);
			newPort.Name = portName;
			if (protocol.Equals(HTTP)) 
			{
				HttpAddressBinding addr = new HttpAddressBinding();
				addr.Location = port.getLocation();
				newPort.Extensions.Add(addr);
			} 
			else if (protocol.Equals(FTP)) 
			{
				FTPLocationImpl loc = new FTPLocationImpl(port.getLocation(), port.getPath());				
				ftpPorts.Add(newPort.Name, loc);
				//newPort.Extensions.Add(loc);				
				try
				{
					System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(FTPLocationImpl));
					MemoryStream ms = new MemoryStream();
					TextWriter t = new StreamWriter(ms);
					s.Serialize(t, loc);

					ms.Position = 0;
					StreamReader rdr = new StreamReader(ms);
					String l = rdr.ReadToEnd();
					String m = l;
				}
				catch(Exception ex)
				{
					string m = ex.Message;
				}
			} 
				//TODO
				//            else if (protocol.equals(FILE)) 
				//            {
				//                FileLocation loc = new FileLocationImpl(port.getLocation());
				//                newPort.addExtensibilityElement(loc);
				//            } 
			else if (protocol.Equals(SOAP)) 
			{
				SoapAddressBinding addr = new SoapAddressBinding();
				addr.Location = port.getLocation();
				newPort.Extensions.Add(addr);
			}
			return newPort;
		}

		/**
		 * Generate a unique port name using the counter and protocol
		 */
		private String newPortName(String protocol) 
		{
			String name = protocol + "Port" + currPortNum.ToString();
			currPortNum++;
			return name;
		}

        /**
         * create the string for the target namespace of a WSDL doc with the given LSID
         */
        private static String getTargetNamespace(LSID lsid) 
        {
            if (lsid == null)
                return OMG_LSID_PORT_TYPES_WSDL_NS_URI;
            return "http://" + lsid.Authority.Authority + "/" + "availableServices?" + lsid.ToString();
        }

        /**
         * get the name of a key for a port
         */
        private String getPortKey(LSIDPort port) 
        {
            return (port.getServiceName() != null ? port.getServiceName() : SERVICE_NAME) + ":" + port.getName();
        }


		public class LSIDWSDLResolver : XmlUrlResolver
		{
			public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
			{
				return null;
			}

			public override System.Net.ICredentials Credentials
			{
				set
				{
				}
			}

			public override Uri ResolveUri(Uri baseUri, string relativeUri)
			{
				return base.ResolveUri (baseUri, relativeUri);
			}
		}

	}
   
    
}
