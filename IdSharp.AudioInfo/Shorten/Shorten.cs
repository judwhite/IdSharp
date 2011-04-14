using System;
using System.IO;
using IdSharp.Common.Utils;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// Shorten
    /// </summary>
    public class Shorten : IAudioFile
    {
        static Shorten()
        {
            uint val = 0;
            masktab[0] = val;
            for (int i = 1; i < MASKTABSIZE; i++)
            {
                val <<= 1;
                val |= 1;
                masktab[i] = val;
            }
        }

        private const int MASKTABSIZE = 33;
        private const int BUFSIZE = 32768;

        private const int DEFAULT_BLOCK_SIZE = 256;
        private const int CHANSIZE = 0;
        private const int LPCQUANT = 5;
        private const int VERBATIM_CKSIZE_SIZE = 5;   // a var_put code size
        private const int BITSHIFTSIZE = 2;
        private const int ULONGSIZE = 2;
        private const int NSKIPSIZE = 1;
        private const int XBYTESIZE = 7;
        private const int MAX_VERSION = 7;
        private const int MAX_SUPPORTED_VERSION = 3;
        private const int ENERGYSIZE = 3;
        private const int LPCQSIZE = 2;
        private const int TYPESIZE = 4;
        private const int FNSIZE = 2;
        private const int FN_DIFF0 = 0;
        private const int FN_DIFF1 = 1;
        private const int FN_DIFF2 = 2;
        private const int FN_DIFF3 = 3;
        private const int FN_QUIT = 4;
        private const int FN_BLOCKSIZE = 5;
        private const int FN_BITSHIFT = 6;
        private const int FN_QLPC = 7;
        private const int FN_ZERO = 8;
        private const int FN_VERBATIM = 9;
        private const int VERBATIM_BYTE_SIZE = 8;   // code size 8 on single bytes means no compression at all
        private const double M_LN2 = 0.69314718055994530942;

        private static readonly byte[] MAGIC = new byte[] { (byte)'a', (byte)'j', (byte)'k', (byte)'g', 0x00 };
        private static readonly uint[] masktab = new uint[MASKTABSIZE];

        private readonly int _frequency = 44100; // TODO: Hard-coded
        private readonly decimal _totalSeconds;
        private readonly decimal _bitrate;
        private readonly int _channels = 2; // TODO: Hard-coded
        private readonly int _samples;

        private readonly byte[] getbuf;
        private int nbyteget;
        private uint gbuffer;
        private int nbitget;
        private int getbufOffset;
        private int version;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shorten"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public Shorten(string path)
        {
            getbuf = new byte[BUFSIZE];
            nbyteget = 0;
            gbuffer = 0;
            nbitget = 0;

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _samples = getSamples(fs);
                _totalSeconds = (decimal)_samples / _frequency;
                _bitrate = fs.Length / _totalSeconds / 125.0m;
            }
        }

        private uint UINT_GET(int nbit, Stream file)
        {
            if (version == 0)
                return (uint)uvar_get(nbit, file);
            else
                return ulong_get(file);
        }

        private uint ulong_get(Stream stream)
        {
            int nbit = uvar_get(ULONGSIZE, stream);
            return (uint)uvar_get(nbit, stream);
        }

        private uint word_get(Stream stream)
        {
            if (nbyteget < 4)
            {
                int bytes = stream.Read(getbuf, 0, BUFSIZE);
                getbufOffset = 0;
                nbyteget += bytes;
                if (nbyteget < 4)
                {
                    unchecked
                    {
                        return (uint)(-1);
                    }
                }
            }

            uint buffer = (((uint)getbuf[getbufOffset]) << 24) | (((uint)getbuf[getbufOffset + 1]) << 16) |
                          (((uint)getbuf[getbufOffset + 2]) << 8) | (getbuf[getbufOffset + 3]);

            getbufOffset += 4;
            nbyteget -= 4;

            return (buffer);
        }

        private int uvar_get(int nbin, Stream stream)
        {
            int result;

            if (nbitget == 0)
            {
                gbuffer = word_get(stream);
                nbitget = 32;
            }

            for (result = 0; (gbuffer & (1L << --nbitget)) == 0; result++)
            {
                if (nbitget == 0)
                {
                    gbuffer = word_get(stream);
                    nbitget = 32;
                }
            }

            while (nbin != 0)
            {
                if (nbitget >= nbin)
                {
                    result = (int)((uint)(result << nbin) | ((gbuffer >> (nbitget - nbin)) & masktab[nbin]));
                    nbitget -= nbin;
                    nbin = 0;
                }
                else
                {
                    result = (int)((uint)(result << nbitget) | (gbuffer & masktab[nbitget]));
                    gbuffer = word_get(stream);
                    nbin -= nbitget;
                    nbitget = 32;
                }
            }

            return (result);
        }

        private int var_get(int nbin, Stream stream)
        {
            uint uvar = (uint)uvar_get(nbin + 1, stream);

            if ((uvar & 1) == 1) return ((int)~(uvar >> 1));
            else return ((int)(uvar >> 1));
        }

        private int getSamples(Stream stream)
        {
            int SampleNumber = 0;
            int i;
            int nscan = 0;

            version = MAX_VERSION + 1;
            while (version > MAX_VERSION)
            {
                int myByte = stream.Read1();
                if (MAGIC[nscan] != '\0' && myByte == MAGIC[nscan])
                {
                    nscan++;
                }
                else if (MAGIC[nscan] == '\0' && myByte <= MAX_VERSION)
                {
                    version = myByte;
                }
                else
                {
                    if (myByte == MAGIC[0])
                    {
                        nscan = 1;
                    }
                    else
                    {
                        nscan = 0;
                    }
                    version = MAX_VERSION + 1;
                }
            }

            // check version number
            if (version > MAX_SUPPORTED_VERSION)
                return 0;
            UINT_GET(TYPESIZE, stream);

            int blocksize;
            int nchan = (int)UINT_GET(CHANSIZE, stream);

            if (version > 0)
            {
                blocksize = (int)UINT_GET((int)(Math.Log(DEFAULT_BLOCK_SIZE) / M_LN2), stream);
                UINT_GET(LPCQSIZE, stream);
                UINT_GET(0, stream);
                int nskip = (int)UINT_GET(NSKIPSIZE, stream);
                for (i = 0; i < nskip; i++)
                {
                    uvar_get(XBYTESIZE, stream);
                }
            }
            else
            {
                blocksize = DEFAULT_BLOCK_SIZE;
            }

            // get commands from file and execute them
            int chan = 0;
            int cmd = uvar_get(FNSIZE, stream);
            while (cmd != FN_QUIT)
            {
                switch (cmd)
                {
                    case FN_ZERO:
                    case FN_DIFF0:
                    case FN_DIFF1:
                    case FN_DIFF2:
                    case FN_DIFF3:
                    case FN_QLPC:
                        int resn = 0;

                        if (cmd != FN_ZERO)
                        {
                            resn = uvar_get(ENERGYSIZE, stream);
                            // this is a hack as version 0 differed in definition of var_get
                            if (version == 0) resn--;
                        }

                        switch (cmd)
                        {
                            case FN_ZERO:
                                break;
                            case FN_DIFF0:
                            case FN_DIFF1:
                            case FN_DIFF2:
                            case FN_DIFF3:
                                for (i = 0; i < blocksize; i++)
                                {
                                    int nbin = resn + 1;

                                    if (nbitget == 0)
                                    {
                                        if (nbyteget < 4)
                                        {
                                            int bytes = stream.Read(getbuf, 0, BUFSIZE);
                                            getbufOffset = 0;
                                            nbyteget += bytes;
                                        }

                                        gbuffer = (((uint)getbuf[getbufOffset]) << 24) | (((uint)getbuf[getbufOffset + 1]) << 16) |
                                                  (((uint)getbuf[getbufOffset + 2]) << 8) | (getbuf[getbufOffset + 3]);

                                        getbufOffset += 4;
                                        nbyteget -= 4;

                                        nbitget = 32;
                                    }

                                    while ((gbuffer & (1L << --nbitget)) == 0)
                                    {
                                        if (nbitget == 0)
                                        {
                                            if (nbyteget < 4)
                                            {
                                                int bytes = stream.Read(getbuf, 0, BUFSIZE);
                                                getbufOffset = 0;
                                                nbyteget += bytes;
                                            }

                                            gbuffer = (((uint)getbuf[getbufOffset]) << 24) | (((uint)getbuf[getbufOffset + 1]) << 16) |
                                                      (((uint)getbuf[getbufOffset + 2]) << 8) | (getbuf[getbufOffset + 3]);

                                            getbufOffset += 4;
                                            nbyteget -= 4;

                                            nbitget = 32;
                                        }
                                    }

                                    while (nbin != 0)
                                    {
                                        if (nbitget >= nbin)
                                        {
                                            nbitget -= nbin;
                                            nbin = 0;
                                        }
                                        else
                                        {
                                            if (nbyteget < 4)
                                            {
                                                int bytes = stream.Read(getbuf, 0, BUFSIZE);
                                                getbufOffset = 0;
                                                nbyteget += bytes;
                                            }

                                            gbuffer = (((uint)getbuf[getbufOffset]) << 24) | (((uint)getbuf[getbufOffset + 1]) << 16) |
                                                      (((uint)getbuf[getbufOffset + 2]) << 8) | (getbuf[getbufOffset + 3]);

                                            getbufOffset += 4;
                                            nbyteget -= 4;

                                            nbin -= nbitget;
                                            nbitget = 32;
                                        }
                                    }
                                }
                                break;
                            case FN_QLPC:
                                int nlpc = uvar_get(LPCQSIZE, stream);

                                for (i = 0; i < nlpc; i++)
                                    var_get(LPCQUANT, stream);
                                break;
                        }

                        if (chan == nchan - 1)
                        {
                            SampleNumber += blocksize;
                        }
                        chan = (chan + 1) % nchan;
                        break;
                    case FN_BLOCKSIZE:
                        blocksize = (int)UINT_GET((int)(Math.Log(blocksize) / M_LN2), stream);
                        break;
                    case FN_BITSHIFT:
                        uvar_get(BITSHIFTSIZE, stream);
                        break;
                    case FN_VERBATIM:
                        int cklen = uvar_get(VERBATIM_CKSIZE_SIZE, stream);
                        while (cklen-- != 0)
                        {
                            uvar_get(VERBATIM_BYTE_SIZE, stream);
                        }
                        break;

                    default:
                        return 0;
                }

                cmd = uvar_get(FNSIZE, stream);
            }

            return SampleNumber;
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get { return _frequency; }
        }

        /// <summary>
        /// Gets the sample count.
        /// </summary>
        /// <value>The sample count.</value>
        public int Samples
        {
            get { return _samples; }
        }

        /// <summary>
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        public decimal TotalSeconds
        {
            get { return _totalSeconds; }
        }

        /// <summary>
        /// Gets the bitrate.
        /// </summary>
        /// <value>The bitrate.</value>
        public decimal Bitrate
        {
            get { return _bitrate; }
        }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        /// <value>The number of channels.</value>
        public int Channels
        {
            get { return _channels; }
        }

        /// <summary>
        /// Gets the type of the audio file.
        /// </summary>
        /// <value>The type of the audio file.</value>
        public AudioFileType FileType
        {
            get { return AudioFileType.Shorten; }
        }
    }
}
