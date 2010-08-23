
Namespace Query

    Interface RDFQuerier

        Function DoQuery(ByVal resource As QueryResource, ByVal terms As List(Of RDFQueryTerm)) As Xml.XmlDocument

        Function ResolveId(ByVal resource As QueryResource, ByVal Id As String) As Xml.XmlDocument

        Function ListIds(ByVal resource As QueryResource) As List(Of String)

        Function GetResourceProtocol() As ResourceProtocol

        'todo - do we need this?
        Function GetSchema() As Xml.XmlDocument

    End Interface

End Namespace
