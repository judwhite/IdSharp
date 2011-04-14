using System;
using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;

namespace IdSharp.Tagging.SimpleTag
{
    /// <summary>
    /// A simple wrapper for working with tags.  This class does not require knowing which tag format was used, however
    /// it only offers basic functionality.
    /// </summary>
    public class SimpleTag : ISimpleTag
    {
        #region <<< Private Fields >>>

        private String m_FileName;
        private String m_Title;
        private String m_Artist;
        private String m_Album;
        private String m_Year;
        private String m_Comment;
        private String m_TrackNumber;
        private String m_Genre;
        private String m_TagVersion;

        #endregion <<< Private Fields >>>

        #region <<< INotifyPropertyChanged Members >>>

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion <<< INotifyPropertyChanged Members >>>

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTag"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public SimpleTag(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            Read(path);
        }

        #region <<< ISimpleTag Members >>>

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public String Title
        {
            get { return m_Title; }
            set
            {
                m_Title = value;
                SendPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The artist.</value>
        public String Artist
        {
            get { return m_Artist; }
            set
            {
                m_Artist = value;
                SendPropertyChanged("Artist");
            }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        /// <value>The album.</value>
        public String Album
        {
            get { return m_Album; }
            set
            {
                m_Album = value;
                SendPropertyChanged("Album");
            }
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public String Year
        {
            get { return m_Year; }
            set
            {
                m_Year = value;
                SendPropertyChanged("Year");
            }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public String Comment
        {
            get { return m_Comment; }
            set
            {
                m_Comment = value;
                SendPropertyChanged("Comment");
            }
        }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        /// <value>The track number.</value>
        public String TrackNumber
        {
            get { return m_TrackNumber; }
            set
            {
                m_TrackNumber = value;
                SendPropertyChanged("TrackNumber");
            }
        }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>The genre.</value>
        public String Genre
        {
            get { return m_Genre; }
            set
            {
                m_Genre = value;
                SendPropertyChanged("Genre");
            }
        }

        /// <summary>
        /// Gets the tag type and version.
        /// </summary>
        /// <value>The tag type and version.</value>
        public String TagVersion
        {
            get { return m_TagVersion; }
            private set
            {
                m_TagVersion = value;
                SendPropertyChanged("TagVersion");
            }
        }

        /// <summary>
        /// Saves the tag.
        /// </summary>
        public void Save()
        {
            Save(m_FileName);
        }

        #endregion <<< ISimpleTag Members >>>

        #region <<< Private Methods >>>

        /// <summary>
        /// Reads the tag from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private void ReadStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            bool id3v2Found = false;
            String tagVersion = String.Empty;

            // ID3v2
            if (ID3v2.ID3v2.DoesTagExist(stream))
            {
                ID3v2.ID3v2 id3v2 = new ID3v2.ID3v2(stream);
                Title = id3v2.Title;
                Artist = id3v2.Artist;
                Album = id3v2.Album;
                Year = id3v2.Year;
                Genre = id3v2.Genre;
                TrackNumber = id3v2.TrackNumber;

                if (id3v2.CommentsList.Count > 0)
                    Comment = id3v2.CommentsList[0].Description;
                else
                    Comment = null;

                switch (id3v2.Header.TagVersion)
                {
                    case ID3v2TagVersion.ID3v22:
                        tagVersion = "ID3v2.2";
                        break;
                    case ID3v2TagVersion.ID3v23:
                        tagVersion = "ID3v2.3";
                        break;
                    case ID3v2TagVersion.ID3v24:
                        tagVersion = "ID3v2.4";
                        break;
                    default:
                        tagVersion = "ID3v2.?";
                        break;
                }

                id3v2Found = true;
            }

            // ID3v1
            if (ID3v1.ID3v1.DoesTagExist(stream))
            {
                ID3v1.ID3v1 id3v1 = new ID3v1.ID3v1(stream);

                // Use ID3v1 fields if ID3v2 doesn't exist or field is blank
                if (!id3v2Found || String.IsNullOrEmpty(Title)) Title = id3v1.Title;
                if (!id3v2Found || String.IsNullOrEmpty(Artist)) Artist = id3v1.Artist;
                if (!id3v2Found || String.IsNullOrEmpty(Album)) Album = id3v1.Album;
                if (!id3v2Found || String.IsNullOrEmpty(Year)) Year = id3v1.Year;
                if (!id3v2Found || String.IsNullOrEmpty(Genre)) Genre = GenreHelper.GenreByIndex[id3v1.GenreIndex];
                if (!id3v2Found || String.IsNullOrEmpty(TrackNumber)) TrackNumber = id3v1.TrackNumber.ToString();
                if (!id3v2Found || String.IsNullOrEmpty(Comment)) Comment = id3v1.Comment;

                switch (id3v1.TagVersion)
                {
                    case ID3v1TagVersion.ID3v10:
                        tagVersion += (!String.IsNullOrEmpty(tagVersion) ? ", " : "") + "ID3v1.0";
                        break;
                    case ID3v1TagVersion.ID3v11:
                        tagVersion += (!String.IsNullOrEmpty(tagVersion) ? ", " : "") + "ID3v1.1";
                        break;
                    default:
                        tagVersion += (!String.IsNullOrEmpty(tagVersion) ? ", " : "") + "ID3v1.?";
                        break;
                }
            }

            // Vorbis Comment
            if (VorbisComment.VorbisComment.IsFlac(stream))
            {
                VorbisComment.VorbisComment vc = new VorbisComment.VorbisComment(stream);
                Title = vc.Title;
                Artist = vc.Artist;
                Album = vc.Album;
                Year = vc.Year;
                Genre = vc.Genre;
                TrackNumber = vc.TrackNumber;
                Comment = vc.Comment;
                tagVersion = (!String.IsNullOrEmpty(tagVersion) ? ", " : "") + "Vorbis Comment";
            }

            // No tag found
            if (String.IsNullOrEmpty(tagVersion))
            {
                Title = null;
                Artist = null;
                Album = null;
                Year = null;
                Genre = null;
                TrackNumber = null;
                Comment = null;
                tagVersion = "None";
            }

            // Set TagVersion
            TagVersion = tagVersion;
        }

        /// <summary>
        /// Saves the tag to the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        private void Save(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (VorbisComment.VorbisComment.IsFlac(path))
            {
                VorbisComment.VorbisComment vc = new VorbisComment.VorbisComment(path);
                vc.Title = Title;
                vc.Artist = Artist;
                vc.Album = Album;
                vc.Year = Year;
                vc.Genre = Genre;
                vc.TrackNumber = TrackNumber;
                vc.Comment = Comment;
                vc.Write(path);

                ID3v2.ID3v2.RemoveTag(path);
                ID3v1.ID3v1.RemoveTag(path);
            }
            else
            {
                ID3v2.ID3v2 id3v2 = new ID3v2.ID3v2(path);
                id3v2.Title = Title;
                id3v2.Artist = Artist;
                id3v2.Album = Album;
                id3v2.Year = Year;
                id3v2.Genre = Genre;
                id3v2.TrackNumber = TrackNumber;

                if (id3v2.CommentsList.Count > 0)
                {
                    id3v2.CommentsList[0].Description = Comment;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Comment))
                        id3v2.CommentsList.AddNew().Description = Comment;
                }

                ID3v1.ID3v1 id3v1 = new ID3v1.ID3v1(path);
                id3v1.Title = Title;
                id3v1.Artist = Artist;
                id3v1.Album = Album;
                id3v1.Year = Year;
                id3v1.GenreIndex = GenreHelper.GetGenreIndex(Genre);
                id3v1.Comment = Comment;
                int trackNumber;
                if (int.TryParse(TrackNumber, out trackNumber))
                {
                    id3v1.TrackNumber = trackNumber;
                }
                else
                {
                    // Handle ##/## format
                    if (TrackNumber.Contains("/"))
                    {
                        if (int.TryParse(TrackNumber.Split('/')[0], out trackNumber))
                            id3v1.TrackNumber = trackNumber;
                        else
                            id3v1.TrackNumber = 0;
                    }
                    else
                    {
                        id3v1.TrackNumber = 0;
                    }
                }

                id3v2.Save(path);
                id3v1.Save(path);
            }

            Read(path);
        }

        /// <summary>
        /// Reads the tag from the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        private void Read(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ReadStream(fileStream);
            }

            m_FileName = path;
        }

        private void SendPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion <<< Private Methods >>>
    }
}
