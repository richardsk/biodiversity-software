Imports System.Xml

Public Class SearchControl

    Private _loading As Boolean = False
    Private _searchFieldsTable As DataTable

    Private Sub SearchControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not DesignMode Then
            _loading = True

            Try
                Dim ds As DataSet = DataAccess.Config.ListRDFModels()
                RDFModelCombo.DataSource = ds.Tables(0)
                RDFModelCombo.DisplayMember = "Name"

                _searchFieldsTable = New DataTable
                _searchFieldsTable.Columns.Add("AndOr")
                _searchFieldsTable.Columns.Add("Field", GetType(RDFClasses.RDFConcept))
                _searchFieldsTable.Columns.Add("Value")
                _searchFieldsTable.Columns.Add("Exact", GetType(Boolean))

                SearchFieldsGrid.DataSource = _searchFieldsTable

                AddHandler SearchFieldsGrid.DataError, AddressOf Me.SearchFiledsGrid_DataError
            Catch ex As Exception
                RDFClasses.TDWGRDFException.LogError(ex)
                MsgBox(ex.Message)
            End Try

            _loading = False

            RDFModelCombo.SelectedIndex = -1
        End If
    End Sub

    Protected Sub SearchFiledsGrid_DataError(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs)
        
    End Sub

    Private Sub RDFModelCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RDFModelCombo.SelectedIndexChanged
        If Not _loading Then
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Try
                _searchFieldsTable.Rows.Clear()

                Dim cbc As DataGridViewComboBoxColumn = CType(SearchFieldsGrid.Columns("Field"), DataGridViewComboBoxColumn)
                cbc.Items.Clear()

                If RDFModelCombo.SelectedItem IsNot Nothing Then
                    Dim row As DataRowView = RDFModelCombo.SelectedItem

                    Dim docType As RDFClasses.RDFModel.RDFModelType = RDFClasses.RDFModel.RDFModelType.Parse(GetType(RDFClasses.RDFModel.RDFModelType), row("Type").ToString)
                    Dim cs As List(Of RDFClasses.RDFConcept) = RDFClasses.RDFModel.GetModelConcepts(row("Url").ToString, docType)

                    cbc.ValueType = GetType(RDFClasses.RDFConcept)
                    cbc.ValueMember = "Value"
                    cbc.DisplayMember = "Text"

                    For Each concept As RDFClasses.RDFConcept In cs
                        cbc.Items.Add(New ComboItem(concept, concept.ToString()))
                    Next

                End If

            Catch ex As Exception
                RDFClasses.TDWGRDFException.LogError(ex)
                MsgBox(ex.Message)
            End Try
            Windows.Forms.Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub SearchFieldsGrid_RowsAdded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles SearchFieldsGrid.RowsAdded
        If SearchFieldsGrid.RowCount = 1 Then
            SearchFieldsGrid.Rows(0).Cells(0).ReadOnly = True
        End If
    End Sub


    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click

        'Dim rdf As XmlDocument = RDFClasses.Query.QueryController.ResolveID("urn:lsid:sp2000.cs.cf.ac.uk:AC2007:159044")

        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            'todo check search fields are correct

            Dim modelRow As DataRowView = RDFModelCombo.SelectedItem

            Dim terms As New List(Of RDFClasses.Query.RDFQueryTerm)

            For Each row As DataRow In _searchFieldsTable.Rows
                Dim qt As New RDFClasses.Query.RDFQueryTerm

                If row("AndOr").ToString() = "And" Then qt.AndOr = RDFClasses.Query.QueryAndOrEnum._And
                If row("AndOr").ToString() = "Or" Then qt.AndOr = RDFClasses.Query.QueryAndOrEnum._Or

                qt.Field = row("Field").ToString
                qt.Value = row("Value").ToString
                If Not row.IsNull("Exact") Then qt.Exact = row("Exact")

                terms.Add(qt)
            Next

            Dim model As New RDFClasses.RDFModel(modelRow("ModelId").ToString, modelRow("Name").ToString, modelRow("Url").ToString, modelRow("Type").ToString, modelRow("QueryTemplateUrl").ToString, modelRow("GUIDElement").ToString, modelRow("HarvestTemplateUrl").ToString)

            Dim ds As DataSet = RDFClasses.Query.QueryController.DoQueryDs(model, terms)
            If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                Windows.Forms.Cursor.Current = Cursors.Default
                MsgBox("No records found")
            ElseIf ds.Tables.Contains("error") Then
                Dim msg As String = "ERROR : " + ds.Tables("error").Rows(0)("error_Text").ToString
                If ds.Tables.Contains("diagnostic") Then
                    msg += " : " + ds.Tables("diagnostic").Rows(0)("diagnostic_Text").ToString
                End If
                MsgBox(msg)
            Else
                ResultsGrid.DataSource = ds.Tables(0)

                'set links for LSID rows
                For Each r As DataGridViewRow In ResultsGrid.Rows
                    If r.Cells("about").Value IsNot Nothing AndAlso RDFClasses.Utility.IsLSID(r.Cells("about").Value.ToString) Then
                        r.Cells("ViewCol").Value = "View"
                    End If
                Next
            End If

        Catch ex As Exception
            RDFClasses.TDWGRDFException.LogError(ex)
            MsgBox(ex.Message)
        End Try
        Windows.Forms.Cursor.Current = Cursors.Default
    End Sub


    Private Sub ResultsGrid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ResultsGrid.CellContentClick
        If ResultsGrid.Columns(e.ColumnIndex).Name = "ViewCol" Then
            'get rdf
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim rdf As XmlDocument = RDFClasses.Query.QueryController.ResolveLSID(ResultsGrid.CurrentRow.Cells("about").Value.ToString)
            Windows.Forms.Cursor.Current = Cursors.Default
            If rdf.DocumentElement IsNot Nothing Then
                Dim det As New DetailsForm
                det.DetailsControl1.RdfDocument = rdf

                det.ShowDialog()
            Else
                MsgBox("Failed to resolve data.")
            End If
        End If
    End Sub
End Class
