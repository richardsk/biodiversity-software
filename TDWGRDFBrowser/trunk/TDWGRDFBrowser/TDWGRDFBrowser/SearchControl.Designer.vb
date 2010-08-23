<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SearchControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label1 = New System.Windows.Forms.Label
        Me.RDFModelCombo = New System.Windows.Forms.ComboBox
        Me.SearchFieldsGrid = New System.Windows.Forms.DataGridView
        Me.AdnOr = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Field = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Exact = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ResultsGrid = New System.Windows.Forms.DataGridView
        Me.ViewCol = New System.Windows.Forms.DataGridViewLinkColumn
        Me.SearchButton = New System.Windows.Forms.Button
        CType(Me.SearchFieldsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ResultsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Search using RDF model"
        '
        'RDFModelCombo
        '
        Me.RDFModelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RDFModelCombo.FormattingEnabled = True
        Me.RDFModelCombo.Location = New System.Drawing.Point(134, 3)
        Me.RDFModelCombo.Name = "RDFModelCombo"
        Me.RDFModelCombo.Size = New System.Drawing.Size(378, 21)
        Me.RDFModelCombo.TabIndex = 1
        '
        'SearchFieldsGrid
        '
        Me.SearchFieldsGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SearchFieldsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.SearchFieldsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SearchFieldsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.AdnOr, Me.Field, Me.Value, Me.Exact})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.SearchFieldsGrid.DefaultCellStyle = DataGridViewCellStyle2
        Me.SearchFieldsGrid.Location = New System.Drawing.Point(3, 58)
        Me.SearchFieldsGrid.Name = "SearchFieldsGrid"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SearchFieldsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.SearchFieldsGrid.Size = New System.Drawing.Size(738, 135)
        Me.SearchFieldsGrid.TabIndex = 2
        '
        'AdnOr
        '
        Me.AdnOr.DataPropertyName = "AndOr"
        Me.AdnOr.HeaderText = "And / Or"
        Me.AdnOr.Items.AddRange(New Object() {"And", "Or"})
        Me.AdnOr.Name = "AdnOr"
        Me.AdnOr.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AdnOr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Field
        '
        Me.Field.DataPropertyName = "Field"
        Me.Field.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Field.HeaderText = "Field"
        Me.Field.Name = "Field"
        Me.Field.Width = 380
        '
        'Value
        '
        Me.Value.DataPropertyName = "Value"
        Me.Value.HeaderText = "Search Value"
        Me.Value.Name = "Value"
        '
        'Exact
        '
        Me.Exact.DataPropertyName = "Exact"
        Me.Exact.HeaderText = "Exact"
        Me.Exact.Name = "Exact"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Search fields"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 235)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Results"
        '
        'ResultsGrid
        '
        Me.ResultsGrid.AllowUserToAddRows = False
        Me.ResultsGrid.AllowUserToDeleteRows = False
        Me.ResultsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.ResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ResultsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ViewCol})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultsGrid.DefaultCellStyle = DataGridViewCellStyle5
        Me.ResultsGrid.Location = New System.Drawing.Point(3, 251)
        Me.ResultsGrid.Name = "ResultsGrid"
        Me.ResultsGrid.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.ResultsGrid.Size = New System.Drawing.Size(738, 242)
        Me.ResultsGrid.TabIndex = 5
        '
        'ViewCol
        '
        Me.ViewCol.HeaderText = ""
        Me.ViewCol.Name = "ViewCol"
        Me.ViewCol.ReadOnly = True
        Me.ViewCol.Text = ""
        '
        'SearchButton
        '
        Me.SearchButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchButton.BackColor = System.Drawing.Color.AliceBlue
        Me.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SearchButton.Location = New System.Drawing.Point(656, 199)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 6
        Me.SearchButton.Text = "Search"
        Me.SearchButton.UseVisualStyleBackColor = False
        '
        'SearchControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.ResultsGrid)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.SearchFieldsGrid)
        Me.Controls.Add(Me.RDFModelCombo)
        Me.Controls.Add(Me.Label1)
        Me.Name = "SearchControl"
        Me.Size = New System.Drawing.Size(744, 496)
        CType(Me.SearchFieldsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ResultsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RDFModelCombo As System.Windows.Forms.ComboBox
    Friend WithEvents SearchFieldsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ResultsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents AdnOr As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Field As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Exact As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ViewCol As System.Windows.Forms.DataGridViewLinkColumn

End Class
