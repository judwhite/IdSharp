using System;

namespace IdSharp.WebLookup.Amazon
{
    /// <summary>
    /// Amazon album
    /// </summary>
    public class AmazonAlbum
    {
        private readonly AmazonServer _amazonServer;
        private readonly string _awsAccessKeyId;
        private readonly string _secretAccessKey;
        private string _asin;

        internal AmazonAlbum(AmazonServer server, string awsAccessKeyId, string secretAccessKey)
        {
            if (string.IsNullOrWhiteSpace(awsAccessKeyId))
                throw new ArgumentNullException("awsAccessKeyId");
            if (string.IsNullOrWhiteSpace(secretAccessKey))
                throw new ArgumentNullException("secretAccessKey");

            _amazonServer = server;
            _awsAccessKeyId = awsAccessKeyId;
            _secretAccessKey = secretAccessKey;
        }

        /// <summary>
        /// Gets the ASIN.
        /// </summary>
        /// <value>The ASIN.</value>
        public string Asin
        {
            get
            {
                return _asin;
            }
            internal set
            {
                _asin = value;
                Images = new AmazonImages(_amazonServer, _awsAccessKeyId, _secretAccessKey, _asin);
            }
        }

        /// <summary>
        /// Gets the detail page URL.
        /// </summary>
        /// <value>The detail page URL.</value>
        public string DetailPageUrl { get; internal set; }

        /// <summary>
        /// Gets the artist.
        /// </summary>
        /// <value>The artist.</value>
        public string Artist { get; internal set; }

        /// <summary>
        /// Gets the album.
        /// </summary>
        /// <value>The album.</value>
        public string Album { get; internal set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer { get; internal set; }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <value>The images.</value>
        public AmazonImages Images { get; private set; }
    }
}
