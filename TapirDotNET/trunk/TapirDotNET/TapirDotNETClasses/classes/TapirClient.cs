using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace TapirDotNET
{
    public class TapirClient
    {
        public static string QueryTapirResource(string resourceUrl, string queryUrl, string queryBody, string queryType)
        {
            string result = "";

            try
            {
                WebRequest http_request;

                if (queryType == "GET")
                {
                    http_request = WebRequest.Create(queryUrl);
                    http_request.Method = "GET";
                }
                else
                {
                    http_request = WebRequest.Create(queryUrl);
                    http_request.Method = "POST";

                    if (queryType == "RAWPOST")
                    {
                        http_request.ContentType = "text/xml; charset=\"utf-8\"";
                        Byte[] b = System.Text.Encoding.UTF8.GetBytes(queryBody);
                        http_request.ContentLength = b.Length;

                        Stream s = http_request.GetRequestStream();
                        s.Write(b, 0, b.Length);
                        s.Close();
                    }
                    else
                    {
                        string body = "request=" + queryBody;
                        http_request.ContentType = "application/x-www-form-urlencoded";
                        Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(body);
                        http_request.ContentLength = b.Length;

                        Stream s = http_request.GetRequestStream();
                        s.Write(b, 0, b.Length);
                        s.Close();
                    }
                }

                WebResponse res = http_request.GetResponse();

                StreamReader rdr = new StreamReader(res.GetResponseStream());

                result = rdr.ReadToEnd();
                rdr.Close();
            }
            catch (Exception ex)
            {
                TpLog.debug(ex.Message + " : " + ex.StackTrace);
            }

            return result;
        }
    }
}
