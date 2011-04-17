using IdSharp.Tagging.Harness.WinForms.UserControls;

namespace IdSharp.Tagging.Harness.WinForms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tpFile = new System.Windows.Forms.TabPage();
            this.tabFile = new System.Windows.Forms.TabControl();
            this.tpID3v2 = new System.Windows.Forms.TabPage();
            this.ucID3v2 = new ID3v2UserControl();
            this.tpID3v1 = new System.Windows.Forms.TabPage();
            this.ucID3v1 = new ID3v1UserControl();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnRemoveID3v1 = new System.Windows.Forms.Button();
            this.btnRemoveID3v2 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tpScan = new System.Windows.Forms.TabPage();
            this.btnScan = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnChooseDirectory = new System.Windows.Forms.Button();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.prgScanFiles = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.audioOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabMain.SuspendLayout();
            this.tpFile.SuspendLayout();
            this.tabFile.SuspendLayout();
            this.tpID3v2.SuspendLayout();
            this.tpID3v1.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tpScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(689, 486);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 23);
            this.btnClose.TabIndex = 90;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(9, 9);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 13);
            this.lblVersion.TabIndex = 24;
            this.lblVersion.Text = "[Version]";
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tpFile);
            this.tabMain.Controls.Add(this.tpScan);
            this.tabMain.Location = new System.Drawing.Point(12, 31);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(771, 449);
            this.tabMain.TabIndex = 99;
            // 
            // tpFile
            // 
            this.tpFile.Controls.Add(this.tabFile);
            this.tpFile.Controls.Add(this.pnlButtons);
            this.tpFile.Location = new System.Drawing.Point(4, 22);
            this.tpFile.Name = "tpFile";
            this.tpFile.Padding = new System.Windows.Forms.Padding(3);
            this.tpFile.Size = new System.Drawing.Size(763, 423);
            this.tpFile.TabIndex = 4;
            this.tpFile.Text = "File";
            this.tpFile.UseVisualStyleBackColor = true;
            // 
            // tabFile
            // 
            this.tabFile.Controls.Add(this.tpID3v2);
            this.tabFile.Controls.Add(this.tpID3v1);
            this.tabFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFile.Location = new System.Drawing.Point(3, 3);
            this.tabFile.Name = "tabFile";
            this.tabFile.SelectedIndex = 0;
            this.tabFile.Size = new System.Drawing.Size(757, 385);
            this.tabFile.TabIndex = 0;
            // 
            // tpID3v2
            // 
            this.tpID3v2.Controls.Add(this.ucID3v2);
            this.tpID3v2.Location = new System.Drawing.Point(4, 22);
            this.tpID3v2.Name = "tpID3v2";
            this.tpID3v2.Padding = new System.Windows.Forms.Padding(3);
            this.tpID3v2.Size = new System.Drawing.Size(749, 359);
            this.tpID3v2.TabIndex = 0;
            this.tpID3v2.Text = "ID3v2";
            this.tpID3v2.UseVisualStyleBackColor = true;
            // 
            // ucID3v2
            // 
            this.ucID3v2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucID3v2.Location = new System.Drawing.Point(3, 3);
            this.ucID3v2.Name = "ucID3v2";
            this.ucID3v2.Size = new System.Drawing.Size(743, 353);
            this.ucID3v2.TabIndex = 0;
            // 
            // tpID3v1
            // 
            this.tpID3v1.Controls.Add(this.ucID3v1);
            this.tpID3v1.Location = new System.Drawing.Point(4, 22);
            this.tpID3v1.Name = "tpID3v1";
            this.tpID3v1.Padding = new System.Windows.Forms.Padding(3);
            this.tpID3v1.Size = new System.Drawing.Size(749, 359);
            this.tpID3v1.TabIndex = 1;
            this.tpID3v1.Text = "ID3v1";
            this.tpID3v1.UseVisualStyleBackColor = true;
            // 
            // ucID3v1
            // 
            this.ucID3v1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucID3v1.Location = new System.Drawing.Point(3, 3);
            this.ucID3v1.Name = "ucID3v1";
            this.ucID3v1.Size = new System.Drawing.Size(743, 353);
            this.ucID3v1.TabIndex = 0;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnRemoveID3v1);
            this.pnlButtons.Controls.Add(this.btnRemoveID3v2);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnLoad);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(3, 388);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(757, 32);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnRemoveID3v1
            // 
            this.btnRemoveID3v1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveID3v1.Enabled = false;
            this.btnRemoveID3v1.Location = new System.Drawing.Point(302, 6);
            this.btnRemoveID3v1.Name = "btnRemoveID3v1";
            this.btnRemoveID3v1.Size = new System.Drawing.Size(94, 23);
            this.btnRemoveID3v1.TabIndex = 124;
            this.btnRemoveID3v1.Text = "R&emove ID3v1";
            this.btnRemoveID3v1.UseVisualStyleBackColor = true;
            this.btnRemoveID3v1.Click += new System.EventHandler(this.btnRemoveID3v1_Click);
            // 
            // btnRemoveID3v2
            // 
            this.btnRemoveID3v2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveID3v2.Enabled = false;
            this.btnRemoveID3v2.Location = new System.Drawing.Point(202, 6);
            this.btnRemoveID3v2.Name = "btnRemoveID3v2";
            this.btnRemoveID3v2.Size = new System.Drawing.Size(94, 23);
            this.btnRemoveID3v2.TabIndex = 123;
            this.btnRemoveID3v2.Text = "&Remove ID3v2";
            this.btnRemoveID3v2.UseVisualStyleBackColor = true;
            this.btnRemoveID3v2.Click += new System.EventHandler(this.btnRemoveID3v2_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(102, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 23);
            this.btnSave.TabIndex = 122;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(2, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(94, 23);
            this.btnLoad.TabIndex = 121;
            this.btnLoad.Text = "&Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // tpScan
            // 
            this.tpScan.Controls.Add(this.btnScan);
            this.tpScan.Controls.Add(this.dataGridView1);
            this.tpScan.Controls.Add(this.btnChooseDirectory);
            this.tpScan.Controls.Add(this.txtDirectory);
            this.tpScan.Controls.Add(this.lblDirectory);
            this.tpScan.Controls.Add(this.prgScanFiles);
            this.tpScan.Location = new System.Drawing.Point(4, 22);
            this.tpScan.Name = "tpScan";
            this.tpScan.Padding = new System.Windows.Forms.Padding(3);
            this.tpScan.Size = new System.Drawing.Size(763, 423);
            this.tpScan.TabIndex = 1;
            this.tpScan.Text = "Scan";
            this.tpScan.UseVisualStyleBackColor = true;
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(666, 9);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 20);
            this.btnScan.TabIndex = 26;
            this.btnScan.Text = "&Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colGenre,
            this.colYear,
            this.colFilename});
            this.dataGridView1.Location = new System.Drawing.Point(17, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(724, 370);
            this.dataGridView1.TabIndex = 25;
            // 
            // colArtist
            // 
            this.colArtist.DataPropertyName = "Artist";
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colAlbum
            // 
            this.colAlbum.DataPropertyName = "Album";
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            // 
            // colGenre
            // 
            this.colGenre.DataPropertyName = "Genre";
            this.colGenre.HeaderText = "Genre";
            this.colGenre.Name = "colGenre";
            this.colGenre.ReadOnly = true;
            // 
            // colYear
            // 
            this.colYear.DataPropertyName = "Year";
            this.colYear.HeaderText = "Year";
            this.colYear.Name = "colYear";
            this.colYear.ReadOnly = true;
            // 
            // colFilename
            // 
            this.colFilename.DataPropertyName = "Filename";
            this.colFilename.HeaderText = "Filename";
            this.colFilename.Name = "colFilename";
            this.colFilename.ReadOnly = true;
            // 
            // btnChooseDirectory
            // 
            this.btnChooseDirectory.Location = new System.Drawing.Point(414, 9);
            this.btnChooseDirectory.Name = "btnChooseDirectory";
            this.btnChooseDirectory.Size = new System.Drawing.Size(24, 20);
            this.btnChooseDirectory.TabIndex = 24;
            this.btnChooseDirectory.Text = "...";
            this.btnChooseDirectory.UseVisualStyleBackColor = true;
            this.btnChooseDirectory.Click += new System.EventHandler(this.btnChooseDirectory_Click);
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(98, 9);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(310, 20);
            this.txtDirectory.TabIndex = 22;
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(14, 12);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(49, 13);
            this.lblDirectory.TabIndex = 23;
            this.lblDirectory.Text = "Directory";
            // 
            // prgScanFiles
            // 
            this.prgScanFiles.Location = new System.Drawing.Point(17, 9);
            this.prgScanFiles.Name = "prgScanFiles";
            this.prgScanFiles.Size = new System.Drawing.Size(549, 20);
            this.prgScanFiles.TabIndex = 27;
            this.prgScanFiles.Visible = false;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // audioOpenFileDialog
            // 
            this.audioOpenFileDialog.Filter = "*.mp3|*.mp3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 521);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabMain);
            this.MinimumSize = new System.Drawing.Size(696, 380);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IdSharp Harness";
            this.tabMain.ResumeLayout(false);
            this.tpFile.ResumeLayout(false);
            this.tabFile.ResumeLayout(false);
            this.tpID3v2.ResumeLayout(false);
            this.tpID3v1.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tpScan.ResumeLayout(false);
            this.tpScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpScan;
        private System.Windows.Forms.Button btnChooseDirectory;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ProgressBar prgScanFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.TabPage tpFile;
        private ID3v2UserControl ucID3v2;
        private ID3v1UserControl ucID3v1;
        private System.Windows.Forms.TabControl tabFile;
        private System.Windows.Forms.TabPage tpID3v2;
        private System.Windows.Forms.TabPage tpID3v1;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnRemoveID3v2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.OpenFileDialog audioOpenFileDialog;
        private System.Windows.Forms.Button btnRemoveID3v1;

    }
}

