using System;
using System.IO;

using LSIDFramework;
using LSIDClient;

namespace LSIDAuthoritySample
{
	/// <summary>
	/// This class is the main implementation class for the Sampl LSID authority.
	/// The class derives from SimpleResolutionService which implements all the required interfaces for 
	/// handling LSID requests.
	/// 
	/// All that needs to be done in this class is to return the data and metadata in the appropriate methods,
	/// everything else is handled by the base classes and the LSID framework.
	/// </summary>
	public class SampleAuthority : SimpleResolutionService
	{
		public SampleAuthority()
		{
		}
	
		public override MetadataResponse doGetMetadata(LSIDRequestContext ctx, string[] acceptedFormats)
		{
			String anotherId = "urn:lsid:lsidsample.org:sample:" + new Random().Next(9999).ToString();

			MemoryStream ms = new MemoryStream();
			StreamWriter wr = new StreamWriter(ms);
			String rdf = "<rdf:RDF xmlns:dc=\"http://purl.org/dc/elements/1.1/\" ";
			rdf += "xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">";
			rdf += "<rdf:Description rdf:about=\"" + ctx.Lsid.ToString() + "\">";
			rdf += "<dc:Title>Sample metadata for LSID " + ctx.Lsid.ToString() + "</dc:Title>";
			rdf += "<rdf:literal rdf:resource=\"" + anotherId + "\">Another Id</rdf:literal></rdf:Description>";
			rdf += "</rdf:RDF>";
			wr.WriteLine(rdf);
			wr.Flush();
			ms.Position = 0;
			MetadataResponse resp = new MetadataResponse(new BufferedStream(ms), DateTime.Now.AddDays(30), MetadataResponse.RDF_FORMAT);			
			return resp;
		}
		
		public override System.IO.Stream getData(LSIDRequestContext req)
		{
			MemoryStream ms = new MemoryStream();
			StreamWriter wr = new StreamWriter(ms);
			wr.WriteLine("Sample data for LSID " + req.Lsid.ToString() );
			wr.Flush();
			ms.Position = 0;

			return new BufferedStream(ms);
		}
		
		public override Stream getDataByRange(LSIDRequestContext req, int start, int length)
		{
			MemoryStream ms = new MemoryStream();
			StreamWriter wr = new StreamWriter(ms);
			wr.WriteLine("Sample data for LSID " + req.Lsid.ToString() + ", from " + start.ToString() + " to " + (start + length-1).ToString() );
			wr.Flush();
			ms.Position = 0;

			return new BufferedStream(ms);
		}

		protected override string getServiceName()
		{
			return "SampleAuthority";
		}
	
		protected override bool hasData(LSIDRequestContext req)
		{
			return true;
		}
	
		protected override bool hasMetadata(LSIDRequestContext req)
		{
			return true;
		}
	
		protected override void validate(LSIDRequestContext req)
		{
			//all is ok

		}
		
	}
}
