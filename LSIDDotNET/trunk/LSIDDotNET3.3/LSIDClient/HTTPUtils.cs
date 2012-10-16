using System;
using System.Collections;
using System.IO;
using System.Net;

using System.Security.Policy;

namespace LSIDClient
{
	/**
	 * 
	 * Utilities for HTTP
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class HTTPUtils 
    {	
        public static String WebProxy = null;


        /**
         * perform an HTTP GET
         * @param String the URL
         * @param Map the HTTP headers to send
         * @param Map the GET params to send, will be sent in addition to any headers already in the URL
         * @param LSIDCredentials the creds to send for basic auth
         */
        public static HTTPResponse doGet(String url, Hashtable headers, Hashtable parameters, LSIDCredentials credentials) 
        {
            WebRequest req = null;
            WebResponse resp = null;
            
            Stream inp = null;
            try 
            {
                Uri lsidURL = new Uri(addParamsToURL(url, parameters));
            	req = WebRequest.Create(lsidURL);

                if (WebProxy != null) req.Proxy = new WebProxy(WebProxy);

            	// add basic authentication in the http headers if present
            	// try creds from the port
            	if( credentials != null ) 
				{
					if ( credentials != null ) 
					{
						String password = (String)credentials.getProperty(LSIDCredentials.BASICUSERNAME);
						if (password != null) 
						{
							password += ":" + credentials.getProperty(LSIDCredentials.BASICPASSWORD);
						
							String encodedPassword = System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(password));
							req.Headers.Add(HTTPConstants.HEADER_AUTHORIZATION, "Basic " + encodedPassword);
						}
					}
            	}				
            	// add extra headers, such as no cache headers
            	if (headers != null) 
				{
                    IDictionaryEnumerator en = headers.GetEnumerator();
                    while (en.MoveNext())
                    {
                        req.Headers.Add(en.Key.ToString(), en.Value.ToString());            			
            		}					
            	}
            	
                resp = req.GetResponse();

            	inp = resp.GetResponseStream();
            	
            	String expHeader = resp.Headers[HTTPConstants.EXPIRES_HEADER];
            	DateTime expires = DateTime.MinValue;
            	if (expHeader != null)
				{
            		try 
					{
						expires = DateTime.ParseExact(expHeader, HTTPConstants.HTTP_DATE_FORMAT, System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
            			//expires = DateTime.Parse(expHeader);	
            		} 
					catch (Exception ) 
					{
						//todo log error
            			//throw new LSIDException(e,"Error parsing expiration date: " + expHeader);
            		}
            	}				
            	return new HTTPResponse(inp, resp.ContentType, expires);
            } 
            catch (Exception e) 
            {
            	if (inp == null)
            		throw new LSIDException(e,"Error making HTTP GET call");
            	try 
                {
            		String error = e.Message + " : " + GetStreamBuffer(inp);
            		throw new LSIDException(LSIDException.INTERNAL_PROCESSING_ERROR,"error : " + error);            		
            	} 
                catch (Exception ex) 
                {
            		throw new LSIDException(ex, LSIDException.INTERNAL_PROCESSING_ERROR, "IO Error reading http error, partial error message : " + ex.Message);	
            	} 
                finally 
                {
            		if (inp != null) 
                    {
                        try 
                        {
                            inp.Close();
                        } 
                        catch (Exception) {}
                    }
            	}				
            }
        }
	
        public static String GetStreamBuffer(Stream s)
        {
            String res = "";

            StreamReader rdr = new StreamReader(s);
			res = rdr.ReadToEnd();

//            Char[] read = new Char[256];
//            int count = rdr.Read( read, 0, 256 );
//            
//            while (count > 0) 
//            {
//                String str = new String(read, 0, count);
//                res += str;
//                count = rdr.Read(read, 0, 256);
//            }

            return res;
        }

        private static String addParamsToURL(String url, Hashtable parameters) 
        {
            if (parameters == null)
                return url;
            String paramStr = "";
            if (parameters != null) 
            {
                IDictionaryEnumerator e = parameters.GetEnumerator();
                if (e.MoveNext())
                {
                    paramStr += e.Key.ToString() + "=" + e.Value.ToString();
                }
                while (e.MoveNext()) 
                {
                    paramStr += "&" + e.Key.ToString() + "=" + e.Value.ToString();
                }						
            }
            for (int i=0;i<paramStr.Length;i++) 
            {
                paramStr = paramStr.Replace("+", "%2B");	
            }
            if (paramStr.Length > 0) 
            {				
                if (url.IndexOf("?") == -1)
                    return url + "?" + paramStr; // add query string
                return url + "&" + paramStr; // append to query string
            }
            
            return url;
        }
	
    }
}
