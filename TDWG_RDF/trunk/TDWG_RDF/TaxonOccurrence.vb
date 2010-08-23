Public Enum BasisOfRecordTerm
    Digital
    Fossil
    Illustration
    Living
    MovingImage
    Preserved
    StillImage
    NotSpecified
End Enum

Public Class TaxonOccurrence
    Public Id As String = ""

    Public basisOfRecord As BasisOfRecordTerm = BasisOfRecordTerm.NotSpecified
    Public basisOfRecordString As String = ""
    Public catalogNumber As String = ""
    Public collector As String = ""
    Public collectorsBatchNumber As String = ""
    Public collectorsFieldNumber As String = ""
    Public collectorTeam As Team
    Public continent As String = ""
    Public coordinateUncertaintyInMeters As Double = -1
    Public country As String = ""
    Public county As String = ""
    Public dayOfYear As Integer = -1
    Public decimalLatitude As Double = -1
    Public decimalLongitude As Double = -1
    Public derivedFrom As TaxonOccurrence
    Public disposition As String = ""
    Public earliestDateCollected As DateTime = DateTime.MinValue
    Public fieldNotes As String = ""
    Public footprintSpatialFit As Double = -1
    Public geodeticDatum As String = ""
    Public georeferenceProtocol As String = ""
    Public georeferenceRemarks As String = ""
    Public georeferenceSources As String = ""
    Public georeferenceVerificationStatus As String = ""
    Public higherGeography As String = ""
    Public hostCollection As Collection
    Public hostCollectionString As String = ""
    Public identifiedTo As Identification
    Public identifiedToString As String = ""
    Public individualCount As String = ""
    Public institutionCode As String = ""
    Public island As String = ""
    Public islandGroup As String = ""
    Public lastDateCollected As DateTime = DateTime.MinValue
    Public lifeStage As String = ""
    Public locality As String = ""
    Public maximumDepthInMeters As String = ""
    Public maximumElevationInMeters As String = ""
    Public minimumDepthInMeters As String = ""
    Public minimumElevationInMeters As String = ""
    Public pointRadiusSpatialFit As Double = -1
    Public procedure As Object 'procedure that generated this record
    Public procedureDescriptor As String = ""
    Public sex As String = ""
    Public stateProvince As String = ""
    Public typeForName As TaxonName
    Public typeStatus As NomenclaturalTypeType = NomenclaturalTypeType.NotSpecified
    Public typeStatusString As String = ""
    Public validDistibutionFlag As New NullableBoolean
    Public value As Double = -1
    Public valueConfidence As Double = -1
    Public verbatimCollectingDate As String = ""
    Public verbatimCoordinates As String = ""
    Public verbatimCoordinateSystem As String = ""
    Public verbatimDepth As String = ""
    Public verbatimElevation As String = ""
    Public verbatimLabelText As String = ""
    Public verbatimLatitude As String = ""
    Public verbatimLongitude As String = ""
    Public waterBody As String = ""
    Public wktFootprint As String = ""

    'other
    Public isReplacedBy As String = ""
    Public isRestricted As New NullableBoolean
    Public accessMessage As String = ""
    Public rightsOwner As String = ""

    Public Interactions As New ArrayList 'of TaxonOccurrenceInteraction

    Public AdditionalStatements As New SemWeb.MemoryStore
End Class

