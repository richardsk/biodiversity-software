using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;


namespace LSIDClient
{
	/**
	*
	* Encapsulates the cache for an LSIDResolver.
	* A VM argument must be set to specify the location of the cache.  
	* The properties file cache-config.properties should reside in the root of
	* the cache tree. Other config files can be placed further down the tree to specify
	* tighter cache lifetime and cache size for those directories.  However, child directories 
	* will be held to the bounds of the parent directory if the child bounds are looser.  
	* If no cache config resides in a directory and no parent config exists, the default values 
	* below are assumed. 
	* <p>
	* <p>The environment can be set in the app config file.
	* <p>
	* <p>In general, this property will be set authomatically by the LSIDResolver system that is using the cache.
	* <p>However, if the user wishes to access the cache directly, or load it to perform maintenance, she should 
	* <p>ensure that the system property is set.
	* <p>
	* <p>The settings in the config file include:
	* <p>
	* <br># LSID CACHE CONFIGURATION, cache-config.xml
	* <br>
	* <br># The maximum cache size in bytes, -1 means no limit
	* <br>max-cache-size:10000000
	* <br>
	* <br># The maximum lifetime of a cached item in MILLIS, -1 means no limit
	* <br>max-cache-lifetime:-1
	* <br>
	* <br># logging configuration is taken only from root config file because all activity 
	* <br># is logged to the config file in the root
	* <br>
	* <br># Whether or not to log cache activity in the entire tree
	* <br>logging-on:true
	* <br>
	* <br># Which file in the root cache directory. 
	* <br>log-file:lsid-cache.log 
	* <br>
	* 
	* @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	*
	*/
    public class LSIDCache : IComparer, LSIDCachingInputStreamListener
    {	
        /**
         * The default cache dir for win32
         */
        public static readonly String DEFAULT_CACHE_DIR = "\\lsid-client\\cache";
	
	
        /**
         * The name of the cache config file
         */
        public static readonly String CACHE_CONFIG_FILE = "lsid-cache.xml";
	
        /**
         * The property which must be set for caching to work
         */
        public static readonly String CACHE_DIR_PROPERTY = "LSID_CACHE_DIR";
	
        // properties for conf file
	
        /**
         * The config file property for maximum cache size in bytes
         */
        public static readonly String MAX_CACHE_SIZE = "max-cache-size";
	
        /**
         * The config file property for maximum cache lifetime in millis
         */
        public static readonly String MAX_CACHE_LIFETIME = "max-cache-lifetime";
	
        /**
         * The config file property for the high bits of maximum cache size in bytes.  This is included
         * to provide compatability with the LSID COM Stack cache config.
         */
        public static readonly String MAX_CACHE_SIZE_HIGH = "max-cache-size-high";
	
        /**
         * The config file property for the high bits of maximum cache lifetime in millis. This is included
         * to provide compatability with the LSID COM Stack cache config.
         */
        public static readonly String MAX_CACHE_LIFETIME_HIGH = "max-cache-lifetime-high";
	
        /**
         * The config file property for toggling cache logging
         */
        public static readonly String LOGGING_ON="logging-on";
	
        /**
         * The config file property for toggling case mangling
         */
        public static readonly String MANGLE_CASE="mangle-case";
	
        /**
         * The config file property for cache log file.
         */
        public static readonly String LOG_FILE="log-file";
	
        // string constants
        private static readonly String STD_ERR="std-err";
        private static readonly String DATA_EXT="tmp";
        private static readonly String EXPIRATION_FILENAME = "EXPIRES";
        private static readonly String COMPLETE_FILE = "complete.tmp";
        private static readonly Hashtable METADATA_TYPE_2_EXTENSION = new Hashtable();
        private static readonly String[] ALL_FORMATS = {MetadataResponse.RDF_FORMAT,MetadataResponse.XMI_FORMAT,MetadataResponse.NO_FORMAT,null};
        		
        private static readonly string LOG_DATE_FORMAT = "EEE, d MMM yyyy HH:mm:ss:SSS";
	
        private int maxCacheSize = -1;
        private int maxCacheLifetime = -1;
        private Boolean loggingOn = false;
        private String logFile = STD_ERR;
		private StreamWriter logFileWriter = null;
	
        private int mangleCase = MANGLE_SYSTEM;
        private static readonly int MANGLE_SYSTEM = 0;
        private static readonly int MANGLE_ON = 1;
        private static readonly int MANGLE_SHA1 = 2;
        private static readonly int MANGLE_OFF = -1;
	
        private static String cacheDir;
		private static String cuurentCacheDir;
		
        // LSIDCache is a singleton class. 
        private static LSIDCache singletonInstance = null;
	
