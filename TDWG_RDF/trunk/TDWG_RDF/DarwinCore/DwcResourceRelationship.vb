
Public Class DwcResourceRelationship

    Public Shared IsCongruentToRelationship As String = "is congruent to"
    Public Shared HasSynonymRelationship As String = "has synonym"
    Public Shared IsChildOfRelationship As String = "is child taxon of"
    Public Shared IsParentOfRelationship As String = "is parent taxon of"
    Public Shared HasVernacularRelationship As String = "has vernacular"

    Public resourceRelationshipID As String = ""
    Public resourceID As String = ""
    Public relatedResourceID As String = ""
    Public relationshipOfResource As String = ""
    Public relationshipAccordingTo As String = ""
    Public relationshipEstablishedDate As DateTime = DateTime.MinValue
    Public relationshipRemarks As String = ""

    Public FromResource As Object = Nothing
    Public ToResource As Object = Nothing

End Class
