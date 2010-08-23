Imports TDWG_RDF
Imports SemWeb

Public Class TaxonConceptList

    Dim tcList As New System.Collections.Generic.List(Of TaxonConcept)

    Public Sub AddConcept(ByVal tc As TaxonConcept)
        tcList.Add(tc)
    End Sub

    Public Function GetConceptAt(ByVal index As Integer) As TaxonConcept
        Return tcList(index)
    End Function


    Public Function GetConceptListRDF() As String
        Dim ms As New MemoryStore

        Dim tnRdf As New TaxonNameRDF
        For Each tc As TaxonConcept In tcList
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = tnRdf.GetConceptMemoryStore(tc, bn, True)
            If tmpMs.StatementCount > 0 Then
                For Each s As Statement In tmpMs.Statements
                    If Not ms.Contains(s) Then ms.Add(s)
                Next
            End If
        Next

        Dim sw As New System.IO.StringWriter
        Dim xw As New System.Xml.XmlTextWriter(sw)
        xw.Formatting = System.Xml.Formatting.Indented
        xw.Namespaces = True

        Dim wr As New RdfXmlWriter(xw)
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsTCS, "tn")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsConcept, "tc")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsRank, "trank")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsTCom, "tcom")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsPub, "tpub")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsDC, "dc")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsDCTerm, "dcterms")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsOwl, "owl")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsRDF, "rdf")
        wr.Namespaces.AddNamespace(TaxonNameRDF.nsSpecimen, "tto")

        ms.Write(wr)
        wr.Close()

        Return sw.ToString
    End Function


End Class