        /**
         * Load the configuration into a new instance and return it.  This loads the cache
         * configuration for the base of the cache tree specified in the system property
         * LSID_CACHE_DIR
         * @return LSIDCache a ready to use LSIDCache that can be used for lookups or maintenance.
         */
		public static LSIDCache load() 
		{
			METADATA_TYPE_2_EXTENSION.Add(MetadataResponse.RDF_FORMAT,"rdf");
			METADATA_TYPE_2_EXTENSION.Add(MetadataResponse.XMI_FORMAT,"xmi");	
			METADATA_TYPE_2_EXTENSION.Add(MetadataResponse.NO_FORMAT,"tmp");

			String dirprop = System.Configuration.ConfigurationSettings.AppSettings[CACHE_DIR_PROPERTY];
			Boolean newCacheDir = true;
			if (singletonInstance != null) 
			{
				if (dirprop != null) 
				{
					newCacheDir = !dirprop.Equals(LSIDCache.cacheDir);
				}
			}
			if (newCacheDir) 
			{
				String cacheDir = null;
				if (dirprop == null) 
				{
					cacheDir = DEFAULT_CACHE_DIR;
				} 
				else 
					cacheDir = dirprop;

				if (!File.Exists(cacheDir))
					Directory.CreateDirectory(cacheDir);
				singletonInstance = new LSIDCache(null,cacheDir);
			}
			return singletonInstance;
		}
	
        /**
         * Create a cache for the given cache dir.  If this method is used to load a cache then the cache should
         * not be used as unique global LSID cache.  Can can be used as a temporary file-based LSID store.
         * @param cacheDir
         * @return
         */
        public static LSIDCache load(String cacheDir) 
        {
            return new LSIDCache(null,cacheDir);
        }
	
        /**
         * Get location of this cache
         * @return String the canonicalized path of the cache directory.
         */
        public static String getLocation() 
        {
            try 
            {
                return Directory.GetParent(cacheDir).FullName;
            } 
            catch (IOException e) 
            {
                LSIDException.WriteError("Error canonicalizing path to cache dir: " + cacheDir);
                LSIDException.PrintStackTrace(e);
                return e.ToString();
            }	
        }
	
        /**
         * Get the log file
         * @return String the log file
         */
        public String getLogFileName() 
        {
            return logFile;	
        }
	
        /**
         * Get the list of LSIDs that we have WSDL cached for
         * @return LSID[] the list of LSIDs
         */
	
        /*
        public LSID[] getLSIDsWithCachedWSDL() {
            Vector lsidVector = new Vector();
            File wsdlDir = new File(cacheDir, "/wsdl");
            if (!wsdlDir.exists())
                return new LSID[0];
            File[] files = wsdlDir.listFiles();
            for (int i=0;i<files.length;i++) {
                File file = files[i];
                if (file.isDirectory()) {
                    File[] files2 = file.listFiles(new FileFilter() {
                        public boolean accept(File f) {
                            return f.toString().endsWith(".wsdl");	
                        }
                    });
                    for (int j=0;j<files2.length;j++) {
                        try {
                            String filename = files2[j].getName();
                            filename = getDecodedFilename(filename);
                            lsidVector.add(new LSID(filename.substring(0,filename.indexOf(".wsdl"))));	
                        } catch (MalformedLSIDException e) {
                            throw new LSIDCacheException(e,"Bad lsid name in WSDL cache: " + files2[j].getName());
                        }
                    }
                }
            }
            LSID[] lsids = new LSID[lsidVector.size()];
            for (int i=0;i<lsids.length;i++) {
                lsids[i] = (LSID)lsidVector.elementAt(i);
            }
            return lsids;
        }*/
	
        /**
         * Chech if this cache has a WSDL for the given LSID or authority.  Return the WSDL of it exists, 
         * null otherwise.
         * 
         * @param LSIDAuthority the authority on which the operation was invoked, or the authority whose WSDL we want
         * @param LSID the LSID to lookup.
         * @return LSIDWSDLWrapper the WSDL
         */
        public LSIDWSDLWrapper readWSDL(LSIDAuthority auth, LSID lsid) 
        {
            String fileName = null;
            if (lsid != null) 
                fileName = canonicalizeFilename(lsid.ToString());
            else // this is the case where we are caching the WSDL for an entire authority
                fileName = "authority";
            fileName += ".wsdl";
            string file = cacheDir + "\\wsdl\\" + auth + "\\" + fileName;
            if (!File.Exists(file))
                return null;
            Hashtable expEntries = loadExpirationEntries(Directory.GetParent(file).FullName);
            if (fileExpired(expEntries,fileName))
                return null;
            FileStream inp = null;
            try 
            {
                inp = File.OpenRead(file);
                LSIDWSDLWrapper wsdl = new LSIDWSDLWrapper(inp);
                long exp = getExpiration(expEntries,fileName);
                if (exp != -1)
                    wsdl.setExpiration(new DateTime(exp));
                
                if (getLoggingOn()) 
                {
                    if (lsid != null)
                        logOpenClose(cacheDir + "\\wsdl\\" + auth, "Read WSDL for " + lsid);
                    else
                        logOpenClose(cacheDir + "\\wsdl\\" + auth, "Read WSDL for " + auth);
                }
                return wsdl;
            } 
            catch (IOException e) 
            {
                throw new LSIDCacheException(e, "Error reading WSDL cache file: " + fileName);
            } 
            finally 
            {
                try 
                {
                    if (inp != null)
                        inp.Close();
                } 
                catch (IOException e) 
                {
                    throw new LSIDCacheException(e, "Error closing WSDL cache file: " + fileName);
                }	
            }
        }
	
