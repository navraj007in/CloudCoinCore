namespace WindowsFormsApp1
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
            this.cmdEcho = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdImport = new System.Windows.Forms.Button();
            this.cmdExport = new System.Windows.Forms.Button();
            this.cmdBank = new System.Windows.Forms.Button();
            this.cmdDetect = new System.Windows.Forms.Button();
            this.txtLogs = new System.Windows.Forms.RichTextBox();
            this.cmdShowCoins = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdEcho
            // 
            this.cmdEcho.Location = new System.Drawing.Point(80, 59);
            this.cmdEcho.Name = "cmdEcho";
            this.cmdEcho.Size = new System.Drawing.Size(150, 39);
            this.cmdEcho.TabIndex = 0;
            this.cmdEcho.Text = "Echo";
            this.cmdEcho.UseVisualStyleBackColor = true;
            this.cmdEcho.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Logs";
            // 
            // cmdImport
            // 
            this.cmdImport.Location = new System.Drawing.Point(80, 116);
            this.cmdImport.Name = "cmdImport";
            this.cmdImport.Size = new System.Drawing.Size(150, 39);
            this.cmdImport.TabIndex = 3;
            this.cmdImport.Text = "Import";
            this.cmdImport.UseVisualStyleBackColor = true;
            this.cmdImport.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // cmdExport
            // 
            this.cmdExport.Location = new System.Drawing.Point(80, 184);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(150, 39);
            this.cmdExport.TabIndex = 4;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            // 
            // cmdBank
            // 
            this.cmdBank.Location = new System.Drawing.Point(80, 247);
            this.cmdBank.Name = "cmdBank";
            this.cmdBank.Size = new System.Drawing.Size(150, 39);
            this.cmdBank.TabIndex = 5;
            this.cmdBank.Text = "Bank";
            this.cmdBank.UseVisualStyleBackColor = true;
            // 
            // cmdDetect
            // 
            this.cmdDetect.Location = new System.Drawing.Point(80, 312);
            this.cmdDetect.Name = "cmdDetect";
            this.cmdDetect.Size = new System.Drawing.Size(150, 39);
            this.cmdDetect.TabIndex = 6;
            this.cmdDetect.Text = "Detect";
            this.cmdDetect.UseVisualStyleBackColor = true;
            // 
            // txtLogs
            // 
            this.txtLogs.Location = new System.Drawing.Point(285, 43);
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.Size = new System.Drawing.Size(428, 347);
            this.txtLogs.TabIndex = 7;
            this.txtLogs.Text = "";
            // 
            // cmdShowCoins
            // 
            this.cmdShowCoins.Location = new System.Drawing.Point(80, 367);
            this.cmdShowCoins.Name = "cmdShowCoins";
            this.cmdShowCoins.Size = new System.Drawing.Size(150, 39);
            this.cmdShowCoins.TabIndex = 8;
            this.cmdShowCoins.Text = "Show Coins";
            this.cmdShowCoins.UseVisualStyleBackColor = true;
            this.cmdShowCoins.Click += new System.EventHandler(this.cmdShowCoins_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 418);
            this.Controls.Add(this.cmdShowCoins);
            this.Controls.Add(this.txtLogs);
            this.Controls.Add(this.cmdDetect);
            this.Controls.Add(this.cmdBank);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.cmdImport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdEcho);
            this.Name = "Form1";
            this.Text = "CloudCoin - Windows Application";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdEcho;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdImport;
        private System.Windows.Forms.Button cmdExport;
        private System.Windows.Forms.Button cmdBank;
        private System.Windows.Forms.Button cmdDetect;
        private System.Windows.Forms.RichTextBox txtLogs;
        private System.Windows.Forms.Button cmdShowCoins;
    }
}

