namespace TaxonBlaster
{
    partial class TaxonBlasterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SequencesTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SequencesTextbox
            // 
            this.SequencesTextbox.Location = new System.Drawing.Point(111, 12);
            this.SequencesTextbox.Multiline = true;
            this.SequencesTextbox.Name = "SequencesTextbox";
            this.SequencesTextbox.Size = new System.Drawing.Size(657, 239);
            this.SequencesTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sequences";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(693, 257);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // TaxonBlasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 413);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SequencesTextbox);
            this.Name = "TaxonBlasterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SequencesTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SendButton;
    }
}