        /**
         * check if this cache has data for the given LSID. Return an input stream to the data,
         * null otherwise.  Range parameters may be specified for a byte range within the data.  
         * @param LSIDAuthority the authority that returned the given data location
         * @param LSID the LSID to lookup.
         * @return InputStream an input stream for the data. 
         */
        public Stream readData(LSID lsid, int start, int length) 
        {
			lock(this)
			{
				string dir = cacheDir + "\\data\\" + canonicalizeFilename(lsid.ToString());
				string file = dir + "\\" + COMPLETE_FILE;
				if (File.Exists(file)) 
				{
					try 
					{
						// even though logOpenClose checks logging, we check so that we don't create the unnecessary file object
						if (getLoggingOn())
							logOpenClose(dir, "Read data for " + lsid);
						FileStream inp = File.OpenRead(file);
						if (start != -1)
							inp.Seek(start, SeekOrigin.Begin);
						return inp;
					} 
					catch (IOException e) 
					{
						throw new LSIDCacheException(e, "Error opening data cache file " + file);
					}
				} 
				else 
				{
					if (start == -1 && length == -1)
						return null;
					string[] files = Directory.GetFiles(dir);
					if (files.Length == 0)
						return null;
					string f = null;
					int s = -1;
					int l = -1;
					for (int i=0;i<files.Length;i++) 
					{
						String name = files[i];
						if (name.StartsWith("range")) 
						{
							int ind = name.LastIndexOf('_');
							s = int.Parse(name.Substring(6,ind));
							l = int.Parse(name.Substring(ind + 1, name.IndexOf('.')));
							if (s <= start && (s + l >= start + length)) 
							{
								f = files[i];
								break;
							}
						}
					}
					if (f != null) 
					{
						try 
						{
							// even though logOpenClose checks logging, we check so that we don't create the unnecessary file object
							if (getLoggingOn())
								logOpenClose(dir,"Read data for " + lsid);
							FileStream inp = File.OpenRead(f);
							inp.Seek(start-s, SeekOrigin.Begin);
							return inp;
						} 
						catch (IOException e) 
						{
							throw new LSIDCacheException(e, "Error opening data cache file " + f);
						}
					}
				}
				return null;		
			}
        }
	
        /**
         * check if this cache has meta data for the given LSID. Return an input stream to the data,
         * null otherwise.
         * @param LSIDAuthority the authority that provided the location of the meta data
         * @param LSID the LSID to lookup.
         * @param String the WSDL service that contained the meta data port
         * @param String the name of the meta data port
         * @param String[] the accepted formats
         * @return MetadataResponse that contains an InputStream for the meta data. 
         */
        public MetadataResponse readMetadata(LSIDAuthority authority, LSID lsid, String serviceName, String portName, String[] acceptedFormats) 
        {
			lock(this)
			{
				if (acceptedFormats == null)
					acceptedFormats = ALL_FORMATS;

				for (int i=0;i<acceptedFormats.Length;i++) 
				{
					String fileName = serviceName + "." + portName;
					if (acceptedFormats[i] != null)
						fileName += "." + METADATA_TYPE_2_EXTENSION[acceptedFormats[i].ToString()];
					else
						fileName += ".tmp";
					string file = cacheDir + "\\metadata\\" + authority + "\\" + canonicalizeFilename(lsid.ToString()) + "/" + fileName;
					Hashtable expEntries = loadExpirationEntries( Directory.GetParent( file).FullName);
					if (File.Exists(file) && !fileExpired(expEntries,fileName)) 
					{
						try 
						{
							// even though logOpenClose checks logging, we check so that we don't create the unnecessary file object
							if (getLoggingOn())
								logOpenClose(cacheDir + "/metadata/" + authority + "/" + canonicalizeFilename(lsid.ToString()), "Read meta data for " + lsid + " at: " + serviceName + "." + portName);
							return new MetadataResponse(File.OpenRead(file), getExpiration(expEntries,fileName),acceptedFormats[i].ToString());
						} 
						catch (FileNotFoundException e) 
						{
							throw new LSIDCacheException(e, "Error opening meta data cache file " + file);
						}
					} 
				}
				return null;	
			}
        }
	
