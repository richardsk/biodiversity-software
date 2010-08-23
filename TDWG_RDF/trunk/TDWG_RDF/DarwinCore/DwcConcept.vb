Public Class DwcConcept
    Public conceptID As String = ""
    Public accordingToID As String = ""
    Public accordingTo As String = ""

    Public Taxon As DwcTaxon = Nothing
    Public Relationships As New List(Of DwcResourceRelationship)

    Public Sub AddRelationship(ByVal toConcept As DwcConcept, ByVal relAccordingTo As String, ByVal relationshipType As String, ByVal establishedDate As DateTime, ByVal remarks As String)
        Dim rr As New DwcResourceRelationship
        rr.FromResource = Me
        rr.ToResource = toConcept
        rr.resourceID = conceptID
        rr.relatedResourceID = toConcept.conceptID
        rr.relationshipAccordingTo = relAccordingTo
        rr.relationshipEstablishedDate = establishedDate
        rr.relationshipOfResource = relationshipType
        rr.relationshipRemarks = remarks

        Relationships.Add(rr)
    End Sub

End Class
