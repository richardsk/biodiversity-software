using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using LSIDClient;

namespace TestClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button resolveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox resultsTxt;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox lsidCombo;
		private System.Windows.Forms.Button allTests;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox protocolCombo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox userNameText;
		private System.Windows.Forms.TextBox passwordText;
		private System.Windows.Forms.Label label5;
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

			protocolCombo.Items.Add(WSDLConstants.SOAP);
			protocolCombo.Items.Add(WSDLConstants.HTTP);
			protocolCombo.Items.Add(WSDLConstants.FTP);
			protocolCombo.SelectedIndex = 0;

			lsidCombo.Items.Add("urn:lsid:lsidsample.org:sample:12345");
			lsidCombo.Items.Add("urn:lsid:lsidsample.org:ftpTest:12345");
			lsidCombo.Items.Add("urn:lsid:myres.org:aa:12345");
            
			//lsidCombo.Items.Add(LSIDTestClient.lsid2000);
			//lsidCombo.Items.Add(LSIDTestClient.lsid2001);
//			lsidCombo.Items.Add(LSIDTestClient.lsid01);
//			lsidCombo.Items.Add(LSIDTestClient.lsid02);
//			lsidCombo.Items.Add(LSIDTestClient.lsid03);
//			lsidCombo.Items.Add(LSIDTestClient.lsid04);
//			lsidCombo.Items.Add(LSIDTestClient.lsid10);
//			lsidCombo.Items.Add(LSIDTestClient.lsid11);
//			lsidCombo.Items.Add(LSIDTestClient.lsid14);
//			lsidCombo.Items.Add(LSIDTestClient.lsid13);
//			lsidCombo.Items.Add(LSIDTestClient.lsid30);
//			lsidCombo.Items.Add(LSIDTestClient.lsid40);

			lsidCombo.SelectedIndex = 0;
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
			this.resolveButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.resultsTxt = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lsidCombo = new System.Windows.Forms.ComboBox();
			this.allTests = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.protocolCombo = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.userNameText = new System.Windows.Forms.TextBox();
			this.passwordText = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// resolveButton
			// 
			this.resolveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.resolveButton.Location = new System.Drawing.Point(424, 88);
			this.resolveButton.Name = "resolveButton";
			this.resolveButton.TabIndex = 0;
			this.resolveButton.Text = "Resolve";
			this.resolveButton.Click += new System.EventHandler(this.resolveButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "LSID";
			// 
			// resultsTxt
			// 
			this.resultsTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.resultsTxt.Location = new System.Drawing.Point(8, 200);
			this.resultsTxt.Multiline = true;
			this.resultsTxt.Name = "resultsTxt";
			this.resultsTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.resultsTxt.Size = new System.Drawing.Size(496, 208);
			this.resultsTxt.TabIndex = 3;
			this.resultsTxt.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 184);
			this.label2.Name = "label2";
			this.label2.TabIndex = 4;
			this.label2.Text = "Results";
			// 
			// lsidCombo
			// 
			this.lsidCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lsidCombo.Location = new System.Drawing.Point(56, 24);
			this.lsidCombo.Name = "lsidCombo";
			this.lsidCombo.Size = new System.Drawing.Size(448, 21);
			this.lsidCombo.TabIndex = 5;
			this.lsidCombo.Text = "comboBox1";
			// 
			// allTests
			// 
			this.allTests.Location = new System.Drawing.Point(56, 152);
			this.allTests.Name = "allTests";
			this.allTests.Size = new System.Drawing.Size(120, 23);
			this.allTests.TabIndex = 6;
			this.allTests.Text = "Run all tests";
			this.allTests.Click += new System.EventHandler(this.allTests_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(432, 416);
			this.button1.Name = "button1";
			this.button1.TabIndex = 7;
			this.button1.Text = "Close";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// protocolCombo
			// 
			this.protocolCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.protocolCombo.Location = new System.Drawing.Point(136, 88);
			this.protocolCombo.Name = "protocolCombo";
			this.protocolCombo.Size = new System.Drawing.Size(176, 21);
			this.protocolCombo.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Protocol";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(56, 56);
			this.label4.Name = "label4";
			this.label4.TabIndex = 10;
			this.label4.Text = "Username";
			// 
			// userNameText
			// 
			this.userNameText.Location = new System.Drawing.Point(120, 56);
			this.userNameText.Name = "userNameText";
			this.userNameText.Size = new System.Drawing.Size(112, 20);
			this.userNameText.TabIndex = 11;
			this.userNameText.Text = "";
			// 
			// passwordText
			// 
			this.passwordText.Location = new System.Drawing.Point(312, 56);
			this.passwordText.Name = "passwordText";
			this.passwordText.Size = new System.Drawing.Size(128, 20);
			this.passwordText.TabIndex = 13;
			this.passwordText.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(248, 56);
			this.label5.Name = "label5";
			this.label5.TabIndex = 12;
			this.label5.Text = "Password";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 446);
			this.Controls.Add(this.passwordText);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.userNameText);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.protocolCombo);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.allTests);
			this.Controls.Add(this.lsidCombo);
			this.Controls.Add(this.resultsTxt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.resolveButton);
			this.Name = "Form1";
			this.Text = "LSID Tester";
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

        private void resolveButton_Click(object sender, System.EventArgs e)
		{   
			string dtStr = DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss z");
			DateTime dt = DateTime.ParseExact("Sat, 01 Mar 2008 01:35:38 GMT", "ddd, dd MMM yyyy HH:mm:ss Z", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);


            String lsid = lsidCombo.Text;
            resultsTxt.Text = "";
			
			Cursor.Current = Cursors.WaitCursor;
			LSIDCredentials cred = null;
			if (userNameText.Text.Length > 0)
			{
				cred = new LSIDCredentials();
				cred.setProperty(LSIDCredentials.BASICUSERNAME, userNameText.Text);
				cred.setProperty(LSIDCredentials.BASICPASSWORD, passwordText.Text);
			}

			//string res = LSIDTestClient.testAuthority(lsid, protocolCombo.SelectedItem.ToString(), cred); 
			string res = LSIDTestClient.getMetadata(lsid, cred);
			Update();
			Cursor.Current = Cursors.Default;

			resultsTxt.Text = res;
        }

		private void allTests_Click(object sender, System.EventArgs e)
		{
			resultsTxt.Text = "";

			Cursor.Current = Cursors.WaitCursor;

			LSIDTestClient cl = new LSIDTestClient();
			cl.UpdateDelegate = new TestClient.LSIDTestClient.UpdateResults(this.UpdateResults);
			cl.RunTests();

			UpdateResults("\r\nDone\r\n");

			Cursor.Current = Cursors.Default;
		}

		public void UpdateResults(string res)
		{
			resultsTxt.Text += res;
			Update();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