        /**
         * Write the LSID WSDL from a string to the cache.
         * @param LSIDAuthority the authority on which the operation was invoked
         * @param LSID the lsid for the WSDL
         * @param LSIDWSDLWrapper the WSDL to cache.
         */
        public void writeWSDL(LSIDAuthority auth, LSID lsid, LSIDWSDLWrapper wsdl) 
        {
            string dir = cacheDir + "\\wsdl\\" + auth;
            Directory.CreateDirectory(dir);
            String fileName = null;
            String logStr = null;
            if (lsid != null) 
            {
                fileName = canonicalizeFilename(lsid.ToString());
                logStr = lsid.ToString();
            } 
            else 
            {
                fileName = "authority";
                logStr = auth.ToString();
            }
            fileName += ".wsdl";
            string file = dir + "\\" + fileName;
            StreamWriter outs = null;
		
            string cDir = getLocation() +  "\\temp\\";
            Directory.CreateDirectory(cDir);
			
            try 
            {
                string tempFile = cDir + "\\cache";		
                outs = File.CreateText(tempFile);
                outs.WriteLine(wsdl.ToString());
                outs.Flush();
                outs.Close();
                if (File.Exists(file))
                    File.Delete(file);
                File.Move(tempFile, file);
                File.Delete(tempFile);
                logOpenClose(dir,"Wrote WSDL for " + logStr);
            } 
            catch (IOException e) 
            {
                throw new LSIDCacheException(e, "Error writing WSDL for " + logStr);
            } 
            finally 
            {
                try 
                {
                    if (outs != null)
                        outs.Close();
                } 
                catch (IOException e) 
                {
                    throw new LSIDCacheException(e, "Error closing WSDL cache file for " + logStr);
                }
            }
            DateTime expiration = wsdl.getExpiration();
            if (expiration != DateTime.MinValue)
                updateExpirationEntry(dir,fileName,expiration.Ticks);
        }
	
        /**
         * Write the LSID data from an input stream to the cache.  This method opens and returns a new input
         * stream to the cached data so that users of this method will still be able to use the data.  If 
         * a copy of the stream is not required, callers must close the stream upon exit or caching may not be complete. 
         * @param LSID the lsid of the data
         * @param InputStream and input stream into the data to cache.  This input stream must not have been read from
         * before it is passed in and will be closed when the return stream has been closed.
         * @param int position in the data for the given lsid that the given stream starts at, 
         * -1 if the stream is all the data for the lsid.
         * @param int length of the stream, -1 if the stream is all the data for the lsid.
         * @return InputStream A fresh input stream to the data.  
         */
        public Stream writeData(LSID lsid, Stream data, int start, int length) 
        {
            cuurentCacheDir = cacheDir + "\\data\\" + canonicalizeFilename(lsid.ToString());
			Directory.CreateDirectory(cuurentCacheDir);
            String fileName = null;
            if (start == -1 && length == -1)
                fileName = COMPLETE_FILE;
            else
                fileName = "range_" + start + "_" + length + "." + DATA_EXT;
            
			//return File.OpenWrite(dir + "\\" + fileName);

            LSIDCachingInputStream lcis = new LSIDCachingInputStream(data, cuurentCacheDir + "\\" + fileName, null, lsid);
			lcis.setInputStreamListener(this);
            return lcis;	
        }

		public void inputStreamClosed(LSIDAuthority authority, LSID lsid) 
		{
			try 
			{
				logOpenClose(cuurentCacheDir, "Wrote data for " + lsid);
			} 
			catch (LSIDCacheException e) 
			{
				e.PrintStackTrace();
				throw new IOException("Error handling inputStreamClosed event for " + lsid + " : " + e.getMessage());
			}
		}

        /**
         * Write the LSID data from an input stream to the cache.  This method opens and returns a new input
         * stream to the cached data so that users of this method will still be able to use the data.  If 
         * a copy of the stream is not required, callers must close the stream upon exit or cache may not be complete. 
         * @param LSIDAuthority the authority
         * @param LSID the lsid of the data
         * @param String the name of the service that contains the meta data port
         * @param String the name of the meta data port
         * @param InputStream and input stream into the data to cache.  This input stream must not have been read from
         * before it is passed in and will be closed when the return stream has been closed
         * @param Date The expiration date/time in millis.
         * @param String the format
         * @return InputStream A fresh input stream to the data.  
         */
        public Stream writeMetadata(LSIDAuthority authority, LSID lsid, String serviceName, String portName, Stream data, DateTime expiration, String format) 
        {
			lock(this)
			{
				cuurentCacheDir = cacheDir + "\\metadata\\" + authority + "\\" + canonicalizeFilename(lsid.ToString());
				Directory.CreateDirectory(cuurentCacheDir);
				String fileName = serviceName + "." + portName;
				if (format != null)
					fileName += "." + METADATA_TYPE_2_EXTENSION[format].ToString();
				else
					fileName += ".tmp";
				string file = cuurentCacheDir + "\\" + fileName;
				
				LSIDCachingInputStream lcis = new LSIDCachingInputStream(data, file, authority, lsid);
				lcis.setInputStreamListener(this);
				return lcis;

			}
        }
		
