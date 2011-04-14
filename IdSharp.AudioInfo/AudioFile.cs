using System;
using System.Collections.Generic;
using System.IO;

namespace IdSharp.AudioInfo
{
    /// <summary>
    /// Class which creates an appropriate IAudioFile object based on the file extension.
    /// </summary>
    public static class AudioFile
    {
        /// <summary>
        /// Creates an IAudioFile object from the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <param name="throwExceptionIfUnknown">if set to <c>true</c>, throws an exception if unknown file extension; otherwise, returns <c>null</c>.</param>
        public static IAudioFile Create(string path, bool throwExceptionIfUnknown)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            string ext = Path.GetExtension(path).ToLower();
            IAudioFile audioFile = null;

            if (ext == ".mp3" || ext == ".mp2")
            {
                audioFile = new Mpeg(path, false);
            }
            else if (ext == ".ogg")
            {
                audioFile = new OggVorbis(path);
            }
            else if (ext == ".flac" || ext == ".fla")
            {
                audioFile = new Flac(path);
            }
            else if (ext == ".mpc" || ext == ".mpp" || ext == ".mp+")
            {
                audioFile = new Musepack(path);
            }
            else if (ext == ".shn")
            {
                audioFile = new Shorten(path);
            }
            else if (ext == ".ape" || ext == ".mac")
            {
                audioFile = new MonkeysAudio(path);
            }
            else if (ext == ".m4a")
            {
                audioFile = new Mpeg4(path);
            }
            else if (ext == ".ofr")
            {
                audioFile = new OptimFrog(path);
            }
            else if (throwExceptionIfUnknown)
            {
                throw new NotSupportedException(string.Format("Extension '{0}' not supported", ext));
            }

            return audioFile;
        }

        /// <summary>
        /// Gets a list of IAudioFile objects from an array of file paths.  Unknown file extensions are ignored.
        /// </summary>
        /// <param name="fileList">The file list.</param>
        public static List<IAudioFile> GetList(string[] fileList)
        {
            return GetList(fileList, false, false);
        }

        /// <summary>
        /// Gets a list of IAudioFile objects from an array of file paths.  Unknown file extensions are ignored.
        /// </summary>
        /// <param name="fileList">The file list.</param>
        /// <param name="sort">if set to <c>true</c> the array will be sorted before building the list.</param>
        public static List<IAudioFile> GetList(string[] fileList, bool sort)
        {
            return GetList(fileList, sort, false);
        }

        /// <summary>
        /// Gets a list of IAudioFile objects from an array of file paths.
        /// </summary>
        /// <param name="fileList">The file list.</param>
        /// <param name="sort">if set to <c>true</c> the array will be sorted before building the list.</param>
        /// <param name="throwExceptionIfUnknown">if set to <c>true</c>, throws an exception if an unknown file extension is encountered; otherwise, unknown file extensions are not added to the list.</param>
        public static List<IAudioFile> GetList(string[] fileList, bool sort, bool throwExceptionIfUnknown)
        {
            if (sort)
            {
                Array.Sort(fileList);
            }

            List<IAudioFile> audioFileList = new List<IAudioFile>();
            foreach (string path in fileList)
            {
                IAudioFile audioFile = Create(path, throwExceptionIfUnknown);
                if (audioFile != null)
                    audioFileList.Add(audioFile);
            }

            return audioFileList;
        }

        /// <summary>
        /// Gets all supported extensions.
        /// </summary>
        public static string[] GetExtensions()
        {
            return new[] { ".mp3", ".m4a", ".flac", ".fla", ".ogg", ".mpc", ".mpp", ".mp+", ".ape", ".mac", ".shn", ".ofr" };
        }

        /// <summary>
        /// Gets the filter for use with an OpenFileDialog.
        /// </summary>
        /// <param name="includeAllFiles">if set to <c>true</c> "All files" will be added to the filter.</param>
        /// <param name="includeAllFormats">if set to <c>true</c> "All supported formats" will be added to the filter.</param>
        public static string GetFilter(bool includeAllFiles, bool includeAllFormats)
        {
            string filter = "";
            if (includeAllFiles)
            {
                filter += "All files|*.*|";
            }
            if (includeAllFormats)
            {
                string allExtensions = "";
                foreach (string extension in GetExtensions())
                {
                    if (allExtensions != "") allExtensions += ";";
                    allExtensions += "*" + extension;
                }

                filter += string.Format("All supported formats|{0}|", allExtensions);
            }
            filter += "MP3 (*.mp3)|*.mp3|";
            filter += "MP4 (*.m4a)|*.m4a|";
            filter += "FLAC (*.flac, *.fla)|*.flac, *.fla|";
            filter += "Ogg-Vorbis (*.ogg)|*.ogg|";
            filter += "Musepack (*.mpc, *.mpp, *.mp+)|*.mpc;*.mpp;*.mp+|";
            filter += "Monkey's Audio (*.ape, *.mac)|*.ape;*.mac|";
            filter += "Shorten (*.shn)|*.shn|";
            filter += "OptimFROG (*.ofr)|*.ofr";

            return filter;
        }
    }
}
