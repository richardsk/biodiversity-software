Imports TDWG_RDF
Imports SemWeb

Public Class TaxonNameList

    Dim tnList As New System.Collections.Generic.List(Of TaxonName)

    Public Sub AddName(ByVal tn As TaxonName)
        tnList.Add(tn)
    End Sub

    Public Function GetNameAt(ByVal index As Integer) As TaxonName
        Return tnList(index)
    End Function

    Public Function Count() As Integer
        Return tnList.Count
    End Function

    Public Function GetNameListRDF() As String
        Dim ms As New MemoryStore

        Dim tnRdf As New TaxonNameRDF
        For Each tn As TaxonName In tnList
            Dim bn As Resource = Nothing
            Dim tmpMs As MemoryStore = tnRdf.GetNameMemoryStore(tn, bn, True)
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
