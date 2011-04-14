namespace IdSharp.Tagging.Harness.WinForms.UserControls
{
    partial class ID3v1UserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblArtist = new System.Windows.Forms.Label();
            this.txtArtist = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtAlbum = new System.Windows.Forms.TextBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAlbum = new System.Windows.Forms.Label();
            this.lblTrack = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.txtTrackNumber = new System.Windows.Forms.TextBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.cmbID3v1 = new System.Windows.Forms.ComboBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.lblID3v1 = new System.Windows.Forms.Label();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(94, 9);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(310, 20);
            this.txtFilename.TabIndex = 101;
            this.txtFilename.TabStop = false;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Location = new System.Drawing.Point(10, 65);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(30, 13);
            this.lblArtist.TabIndex = 106;
            this.lblArtist.Text = "Artist";
            // 
            // txtArtist
            // 
            this.txtArtist.Location = new System.Drawing.Point(94, 62);
            this.txtArtist.MaxLength = 30;
            this.txtArtist.Name = "txtArtist";
            this.txtArtist.Size = new System.Drawing.Size(310, 20);
            this.txtArtist.TabIndex = 104;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(94, 88);
            this.txtTitle.MaxLength = 30;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(310, 20);
            this.txtTitle.TabIndex = 105;
            // 
            // txtAlbum
            // 
            this.txtAlbum.Location = new System.Drawing.Point(94, 114);
            this.txtAlbum.MaxLength = 30;
            this.txtAlbum.Name = "txtAlbum";
            this.txtAlbum.Size = new System.Drawing.Size(310, 20);
            this.txtAlbum.TabIndex = 107;
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(94, 166);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(75, 20);
            this.txtYear.TabIndex = 114;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(10, 91);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 108;
            this.lblTitle.Text = "Title";
            // 
            // lblAlbum
            // 
            this.lblAlbum.AutoSize = true;
            this.lblAlbum.Location = new System.Drawing.Point(10, 117);
            this.lblAlbum.Name = "lblAlbum";
            this.lblAlbum.Size = new System.Drawing.Size(36, 13);
            this.lblAlbum.TabIndex = 109;
            this.lblAlbum.Text = "Album";
            // 
            // lblTrack
            // 
            this.lblTrack.AutoSize = true;
            this.lblTrack.Location = new System.Drawing.Point(10, 195);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Size = new System.Drawing.Size(35, 13);
            this.lblTrack.TabIndex = 122;
            this.lblTrack.Text = "Track";
            // 
            // lblGenre
            // 
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(10, 143);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(36, 13);
            this.lblGenre.TabIndex = 110;
            this.lblGenre.Text = "Genre";
            // 
            // txtTrackNumber
            // 
            this.txtTrackNumber.Location = new System.Drawing.Point(94, 192);
            this.txtTrackNumber.MaxLength = 3;
            this.txtTrackNumber.Name = "txtTrackNumber";
            this.txtTrackNumber.Size = new System.Drawing.Size(75, 20);
            this.txtTrackNumber.TabIndex = 115;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(10, 169);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(29, 13);
            this.lblYear.TabIndex = 112;
            this.lblYear.Text = "Year";
            // 
            // cmbID3v1
            // 
            this.cmbID3v1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbID3v1.FormattingEnabled = true;
            this.cmbID3v1.Items.AddRange(new object[] {
            "ID3v1.0",
            "ID3v1.1"});
            this.cmbID3v1.Location = new System.Drawing.Point(94, 35);
            this.cmbID3v1.Name = "cmbID3v1";
            this.cmbID3v1.Size = new System.Drawing.Size(75, 21);
            this.cmbID3v1.TabIndex = 103;
            this.cmbID3v1.SelectedIndexChanged += new System.EventHandler(this.cmbID3v1_SelectedIndexChanged);
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(10, 12);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(52, 13);
            this.lblFilename.TabIndex = 113;
            this.lblFilename.Text = "File name";
            // 
            // lblID3v1
            // 
            this.lblID3v1.AutoSize = true;
            this.lblID3v1.Location = new System.Drawing.Point(10, 38);
            this.lblID3v1.Name = "lblID3v1";
            this.lblID3v1.Size = new System.Drawing.Size(36, 13);
            this.lblID3v1.TabIndex = 121;
            this.lblID3v1.Text = "ID3v1";
            // 
            // cmbGenre
            // 
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(94, 140);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(146, 21);
            this.cmbGenre.TabIndex = 111;
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(94, 218);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(310, 20);
            this.txtComment.TabIndex = 123;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(10, 221);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(51, 13);
            this.lblComment.TabIndex = 124;
            this.lblComment.Text = "Comment";
            // 
            // ID3v1UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblArtist);
            this.Controls.Add(this.txtArtist);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.txtAlbum);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblAlbum);
            this.Controls.Add(this.lblTrack);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.txtTrackNumber);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.cmbID3v1);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.lblID3v1);
            this.Controls.Add(this.cmbGenre);
            this.Name = "ID3v1UserControl";
            this.Size = new System.Drawing.Size(704, 333);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.TextBox txtArtist;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtAlbum;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.Label lblTrack;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.TextBox txtTrackNumber;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cmbID3v1;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.Label lblID3v1;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblComment;
    }
}
