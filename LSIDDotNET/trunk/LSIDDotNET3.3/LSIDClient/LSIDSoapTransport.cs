using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Text;
using System.Net;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Messaging;
using Microsoft.Web.Services2.Addressing;
using Microsoft.Web.Services2.Attachments;

namespace LSIDClient
{
	/**
	 * Class to implement multipart mime messages for SOAP calls
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class LSIDInputChannel : Microsoft.Web.Services2.Messaging.SoapInputChannel
	{
		LSIDSoapTransport _transport = null;

		//mime STUFF
		string MIMEBoundary = "";
		string startContentID = "";
		string envelope = "";
		SoapEnvelope soapEnvelope = new SoapEnvelope();
		Microsoft.Web.Services2.Attachments.AttachmentCollection attachments = new Microsoft.Web.Services2.Attachments.AttachmentCollection();

		internal LSIDInputChannel(EndpointReference endpoint, LSIDSoapTransport transport ) : base(endpoint)
		{
			_transport = transport;
		}


		//MIME stuff
		public bool isMultiPartMIME(string contentType)
		{
			bool results = false;
			String[] contentTypeItems = contentType.Split(new Char[]{';'});
			foreach (String item in contentTypeItems)
			{
				String trimItem = item.Trim();
				if (trimItem.ToUpper() == "MULTIPART/RELATED")
				{
					results = true;
				} 
				if (trimItem.IndexOf("boundary=") >= 0) //MIMEBoundary member variable
				{
					MIMEBoundary = 
						trimItem.Substring(trimItem.IndexOf("boundary=") + 9).Trim().Trim('"');
				}
				if (trimItem.IndexOf("start=") >= 0)   //startContentID member variable
				{
					startContentID = ("Content-ID: " + 
						trimItem.Substring(trimItem.IndexOf("start=") + 6).Trim('"')).Trim();
				}
			}
			return results;
		}

		public SoapEnvelope ConvertSoapEnvelope(Stream soapStream)
		{
			soapEnvelope = new SoapEnvelope();
			//newStream = new MemoryStream();
			//extract the SOAP envelope from the MIME Message
			envelope = getSOAPEnvelope(soapStream);
			//extract the encoded content from the MIME message and
			// insert the encoded content into the SOAP message
			resolveContentRefs(soapStream, envelope); 
			//replace the original (MIME) stream with the SOAP message
			//replaceMessageStream(newStream, envelope);
			
			soapEnvelope.LoadXml(envelope);


			return soapEnvelope;
		}

		public string getSOAPEnvelope(Stream input)
		{
			string envelope = "";
			try
			{
				//may not be supported by stream
				input.Position = 0;
			}
			catch(Exception ){}

			StreamReader sr = new StreamReader(input, System.Text.Encoding.Default, true);

			// Read through the (non null) lines of the incoming message
			// looking for the ContentID header equal to the start content id
			// the start content id identifies the soap envelope content item
			// NOTE - this logic assumes that the Content-ID header occurs right
			// before the encoded content.
			for (String Line = sr.ReadLine(); Line != null; Line = sr.ReadLine()) 
			{
				if (System.Text.RegularExpressions.Regex.Match(Line, startContentID, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success) //found the SOAP envelope
				{
					bool foundEndOfEnvelope = false;
					String envLine = sr.ReadLine();
					while (envLine != null && !foundEndOfEnvelope)
					{
						envelope = envelope + envLine;
			  
						System.Text.RegularExpressions.Match m = 
							System.Text.RegularExpressions.Regex.Match(envLine, "</\\S+:Envelope>");
						if ( m.Success ) //envLine.IndexOf("</soap:Envelope>") >= 0 || envLine.IndexOf("/soapenv>") >= 0)
						{
							foundEndOfEnvelope = true;
						}
						envLine = sr.ReadLine();
					}
				}
			}
			try
			{
				input.Position = 0;
			}
			catch(Exception){}

			return envelope;
		}

		public void resolveContentRefs(Stream MIMEstream, string envelope)
		{
			string contID;
			string content;

			MIMEstream.Position = 0;
			StreamReader sr = 
				new StreamReader(MIMEstream, System.Text.Encoding.Default, true);

			// Read through the request stream, looking for the Content-ID headers
			// ignore the content of the root item (the soap envelope content)
			for (String Line = sr.ReadLine(); (Line != null) ; Line = sr.ReadLine()) 
			{
				// process every Content-ID *other* than the start Content-ID
				System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(Line, "Content-ID: ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				if (m.Success &&  //Line.IndexOf("Content-ID: ") >= 0) && 
					!(System.Text.RegularExpressions.Regex.Match(Line, startContentID, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)) //Line.IndexOf(startContentID) < 0))
				{
					//save the Content-ID value, and the base64 encoded content
					contID = Line.Substring(m.Index + 12).Trim();  //Line.IndexOf("Content-ID: ") + 12).Trim();
					//remove the starting and ending "<" and ">"
					if (contID.StartsWith("<")) contID = contID.Substring(1);
					if (contID.EndsWith(">")) contID = contID.Substring(0,contID.Length - 1);
					//get the encoded content immediately following the Content-ID header
					//(until the next MIME Boundary is reached)
					content = "";
					Line = sr.ReadLine();
					while ((Line != null) && !(System.Text.RegularExpressions.Regex.Match(Line, MIMEBoundary, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success )) //Line.IndexOf(MIMEBoundary) < 0))
					{
						content = content + Line;
						Line = sr.ReadLine();
					}
					insertContent(contID, content, ref envelope);
				}
			}
			// now that we've processed all the MIME 'attachments', look through
			// the envelope and make sure there are no remaining unresolved href's
			if (envelope.IndexOf("href=\"cid:")>=0)
			{
				throw new Exception("Envelope contains unresolved content references");
			} 
			MIMEstream.Position = 0;  //reset the stream position
		}

		private void insertContent(string contentID, string content, ref String envelope)
		{
			string newSOAPEnvelope;
			// find the start of the href, and the length of the reference attribute
			int refStart = envelope.IndexOf("href=\"cid:" + contentID);
			int refLength = contentID.Length + 11; // length includes close quote
			// if a corresponding reference was found, replace it with the content
			if (refStart >=0)
			{ 
				// remove the href attribute (so we know we've resolved the reference)
				newSOAPEnvelope = envelope.Substring(0,refStart) + 
					envelope.Substring(refStart + refLength);
				// insert data after the element closure (">") that contained the href
				int dataStart = newSOAPEnvelope.IndexOf(">",refStart) + 1;
				envelope = newSOAPEnvelope.Substring(0,dataStart) + content +
					newSOAPEnvelope.Substring(dataStart);
			}
			else
			{
				//add as soap attachment
				MemoryStream attStr = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(content));
				attachments.Add(new Microsoft.Web.Services2.Attachments.Attachment(contentID, HTTPConstants.XML_CONTENT, attStr));
			}
		}

		public void replaceMessageStream(Stream target, String envelope)
		{
			target.Position = 0;
			TextWriter writer = new StreamWriter(target);
			// write the SOAPenvelope member variable contents to the target stream
			writer.WriteLine(envelope);
			writer.Flush();
			target.Flush();
			// tidy up the target stream
			target.SetLength(target.Position);
			target.Position = 0;
		}


		public override SoapChannelCapabilities Capabilities
		{
			get {    return SoapChannelCapabilities.None; }
		}

		public override IAsyncResult BeginReceive(AsyncCallback callback, object state)
		{
			base.Enqueue(Receive());
			
			//return null;
			return base.BeginReceive (callback, state);
		}

		public override void Enqueue(Microsoft.Web.Services2.SoapEnvelope message)
		{
			base.Enqueue (message);			
		}
		
		public override SoapEnvelope EndReceive(IAsyncResult result)
		{
			return base.EndReceive (result);
		}

		public override Microsoft.Web.Services2.SoapEnvelope Receive()
		{			
			SoapEnvelope respEnv = null;
			if (_transport != null && _transport.Response != null)
			{	
				try
				{
					respEnv = new SoapEnvelope();
			
					if (isMultiPartMIME(_transport.Response.ContentType) )
					{
						StreamReader rdr = new StreamReader(_transport.Response.GetResponseStream());
						MemoryStream inpStream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(rdr.ReadToEnd()));
						respEnv = ConvertSoapEnvelope(inpStream);

						if (attachments.Count > 0)
							_transport.Attachments = attachments;

					}
					else
					{
						StreamReader rdr = new StreamReader(_transport.Response.GetResponseStream());
						string env = rdr.ReadToEnd();
						StringReader sr = new StringReader(env);
						respEnv.Load(sr);
					}			
				}
				catch(Exception e)
				{
					throw new LSIDException(e, LSIDException.SERVER_CONFIGURATION_ERROR, "Error getting SOAP envelope");
				}
			}
			return respEnv;
			//return base.Receive ();
		}

	}

	public class LSIDOutputChannel : SoapOutputChannel
	{
		EndpointReference _endPoint = null;
		LSIDSoapTransport _transport = null;

		internal LSIDOutputChannel(EndpointReference endpoint, LSIDSoapTransport transport) : base(endpoint)
		{
			_endPoint = endpoint;
			_transport = transport;
		}

		public override SoapChannelCapabilities Capabilities
		{
			get
			{
				return SoapChannelCapabilities.None;
			}
		}

		public override void EndSend(IAsyncResult result)
		{						
			base.EndSend (result);			
		}


		public override void Send(Microsoft.Web.Services2.SoapEnvelope message)
		{
			System.Net.WebRequest request = System.Net.HttpWebRequest.Create(_endPoint.Address.Value); // Destination.Address.Value);
			
			if (HTTPUtils.WebProxy != null) request.Proxy = new WebProxy(HTTPUtils.WebProxy);

			request.Method = "POST";
			request.ContentType = HTTPConstants.XML_CONTENT;
			//add soap action to headers
			request.Headers.Add("SOAPAction", message.Context.Addressing.Action.Value);
			//remove action from body
			message.Context.Addressing.Action = new Microsoft.Web.Services2.Addressing.Action("");
			//add lsid credentials?, basic security
			if (SOAPUtils.LSIDTransport.Credentials != null)
			{
				String password = (String)SOAPUtils.LSIDTransport.Credentials.getProperty(LSIDCredentials.BASICUSERNAME);
				if (password != null) 
				{
					password += ":" + SOAPUtils.LSIDTransport.Credentials.getProperty(LSIDCredentials.BASICPASSWORD);
					
					String encodedPassword = System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(password));
					request.Headers.Add(HTTPConstants.HEADER_AUTHORIZATION, "Basic " + encodedPassword);
				}
			}
			request.ContentLength = message.InnerXml.Length;
			StreamWriter sw = new StreamWriter(request.GetRequestStream());
			sw.Write(message.InnerXml);
			sw.Close();

			_transport.Context = message.Context;
			_transport.Response = request.GetResponse();

		}

	}

	public class LSIDSoapTransport : Microsoft.Web.Services2.Messaging.SoapTransport, Microsoft.Web.Services2.Messaging.ISoapTransport
	{
		public WebResponse Response = null;
		public SoapContext Context = null;
		public AttachmentCollection Attachments = null;
		public LSIDCredentials Credentials = null;
		
		public LSIDSoapTransport()
		{	
		}
		
		public void Clear()
		{
			Response = null;
			Context = null;
			Attachments = null;
		}

		public ISoapOutputChannel GetOutputChannel(EndpointReference endpoint, 
			SoapChannelCapabilities capabilities)
		{			
			return new LSIDOutputChannel(endpoint, this);
		}

		public ISoapInputChannel GetInputChannel(EndpointReference endpoint, 
			SoapChannelCapabilities capabilities)
		{
			return new LSIDInputChannel(endpoint, this);
		}
	}

	public class LSIDWebServiceClient : //Microsoft.Web.Services2.Messaging.SoapSender
		Microsoft.Web.Services2.Messaging.SoapClient
	{
		public LSIDWebServiceClient() : base()
		{
		}

		[Microsoft.Web.Services2.Messaging.SoapMethod("RequestResponseMethod")] 			
		public Microsoft.Web.Services2.SoapEnvelope SendMessage(String method, Microsoft.Web.Services2.SoapEnvelope requestEnvelope)
		{	
			SOAPUtils.LSIDTransport.Clear();

			requestEnvelope.Context.Addressing.Action = new Microsoft.Web.Services2.Addressing.Action(method);
			
			SoapEnvelope respEnv = base.SendRequestResponse(method, requestEnvelope);

			if (SOAPUtils.LSIDTransport.Attachments != null)
				respEnv.Context.Attachments.AddRange(SOAPUtils.LSIDTransport.Attachments);

			if (SOAPUtils.LSIDTransport.Response.Headers["LSID-Error-Code"] != null)
			{
				throw new LSIDException("LSID Error : " + respEnv.Body.InnerText);
			}

			return respEnv;
		}

	}
}
