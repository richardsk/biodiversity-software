using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net;

namespace TapirDotNET
{
    class CnsSchemaHandler_v2 : TpConceptualSchemaHandler
    {
        public String mDesiredSchemaAlias;
        public bool mAddConcepts = false; // flag to indicate if concepts should be added
        // (if user specified a schema alias, only concepts from that
        // schema should be added).
        public bool mConceptHasAlias = false; // flag to indicate if concept has an alias
        public TpConceptualSchema mrConceptualSchema;
        public String mCurrentSchemaNamespace;
        public String mCurrentSchemaAlias;
        public String mCurrentSchemaLocation;
        public String mCurrentSchemaLabel;
        public TpConcept mCurrentConcept;
        public bool mInterrupt = false;
        public Utility.OrderedMap mInTags = new Utility.OrderedMap();
        public String mPath;

        public CnsSchemaHandler_v2()
        {
        }

        public override bool Load(TpConceptualSchema conceptualSchema)
        {
            this.mrConceptualSchema = conceptualSchema;

            String file = conceptualSchema.GetSource();

            // If location follows http://host/path/file#somealias
            // then load only the concepts from the schema with alias "somealias",
            // otherwise load the first schema in the file.
            string[] parts = file.Split('#');

            if (parts.Length == 2 && parts[1].Length > 0)
            {
                mDesiredSchemaAlias = parts[1];
                file = parts[0];
            }

            try
            {
                string xml = "";

                //if file is a url then get file first
                if (TpUtils.IsUrl(file))
                {
                    WebRequest r = FileWebRequest.Create(file);
                    if (TpConfigManager.TP_WEB_PROXY.Length > 0)
                    {
                        r.Proxy = new WebProxy(TpConfigManager.TP_WEB_PROXY);
                    }

                    WebResponse resp = r.GetResponse();
                    System.IO.StreamReader srdr = new System.IO.StreamReader(resp.GetResponseStream());
                    xml = srdr.ReadToEnd();
                    srdr.Close();
                }
                else
                {
                    System.IO.StreamReader srdr = new System.IO.StreamReader(file);
                    xml = srdr.ReadToEnd();
                    srdr.Close();
                }

                TpXmlReader rdr = new TpXmlReader();
                rdr.StartElementHandler = new StartElement(this.StartElement);
                rdr.EndElementHandler = new EndElement(this.EndElement);
                rdr.CharacterDataHandler = new CharacterData(this.CharacterData);

                try
                {
                    rdr.ReadXmlStr(xml);
                }
                catch (Exception ex)
                {
                    string error = "Could not import content from XML file: " + ex.Message + " : " + ex.StackTrace;
                    new TpDiagnostics().Append(TpConfigManager.CFG_INTERNAL_ERROR, error, TpConfigManager.DIAG_ERROR);
                    return false;
                }
            }
            catch (Exception ex)
            {
                new TpDiagnostics().Append(TpConfigManager.DC_IO_ERROR, ex.Message, TpConfigManager.DIAG_ERROR);
            }

            return true;
        }

        public virtual void StartElement(TpXmlReader reader, Utility.OrderedMap attrs)
        {
            this.mInTags.Push(reader.XmlReader.Name);


            if (mInterrupt) return;

            this.mPath = "";
            foreach (string p in this.mInTags.Values) this.mPath += p + "/";
            mPath = mPath.TrimEnd('/');

            // Schema element
            if (reader.XmlReader.Name == "schema" && attrs["namespace"] != null)
            {
                this.mCurrentSchemaNamespace = attrs["namespace"].ToString();
            }
            // Concepts element
            else if (reader.XmlReader.Name == "concepts")
            {
                if (this.mCurrentSchemaAlias == this.mDesiredSchemaAlias)
                {
                    this.mrConceptualSchema.SetAlias(this.mDesiredSchemaAlias);
                    this.mrConceptualSchema.SetNamespace(this.mCurrentSchemaNamespace);
                    this.mrConceptualSchema.SetLocation(this.mCurrentSchemaLocation);

                    this.mAddConcepts = true;
                }
                else if ((this.mDesiredSchemaAlias == null || this.mDesiredSchemaAlias == "") && this.mAddConcepts == false)
                {
                    this.mrConceptualSchema.SetAlias(this.mCurrentSchemaAlias);
                    this.mrConceptualSchema.SetNamespace(this.mCurrentSchemaNamespace);
                    this.mrConceptualSchema.SetLocation(this.mCurrentSchemaLocation);

                    this.mAddConcepts = true;
                }
                else
                {
                    // Ignore start and character events until the next </schema> 
                    //TODO ?? OK ??
                    //xml_set_element_handler( $parser, null, '_EndElement' );
                    //xml_set_character_data_handler( $parser, null );
                    reader.CharacterDataHandler = null;
                    reader.StartElementHandler = null;
                }
            }
            // Concept element
            else if (reader.XmlReader.Name == "concept")
            {
                this.mConceptHasAlias = false;

                if (this.mAddConcepts && attrs["id"] != null)
                {
                    this.mCurrentConcept = new TpConcept();
                    this.mCurrentConcept.SetRequired(false);
                    this.mCurrentConcept.SetId(attrs["id"].ToString());

                    if (attrs["required"] != null &&
                         (attrs["required"] == "1" ||
                           attrs["required"].ToString().ToLower() == "true"))
                    {
                        this.mCurrentConcept.SetRequired(true);
                    }
                }
                else
                {
                    this.mCurrentConcept = null;
                }
            }

        } // end of member function _StartElement

