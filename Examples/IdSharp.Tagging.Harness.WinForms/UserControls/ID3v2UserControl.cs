using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IdSharp.AudioInfo;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.Harness.WinForms.UserControls
{
    public partial class ID3v2UserControl : UserControl
    {
        private IID3v2 _id3v2;

        public ID3v2UserControl()
        {
            InitializeComponent();

            cmbGenre.Sorted = true;
            cmbGenre.Items.AddRange(GenreHelper.GenreByIndex);

            cmbImageType.Items.AddRange(PictureTypeHelper.PictureTypes);
        }

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            if (attachedPicture != null)
                LoadImageData(attachedPicture);
            else
                ClearImageData();
        }

        private void imageContextMenu_Opening(object sender, CancelEventArgs e)
        {
            miSaveImage.Enabled = (this.pictureBox1.Image != null);
            miLoadImage.Enabled = (GetCurrentPictureFrame() != null);
        }

        private void miSaveImage_Click(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            SaveImageToFile(attachedPicture);
        }

        private void miLoadImage_Click(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            LoadImageFromFile(attachedPicture);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            LoadImageFromFile(attachedPicture);
        }

        private void cmbImageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            if (attachedPicture != null)
                attachedPicture.PictureType = PictureTypeHelper.GetPictureTypeFromString(cmbImageType.Text);
        }

        private void txtImageDescription_Validated(object sender, EventArgs e)
        {
            IAttachedPicture attachedPicture = GetCurrentPictureFrame();
            if (attachedPicture != null)
                attachedPicture.Description = txtImageDescription.Text;
        }

        private void LoadImageData(IAttachedPicture attachedPicture)
        {
            pictureBox1.Image = attachedPicture.Picture;

            txtImageDescription.Text = attachedPicture.Description;
            cmbImageType.SelectedIndex = cmbImageType.Items.IndexOf(PictureTypeHelper.GetStringFromPictureType(attachedPicture.PictureType));

            txtImageDescription.Enabled = true;
            cmbImageType.Enabled = true;
        }

        private void ClearImageData()
        {
            pictureBox1.Image = null;
            txtImageDescription.Text = "";
            cmbImageType.SelectedIndex = -1;

            txtImageDescription.Enabled = false;
            cmbImageType.Enabled = false;
        }

        private void SaveImageToFile(IAttachedPicture attachedPicture)
        {
            string extension = attachedPicture.PictureExtension;

            imageSaveFileDialog.FileName = "image." + extension;

            DialogResult dialogResult = imageSaveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                using (FileStream fs = File.Open(imageSaveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    fs.Write(attachedPicture.PictureData, 0, attachedPicture.PictureData.Length);
                }
            }
        }

        private void LoadImageFromFile(IAttachedPicture attachedPicture)
        {
            DialogResult dialogResult = imageOpenFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                attachedPicture.Picture = Image.FromFile(imageOpenFileDialog.FileName);
                pictureBox1.Image = attachedPicture.Picture;
            }
        }

        private IAttachedPicture GetCurrentPictureFrame()
        {
            if (imageBindingNavigator.BindingSource == null)
                return null;
            return imageBindingNavigator.BindingSource.Current as IAttachedPicture;
        }

        public void LoadFile(string path)
        {
            ClearImageData();

            _id3v2 = new ID3v2.ID3v2(path);

            txtFilename.Text = Path.GetFileName(path);
            txtArtist.Text = _id3v2.Artist;
            txtTitle.Text = _id3v2.Title;
            txtAlbum.Text = _id3v2.Album;
            cmbGenre.Text = _id3v2.Genre;
            txtYear.Text = _id3v2.Year;
            txtTrackNumber.Text = _id3v2.TrackNumber;

            BindingSource bindingSource = new BindingSource();
            imageBindingNavigator.BindingSource = bindingSource;
            bindingSource.CurrentChanged += bindingSource_CurrentChanged;
            bindingSource.DataSource = _id3v2.PictureList;

            switch (_id3v2.Header.TagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    cmbID3v2.SelectedIndex = cmbID3v2.Items.IndexOf("ID3v2.2");
                    break;
                case ID3v2TagVersion.ID3v23:
                    cmbID3v2.SelectedIndex = cmbID3v2.Items.IndexOf("ID3v2.3");
                    break;
                case ID3v2TagVersion.ID3v24:
                    cmbID3v2.SelectedIndex = cmbID3v2.Items.IndexOf("ID3v2.4");
                    break;
            }

            txtPlayLength.Text = string.Empty;
            txtBitrate.Text = string.Empty;
            txtFrequency.Text = string.Empty;

            Thread t = new Thread(LoadAudioFileDetails);
            t.Start(path);
        }

        private void LoadAudioFileDetails(object pathObject)
        {
            string path = (string)pathObject;

            IAudioFile audioFile = AudioFile.Create(path, false);
            decimal bitrate = audioFile.Bitrate; // force bitrate calculation

            Invoke(new Action<IAudioFile>(SetAudioFileDetails), audioFile);
        }

        private void SetAudioFileDetails(IAudioFile audioFile)
        {
            txtPlayLength.Text = string.Format("{0}:{1:00}", (int)audioFile.TotalSeconds / 60, (int)audioFile.TotalSeconds % 60);
            txtBitrate.Text = string.Format("{0:#,0} kbps", audioFile.Bitrate);
            txtFrequency.Text = string.Format("{0:#,0} Hz", audioFile.Frequency);
        }

        public void SaveFile(string path)
        {
            if (_id3v2 == null)
            {
                MessageBox.Show("Nothing to save!");
                return;
            }

            if (cmbID3v2.SelectedIndex == cmbID3v2.Items.IndexOf("ID3v2.2"))
                _id3v2.Header.TagVersion = ID3v2TagVersion.ID3v22;
            else if (cmbID3v2.SelectedIndex == cmbID3v2.Items.IndexOf("ID3v2.3"))
                _id3v2.Header.TagVersion = ID3v2TagVersion.ID3v23;
            else if (cmbID3v2.SelectedIndex == cmbID3v2.Items.IndexOf("ID3v2.4"))
                _id3v2.Header.TagVersion = ID3v2TagVersion.ID3v24;
            else
                throw new Exception("Unknown tag version");

            _id3v2.Artist = txtArtist.Text;
            _id3v2.Title = txtTitle.Text;
            _id3v2.Album = txtAlbum.Text;
            _id3v2.Genre = cmbGenre.Text;
            _id3v2.Year = txtYear.Text;
            _id3v2.TrackNumber = txtTrackNumber.Text;

            _id3v2.Save(path);
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_id3v2.PictureList.Count);
        }
    }
}
