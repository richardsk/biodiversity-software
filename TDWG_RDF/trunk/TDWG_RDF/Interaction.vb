Public Enum TaxonOccurrenceInteractionTerm
    IsHostOf
    HasHost
    NotSpecified
End Enum

Public Class TaxonOccurrenceInteraction
    Public Id As String = ""
    Public fromOccurence As TaxonOccurrence
    Public toOccurrence As TaxonOccurrence
    Public accordingTo As Object
    Public accordingToString As String = ""
    Public interactionCategory As TaxonOccurrenceInteractionTerm = TaxonOccurrenceInteractionTerm.NotSpecified
    Public interactionCategoryString As String = ""

End Class
