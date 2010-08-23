using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TapirDotNET
{
    public class TpLsidResolver
    {
        private string _lsid;
        private string _authorityCode;
        private string _namespace;
        private Hashtable _settings = new Hashtable();
        private string _error;

        private Utility.OrderedMap _InTags = new Utility.OrderedMap();
        private string _InNamespace;

        public TpLsidResolver(string lsid)
        {
            _lsid = lsid;

            string[] parts = lsid.Split(':');

            if (parts.Length > 4)
            {
                _authorityCode = parts[2];
                _namespace = parts[3];

                LoadSettings("");
            }
            else
            {
                _error = "LSID has less then 5 parts";
            }

        }

        private bool LoadSettings(string configFile)
        {
            if (configFile == "")
            {
                configFile = TpConfigManager.TP_CONFIG_DIR + "\\lsid_settings.xml";
            }

            try
            {
                TpXmlReader rdr = new TpXmlReader();
                rdr.StartElementHandler = new StartElement(this.StartElement);
                rdr.EndElementHandler = new EndElement(this.EndElement);
                rdr.CharacterDataHandler = new CharacterData(this.CharacterData);

                rdr.ReadXml(configFile);
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                new TpDiagnostics().Append(TpConfigManager.CFG_INTERNAL_ERROR, ex.Message, TpConfigManager.DIAG_ERROR);
                return false;
            }

            return true;
        }
        
		public virtual void  StartElement(TpXmlReader reader, Utility.OrderedMap attrs)
        {
            _InTags.Push(reader.XmlReader.Name);

            if (reader.XmlReader.Name == "LSIDNamespace")
            {
                if (reader.XmlReader.GetAttribute("Name") != null)
                {
                    _InNamespace = reader.XmlReader.GetAttribute("Name");
                    _settings.Add(_namespace, new Hashtable());
                }
            }
        }
        
		public virtual void  EndElement(TpXmlReader reader)
        {
            _InTags.Pop();
        }

        public virtual void CharacterData(TpXmlReader reader, string data)
        {
            data = data.Trim();

            if (_namespace != null && _namespace.Length > 0 && data.Length > 0)
            {
                int depth = _InTags.Count;
                string tag = _InTags[depth-1].ToString();

                Hashtable sett = (Hashtable)_settings[_namespace];
                if (tag == "TAPIRResource")
                {
                    sett["res"] = data;
                }
                else if (tag == "TAPIROperation")
                {
                    sett["op"] = data;
                }
                else if (tag == "TAPIRTemplate")
                {
                    sett["tmpl"] = data;
                }
            }
        }

        public string GetTemplateUrl()
        {
            if (_namespace == null)
            {
                _error = "LSID has no namespace defined.";
                return "";
            }

            if (_settings[_namespace] == null)
            {
                _error = "Unknown LSID namespace : " + _namespace;
                return "";
            }

            TpResources resources = new TpResources();
            resources.Load();

            Hashtable sett = (Hashtable)_settings[_namespace];
            string resCode = sett["res"].ToString();

            TpResource resource = resources.GetResource(resCode, false);

            if (resource == null)
            {
                _error = "Unknown TAPIR resource : " + resCode;
                return "";
            }

            string op = sett["op"].ToString();
            string tmpl = sett["tmpl"].ToString();

            string accessPoint = resource.GetAccesspoint();

            string url = accessPoint + "?op=" + op + "&amp;envelope=0&amp;t=" + tmpl;
            return url;
        }

        public string GetError()
        {
            return _error;
        }
    }
}