        /** 
         * Update the cache according to policy.
         */
        public void maintainCache() 
        {
            openLogWriter();
            try 
            {
                maintainCache(-1);
            } 
            finally 
            {
                closeLogWriter();
            }	 			
        }	
	
        /**
         * Construct a new LSID Cache using the parent cache for configuration and the given directory
         */
        private LSIDCache(LSIDCache parent, String cacheDir) 
        {
            // load the configuration from the properties file if it exists, otherwise use the parent
            LSIDCache.cacheDir = cacheDir;
            String cacheFile = cacheDir + "\\" +  CACHE_CONFIG_FILE;
            if (File.Exists(cacheFile)) 
            {
                try 
                {
					XmlDocument d = new XmlDocument();
					d.Load(cacheFile);
					
					String mcs = "10"; 
					String mcl = "1000"; //props.getProperty(MAX_CACHE_LIFETIME);
					String mcshigh = "15"; //props.getProperty(MAX_CACHE_SIZE_HIGH);
					String mclhigh = "2000"; //props.getProperty(MAX_CACHE_LIFETIME_HIGH);
					//String mangle = props.getProperty(MANGLE_CASE);
					String log = "false"; //props.getProperty(LOGGING_ON);
					logFile = "log.txt"; //props.getProperty(LOG_FILE);
                      
					XmlNode n = d.SelectSingleNode(MAX_CACHE_SIZE);
					if (n != null)
					{
						mcs = n.Value;
					}

					n = d.SelectSingleNode(MAX_CACHE_LIFETIME);
					if (n != null)
					{
						mcl = n.Value;
					}
					
					n = d.SelectSingleNode(MAX_CACHE_SIZE_HIGH);
					if (n != null)
					{
						mcshigh = n.Value;
					}

					n = d.SelectSingleNode(MAX_CACHE_LIFETIME_HIGH);
					if (n != null)
					{
						mclhigh = n.Value;
					}

					n = d.SelectSingleNode(LOGGING_ON);
					if (n != null)
					{
						log = n.Value;
					}

					n = d.SelectSingleNode(LOG_FILE);
					if (n != null)
					{
						logFile = n.Value;
					}

                    try 
                    {
                        maxCacheSize = int.Parse(mcs);
                    } 
                    catch (Exception e) 
                    {
                        throw new LSIDCacheException(e, "Error parsing cache config value: " + mcs); 
                    }

					//todo?
//                    if(mangle != null) 
//                    {
//                        try 
//                        {
//                            mangleCase = int.Parse(mangle);
//                            if(mangleCase != MANGLE_OFF && mangleCase != MANGLE_ON 
//                                && mangleCase != MANGLE_SYSTEM && mangleCase != MANGLE_SHA1) 
//                            {
//                                throw new Exception("Error parsing cache config mangle value: " + mangle); 
//                            }
//                        } 
//                        catch (Exception e) 
//                        {
//                            throw new Exception(e, "Error parsing cache config mangle value: " + mangle); 
//                        }
//                    }

                    if (mcshigh != null) 
                    {
                        try 
                        {
                            int i = int.Parse(mcshigh);
                            i <<= 32;
                            maxCacheSize = i + maxCacheSize; 
                        } 
                        catch (Exception e) 
                        {
                            throw new LSIDCacheException(e, "Error parsing cache config value: " + mcshigh); 
                        }
                    }
                    try 
                    {
                        maxCacheLifetime = int.Parse(mcl);
                    } 
                    catch (Exception e) 
                    {
                        throw new LSIDCacheException(e, "Error parsing cache config value: " + mcl); 
                    }
                    if (mclhigh != null) 
                    {
                        try 
                        {
                            int i = int.Parse(mclhigh);
                            i <<= 32;
                            maxCacheLifetime = i + maxCacheLifetime; 
                        } 
                        catch (Exception e) 
                        {
                            throw new LSIDCacheException(e, "Error parsing cache config value: " + mclhigh); 
                        }
                    }
                    try 
                    {
                        loggingOn = (log.ToLower() == "true");
                    } 
                    catch (Exception e) 
                    {
                        throw new LSIDCacheException(e, "Error parsing cache config value: " + log); 
                    }
                    // apply any tighter bounds or logging instructions from the parent
                    if (parent != null) 
                    {
                        if (parent.getMaxCacheSize() != -1 && parent.getMaxCacheSize() < maxCacheSize)
                            maxCacheSize = parent.getMaxCacheSize();
                        if (parent.getMaxCacheLifetime() != -1 && parent.getMaxCacheLifetime() < maxCacheLifetime)
                            maxCacheLifetime = parent.getMaxCacheLifetime();
                        if (!parent.getLoggingOn())
                            loggingOn = false;
                    }
                } 
                catch (IOException e) 
                {
                    throw new LSIDCacheException(e, "Error loading properties file from: " + cacheDir + "/" + CACHE_CONFIG_FILE);
                } 
                finally 
                {
                }
            } 
            else 
            {
                if (parent != null)
                    copyConfig(parent);
            }
            // copy the file logger from the parent
            if (parent != null)
                logFileWriter = parent.logFileWriter;	
        }
	
