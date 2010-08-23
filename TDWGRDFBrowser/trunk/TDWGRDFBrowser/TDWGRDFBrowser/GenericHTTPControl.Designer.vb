<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GenericHTTPControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ResolveButton = New System.Windows.Forms.Button
        Me.uriText = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.resultsText = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.contentTypeCombo = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ResolveButton
        '
        Me.ResolveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResolveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ResolveButton.Location = New System.Drawing.Point(564, 29)
        Me.ResolveButton.Name = "ResolveButton"
        Me.ResolveButton.Size = New System.Drawing.Size(75, 23)
        Me.ResolveButton.TabIndex = 5
        Me.ResolveButton.Text = "Resolve"
        Me.ResolveButton.UseVisualStyleBackColor = True
        '
        'uriText
        '
        Me.uriText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uriText.Location = New System.Drawing.Point(54, 5)
        Me.uriText.Name = "uriText"
        Me.uriText.Size = New System.Drawing.Size(504, 20)
        Me.uriText.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Uri"
        '
        'resultsText
        '
        Me.resultsText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.resultsText.Location = New System.Drawing.Point(11, 75)
        Me.resultsText.Multiline = True
        Me.resultsText.Name = "resultsText"
        Me.resultsText.Size = New System.Drawing.Size(628, 310)
        Me.resultsText.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Result"
        '
        'contentTypeCombo
        '
        Me.contentTypeCombo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.contentTypeCombo.FormattingEnabled = True
        Me.contentTypeCombo.Location = New System.Drawing.Point(85, 31)
        Me.contentTypeCombo.Name = "contentTypeCombo"
        Me.contentTypeCombo.Size = New System.Drawing.Size(473, 21)
        Me.contentTypeCombo.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Content Type"
        '
        'GenericHTTPControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.contentTypeCombo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.resultsText)
        Me.Controls.Add(Me.ResolveButton)
        Me.Controls.Add(Me.uriText)
        Me.Controls.Add(Me.Label1)
        Me.Name = "GenericHTTPControl"
        Me.Size = New System.Drawing.Size(650, 398)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ResolveButton As System.Windows.Forms.Button
    Friend WithEvents uriText As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents resultsText As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents contentTypeCombo As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
