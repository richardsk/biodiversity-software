Imports SemWeb


Public Class GeneralRDF
    Public Const nsInfo As String = "http://rs.tdwg.org/ontology/voc/InfoItem#"
    Public Const nsConcept As String = "http://rs.tdwg.org/ontology/voc/TaxonConcept#"
    Public Const nsOcc As String = "http://rs.tdwg.org/ontology/voc/TaxonOccurrence#"
    Public Const nsDC As String = "http://purl.org/dc/elements/1.1/"
    Public Const nsDCTerm As String = "http://purl.org/dc/terms/"
    Public Const nsTCom As String = "http://rs.tdwg.org/ontology/voc/Common#"
    Public Const nsPub As String = "http://rs.tdwg.org/ontology/voc/PublicationCitation#"
    Public Const nsOwl As String = "http://www.w3.org/2002/07/owl#"
    Public Const nsRDF As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    Public Const nsType As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type"
    
    Public Function GetInfoMemoryStore(ByVal infoItem As InfoItem, ByRef infoEntity As BNode) As MemoryStore
        Dim ms As New MemoryStore

        infoEntity = New BNode(infoItem.Id)
        ms.Add(New Statement(infoEntity, New Entity(nsType), New Entity(nsInfo + "InfoItem")))

        ms.Add(New Statement(infoEntity, New Entity(nsDC + "title"), New Literal(infoItem.hasContent)))

        If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
            ms.Add(New Statement(infoEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + infoItem.Id)))
        End If

        If Not infoItem.associatedTaxon Is Nothing Then
            AddInfoTripleEntity(ms, infoEntity, "associatedTaxon", Proxy.LSIDProxy + infoItem.associatedTaxon.Id, nsConcept + "TaxonConcept")
        End If

        If Not infoItem.contextOccurrence Is Nothing Then
            AddInfoTripleEntity(ms, infoEntity, "contextOccurrence", Proxy.LSIDProxy + infoItem.contextOccurrence.Id, nsOcc + "TaxonOccurrence")
        End If

        AddInfoTripleVal(ms, infoEntity, "context", infoItem.context)
        AddInfoTripleVal(ms, infoEntity, "hasContent", infoItem.hasContent)

        If Not infoItem.contextValue Is Nothing Then
            AddInfoTripleVal(ms, infoEntity, "contextValue", infoItem.contextValue.definition)
        End If

        If Not infoItem.hasValue Is Nothing Then
            AddInfoTripleVal(ms, infoEntity, "hasValue", infoItem.hasValue.definition)
        End If

        Return ms
    End Function

    Private Sub AddInfoTripleVal(ByVal ms As MemoryStore, ByVal info As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 Then
            ms.Add(New Statement(info, New Entity(nsInfo + predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddInfoTripleEntity(ByVal ms As MemoryStore, ByVal info As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                ms.Add(New Statement(obj, New Entity(nsType), New Entity(entType)))
            End If
            ms.Add(New Statement(info, New Entity(nsInfo + predicate), obj))
        End If
    End Sub
End Class
