using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using IdSharp.Common.Utils;

namespace IdSharp.WebLookup.Amazon
{
    /// <summary>
    /// Amazon images
    /// </summary>
    public class AmazonImages : INotifyPropertyChanged
    {
        private readonly AmazonServer _amazonServer;
        private readonly string _awsAccessKeyId;
        private readonly string _secretAccessKey;
        private readonly string _asin;
        private bool _imageUrlsDownloaded;
        private bool _smallImageDownloaded;
        private bool _mediumImageDownloaded;
        private bool _largeImageDownloaded;
        private byte[] _smallImageBytes;
        private byte[] _mediumImageBytes;
        private byte[] _largeImageBytes;
        private string _smallImageUrl;
        private string _mediumImageUrl;
        private string _largeImageUrl;
        private bool? _hasImage;
        private Size? _largestImageSize;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        internal AmazonImages(AmazonServer server, string awsAccessKeyId, string secretAccessKey, string asin)
        {
            if (string.IsNullOrWhiteSpace(awsAccessKeyId))
                throw new ArgumentNullException("awsAccessKeyId");
            if (string.IsNullOrWhiteSpace(secretAccessKey))
                throw new ArgumentNullException("secretAccessKey");
            if (string.IsNullOrWhiteSpace(asin))
                throw new ArgumentNullException("asin");

            _amazonServer = server;
            _awsAccessKeyId = awsAccessKeyId;
            _secretAccessKey = secretAccessKey;
            _asin = asin;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the small image URL.
        /// </summary>
        /// <value>The small image URL.</value>
        public string SmallImageUrl
        {
            get
            {
                GetImageUrls();
                return _smallImageUrl;
            }
        }

        /// <summary>
        /// Gets the medium image URL.
        /// </summary>
        /// <value>The medium image URL.</value>
        public string MediumImageUrl
        {
            get
            {
                GetImageUrls();
                return _mediumImageUrl;
            }
        }

        /// <summary>
        /// Gets the large image URL.
        /// </summary>
        /// <value>The large image URL.</value>
        public string LargeImageUrl
        {
            get
            {
                GetImageUrls();
                return _largeImageUrl;
            }
        }

        /// <summary>
        /// Gets the small image.
        /// </summary>
        public byte[] SmallImageBytes
        {
            get
            {
                GetSmallImage();
                return _smallImageBytes;
            }
        }

        /// <summary>
        /// Gets the medium image.
        /// </summary>
        public byte[] MediumImageBytes
        {
            get
            {
                GetMediumImage();
                return _mediumImageBytes;
            }
        }

        /// <summary>
        /// Gets the large image.
        /// </summary>
        public byte[] LargeImageBytes
        {
            get
            {
                GetLargeImage();
                return _largeImageBytes;
            }
        }

        /// <summary>
        /// Gets the largest image.
        /// </summary>
        public byte[] GetLargestImageBytes()
        {
            byte[] imageBytes = LargeImageBytes;
            if (imageBytes == null)
                imageBytes = MediumImageBytes;
            if (imageBytes == null)
                imageBytes = SmallImageBytes;

            if (imageBytes == null)
            {
                HasImage = false;
                LargestImageSize = Size.Empty;
            }
            else
            {
                try
                {
                    ImageSource imageSource = GetImageSourceFromBytes(imageBytes);
                    LargestImageSize = new Size(imageSource.Width, imageSource.Height);
                }
                catch
                {
                }

                HasImage = true;
            }

            return imageBytes;
        }

        /// <summary>
        /// Gets a value indicating where this instance contains images. Returns <c>null</c> if unchecked.
        /// </summary>
        /// <value>A value indicating where this instance contains images. Returns <c>null</c> if unchecked.</value>
        public bool? HasImage
        {
            get { return _hasImage; }
            private set
            {
                if (_hasImage != value)
                {
                    _hasImage = value;
                    OnPropertyChanged("HasImage");
                }
            }
        }

        /// <summary>
        /// Gets the largest image size. Returns <c>null</c> if unchecked.
        /// </summary>
        /// <value>The largest image size. Returns <c>null</c> if unchecked.</value>
        public Size? LargestImageSize
        {
            get { return _largestImageSize; }
            set
            {
                if (_largestImageSize != value)
                {
                    _largestImageSize = value;
                    OnPropertyChanged("LargestImageSize");
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="ImageSource"/> from an array of bytes.
        /// </summary>
        /// <param name="imageBytes">The array of bytes.</param>
        /// <returns>An <see cref="ImageSource"/> from an array of bytes.</returns>
        public static ImageSource GetImageSourceFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null)
                return null;

            MemoryStream ms = new MemoryStream(imageBytes);

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = ms;
            src.EndInit();
            return src;
        }

        private void GetSmallImage()
        {
            if (_smallImageDownloaded) 
                return;
            
            GetImageUrls();

            if (_smallImageUrl != null)
            {
                _smallImageBytes = Http.Get(_smallImageUrl);
            }

            _smallImageDownloaded = true;
        }

        private void GetMediumImage()
        {
            if (_mediumImageDownloaded) 
                return;
            
            GetImageUrls();

            if (_mediumImageUrl != null)
            {
                _mediumImageBytes = Http.Get(_mediumImageUrl);
            }

            _mediumImageDownloaded = true;
        }

        private void GetLargeImage()
        {
            if (_largeImageDownloaded) 
                return;
            
            GetImageUrls();

            if (_largeImageUrl != null)
            {
                _largeImageBytes = Http.Get(_largeImageUrl);
            }

            _largeImageDownloaded = true;
        }

        private void GetImageUrls()
        {
            // TODO: Good candidate for System.Xml.Linq

            if (_imageUrlsDownloaded)
                return;

            List<PostData> postData = new List<PostData>();
            postData.Add(new PostData("Service", "AWSECommerceService"));
            postData.Add(new PostData("AWSAccessKeyId", _awsAccessKeyId));
            postData.Add(new PostData("Operation", "ItemLookup"));
            postData.Add(new PostData("ItemId", _asin));
            postData.Add(new PostData("ResponseGroup", "Images"));
            postData.Add(new PostData("Timestamp", string.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}Z", DateTime.UtcNow)));

            string amazonDomain = Amazon.GetDomain(_amazonServer);
            string hostHeader = string.Format("ecs.{0}", amazonDomain);
            string signature = Amazon.GetSignature(postData, hostHeader, _secretAccessKey);
            postData.Add(new PostData("Signature", signature));

            //postData = GetOrderedPostData(postData);

            string requestUri = String.Format("http://{0}{1}", hostHeader, Amazon.HttpRequestUri);
            requestUri = Http.GetQueryString(requestUri, postData);

            byte[] byteResponse = Http.Get(requestUri);
            string response = Encoding.UTF8.GetString(byteResponse);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(response);
            foreach (XmlNode node in xmlDocument.ChildNodes)
            {
                if (node.Name == "ItemLookupResponse")
                {
                    foreach (XmlNode responseNode in node.ChildNodes)
                    {
                        if (responseNode.Name == "Items")
                        {
                            foreach (XmlNode itemNode in responseNode.ChildNodes)
                            {
                                if (itemNode.Name == "Item")
                                {
                                    foreach (XmlNode itemDetail in itemNode.ChildNodes)
                                    {
                                        if (itemDetail.Name == "SmallImage")
                                        {
                                            foreach (XmlNode imageNode in itemDetail.ChildNodes)
                                            {
                                                if (imageNode.Name == "URL")
                                                    _smallImageUrl = imageNode.InnerText;
                                            }
                                        }
                                        else if (itemDetail.Name == "MediumImage")
                                        {
                                            foreach (XmlNode imageNode in itemDetail.ChildNodes)
                                            {
                                                if (imageNode.Name == "URL")
                                                    _mediumImageUrl = imageNode.InnerText;
                                            }
                                        }
                                        else if (itemDetail.Name == "LargeImage")
                                        {
                                            foreach (XmlNode imageNode in itemDetail.ChildNodes)
                                            {
                                                if (imageNode.Name == "URL")
                                                    _largeImageUrl = imageNode.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _imageUrlsDownloaded = true;
        }
    }
}