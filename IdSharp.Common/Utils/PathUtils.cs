using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// PathUtils.
    /// </summary>
    public static class PathUtils
    {
        private static readonly List<char> _invalidFileNameChars;

        static PathUtils()
        {
            _invalidFileNameChars = new List<char>(Path.GetInvalidFileNameChars());
        }

        /// <summary>
        /// Gets a unique file name which does not exist based on the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Unique file name which does not exist.</returns>
        public static string GetUniqueFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (!File.Exists(path))
                return path;

            string basePath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            string ext = Path.GetExtension(path);

            for (int i = 1; i == 1 || File.Exists(path); i++)
            {
                path = string.Format("{0} ({1}){2}", basePath, i, ext);
            }

            return path;
        }

        /// <summary>
        /// Returns a unique file name in the temporary path with the specified extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>A unique file name in the temporary path with the specified extension.</returns>
        public static string GetTempFileName(string extension)
        {
            if (!string.IsNullOrEmpty(extension))
            {
                if (extension[0] != '.')
                    extension = "." + extension;

                if (extension.EndsWith("."))
                {
                    throw new ArgumentException("Parameter 'extension' cannot end with a '.'", "extension");
                }

                foreach (char c in extension)
                {
                    if (_invalidFileNameChars.Contains(c))
                        throw new ArgumentException(string.Format("Parameter 'extension' cannot contain '{0}'", c), "extension");
                }
            }

            string tempPath = Path.GetTempPath();
            string guid = Guid.NewGuid().ToString();

            string fileName = Path.Combine(tempPath, guid) + extension;
            return GetUniqueFileName(fileName);
        }

        /// <summary>
        /// Gets a new temporary file name using the <paramref name="baseFileName"/> as a base.
        /// </summary>
        /// <param name="baseFileName">The base file name.</param>
        /// <returns>A new temporary file name with the same prefix as <paramref name="baseFileName"/>.</returns>
        public static string GetTemporaryFileNameBasedOnFileName(string baseFileName)
        {
            if (string.IsNullOrEmpty(baseFileName))
                throw new ArgumentNullException("basefileName");

            string tempFile;
            Random rnd = new Random();
            byte[] randomBytes = new byte[8];
            do
            {
                for (int i = 0; i < randomBytes.Length; i++)
                {
                    randomBytes[i] = (byte)rnd.Next(65, 91);
                }

                string randomString = Encoding.ASCII.GetString(randomBytes);
                tempFile = string.Format("{0}.{1}.tmp", baseFileName, randomString);
            } while (File.Exists(tempFile));
            return tempFile;
        }
    }
}
