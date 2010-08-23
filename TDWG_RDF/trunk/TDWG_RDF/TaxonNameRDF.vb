Imports SemWeb

Public Class TaxonNameRDF
    Public Class TaxonNameEntity
        Inherits Entity

        Public Sub New(ByVal uri As String)
            MyBase.New(uri)
        End Sub
    End Class


    Public Const nsTCS As String = "http://rs.tdwg.org/ontology/voc/TaxonName#"
    Public Const nsConcept As String = "http://rs.tdwg.org/ontology/voc/TaxonConcept#"
    Public Const nsRank As String = "http://rs.tdwg.org/ontology/voc/TaxonRank#"
    Public Const nsTCom As String = "http://rs.tdwg.org/ontology/voc/Common#"
    Public Const nsPub As String = "http://rs.tdwg.org/ontology/voc/PublicationCitation#"
    Public Const nsDC As String = "http://purl.org/dc/elements/1.1/"
    Public Const nsDCTerm As String = "http://purl.org/dc/terms/"
    Public Const nsOwl As String = "http://www.w3.org/2002/07/owl#"
    Public Const nsRDF As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    Public Const nsType As String = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type"
    Public Const nsSpecimen As String = "http://rs.tdwg.org/ontology/voc/Specimen#"

    Public Function GetTaxonNameRDF(ByVal taxonNameItem As TaxonName, ByVal additionalStatements As MemoryStore) As String
        Dim bn As Resource = Nothing
        Dim ms As MemoryStore = GetNameMemoryStore(taxonNameItem, bn, True)

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
        wr.Namespaces.AddNamespace(nsRank, "trank")
        wr.Namespaces.AddNamespace(nsTCom, "tcom")
        wr.Namespaces.AddNamespace(nsPub, "tpub")
        wr.Namespaces.AddNamespace(nsDC, "dc")
        wr.Namespaces.AddNamespace(nsDCTerm, "dcterms")
        wr.Namespaces.AddNamespace(nsOwl, "owl")
        wr.Namespaces.AddNamespace(nsRDF, "rdf")
        wr.Namespaces.AddNamespace(nsSpecimen, "tto")

        ms.Write(wr)
        wr.Close()

        Return sw.ToString
    End Function

    Public Function GetNameMemoryStore(ByVal TaxonNameItem As TaxonName, ByRef nameEntity As Resource, ByVal topLevel As Boolean) As SemWeb.MemoryStore
        Dim ms As New MemoryStore

        If topLevel Then
            nameEntity = New Entity(TaxonNameItem.Id)
        Else
            Dim bn As New BNode(TaxonNameItem.Id)
            bn.Uri = bn.LocalName
            nameEntity = bn
        End If

        ms.Add(New Statement(nameEntity, New Entity(nsType), New Entity(nsTCS + "TaxonName")))

        ms.Add(New Statement(nameEntity, New Entity(nsDC + "title"), New Literal(TaxonNameItem.fullName)))

        If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
            ms.Add(New Statement(nameEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + TaxonNameItem.Id)))
        End If

        ms.Add(New Statement(nameEntity, New Entity(nsOwl + "versionInfo"), New Literal("1.1.2.1")))

        AddTaxonTripleVal(ms, nameEntity, "nameComplete", TaxonNameItem.nameComplete)
        AddTaxonTripleVal(ms, nameEntity, "uninomial", TaxonNameItem.uninomial)
        AddTaxonTripleVal(ms, nameEntity, "genusPart", TaxonNameItem.genusPart)
        AddTaxonTripleVal(ms, nameEntity, "infragenericEpithet", TaxonNameItem.infraGenericEpithet)
        AddTaxonTripleVal(ms, nameEntity, "specificEpithet", TaxonNameItem.specificEpithet)
        AddTaxonTripleVal(ms, nameEntity, "infraspecificEpithet", TaxonNameItem.infraSpecificEpithet)
        AddTaxonTripleVal(ms, nameEntity, "cultivarNameGroup", TaxonNameItem.cultivarNameGroup)
        AddTaxonTripleVal(ms, nameEntity, "authorship", TaxonNameItem.authorship)
        AddTaxonTripleVal(ms, nameEntity, "authorTeam", TaxonNameItem.authorTeam)
        AddTaxonTripleVal(ms, nameEntity, "basionymAuthorship", TaxonNameItem.basionymAuthorship)
        AddTaxonTripleVal(ms, nameEntity, "combinationAuthorship", TaxonNameItem.combinationAuthorship)
        AddTaxonTripleVal(ms, nameEntity, "year", TaxonNameItem.year)
        AddTaxonTripleVal(ms, nameEntity, "microReference", TaxonNameItem.microReference)

        If Not TaxonNameItem.isRestricted.IsNull AndAlso TaxonNameItem.isRestricted.Value Then
            ms.Add(New Statement(nameEntity, New Entity(nsTCom + "isRestricted"), New Literal("true")))

            If TaxonNameItem.accessMessage.Length > 0 Then
                ms.Add(New Statement(nameEntity, New Entity(nsDCTerm + "accessRights"), New Literal(TaxonNameItem.accessMessage)))
            End If

            If TaxonNameItem.rightsOwner.Length > 0 Then
                Dim own As New RDFResource(TaxonNameItem.rightsOwner)
                ms.Add(New Statement(nameEntity, New Entity(nsDCTerm + "rightsHolder"), own))
            End If
        End If

        If TaxonNameItem.publishedIn IsNot Nothing AndAlso TaxonNameItem.publishedIn.Length > 0 Then
            ms.Add(New Statement(nameEntity, New Entity(nsTCom + "publishedIn"), New Literal(TaxonNameItem.publishedIn)))
        End If

        Dim pubRDF As New PublicationRDF

        If Not TaxonNameItem.publication Is Nothing Then
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = pubRDF.GetPublicationMemoryStore(TaxonNameItem.publication, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(nameEntity, New Entity(nsTCS + "publication"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        If TaxonNameItem.rank <> TCSRank.NotSpecified Then
            Dim rnk As String = TaxonNameItem.rank.ToString()
            If rnk = "SubSubVariety" Then rnk = "Sub-Sub-Variety"
            If rnk = "SubVariety" Then rnk = "Sub-Variety"
            If rnk = "PathoVariety" Then rnk = "Patho-Variety"
            If rnk = "BioVariety" Then rnk = "Bio-Variety"
            If rnk = "CultivarGroup" Then rnk = "Cultivar-Group"
            If rnk = "GraftChimera" Then rnk = "Graft-Chimera"

            AddTaxonTripleEntity(ms, nameEntity, "rank", nsRank + rnk, nsRank + "TaxonRankTerm")
        End If

        AddTaxonTripleVal(ms, nameEntity, "rankString", TaxonNameItem.rankString)

        If TaxonNameItem.nomenclaturalCode <> TCSNomenclaturalCode.NotSpecified Then
            AddTaxonTripleEntity(ms, nameEntity, "nomenclaturalCode", nsTCS + TaxonNameItem.nomenclaturalCode.ToString(), nsTCS + "NomenclaturalCodeTerm")
        End If

        If TaxonNameItem.hasBasionym <> TaxonNameItem.Id And TaxonNameItem.hasBasionym.Length > 0 Then
            AddTaxonTripleEntity(ms, nameEntity, "hasBasionym", Proxy.LSIDProxy + TaxonNameItem.hasBasionym, nsTCS + "TaxonName")
        End If

        If Not TaxonNameItem.typifiedBy Is Nothing Then
            AddNomenclaturalType(ms, nameEntity, TaxonNameItem.typifiedBy)
        End If

        For Each nn As NomenclaturalNote In TaxonNameItem.Notes
            AddNomenclaturalNote(ms, nameEntity, nn)
        Next

        'If TaxonNameItem.isAnamorphic Then
        '    AddTaxonTripleVal(ms, name, "isAnamorphic", "true")
        'Else
        '    AddTaxonTripleVal(ms, name, "isAnamorphic", "false")
        'End If

        If TaxonNameItem.referenceTo.Length > 0 Then
            AddTaxonTripleVal(ms, nameEntity, "referenceTo", Proxy.LSIDProxy + TaxonNameItem.referenceTo)
        End If

        If TaxonNameItem.isReplacedBy.Length > 0 Then
            AddTaxonTripleEntity(ms, nameEntity, nsDC + "isReplacedBy", Proxy.LSIDProxy + TaxonNameItem.isReplacedBy, nsTCS + "TaxonName")
        End If

        If TaxonNameItem.AdditionalStatements IsNot Nothing Then
            For Each st As Statement In TaxonNameItem.AdditionalStatements.Statements
                Dim s As New Statement(nameEntity, st.Predicate, st.Object)
                If Not ms.Contains(s) Then ms.Add(s)
            Next
        End If

        Return ms
    End Function


    Private Sub AddTaxonTripleVal(ByVal ms As MemoryStore, ByVal name As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 Then
            ms.Add(New Statement(name, New Entity(nsTCS + predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddTaxonTripleEntity(ByVal ms As MemoryStore, ByVal name As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                Dim st As New Statement(obj, New Entity(nsType), New Entity(entType))
                If Not ms.Contains(st) Then
                    ms.Add(st)
                End If
            End If
            ms.Add(New Statement(name, New Entity(nsTCS + predicate), obj))
        End If
    End Sub

    Private Sub AddNomenclaturalType(ByVal ms As MemoryStore, ByVal name As Entity, ByVal nt As NomenclaturalType)
        If Not nt Is Nothing Then
            Dim ntNode As New BNode

            ms.Add(New Statement(ntNode, New Entity(nsType), New Entity(nsTCS + "NomenclaturalType")))
            ms.Add(New Statement(name, New Entity(nsTCS + "typifiedBy"), ntNode))

            If nt.typeOfType <> NomenclaturalTypeType.NotSpecified Then
                Dim t As New Entity(nt.typeOfType.ToString)
                ms.Add(New Statement(t, New Entity(nsType), New Entity(nsTCS + "NomenclaturalTypeTypeTerm")))
                ms.Add(New Statement(ntNode, New Entity(nsTCS + "typeOfType"), t))
            End If

            If nt.NameLSID.Length > 0 Then
                Dim nn As New Entity(Proxy.LSIDProxy + nt.NameLSID)
                ms.Add(New Statement(nn, New Entity(nsType), New Entity(nsTCS + "TaxonName")))
                ms.Add(New Statement(ntNode, New Entity(nsTCS + "typeName"), nn))
            End If

            If nt.SpecimenLSID.Length > 0 Then
                'todo this should be an LSID one day
                'Dim sn As New Entity(nt.SpecimenLSID)
                'ms.Add(New Statement(sn, New Entity(nsType), New Entity(nsSpecimen + "Specimen")))
                'ms.Add(New Statement(ntNode, New Entity(nsTCS + "typeSpecimen"), sn))
                ms.Add(New Statement(ntNode, New Entity(nsTCS + "typeSpecimen"), New Literal(Proxy.LSIDProxy + nt.SpecimenLSID)))
            End If
        End If
    End Sub

    Private Sub AddNoteTripleEntity(ByVal ms As MemoryStore, ByVal note As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                Dim st As New Statement(obj, New Entity(nsType), New Entity(entType))
                If Not ms.Contains(st) Then
                    ms.Add(st)
                End If
            End If
            ms.Add(New Statement(note, New Entity(nsTCS + predicate), obj))
        End If
    End Sub

    Private Sub AddNomenclaturalNote(ByVal ms As MemoryStore, ByVal name As Entity, ByVal note As NomenclaturalNote)
        If Not note Is Nothing Then
            Dim nn As New BNode

            ms.Add(New Statement(nn, New Entity(nsType), New Entity(nsTCS + "NomenclaturalNote")))
            ms.Add(New Statement(name, New Entity(nsTCS + "hasAnnotation"), nn))

            If note.noteType <> NomenclaturalNoteType.NotSpecified Then
                Dim nt As New Entity(note.noteType.ToString())
                ms.Add(New Statement(nt, New Entity(nsType), New Entity(nsTCS + "NomenclaturalNoteTypeTerm")))
                ms.Add(New Statement(nn, New Entity(nsTCS + "type"), nt))
            End If

            If note.code <> TCSNomenclaturalCode.NotSpecified Then
                Dim c As New Entity(note.code.ToString())
                ms.Add(New Statement(c, New Entity(nsType), New Entity(nsTCS + "NomenclaturalCodeTerm")))
                ms.Add(New Statement(nn, New Entity(nsTCS + "code"), c))
            End If

            If note.note.Length > 0 Then
                ms.Add(New Statement(nn, New Entity(nsTCS + "note"), New Literal(note.note)))
            End If

            If note.objectTaxonName.Length > 0 Then
                AddNoteTripleEntity(ms, nn, "objectTaxonName", Proxy.LSIDProxy + note.objectTaxonName, nsTCS + "TaxonName")
            End If

            If note.subjectTaxonName.Length > 0 Then
                AddNoteTripleEntity(ms, nn, "subjectTaxonName", Proxy.LSIDProxy + note.subjectTaxonName, nsTCS + "TaxonName")
            End If

            If note.ruleConsidered.Length > 0 Then
                ms.Add(New Statement(nn, New Entity(nsTCS + "ruleConsidered"), New Literal(note.ruleConsidered)))
            End If
        End If
    End Sub

    Public Function GetConceptMemoryStore(ByVal conceptItem As TaxonConcept, ByRef conceptEntity As Resource, ByVal topLevel As Boolean) As MemoryStore
        Dim ms As New MemoryStore

        If topLevel Then
            conceptEntity = New Entity(conceptItem.Id)
        Else
            Dim bn As New BNode
            If conceptItem.Id.Length > 0 Then
                bn = New BNode(conceptItem.Id)
            End If

            bn.Uri = bn.LocalName
            conceptEntity = bn
        End If

        ms.Add(New Statement(conceptEntity, New Entity(nsType), New Entity(nsConcept + "TaxonConcept")))

        ms.Add(New Statement(conceptEntity, New Entity(nsDC + "title"), New Literal(conceptItem.nameString + " according to " + conceptItem.accordingToString)))

        If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 AndAlso conceptItem.Id.Length > 0 Then
            ms.Add(New Statement(conceptEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + conceptItem.Id)))
        End If

        'todo conceptItem.accordingTo object ? can be publication, person etc

        AddConceptTripleVal(ms, conceptEntity, "accordingToString", conceptItem.accordingToString)
        AddConceptTripleVal(ms, conceptEntity, "nameString", conceptItem.nameString)
        AddConceptTripleVal(ms, conceptEntity, "primary", conceptItem.primary.ToString())

        Dim specRdf As New SpecimenRDF

        If Not conceptItem.circumscribedBy Is Nothing Then
            'todo is this recursive - need setting to work out number of levels of recursion
            ' to work out whether to add whole object or just a link to it

            Dim occEntity As New RDFResource(Proxy.LSIDProxy + conceptItem.circumscribedBy.Id)
            ms.Add(New Statement(conceptEntity, New Entity(nsConcept + "circumscribedBy"), occEntity))

            'Dim bn As BNode
            'Dim tmpMs As MemoryStore = specRdf.GetOccurrenceMemoryStore(conceptItem.circumscribedBy, bn)
            'If tmpMs.StatementCount > 0 Then
            '    ms.Add(New Statement(conceptEntity, New Entity(nsConcept + "circumscribedBy"), bn))
            '    For Each s As Statement In tmpMs.Statements
            '        ms.Add(s)
            '    Next
            'End If
        End If

        If Not conceptItem.describedBy Is Nothing Then
            'todo
            'Dim bn As BNode
            'Dim tmpMs As MemoryStore = GetDescriptionMemoryStore(conceptItem.describedBy, bn)
            'If tmpMs.StatementCount > 0 Then
            '    ms.Add(New Statement(conceptEntity, New Entity(nsConcept + "describedBy"), bn))
            '    For Each s As Statement In tmpMs.Statements
            '        ms.Add(s)
            '    Next
            'End If
        End If

        Dim genRdf As New GeneralRDF

        If Not conceptItem.hasInformation Is Nothing Then
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = genRdf.GetInfoMemoryStore(conceptItem.hasInformation, bn)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(conceptEntity, New Entity(nsConcept + "hasInformation"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        If Not conceptItem.hasName Is Nothing Then
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = GetNameMemoryStore(conceptItem.hasName, bn, False) 'todo - needs to be true to show rdf:about with lsid??
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(conceptEntity, New Entity(nsConcept + "hasName"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        If topLevel Then
            For Each rel As Relationship In conceptItem.Relationships
                Dim bn As BNode = Nothing
                Dim tmpMs As MemoryStore = GetRelationshipMemoryStore(rel, bn)
                If tmpMs.StatementCount > 0 Then
                    ms.Add(New Statement(conceptEntity, New Entity(nsConcept + "hasRelationship"), bn))
                    For Each s As Statement In tmpMs.Statements
                        ms.Add(s)
                    Next
                End If
            Next
        End If

        Return ms
    End Function

    Private Sub AddConceptTripleVal(ByVal ms As MemoryStore, ByVal concept As Entity, ByVal predicate As String, ByVal val As String)
        If val.Length > 0 Then
            ms.Add(New Statement(concept, New Entity(nsConcept + predicate), New Literal(val)))
        End If
    End Sub

    Private Sub AddConceptTripleEntity(ByVal ms As MemoryStore, ByVal subject As Entity, ByVal predicate As String, ByVal entityName As String, ByVal entType As String)
        If entityName.Length > 0 Then
            Dim obj As Entity = New Entity(entityName)
            If entType.Length > 0 Then
                Dim st As New Statement(obj, New Entity(nsType), New Entity(entType))
                If Not ms.Contains(st) Then
                    ms.Add(st)
                End If
            End If
            ms.Add(New Statement(subject, New Entity(nsConcept + predicate), obj))
        End If
    End Sub

    Public Function GetRelationshipMemoryStore(ByVal rel As Relationship, ByRef relEntity As BNode) As MemoryStore
        Dim ms As New MemoryStore

        relEntity = New BNode
        ms.Add(New Statement(relEntity, New Entity(nsType), New Entity(nsConcept + "Relationship")))

        ms.Add(New Statement(relEntity, New Entity(nsDC + "title"), New Literal(rel.fromTaxon.nameString + " " + rel.relationshipCategory.ToString() + " " + rel.toTaxon.nameString)))

        'If Not Proxy.LSIDProxy Is Nothing AndAlso Proxy.LSIDProxy.Length > 0 Then
        '    ms.Add(New Statement(relEntity, New Entity(nsOwl + "sameAs"), New RDFResource(Proxy.LSIDProxy + rel.Id)))
        'End If

        If Not rel.fromTaxon Is Nothing Then
            Dim bn As BNode = Nothing
            Dim tmpMs As MemoryStore = GetConceptMemoryStore(rel.fromTaxon, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(relEntity, New Entity(nsConcept + "fromTaxon"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        If Not rel.toTaxon Is Nothing Then
            Dim bn As BNode = Nothing
            Dim tmpMs As MemoryStore = GetConceptMemoryStore(rel.toTaxon, bn, False)
            If tmpMs.StatementCount > 0 Then
                ms.Add(New Statement(relEntity, New Entity(nsConcept + "toTaxon"), bn))
                For Each s As Statement In tmpMs.Statements
                    ms.Add(s)
                Next
            End If
        End If

        If rel.relationshipCategory <> TaxonRelationshipTerm.NotSpecified Then
            AddConceptTripleEntity(ms, relEntity, "relationshipCategory", nsConcept + rel.relationshipCategory.ToString(), nsConcept + "TaxonRelationshipTerm")
        End If

        Return ms
    End Function
End Class