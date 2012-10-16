using System;
using System.Web.Services.Protocols;
using System.Collections;
using System.Xml;

using Microsoft.Web.Services2;

namespace LSIDClient
{
    public class SOAPProxy : SoapHttpClientProtocol
    {
        public SOAPProxy(string url)
        {
            this.Url = url;
            this.Discover();
        }

        public object[] InvokeCall(string functionName, object[] parameters)
        {
            return this.Invoke(functionName, parameters);
        }
    }

    
	/**
	 * 
	 * This class provides utility methods for building and parsing SOAP Envelopes.  These methods are very specific to
	 * LSID Web Service methods and rely several assumptions about what will be received and sent.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
    public class SOAPUtils 
    {	
		public static LSIDSoapTransport LSIDTransport = new LSIDSoapTransport();

		public static SoapEnvelope MakeSoapCall(string destination, string opName, string nameSpace, Hashtable nsPrefixes, Hashtable parameters, LSIDCredentials credentials)
		{
			LSIDWebServiceClient cl = new LSIDWebServiceClient();
			                    
			Microsoft.Web.Services2.SoapEnvelope env = new Microsoft.Web.Services2.SoapEnvelope();
													
			env.CreateBody();
								
			string body = "<ls:" + opName + " xmlns:ls='" + nameSpace + "' ";
			
			if (nsPrefixes != null)
			{
				foreach (String pref in nsPrefixes.Keys)
				{
					body += "xmlns:" + pref + "='" + nsPrefixes[pref] + "' ";
				}
			}

			body += ">";

			foreach (string key in parameters.Keys)
			{
				body += "<" + key + ">" + parameters[key] + "</" + key + ">"; 
			}
			body += "</ls:" + opName + ">";
					                   
			env.Body.InnerXml = body;
                    
			cl.Destination = new Microsoft.Web.Services2.Addressing.EndpointReference(new Uri(destination));
					
			String method = nameSpace + "/" + opName;

			LSIDTransport.Credentials = credentials;
			Microsoft.Web.Services2.SoapEnvelope respEnv = cl.SendMessage(method, env);

			return respEnv;
		}


        // date time, works with Java 1.4
        private static string DATE_TIME_FORMAT = "yyyy-MM-dd'T'HH:mm:ss.SSSZ";
        // date time
        private static string DATE_TIME_FORMAT_131 = "yyyy-MM-dd'T'HH:mm:ss.SSS";
		
	
        public static String writeDate(DateTime date) 
        {
            return date.ToString(DATE_TIME_FORMAT);
        }
	
        public static DateTime parseDate(String date) 
        {
            try 
            {				
                try 
				{
					return DateTime.ParseExact(date, DATE_TIME_FORMAT, System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                } 
                catch (Exception) 
                { // some date format constructs are not supported in JDK 131 and below                    
					return DateTime.ParseExact(date, DATE_TIME_FORMAT_131, System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                }
            } 
            catch (Exception e) 
            {
                throw new LSIDException(e,"Error parsing date: " + date);
            }
        }
    }
}
