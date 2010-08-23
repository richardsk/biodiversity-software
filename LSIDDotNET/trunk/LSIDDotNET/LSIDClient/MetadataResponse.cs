using System;

namespace LSIDClient
{
	/**
	 * Simple class to hold the return value of a call to getMetadata
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class MetadataResponse : ExpiringResponse
    {
	
		 public static readonly String XML_FORMAT      = "application/xml";
         public static readonly String RDF_FORMAT      = "application/xml+rdf";
         public static readonly String XMI_FORMAT      = "application/xml+xmi";
         public static readonly String N3_FORMAT       = "application/n3";
         public static readonly String NTRIPLES_FORMAT = "application/ntriples";
         public static readonly String SVG_FORMAT      = "image/svg+xml";
         public static readonly String NO_FORMAT       = "application/octet-stream";
	
         private String format;
	
         /**
          * Construct a new response
          * @param Object the value of the response
          * @param Date the expiration date/time of the response
          * @param String the format of the metadata
          */
         public MetadataResponse(Object value, DateTime expires, String format) :
             base(value, expires)
         {
             this.format = format;
         }
	
         /**
          * Construct a new response
          * @param Object the value of the response
          * @param long the expiration date/time in millis of the response
          * @param String the format of the metadata
          */
         public MetadataResponse(Object value, long expires, String format) :
             base(value, expires)
         {
             this.format = format;
         }

         /**
          * Construct a new response
          * @param Object the value of the response
          * @param Date the expiration date/time of the response
          */
         public MetadataResponse(Object value, DateTime expires) :
             base(value, expires)
         {
         }

         /**
          * Construct a new response
          * @param Object the value of the response
          * @param long the expiration date/time in millis of the response
          */
         public MetadataResponse(Object value, long expires) :
             base(value, expires)
         {
         }
	
         /**
          * get the format
          * @return String the format of the metadata
          */
         public String getFormat() 
         {
             return format;
         }
	
         /**
          * get the metdata, same as calling  (InputStream)getValue()
          * @return InputStream the metadata, this stream MUST be closed by the user.
          */
         public System.IO.Stream getMetadata() 
         {
             return (System.IO.Stream)getValue();
         }

	}
}