        /**
         * The timestamp is the time when the outer maintenance call was made.  This timestamp is used for
         * all time-based decisions.  
         */
        private ArrayList maintainCache(long timestamp) 
        {
            // make a timestamp to use when filter files by date so that all files in the tree receive equal treatment
            if (timestamp == -1)
                timestamp = DateTime.Now.Ticks;
            log(cacheDir, "Beginning cache maintenance");
            ArrayList remainingFiles = new ArrayList();
            string[] dirListing = Directory.GetFiles(cacheDir);
            for (int i=0;i<dirListing.Length;i++) 
            {
                string file = dirListing[i];
                // if we have a directory, process the remaining files after recursively applying policy
                if (Directory.Exists(file)) 
                {
                    // since we pass down our time bounds to the children, we don't need to time filter the results
                    // of cache maintenance on the children. 
                    remainingFiles.AddRange(new LSIDCache(this,file).maintainCache(timestamp));
                    // if the director is empty, delete it.
                    // note, we accept the side effect of directories with only the config and log files remaining
                    // not being deleted because the user might wish to save them.  The user
                    // could then manually remove such a dir.
                    if (Directory.GetFiles(file).Length == 0) 
                    {
                        log(cacheDir,"Directory " + file + " is empty, removing");
                        File.Delete(file);
                    }
                } 
                else if (!file.Equals(CACHE_CONFIG_FILE) && !file.Equals(logFile) && !file.Equals(EXPIRATION_FILENAME)) 
                {
                    if (filterFileByDate(file, new DateTime(timestamp)))
                        remainingFiles.Add(file);
                }
            }
            filterFilesBySize(remainingFiles);
            log(cacheDir,"Cache maintenance complete");	
            return remainingFiles;
        }
	
        /**
         * return the maximum size of the cache in bytes
         */
        private int getMaxCacheSize() 
        {
            return maxCacheSize;
        }
	
        /**
         * return the maximum lifetime of a cache entry in millis
         */
        private int getMaxCacheLifetime() 
        {
            return maxCacheLifetime;
        }	
	
        /**
         * return the base-dir of the cache tree
         */
        private string getCacheDir() 
        {
            return cacheDir;
        }
	
        /**
         * return whether or not cache reads/writes should be logged
         */
        private Boolean getLoggingOn() 
        {
            return loggingOn;
        }
	
        /**
         * return the name of the log file
         */
        private String getLogFile() 
        {
            return logFile;
        }
	
        /**
         * Initializes the log writer.
         */
        private void openLogWriter() 
        {
            try 
            {
                if (getLoggingOn() && !logFile.Equals(STD_ERR)) 
                {
                    logFileWriter = new StreamWriter(cacheDir + "\\" + logFile, true); // append to the log
                }
            } 
            catch (IOException e) 
            {
                throw new LSIDCacheException(e, "error opening cache log file: " + cacheDir + "/" + logFile);
            }
        }
	
        /**
         * Closes the log writer.
         */
        private void closeLogWriter() 
        {
            if (logFileWriter != null)
                logFileWriter.Close();
        }
	
        /**
         * Logging is configured in the root. Log using the given print writer.  In the message,
         * print the time, the active directory (dir), and the msg
         */
        private void log(string dir, String msg) 
        {
            if (getLoggingOn()) 
            {
                string stamp = DateTime.Now.ToString(LOG_DATE_FORMAT);
                if (logFileWriter != null) 
                {
                    logFileWriter.WriteLine("[" + stamp + "]" + "[" + dir + "]" + msg);
                    logFileWriter.Flush();
                } 
                else 
                {
                    // write the directory name when we log to std err.
					LSIDException.WriteError("[" + stamp + "]" + "[" + dir + "]" + msg);
                }
            }
        }

        /**
         * Opens the log, writes a message, then closes it
         */
        private void logOpenClose(string dir, String msg) 
        {
            // always check logging to avoid extra function calls.
            if (getLoggingOn()) 
            {
                try 
                {
                    openLogWriter();
                    log(dir, msg);
                } 
                finally 
                {
                    closeLogWriter();
                }
            }
        }
	
