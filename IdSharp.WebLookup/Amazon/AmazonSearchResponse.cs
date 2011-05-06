using System.Collections.Generic;

namespace IdSharp.WebLookup.Amazon
{
    /// <summary>
    /// Amazon search response
    /// </summary>
    public class AmazonSearchResponse
    {
        private readonly List<AmazonAlbum> _amazonAlbumList = new List<AmazonAlbum>();

        internal AmazonSearchResponse()
        {
        }

        /// <summary>
        /// Gets the total number of results on all pages.
        /// </summary>
        /// <value>The total number of results on all pages.</value>
        public int TotalResults { get; internal set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        /// <value>The total number of pages.</value>
        public int TotalPages { get; internal set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<AmazonAlbum> Items
        {
            get
            {
                return _amazonAlbumList;
            }
        }
    }
}
