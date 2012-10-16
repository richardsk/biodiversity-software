using System;
using System.IO;

using LSIDClient;

namespace LSIDFramework
{
public class SimpleFANStoreMetadataService : LSIDMetadataService 
{

	public MetadataResponse getMetadata(LSIDRequestContext req, String[] acceptedFormats)
    {		
		MemoryStream ms = new MemoryStream();
		StreamWriter wr = new StreamWriter(ms);

		String rdf = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">" + Environment.NewLine;
		rdf += "<rdf:Description rdf:about=\"" + req.Lsid.ToString() + "\">" + Environment.NewLine;
		
		LSIDAuthority[] auths = SimpleFANStore.lookupFAPs(req.Lsid);
		foreach (LSIDAuthority a in auths)
		{
			rdf += "	<urn:lsid:i3c.org:predicates:foreignauthority>" + a.Authority + "</urn:lsid:i3c.org:predicates:foreignauthority>" + Environment.NewLine;
		}
		rdf += "</rdf:Description>";
		rdf += "</rdf:RDF>";

		wr.WriteLine(rdf);
		wr.Flush();

		ms.Position = 0;

		return new MetadataResponse(ms, DateTime.MinValue, MetadataResponse.RDF_FORMAT);

	}

	public void initService(LSIDServiceConfig config)
    {
		// TODO Auto-generated method stub

	}

}

}
