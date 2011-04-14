using System.Collections.Generic;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// Static helper class for <see cref="PictureType"/>.
    /// </summary>
    public static class PictureTypeHelper
    {
        private static readonly SortedDictionary<string, PictureType> m_PictureTypeDictionary;
        private static readonly string[] m_PictureTypes;

        /// <summary>
        /// Gets the picture types.
        /// </summary>
        /// <value>The picture types.</value>
        public static string[] PictureTypes
        {
            get { return m_PictureTypes; }
        }

        /// <summary>
        /// Gets the picture type from a string.
        /// </summary>
        /// <param name="pictureTypeString">The picture type string.</param>
        /// <returns>The picture type.</returns>
        public static PictureType GetPictureTypeFromString(string pictureTypeString)
        {
            PictureType pictureType;
            if (m_PictureTypeDictionary.TryGetValue(pictureTypeString, out pictureType))
                return pictureType;
            else
                return PictureType.Other;
        }

        /// <summary>
        /// Gets a string from a picture picture.
        /// </summary>
        /// <param name="pictureType">Type of the picture.</param>
        public static string GetStringFromPictureType(PictureType pictureType)
        {
            foreach (KeyValuePair<string, PictureType> pictureTypeKVP in m_PictureTypeDictionary)
            {
                if (pictureTypeKVP.Value == pictureType)
                    return pictureTypeKVP.Key;
            }

            return "Other";
        }

        static PictureTypeHelper()
        {
            m_PictureTypeDictionary = new SortedDictionary<string, PictureType>();
            m_PictureTypeDictionary.Add("Other", PictureType.Other);
            m_PictureTypeDictionary.Add("File icon (32x32 PNG)", PictureType.FileIcon32x32Png);
            m_PictureTypeDictionary.Add("Other file icon", PictureType.OtherFileIcon);
            m_PictureTypeDictionary.Add("Cover (front)", PictureType.CoverFront);
            m_PictureTypeDictionary.Add("Cover (back)", PictureType.CoverBack);
            m_PictureTypeDictionary.Add("Leaflet page", PictureType.LeafletPage);
            m_PictureTypeDictionary.Add("Media (e.g. label side of CD)", PictureType.MediaLabelSideOfCD);
            m_PictureTypeDictionary.Add("Lead artist/performer", PictureType.LeadArtistPerformer);
            m_PictureTypeDictionary.Add("Artist/performer", PictureType.ArtistPerformer);
            m_PictureTypeDictionary.Add("Conductor", PictureType.Conductor);
            m_PictureTypeDictionary.Add("Band/orchestra", PictureType.BandOrchestra);
            m_PictureTypeDictionary.Add("Composer", PictureType.Composer);
            m_PictureTypeDictionary.Add("Lyricist", PictureType.Lyricist);
            m_PictureTypeDictionary.Add("Recording location", PictureType.RecordingLocation);
            m_PictureTypeDictionary.Add("During recording", PictureType.DuringRecording);
            m_PictureTypeDictionary.Add("During performance", PictureType.DuringPerformance);
            m_PictureTypeDictionary.Add("Movie/video screen capture", PictureType.MovieVideoScreenCapture);
            m_PictureTypeDictionary.Add("Illustration", PictureType.Illustration);
            m_PictureTypeDictionary.Add("Band/artist logo", PictureType.BandArtistLogo);
            m_PictureTypeDictionary.Add("Publisher/studio logo", PictureType.PublisherStudioLogo);

            m_PictureTypes = new string[m_PictureTypeDictionary.Count];
            int i = 0;
            foreach (string pictureType in m_PictureTypeDictionary.Keys)
            {
                m_PictureTypes[i++] = pictureType;
            }
        }
    }
}
