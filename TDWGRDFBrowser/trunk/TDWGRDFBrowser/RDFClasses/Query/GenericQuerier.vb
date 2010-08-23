Namespace Query

    Public Class GenericQuerier

        Public Function ResolveGenericHTTP(ByVal uri As String, ByVal contentType As String) As String
            Dim req As System.Net.HttpWebRequest = Net.HttpWebRequest.Create(uri)
            req.Accept = contentType
            req.Timeout = 200000

            req.IfModifiedSince = "2009-01-01"

            Dim rdr As New IO.StreamReader(req.GetResponse().GetResponseStream())

            Dim res As String = rdr.ReadToEnd()

            Return res
        End Function

    End Class
End Namespace
