<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DetailsControl
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailsControl))
        Me.ViewGraphLink = New System.Windows.Forms.LinkLabel
        Me.DetailsGrid = New AdvancedDataGridView.TreeGridView
        Me.Field = New AdvancedDataGridView.TreeGridColumn
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BackButton = New System.Windows.Forms.Button
        Me.ViewRDFLink = New System.Windows.Forms.LinkLabel
        CType(Me.DetailsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ViewGraphLink
        '
        Me.ViewGraphLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewGraphLink.AutoSize = True
        Me.ViewGraphLink.Location = New System.Drawing.Point(529, 468)
        Me.ViewGraphLink.Name = "ViewGraphLink"
        Me.ViewGraphLink.Size = New System.Drawing.Size(87, 13)
        Me.ViewGraphLink.TabIndex = 0
        Me.ViewGraphLink.TabStop = True
        Me.ViewGraphLink.Text = "View RDF Graph"
        '
        'DetailsGrid
        '
        Me.DetailsGrid.AllowUserToAddRows = False
        Me.DetailsGrid.AllowUserToDeleteRows = False
        Me.DetailsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DetailsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DetailsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DetailsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Field, Me.Value})
        Me.DetailsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DetailsGrid.ImageList = Nothing
        Me.DetailsGrid.Location = New System.Drawing.Point(3, 31)
        Me.DetailsGrid.Name = "DetailsGrid"
        Me.DetailsGrid.ReadOnly = True
        Me.DetailsGrid.RowHeadersVisible = False
        Me.DetailsGrid.Size = New System.Drawing.Size(624, 419)
        Me.DetailsGrid.TabIndex = 1
        '
        'Field
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige
        Me.Field.DefaultCellStyle = DataGridViewCellStyle2
        Me.Field.DefaultNodeImage = Nothing
        Me.Field.FillWeight = 70.0!
        Me.Field.HeaderText = "Field"
        Me.Field.Name = "Field"
        Me.Field.ReadOnly = True
        Me.Field.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Field.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Value
        '
        Me.Value.HeaderText = "Value"
        Me.Value.Name = "Value"
        Me.Value.ReadOnly = True
        Me.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'BackButton
        '
        Me.BackButton.Enabled = False
        Me.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BackButton.Image = CType(resources.GetObject("BackButton.Image"), System.Drawing.Image)
        Me.BackButton.Location = New System.Drawing.Point(3, 3)
        Me.BackButton.Name = "BackButton"
        Me.BackButton.Size = New System.Drawing.Size(42, 22)
        Me.BackButton.TabIndex = 22
        Me.BackButton.UseVisualStyleBackColor = True
        '
        'ViewRDFLink
        '
        Me.ViewRDFLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewRDFLink.AutoSize = True
        Me.ViewRDFLink.Location = New System.Drawing.Point(421, 468)
        Me.ViewRDFLink.Name = "ViewRDFLink"
        Me.ViewRDFLink.Size = New System.Drawing.Size(92, 13)
        Me.ViewRDFLink.TabIndex = 23
        Me.ViewRDFLink.TabStop = True
        Me.ViewRDFLink.Text = "View Source RDF"
        '
        'DetailsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ViewRDFLink)
        Me.Controls.Add(Me.BackButton)
        Me.Controls.Add(Me.DetailsGrid)
        Me.Controls.Add(Me.ViewGraphLink)
        Me.Name = "DetailsControl"
        Me.Size = New System.Drawing.Size(630, 494)
        CType(Me.DetailsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ViewGraphLink As System.Windows.Forms.LinkLabel
    Friend WithEvents DetailsGrid As AdvancedDataGridView.TreeGridView
    Friend WithEvents Field As AdvancedDataGridView.TreeGridColumn
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BackButton As System.Windows.Forms.Button
    Friend WithEvents ViewRDFLink As System.Windows.Forms.LinkLabel

End Class
