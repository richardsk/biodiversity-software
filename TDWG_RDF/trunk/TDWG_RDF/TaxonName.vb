
Public Enum KingdomTypeTerm
    Anamalia
    Archaebacteria
    Eubacteria
    Fungi
    Plantae
    Protista
    NotSpecified
End Enum


Public Enum TCSRank
    InfraspecificTaxon 'undefinedInfraSpecific
    Candidate 'candidate
    SpecialForm 'specialForm
    Subsubform
    Subform 'subForm
    Form 'form
    SubSubVariety
    SubVariety 'subVariety
    Variety 'variety
    PathoVariety 'pathoVariety
    BioVariety 'bioVariety
    Infraspecies 'infraSpecies
    SubspecificAggregate 'subSpecificAggregate
    Subspecies 'subSpecies
    Species 'species
    SpeciesAggregate 'speciesAggregate
    InfragenericTaxon 'undefined Infra Generic
    Subseries
    Series 'series
    Subsection 'sub Section
    Section
    Infragenus
    Subgenus
    Genus 'genus
    Infratribe
    Subtribe
    Tribe 'tribe
    Supertribe
    Infrafamily
    Subfamily
    Family
    Superfamily
    Infraorder
    Suborder
    Order
    Superorder
    Infraclass 'infraClass
    Subclass 'subClass
    Cls 'class
    Superclass 'superClass
    Infraphylum 'infraPhylum
    Infradivision
    Subphylum
    Subdivision
    Phylum
    Division
    Superphylum
    Superdivision
    Infrakingdom
    Subkingdom 'subKingdom
    Kingdom 'kingdom
    SuperKingdom 'super Kingdom
    Domain
    Empire
    SupragenericTaxon 'undefined supra generic rank
    Cultivar
    Convar
    Grex
    CultivarGroup
    GraftChimaera
    DenominationClass

    NotSpecified
End Enum

Public Enum TCSNomenclaturalCode
    Viral
    Bacteriological
    ICBN
    ICZN
    ICNCP 'cultivated plants
    NotSpecified
End Enum

Public Enum NomenclaturalNoteType
    NotSpecified
    SpellingCorrection
    BasedOn
    ConservedAgainst
    LaterHomonymOf
    Sanctioned
    ReplacementNameFor
    PublicationStatus
    'allo
    'allolecto
    'alloneo
End Enum

Public Class NomenclaturalNote
    Public subjectTaxonName As String = "" 'lsid of taxon name
    Public objectTaxonName As String = "" 'lsid of target taxon name
    Public note As String = ""
    Public ruleConsidered As String = ""
    Public noteType As NomenclaturalNoteType = NomenclaturalNoteType.NotSpecified
    Public code As TCSNomenclaturalCode
End Class

