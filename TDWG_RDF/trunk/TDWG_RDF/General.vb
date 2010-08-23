Public Class Formats
    Public Const DateTimeFormat As String = "yyyy-MM-ddThh:mm:ss" 'todo time zone 
End Class

Public Class TermWithSource
    Public idInSource As String = ""
    Public source As String = ""
    Public term As String = ""
End Class

Public Class DefinedTerm
    Public definition As String = ""
End Class

Public Class InfoItem
    Public Id As String = ""

    Public associatedTaxon As TaxonConcept
    Public context As String = ""
    Public contextOccurrence As TaxonOccurrence
    Public contextValue As DefinedTerm
    Public hasContent As String = ""
    Public hasValue As DefinedTerm
End Class