        /**
         * process a single file.  Return true if file remains in cache.
         * Delete if it doesn't meet the time requirement
         */
        private Boolean filterFileByDate(String file, DateTime timestamp) 
        {
            if (maxCacheLifetime == -1)
                return true;
            TimeSpan ts = timestamp - File.GetLastWriteTime(file);

            if (ts.TotalSeconds >= maxCacheLifetime) 
            {
                //log(cacheDir,"File " + file + " has expired, deleting");
                File.Delete(file);
                return false;
            }			
            return true;
        }
	
        /**
         * process a list of files.  Remove files, starting with oldest until size req is met
         */
        	private void filterFilesBySize(ArrayList files) 
            {
        		// first sort the files in increasing order of age.  
        		// the compare function below specifies how this list is sorted.
        		if (maxCacheSize == -1)
        			return;
                
				files.Sort(this);

        		// now compute the total size
        		long totalSize = 0;
        		for (int i=0; i<files.Count; i++) 
				{					
        			totalSize += new FileInfo(files[i].ToString()).Length;
        		}
        		// have to continue while maxCacheSize == 0 so that we delete empty files
        		while (totalSize > maxCacheSize || (maxCacheSize == 0 && files.Count > 0)) 
				{
        			String f = files[0].ToString();
        			totalSize -= new FileInfo(f).Length;
        			log(cacheDir,"Cache too big, removing file " + f);
        			File.Delete(f);
        			files.RemoveAt(0);
        		}				
        	}
	
        /**
         * check if a file has expired
         */
        private static Boolean fileExpired(Hashtable entries, String filename) 
        {
            long timestamp = DateTime.Now.Ticks;
            ExpEntry entry = (ExpEntry)entries[filename];
            if (entry == null)
                return false;
            if (entry.expires == -1)
                return false;
            return timestamp >= entry.expires;
        }
	
        /**
         * get the expiration date
         */
        private static long getExpiration(Hashtable entries, String filename) 
        {
            ExpEntry entry = ((ExpEntry)entries[filename]);
            if (entry == null)
                return -1;
            return entry.expires;
        }
	
        /**
         * create or update an entry for a file in the expiration file
         */	
        private void updateExpirationEntry(string dir, String filename, long expiration) 
        {
            string expFile = dir + "\\" + EXPIRATION_FILENAME;
            Hashtable entries = loadExpirationEntries(dir);
            entries[filename] = new ExpEntry(filename,expiration);
            writeExpirationEntries(dir,entries);
        }
	
        /**
         * load the expiration file entries into a hashtable
         */
        private  Hashtable loadExpirationEntries(string dir) 
        {
            string expFile = dir + "\\" + EXPIRATION_FILENAME;
            if (!File.Exists(expFile))
                return new Hashtable();
            StreamReader reader = null;
            Hashtable entries = null;
            try 
            {
                reader = File.OpenText(expFile);
                entries = new Hashtable();
                String line = reader.ReadLine();
                while (line != null) 
                {
                    ExpEntry entry = new ExpEntry(line);
                    entries[entry.filename] = entry;
                    line = reader.ReadLine();
                }
            } 
            catch (IOException e) 
            {
                throw new LSIDCacheException(e, "Error updating expiration file in directory: " + dir);	
            } 
            finally 
            {
                if (reader != null) 
                {
                    try 
                    {
                        reader.Close();
                    } 
                    catch (IOException e) 
                    {
						LSIDException.PrintStackTrace(e);
                    }	
                }
            }
            return entries;	
        }
	
        /**
         * write out the expiration file entries
         */
        private static void writeExpirationEntries(string dir, Hashtable entries) 
        {
            string expFile = dir + "\\" + EXPIRATION_FILENAME;
            StreamWriter tempFile = null;
            try 
            {
                if (File.Exists(expFile))
                    File.Delete(expFile);
                String cd = getLocation() + "\\temp\\";
				Directory.CreateDirectory(cd);
                tempFile = File.CreateText(cd + "cache.tmp");			
					
                //writer = new PrintWriter(new FileWriter(tempFile.getCanonicalPath(),false));		
                IEnumerator filenames = entries.Keys.GetEnumerator();
                while (filenames.MoveNext()) 
                {
                    ExpEntry entry = (ExpEntry)entries[filenames.Current];
					tempFile.WriteLine(entry.filename + ":" + entry.expires);
                    tempFile.Flush();
                }
                tempFile.Close();
				File.Move(cd + "cache.tmp", expFile);
                File.Delete(cd + "cache.tmp");
            } 
            catch (IOException e) 
            {
                throw new LSIDCacheException(e,"Error writing expiration file to: " + dir);
            } 
            finally 
            {
                if (tempFile != null)
                    tempFile.Close();	
            }		
        }
	