Public Enum NomenclaturalTypeType
    NotSpecified
    Allolectotype 'A paralectotype specimen that is the opposite sex of the lectotype. The term is not regulated by the ICZN. [Zoo.] 
    Alloneotype 'A paraneotype specimen that is the opposite sex of the neotype. The term is not regulated by the ICZN. [Zoo.] 
    Allotype 'A paratype specimen designated from the type series by the original author that is the opposite sex of the holotype. The term is not regulated by the ICZN. [Zoo.] 
    Cotype 'A deprecated term no longer recognized in the ICZN; formerly used for either syntype or paratype [see ICZN Recommendation 73E]. [Zoo.] 
    Epitype 'An epitype is a specimen or illustration selected to serve as an interpretative type when any kind of holotype, lectotype, etc. is demonstrably ambiguous and cannot be critically identified for purposes of the precise application of the name of a taxon (see Art. ICBN 9.7, 9.18). An epitype supplements, rather than replaces existing types. [Bot./Bio.] 
    ExEpitype 'A strain or cultivation derived from epitype material. Ex-types are not regulated by the botanical or zoological code. [Bot.] 
    ExHolotype 'A strain or cultivation derived from holotype material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    ExIsotype 'A strain or cultivation derived from isotype material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    ExLectotype 'A strain or cultivation derived from lectotype material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    ExNeotype 'A strain or cultivation derived from neotype material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    ExParatype 'A strain or cultivation derived from paratype material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    ExSyntype 'A strain or cultivation derived from neotype material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    ExType 'A strain or cultivation derived from some kind of type material. Ex-types are not regulated by the botanical or zoological code. [Zoo./Bot.] 
    Hapantotype 'One or more preparations of directly related individuals representing distinct stages in the life cycle, which together form the type in an extant species of protistan [ICZN Article 72.5.4]. A hapantotype, while a series of individuals, is a holotype that must not be restricted by lectotype selection. If an hapantotype is found to contain individuals of more than one species, however, components may be excluded until it contains individuals of only one species [ICZN Article 73.3.2]. [Zoo.] 
    Holotype 'The one specimen or other element used or designated by the original author at the time of publication of the original description as the nomenclatural type of a species or infraspecific taxon. A holotype may be 'explicit' if it is clearly stated in the originating publication or 'implicit' if it is the single specimen proved to have been in the hands of the originating author when the description was published. [Zoo./Bot./Bio.] 
    Iconotype 'A drawing or photograph (also called 'phototype') of a type specimen. Note: the term "iconotype" is not used in the ICBN, but implicit in, e. g., ICBN Art. 7 and 38. [Zoo./Bot.] 
    Isolectotype 'A duplicate of a lectotype, compare lectotype. [Bot.] 
    Isoneotype 'A duplicate of a neotype, compare neotype. [Bot.] 
    Isosyntype 'A duplicate of a syntype, compare isotype = duplicate of holotype. [Bot.] 
    Isotype 'An isotype is any duplicate of the holotype (i. e. part of a single gathering made by a collector at one time, from which the holotype was derived); it is always a specimen (ICBN Art. 7). [Bot.] 
    Lectotype 'A specimen or other element designated subsequent to the publication of the original description from the original material (syntypes or paratypes) to serve as nomenclatural type. Lectotype designation can occur only where no holotype was designated at the time of publication or if it is missing (ICBN Art. 7, ICZN Art. 74). [Zoo./Bot.] -- Note: the BioCode defines lectotype as selection from holotype material in cases where the holotype material contains more than one taxon [Bio.]. 
    Neotype 'A specimen designated as nomenclatural type subsequent to the publication of the original description in cases where the original holotype, lectotype, all paratypes and syntypes are lost or destroyed, or suppressed by the (botanical or zoological) commission on nomenclature. In zoology also called "Standard specimen" or "Representative specimen". [Zoo./Bot./Bio.] 
    NotAType 'For specimens erroneously labelled as types an explicit negative statement may be desirable. [General] 
    Paralectotype 'All of the specimens in the syntype series of a species or infraspecific taxon other than the lectotype itself. Also called "lectoparatype". [Zoo.] 
    Paraneotype 'All of the specimens in the syntype series of a species or infraspecific taxon other than the neotype itself. Also called "neoparatype". [Zoo.] 
    Paratype 'All of the specimens in the type series of a species or infraspecific taxon other than the holotype (and, in botany, isotypes). Paratypes must have been at the disposition of the author at the time when the original description was created and must have been designated and indicated in the publication. Judgment must be exercised on paratype status, for only rarely are specimens explicitly cited as paratypes, but usually as "specimens examined," "other material seen", etc. [Zoo./Bot.] 
    Plastoholotype 'A copy or cast of holotype material (compare Plastotype). 
    Plastoisotype 'A copy or cast of isotype material (compare Plastotype). 
    Plastolectotype 'A copy or cast of lectotype material (compare Plastotype). 
    Plastoneotype 'A copy or cast of neotype material (compare Plastotype). 
    Plastoparatype 'A copy or cast of paratype material (compare Plastotype). 
    Plastosyntype 'A copy or cast of syntype material (compare Plastotype). 
    Plastotype 'A copy or cast of type material, esp. relevant for fossil types. Not regulated by the botanical or zoological code (?). [Zoo./Bot.] 
    SecondaryType 'A referred, described, measured or figured specimen in the original publication (including a neo/lectotypification publication) that is not a primary type. [Zoo.]  
    SupplementaryType 'A referred, described, measured or figured specimen in a revision of a previously described taxon. [Zoo.] 
    Syntype 'One of the series of specimens used to describe a species or infraspecific taxon when neither a single holotype nor a lectotype has been designated. The syntypes collectively constitute the name-bearing type. [Zoo./Bot.] 
    Topotype 'One or more specimens collected at the same location as the type series (type locality), regardless of whether they are part of the type series. Topotypes are not regulated by the botanical or zoological code. Also called "locotype". [Zoo./Bot.] 
    Type 'a) A specimen designated or indicated any kind of type of a species or infraspecific taxon. If possible more specific type terms (holotype, syntype, etc.) should be applied. b) the type name of a name of higher rank for taxa above the species rank. [General] 

End Enum

Public Class NomenclaturalType
    Public NameLSID As String = ""
    Public SpecimenLSID As String = ""
    Public typeOfType As NomenclaturalTypeType = NomenclaturalTypeType.NotSpecified
End Class

Public Class TaxonName
    Public Id As String = ""

    'name
    Public nameComplete As String = ""
    Public uninomial As String = ""
    Public genusPart As String = ""
    Public infraGenericEpithet As String = ""
    Public specificEpithet As String = ""
    Public infraSpecificEpithet As String = ""
    Public cultivarNameGroup As String = ""
    Public authorship As String = ""
    Public authorTeam As String = ""
    Public basionymAuthorship As String = ""
    'Public basionymAuthorTeam As String = ""
    Public combinationAuthorship As String = ""
    'Public combinationAuthorTeam As String = ""
    Public publishedIn As String = "" 'actual citation, links to PublicationCitation objects are handled by the publication property
    Public year As String = ""
    Public microReference As String = ""
    Public rank As TCSRank = TCSRank.NotSpecified
    Public rankString As String = ""
    Public nomenclaturalCode As TCSNomenclaturalCode = TCSNomenclaturalCode.NotSpecified
    Public hasBasionym As String = "" 'lsid of basionym
    Public typifiedBy As NomenclaturalType
    Public isAnamorphic As New NullableBoolean
    Public referenceTo As String = ""

    'publication
    Public publication As PublicationCitation

    'other
    Public isReplacedBy As String = ""
    Public isRestricted As New NullableBoolean
    Public accessMessage As String = ""
    Public rightsOwner As String = ""
    Public fullName As String = ""

    Public Notes As New ArrayList 'of NomenclaturalNote

    Public AdditionalStatements As New SemWeb.MemoryStore
End Class
