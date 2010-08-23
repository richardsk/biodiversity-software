Imports System.Net
Imports System.Xml
Imports System.Configuration
Imports System.IO

Public Class Utility

    Public Shared Function GetWebDocument(ByVal url As String) As String
        Dim req As HttpWebRequest = WebRequest.Create(url)

        Dim proxy As String = ConfigurationManager.AppSettings.Get("WebProxy")
        If proxy.Length > 0 Then
            req.Proxy = New WebProxy(proxy)
        End If

        Dim resp As WebResponse = req.GetResponse()

        Dim rdr As New StreamReader(resp.GetResponseStream())
        Dim xml As String = rdr.ReadToEnd()
        rdr.Close()

        Return xml
    End Function

    Public Shared Function GetWebXmlDocument(ByVal url As String) As Xml.XmlDocument
        Dim doc As XmlDocument

        Dim xml As String = ""

        'try cache
        Dim file As String = ConfigurationManager.AppSettings.Get("HomeDir") + "\Cache\" + Path.GetFileName(url)
        If IO.File.Exists(file) Then
            xml = IO.File.ReadAllText(file)
        Else
            xml = GetWebDocument(url)

            'write to cache
            IO.File.WriteAllText(file, xml)
        End If

        doc = New XmlDocument
        doc.LoadXml(xml)

        Return doc
    End Function

    Public Shared Function GetXmlDocumentFromRDF(ByVal ms As SemWeb.MemoryStore) As XmlDocument
        Dim doc As New XmlDocument
        Dim memStr As New MemoryStream
        Dim wr As New SemWeb.RdfXmlWriter(New StreamWriter(memStr))
        ms.Write(wr)

        memStr.Position = 0
        Dim rdr As New StreamReader(memStr)
        doc.LoadXml(rdr.ReadToEnd())

        Return doc
    End Function

    Public Shared Function IsLSID(ByVal uri As String) As Boolean
        Return (uri.ToLower.StartsWith("urn:lsid"))
    End Function
End Class
