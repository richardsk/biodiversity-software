Public Class Identification
    Public Id As String = ""

    Public confidence As Double = -1
    Public idDate As DateTime = DateTime.MinValue
    Public expert As Person
    Public expertName As String = ""
    Public higherTaxon As TaxonConcept
    Public location As String = ""
    Public method As Object 'procedure used to identify occurrence
    Public methodDescriptor As String = ""
    Public occurrence As TaxonOccurrence
    Public taxon As TaxonConcept
    Public taxonName As String = ""
    Public verbatimDet As String = ""
End Class
