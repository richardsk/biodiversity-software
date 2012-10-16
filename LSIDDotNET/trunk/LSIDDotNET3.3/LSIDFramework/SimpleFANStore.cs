using System;
using System.IO;
using System.Collections;

using LSIDClient;

namespace LSIDFramework
{
    
    public class SimpleFANStore 
    {

        /** 
         * Add a lsid authority and lsid to the store
         * 
         * @param authority
         * @param lsid
         * @throws LSIDServerException
         */
        public static void addFAP(LSIDAuthority authority, LSID lsid) 
        {
            String FAPHome = getFAPHome();
            String LSIDauth = FAPHome + "\\" + LSIDCache.getEncodedFilename(lsid.Lsid);
            StreamWriter os = null;
            try 
            {
                os = File.CreateText(LSIDauth);
                os.WriteLine(authority.Authority);
            } 
            catch (FileNotFoundException e) 
            {
                LSIDException.PrintStackTrace(e);
            } 
            catch (IOException e) 
            {
                LSIDException.PrintStackTrace(e);
            }
            finally 
            {
                if(os != null) 
                {
                    try 
                    {
                        os.Close();
                    } 
                    catch (IOException e1) 
                    {
                        LSIDException.PrintStackTrace(e1);
                    }
                }
            }

        }
	
        /**
         * remove an authority from the FAN list for a specific lsid
         * @param authority
         * @param lsid
         * @throws LSIDServerException
         */
        public static void removeFAP(LSIDAuthority authority, LSID lsid) 
        {
            ArrayList v = getFAPsFromFile(lsid);
            if(!v.Contains(authority.Authority)) 
            {
                throw new LSIDServerException(LSIDServerException.UNKNOWN_LSID,"LSID does not have authority listed in FAN");
            }
            v.Remove(authority.Authority);
            String FAPHome = getFAPHome();
            String LSIDauth = FAPHome + "\\" + LSIDCache.getEncodedFilename(lsid.Lsid);
            StreamWriter os = null;
            try 
            {
                os = File.CreateText(LSIDauth);
                for(int i = 0; i < v.Count; ++i) 
                {			
                    os.WriteLine(v[i]);
                }			
            } 
            catch (FileNotFoundException e) 
            {
                LSIDException.PrintStackTrace(e);
            } 
            catch (IOException e) 
            {
                LSIDException.PrintStackTrace(e);
            }
            finally 
            {
                if(os != null) 
                {
                    try 
                    {
                        os.Close();
                    } 
                    catch (IOException e1) 
                    {
                        LSIDException.PrintStackTrace(e1);
                    }
                }
            }		
        }
	
	
        /**
         * return a list of foreign lsid authorities for a given lsid
         * @param lsid
         * @return
         * @throws LSIDServerException
         */
        public static LSIDAuthority[] lookupFAPs(LSID lsid) 
        {
            ArrayList v = getFAPsFromFile(lsid);
            if(v.Count == 0) 
            {
                return new LSIDAuthority[0];
            }
		
            LSIDAuthority[] auths = new LSIDAuthority[v.Count];
            for(int i = 0; i < v.Count; ++i) 
            {
                try 
                {
                    auths[i] = new LSIDAuthority((String)v[i]);
                } 
                catch (MalformedLSIDException) 
                {
                    // skip a malformed authority
                }
            }
            return auths;
        }
	
        protected static ArrayList getFAPsFromFile(LSID lsid)
        {
            String FAPHome = getFAPHome();
            String LSIDAuth = FAPHome + "\\" + LSIDCache.getEncodedFilename(lsid.Lsid);
            ArrayList v = new ArrayList(1);
            if(!File.Exists(LSIDAuth)) 
            {
                return v;
            }

            // read file to find previous
            StreamReader input = null;		
            try 
            {
                input = File.OpenText(LSIDAuth);
                String line = null;
                while ((line = input.ReadLine()) != null) 
                {
                    if (! v.Contains(line))
                    {				
                        v.Add(line);
                    }
                }
            } 
            catch (FileNotFoundException) 
            {
                throw new LSIDServerException(LSIDServerException.INTERNAL_PROCESSING_ERROR,"FAP file not found.");
            } 
            catch (IOException) 
            {
                throw new LSIDServerException(LSIDServerException.INTERNAL_PROCESSING_ERROR,"Could not read FAP file.");
            } 
            finally 
            {
                try 
                {
                    if (input != null) 
                    {
                        input.Close();
                    }
                } 
                catch (IOException ex) 
                {
					LSIDException.PrintStackTrace(ex);                    
                }
            }
            return v;
        }

	
        /**
         * Return FAP home directory
         * @return
         * @throws LSIDServerException
         */
        protected static String getFAPHome() 
        {
            String clientHome = System.Configuration.ConfigurationSettings.AppSettings[LSIDResolverConfig.LSID_CLIENT_HOME];
            if(clientHome == null) 
            {
                throw new LSIDServerException(LSIDServerException.SERVER_CONFIGURATION_ERROR,"Unable to create FAN store directory");
            }
            if(!Directory.Exists(clientHome)) 
            {
                Directory.CreateDirectory(clientHome);
            }		
			if (!clientHome.EndsWith("\\")) clientHome += "\\";
            String FAPDir = clientHome + "FAN";
            if(!Directory.Exists(FAPDir)) 
            {
                Directory.CreateDirectory(FAPDir);
            }
		
            if(Directory.Exists(FAPDir)) 
            {
                return FAPDir;
            } 
            else 
            {
                throw new LSIDServerException(LSIDServerException.SERVER_CONFIGURATION_ERROR,"Unable to create FAN store directory");
            }
        }
    }
	
}
