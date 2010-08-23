Public Enum PublicationType
    Generic
    Artwork
    AudiovisualMaterial
    Book
    BookSection
    BookSeries
    ComputerProgram
    ConferenceProceedings
    EditedBook
    Journal
    JournalArticle
    MagazineArticle
    Map
    NewspaperArticle
    Patent
    Report
    Thesis
    Communication
    SubReference
    Determination
    Commentary
    WebPage
    NotSpecififed
End Enum

Public Class PublicationCitation
    Public Id As String = ""
    Public publicationType As publicationType = publicationType.NotSpecififed
    Public authorTeam As String = ""
    Public parentPublication As String = "" ' LSID of parent publication
    Public authorship As String = ""
    Public year As String = ""
    Public title As String = ""
    Public parentPublicationString As String = ""
    Public shortTitle As String = ""
    Public alternateTitle As String = ""
    Public publisher As String = ""
    Public placePublished As String = ""
    Public volume As String = ""
    Public numberOfVolumes As String = ""
    Public number As String = ""
    Public pages As String = ""
    Public section As String = ""
    Public edition As String = ""
    Public datePublished As String = ""
    Public startDate As String = ""
    Public endDate As String = ""
    Public isbn As String = ""
    Public issn As String = ""
    Public doi As String = ""
    Public reprintEdition As String = ""
    Public figures As String = ""
    Public url As String = ""
End Class
