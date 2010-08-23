<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataSourceControl
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
        Me.DataSourcesGrid = New System.Windows.Forms.DataGridView
        Me.Include = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Type = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.NameCol = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Url = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Id = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label1 = New System.Windows.Forms.Label
        Me.ModelsList = New System.Windows.Forms.ListBox
        Me.AddButton = New System.Windows.Forms.Button
        Me.SaveButton = New System.Windows.Forms.Button
        CType(Me.DataSourcesGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataSourcesGrid
        '
        Me.DataSourcesGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataSourcesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataSourcesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Include, Me.Type, Me.NameCol, Me.Url, Me.Id})
        Me.DataSourcesGrid.Location = New System.Drawing.Point(0, 0)
        Me.DataSourcesGrid.Name = "DataSourcesGrid"
        Me.DataSourcesGrid.Size = New System.Drawing.Size(765, 199)
        Me.DataSourcesGrid.TabIndex = 9
        '
        'Include
        '
        Me.Include.DataPropertyName = "Include"
        Me.Include.HeaderText = "Include"
        Me.Include.Name = "Include"
        Me.Include.Width = 60
        '
        'Type
        '
        Me.Type.DataPropertyName = "QuerierClass"
        Me.Type.HeaderText = "Type"
        Me.Type.Name = "Type"
        '
        'NameCol
        '
        Me.NameCol.DataPropertyName = "Name"
        Me.NameCol.HeaderText = "Name"
        Me.NameCol.Name = "NameCol"
        Me.NameCol.Width = 150
        '
        'Url
        '
        Me.Url.DataPropertyName = "Url"
        Me.Url.HeaderText = "Url"
        Me.Url.Name = "Url"
        Me.Url.Width = 400
        '
        'Id
        '
        Me.Id.DataPropertyName = "DataSourceId"
        Me.Id.HeaderText = "Id"
        Me.Id.Name = "Id"
        Me.Id.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 223)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(162, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Models for selected Data Source"
        '
        'ModelsList
        '
        Me.ModelsList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModelsList.FormattingEnabled = True
        Me.ModelsList.Location = New System.Drawing.Point(6, 239)
        Me.ModelsList.Name = "ModelsList"
        Me.ModelsList.Size = New System.Drawing.Size(584, 134)
        Me.ModelsList.TabIndex = 13
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddButton.Location = New System.Drawing.Point(596, 239)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 14
        Me.AddButton.Text = "Edit"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SaveButton.Location = New System.Drawing.Point(690, 350)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 15
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'DataSourceControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.ModelsList)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataSourcesGrid)
        Me.Name = "DataSourceControl"
        Me.Size = New System.Drawing.Size(768, 682)
        CType(Me.DataSourcesGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataSourcesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ModelsList As System.Windows.Forms.ListBox
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents Include As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Type As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents NameCol As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Url As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Id As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SaveButton As System.Windows.Forms.Button

End Class
