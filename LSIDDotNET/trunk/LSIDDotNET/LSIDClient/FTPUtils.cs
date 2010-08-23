using System;
using System.IO;
using System.Net;

namespace LSIDClient
{
    public class FTPUtils 
    {

        /**
         * do an FTP get on the given location and path
         * @param String the server
         * @param String the file path
         * @return InputStream the data
         */
        public static Stream doGet(String location, String path, LSIDCredentials credentials) 
        {
            Stream inp = null;
            try 
            {
				String url = "file://" + location;
				if (!url.EndsWith("/") && !url.EndsWith("\\")) url += "/";
				url += path;
				WebRequest req = FileWebRequest.Create(new Uri(url));
				if (HTTPUtils.WebProxy != null) req.Proxy = new WebProxy(HTTPUtils.WebProxy);
				inp = req.GetResponse().GetResponseStream();		
                return inp;
            } 
            catch (IOException e) 
            {
                if (inp != null) 
                {
                    try 
                    {
                        inp.Close();
                    } 
                    catch (IOException ex) 
                    {
                        LSIDException.PrintStackTrace(ex);
                    }
                }
                throw new LSIDException(e, "Error getting stream with FTP at: " + location);
            } 	
        }
    }
}
