Imports System.Xml

Namespace Query

    Public Class QueryController


        Public Shared Function DoQuery(ByVal model As RDFModel, ByVal terms As List(Of RDFQueryTerm)) As Xml.XmlDocument
            Dim doc As New XmlDocument
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", ""))
            doc.AppendChild(doc.CreateElement("DataSet"))

            Dim opt As DataAccess.Options = DataAccess.OptionData.GetOptions

            'query each relevant data source
            Dim dsDs As DataSet = DataAccess.DataSource.ListModelDataSources(model.ModelId)

            For Each row As DataRow In dsDs.Tables(0).Rows
                If row("Include") Then
                    Dim res As New QueryResource
                    res.Model = model
                    res.ClassName = row("QuerierClass").ToString
                    res.ResourceName = row("Name").ToString
                    res.ResourceUrl = row("Url").ToString

                    Dim cls As String = row("QuerierClass").ToString

                    'Dim o As Runtime.Remoting.ObjectHandle = Activator.CreateInstance(Type.GetType(cls))
                    'Dim querier As RDFQuerier = CType(o.Unwrap, RDFQuerier)

                    Dim querier As RDFQuerier = Activator.CreateInstance(Type.GetType(cls))

                    res.ResProtocol = querier.GetResourceProtocol()

                    Dim qDoc As XmlDocument = querier.DoQuery(res, terms)

                    If qDoc.DocumentElement IsNot Nothing Then
                        For Each c As XmlNode In qDoc.DocumentElement.ChildNodes
                            'todo auto resolve lsids to opt.AutoResolveLevel
                            doc.DocumentElement.AppendChild(doc.ImportNode(c, True))
                        Next
                    End If
                End If
            Next

            'save to cache?
            If opt.CacheQueryResults Then
                Try
                    RDFCache.SaveDataToCache(doc)
                Catch ex As Exception
                    TDWGRDFException.LogError(ex)
                End Try
            End If

            Return doc
        End Function

        Public Shared Function DoQueryDs(ByVal model As RDFModel, ByVal terms As List(Of RDFQueryTerm)) As DataSet
            Dim ds As New DataSet

            Dim doc As XmlDocument = DoQuery(model, terms)

            'load dataset
            If doc.OuterXml.Length > 0 Then
                Dim sr As New IO.StringReader(doc.OuterXml)
                Dim rdr As New Xml.XmlTextReader(sr)
                ds.ReadXml(rdr)
            End If

            Return ds
        End Function

        Public Shared Function ResolveID(ByVal model As RDFModel, ByVal Id As String) As XmlDocument
            Dim doc As New XmlDocument

            If model IsNot Nothing Then
                'try each data source to resolve id
                Dim dsDs As DataSet = DataAccess.DataSource.ListModelDataSources(model.ModelId)

                For Each row As DataRow In dsDs.Tables(0).Rows
                    If row("Include") Then
                        Dim res As New QueryResource
                        res.Model = model
                        res.ClassName = row("QuerierClass").ToString
                        res.ResourceName = row("Name").ToString
                        res.ResourceUrl = row("Url").ToString

                        Dim cls As String = row("QuerierClass").ToString

                        Dim querier As RDFQuerier = Activator.CreateInstance(Type.GetType(cls))

                        res.ResProtocol = querier.GetResourceProtocol()

                        doc = querier.ResolveId(res, Id)
                        If doc.DocumentElement IsNot Nothing Then Exit For 'found
                    End If
                Next
            End If

            Return doc
        End Function

        Public Shared Function ResolveLSID(ByVal lsid As String) As XmlDocument
            Dim doc As New XmlDocument

            If Utility.IsLSID(lsid) Then
                Dim ls As New LSIDQuerier
                doc = ls.ResolveId(Nothing, lsid)
            End If

            Return doc
        End Function

        Public Shared Function ResolveUri(ByVal uri As String) As XmlDocument
            Dim doc As New XmlDocument

            If System.Uri.IsWellFormedUriString(uri, UriKind.Absolute) Then
                Dim rs As New RESTQuerier
                doc = rs.ResolveId(Nothing, uri)
            End If

            Return doc
        End Function

        Public Shared Function ListIds(ByVal dataSourceId As Integer, ByVal model As RDFModel) As List(Of String)

            Dim ds As New RDFDataSource
            ds.Load(dataSourceId)

            Dim querier As RDFQuerier = Activator.CreateInstance(Type.GetType(ds.QuerierClass))

            Dim res As New QueryResource
            res.ClassName = ds.QuerierClass
            res.Model = model
            res.ResourceName = ds.Name
            res.ResourceUrl = ds.Url
            res.ResProtocol = model.ModelType

            Dim ids As List(Of String) = querier.ListIds(res)
            Return ids
        End Function

    End Class

End Namespace
