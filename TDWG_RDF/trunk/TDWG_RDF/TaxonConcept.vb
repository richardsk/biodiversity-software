Public Enum TaxonRelationshipTerm
    DoesNotInclude
    DoesNotOverlap
    Excludes
    HasSynonym
    HasVernacular
    Includes
    IsAmbiregnalOf
    IsAnamorphOf
    IsChildTaxonOf
    IsCongruentTo
    IsFemaleParentOf
    IsFirstParentOf
    IsHybridChildOf
    IsHybridParentOf
    IsIncludedIn
    IsMaleParentOf
    IsNotCongruentTo
    IsNotIncludedIn
    IsParentTaxonOf
    IsSecondParentOf
    IsSynonymFor
    IsTeleomorphOf
    IsVernacularFor
    Overlaps
    NotSpecified
End Enum

Public Class Description
    Public Id As String = ""
    'todo
End Class

Public Class Relationship
    Public Id As String = ""

    Public fromTaxon As TaxonConcept
    Public relationshipCategory As TaxonRelationshipTerm = TaxonRelationshipTerm.NotSpecified
    Public toTaxon As TaxonConcept
End Class

Public Class TaxonConcept
    Public Id As String = ""

    Public accordingTo As Object
    Public accordingToString As String = ""
    Public circumscribedBy As TaxonOccurrence
    Public describedBy As Description
    Public hasInformation As InfoItem
    Public hasName As TaxonName
    Public nameString As String = ""
    Public primary As New NullableBoolean
    'Public rank As TCSRank = TCSRank.NotSpecified -- deprecated
    'public rankString as String = "" -- deprecated

    Public Relationships As New List(Of Relationship)

End Class