        /**
         * Copies the config from the cache argument into this cache
         */
        private void copyConfig(LSIDCache cache) 
        {
            maxCacheLifetime = cache.getMaxCacheLifetime();
            maxCacheSize = cache.getMaxCacheSize();
            loggingOn = cache.getLoggingOn();
        }
		
        /**
         * Format the String so that it may be used as a valid filename.  Replace illegal filename chars
         * with their ascii represenation: %HEX_VALUE
         */
        public String canonicalizeFilename(String name) 
        {
            /*
            StringBuffer src = new StringBuffer(name);
            StringBuffer sink = new StringBuffer();
            for (int i=0;i<src.length();i++) {
                char c = src.charAt(i);
                if (c == '<')
                    sink.append("%3C");
                else if (c == '>')
                    sink.append("%3E");
                else if (c == '?')
                    sink.append('^');
                else if (c == ':')
                    sink.append(';');
                else if (c == '/')
                    sink.append("%2F");
                else
                    sink.append(c);		
            }
            return sink.toString();
            */
		
            if (mangleCase == MANGLE_ON) 
            {
                return getEncodedFilename(name);
            } 
            else if (mangleCase == MANGLE_OFF) 
            {
                return getEncodedFilename(name);
            } 
            else if(mangleCase == MANGLE_SHA1) 
            {
                try 
                {
                    System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create(name);
                    ArrayList chars = new ArrayList();
                    chars.AddRange(sha1.Hash);

                    return new String((char[])chars.ToArray(typeof(char)));

//                    sha1 = MessageDigest.getInstance("SHA");
//                    sha1.update(name.getBytes("UTF-8"));
//                    byte[] hash = sha1.digest();
//                    char[] ca = new char[hash.length*2];
//                    for (int i=0, j=0; i<hash.length; ++i) 
//                    {
//                        ca[j++]="0123456789ABCDEF".charAt((hash[i]>>4) & 0x0F);
//                        ca[j++]="0123456789ABCDEF".charAt(hash[i] & 0x0F);
//                    }
                    
                } 
                catch (Exception e) 
                {
                    throw new LSIDCacheException(e, "Could not find SHA1 hashing algorithm");
                } 
//                catch (UnsupportedEncodingException e) 
//                {
//                    throw new LSIDCacheException("Could not find UTF-8 encoding");
//                }
			
			
			
            } 
            else 
            {
                return getEncodedFilename(name);
//                if (File.separatorChar == '/') 
//                {
//                    return getEncodedFilename(name);
//                } 
//                else 
//                {
//                    return getEncodedFilename(name);
//                }
            }
		
        }
	
        /**
         * Simple struct to store expfile entries
         */
        private class ExpEntry 
        {
		
            public String filename;
            public long expires;
		
            public ExpEntry(String filename, long expires) 
            {
                this.filename = filename;
                this.expires = expires;
            }
		
            public ExpEntry(String entryStr) 
            {
                int ind = entryStr.IndexOf(':');
                if (ind == -1)
                    throw new LSIDCacheException("Bad Expiration file entry: " + entryStr);
                filename = entryStr.Substring(0,ind);
                try 
                {
                    expires = long.Parse(entryStr.Substring(ind+1));
                } 
                catch (Exception e) 
                {
                    throw new LSIDCacheException(e, "Bad Expiration file entry: " + entryStr);
                }
            }
			
        }
        /**
         * Returns the singletonInstance, may be null if not initialized.
         * @return LSIDCache
         */
        public static LSIDCache getSingletonInstance() 
        {
            return singletonInstance;
        }
	
	
        /**
         * Encode a string with url encoding, leaving ._-0-9a-z (A-Z)
         * @param s
         * @param encodeUpperCase
         * @return
         */
	
        /**
         * Decode an entire url encoded string
         * @param s
         * @return
         */
        public static String getDecodedFilename(String s) 
        {
            return System.Web.HttpUtility.UrlDecode(s);
        }
	
        /**
         * URLEncode a string for windows based systems
         * @param s
         * @return
         */
        public static String getEncodedFilename(String s) 
        {
            return System.Web.HttpUtility.UrlEncode(s);
        }

	
		#region IComparer Members

		/**
		 * Returns 1 if f1 was modified later than f2
		 * Returns -1 if f2 was modified later than f1
		 * Returns 0 if f1 and f2 were modified at the same time
		 *
		 * Note: this comparator imposes orderings that are inconsistent with equals()
		 * 
		 */
		public int Compare(object x, object y)
		{
			LSIDCache c = (LSIDCache)x;
			LSIDCache c2 = (LSIDCache)y;
			long file1mod = File.GetLastWriteTime(c.getCacheDir()).Ticks;
			long file2mod = File.GetLastWriteTime(c2.getCacheDir()).Ticks;
			if (file1mod > file2mod)
				return 1;
			if (file1mod < file2mod)
				return -1;
			return 0;
		}

		#endregion
	}
}
