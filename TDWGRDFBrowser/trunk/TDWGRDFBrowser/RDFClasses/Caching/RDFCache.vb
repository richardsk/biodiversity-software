Imports System.Xml
Imports System.Configuration
Imports SemWeb
Imports System.IO

Public Class RDFCache

    Public Shared Sub SaveDataToCache(ByVal doc As XmlDocument)
        'load current cache
        Dim file As String = ConfigurationManager.AppSettings.Get("HomeDir") + "\Cache\rdfcache.n3"

        Dim cache As New MemoryStore()
        If System.IO.File.Exists(file) Then
            Dim crdr As New N3Reader(file)
            cache.Import(crdr)
            crdr.Close()
        Else
            Dim swr As StreamWriter = System.IO.File.CreateText(file)
            swr.Close()
        End If

        'load new data
        Dim rdr As New RdfXmlReader(doc)
        Dim data As New MemoryStore()
        data.Import(rdr)


        'insert / update triples
        For Each st As Statement In data.Statements
            Dim sr As SemWeb.SelectResult = cache.Select(st)
            If sr.StatementCount > 0 Then
                'update
                cache.Replace(sr.ToArray()(0), st)
            Else
                'insert
                cache.Add(st)
            End If
        Next

        'save
        Dim wr As New N3Writer(file)
        cache.Write(wr)
        wr.Close()

    End Sub

End Class
