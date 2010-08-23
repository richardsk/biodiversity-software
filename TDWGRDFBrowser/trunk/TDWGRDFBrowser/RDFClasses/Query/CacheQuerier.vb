Imports System.Xml

Namespace Query

    'TODO
    Public Class CacheQuerier
        Implements RDFQuerier

        Public Function DoQuery(ByVal resource As QueryResource, ByVal terms As System.Collections.Generic.List(Of RDFQueryTerm)) As System.Xml.XmlDocument Implements RDFQuerier.DoQuery
            Return New XmlDocument
        End Function

        Public Function GetSchema() As System.Xml.XmlDocument Implements RDFQuerier.GetSchema
            Return New XmlDocument
        End Function

        Public Function ListIds(ByVal resource As QueryResource) As System.Collections.Generic.List(Of String) Implements RDFQuerier.ListIds
            Return New List(Of String)
        End Function

        Public Function ResolveId(ByVal resource As QueryResource, ByVal Id As String) As System.Xml.XmlDocument Implements RDFQuerier.ResolveId
            Return New XmlDocument
        End Function

        Public Function GetResourceProtocol() As ResourceProtocol Implements RDFQuerier.GetResourceProtocol
            Return ResourceProtocol.CACHE
        End Function
    End Class
End Namespace
