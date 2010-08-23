Imports SemWeb

Public Class TaxonOccurrenceEntity
    Inherits Entity

    Public Sub New(ByVal uri As String)
        MyBase.New(uri)
    End Sub
End Class

Public Class SpecimenRDF
    Public Const nsTO As String = "http://rs.tdwg.org/ontology/voc/TaxonOccurrence#"
    Public Const nsPerson As String = "http://rs.tdwg.org/ontology/voc/Person#"
    Public Const nsPub As String = "http://rs.tdwg.org/ontology/voc/PublicationCitation#"
    Public Const nsTCS As String = "http://rs.tdwg.org/ontology/voc/TaxonName#"
    Public Const nsTCom As String = "http://rs.tdwg.org/ontology/voc/Common#"
    Public Const nsInteract As String = "http://rs.tdwg.org/ontology/voc/TaxonOccurrenceInteraction#"
    Public Const nsConcept As String = "http://rs.tdwg.org/ontology/voc/TaxonConcept#"
    Public Const nsDC As String = "http://purl.org/dc/elements/1.1/"
    Public Const nsDCTerm As String = "http://purl.org/dc/terms/"
    Public Const nsOwl As String = "http://www.w3.org/2002/07/owl#"
    Public Const nsRDF As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    Public Const nsType As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type"


    Public Function GetOccurrenceRDF(ByVal taxonOccurrenceItem As TaxonOccurrence, ByVal additionalStatements As MemoryStore) As String
        Dim bn As Resource = Nothing
        Dim ms As MemoryStore = GetOccurrenceMemoryStore(taxonOccurrenceItem, bn, True)

        If additionalStatements IsNot Nothing Then
            For Each st As Statement In additionalStatements.Statements
                Dim s As New Statement(bn, st.Predicate, st.Object)
                If Not ms.Contains(s) Then ms.Add(s)
            Next
        End If

        Dim sw As New System.IO.StringWriter
        Dim xw As New System.Xml.XmlTextWriter(sw)
        xw.Formatting = System.Xml.Formatting.Indented
        xw.Namespaces = True

        Dim wr As New RdfXmlWriter(xw)
        wr.Namespaces.AddNamespace(nsTCS, "tn")
        wr.Namespaces.AddNamespace(nsConcept, "tc")
        wr.Namespaces.AddNamespace(nsTCom, "tcom")
        wr.Namespaces.AddNamespace(nsPub, "tpub")
        wr.Namespaces.AddNamespace(nsDC, "dc")
        wr.Namespaces.AddNamespace(nsDCTerm, "dcterms")
        wr.Namespaces.AddNamespace(nsOwl, "owl")
        wr.Namespaces.AddNamespace(nsRDF, "rdf")
        wr.Namespaces.AddNamespace(nsTO, "tto")
        wr.Namespaces.AddNamespace(nsPerson, "tto")

        ms.Write(wr)
        wr.Close()

        Return sw.ToString
    End Function

    Public Function GetOccurrenceMemoryStore(ByVal TaxonOccurrenceItem As TaxonOccurrence, ByRef occurrenceEntity As Resource, ByVal topLevel As Boolean) As SemWeb.MemoryStore
        Dim ms As New MemoryStore

        If topLevel Then
            occurrenceEntity = New Entity(TaxonOccurrenceItem.Id)
        Else
            occurrenceEntity = New BNode(TaxonOccurrenceItem.Id)
        End If

        ms.Add(New Statement(occurrenceEntity, New Entity(nsType), New Entity(nsTO + "TaxonOccurence")))

        ms.Add(New Statement(occurrenceEntity, New Entity(nsDC + "title"), New Literal(TaxonOccurrenceItem.catalogNumber)))

        If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
            ms.Add(New Statement(occurrenceEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + TaxonOccurrenceItem.Id)))
        End If

        If TaxonOccurrenceItem.basisOfRecord <> BasisOfRecordTerm.NotSpecified Then
            AddOccurrenceTripleEntity(ms, occurrenceEntity, "basisOfRecord", nsTO + TaxonOccurrenceItem.basisOfRecord.ToString(), nsTO + "BasisOfRecordTerm")
        End If

        AddOccurenceTripleVal(ms, occurrenceEntity, "catalogNumber", TaxonOccurrenceItem.catalogNumber)
        AddOccurenceTripleVal(ms, occurrenceEntity, "collector", TaxonOccurrenceItem.collector)
        AddOccurenceTripleVal(ms, occurrenceEntity, "collectorsBatchNumber", TaxonOccurrenceItem.collectorsBatchNumber)
        AddOccurenceTripleVal(ms, occurrenceEntity, "collectorsFieldNumber", TaxonOccurrenceItem.collectorsFieldNumber)
        'todo TaxonOccurrenceItem.collectorTeam
        AddOccurenceTripleVal(ms, occurrenceEntity, "continent", TaxonOccurrenceItem.continent)
        AddOccurenceTripleVal(ms, occurrenceEntity, "coordinateUncertaintyInMeters", TaxonOccurrenceItem.coordinateUncertaintyInMeters.ToString())
        AddOccurenceTripleVal(ms, occurrenceEntity, "country", TaxonOccurrenceItem.country)
        AddOccurenceTripleVal(ms, occurrenceEntity, "county", TaxonOccurrenceItem.county)
        AddOccurenceTripleVal(ms, occurrenceEntity, "dayOfYear", TaxonOccurrenceItem.dayOfYear.ToString())
        AddOccurenceTripleVal(ms, occurrenceEntity, "decimalLatitude", TaxonOccurrenceItem.decimalLatitude.ToString())
        AddOccurenceTripleVal(ms, occurrenceEntity, "decimalLongitude", TaxonOccurrenceItem.decimalLongitude.ToString())

        'derivedFrom
        If Not TaxonOccurrenceItem.derivedFrom Is Nothing Then
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = GetOccurrenceMemoryStore(TaxonOccurrenceItem.derivedFrom, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(occurrenceEntity, New Entity(nsTO + "derivedFrom"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        AddOccurenceTripleVal(ms, occurrenceEntity, "disposition", TaxonOccurrenceItem.disposition)
        AddOccurenceTripleVal(ms, occurrenceEntity, "earliestDateCollected", TaxonOccurrenceItem.earliestDateCollected.ToString(Formats.DateTimeFormat))
        AddOccurenceTripleVal(ms, occurrenceEntity, "fieldNotes", TaxonOccurrenceItem.fieldNotes)
        AddOccurenceTripleVal(ms, occurrenceEntity, "footprintSpatialFit", TaxonOccurrenceItem.footprintSpatialFit.ToString())
        AddOccurenceTripleVal(ms, occurrenceEntity, "geodeticDatum", TaxonOccurrenceItem.geodeticDatum)
        AddOccurenceTripleVal(ms, occurrenceEntity, "georeferenceProtocol", TaxonOccurrenceItem.georeferenceProtocol)
        AddOccurenceTripleVal(ms, occurrenceEntity, "georeferenceRemarks", TaxonOccurrenceItem.georeferenceRemarks)
        AddOccurenceTripleVal(ms, occurrenceEntity, "georeferenceSources", TaxonOccurrenceItem.georeferenceSources)
        AddOccurenceTripleVal(ms, occurrenceEntity, "georeferenceVerificationStatus", TaxonOccurrenceItem.georeferenceVerificationStatus)
        AddOccurenceTripleVal(ms, occurrenceEntity, "higherGeography", TaxonOccurrenceItem.higherGeography)
        'todo TaxonOccurrenceItem.hostCollection
        AddOccurenceTripleVal(ms, occurrenceEntity, "hostCollectionString", TaxonOccurrenceItem.hostCollectionString)

        AddOccurenceTripleVal(ms, occurrenceEntity, "identifiedToString", TaxonOccurrenceItem.identifiedToString)
        AddOccurenceTripleVal(ms, occurrenceEntity, "individualCount", TaxonOccurrenceItem.individualCount)
        AddOccurenceTripleVal(ms, occurrenceEntity, "institutionCode", TaxonOccurrenceItem.institutionCode)
        AddOccurenceTripleVal(ms, occurrenceEntity, "island", TaxonOccurrenceItem.island)
        AddOccurenceTripleVal(ms, occurrenceEntity, "islandGroup", TaxonOccurrenceItem.islandGroup)
        If TaxonOccurrenceItem.lastDateCollected <> DateTime.MinValue Then
            AddOccurenceTripleVal(ms, occurrenceEntity, "lastDateCollected", TaxonOccurrenceItem.lastDateCollected.ToString(Formats.DateTimeFormat))
        End If
        AddOccurenceTripleVal(ms, occurrenceEntity, "lifeStage", TaxonOccurrenceItem.lifeStage)
        AddOccurenceTripleVal(ms, occurrenceEntity, "locality", TaxonOccurrenceItem.locality)
        AddOccurenceTripleVal(ms, occurrenceEntity, "maximumDepthInMeters", TaxonOccurrenceItem.maximumDepthInMeters)
        AddOccurenceTripleVal(ms, occurrenceEntity, "maximumElevationInMeters", TaxonOccurrenceItem.maximumElevationInMeters)
        AddOccurenceTripleVal(ms, occurrenceEntity, "minimumDepthInMeters", TaxonOccurrenceItem.minimumDepthInMeters)
        AddOccurenceTripleVal(ms, occurrenceEntity, "minimumElevationInMeters", TaxonOccurrenceItem.minimumElevationInMeters)
        AddOccurenceTripleVal(ms, occurrenceEntity, "pointRadiusSpatialFit", TaxonOccurrenceItem.pointRadiusSpatialFit.ToString())
        'todo TaxonOccurrenceItem.procedure
        AddOccurenceTripleVal(ms, occurrenceEntity, "procedureDescriptor", TaxonOccurrenceItem.procedureDescriptor)
        AddOccurenceTripleVal(ms, occurrenceEntity, "sex", TaxonOccurrenceItem.sex)
        AddOccurenceTripleVal(ms, occurrenceEntity, "stateProvince", TaxonOccurrenceItem.stateProvince)

        If TaxonOccurrenceItem.typeStatus <> NomenclaturalTypeType.NotSpecified Then
            AddOccurrenceTripleEntity(ms, occurrenceEntity, "typeStatus", nsTCS + TaxonOccurrenceItem.typeStatus.ToString(), nsTCS + "NomenclaturalTypeTypeTerm")
        End If

        AddOccurenceTripleVal(ms, occurrenceEntity, "typeStatusString", TaxonOccurrenceItem.typeStatusString)

        Dim tcsRDf As New TaxonNameRDF

        If Not TaxonOccurrenceItem.typeForName Is Nothing Then
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = tcsRDf.GetNameMemoryStore(TaxonOccurrenceItem.typeForName, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(occurrenceEntity, New Entity(nsTO + "typeForName"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        AddOccurenceTripleVal(ms, occurrenceEntity, "validDistibutionFlag", TaxonOccurrenceItem.validDistibutionFlag.ToString())
        AddOccurenceTripleVal(ms, occurrenceEntity, "value", TaxonOccurrenceItem.value)
        AddOccurenceTripleVal(ms, occurrenceEntity, "valueConfidence", TaxonOccurrenceItem.valueConfidence.ToString())
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimCollectingDate", TaxonOccurrenceItem.verbatimCollectingDate)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimCoordinates", TaxonOccurrenceItem.verbatimCoordinates)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimCoordinateSystem", TaxonOccurrenceItem.verbatimCoordinateSystem)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimDepth", TaxonOccurrenceItem.verbatimDepth)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimElevation", TaxonOccurrenceItem.verbatimElevation)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimLabelText", TaxonOccurrenceItem.verbatimLabelText)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimLatitude", TaxonOccurrenceItem.verbatimLatitude)
        AddOccurenceTripleVal(ms, occurrenceEntity, "verbatimLongitude", TaxonOccurrenceItem.verbatimLongitude)
        AddOccurenceTripleVal(ms, occurrenceEntity, "waterBody", TaxonOccurrenceItem.waterBody)
        AddOccurenceTripleVal(ms, occurrenceEntity, "wktFootprint", TaxonOccurrenceItem.wktFootprint)

        'identifiedTo
        If Not TaxonOccurrenceItem.identifiedTo Is Nothing Then
            Dim bn As BNode = Nothing
            Dim tmpMs As MemoryStore = GetIdentificationMemoryStore(TaxonOccurrenceItem.identifiedTo, bn)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(occurrenceEntity, New Entity(nsTO + "identifiedTo"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        'interactions
        For Each intObj As TaxonOccurrenceInteraction In TaxonOccurrenceItem.Interactions
            Dim bn As BNode = Nothing
            Dim tmpMs As MemoryStore = GetInteractionMemoryStore(intObj, bn)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(occurrenceEntity, New Entity(nsTO + "hasInteraction"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        Next

        ''add. statements
        If TaxonOccurrenceItem.AdditionalStatements IsNot Nothing Then
            For Each st As Statement In TaxonOccurrenceItem.AdditionalStatements.Statements
                Dim s As New Statement(occurrenceEntity, st.Predicate, st.Object)
                If Not ms.Contains(s) Then ms.Add(s)
            Next
        End If

        Return ms
    End Function

    Private Sub AddOccurenceTripleVal(ByVal ms As MemoryStore, ByVal occ As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 AndAlso val <> "-1" Then
            ms.Add(New Statement(occ, New Entity(nsTO + predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddOccurrenceTripleEntity(ByVal ms As MemoryStore, ByVal occ As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                ms.Add(New Statement(obj, New Entity(nsType), New Entity(entType)))
            End If
            ms.Add(New Statement(occ, New Entity(nsTO + predicate), obj))
        End If
    End Sub

    Public Function GetIdentificationMemoryStore(ByVal identificationItem As Identification, ByRef identificationEntity As BNode) As MemoryStore
        Dim ms As New MemoryStore

        'todo setting for embedding BNode, linking to external resource or linking to internal resource??
        'for now use BNode as we dont have ids for identifications
        identificationEntity = New BNode(identificationItem.Id)
        ms.Add(New Statement(identificationEntity, New Entity(nsType), New Entity(nsTO + "Identification")))

        ms.Add(New Statement(identificationEntity, New Entity(nsDC + "title"), New Literal(identificationItem.taxonName)))

        If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
            ms.Add(New Statement(identificationEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + identificationItem.Id)))
        End If

        AddIdentificationTripleVal(ms, identificationEntity, "confidence", identificationItem.confidence.ToString())

        If Not identificationItem.expert Is Nothing Then
            'todo whole person - need configurable recursive depth setting ??
            AddIdentificationTripleEntity(ms, identificationEntity, "expert", identificationItem.expert.Id, nsPerson + "Person")
        End If

        AddIdentificationTripleVal(ms, identificationEntity, "expertName", identificationItem.expertName)

        If Not identificationItem.higherTaxon Is Nothing Then
            Dim tcsRdf As New TaxonNameRDF
            Dim bn As BNode = Nothing
            Dim tmpMs As MemoryStore = tcsRdf.GetConceptMemoryStore(identificationItem.higherTaxon, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(identificationEntity, New Entity(nsTO + "higherTaxon"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        AddIdentificationTripleVal(ms, identificationEntity, "location", identificationItem.location)

        If Not identificationItem.method Is Nothing Then
            'todo method object??
            AddIdentificationTripleVal(ms, identificationEntity, "location", identificationItem.method.ToString)
        End If

        AddIdentificationTripleVal(ms, identificationEntity, "methodDescriptor", identificationItem.methodDescriptor)

        If Not identificationItem.occurrence Is Nothing Then
            'todo do we need this, ie a pointer back to the occurrence
            ' should we have a setting to include the whole occurence, eg when getting identification as base object
            AddIdentificationTripleEntity(ms, identificationEntity, "occurrence", identificationItem.occurrence.Id, nsTO + "TaxonOccurrence")
        End If

        AddIdentificationTripleVal(ms, identificationEntity, "taxonName", identificationItem.taxonName)
        AddIdentificationTripleVal(ms, identificationEntity, "verbatimDet", identificationItem.verbatimDet)
        AddIdentificationTripleVal(ms, identificationEntity, "date", identificationItem.idDate.ToString(Formats.DateTimeFormat))

        If Not identificationItem.taxon Is Nothing Then
            Dim tcsRdf As New TaxonNameRDF
            Dim bn As BNode = Nothing
            Dim tmpMs As MemoryStore = tcsRdf.GetConceptMemoryStore(identificationItem.taxon, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(identificationEntity, New Entity(nsTO + "taxon"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        Return ms
    End Function

    Private Sub AddIdentificationTripleVal(ByVal ms As MemoryStore, ByVal ident As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 AndAlso val <> "-1" Then
            ms.Add(New Statement(ident, New Entity(nsTO + predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddIdentificationTripleEntity(ByVal ms As MemoryStore, ByVal ident As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                ms.Add(New Statement(obj, New Entity(nsType), New Entity(entType)))
            End If
            ms.Add(New Statement(ident, New Entity(nsTO + predicate), obj))
        End If
    End Sub

    Public Function GetInteractionMemoryStore(ByVal interactionItem As TaxonOccurrenceInteraction, ByRef intEntity As BNode) As MemoryStore
        Dim ms As New MemoryStore

        If Not interactionItem.fromOccurence Is Nothing AndAlso _
            Not interactionItem.toOccurrence Is Nothing AndAlso _
            interactionItem.interactionCategory <> TaxonOccurrenceInteractionTerm.NotSpecified Then

            intEntity = New BNode(interactionItem.Id)
            ms.Add(New Statement(intEntity, New Entity(nsType), New Entity(nsInteract + "TaxonOccurrenceInteraction")))

            ms.Add(New Statement(intEntity, New Entity(nsDC + "title"), New Literal(interactionItem.fromOccurence.identifiedTo.taxonName + " " + interactionItem.interactionCategory.ToString() + " " + interactionItem.toOccurrence.identifiedTo.taxonName)))

            If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
                ms.Add(New Statement(intEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + interactionItem.Id)))
            End If

            AddInteractionTripleEntity(ms, intEntity, "fromOccurence", interactionItem.fromOccurence.Id, nsTO + "TaxonOccurrence")
            AddInteractionTripleEntity(ms, intEntity, "interactionCategory", nsInteract + interactionItem.interactionCategory.ToString(), nsInteract + "InteractionTerm")

            AddInteractionTripleVal(ms, intEntity, "interactionCategoryString", interactionItem.interactionCategoryString)

            'todo accordingTo object?
            AddInteractionTripleVal(ms, intEntity, "accordingToString", interactionItem.accordingToString)

            If interactionItem.toOccurrence.Id = "" Then
                'special case where interaction is to a TaxonName, so recurse down to the name object
                Dim occ As New BNode
                ms.Add(New Statement(intEntity, New Entity(nsInteract + "toOccurrence"), occ))
                ms.Add(New Statement(occ, New Entity(nsType), New Entity(nsTO + "TaxonOccurrence")))

                Dim ident As New BNode
                ms.Add(New Statement(occ, New Entity(nsTO + "identifiedTo"), ident))
                ms.Add(New Statement(ident, New Entity(nsType), New Entity(nsTO + "Identification")))

                Dim tc As New BNode
                ms.Add(New Statement(ident, New Entity(nsTO + "taxon"), tc))
                ms.Add(New Statement(tc, New Entity(nsType), New Entity(nsConcept + "TaxonConcept")))

                Dim tn As New RDFResource(Proxy.LSIDProxy + interactionItem.toOccurrence.identifiedTo.taxon.hasName.Id)
                ms.Add(New Statement(tc, New Entity(nsConcept + "hasName"), tn))
                ms.Add(New Statement(tn, New Entity(nsType), New Entity(nsTCS + "TaxonName")))
            Else
                AddInteractionTripleEntity(ms, intEntity, "toOccurence", interactionItem.toOccurrence.Id, nsTO + "TaxonOccurrence")
            End If
        End If

        Return ms
    End Function

    Private Sub AddInteractionTripleVal(ByVal ms As MemoryStore, ByVal intEntity As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 AndAlso val <> "-1" Then
            ms.Add(New Statement(intEntity, New Entity(nsInteract + predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddInteractionTripleEntity(ByVal ms As MemoryStore, ByVal intEntity As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                ms.Add(New Statement(obj, New Entity(nsType), New Entity(entType)))
            End If
            ms.Add(New Statement(intEntity, New Entity(nsInteract + predicate), obj))
        End If
    End Sub

End Class
