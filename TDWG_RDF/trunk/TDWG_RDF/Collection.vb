Public Enum CollectionTypeTerm
    archive
    culture
    dormant
    fossil
    herbarium
    library
    living
    observations
    photography
    preserved
    notSpecified
End Enum

Public Enum DevelopmentStatusTypeTerm
    active
    closed
    decreasing
    lost
    passive
    sampling
    staticStatus
    notSpecified
End Enum

Public Enum PrimaryFocusTerm
    archive
    artwork
    bibliographic
    expedition
    regional
    taxonomic
    temporal
    notSpecified
End Enum

Public Enum PrimaryPurposeTerm
    education
    exhibition
    ornamental
    research
    notSpecified
End Enum

Public Enum PreservationMethodTypeTerm
    alcohol
    deep_frozen
    dried
    dired_and_pressed
    formalin
    freeze_dried
    glycerin
    gum_arabic
    microscopic
    mounted
    no_treatment
    pinned
    refrigerated
    notSpecified
End Enum

Public Class Collection
    Public Id As String = ""

    Public alternativeId As TermWithSource
    Public citeAs As String = ""
    Public collectionType As CollectionTypeTerm = CollectionTypeTerm.notSpecified
    Public collectorNameCoverage As TermWithSource
    Public commonNameCoverage As TermWithSource
    Public conservationStatus As TermWithSource
    Public conservationStatusDate As DateTime = DateTime.MinValue
    Public descriptionForSpecialists As String = ""
    Public developmentStatus As DevelopmentStatusTypeTerm = DevelopmentStatusTypeTerm.notSpecified
    Public expeditionNameCoverage As TermWithSource
    Public formationPeriod As String = ""
    Public geospatialCoverage As TermWithSource
    Public hasContact As ContactDetails
    Public hasOwner As Institution
    Public isPartOfCollection As Collection
    Public itemLevelAccess As String = "" 'URL to web site/servcie of data for this collection's items
    Public kingdomCoverage As KingdomTypeTerm = KingdomTypeTerm.NotSpecified
    Public knowToContainTypes As New NullableBoolean
    Public livingTimePeriodCoverage As TermWithSource
    Public physicalLocation As Institution
    Public primaryFocus As PrimaryFocusTerm = PrimaryFocusTerm.notSpecified
    Public primaryPurpose As PrimaryPurposeTerm = PrimaryPurposeTerm.notSpecified
    Public relatedCollection As String = "" ' Id of collection
    Public specimenPreservationMethod As PreservationMethodTypeTerm = PreservationMethodTypeTerm.notSpecified
    Public taxonCoverage As TermWithSource
    Public usageRestrictions As String = ""
End Class
