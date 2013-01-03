namespace GathererImport
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtError = new System.Windows.Forms.TextBox();
            this.cmdGrabPrices = new System.Windows.Forms.Button();
            this.txtSets = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gotoGitHub = new System.Windows.Forms.LinkLabel();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtError
            // 
            this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtError.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtError.Location = new System.Drawing.Point(294, 12);
            this.txtError.Multiline = true;
            this.txtError.Name = "txtError";
            this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtError.Size = new System.Drawing.Size(479, 275);
            this.txtError.TabIndex = 0;
            // 
            // cmdGrabPrices
            // 
            this.cmdGrabPrices.Location = new System.Drawing.Point(12, 12);
            this.cmdGrabPrices.Name = "cmdGrabPrices";
            this.cmdGrabPrices.Size = new System.Drawing.Size(276, 109);
            this.cmdGrabPrices.TabIndex = 1;
            this.cmdGrabPrices.Text = "GATHER!";
            this.cmdGrabPrices.UseVisualStyleBackColor = true;
            this.cmdGrabPrices.Click += new System.EventHandler(this.cmdGrabPrices_Click);
            // 
            // txtSets
            // 
            this.txtSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSets.Location = new System.Drawing.Point(12, 140);
            this.txtSets.Multiline = true;
            this.txtSets.Name = "txtSets";
            this.txtSets.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSets.Size = new System.Drawing.Size(276, 220);
            this.txtSets.TabIndex = 2;
            this.txtSets.Text = resources.GetString("txtSets.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Magic Sets (Edit as you see fit)";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gotoGitHub);
            this.panel1.Location = new System.Drawing.Point(294, 325);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(488, 35);
            this.panel1.TabIndex = 4;
            // 
            // gotoGitHub
            // 
            this.gotoGitHub.AutoSize = true;
            this.gotoGitHub.Location = new System.Drawing.Point(241, 10);
            this.gotoGitHub.Name = "gotoGitHub";
            this.gotoGitHub.Size = new System.Drawing.Size(226, 13);
            this.gotoGitHub.TabIndex = 0;
            this.gotoGitHub.TabStop = true;
            this.gotoGitHub.Text = "https://github.com/nickcdavis/GathererImport";
            this.gotoGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gotoGitHub_LinkClicked);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 140);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(276, 214);
            this.checkedListBox1.TabIndex = 5;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(295, 295);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(478, 24);
            this.progressBar1.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 366);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSets);
            this.Controls.Add(this.cmdGrabPrices);
            this.Controls.Add(this.txtError);
            this.Name = "Form1";
            this.Text = "Gatherer Import";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Button cmdGrabPrices;
        private System.Windows.Forms.TextBox txtSets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel gotoGitHub;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

