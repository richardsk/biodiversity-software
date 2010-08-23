Imports System.Xml
Imports System.IO
Imports System.Web

Imports TapirDotNET

Namespace Query

    Public Class TAPIRQuerier
        Implements RDFQuerier

        Public Function DoQuery(ByVal resource As QueryResource, ByVal terms As System.Collections.Generic.List(Of RDFQueryTerm)) As System.Xml.XmlDocument Implements RDFQuerier.DoQuery
            Dim doc As New XmlDocument

            Try
                Dim query As String = GetModelTapirQuery(resource.Model, terms)

                Dim qu As String = resource.ResourceUrl + "?request=" + HttpUtility.UrlEncode(query)

                Dim xml As String = TAPIRClient.QueryTapirResource(resource.ResourceUrl, qu, query, "GET")

                doc.LoadXml(xml)

            Catch ex As Exception

            End Try

            Return doc
        End Function

        Public Function GetSchema() As System.Xml.XmlDocument Implements RDFQuerier.GetSchema
            'todo - do we need this?
            Return New XmlDocument
        End Function

        Public Function ListIds(ByVal resource As QueryResource) As System.Collections.Generic.List(Of String) Implements RDFQuerier.ListIds
            Dim ids As New List(Of String)

            Dim query As String = GetInventoryXml(resource.Model)

            Dim qu As String = resource.ResourceUrl + "?request=" + HttpUtility.UrlEncode(query)

            Dim xml As String = TAPIRClient.QueryTapirResource(resource.ResourceUrl, qu, query, "GET")

            Dim doc As New XmlDocument
            doc.LoadXml(xml)

            Dim nsMgr As New XmlNamespaceManager(doc.NameTable)
            nsMgr.AddNamespace("tp", "http://rs.tdwg.org/tapir/1.0")
            Dim nodes As XmlNodeList = doc.SelectNodes("/tp:response/tp:inventory/tp:record/tp:value", nsMgr)
            For Each n As XmlNode In nodes
                ids.Add(n.InnerText)
            Next

            Return ids
        End Function

        Public Function ResolveId(ByVal resource As QueryResource, ByVal Id As String) As System.Xml.XmlDocument Implements RDFQuerier.ResolveId
            Return New XmlDocument
        End Function

        Public Function GetResourceProtocol() As ResourceProtocol Implements RDFQuerier.GetResourceProtocol
            Return ResourceProtocol.TAPIR
        End Function

#Region "Tapir functions"

        Private Function GetInventoryXml(ByVal model As RDFModel) As String
            Dim tmplFile As String = System.Configuration.ConfigurationManager.AppSettings.Get("HomeDir") _
                            + "\templates\" + model.ModelHarvestTemplate
            Dim res As String = File.ReadAllText(tmplFile)

            res = res.Replace("[CONCEPT]", model.ModelGUIDElement)
            '2005-11-11T12:23:56.023+01:00
            res = res.Replace("[TIME]", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.ffz"))

            Return res
        End Function

        Private Function GetModelTapirQuery(ByVal model As RDFModel, ByVal terms As List(Of RDFQueryTerm)) As String
            Dim tmplFile As String = System.Configuration.ConfigurationManager.AppSettings.Get("HomeDir") _
                + "\templates\" + model.ModelQueryTemplate
            Dim res As String = File.ReadAllText(tmplFile)

            Dim qt As String = GetQueryFilterXml(terms)
            res = res.Replace("[QUERY_FILTER]", qt)
            '2005-11-11T12:23:56.023+01:00
            res = res.Replace("[TIME]", DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.ffz"))

            Return res
        End Function

        Private Function GetQueryFilterXml(ByVal terms As List(Of RDFQueryTerm)) As String
            Dim xml As String = ""

            Dim isAnd As Boolean = False
            For Each t As RDFQueryTerm In terms
                If xml = "" Then
                    'first term, add <and>, or <or>
                    If terms.Count > 1 Then
                        'base and / or on the second query term
                        Dim t2 As RDFQueryTerm = terms(1)
                        If t2.AndOr = QueryAndOrEnum._None Or t2.AndOr = QueryAndOrEnum._And Then
                            xml += "<and>" + Environment.NewLine
                            isAnd = True
                        Else
                            xml += "<or>" + Environment.NewLine
                            isAnd = False
                        End If
                    Else
                        If t.Exact Then
                            xml += "<equals>" + Environment.NewLine
                        Else
                            xml += "<like>" + Environment.NewLine
                        End If
                        xml += "<concept id='" + t.Field + "'/>" + Environment.NewLine
                        xml += "<literal value='" + t.Value + "'/>" + Environment.NewLine
                        If t.Exact Then
                            xml += "</equals>" + Environment.NewLine
                        Else
                            xml += "</like>" + Environment.NewLine
                        End If
                    End If
                Else
                    'not first term
                    'if there are following terms, then embed in a <and> or <or>
                    Dim hasMore As Boolean = (terms.Count > terms.IndexOf(t) + 1)
                    If hasMore Then
                        Dim t2 As RDFQueryTerm = terms(terms.IndexOf(t) + 1)
                        If t2.AndOr = QueryAndOrEnum._None Or t2.AndOr = QueryAndOrEnum._And Then
                            xml += "<and>" + Environment.NewLine
                            isAnd = True
                        Else
                            xml += "<or>" + Environment.NewLine
                            isAnd = False
                        End If
                    End If

                    'add search val
                    If t.Exact Then
                        xml += "<equals>" + Environment.NewLine
                    Else
                        xml += "<like>" + Environment.NewLine
                    End If
                    xml += "<concept id='" + t.Field + "'/>" + Environment.NewLine
                    xml += "<literal value='" + t.Value + "'/>" + Environment.NewLine
                    If t.Exact Then
                        xml += "</equals>" + Environment.NewLine
                    Else
                        xml += "</like>" + Environment.NewLine
                    End If

                    If Not hasMore Then
                        'no more terms
                        'end the and/or
                        If isAnd Then
                            xml += "</and>" + Environment.NewLine
                        Else
                            xml += "</or>" + Environment.NewLine
                        End If
                    End If

                End If

            Next

            Return xml
        End Function
#End Region

    End Class

End Namespace