        public virtual void EndElement(TpXmlReader reader)
        {
            // Concept element
            if (reader.XmlReader.Name == "concept")
            {
                if (this.mAddConcepts &&
                     this.mCurrentConcept != null &&
                     this.mConceptHasAlias)
                {
                    this.mrConceptualSchema.AddConcept(this.mCurrentConcept);

                    this.mCurrentConcept = null;
                }
            }
            // Schema element
            else if (reader.XmlReader.Name == "schema")
            {
                if (this.mCurrentSchemaAlias == this.mDesiredSchemaAlias ||
                     this.mDesiredSchemaAlias == null || this.mDesiredSchemaAlias == "")
                {
                    // Interrupt all parsing
                    mInterrupt = true;
                    reader.CharacterDataHandler = null;
                    reader.EndElementHandler = null;
                    reader.StartElementHandler = null;
                }
                else
                {
                    // Restart character and start events
                    //TODO - OK ?
                    //xml_set_element_handler( $parser, '_StartElement', '_EndElement' );
                    //xml_set_character_data_handler( $parser, '_CharacterData' );

                    reader.StartElementHandler = new StartElement(this.StartElement);
                    reader.EndElementHandler = new EndElement(this.EndElement);
                    reader.CharacterDataHandler = new CharacterData(this.CharacterData);

                    this.mInTags = new Utility.OrderedMap();
                    this.mInTags.Push("cns", "schema");
                }
            }

            // When character and start element handlers are deactivated, the
            // following lines will assign wrong values to the corresponding properties.
            // This should not be a problem because only the character handler depends
            // on them and it will get fixed as soon as the handlers are reactivated.
            // (see mInTags assignment in the previoud lines)
            this.mInTags.Pop();

            this.mPath = "";
            foreach (string p in this.mInTags.Values) this.mPath += p + "/";
            mPath = mPath.TrimEnd('/');

        } // end of member function _EndElement

        public virtual void CharacterData(TpXmlReader reader, string data)
        {
            data = data.Trim();

            if (data.Length == 0)
            {
                return;
            }

            // Schema alias
            if (this.mPath == "cns/schema/alias")
            {
                this.mCurrentSchemaAlias = data;
            }
            // Schema location
            else if (this.mPath == "cns/schema/location")
            {
                this.mCurrentSchemaLocation = data;
            }
            // Schema label
            else if (this.mPath == "cns/schema/label")
            {
                this.mCurrentSchemaLabel = data;
            }
            // Concept alias
            else if (this.mPath == "cns/schema/concepts/concept/alias")
            {
                if (this.mCurrentConcept != null)
                {
                    this.mConceptHasAlias = true;

                    this.mCurrentConcept.SetName(data);
                }
            }
            // Concept datatype
            else if (this.mPath == "cns/schema/concepts/concept/datatype")
            {
                if (this.mCurrentConcept != null)
                {
                    this.mCurrentConcept.SetType(data);
                }
            }
            // Concept documentation
            else if (this.mPath == "cns/schema/concepts/concept/doc")
            {
                if (this.mCurrentConcept != null)
                {
                    this.mCurrentConcept.SetDocumentation(data);
                }
            }

        } // end of member function _CharacterData

    } // end of CnsSchemaHandler_v2

}

