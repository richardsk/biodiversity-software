<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsControl
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
        Me.AutoResolveText = New System.Windows.Forms.TextBox
        Me.CacheResultsCheck = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Auto resolve to level"
        '
        'AutoResolveText
        '
        Me.AutoResolveText.Enabled = False
        Me.AutoResolveText.Location = New System.Drawing.Point(112, 6)
        Me.AutoResolveText.Name = "AutoResolveText"
        Me.AutoResolveText.Size = New System.Drawing.Size(100, 20)
        Me.AutoResolveText.TabIndex = 2
        '
        'CacheResultsCheck
        '
        Me.CacheResultsCheck.AutoSize = True
        Me.CacheResultsCheck.Location = New System.Drawing.Point(6, 41)
        Me.CacheResultsCheck.Name = "CacheResultsCheck"
        Me.CacheResultsCheck.Size = New System.Drawing.Size(167, 17)
        Me.CacheResultsCheck.TabIndex = 5
        Me.CacheResultsCheck.Text = "Cache RDF from query results"
        Me.CacheResultsCheck.UseVisualStyleBackColor = True
        '
        'OptionsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CacheResultsCheck)
        Me.Controls.Add(Me.AutoResolveText)
        Me.Controls.Add(Me.Label1)
        Me.Name = "OptionsControl"
        Me.Size = New System.Drawing.Size(628, 521)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AutoResolveText As System.Windows.Forms.TextBox
    Friend WithEvents CacheResultsCheck As System.Windows.Forms.CheckBox

End Class
