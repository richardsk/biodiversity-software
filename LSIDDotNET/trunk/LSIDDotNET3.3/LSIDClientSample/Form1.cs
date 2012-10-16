using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using LSIDClient;

namespace LSIDClientSample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button GetMetadataButton;
		private System.Windows.Forms.Button GetDataButton;
		private System.Windows.Forms.Button AssignButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox AuthorityText;
		private System.Windows.Forms.TextBox ResultsText;
		private System.Windows.Forms.TextBox NewLSIDText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.TextBox NamespaceText;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox AuthUrlText;
        private System.Windows.Forms.ComboBox LSIDCombo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            LSIDCombo.Items.Add("urn:lsid:lsidsample.org:sample:12345");
            LSIDCombo.Items.Add("urn:lsid:lsidsample.org:ftpTest:12345");
            LSIDCombo.Items.Add("urn:lsid:indexfungorum.org:names:213649");
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.GetMetadataButton = new System.Windows.Forms.Button();
            this.GetDataButton = new System.Windows.Forms.Button();
            this.AssignButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AuthorityText = new System.Windows.Forms.TextBox();
            this.ResultsText = new System.Windows.Forms.TextBox();
            this.NewLSIDText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.NamespaceText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AuthUrlText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LSIDCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.TabIndex = 0;
            this.label1.Text = "LSID";
            // 
            // GetMetadataButton
            // 
            this.GetMetadataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetMetadataButton.Location = new System.Drawing.Point(256, 48);
            this.GetMetadataButton.Name = "GetMetadataButton";
            this.GetMetadataButton.Size = new System.Drawing.Size(96, 23);
            this.GetMetadataButton.TabIndex = 2;
            this.GetMetadataButton.Text = "Get Metadata";
            this.GetMetadataButton.Click += new System.EventHandler(this.GetMetadataButton_Click);
            // 
            // GetDataButton
            // 
            this.GetDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetDataButton.Location = new System.Drawing.Point(360, 48);
            this.GetDataButton.Name = "GetDataButton";
            this.GetDataButton.Size = new System.Drawing.Size(80, 23);
            this.GetDataButton.TabIndex = 3;
            this.GetDataButton.Text = "Get Data";
            this.GetDataButton.Click += new System.EventHandler(this.GetDataButton_Click);
            // 
            // AssignButton
            // 
            this.AssignButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AssignButton.Location = new System.Drawing.Point(336, 304);
            this.AssignButton.Name = "AssignButton";
            this.AssignButton.Size = new System.Drawing.Size(104, 23);
            this.AssignButton.TabIndex = 4;
            this.AssignButton.Text = "Assign New LSID";
            this.AssignButton.Click += new System.EventHandler(this.AssignButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(8, 240);
            this.label2.Name = "label2";
            this.label2.TabIndex = 5;
            this.label2.Text = "Authority";
            // 
            // AuthorityText
            // 
            this.AuthorityText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthorityText.Location = new System.Drawing.Point(72, 240);
            this.AuthorityText.Name = "AuthorityText";
            this.AuthorityText.Size = new System.Drawing.Size(368, 20);
            this.AuthorityText.TabIndex = 6;
            this.AuthorityText.Text = "lsidsample.org";
            // 
            // ResultsText
            // 
            this.ResultsText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsText.Location = new System.Drawing.Point(64, 80);
            this.ResultsText.Multiline = true;
            this.ResultsText.Name = "ResultsText";
            this.ResultsText.ReadOnly = true;
            this.ResultsText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ResultsText.Size = new System.Drawing.Size(376, 120);
            this.ResultsText.TabIndex = 7;
            this.ResultsText.Text = "";
            // 
            // NewLSIDText
            // 
            this.NewLSIDText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.NewLSIDText.Location = new System.Drawing.Point(136, 336);
            this.NewLSIDText.Name = "NewLSIDText";
            this.NewLSIDText.Size = new System.Drawing.Size(304, 20);
            this.NewLSIDText.TabIndex = 8;
            this.NewLSIDText.Text = "";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(72, 336);
            this.label3.Name = "label3";
            this.label3.TabIndex = 9;
            this.label3.Text = "New LSID";
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(368, 376);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.TabIndex = 10;
            this.CloseButton.Text = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // NamespaceText
            // 
            this.NamespaceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.NamespaceText.Location = new System.Drawing.Point(72, 272);
            this.NamespaceText.Name = "NamespaceText";
            this.NamespaceText.Size = new System.Drawing.Size(368, 20);
            this.NamespaceText.TabIndex = 12;
            this.NamespaceText.Text = "sample";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(8, 272);
            this.label4.Name = "label4";
            this.label4.TabIndex = 11;
            this.label4.Text = "Namespace";
            // 
            // AuthUrlText
            // 
            this.AuthUrlText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthUrlText.Location = new System.Drawing.Point(72, 208);
            this.AuthUrlText.Name = "AuthUrlText";
            this.AuthUrlText.Size = new System.Drawing.Size(368, 20);
            this.AuthUrlText.TabIndex = 14;
            this.AuthUrlText.Text = "http://localhost/authority/";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(8, 208);
            this.label5.Name = "label5";
            this.label5.TabIndex = 13;
            this.label5.Text = "Authority Url";
            // 
            // LSIDCombo
            // 
            this.LSIDCombo.Location = new System.Drawing.Point(56, 16);
            this.LSIDCombo.Name = "LSIDCombo";
            this.LSIDCombo.Size = new System.Drawing.Size(384, 21);
            this.LSIDCombo.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(456, 414);
            this.Controls.Add(this.LSIDCombo);
            this.Controls.Add(this.AuthUrlText);
            this.Controls.Add(this.NamespaceText);
            this.Controls.Add(this.NewLSIDText);
            this.Controls.Add(this.ResultsText);
            this.Controls.Add(this.AuthorityText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AssignButton);
            this.Controls.Add(this.GetDataButton);
            this.Controls.Add(this.GetMetadataButton);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "LSID Client Sample";
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void CloseButton_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void GetMetadataButton_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string res = "";

			try
			{
				LSIDResolver resolver = new LSIDResolver(new LSID(LSIDCombo.Text));
				MetadataResponse md = resolver.getMetadata();
				
				System.IO.StreamReader rdr = new System.IO.StreamReader(md.getMetadata());
				res = rdr.ReadToEnd();
				rdr.Close();
			}
			catch(Exception ex)
			{
				res = "Error : " + ex.Message;
			}

			ResultsText.Text = res;

			Cursor.Current = Cursors.Default;
		}

		private void GetDataButton_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string res = "";

			try
			{
				LSIDResolver resolver = new LSIDResolver(new LSID(LSIDCombo.Text));
				System.IO.Stream s = resolver.getData();
				
				System.IO.StreamReader rdr = new System.IO.StreamReader(s);
				res = rdr.ReadToEnd();
				rdr.Close();
			}
			catch(Exception ex)
			{
				res = "Error : " + ex.Message;
			}

			ResultsText.Text = res;

			Cursor.Current = Cursors.Default;
		
		}

		private void AssignButton_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			string res = "";

			try
			{
				LSIDAssigner asn = new LSIDAssigner(new SOAPLocation(AuthUrlText.Text + "assigning"));
				LSID l = asn.assignLSID(AuthorityText.Text, NamespaceText.Text, new Properties());
				res = l.ToString();
			}
			catch(Exception ex)
			{
				res = "Error : " + ex.Message;
			}

			NewLSIDText.Text = res;

			Cursor.Current = Cursors.Default;
		
		}
	}
}
