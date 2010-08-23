<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResolveControl
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.lsidText = New System.Windows.Forms.TextBox
        Me.ResolveButton = New System.Windows.Forms.Button
        Me.ModelCombo = New System.Windows.Forms.ComboBox
        Me.ModelRadio = New System.Windows.Forms.RadioButton
        Me.LSIDRadio = New System.Windows.Forms.RadioButton
        Me.DetailsControl1 = New TDWGRDFBrowser.DetailsControl
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Id"
        '
        'lsidText
        '
        Me.lsidText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lsidText.Location = New System.Drawing.Point(52, 32)
        Me.lsidText.Name = "lsidText"
        Me.lsidText.Size = New System.Drawing.Size(530, 20)
        Me.lsidText.TabIndex = 1
        '
        'ResolveButton
        '
        Me.ResolveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResolveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ResolveButton.Location = New System.Drawing.Point(588, 30)
        Me.ResolveButton.Name = "ResolveButton"
        Me.ResolveButton.Size = New System.Drawing.Size(75, 23)
        Me.ResolveButton.TabIndex = 2
        Me.ResolveButton.Text = "Resolve"
        Me.ResolveButton.UseVisualStyleBackColor = True
        '
        'ModelCombo
        '
        Me.ModelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ModelCombo.Enabled = False
        Me.ModelCombo.FormattingEnabled = True
        Me.ModelCombo.Location = New System.Drawing.Point(243, 6)
        Me.ModelCombo.Name = "ModelCombo"
        Me.ModelCombo.Size = New System.Drawing.Size(335, 21)
        Me.ModelCombo.TabIndex = 5
        '
        'ModelRadio
        '
        Me.ModelRadio.AutoSize = True
        Me.ModelRadio.Location = New System.Drawing.Point(151, 9)
        Me.ModelRadio.Name = "ModelRadio"
        Me.ModelRadio.Size = New System.Drawing.Size(54, 17)
        Me.ModelRadio.TabIndex = 6
        Me.ModelRadio.Text = "Model"
        Me.ModelRadio.UseVisualStyleBackColor = True
        '
        'LSIDRadio
        '
        Me.LSIDRadio.AutoSize = True
        Me.LSIDRadio.Checked = True
        Me.LSIDRadio.Location = New System.Drawing.Point(52, 10)
        Me.LSIDRadio.Name = "LSIDRadio"
        Me.LSIDRadio.Size = New System.Drawing.Size(73, 17)
        Me.LSIDRadio.TabIndex = 7
        Me.LSIDRadio.TabStop = True
        Me.LSIDRadio.Text = "LSID / Uri"
        Me.LSIDRadio.UseVisualStyleBackColor = True
        '
        'DetailsControl1
        '
        Me.DetailsControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DetailsControl1.Location = New System.Drawing.Point(6, 74)
        Me.DetailsControl1.Name = "DetailsControl1"
        Me.DetailsControl1.RdfDocument = Nothing
        Me.DetailsControl1.Size = New System.Drawing.Size(725, 443)
        Me.DetailsControl1.TabIndex = 3
        '
        'ResolveControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LSIDRadio)
        Me.Controls.Add(Me.ModelRadio)
        Me.Controls.Add(Me.ModelCombo)
        Me.Controls.Add(Me.DetailsControl1)
        Me.Controls.Add(Me.ResolveButton)
        Me.Controls.Add(Me.lsidText)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ResolveControl"
        Me.Size = New System.Drawing.Size(737, 523)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lsidText As System.Windows.Forms.TextBox
    Friend WithEvents ResolveButton As System.Windows.Forms.Button
    Friend WithEvents DetailsControl1 As TDWGRDFBrowser.DetailsControl
    Friend WithEvents ModelCombo As System.Windows.Forms.ComboBox
    Friend WithEvents ModelRadio As System.Windows.Forms.RadioButton
    Friend WithEvents LSIDRadio As System.Windows.Forms.RadioButton

End Class
