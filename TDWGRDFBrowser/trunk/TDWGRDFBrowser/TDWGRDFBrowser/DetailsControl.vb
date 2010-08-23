Imports System.Xml

Public Class DetailsControl

    Private Class BrowseItem
        Public Lsid As String
        Public Doc As XmlDocument
    End Class

    Private _rdfDoc As XmlDocument
    Private _rdfId As String
    Private _browseHistory As New Stack

    Public Property RdfDocument() As XmlDocument
        Get
            Return _rdfDoc
        End Get
        Set(ByVal value As XmlDocument)
            _rdfDoc = value
            Initialise()
        End Set
    End Property

    Public Sub Initialise()
        'DetailsGrid.DataSource = Nothing
        DetailsGrid.AutoGenerateColumns = False

        'show the properties of the top level rdf element
        'add browser links for browsable values

        DetailsGrid.Nodes.Clear()
        _rdfId = ""

        If _rdfDoc IsNot Nothing Then
            If _rdfDoc.DocumentElement.ChildNodes.Count > 0 Then
                If _rdfDoc.DocumentElement.ChildNodes(0).Attributes IsNot Nothing AndAlso _rdfDoc.DocumentElement.ChildNodes(0).Attributes("rdf:about") IsNot Nothing Then
                    _rdfId = _rdfDoc.DocumentElement.ChildNodes(0).Attributes("rdf:about").InnerText
                End If
            End If

            Dim bi As New BrowseItem
            bi.Lsid = _rdfId
            bi.Doc = _rdfDoc
            _browseHistory.Push(bi)
            BackButton.Enabled = (_browseHistory.Count > 1)

            For Each node As XmlNode In _rdfDoc.DocumentElement.ChildNodes
                If node IsNot Nothing Then
                    AddNode(DetailsGrid.Nodes, node)
                End If
            Next

            DetailsGrid.Nodes(0).Expand()

        End If
    End Sub

    Private Function HasChildElements(ByVal node As XmlNode) As Boolean
        If Not node.HasChildNodes Then Return False

        Dim hasEl As Boolean = False
        For Each cn As XmlNode In node.ChildNodes
            If cn.NodeType = XmlNodeType.Element Then
                hasEl = True
                Exit For
            End If
        Next

        Return hasEl
    End Function

    Private Sub AddNode(ByVal parent As AdvancedDataGridView.TreeGridNodeCollection, ByVal node As XmlNode)
        Dim val As String = "..."
        Dim hasChild As Boolean = HasChildElements(node)
        If node.Attributes IsNot Nothing AndAlso node.Attributes("rdf:about") IsNot Nothing Then
            val = node.Attributes("rdf:about").InnerText
        ElseIf Not hasChild Then
            'rdf resource?
            If node.Attributes IsNot Nothing AndAlso node.Attributes("rdf:resource") IsNot Nothing Then
                val = node.Attributes("rdf:resource").InnerText
            Else
                val = node.InnerText
            End If
        End If

        Dim pn As AdvancedDataGridView.TreeGridNode = parent.Add(node.LocalName, val)

        If RDFClasses.Utility.IsLSID(val) Or System.Uri.IsWellFormedUriString(val, UriKind.Absolute) Then
            Dim fn As New Font(DetailsGrid.Font.FontFamily, DetailsGrid.Font.Size, FontStyle.Underline)
            pn.Cells(1).Style.Font = fn
            pn.Cells(1).Style.ForeColor = Color.Blue
        End If

        If hasChild Then
            For Each cn As XmlNode In node.ChildNodes
                AddNode(pn.Nodes, cn)
            Next
        End If
    End Sub

    Private Sub ViewGraphLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ViewGraphLink.LinkClicked
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim img As Drawing.Image = RDFClasses.RDFValidator.GetValidatorGraphImage(_rdfDoc.OuterXml)
            Windows.Forms.Cursor.Current = Cursors.Default

            Dim iForm As New ImageForm
            iForm.GraphImage = img
            iForm.ShowDialog()

        Catch ex As Exception
            RDFClasses.TDWGRDFException.LogError(ex)

        End Try
    End Sub

    Private Sub ViewRDFLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ViewRDFLink.LinkClicked
        If _rdfDoc IsNot Nothing Then
            Dim rdfForm As New RDFSourceForm
            rdfForm.RDFDocument = _rdfDoc
            rdfForm.ShowDialog()
        End If
    End Sub

    Private Sub DetailsGrid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DetailsGrid.CellContentClick
        If e.ColumnIndex = 1 Then
            Dim val As String = DetailsGrid.CurrentCell.Value.ToString
            If (RDFClasses.Utility.IsLSID(val) Or System.Uri.IsWellFormedUriString(val, UriKind.Absolute)) AndAlso val <> _rdfId Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                Try
                    Dim rdf As XmlDocument = Nothing
                    If RDFClasses.Utility.IsLSID(val) Then
                        rdf = RDFClasses.Query.QueryController.ResolveLSID(val)
                    Else
                        rdf = RDFClasses.Query.QueryController.ResolveUri(val)
                    End If
                    RdfDocument = rdf
                Catch ex As Exception
                    MsgBox("Failed to get RDF document")
                    RDFClasses.TDWGRDFException.LogError(ex)
                End Try
                Windows.Forms.Cursor.Current = Cursors.Default
            End If
        End If
    End Sub

    Private Sub BackButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackButton.Click
        Dim last As Object = _browseHistory.Pop() 'remove current item
        Dim prev As Object = _browseHistory.Peek() 'get previous item
        If prev IsNot Nothing Then
            Dim bi As BrowseItem = prev
            RdfDocument = bi.Doc
            _browseHistory.Pop() 'remove last item it has just re-added in Initialise
        End If
        If _browseHistory.Count = 1 Then BackButton.Enabled = False
    End Sub

End Class
