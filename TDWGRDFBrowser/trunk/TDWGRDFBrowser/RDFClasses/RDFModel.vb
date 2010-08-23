Imports System.Xml
Imports System.Net
Imports System.Configuration
Imports System.IO
Imports System.Xml.XPath

Public Class RDFModel

    Public Enum RDFModelType
        OWL
        RDFS
        CNS 'TAPIR concept name schema
        XSD
    End Enum

    Public ModelId As String
    Public ModelName As String
    Public ModelUrl As String
    Public ModelType As RDFModelType = RDFModelType.OWL
    Public ModelQueryTemplate As String
    Public ModelGUIDElement As String
    Public ModelHarvestTemplate As String

    Sub New()
    End Sub

    Sub New(ByVal Id As String, ByVal name As String, ByVal url As String, ByVal type As String, ByVal template As String, ByVal guidElement As String, ByVal harvestTemplate As String)
        ModelId = Id
        ModelName = name
        ModelUrl = url
        ModelType = RDFModelType.Parse(GetType(RDFModelType), type)
        ModelQueryTemplate = template
        ModelGUIDElement = guidElement
        ModelHarvestTemplate = harvestTemplate
    End Sub

    Public Sub Load(ByVal modelId As Integer)
        Dim ds As DataSet = DataAccess.RDFModel.GetModel(modelId)

        If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            Dim row As DataRow = ds.Tables(0).Rows(0)

            modelId = modelId
            ModelName = row("Name").ToString
            ModelUrl = row("Url").ToString
            ModelType = RDFModelType.Parse(GetType(RDFModelType), row("Type").ToString)
            ModelQueryTemplate = row("QueryTemplateUrl").ToString
            ModelGUIDElement = row("GUIDElement").ToString
            ModelHarvestTemplate = row("HarvestTemplateUrl").ToString
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return ModelName
    End Function

    Public Shared Function GetModelConcepts(ByVal modelUrl As String, ByVal modelType As RDFModelType) As List(Of RDFConcept)
        If modelType = RDFModelType.OWL Then
            Dim doc As XmlDocument = Utility.GetWebXmlDocument(modelUrl)
            Return GetRDFModelConcepts(doc)
        ElseIf modelType = RDFModelType.CNS Then
            Dim doc As String = Utility.GetWebDocument(modelUrl)
            Return GetCNSModelConcepts(doc)
        ElseIf modelType = RDFModelType.XSD Then
            Dim doc As XmlDocument = Utility.GetWebXmlDocument(modelUrl)
            Return GetXSDModelConcepts(doc)
        End If
        Return Nothing
    End Function

    Private Shared Function GetXSDModelConcepts(ByVal modelDoc As XmlDocument) As List(Of RDFConcept)
        Dim cs As New List(Of RDFConcept)

        Dim nav As XPathNavigator = modelDoc.CreateNavigator()
        Dim itr As XPathNodeIterator = nav.SelectDescendants(XPathNodeType.Element, False)

        Dim tns As String = modelDoc.DocumentElement.GetAttribute("targetNamespace")
        If tns = "" Then tns = modelDoc.DocumentElement.GetAttribute("xmlns")

        While itr.MoveNext
            If itr.Current.LocalName.ToLower = "element" Then
                Dim path As String = "" 'todo get path to node - do we need this?
                cs.Add(New RDFConcept("", itr.Current.GetAttribute("name", ""), tns, path))
            End If

        End While

        Return cs
    End Function

    Private Shared Function GetRDFModelConcepts(ByVal modelDoc As XmlDocument) As List(Of RDFConcept)
        Dim cs As New List(Of RDFConcept)

        Dim nav As XPathNavigator = modelDoc.CreateNavigator()
        Dim itr As XPathNodeIterator = nav.SelectDescendants(XPathNodeType.Element, False)

        Dim tns As String = modelDoc.DocumentElement.GetAttribute("targetNamespace")
        If tns = "" Then tns = modelDoc.DocumentElement.GetAttribute("xmlns")

        While itr.MoveNext
            If itr.Current.LocalName.ToLower = "objectproperty" Or _
                itr.Current.LocalName.ToLower = "datatypeproperty" Or _
                itr.Current.LocalName.ToLower = "property" Then

                Dim path As String = "" 'todo get path to node - do we need this?
                cs.Add(New RDFConcept("", itr.Current.GetAttribute("ID", RDFNamespaces.nsRDF), tns, path))
            End If

        End While

        Return cs
    End Function

    Private Shared Function GetCNSModelConcepts(ByVal modelTxt As String) As List(Of RDFConcept)
        Dim cs As New List(Of RDFConcept)

        Dim inAliases As Boolean = False
        Dim inSource As Boolean = False
        Dim ns As String = ""

        Dim rdr As New StringReader(modelTxt)
        While rdr.Peek <> 0
            Dim line As String = rdr.ReadLine
            If line Is Nothing Then Exit While

            If line.Trim.StartsWith("[concept_source]") Then
                inSource = True
            ElseIf line.Trim.StartsWith("[aliases]") Then
                inSource = False
                inAliases = True
            ElseIf inSource Then
                If line.Trim.StartsWith("namespace") Then
                    Dim parts As String() = line.Split("=")
                    'TODO ?? ns = parts(1).Trim
                End If
            ElseIf inAliases Then
                Dim parts As String() = line.Split("=")
                If parts IsNot Nothing AndAlso parts.Length > 1 Then
                    Dim cn As New RDFConcept("", parts(1).Trim, ns, "")
                    cs.Add(cn)
                End If
            End If
        End While

        Return cs
    End Function

End Class
