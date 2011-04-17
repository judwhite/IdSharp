using System;
using System.IO;
using System.Windows.Forms;
using IdSharp.Tagging.ID3v1;

namespace IdSharp.Tagging.Harness.WinForms.UserControls
{
    public partial class ID3v1UserControl : UserControl
    {
        private IID3v1Tag _id3v1;

        public ID3v1UserControl()
        {
            InitializeComponent();

            cmbGenre.Sorted = true;
            cmbGenre.Items.AddRange(GenreHelper.GenreByIndex);
        }

        private void cmbID3v1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ID3v1.0
            if (cmbID3v1.SelectedIndex == 0)
            {
                txtTrackNumber.Enabled = false;
                txtTrackNumber.Text = "";
                txtComment.MaxLength = 30;
            }
            // ID3v1.1
            else
            {
                txtTrackNumber.Enabled = true;
                txtComment.MaxLength = 28;
                // Resets to the appropriate length for the ID3v1 version.
                // Normally this would be handled by binding.  This will be updated in later versions of the harness.
                // TODO
                _id3v1.Comment = txtComment.Text;
                txtComment.Text = _id3v1.Comment;
            }
        }

        public void LoadFile(string path)
        {
            _id3v1 = new ID3v1Tag(path);

            txtFilename.Text = Path.GetFileName(path);
            txtArtist.Text = _id3v1.Artist;
            txtTitle.Text = _id3v1.Title;
            txtAlbum.Text = _id3v1.Album;
            cmbGenre.SelectedIndex = cmbGenre.Items.IndexOf(GenreHelper.GenreByIndex[_id3v1.GenreIndex]);
            txtYear.Text = _id3v1.Year;
            txtComment.Text = _id3v1.Comment;
            if (_id3v1.TrackNumber > 0)
                txtTrackNumber.Text = _id3v1.TrackNumber.ToString();
            else
                txtTrackNumber.Text = string.Empty;

            switch (_id3v1.TagVersion)
            {
                case ID3v1TagVersion.ID3v10:
                    cmbID3v1.SelectedIndex = cmbID3v1.Items.IndexOf("ID3v1.0");
                    break;
                case ID3v1TagVersion.ID3v11:
                    cmbID3v1.SelectedIndex = cmbID3v1.Items.IndexOf("ID3v1.1");
                    break;
            }
        }

        public void SaveFile(string path)
        {
            if (_id3v1 == null)
            {
                MessageBox.Show("Nothing to save!");
                return;
            }

            _id3v1.TagVersion = (cmbID3v1.SelectedIndex == 0 ? ID3v1TagVersion.ID3v10 : ID3v1TagVersion.ID3v11);
            _id3v1.Artist = txtArtist.Text;
            _id3v1.Title = txtTitle.Text;
            _id3v1.Album = txtAlbum.Text;
            _id3v1.GenreIndex = GenreHelper.GetGenreIndex(cmbGenre.Text);
            _id3v1.Year = txtYear.Text;
            _id3v1.Comment = txtComment.Text;
            int trackNumber;
            int.TryParse(txtTrackNumber.Text, out trackNumber);
            _id3v1.TrackNumber = trackNumber;

            _id3v1.Save(path);
        }
    }
}
