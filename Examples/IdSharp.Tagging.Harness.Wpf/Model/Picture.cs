using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.Harness.Wpf.Model
{
    public class Picture : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ImageSource _imageSource;
        private string _description;
        private PictureType _pictureType;

        public Picture(IAttachedPicture attachedPicture)
        {
            Description = attachedPicture.Description;
            PictureType = attachedPicture.PictureType;

            byte[] pictureData = attachedPicture.PictureData;
            if (pictureData != null)
            {
                MemoryStream memoryStream = new MemoryStream(attachedPicture.PictureData);
                memoryStream.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                ImageSource = bitmapImage;
            }
        }

        private void SendPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    SendPropertyChanged("ImageSource");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        public PictureType PictureType
        {
            get { return _pictureType; }
            set
            {
                if (_pictureType != value)
                {
                    _pictureType = value;
                    SendPropertyChanged("PictureType");
                }
            }
        }
    }
}
