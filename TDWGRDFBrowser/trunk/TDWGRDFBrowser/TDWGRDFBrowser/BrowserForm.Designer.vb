<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BrowserForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BrowserForm))
        Me.NavigationContainer1 = New TD.Eyefinder.NavigationContainer
        Me.SearchHeaderControl = New TD.Eyefinder.HeaderControl
        Me.SearchPane = New TD.Eyefinder.NavigationPane
        Me.SearchControl1 = New TDWGRDFBrowser.SearchControl
        Me.HeaderControl4 = New TD.Eyefinder.HeaderControl
        Me.HttpPane = New TD.Eyefinder.NavigationPane
        Me.HttpControl1 = New TDWGRDFBrowser.GenericHTTPControl
        Me.HeaderControl3 = New TD.Eyefinder.HeaderControl
        Me.HarvestPane = New TD.Eyefinder.NavigationPane
        Me.HarvestControl1 = New TDWGRDFBrowser.HarvestControl
        Me.HeaderControl1 = New TD.Eyefinder.HeaderControl
        Me.ResolvePane = New TD.Eyefinder.NavigationPane
        Me.ResolveControl1 = New TDWGRDFBrowser.ResolveControl
        Me.HeaderControl2 = New TD.Eyefinder.HeaderControl
        Me.NavigationPane1 = New TD.Eyefinder.NavigationPane
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.RDFModelsHeaderControl = New TD.Eyefinder.HeaderControl
        Me.RDFModelsPane = New TD.Eyefinder.NavigationPane
        Me.ModelsControl = New TDWGRDFBrowser.RDFModelsControl
        Me.OptionsHeaderControl = New TD.Eyefinder.HeaderControl
        Me.OptionsPane = New TD.Eyefinder.NavigationPane
        Me.OptionsControl1 = New TDWGRDFBrowser.OptionsControl
        Me.DSControl = New TD.Eyefinder.HeaderControl
        Me.DataSourcePane = New TD.Eyefinder.NavigationPane
        Me.DataSourceControl1 = New TDWGRDFBrowser.DataSourceControl
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.NavigationBar1 = New TD.Eyefinder.NavigationBar
        Me.NavigationContainer1.SuspendLayout()
        Me.SearchHeaderControl.SuspendLayout()
        Me.HeaderControl4.SuspendLayout()
        Me.HeaderControl3.SuspendLayout()
        Me.HeaderControl1.SuspendLayout()
        Me.HeaderControl2.SuspendLayout()
        Me.RDFModelsHeaderControl.SuspendLayout()
        Me.OptionsHeaderControl.SuspendLayout()
        Me.DSControl.SuspendLayout()
        Me.NavigationBar1.SuspendLayout()
        Me.SuspendLayout()
        '
        'NavigationContainer1
        '
        Me.NavigationContainer1.Controls.Add(Me.SearchHeaderControl)
        Me.NavigationContainer1.Controls.Add(Me.HeaderControl4)
        Me.NavigationContainer1.Controls.Add(Me.HeaderControl3)
        Me.NavigationContainer1.Controls.Add(Me.HeaderControl1)
        Me.NavigationContainer1.Controls.Add(Me.HeaderControl2)
        Me.NavigationContainer1.Controls.Add(Me.RDFModelsHeaderControl)
        Me.NavigationContainer1.Controls.Add(Me.OptionsHeaderControl)
        Me.NavigationContainer1.Controls.Add(Me.DSControl)
        Me.NavigationContainer1.Controls.Add(Me.Splitter1)
        Me.NavigationContainer1.Controls.Add(Me.NavigationBar1)
        Me.NavigationContainer1.Location = New System.Drawing.Point(0, 0)
        Me.NavigationContainer1.Name = "NavigationContainer1"
        Me.NavigationContainer1.Size = New System.Drawing.Size(1145, 785)
        Me.NavigationContainer1.TabIndex = 0
        '
        'SearchHeaderControl
        '
        Me.SearchHeaderControl.BuddyPane = Me.SearchPane
        Me.SearchHeaderControl.Controls.Add(Me.SearchControl1)
        Me.SearchHeaderControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SearchHeaderControl.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.SearchHeaderControl.Location = New System.Drawing.Point(167, 4)
        Me.SearchHeaderControl.Name = "SearchHeaderControl"
        Me.SearchHeaderControl.Size = New System.Drawing.Size(974, 784)
        Me.SearchHeaderControl.TabIndex = 2
        Me.SearchHeaderControl.Text = "Search"
        '
        'SearchPane
        '
        Me.SearchPane.LargeImage = CType(resources.GetObject("SearchPane.LargeImage"), System.Drawing.Image)
        Me.SearchPane.Location = New System.Drawing.Point(1, 26)
        Me.SearchPane.Name = "SearchPane"
        Me.SearchPane.Size = New System.Drawing.Size(158, 454)
        Me.SearchPane.TabIndex = 0
        Me.SearchPane.Text = "Search"
        Me.SearchPane.Visible = False
        '
        'SearchControl1
        '
        Me.SearchControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SearchControl1.Location = New System.Drawing.Point(1, 26)
        Me.SearchControl1.Name = "SearchControl1"
        Me.SearchControl1.Size = New System.Drawing.Size(972, 757)
        Me.SearchControl1.TabIndex = 0
        '
        'HeaderControl4
        '
        Me.HeaderControl4.BuddyPane = Me.HttpPane
        Me.HeaderControl4.Controls.Add(Me.HttpControl1)
        Me.HeaderControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HeaderControl4.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderControl4.Location = New System.Drawing.Point(167, 4)
        Me.HeaderControl4.Name = "HeaderControl4"
        Me.HeaderControl4.Size = New System.Drawing.Size(974, 777)
        Me.HeaderControl4.TabIndex = 9
        Me.HeaderControl4.Text = "HTTP Resolution"
        '
        'HttpPane
        '
        Me.HttpPane.LargeImage = CType(resources.GetObject("HttpPane.LargeImage"), System.Drawing.Image)
        Me.HttpPane.Location = New System.Drawing.Point(1, 26)
        Me.HttpPane.Name = "HttpPane"
        Me.HttpPane.Size = New System.Drawing.Size(158, 454)
        Me.HttpPane.TabIndex = 7
        Me.HttpPane.Text = "HTTP Resolution"
        '
        'HttpControl1
        '
        Me.HttpControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HttpControl1.Location = New System.Drawing.Point(6, 29)
        Me.HttpControl1.Name = "HttpControl1"
        Me.HttpControl1.Size = New System.Drawing.Size(960, 740)
        Me.HttpControl1.TabIndex = 0
        '
        'HeaderControl3
        '
        Me.HeaderControl3.BuddyPane = Me.HarvestPane
        Me.HeaderControl3.Controls.Add(Me.HarvestControl1)
        Me.HeaderControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HeaderControl3.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderControl3.Location = New System.Drawing.Point(167, 4)
        Me.HeaderControl3.Name = "HeaderControl3"
        Me.HeaderControl3.Size = New System.Drawing.Size(974, 784)
        Me.HeaderControl3.TabIndex = 8
        Me.HeaderControl3.Text = "Harvest Ids"
        '
        'HarvestPane
        '
        Me.HarvestPane.LargeImage = CType(resources.GetObject("HarvestPane.LargeImage"), System.Drawing.Image)
        Me.HarvestPane.Location = New System.Drawing.Point(1, 26)
        Me.HarvestPane.Name = "HarvestPane"
        Me.HarvestPane.Size = New System.Drawing.Size(158, 454)
        Me.HarvestPane.TabIndex = 6
        Me.HarvestPane.Text = "Harvest"
        Me.HarvestPane.Visible = False
        '
        'HarvestControl1
        '
        Me.HarvestControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HarvestControl1.Location = New System.Drawing.Point(6, 29)
        Me.HarvestControl1.Name = "HarvestControl1"
        Me.HarvestControl1.Size = New System.Drawing.Size(960, 706)
        Me.HarvestControl1.TabIndex = 0
        '
        'HeaderControl1
        '
        Me.HeaderControl1.BuddyPane = Me.ResolvePane
        Me.HeaderControl1.Controls.Add(Me.ResolveControl1)
        Me.HeaderControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HeaderControl1.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderControl1.Location = New System.Drawing.Point(167, 4)
        Me.HeaderControl1.Name = "HeaderControl1"
        Me.HeaderControl1.Size = New System.Drawing.Size(974, 777)
        Me.HeaderControl1.TabIndex = 5
        Me.HeaderControl1.Text = "Resolve Id"
        '
        'ResolvePane
        '
        Me.ResolvePane.LargeImage = CType(resources.GetObject("ResolvePane.LargeImage"), System.Drawing.Image)
        Me.ResolvePane.Location = New System.Drawing.Point(1, 26)
        Me.ResolvePane.Name = "ResolvePane"
        Me.ResolvePane.Size = New System.Drawing.Size(158, 454)
        Me.ResolvePane.TabIndex = 3
        Me.ResolvePane.Text = "Resolve Id"
        Me.ResolvePane.Visible = False
        '
        'ResolveControl1
        '
        Me.ResolveControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResolveControl1.Location = New System.Drawing.Point(6, 29)
        Me.ResolveControl1.Name = "ResolveControl1"
        Me.ResolveControl1.Size = New System.Drawing.Size(960, 740)
        Me.ResolveControl1.TabIndex = 0
        '
        'HeaderControl2
        '
        Me.HeaderControl2.BuddyPane = Me.NavigationPane1
        Me.HeaderControl2.Controls.Add(Me.Label2)
        Me.HeaderControl2.Controls.Add(Me.Label1)
        Me.HeaderControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HeaderControl2.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderControl2.Location = New System.Drawing.Point(167, 4)
        Me.HeaderControl2.Name = "HeaderControl2"
        Me.HeaderControl2.Size = New System.Drawing.Size(697, 582)
        Me.HeaderControl2.TabIndex = 7
        Me.HeaderControl2.Text = "SPARQL Query"
        '
        'NavigationPane1
        '
        Me.NavigationPane1.LargeImage = CType(resources.GetObject("NavigationPane1.LargeImage"), System.Drawing.Image)
        Me.NavigationPane1.Location = New System.Drawing.Point(1, 26)
        Me.NavigationPane1.Name = "NavigationPane1"
        Me.NavigationPane1.Size = New System.Drawing.Size(158, 454)
        Me.NavigationPane1.TabIndex = 5
        Me.NavigationPane1.Text = "SPARQL Querry"
        Me.NavigationPane1.Visible = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(40, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(470, 76)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "The idea is to somehow use SPARQL to query the cache, therefore performing querie" & _
            "s and inferences over all previously queried data"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(40, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Not implemented due to lack of time"
        '
        'RDFModelsHeaderControl
        '
        Me.RDFModelsHeaderControl.BuddyPane = Me.RDFModelsPane
        Me.RDFModelsHeaderControl.Controls.Add(Me.ModelsControl)
        Me.RDFModelsHeaderControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RDFModelsHeaderControl.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.RDFModelsHeaderControl.Location = New System.Drawing.Point(167, 4)
        Me.RDFModelsHeaderControl.Name = "RDFModelsHeaderControl"
        Me.RDFModelsHeaderControl.Size = New System.Drawing.Size(691, 582)
        Me.RDFModelsHeaderControl.TabIndex = 6
        Me.RDFModelsHeaderControl.Text = "RDF Models"
        '
        'RDFModelsPane
        '
        Me.RDFModelsPane.LargeImage = CType(resources.GetObject("RDFModelsPane.LargeImage"), System.Drawing.Image)
        Me.RDFModelsPane.Location = New System.Drawing.Point(1, 26)
        Me.RDFModelsPane.Name = "RDFModelsPane"
        Me.RDFModelsPane.Size = New System.Drawing.Size(158, 454)
        Me.RDFModelsPane.TabIndex = 4
        Me.RDFModelsPane.Text = "RDF Models"
        Me.RDFModelsPane.Visible = False
        '
        'ModelsControl
        '
        Me.ModelsControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModelsControl.Location = New System.Drawing.Point(6, 29)
        Me.ModelsControl.Name = "ModelsControl"
        Me.ModelsControl.Size = New System.Drawing.Size(677, 545)
        Me.ModelsControl.TabIndex = 0
        '
        'OptionsHeaderControl
        '
        Me.OptionsHeaderControl.BuddyPane = Me.OptionsPane
        Me.OptionsHeaderControl.Controls.Add(Me.OptionsControl1)
        Me.OptionsHeaderControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OptionsHeaderControl.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.OptionsHeaderControl.Location = New System.Drawing.Point(167, 4)
        Me.OptionsHeaderControl.Name = "OptionsHeaderControl"
        Me.OptionsHeaderControl.Size = New System.Drawing.Size(691, 582)
        Me.OptionsHeaderControl.TabIndex = 3
        Me.OptionsHeaderControl.Text = "Options"
        '
        'OptionsPane
        '
        Me.OptionsPane.LargeImage = CType(resources.GetObject("OptionsPane.LargeImage"), System.Drawing.Image)
        Me.OptionsPane.Location = New System.Drawing.Point(1, 26)
        Me.OptionsPane.Name = "OptionsPane"
        Me.OptionsPane.Size = New System.Drawing.Size(158, 454)
        Me.OptionsPane.TabIndex = 2
        Me.OptionsPane.Text = "Options"
        Me.OptionsPane.Visible = False
        '
        'OptionsControl1
        '
        Me.OptionsControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OptionsControl1.Location = New System.Drawing.Point(6, 29)
        Me.OptionsControl1.Name = "OptionsControl1"
        Me.OptionsControl1.Size = New System.Drawing.Size(677, 545)
        Me.OptionsControl1.TabIndex = 0
        '
        'DSControl
        '
        Me.DSControl.BuddyPane = Me.DataSourcePane
        Me.DSControl.Controls.Add(Me.DataSourceControl1)
        Me.DSControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DSControl.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.DSControl.Location = New System.Drawing.Point(167, 4)
        Me.DSControl.Name = "DSControl"
        Me.DSControl.Size = New System.Drawing.Size(691, 582)
        Me.DSControl.TabIndex = 4
        Me.DSControl.Text = "Data Sources"
        '
        'DataSourcePane
        '
        Me.DataSourcePane.LargeImage = CType(resources.GetObject("DataSourcePane.LargeImage"), System.Drawing.Image)
        Me.DataSourcePane.Location = New System.Drawing.Point(1, 26)
        Me.DataSourcePane.Name = "DataSourcePane"
        Me.DataSourcePane.Size = New System.Drawing.Size(158, 454)
        Me.DataSourcePane.TabIndex = 1
        Me.DataSourcePane.Text = "Data Sources"
        Me.DataSourcePane.Visible = False
        '
        'DataSourceControl1
        '
        Me.DataSourceControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataSourceControl1.Location = New System.Drawing.Point(4, 29)
        Me.DataSourceControl1.Name = "DataSourceControl1"
        Me.DataSourceControl1.Size = New System.Drawing.Size(679, 434)
        Me.DataSourceControl1.TabIndex = 0
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(164, 4)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 777)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'NavigationBar1
        '
        Me.NavigationBar1.Controls.Add(Me.HttpPane)
        Me.NavigationBar1.Controls.Add(Me.HarvestPane)
        Me.NavigationBar1.Controls.Add(Me.NavigationPane1)
        Me.NavigationBar1.Controls.Add(Me.OptionsPane)
        Me.NavigationBar1.Controls.Add(Me.RDFModelsPane)
        Me.NavigationBar1.Controls.Add(Me.DataSourcePane)
        Me.NavigationBar1.Controls.Add(Me.SearchPane)
        Me.NavigationBar1.Controls.Add(Me.ResolvePane)
        Me.NavigationBar1.HeaderFont = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.NavigationBar1.Location = New System.Drawing.Point(4, 4)
        Me.NavigationBar1.Name = "NavigationBar1"
        Me.NavigationBar1.PaneFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.NavigationBar1.SelectedPane = Me.ResolvePane
        Me.NavigationBar1.ShowPanes = 8
        Me.NavigationBar1.Size = New System.Drawing.Size(160, 777)
        Me.NavigationBar1.TabIndex = 0
        '
        'BrowserForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1145, 785)
        Me.Controls.Add(Me.NavigationContainer1)
        Me.Name = "BrowserForm"
        Me.Text = "Browser"
        Me.NavigationContainer1.ResumeLayout(False)
        Me.SearchHeaderControl.ResumeLayout(False)
        Me.HeaderControl4.ResumeLayout(False)
        Me.HeaderControl3.ResumeLayout(False)
        Me.HeaderControl1.ResumeLayout(False)
        Me.HeaderControl2.ResumeLayout(False)
        Me.HeaderControl2.PerformLayout()
        Me.RDFModelsHeaderControl.ResumeLayout(False)
        Me.OptionsHeaderControl.ResumeLayout(False)
        Me.DSControl.ResumeLayout(False)
        Me.NavigationBar1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NavigationContainer1 As TD.Eyefinder.NavigationContainer
    Friend WithEvents SearchHeaderControl As TD.Eyefinder.HeaderControl
    Friend WithEvents SearchPane As TD.Eyefinder.NavigationPane
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents NavigationBar1 As TD.Eyefinder.NavigationBar
    Friend WithEvents OptionsHeaderControl As TD.Eyefinder.HeaderControl
    Friend WithEvents OptionsPane As TD.Eyefinder.NavigationPane
    Friend WithEvents SearchControl1 As TDWGRDFBrowser.SearchControl
    Friend WithEvents DSControl As TD.Eyefinder.HeaderControl
    Friend WithEvents DataSourcePane As TD.Eyefinder.NavigationPane
    Friend WithEvents DataSourceControl1 As TDWGRDFBrowser.DataSourceControl
    Friend WithEvents HeaderControl1 As TD.Eyefinder.HeaderControl
    Friend WithEvents ResolvePane As TD.Eyefinder.NavigationPane
    Friend WithEvents ResolveControl1 As TDWGRDFBrowser.ResolveControl
    Friend WithEvents OptionsControl1 As TDWGRDFBrowser.OptionsControl
    Friend WithEvents RDFModelsHeaderControl As TD.Eyefinder.HeaderControl
    Friend WithEvents RDFModelsPane As TD.Eyefinder.NavigationPane
    Friend WithEvents ModelsControl As TDWGRDFBrowser.RDFModelsControl
    Friend WithEvents HeaderControl2 As TD.Eyefinder.HeaderControl
    Friend WithEvents NavigationPane1 As TD.Eyefinder.NavigationPane
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents HeaderControl3 As TD.Eyefinder.HeaderControl
    Friend WithEvents HarvestPane As TD.Eyefinder.NavigationPane
    Friend WithEvents HarvestControl1 As TDWGRDFBrowser.HarvestControl
    Friend WithEvents HeaderControl4 As TD.Eyefinder.HeaderControl
    Friend WithEvents HttpPane As TD.Eyefinder.NavigationPane
    Friend WithEvents HttpControl1 As TDWGRDFBrowser.GenericHTTPControl

End Class
