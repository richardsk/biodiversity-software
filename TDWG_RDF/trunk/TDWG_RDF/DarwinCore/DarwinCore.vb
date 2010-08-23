
'an xml/abstract schema
Public Class DarwinCore2

    Public TaxonNames As New List(Of DwcTaxon)
    Public TaxonConcepts As New List(Of DwcConcept)
    'todo Public Specimens As New list(Of DwCSpecimen)

    Public Sub AddConcept(ByVal tc As DwcConcept)
        TaxonConcepts.Add(tc)
    End Sub

    Public Sub AddName(ByVal tn As DwcTaxon)
        TaxonNames.Add(tn)
    End Sub

    Public Function GetDwcXml(ByVal source As String) As String
        Dim xml As String = "<?xml version=""1.0""?> " + _
            "<dwr:DarwinRecordSet " + _
            "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" " + _
            "xsi:schemaLocation=""http://rs.tdwg.org/dwc/dwcrecord/  http://rs.tdwg.org/dwc/xsd/tdwg_dwc_classes.xsd"" " + _
            "xmlns:dcterms=""http://purl.org/dc/terms/"" " + _
            "xmlns:dwc=""http://rs.tdwg.org/dwc/terms/"" " + _
            "xmlns:dwr=""http://rs.tdwg.org/dwc/dwcrecord/"" " + _
            "xmlns:dc=""http://purl.org/dc/elements/1.1/"" > "

        If TaxonConcepts.Count > 0 Then
            xml += GetConceptsXml(source)
        End If

        If TaxonNames.Count > 0 Then
            xml += GetNamesXml(source)
        End If

        'todo
        'If Specimens.Count > 0 Then
        '    xml += GetSpecimensXml()
        'End If

        xml += "</dwr:DarwinRecordSet>"

        Return xml
    End Function

    Public Function GetConceptsXml(ByVal source As String) As String
        Dim xml As String = ""
        Dim taxaAdded As New List(Of String)

        For Each tc As DwcConcept In TaxonConcepts
            Dim tn As DwcTaxon = tc.Taxon

            xml += GetNameXml(tn, source)
            taxaAdded.Add(tc.conceptID)

            For Each rr As DwcResourceRelationship In tc.Relationships
                Dim c As DwcConcept = CType(rr.ToResource, DwcConcept)
                If Not taxaAdded.Contains(c.conceptID) Then
                    xml += GetNameXml(c.Taxon, source)
                    taxaAdded.Add(c.conceptID)
                End If
            Next

            For Each rr As DwcResourceRelationship In tc.Relationships
                xml += "<dwc:ResourceRelationship>"

                xml += "<dwc:resourceID" > +rr.resourceID + "</dwc:resourceID>"
                xml += "<dwc:relatedResourceID>" + rr.relatedResourceID + "</dwc:relatedResourceID>"
                xml += "<dwc:relationshipOfResource>" + rr.relationshipOfResource + "</dwc:relationshipOfResource>"
                If rr.relationshipAccordingTo.Length > 0 Then xml += "<dwc:relationshipAccordingTo>" + rr.relationshipAccordingTo + "</dwc:relationshipAccordingTo>"
                If rr.relationshipEstablishedDate <> DateTime.MinValue Then xml += "<dwc:relationshipEstablishedDate>" + rr.relationshipEstablishedDate.ToString(Formats.DateTimeFormat) + "</dwc:relationshipEstablishedDate>"
                If rr.relationshipRemarks.Length > 0 Then xml += "<dwc:relationshipRemarks>" + rr.relationshipRemarks + "</dwc:relationshipRemarks>"

                xml += "</dwc:ResourceRelationship>"
            Next

        Next

        Return xml
    End Function

    Public Function GetNamesXml(ByVal source As String) As String
        Dim xml As String = ""

        For Each tn As DwcTaxon In TaxonNames
            xml += GetNameXml(tn, source)
        Next

        Return xml
    End Function

    Private Function GetNameXml(ByVal tn As DwcTaxon, ByVal source As String) As String

        Dim xml As String = "<dwc:Taxon>"

        If tn.taxonID.Length > 0 Then xml += "<dwc:taxonID>" + tn.taxonID + "</dwc:taxonID>"
        If tn.taxonConceptID.Length > 0 Then xml += "<dwc:taxonConceptID>" + tn.taxonConceptID + "</dwc:taxonConceptID>"
        If source.Length > 0 Then xml += "<dc:source>" + source + "</dc:source>"
        If tn.recordModifiedDate <> DateTime.MinValue Then xml += "<dcterms:modified>" + tn.recordModifiedDate.ToString(Formats.DateTimeFormat) + "</dcterms:modified>"
        If tn.recordCreator.Length > 0 Then xml += "<dc:creator>" + System.Web.HttpUtility.HtmlEncode(tn.recordCreator) + "</dc:creator>"
        xml += "<dcterms:language>en</dcterms:language>"
        xml += "<dcterms:title>" + System.Web.HttpUtility.HtmlEncode(tn.scientificName) + "</dcterms:title>"
        If tn.scientificNameID.Length > 0 Then xml += "<dwc:scientificNameID>" + tn.scientificNameID + "</dwc:scientificNameID>"
        If tn.scientificName.Length > 0 Then xml += "<dwc:scientificName>" + System.Web.HttpUtility.HtmlEncode(tn.scientificName) + "</dwc:scientificName>"
        If tn.acceptedNameUsageID.Length > 0 Then xml += "<dwc:acceptedNameUsageID>" + tn.acceptedNameUsageID + "</dwc:acceptedNameUsageID>"
        If tn.acceptedNameUsage.Length > 0 Then xml += "<dwc:acceptedNameUsage>" + System.Web.HttpUtility.HtmlEncode(tn.acceptedNameUsage) + "</dwc:acceptedNameUsage>"
        If tn.parentNameUsageID.Length > 0 Then xml += "<dwc:parentNameUsageID>" + tn.parentNameUsageID + "</dwc:parentNameUsageID>"
        If tn.parentNameUsage.Length > 0 Then xml += "<dwc:parentNameUsage>" + System.Web.HttpUtility.HtmlEncode(tn.parentNameUsage) + "</dwc:parentNameUsage>"
        If tn.originalNameUsageID.Length > 0 Then xml += "<dwc:originalNameUsageID>" + tn.originalNameUsageID + "</dwc:originalNameUsageID>"
        If tn.originalNameUsage.Length > 0 Then xml += "<dwc:originalNameUsage>" + System.Web.HttpUtility.HtmlEncode(tn.originalNameUsage) + "</dwc:originalNameUsage>"
        If tn.nameAcccordingToID.Length > 0 Then xml += "<dwc:nameAcccordingToID>" + tn.nameAcccordingToID + "</dwc:nameAcccordingToID>"
        If tn.nameAcccordingTo.Length > 0 Then xml += "<dwc:nameAcccordingTo>" + System.Web.HttpUtility.HtmlEncode(tn.nameAcccordingTo) + "</dwc:nameAcccordingTo>"
        If tn.namePublishedInID.Length > 0 Then xml += "<dwc:namePublishedInID>" + tn.namePublishedInID + "</dwc:namePublishedInID>"
        If tn.namePublishedIn.Length > 0 Then xml += "<dwc:namePublishedIn>" + System.Web.HttpUtility.HtmlEncode(tn.namePublishedIn) + "</dwc:namePublishedIn>"
        If tn.higherClassification.Length > 0 Then xml += "<dwc:higherClassification>" + System.Web.HttpUtility.HtmlEncode(tn.higherClassification) + "</dwc:higherClassification>"
        If tn.kingdom.Length > 0 Then xml += "<dwc:kingdom>" + tn.kingdom + "</dwc:kingdom>"
        If tn.phylum.Length > 0 Then xml += "<dwc:phylum>" + tn.phylum + "</dwc:phylum>"
        If tn.[class].Length > 0 Then xml += "<dwc:class>" + tn.class + "</dwc:class>"
        If tn.order.Length > 0 Then xml += "<dwc:order>" + tn.order + "</dwc:order>"
        If tn.family.Length > 0 Then xml += "<dwc:family>" + tn.family + "</dwc:family>"
        If tn.genus.Length > 0 Then xml += "<dwc:genus>" + System.Web.HttpUtility.HtmlEncode(tn.genus) + "</dwc:genus>"
        If tn.subgenus.Length > 0 Then xml += "<dwc:subgenus>" + System.Web.HttpUtility.HtmlEncode(tn.subgenus) + "</dwc:subgenus>"
        If tn.specificEpithet.Length > 0 Then xml += "<dwc:specificEpithet>" + System.Web.HttpUtility.HtmlEncode(tn.specificEpithet) + "</dwc:specificEpithet>"
        If tn.infraspecificEpithet.Length > 0 Then xml += "<dwc:infraspecificEpithet>" + System.Web.HttpUtility.HtmlEncode(tn.infraspecificEpithet) + "</dwc:infraspecificEpithet>"
        If tn.taxonRank.Length > 0 Then xml += "<dwc:taxonRank>" + System.Web.HttpUtility.HtmlEncode(tn.taxonRank) + "</dwc:taxonRank>"
        If tn.verbatimTaxonRank.Length > 0 Then xml += "<dwc:verbatimTaxonRank>" + System.Web.HttpUtility.HtmlEncode(tn.verbatimTaxonRank) + "</dwc:verbatimTaxonRank>"
        If tn.scientificNameAuthorship.Length > 0 Then xml += "<dwc:scientificNameAuthorship>" + System.Web.HttpUtility.HtmlEncode(tn.scientificNameAuthorship) + "</dwc:scientificNameAuthorship>"
        If tn.vernacularName.Length > 0 Then xml += "<dwc:vernacularName>" + System.Web.HttpUtility.HtmlEncode(tn.vernacularName) + "</dwc:vernacularName>"
        If tn.nomenclaturalCode.Length > 0 Then xml += "<dwc:nomenclaturalCode>" + tn.nomenclaturalCode + "</dwc:nomenclaturalCode>"
        If tn.taxonomicStatus.Length > 0 Then xml += "<dwc:taxonomicStatus>" + System.Web.HttpUtility.HtmlEncode(tn.taxonomicStatus) + "</dwc:taxonomicStatus>"
        If tn.nomenclaturalStatus.Length > 0 Then xml += "<dwc:nomenclaturalStatus>" + System.Web.HttpUtility.HtmlEncode(tn.nomenclaturalStatus) + "</dwc:nomenclaturalStatus>"
        If tn.taxonRemarks.Length > 0 Then xml += "<dwc:taxonRemarks>" + System.Web.HttpUtility.HtmlEncode(tn.taxonRemarks) + "</dwc:taxonRemarks>"

        xml += "</dwc:Taxon>"

        Return xml
    End Function

    Public Function GetSpecimensXml() As String
        'TODO
        Return ""
    End Function

    Public Function SaveNamesCsv(ByVal source As String, ByVal includeColumnNames As Boolean, ByVal fileName As String) As Boolean
        Dim csvLine As String = ""
        Dim ok As Boolean = True

        Try
            Dim fs As IO.StreamWriter = IO.File.CreateText(fileName)

            If includeColumnNames Then
                csvLine = "taxonNameID,dc:source,scientificName,acceptedTaxonNameID,acceptedTaxonName,higherTaxonNameID,higherTaxonName,basionymID,basionym,kingdom,phylum,class,order,family,genus,subgenus,specificEpithet,infraspecificEpithet,taxonRank,scientificNameAuthorship,nomenclaturalCode,namePublicationID,namePublishedIn,taxonomicStatus,nomenclaturalStatus,taxonRemarks,taxonConceptID,taxonAccordingTo"
                fs.WriteLine(csvLine)
            End If

            For Each tc As DwcConcept In TaxonConcepts
                Dim tn As DwcTaxon = tc.Taxon

                csvLine = tn.taxonID + ","""
                csvLine += source + ""","
                csvLine += """" + tn.scientificName + ""","
                csvLine += tn.acceptedNameUsageID + ","
                csvLine += """" + tn.acceptedNameUsage + ""","
                csvLine += tn.higherClassification + ","
                csvLine += ","
                csvLine += tn.originalNameUsageID + ","
                csvLine += """" + tn.originalNameUsage + ""","
                csvLine += """" + tn.kingdom + ""","
                csvLine += """" + tn.phylum + ""","
                csvLine += """" + tn.class + ""","
                csvLine += """" + tn.order + ""","
                csvLine += """" + tn.family + ""","
                csvLine += """" + tn.genus + ""","
                csvLine += """" + tn.subgenus + ""","
                csvLine += """" + tn.specificEpithet + ""","
                csvLine += """" + tn.infraspecificEpithet + ""","
                csvLine += """" + tn.taxonRank + ""","
                csvLine += """" + tn.scientificNameAuthorship + ""","
                csvLine += """" + tn.nomenclaturalCode + ""","
                csvLine += tn.namePublishedInID + ","
                csvLine += """" + tn.namePublishedIn + ""","
                csvLine += """" + tn.taxonomicStatus + ""","
                csvLine += """" + tn.nomenclaturalStatus + ""","
                csvLine += """" + tn.taxonRemarks + ""","
                csvLine += tn.taxonConceptID + ","
                csvLine += """" + tn.nameAcccordingTo + """"

                fs.WriteLine(csvLine)

            Next

            fs.Close()

        Catch ex As Exception
            ok = False
        End Try

        Return ok
    End Function

    Public Function GetNamesCsv(ByVal source As String, ByVal includeColumnNames As Boolean) As String
        Dim csv As String = ""

        If includeColumnNames Then
            csv = "taxonNameID,dc:source,scientificName,acceptedTaxonNameID,acceptedTaxonName,higherTaxonNameID,higherTaxonName,basionymID,basionym,kingdom,phylum,class,order,family,genus,subgenus,specificEpithet,infraspecificEpithet,taxonRank,scientificNameAuthorship,nomenclaturalCode,namePublicationID,namePublishedIn,taxonomicStatus,nomenclaturalStatus,taxonRemarks,taxonConceptID,taxonAccordingTo"
            csv += Environment.NewLine
        End If

        For Each tc As DwcConcept In TaxonConcepts

            Dim tn As DwcTaxon = tc.Taxon

            Dim csvLine As String = ""

            csvLine += tn.taxonID + ","""
            csvLine += source + ""","
            csvLine += """" + tn.scientificName + ""","
            csvLine += tn.acceptedNameUsageID + ","
            csvLine += """" + tn.acceptedNameUsage + ""","
            csvLine += tn.higherClassification + ","
            csvLine += ","
            csvLine += tn.originalNameUsageID + ","
            csvLine += """" + tn.originalNameUsage + ""","
            csvLine += """" + tn.kingdom + ""","
            csvLine += """" + tn.phylum + ""","
            csvLine += """" + tn.class + ""","
            csvLine += """" + tn.order + ""","
            csvLine += """" + tn.family + ""","
            csvLine += """" + tn.genus + ""","
            csvLine += """" + tn.subgenus + ""","
            csvLine += """" + tn.specificEpithet + ""","
            csvLine += """" + tn.infraspecificEpithet + ""","
            csvLine += """" + tn.taxonRank + ""","
            csvLine += """" + tn.scientificNameAuthorship + ""","
            csvLine += """" + tn.nomenclaturalCode + ""","
            csvLine += tn.namePublishedInID + ","
            csvLine += """" + tn.namePublishedIn + ""","
            csvLine += """" + tn.taxonomicStatus + ""","
            csvLine += """" + tn.nomenclaturalStatus + ""","
            csvLine += """" + tn.taxonRemarks + ""","
            csvLine += tn.taxonConceptID + ","
            csvLine += """" + tn.nameAcccordingTo + """"

            csv += csvLine + Environment.NewLine

        Next

        Return csv
    End Function

End Class
