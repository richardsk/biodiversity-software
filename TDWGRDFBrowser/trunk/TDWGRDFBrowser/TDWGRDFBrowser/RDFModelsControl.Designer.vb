<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RDFModelsControl
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
        Me.ModelsGrid = New System.Windows.Forms.DataGridView
        Me.SaveButton = New System.Windows.Forms.Button
        Me.TypeCol = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.NameCol = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UrlCol = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TemplCol = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GUIDElement = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.HarvestTempl = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.ModelsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ModelsGrid
        '
        Me.ModelsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModelsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.ModelsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ModelsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TypeCol, Me.NameCol, Me.UrlCol, Me.TemplCol, Me.GUIDElement, Me.HarvestTempl})
        Me.ModelsGrid.Location = New System.Drawing.Point(3, 3)
        Me.ModelsGrid.Name = "ModelsGrid"
        Me.ModelsGrid.Size = New System.Drawing.Size(1113, 213)
        Me.ModelsGrid.TabIndex = 11
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SaveButton.Location = New System.Drawing.Point(1041, 281)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 16
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'TypeCol
        '
        Me.TypeCol.DataPropertyName = "Type"
        Me.TypeCol.FillWeight = 80.0!
        Me.TypeCol.HeaderText = "Type"
        Me.TypeCol.Name = "TypeCol"
        '
        'NameCol
        '
        Me.NameCol.DataPropertyName = "Name"
        Me.NameCol.HeaderText = "Name"
        Me.NameCol.Name = "NameCol"
        '
        'UrlCol
        '
        Me.UrlCol.DataPropertyName = "Url"
        Me.UrlCol.FillWeight = 150.0!
        Me.UrlCol.HeaderText = "Structure Definition Url"
        Me.UrlCol.Name = "UrlCol"
        '
        'TemplCol
        '
        Me.TemplCol.DataPropertyName = "QueryTemplateUrl"
        Me.TemplCol.FillWeight = 150.0!
        Me.TemplCol.HeaderText = "Query Template Url"
        Me.TemplCol.Name = "TemplCol"
        Me.TemplCol.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TemplCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'GUIDElement
        '
        Me.GUIDElement.DataPropertyName = "GUIDElement"
        Me.GUIDElement.FillWeight = 80.0!
        Me.GUIDElement.HeaderText = "GUID Element"
        Me.GUIDElement.Name = "GUIDElement"
        '
        'HarvestTempl
        '
        Me.HarvestTempl.DataPropertyName = "HarvestTemplateUrl"
        Me.HarvestTempl.FillWeight = 80.0!
        Me.HarvestTempl.HeaderText = "Harvest Template Url"
        Me.HarvestTempl.Name = "HarvestTempl"
        '
        'RDFModelsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.ModelsGrid)
        Me.Name = "RDFModelsControl"
        Me.Size = New System.Drawing.Size(1119, 307)
        CType(Me.ModelsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ModelsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents TypeCol As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents NameCol As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UrlCol As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TemplCol As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GUIDElement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HarvestTempl As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
