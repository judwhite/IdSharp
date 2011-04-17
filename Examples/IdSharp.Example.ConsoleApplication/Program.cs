using System;
using System.IO;
using IdSharp.AudioInfo;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.VorbisComment;

namespace IdSharp.Example.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = GetFileName(args);
            if (fileName == null)
                return;

            Console.WriteLine();
            Console.WriteLine(string.Format("File: {0}", fileName));
            Console.WriteLine();

            IAudioFile audioFile = AudioFile.Create(fileName, true);

            Console.WriteLine("Audio Info");
            Console.WriteLine();
            Console.WriteLine(string.Format("Type:      {0}", EnumUtils.GetDescription(audioFile.FileType)));
            Console.WriteLine(string.Format("Length:    {0}:{1:00}", (int)audioFile.TotalSeconds / 60, (int)audioFile.TotalSeconds % 60));
            Console.WriteLine(string.Format("Bitrate:   {0:#,0} kbps", (int)audioFile.Bitrate));
            Console.WriteLine(string.Format("Frequency: {0:#,0} Hz", audioFile.Frequency));
            Console.WriteLine(string.Format("Channels:  {0}", audioFile.Channels));
            Console.WriteLine();

            if (ID3v2Tag.DoesTagExist(fileName))
            {
                IID3v2Tag id3v2 = new ID3v2Tag(fileName);

                Console.WriteLine(EnumUtils.GetDescription(id3v2.Header.TagVersion));
                Console.WriteLine();

                Console.WriteLine(string.Format("Artist:    {0}", id3v2.Artist));
                Console.WriteLine(string.Format("Title:     {0}", id3v2.Title));
                Console.WriteLine(string.Format("Album:     {0}", id3v2.Album));
                Console.WriteLine(string.Format("Year:      {0}", id3v2.Year));
                Console.WriteLine(string.Format("Track:     {0}", id3v2.TrackNumber));
                Console.WriteLine(string.Format("Genre:     {0}", id3v2.Genre));
                Console.WriteLine(string.Format("Pictures:  {0}", id3v2.PictureList.Count));
                Console.WriteLine(string.Format("Comments:  {0}", id3v2.CommentsList.Count));
                Console.WriteLine();

                // Example of saving an ID3v2 tag
                //
                // id3v2.Title = "New song title";
                // id3v2.Save(fileName);
            }

            if (ID3v1Tag.DoesTagExist(fileName))
            {
                IID3v1Tag id3v1 = new ID3v1Tag(fileName);

                Console.WriteLine(EnumUtils.GetDescription(id3v1.TagVersion));
                Console.WriteLine();

                Console.WriteLine(string.Format("Artist:    {0}", id3v1.Artist));
                Console.WriteLine(string.Format("Title:     {0}", id3v1.Title));
                Console.WriteLine(string.Format("Album:     {0}", id3v1.Album));
                Console.WriteLine(string.Format("Year:      {0}", id3v1.Year));
                Console.WriteLine(string.Format("Comment:   {0}", id3v1.Comment));
                Console.WriteLine(string.Format("Track:     {0}", id3v1.TrackNumber));
                Console.WriteLine(string.Format("Genre:     {0}", GenreHelper.GenreByIndex[id3v1.GenreIndex]));
                Console.WriteLine();

                // Example of saving an ID3v1 tag
                //
                // id3v1.Title = "New song title";
                // id3v1.Save(fileName);
            }

            if (audioFile.FileType == AudioFileType.Flac)
            {
                VorbisComment vorbis = new VorbisComment(fileName);

                Console.WriteLine("Vorbis Comment");
                Console.WriteLine();

                Console.WriteLine(string.Format("Artist:    {0}", vorbis.Artist));
                Console.WriteLine(string.Format("Title:     {0}", vorbis.Title));
                Console.WriteLine(string.Format("Album:     {0}", vorbis.Album));
                Console.WriteLine(string.Format("Year:      {0}", vorbis.Year));
                Console.WriteLine(string.Format("Comment:   {0}", vorbis.Comment));
                Console.WriteLine(string.Format("Track:     {0}", vorbis.TrackNumber));
                Console.WriteLine(string.Format("Genre:     {0}", vorbis.Genre));
                Console.WriteLine(string.Format("Vendor:    {0}", vorbis.Vendor));
                Console.WriteLine();

                // Example of saving a Vorbis Comment
                //
                // vorbis.Title = "New song title";
                // vorbis.Save(fileName);
            }
        }

        private static string GetFileName(string[] args)
        {
            string fileName;

            if (args.Length == 1)
            {
                fileName = args[0];
            }
            else
            {
                Console.Write("File name: ");
                fileName = Console.ReadLine();
            }

            if (!File.Exists(fileName))
            {
                string tryFileName = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(tryFileName))
                {
                    fileName = tryFileName;
                }
                else
                {
                    Console.WriteLine(string.Format("\"{0}\" not found.", fileName));
                    return null;
                }
            }

            return fileName;
        }
    }
}
