Imports System.Xml
Imports System.Net
Imports System.Web

Namespace Query
    Public Class OAI_PMHQuerier
        Implements RDFQuerier

        Public Function DoQuery(ByVal resource As QueryResource, ByVal terms As System.Collections.Generic.List(Of RDFQueryTerm)) As System.Xml.XmlDocument Implements RDFQuerier.DoQuery
            Return New XmlDocument
        End Function

        Public Function GetSchema() As System.Xml.XmlDocument Implements RDFQuerier.GetSchema
            Return New XmlDocument
        End Function

        Public Function ListIds(ByVal resource As QueryResource) As System.Collections.Generic.List(Of String) Implements RDFQuerier.ListIds
            Dim ids As New List(Of String)

            Dim qu As String = resource.ResourceUrl + "?" + GetOAIPMHQuery("ListIdentifiers", "")

            Dim doc As XmlDocument = Utility.GetWebXmlDocument(qu)

            Dim nsMgr As New XmlNamespaceManager(doc.NameTable)
            nsMgr.AddNamespace("oai", "http://www.openarchives.org/OAI/2.0/")
            Dim nodes As XmlNodeList = doc.SelectNodes("/oai:OAI-PMH/oai:ListIdentifiers/oai:header/oai:identifier", nsMgr)
            For Each n As XmlNode In nodes
                ids.Add(n.InnerText)
            Next

            Return ids
        End Function

        Public Function ResolveId(ByVal resource As QueryResource, ByVal Id As String) As System.Xml.XmlDocument Implements RDFQuerier.ResolveId
            Dim qu As String = resource.ResourceUrl + "?" + GetOAIPMHQuery("GetRecord", Id)

            Return Utility.GetWebXmlDocument(qu)
        End Function

        Public Function GetResourceProtocol() As ResourceProtocol Implements RDFQuerier.GetResourceProtocol
            Return ResourceProtocol.OAI_PMH
        End Function

#Region "OAI-PMH functions"
        Private Function GetOAIPMHQuery(ByVal op As String, ByVal id As String) As String
            If op = "ListIdentifiers" Then
                Return "verb=ListIdentifiers"
            ElseIf op = "GetRecord" Then
                Return "verb=GetRecord&identifier=" + id
            End If

            Return ""
        End Function
#End Region

    End Class

End Namespace
