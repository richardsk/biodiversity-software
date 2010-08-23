Imports System.Net

Public Class RDFValidator

    Public Shared ValidatorServiceUrl As String = "http://www.w3.org/RDF/Validator/ARPServlet"

    Public Shared Function GetValidatorGraphHtml(ByVal rdfXml As String, ByVal embed As Boolean) As String
        Dim req As HttpWebRequest = WebRequest.Create(ValidatorServiceUrl)

        Dim encRdf As String = Web.HttpUtility.UrlEncode(rdfXml)

        Dim body As String = "RDF=" + encRdf + "%0D%0A++&PARSE=Parse+RDF&TRIPLES_AND_GRAPH=PRINT_GRAPH"
        If embed Then
            body += "&FORMAT=PNG_EMBED"
        Else
            body += "&FORMAT=PNG_LINK"
        End If

        Dim b As Byte() = System.Text.UTF8Encoding.UTF8.GetBytes(body)
        req.ContentLength = b.Length
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"

        Dim str As New IO.StreamWriter(req.GetRequestStream())
        str.Write(body)
        str.Close()

        Dim resp As WebResponse = req.GetResponse()
        Dim rdr As New IO.StreamReader(resp.GetResponseStream())

        Dim result As String = rdr.ReadToEnd()
        rdr.Close()

        Return result
    End Function

    Public Shared Function GetValidatorGraphImage(ByVal rdfXml As String) As Drawing.Image
        Dim html As String = GetValidatorGraphHtml(rdfXml, False)

        Dim res As Drawing.Image = Nothing

        'get image link out of it
        Dim pos As Integer = html.IndexOf("Graph of the data model")
        If pos <> -1 Then
            pos = html.IndexOf("href=", pos)
            Dim endPos As Integer = html.IndexOf(""">", pos)
            Dim url As String = html.Substring(pos + 6, endPos - pos - 6)

            Dim wr As WebRequest = WebRequest.Create("http://www.w3.org/RDF/Validator/" + url)
            res = Drawing.Image.FromStream(wr.GetResponse().GetResponseStream())

        End If

        Return res
    End Function
End Class
