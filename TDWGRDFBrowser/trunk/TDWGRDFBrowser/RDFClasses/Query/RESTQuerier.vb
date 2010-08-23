Imports System.Xml
Imports LSIDClient

Namespace Query

    Public Class RESTQuerier
        Implements RDFQuerier

        Public Function DoQuery(ByVal resource As QueryResource, ByVal terms As System.Collections.Generic.List(Of RDFQueryTerm)) As System.Xml.XmlDocument Implements RDFQuerier.DoQuery
            'todo REST search??
            Return New XmlDocument
        End Function

        Public Function GetSchema() As System.Xml.XmlDocument Implements RDFQuerier.GetSchema
            'todo - do we need this?
            Return New XmlDocument
        End Function

        Public Function ListIds(ByVal resource As QueryResource) As System.Collections.Generic.List(Of String) Implements RDFQuerier.ListIds
            'todo REST harvest??
            Return New List(Of String)
        End Function

        Public Function ResolveId(ByVal resource As QueryResource, ByVal Id As String) As System.Xml.XmlDocument Implements RDFQuerier.ResolveId
            Dim req As System.Net.HttpWebRequest = Net.HttpWebRequest.Create(Id)
            req.Accept = "application/rdf+xml"
            
            Dim rdr As New IO.StreamReader(req.GetResponse().GetResponseStream())

            Dim xml As String = rdr.ReadToEnd()

            Dim doc As New XmlDocument

            Try
                doc.LoadXml(xml)
            Catch ex As Exception
                RDFClasses.TDWGRDFException.LogError(ex)
            End Try

            Return doc
        End Function

        Public Function GetResourceProtocol() As ResourceProtocol Implements RDFQuerier.GetResourceProtocol
            Return ResourceProtocol.URL
        End Function


    End Class

End Namespace
