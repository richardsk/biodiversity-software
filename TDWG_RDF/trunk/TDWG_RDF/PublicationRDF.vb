Imports SemWeb

Public Class PublicationRDF

    
    Public Const nsTCS As String = "http://rs.tdwg.org/ontology/voc/TaxonName#"
    Public Const nsDC As String = "http://purl.org/dc/elements/1.1/"
    Public Const nsDCTerm As String = "http://purl.org/dc/terms/"
    Public Const nsTCom As String = "http://rs.tdwg.org/ontology/voc/Common#"
    Public Const nsPub As String = "http://rs.tdwg.org/ontology/voc/PublicationCitation#"
    Public Const nsOwl As String = "http://www.w3.org/2002/07/owl#"
    Public Const nsRDF As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    Public Const nsType As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type"

    Public Function GetPublicationMemoryStore(ByVal publicationItem As PublicationCitation, ByRef refEntity As Resource, ByVal topLevel As Boolean) As SemWeb.MemoryStore
        Dim ms As New MemoryStore

        If topLevel Then
            refEntity = New Entity(publicationItem.Id)
        Else
            refEntity = New BNode(publicationItem.Id)
        End If

        If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
            ms.Add(New Statement(refEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + publicationItem.Id)))
        End If

        ms.Add(New Statement(refEntity, New Entity(nsType), New Entity(nsPub + "PublicationCitation")))

        If publicationItem.publicationType <> PublicationType.NotSpecififed Then
            Dim pt As New Entity(publicationItem.publicationType.ToString())
            ms.Add(New Statement(pt, New Entity(nsType), New Entity(nsPub + "PublicationTypeTerm")))
            ms.Add(New Statement(refEntity, New Entity(nsPub + "publicationType"), pt))
        End If

        'todo author team, type = actor?

        If publicationItem.parentPublication.Length > 0 Then
            Dim pt As New Entity(publicationItem.parentPublication.ToString())
            ms.Add(New Statement(pt, New Entity(nsType), New Entity(nsPub + "PublicationCitation")))
            ms.Add(New Statement(refEntity, New Entity(nsPub + "parentPublication"), pt))
        End If

        AddPublicationTripleVal(ms, refEntity, nsDC + "identifier", publicationItem.Id)

        AddPublicationTripleVal(ms, refEntity, nsPub + "authorship", publicationItem.authorship)
        AddPublicationTripleVal(ms, refEntity, nsPub + "year", publicationItem.year)
        AddPublicationTripleVal(ms, refEntity, nsPub + "title", publicationItem.title)
        AddPublicationTripleVal(ms, refEntity, nsPub + "parentPublicationString", publicationItem.parentPublicationString)
        AddPublicationTripleVal(ms, refEntity, nsPub + "shortTitle", publicationItem.shortTitle)
        AddPublicationTripleVal(ms, refEntity, nsPub + "alternateTitle", publicationItem.alternateTitle)
        AddPublicationTripleVal(ms, refEntity, nsPub + "publisher", publicationItem.publisher)
        AddPublicationTripleVal(ms, refEntity, nsPub + "placePublished", publicationItem.placePublished)
        AddPublicationTripleVal(ms, refEntity, nsPub + "volume", publicationItem.volume)
        AddPublicationTripleVal(ms, refEntity, nsPub + "numberOfVolumes", publicationItem.numberOfVolumes)
        AddPublicationTripleVal(ms, refEntity, nsPub + "number", publicationItem.number)
        AddPublicationTripleVal(ms, refEntity, nsPub + "pages", publicationItem.pages)
        AddPublicationTripleVal(ms, refEntity, nsPub + "section", publicationItem.section)
        AddPublicationTripleVal(ms, refEntity, nsPub + "edition", publicationItem.edition)
        AddPublicationTripleVal(ms, refEntity, nsPub + "datePublished", publicationItem.datePublished)
        AddPublicationTripleVal(ms, refEntity, nsPub + "startDate", publicationItem.startDate)
        AddPublicationTripleVal(ms, refEntity, nsPub + "endDate", publicationItem.endDate)
        AddPublicationTripleVal(ms, refEntity, nsPub + "isbn", publicationItem.isbn)
        AddPublicationTripleVal(ms, refEntity, nsPub + "issn", publicationItem.issn)
        AddPublicationTripleVal(ms, refEntity, nsPub + "doi", publicationItem.doi)
        AddPublicationTripleVal(ms, refEntity, nsPub + "reprintEdition", publicationItem.reprintEdition)
        AddPublicationTripleVal(ms, refEntity, nsPub + "figures", publicationItem.figures)
        AddPublicationTripleVal(ms, refEntity, nsPub + "url", publicationItem.url)

        Return ms
    End Function

    Private Sub AddPublicationTripleVal(ByVal ms As MemoryStore, ByVal pub As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 Then
            ms.Add(New Statement(pub, New Entity(predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddPublication(ByVal ms As MemoryStore, ByVal name As Entity, ByVal publication As PublicationCitation)
        If Not publication Is Nothing Then
            Dim pub As New BNode(Proxy.LSIDProxy + publication.Id)

            ms.Add(New Statement(pub, New Entity(nsType), New Entity(nsPub + "PublicationCitation")))
            ms.Add(New Statement(name, New Entity(nsTCom + "publishedInCitation"), pub))

            If publication.publicationType <> PublicationType.NotSpecififed Then
                Dim pt As New Entity(publication.publicationType.ToString())
                ms.Add(New Statement(pt, New Entity(nsType), New Entity(nsPub + "PublicationTypeTerm")))
                ms.Add(New Statement(pub, New Entity(nsPub + "publicationType"), pt))
            End If

            'todo author team, type = actor?

            If publication.parentPublication.Length > 0 Then
                Dim pt As New Entity(publication.parentPublication.ToString())
                ms.Add(New Statement(pt, New Entity(nsType), New Entity(nsPub + "PublicationCitation")))
                ms.Add(New Statement(pub, New Entity(nsPub + "parentPublication"), pt))
            End If

            AddPublicationTripleVal(ms, pub, nsPub + "authorship", publication.authorship)
            AddPublicationTripleVal(ms, pub, nsPub + "year", publication.year)
            AddPublicationTripleVal(ms, pub, nsPub + "title", publication.title)
            AddPublicationTripleVal(ms, pub, nsPub + "parentPublicationString", publication.parentPublicationString)
            AddPublicationTripleVal(ms, pub, nsPub + "shortTitle", publication.shortTitle)
            AddPublicationTripleVal(ms, pub, nsPub + "alternateTitle", publication.alternateTitle)
            AddPublicationTripleVal(ms, pub, nsPub + "publisher", publication.publisher)
            AddPublicationTripleVal(ms, pub, nsPub + "placePublished", publication.placePublished)
            AddPublicationTripleVal(ms, pub, nsPub + "volume", publication.volume)
            AddPublicationTripleVal(ms, pub, nsPub + "numberOfVolumes", publication.numberOfVolumes)
            AddPublicationTripleVal(ms, pub, nsPub + "number", publication.number)
            AddPublicationTripleVal(ms, pub, nsPub + "pages", publication.pages)
            AddPublicationTripleVal(ms, pub, nsPub + "section", publication.section)
            AddPublicationTripleVal(ms, pub, nsPub + "edition", publication.edition)
            AddPublicationTripleVal(ms, pub, nsPub + "datePublished", publication.datePublished)
            AddPublicationTripleVal(ms, pub, nsPub + "startDate", publication.startDate)
            AddPublicationTripleVal(ms, pub, nsPub + "endDate", publication.endDate)
            AddPublicationTripleVal(ms, pub, nsPub + "isbn", publication.isbn)
            AddPublicationTripleVal(ms, pub, nsPub + "issn", publication.issn)
            AddPublicationTripleVal(ms, pub, nsPub + "doi", publication.doi)
            AddPublicationTripleVal(ms, pub, nsPub + "reprintEdition", publication.reprintEdition)
            AddPublicationTripleVal(ms, pub, nsPub + "figures", publication.figures)
            AddPublicationTripleVal(ms, pub, nsPub + "url", publication.url)

        End If
    End Sub
End Class
