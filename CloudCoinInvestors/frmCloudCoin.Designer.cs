namespace CloudCoinInvestors
{
    partial class frmCloudCoin
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.echoRAIDAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCoinsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixFrackedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdEcho = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotalCoins = new System.Windows.Forms.Label();
            this.lbl250Total = new System.Windows.Forms.Label();
            this.lblHundredTotal = new System.Windows.Forms.Label();
            this.lblQtrTotal = new System.Windows.Forms.Label();
            this.lblFiveTotal = new System.Windows.Forms.Label();
            this.lblOneTotal = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl250Count = new System.Windows.Forms.Label();
            this.lblHundredCount = new System.Windows.Forms.Label();
            this.lblQtrCount = new System.Windows.Forms.Label();
            this.lblFiveCount = new System.Windows.Forms.Label();
            this.lblOneCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdImport = new System.Windows.Forms.Button();
            this.txtLogs = new System.Windows.Forms.RichTextBox();
            this.importWorker = new System.ComponentModel.BackgroundWorker();
            this.cmdExport = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(870, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.echoRAIDAToolStripMenuItem,
            this.showCoinsToolStripMenuItem,
            this.toolStripSeparator2,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.fixFrackedToolStripMenuItem,
            this.showFoldersToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // echoRAIDAToolStripMenuItem
            // 
            this.echoRAIDAToolStripMenuItem.Name = "echoRAIDAToolStripMenuItem";
            this.echoRAIDAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.echoRAIDAToolStripMenuItem.Text = "Echo RAIDA";
            // 
            // showCoinsToolStripMenuItem
            // 
            this.showCoinsToolStripMenuItem.Name = "showCoinsToolStripMenuItem";
            this.showCoinsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showCoinsToolStripMenuItem.Text = "Show Coins";
            this.showCoinsToolStripMenuItem.Click += new System.EventHandler(this.showCoinsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // fixFrackedToolStripMenuItem
            // 
            this.fixFrackedToolStripMenuItem.Name = "fixFrackedToolStripMenuItem";
            this.fixFrackedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fixFrackedToolStripMenuItem.Text = "Fix Fracked";
            // 
            // showFoldersToolStripMenuItem
            // 
            this.showFoldersToolStripMenuItem.Name = "showFoldersToolStripMenuItem";
            this.showFoldersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showFoldersToolStripMenuItem.Text = "Show Folders";
            this.showFoldersToolStripMenuItem.Click += new System.EventHandler(this.showFoldersToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // cmdEcho
            // 
            this.cmdEcho.Location = new System.Drawing.Point(92, 76);
            this.cmdEcho.Name = "cmdEcho";
            this.cmdEcho.Size = new System.Drawing.Size(75, 23);
            this.cmdEcho.TabIndex = 1;
            this.cmdEcho.Text = "Echo";
            this.cmdEcho.UseVisualStyleBackColor = true;
            this.cmdEcho.Click += new System.EventHandler(this.cmdEcho_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTotalCoins);
            this.groupBox1.Controls.Add(this.lbl250Total);
            this.groupBox1.Controls.Add(this.lblHundredTotal);
            this.groupBox1.Controls.Add(this.lblQtrTotal);
            this.groupBox1.Controls.Add(this.lblFiveTotal);
            this.groupBox1.Controls.Add(this.lblOneTotal);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lbl250Count);
            this.groupBox1.Controls.Add(this.lblHundredCount);
            this.groupBox1.Controls.Add(this.lblQtrCount);
            this.groupBox1.Controls.Add(this.lblFiveCount);
            this.groupBox1.Controls.Add(this.lblOneCount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(229, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 217);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bank";
            // 
            // lblTotalCoins
            // 
            this.lblTotalCoins.AutoSize = true;
            this.lblTotalCoins.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCoins.Location = new System.Drawing.Point(108, 176);
            this.lblTotalCoins.Name = "lblTotalCoins";
            this.lblTotalCoins.Size = new System.Drawing.Size(144, 24);
            this.lblTotalCoins.TabIndex = 18;
            this.lblTotalCoins.Text = "Total Coins : 0";
            // 
            // lbl250Total
            // 
            this.lbl250Total.AutoSize = true;
            this.lbl250Total.Location = new System.Drawing.Point(289, 135);
            this.lbl250Total.Name = "lbl250Total";
            this.lbl250Total.Size = new System.Drawing.Size(13, 13);
            this.lbl250Total.TabIndex = 17;
            this.lbl250Total.Text = "0";
            // 
            // lblHundredTotal
            // 
            this.lblHundredTotal.AutoSize = true;
            this.lblHundredTotal.Location = new System.Drawing.Point(239, 135);
            this.lblHundredTotal.Name = "lblHundredTotal";
            this.lblHundredTotal.Size = new System.Drawing.Size(13, 13);
            this.lblHundredTotal.TabIndex = 16;
            this.lblHundredTotal.Text = "0";
            // 
            // lblQtrTotal
            // 
            this.lblQtrTotal.AutoSize = true;
            this.lblQtrTotal.Location = new System.Drawing.Point(183, 135);
            this.lblQtrTotal.Name = "lblQtrTotal";
            this.lblQtrTotal.Size = new System.Drawing.Size(13, 13);
            this.lblQtrTotal.TabIndex = 15;
            this.lblQtrTotal.Text = "0";
            // 
            // lblFiveTotal
            // 
            this.lblFiveTotal.AutoSize = true;
            this.lblFiveTotal.Location = new System.Drawing.Point(140, 135);
            this.lblFiveTotal.Name = "lblFiveTotal";
            this.lblFiveTotal.Size = new System.Drawing.Size(13, 13);
            this.lblFiveTotal.TabIndex = 14;
            this.lblFiveTotal.Text = "0";
            // 
            // lblOneTotal
            // 
            this.lblOneTotal.AutoSize = true;
            this.lblOneTotal.Location = new System.Drawing.Point(89, 135);
            this.lblOneTotal.Name = "lblOneTotal";
            this.lblOneTotal.Size = new System.Drawing.Size(13, 13);
            this.lblOneTotal.TabIndex = 13;
            this.lblOneTotal.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 135);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Total";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 59);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Denm.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Count";
            // 
            // lbl250Count
            // 
            this.lbl250Count.AutoSize = true;
            this.lbl250Count.Location = new System.Drawing.Point(289, 96);
            this.lbl250Count.Name = "lbl250Count";
            this.lbl250Count.Size = new System.Drawing.Size(13, 13);
            this.lbl250Count.TabIndex = 9;
            this.lbl250Count.Text = "0";
            // 
            // lblHundredCount
            // 
            this.lblHundredCount.AutoSize = true;
            this.lblHundredCount.Location = new System.Drawing.Point(239, 96);
            this.lblHundredCount.Name = "lblHundredCount";
            this.lblHundredCount.Size = new System.Drawing.Size(13, 13);
            this.lblHundredCount.TabIndex = 8;
            this.lblHundredCount.Text = "0";
            // 
            // lblQtrCount
            // 
            this.lblQtrCount.AutoSize = true;
            this.lblQtrCount.Location = new System.Drawing.Point(183, 96);
            this.lblQtrCount.Name = "lblQtrCount";
            this.lblQtrCount.Size = new System.Drawing.Size(13, 13);
            this.lblQtrCount.TabIndex = 7;
            this.lblQtrCount.Text = "0";
            // 
            // lblFiveCount
            // 
            this.lblFiveCount.AutoSize = true;
            this.lblFiveCount.Location = new System.Drawing.Point(140, 96);
            this.lblFiveCount.Name = "lblFiveCount";
            this.lblFiveCount.Size = new System.Drawing.Size(13, 13);
            this.lblFiveCount.TabIndex = 6;
            this.lblFiveCount.Text = "0";
            // 
            // lblOneCount
            // 
            this.lblOneCount.AutoSize = true;
            this.lblOneCount.Location = new System.Drawing.Point(89, 96);
            this.lblOneCount.Name = "lblOneCount";
            this.lblOneCount.Size = new System.Drawing.Size(13, 13);
            this.lblOneCount.TabIndex = 5;
            this.lblOneCount.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "250s";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(232, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "100s";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "25s";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "5s";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "1s";
            // 
            // cmdImport
            // 
            this.cmdImport.Location = new System.Drawing.Point(92, 123);
            this.cmdImport.Name = "cmdImport";
            this.cmdImport.Size = new System.Drawing.Size(75, 23);
            this.cmdImport.TabIndex = 3;
            this.cmdImport.Text = "Import";
            this.cmdImport.UseVisualStyleBackColor = true;
            this.cmdImport.Click += new System.EventHandler(this.cmdImport_Click);
            // 
            // txtLogs
            // 
            this.txtLogs.Location = new System.Drawing.Point(67, 273);
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLogs.Size = new System.Drawing.Size(538, 197);
            this.txtLogs.TabIndex = 4;
            this.txtLogs.Text = "";
            // 
            // importWorker
            // 
            this.importWorker.WorkerReportsProgress = true;
            this.importWorker.WorkerSupportsCancellation = true;
            this.importWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ImportWorker_DoWork);
            // 
            // cmdExport
            // 
            this.cmdExport.Location = new System.Drawing.Point(92, 170);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(75, 23);
            this.cmdExport.TabIndex = 5;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // frmCloudCoin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 497);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.txtLogs);
            this.Controls.Add(this.cmdImport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdEcho);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmCloudCoin";
            this.Text = "Cloudcoin Investors Edition";
            this.Load += new System.EventHandler(this.frmCloudCoin_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem echoRAIDAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCoinsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixFrackedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button cmdEcho;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl250Total;
        private System.Windows.Forms.Label lblHundredTotal;
        private System.Windows.Forms.Label lblQtrTotal;
        private System.Windows.Forms.Label lblFiveTotal;
        private System.Windows.Forms.Label lblOneTotal;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl250Count;
        private System.Windows.Forms.Label lblHundredCount;
        private System.Windows.Forms.Label lblQtrCount;
        private System.Windows.Forms.Label lblFiveCount;
        private System.Windows.Forms.Label lblOneCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalCoins;
        private System.Windows.Forms.Button cmdImport;
        private System.Windows.Forms.RichTextBox txtLogs;
        private System.ComponentModel.BackgroundWorker importWorker;
        private System.Windows.Forms.Button cmdExport;
    }
}

