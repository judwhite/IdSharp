using System.Collections.Generic;
using System.Linq;

namespace IdSharp.Tagging.ID3v1
{
    /// <summary>
    /// Static helper class for ID3v1 Genres.
    /// </summary>
    public static class GenreHelper
    {
        #region <<< Genre by ID3v1 index >>>

        private static readonly string[] _genreByIndex = new[] {
            "Blues",
            "Classic Rock",
            "Country",
            "Dance",
            "Disco",
            "Funk",
            "Grunge",
            "Hip-Hop",
            "Jazz",
            "Metal",
            "New Age",
            "Oldies",
            "Other",
            "Pop",
            "R&B",
            "Rap",
            "Reggae",
            "Rock",
            "Techno",
            "Industrial",
            "Alternative",
            "Ska",
            "Death Metal",
            "Pranks",
            "Soundtrack",
            "Euro-Techno",
            "Ambient",
            "Trip-Hop",
            "Vocal",
            "Jazz+Funk",
            "Fusion",
            "Trance",
            "Classical",
            "Instrumental",
            "Acid",
            "House",
            "Game",
            "Sound Clip",
            "Gospel",
            "Noise",
            "Alt. Rock",
            "Bass",
            "Soul",
            "Punk",
            "Space",
            "Meditative",
            "Instrumental Pop",
            "Instrumental Rock",
            "Ethnic",
            "Gothic",
            "Darkwave",
            "Techno-Industrial",
            "Electronic",
            "Pop-Folk",
            "Eurodance",
            "Dream",
            "Southern Rock",
            "Comedy",
            "Cult",
            "Gangsta Rap",
            "Top 40",
            "Christian Rap",
            "Pop/Funk",
            "Jungle",
            "Native American",
            "Cabaret",
            "New Wave",
            "Psychedelic",
            "Rave",
            "Showtunes",
            "Trailer",
            "Lo-Fi",
            "Tribal",
            "Acid Punk",
            "Acid Jazz",
            "Polka",
            "Retro",
            "Musical",
            "Rock & Roll",
            "Hard Rock",
            "Folk",
            "Folk/Rock",
            "National Folk",
            "Swing",
            "Fast-Fusion",
            "Bebob",
            "Latin",
            "Revival",
            "Celtic",
            "Bluegrass",
            "Avantgarde",
            "Gothic Rock",
            "Progressive Rock",
            "Psychedelic Rock",
            "Symphonic Rock",
            "Slow Rock",
            "Big Band",
            "Chorus",
            "Easy Listening",
            "Acoustic",
            "Humour",
            "Speech",
            "Chanson",
            "Opera",
            "Chamber Music",
            "Sonata",
            "Symphony",
            "Booty Bass",
            "Primus",
            "Porn Groove",
            "Satire",
            "Slow Jam",
            "Club",
            "Tango",
            "Samba",
            "Folklore",
            "Ballad",
            "Power Ballad",
            "Rhythmic Soul",
            "Freestyle",
            "Duet",
            "Punk Rock",
            "Drum Solo",
            "A Cappella",
            "Euro-House",
            "Dance Hall",
            "Goa",
            "Drum & Bass",
            "Club-House",
            "Hardcore",
            "Terror",
            "Indie",
            "BritPop",
            "Negerpunk",
            "Polsk Punk",
            "Beat",
            "Christian Gangsta Rap",
            "Heavy Metal",
            "Black Metal",
            "Crossover",
            "Contemporary Christian",
            "Christian Rock",
            "Merengue",
            "Salsa",
            "Thrash Metal",
            "Anime",
            "Jpop",
            "Synthpop"};
        #endregion <<< Genre by index >>>

        private static readonly string[] _sortedGenreList;
        private static readonly int _genreCount;

        /// <summary>
        /// The genre index of 'Other' (12).
        /// </summary>
        public const int Other_GenreIndex = 12;

        static GenreHelper()
        {
            _genreCount = _genreByIndex.Length;
            _sortedGenreList = _genreByIndex.OrderBy(p => p).ToArray();
        }

        /// <summary>
        /// Gets a string array of ID3v1 genres sorted by index.
        /// </summary>
        /// <value>A string array of ID3v1 genres sorted by index.</value>
        public static string[] GenreByIndex
        {
            get { return _genreByIndex; }
        }

        /// <summary>
        /// Gets a sorted list of ID3v1 genres.
        /// </summary>
        /// <returns>A sorted list of ID3v1 genres.</returns>
        public static List<string> GetSortedGenreList()
        {
            return new List<string>(_sortedGenreList);
        }

        /// <summary>
        /// Gets the index of the genre.  If the genre is not found, 12 is returned to indicate 'Other'.
        /// </summary>
        /// <param name="genre">The genre.</param>
        /// <returns>The index of the genre.  If the genre is not found, 12 is returned to indicate 'Other'.</returns>
        public static int GetGenreIndex(string genre)
        {
            for (int i = 0; i <= _genreCount; i++)
            {
                if (string.Compare(genre, _genreByIndex[i], true) == 0)
                    return i;
            }
            return Other_GenreIndex;
        }
    }
}
