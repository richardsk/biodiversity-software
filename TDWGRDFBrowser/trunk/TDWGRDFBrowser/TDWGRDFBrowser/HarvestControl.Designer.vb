<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HarvestControl
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
        Me.ViewCol = New System.Windows.Forms.DataGridViewLinkColumn
        Me.GetButton = New System.Windows.Forms.Button
        Me.RDFModelCombo = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataSourceCombo = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ResultsGrid = New System.Windows.Forms.DataGridView
        CType(Me.ResultsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ViewCol
        '
        Me.ViewCol.HeaderText = ""
        Me.ViewCol.Name = "ViewCol"
        Me.ViewCol.ReadOnly = True
        Me.ViewCol.Text = ""
        '
        'GetButton
        '
        Me.GetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GetButton.Location = New System.Drawing.Point(99, 58)
        Me.GetButton.Name = "GetButton"
        Me.GetButton.Size = New System.Drawing.Size(75, 23)
        Me.GetButton.TabIndex = 8
        Me.GetButton.Text = "Get"
        Me.GetButton.UseVisualStyleBackColor = True
        '
        'RDFModelCombo
        '
        Me.RDFModelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RDFModelCombo.FormattingEnabled = True
        Me.RDFModelCombo.Location = New System.Drawing.Point(99, 31)
        Me.RDFModelCombo.Name = "RDFModelCombo"
        Me.RDFModelCombo.Size = New System.Drawing.Size(378, 21)
        Me.RDFModelCombo.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "RDF model"
        '
        'DataSourceCombo
        '
        Me.DataSourceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DataSourceCombo.FormattingEnabled = True
        Me.DataSourceCombo.Location = New System.Drawing.Point(99, 4)
        Me.DataSourceCombo.Name = "DataSourceCombo"
        Me.DataSourceCombo.Size = New System.Drawing.Size(378, 21)
        Me.DataSourceCombo.TabIndex = 13
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Data source"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Ids:"
        '
        'ResultsGrid
        '
        Me.ResultsGrid.AllowUserToAddRows = False
        Me.ResultsGrid.AllowUserToDeleteRows = False
        Me.ResultsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.ResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultsGrid.DefaultCellStyle = DataGridViewCellStyle2
        Me.ResultsGrid.Location = New System.Drawing.Point(3, 103)
        Me.ResultsGrid.Name = "ResultsGrid"
        Me.ResultsGrid.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.ResultsGrid.Size = New System.Drawing.Size(761, 422)
        Me.ResultsGrid.TabIndex = 9
        '
        'HarvestControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataSourceCombo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.RDFModelCombo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ResultsGrid)
        Me.Controls.Add(Me.GetButton)
        Me.Name = "HarvestControl"
        Me.Size = New System.Drawing.Size(767, 528)
        CType(Me.ResultsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ViewCol As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents GetButton As System.Windows.Forms.Button
    Friend WithEvents RDFModelCombo As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataSourceCombo As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ResultsGrid As System.Windows.Forms.DataGridView

End Class
