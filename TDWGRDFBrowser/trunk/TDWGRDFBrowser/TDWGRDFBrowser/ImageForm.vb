Public Class ImageForm

    Public Property GraphImage() As Image
        Get
            Return PictureBox1.Image
        End Get
        Set(ByVal value As Image)
            PictureBox1.Image = value
        End Set
    End Property

    Private Sub ImageForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ZoomToolButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomToolButton.Click
        If ZoomToolButton.Text = "Zoom To Fit" Then
            Panel1.HorizontalScroll.Value = 0
            Panel1.VerticalScroll.Value = 0
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
            PictureBox1.Width = Panel1.Width - 4
            PictureBox1.Height = Panel1.Height - 4
            PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
            ZoomToolButton.Text = "Zoom To Actual Size"
        Else
            PictureBox1.Anchor = AnchorStyles.Left Or AnchorStyles.Top
            PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize
            ZoomToolButton.Text = "Zoom To Fit"
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.w3.org/RDF/Validator/")
    End Sub
End Class