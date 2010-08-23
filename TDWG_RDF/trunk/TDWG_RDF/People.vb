Public Enum ContactTypeTerm
    bbs
    car
    cell
    fax
    home
    isdn
    modem
    msg
    pager
    pcs
    pref
    video
    voice
    work
    notSpecified
End Enum

Public Enum InstituteTypeTerm
    administration
    association
    botanic_garden
    field_station
    herbarium
    horticulture
    independent_expert
    laboratory
    library
    museum
    nature_education_centre
    nature_reserves_management
    non_university_college
    private_company
    research_institute
    university
    voluntary_observer
    zoo_or_aquarium
    notSpecified
End Enum

Public Class PersonNameAlias
    Public Id As String = ""

    Public context As String = ""
    Public forenames As String = ""
    Public isPreferred As New NullableBoolean
    Public standardForm As String = ""
    Public surname As String = ""
End Class

Public Class Person
    Public Id As String = ""

    Public nameAlias As PersonNameAlias
    Public authorOf As String = "" 'LSID to publication or other object that the person has authored
    Public birthDate As String = "" 'ISO
    Public contactDetails As contactDetails
    Public deathDate As String = "" 'ISO
    Public flourishedDate As String = "" 'ISO
    Public geographicScope As String = ""
    Public lifespan As String = ""
    Public subjectScope As String = ""
End Class

Public Class Institution
    Public Id As String = ""

    Public acronymOrCoden As TermWithSource
    Public alternativeId As TermWithSource
    Public hasCollection As Collection
    Public hasContact As ContactDetails
    Public institutionType As InstituteTypeTerm = InstituteTypeTerm.notSpecified
    Public isPartOfInstitution As Institution
End Class

Public Class TeamMember
    Public Id As String = ""

    Public index As Integer = -1
    Public member As Person
    Public role As String = ""
End Class

Public Class Team
    Public Id As String = ""

    Public contactDetails As contactDetails
    Public hasMember As TeamMember
End Class

Public Class Address
    Public Country As String = ""
    Public ExtAdd As String = ""
    Public Locality As String = ""
    Public PCode As String = ""
    Public POBox As String = ""
    Public Region As String = ""
    Public Street As String = ""
    Public ContactType As ContactTypeTerm = ContactTypeTerm.notSpecified
End Class

Public Class ContactDetails
    Public Adr As Address
    Public Email As String = ""
    Public Geo As String = ""
    Public Logo As String = ""
    Public Note As String = ""
    Public Tel As String = ""
    Public Url As String = ""
End Class

