Imports System.Xml
Imports LSIDClient

Namespace Query

    Public Class LSIDQuerier
        Implements RDFQuerier

        Public Function DoQuery(ByVal resource As QueryResource, ByVal terms As System.Collections.Generic.List(Of RDFQueryTerm)) As System.Xml.XmlDocument Implements RDFQuerier.DoQuery
            'cannot search LSIDs -> blank doc
            Return New XmlDocument
        End Function

        Public Function GetSchema() As System.Xml.XmlDocument Implements RDFQuerier.GetSchema
            'todo - do we need this?
            Return New XmlDocument
        End Function

        Public Function ListIds(ByVal resource As QueryResource) As System.Collections.Generic.List(Of String) Implements RDFQuerier.ListIds
            'cannot harvest LSIDs -> empty list
            Return New List(Of String)
        End Function

        Public Function ResolveId(ByVal resource As QueryResource, ByVal Id As String) As System.Xml.XmlDocument Implements RDFQuerier.ResolveId
            'ignore query type - must be RDF
            Dim cl As New LSIDClient.LSIDResolver(New LSID(Id))
            Dim mr As MetadataResponse = cl.getMetadata()
            Dim rdr As New IO.StreamReader(mr.getMetadata())

            Dim xml As String = rdr.ReadToEnd()

            Dim doc As New XmlDocument
            doc.LoadXml(xml)

            Return doc
        End Function

        Public Function GetResourceProtocol() As ResourceProtocol Implements RDFQuerier.GetResourceProtocol
            Return ResourceProtocol.LSID
        End Function

    End Class
End Namespace
