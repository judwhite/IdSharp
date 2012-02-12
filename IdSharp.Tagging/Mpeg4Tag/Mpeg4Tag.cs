using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v1;

namespace IdSharp.Tagging.Mpeg4
{
    /// <summary>
    /// MPEG-4
    /// </summary>
    internal class Mpeg4Tag : INotifyPropertyChanged
    {
        static Mpeg4Tag()
        {
            MessageBox.Show("WARNING: Mpeg4Tag class is KNOWN to NOT WORK.");
        }

        // MAJOR TODO: REMOVES ELEMENTS IT DOESN'T KNOW ABOUT. BAD.
        // THIS CLASS DOES NOT WORK. DON'T USE IT. PLEASE CONTRIBUTE IF YOU CAN GET IT TO WORK :)

        private const int TAGSIZE = 750;

        private static readonly string[] ATOM_TYPES = new[]{"moov", "trak", "udta", "tref", "imap", "mdia",
		"minf", "stbl", "edts", "mdra", "rmra", "imag", "vnrp", "dinf" };

        private static readonly byte[] FREE_BYTES = Encoding.ASCII.GetBytes("free");

        private readonly byte[] udta = new byte[] { 0, 0, 0, 0, (byte)'u', (byte)'d', (byte)'t', (byte)'a' };
        private readonly byte[] meta = new byte[] { 0, 0, 0, 0, (byte)'m', (byte)'e', (byte)'t', (byte)'a', 0, 0, 0, 0 };
        private readonly byte[] hdlr = new byte[]{0,0,0,0,(byte)'h',(byte)'d',(byte)'l',(byte)'r',0,0,0,0,0,0,0,0,(byte)'m',(byte)'d',(byte)'i',(byte)'r',
	                          (byte)'a',(byte)'p',(byte)'p',(byte)'l',0,0,0,0,0,0,0,0,0};// removed: ,0
        private readonly byte[] ilst = new byte[] { 0, 0, 0, 0, (byte)'i', (byte)'l', (byte)'s', (byte)'t' };
        private readonly byte[] cnam = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'n', (byte)'a', (byte)'m', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };
        private readonly byte[] cart = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'A', (byte)'R', (byte)'T', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };
        private readonly byte[] calb = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'a', (byte)'l', (byte)'b', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };
        //private readonly byte[] gnre = new byte[] { 0, 0, 0, 0, (byte)'g', (byte)'n', (byte)'r', (byte)'e', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private readonly byte[] cgen = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'g', (byte)'e', (byte)'n', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };
        private readonly byte[] trkn = new byte[]{0,0,0,0,(byte)'t',(byte)'r',(byte)'k',(byte)'n',0,0,0,0,(byte)'d',(byte)'a',(byte)'t',(byte)'a',0,0,0,0,0,0,0,0,0,0,
	                          0,0,0,0,0,0};
        private readonly byte[] disk = new byte[]{0,0,0,0,(byte)'d',(byte)'i',(byte)'s',(byte)'k',0,0,0,0,(byte)'d',(byte)'a',(byte)'t',(byte)'a',0,0,0,0,0,0,0,0,0,0,
	                          0,1,0,1};
        private readonly byte[] cday = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'d', (byte)'a', (byte)'y', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };
        private readonly byte[] cpil = new byte[] { 0, 0, 0, 0, (byte)'c', (byte)'p', (byte)'i', (byte)'l', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 21, 0, 0, 0, 0, 0 };
        private readonly byte[] tmpo = new byte[] { 0, 0, 0, 0, (byte)'t', (byte)'m', (byte)'p', (byte)'o', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 21, 0, 0, 0, 0, 0, 0 };
        private readonly byte[] ctoo = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'t', (byte)'o', (byte)'o', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };
        private readonly byte[] ccmt = new byte[] { 0, 0, 0, 0, (byte)'©', (byte)'c', (byte)'m', (byte)'t', 0, 0, 0, 0, (byte)'d', (byte)'a', (byte)'t', (byte)'a', 0, 0, 0, 1, 0, 0, 0, 0 };

        private readonly List<Atom> _atoms = new List<Atom>();

        private long _moovpos;
        private long _udtapos;
        private int _udtasize;
        private long _freepos;
        private int _freesize;
        private int _mdatsize;

        private string _artist;
        private string _album;
        private string _title;
        private string _genre;
        private string _year;
        private string _comment;
        private string _tool;
        private string _composer;
        private int _trackNumber;
        private int _totalTracks;
        private bool _isPartOfCompilation;
        private int _bpm;
        private int _disc;
        private int _totalDiscs;
        private int _channels;
        private int _frequency;
        private string _codec;
        //private double _totalSeconds;
        private int _samples;
        private bool _isWriting = false;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        internal Mpeg4Tag(string path)
        {
            Read(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mpeg4Tag"/> class.
        /// </summary>
        public Mpeg4Tag()
        {
        }

        private class Atom
        {
            public string name;
            public int size;
            public long pos;
            public int level;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region <<< Public Properties >>>

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The artist.</value>
        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                _artist = value;
                RaisePropertyChanged("Artist");
            }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        /// <value>The album.</value>
        public string Album
        {
            get
            {
                return _album;
            }
            set
            {
                _album = value;
                RaisePropertyChanged("Album");
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>The genre.</value>
        public string Genre
        {
            get
            {
                return _genre;
            }
            set
            {
                _genre = value;
                RaisePropertyChanged("Genre");
            }
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public string Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                RaisePropertyChanged("Year");
            }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        /// <summary>
        /// Gets or sets the tool.
        /// </summary>
        /// <value>The tool.</value>
        public string Tool
        {
            get
            {
                return _tool;
            }
            set
            {
                _tool = value;
                RaisePropertyChanged("Tool");
            }
        }

        /// <summary>
        /// Gets or sets the composer.
        /// </summary>
        /// <value>The composer.</value>
        public string Composer
        {
            get
            {
                return _composer;
            }
            set
            {
                _composer = value;
                RaisePropertyChanged("Composer");
            }
        }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        /// <value>The track number.</value>
        public int TrackNumber
        {
            get
            {
                return _trackNumber;
            }
            set
            {
                _trackNumber = value;
                RaisePropertyChanged("TrackNumber");
            }
        }

        /// <summary>
        /// Gets or sets the total tracks.
        /// </summary>
        /// <value>The total tracks.</value>
        public int TotalTracks
        {
            get
            {
                return _totalTracks;
            }
            set
            {
                _totalTracks = value;
                RaisePropertyChanged("TotalTracks");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compilation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is compilation; otherwise, <c>false</c>.
        /// </value>
        public bool IsPartOfCompilation
        {
            get
            {
                return _isPartOfCompilation;
            }
            set
            {
                _isPartOfCompilation = value;
                RaisePropertyChanged("IsPartOfCompilation");
            }
        }

        /// <summary>
        /// Gets or sets the BPM.
        /// </summary>
        /// <value>The BPM.</value>
        public int BPM
        {
            get
            {
                return _bpm;
            }
            set
            {
                _bpm = value;
                RaisePropertyChanged("BPM");
            }
        }

        /// <summary>
        /// Gets or sets the disc.
        /// </summary>
        /// <value>The disc.</value>
        public int Disc
        {
            get
            {
                return _disc;
            }
            set
            {
                _disc = value;
                RaisePropertyChanged("Disc");
            }
        }

        /// <summary>
        /// Gets or sets the total discs.
        /// </summary>
        /// <value>The total discs.</value>
        public int TotalDiscs
        {
            get
            {
                return _totalDiscs;
            }
            set
            {
                _totalDiscs = value;
                RaisePropertyChanged("TotalDiscs");
            }
        }

        #endregion <<< Public Properties >>>

        #region <<< Internal Properties >>>

        internal int MdatAtomSize
        {
            get { return _mdatsize; }
        }

        internal int Samples
        {
            get { return _samples; }
            private set
            {
                _samples = value;
                RaisePropertyChanged("Samples");
            }
        }

        internal int Channels
        {
            get { return _channels; }
            private set
            {
                _channels = value;
                RaisePropertyChanged("Channels");
            }
        }

        internal int Frequency
        {
            get { return _frequency; }
            private set
            {
                _frequency = value;
                RaisePropertyChanged("Frequency");
            }
        }

        internal string Codec
        {
            get { return _codec; }
            private set
            {
                _codec = value;
                RaisePropertyChanged("Codec");
            }
        }

        /*internal Double TotalSeconds
        {
            get { return m_TotalSeconds; }
            private set
            {
                m_TotalSeconds = value;
                FirePropertyChanged("TotalSeconds");
            }
        }*/

        #endregion <<< Internal Properties >>>

        private void ParseTag(string key, byte[] data, int size)
        {
            if (_isWriting) return;

            if (string.Compare(key, "©nam", true) == 0)
            {
                Title = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "©ART", true) == 0)
            {
                Artist = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "©alb", true) == 0)
            {
                Album = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "©cmt", true) == 0)
            {
                Comment = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "©day", true) == 0)
            {
                Year = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "©too", true) == 0)
            {
                Tool = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "gnre", true) == 0)
            {
                if (string.IsNullOrEmpty(Genre))
                {
                    int genreIndex = data[1];
                    if (genreIndex != 0)
                    {
                        Genre = GenreHelper.GenreByIndex[genreIndex - 1];
                    }
                }
            }
            else if (string.Compare(key, "©gen", true) == 0)
            {
                Genre = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "trkn", true) == 0)
            {
                TrackNumber = data[3];
                TotalTracks = data[5];
            }
            else if (string.Compare(key, "disk", true) == 0)
            {
                Disc = data[3];
                TotalDiscs = data[5];
            }
            else if (string.Compare(key, "©wrt", true) == 0)
            {
                Composer = Encoding.UTF8.GetString(data, 0, size);
            }
            else if (string.Compare(key, "cpil", true) == 0)
            {
                IsPartOfCompilation = (data[0] == 1);
            }
            else if (string.Compare(key, "tmpo", true) == 0)
            {
                BPM = data[1] + (data[0] << 8);
            }
            else
            {
                // TODO: Save unknown keys
                Console.WriteLine(key);
            }
        }

        private static int ReadInt32(byte[] data, int offset)
        {
            int n;

            n = (data[offset] & 0xff) << 24;
            n += (data[offset + 1] & 0xff) << 16;
            n += (data[offset + 2] & 0xff) << 8;
            n += (data[offset + 3] & 0xff);
            return n;
        }

        private void ParseStsdAtom(byte[] atomdata)
        {
            byte[] data_format = new byte[4];
            byte[] encoder_vendor = new byte[4];

            int num = ReadInt32(atomdata, 4);
            int stsdOff = 8;
            for (int i = 0; i < num; i++)
            {
                int size = ReadInt32(atomdata, stsdOff);
                stsdOff += 4;
                Buffer.BlockCopy(atomdata, stsdOff, data_format, 0, 4);

                stsdOff += 12;
                byte[] data = new byte[size - 4 - 12];
                Buffer.BlockCopy(atomdata, stsdOff, data, 0, (size - 4 - 12));
                stsdOff += (size - 4 - 12);

                Buffer.BlockCopy(data, 4, encoder_vendor, 0, 4);
                if (encoder_vendor[0] == 0)
                {
                    Frequency = ((data[16] & 0xff) << 8) + (data[17] & 0xff);
                    Channels = (data[8] << 8) + data[9];
                    if (ByteUtils.Compare(data_format, Encoding.ASCII.GetBytes("mp4a")))
                    {
                        Codec = "AAC";
                    }
                    else
                    {
                        Codec = Encoding.UTF8.GetString(data_format);
                    }
                }
            }
        }

        private void ParseAtom(Stream stream, long firstOffset, long stopAt, int level)
        {
            ++level;

            long end = stream.Length;
            long offset = firstOffset;
            while (offset < stopAt)
            {
                stream.Seek(offset, SeekOrigin.Begin);

                int atomsize = stream.ReadInt32();
                string atomname = ByteUtils.ISO88591.GetString(stream.Read(4));

                if ((offset + atomsize) > end)
                {
                    throw new InvalidDataException(string.Format("atom at {0} claims {1} bytes, end = {2}", offset, atomsize, end));
                }

                Atom atom = new Atom();
                atom.name = atomname;
                atom.size = atomsize;
                atom.pos = stream.Position - 8;
                atom.level = level;

                _atoms.Add(atom);

                if (string.Compare(atomname, "mdat", true) == 0)
                {
                    _mdatsize = atomsize;
                }
                else if (string.Compare(atomname, "moov", true) == 0)
                {
                    _moovpos = stream.Position - 8;
                }
                else if (string.Compare(atomname, "udta", true) == 0)
                {
                    _udtapos = stream.Position - 8;
                    _udtasize = atomsize;
                }
                else if (string.Compare(atomname, "free", true) == 0)
                {
                    // we'd like a free under a moov
                    // so check for level = 1
                    if (atom.level == 2)
                    {
                        // go backwards through the entries
                        for (int i = _atoms.Count - 1; i > 0; i--)
                        {
                            // until we hit a level 1
                            if (_atoms[i].level == 1)
                            {
                                if (string.Compare(_atoms[i].name, "moov", true) == 0)
                                {
                                    if (atomsize > _freesize)
                                    {
                                        // is this free bigger than others?
                                        _freepos = stream.Position - 8;
                                        _freesize = atomsize;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                } // if moov
                            } // if level = 1
                        } // for
                    } // if level = 2
                } // if free

                // if it's a container atom, parse the contents of the atom

                foreach (string atomType in ATOM_TYPES)
                {
                    if (string.Compare(atomname, atomType, true) == 0)
                    {
                        ParseAtom(stream, stream.Position, offset + atomsize, level);
                        break;
                    }
                }

                // meta atom contains tags and some other data
                if (string.Compare(atomname, "meta", true) == 0)
                {
                    // read in meta atom
                    byte[] atomdata = stream.Read(atomsize - 8);
                    int nextTagPosition = 0;
                    for (int i = 0; i < atomdata.Length - 4; i++)
                    {
                        // ilst
                        if (atomdata[i] == 'i' && atomdata[i + 1] == 'l' && atomdata[i + 2] == 's' && atomdata[i + 3] == 't')
                        {
                            nextTagPosition = i + 8;
                            break;
                        }
                    }

                    while (nextTagPosition < (atomsize - 4) && nextTagPosition > 8)
                    {
                        int size = ReadInt32(atomdata, (nextTagPosition - 4)) - 4;
                        if (size < 20)
                        {
                            Console.WriteLine(stream.Position); // TODO: Why is this here...
                        }

                        byte[] keyBytes = new byte[4];
                        byte[] data = new byte[size - 20];

                        Buffer.BlockCopy(atomdata, nextTagPosition, keyBytes, 0, 4);
                        Buffer.BlockCopy(atomdata, nextTagPosition + 20, data, 0, size - 20);
                        string key = ByteUtils.ISO88591.GetString(keyBytes);
                        nextTagPosition += size + 4;

                        ParseTag(key, data, size - 20);
                    }
                } // if meta

                // mdhd has data for calculating playtime
                if (string.Compare(atomname, "mdhd", true) == 0)
                {
                    stream.Seek(12, SeekOrigin.Current);
                    Frequency = stream.ReadInt32();
                    Samples = stream.ReadInt32();
                }

                // stsd has data for sample, channels and codec
                if (string.Compare(atomname, "stsd", true) == 0)
                {
                    byte[] atomdata = stream.Read(atomsize - 8);
                    ParseStsdAtom(atomdata);
                }

                // bump offest and continue
                if (atomsize == 0)
                {
                    // a 0 byte atom is the end
                    offset = stopAt;
                }
                else
                {
                    offset += atomsize;
                }
            } // while

            --level;
        }

        private static void WriteInt32(int num, Stream stream, int offset)
        {
            long oldPosition = stream.Position;
            stream.Position = offset;

            stream.WriteByte((byte)((num >> 24) & 0xff));
            stream.WriteByte((byte)((num >> 16) & 0xff));
            stream.WriteByte((byte)((num >> 8) & 0xff));
            stream.WriteByte((byte)((num >> 0) & 0xff));

            stream.Position = oldPosition;
        }

        private static void WriteInt32(int num, byte[] data, int offset)
        {
            data[offset] = (byte)((num >> 24) & 0xff);
            data[offset + 1] = (byte)((num >> 16) & 0xff);
            data[offset + 2] = (byte)((num >> 8) & 0xff);
            data[offset + 3] = (byte)((num >> 0) & 0xff);
        }

        private static void WriteAtom(Stream stream, byte[] atom, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                byte[] byteValue = Encoding.UTF8.GetBytes(value);
                WriteInt32(atom.Length + byteValue.Length, atom, 0);
                WriteInt32(atom.Length + byteValue.Length - 8, atom, 8);
                stream.Write(atom);
                stream.Write(byteValue);
            }
        }

        /*private void StoreGenreAtom(Stream stream, Byte genreNum)
        {
            if (genreNum > 0 && genreNum < 255)
            {
                store4int(gnre.Length, gnre, 0);
                store4int(gnre.Length - 8, gnre, 8);
                gnre[gnre.Length - 1] = genreNum;
                Utils.Write(stream, gnre);
            }
        }*/

        private void WriteTrackAtom(Stream stream)
        {
            if (_trackNumber > 0 || _totalTracks > 0)
            {
                WriteInt32(trkn.Length, trkn, 0);
                WriteInt32(trkn.Length - 8, trkn, 8);
                trkn[trkn.Length - 5] = (byte)_trackNumber;
                trkn[trkn.Length - 3] = (byte)_totalTracks;
                stream.Write(trkn);
            }
        }

        private void WriteDiscAtom(Stream stream)
        {
            if (_disc > 0 || _totalDiscs > 0)
            {
                WriteInt32(disk.Length, disk, 0);
                WriteInt32(disk.Length - 8, disk, 8);
                disk[disk.Length - 3] = (byte)_disc;
                disk[disk.Length - 1] = (byte)_totalDiscs;
                stream.Write(disk);
            }
        }

        private void WriteCompilationAtom(Stream stream)
        {
            if (_isPartOfCompilation)
            {
                WriteInt32(cpil.Length, cpil, 0);
                WriteInt32(cpil.Length - 8, cpil, 8);
                cpil[cpil.Length - 1] = 1;
                stream.Write(cpil);
            }
        }

        private void WriteBpmAtom(Stream stream)
        {
            if (_bpm > 0)
            {
                WriteInt32(tmpo.Length, tmpo, 0);
                WriteInt32(tmpo.Length - 8, tmpo, 8);
                WriteInt32(_bpm, tmpo, tmpo.Length - 4);
                stream.Write(tmpo);
            }
        }

        private int CreateUDTA(Stream fulltag, int len)
        {
            fulltag.SetLength(0);

            // append each atom to fulltag
            // pos serves as a running total of the size of the atom
            fulltag.Write(udta);
            fulltag.Write(meta);
            fulltag.Write(hdlr);
            fulltag.Write(ilst);

            WriteAtom(fulltag, cnam, _title);
            WriteAtom(fulltag, cart, _artist);
            WriteAtom(fulltag, calb, _album);
            //StoreGenreAtom(fulltag, tag.genrenum);
            WriteAtom(fulltag, cgen, _genre);
            WriteTrackAtom(fulltag);
            WriteDiscAtom(fulltag);
            WriteAtom(fulltag, cday, _year);
            WriteCompilationAtom(fulltag);
            WriteBpmAtom(fulltag);
            WriteAtom(fulltag, ctoo, _tool);
            WriteAtom(fulltag, ccmt, _comment);

            // done with adding tags
            // now clean up the atomsizes and perhaps add a free atom
            // the free atom gives us some space if we edit tags later

            int pos = (int)fulltag.Position;
            WriteInt32(pos - 20 - hdlr.Length, fulltag, 20 + hdlr.Length); // size of ilst

            if (pos < (len + 8))
            {
                WriteInt32(len - pos, fulltag, pos);
                fulltag.Position = pos + 4;
                fulltag.Write(FREE_BYTES);
                fulltag.Write(new byte[len - pos - 8]);
                pos = len;
            }

            WriteInt32(pos, fulltag, 0);		// size of entire atom at beginning
            WriteInt32(pos - 8, fulltag, 8);	// size of meta at meta position
            WriteInt32(hdlr.Length, fulltag, 20);  // size of hdlr at it position

            return pos;
        }

        // take the moov atom and copy it to end
        // change original moov atom to free atom
        // change atomsize of new moov atom to accomodate new tags
        private static bool MoovToEnd(Stream f, int len, long moovpos)
        {
            if (moovpos == 0)
                return false;

            f.Seek(moovpos, SeekOrigin.Begin);

            int atomsize = f.ReadInt32();

            f.Seek(moovpos, SeekOrigin.Begin);  	// move to start of moov atom
            byte[] moov = f.Read(atomsize); // read entire moov atom

            f.Seek(moovpos + 4, SeekOrigin.Begin); // back to start of atom
            f.Write(FREE_BYTES); // change old moov atom to free

            f.Seek(0, SeekOrigin.End);         	// moov atom to end of file
            // todo: atomsize+len isn't making sense.. look into
            WriteInt32(atomsize + len, moov, 0);	// bump the len of the moov
            f.Write(moov); // write the new moov atom

            return true;
        }

        /// <summary>
        /// Removes the tag from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void Remove(string path)
        {
            Mpeg4Tag tag = new Mpeg4Tag();
            tag.Read(path);

            /*if (tag.m_udtapos == 0)
            {
                Console.Write("no existing tags\n");
            }*/

            using (FileStream outf = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                foreach (Atom atom in tag._atoms)
                {
                    if (string.Compare(atom.name, "udta", true) == 0)
                    {
                        outf.Seek(atom.pos + 4, SeekOrigin.Begin);
                        outf.Write(FREE_BYTES);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the tag to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Write(string path)
        {
            throw new NotImplementedException();

            /*m_IsWriting = true;
            try
            {
                Read(path);

                using (FileStream outf = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
                using (MemoryStream fulltag = new MemoryStream())
                {
                    int len = CreateUDTA(fulltag, 0);

                    // is there an existing udta atom we can overwrite?
                    // if there's enough room in the atom, overwrite
                    // if not, change to a free atom
                    bool wrote = false;
                    if (m_udtapos != 0)
                    {
                        if (len < m_udtasize)
                        {
                            // todo: assertions can be done on return value at fi.udtasize
                            CreateUDTA(fulltag, m_udtasize);
                            outf.Seek(m_udtapos, SeekOrigin.Begin);
                            outf.Write(fulltag.ToArray(), 0, m_udtasize);
                            wrote = true;
                        }
                        else
                        {
                            outf.Seek(m_udtasize + 4, SeekOrigin.Begin);
                            Utils.Write(outf, FREE_BYTES);
                        }
                    }

                    // is there an existing free atom we can overwrite?
                    // if there's enough room in the atom, overwrite
                    if (!wrote && m_freepos != 0 && len < m_freesize)
                    {
                        CreateUDTA(fulltag, m_freesize);
                        outf.Seek(m_freepos, SeekOrigin.Begin);
                        outf.Write(fulltag.ToArray(), 0, m_freesize);
                        wrote = true;
                    }

                    // if we didn't overwite an existing udta or free, add to a moov

                    if (!wrote)
                    {
                        len = CreateUDTA(fulltag, TAGSIZE);
                        MoovToEnd(outf, len, m_moovpos);
                        Utils.Write(outf, fulltag.ToArray());
                    }
                }

                // if we used moovtoend, there is a large free atom.  use mpcreator to eliminate it
                // if mp4creator60 is not present, nothing happens
                // consider using mpeg4ip's optimize - same source as mp4creator
                if(wrote == 0) {
                    sprintf(str, "\"%s\"", outfile);
                    spawnlp(P_WAIT, "mp4creator60.exe", "mp4creator60.exe",
                            "-optimize", str, NULL);
                }
            }
            finally
            {
                m_IsWriting = false;
            }*/
        }

        /// <summary>
        /// Reads the tag from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Read(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(fs);
            }
        }

        /// <summary>
        /// Reads the tag from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Read(Stream stream)
        {
            _atoms.Clear();
            _moovpos = 0;
            _udtapos = 0;
            _udtasize = 0;
            _freepos = 0;
            _freesize = 0;
            _mdatsize = 0;

            long end = stream.Length;
            ParseAtom(stream, 0, end, 0);
        }

        //////////////////////////// from libfaad

    }
}
