using System;
using System.Xml;
using System.Xml.XPath;

namespace LSIDClient
{
	/// <summary>
	/// Summary description for WebSettings.
	/// </summary>
	public class WebSettings
	{
        private String _proxy = null;

		public WebSettings()
		{
		}

        public String getProxy()
        {
            return _proxy;
        }

        public void setProxy(String proxy)
        {
            _proxy = proxy;
        }

        public void XmlDeserialise(XPathNavigator nav)
        {
            _proxy = null;
            XPathNodeIterator itr = nav.SelectChildren("proxy", nav.NamespaceURI);
            if (itr.MoveNext())
            {
                _proxy = itr.Current.Value;
            }            
        }

	}
}
