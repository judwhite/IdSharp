using System;
using System.IO;
using IdSharp.AudioInfo;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;

namespace IdSharp.Tagging.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = GetFileName();
            if (fileName == null)
                return;

            System.Console.WriteLine();
            System.Console.WriteLine(string.Format("File: {0}", fileName));
            System.Console.WriteLine();

            IAudioFile audioFile = AudioFile.Create(fileName, true);

            System.Console.WriteLine("Audio Info");
            System.Console.WriteLine();
            System.Console.WriteLine(string.Format("Type:      {0}", EnumUtils.GetDescription(audioFile.FileType)));
            System.Console.WriteLine(string.Format("Length:    {0}:{1:00}", (int)audioFile.TotalSeconds / 60, (int)audioFile.TotalSeconds % 60));
            System.Console.WriteLine(string.Format("Bitrate:   {0:#,0} kbps", (int)audioFile.Bitrate));
            System.Console.WriteLine(string.Format("Frequency: {0:#,0} Hz", audioFile.Frequency));
            System.Console.WriteLine(string.Format("Channels:  {0}", audioFile.Channels));
            System.Console.WriteLine();

            if (ID3v2Tag.DoesTagExist(fileName))
            {
                IID3v2Tag id3v2 = new ID3v2Tag(fileName);

                System.Console.WriteLine(EnumUtils.GetDescription(id3v2.Header.TagVersion));
                System.Console.WriteLine();

                System.Console.WriteLine(string.Format("Artist:    {0}", id3v2.Artist));
                System.Console.WriteLine(string.Format("Title:     {0}", id3v2.Title));
                System.Console.WriteLine(string.Format("Album:     {0}", id3v2.Album));
                System.Console.WriteLine(string.Format("Year:      {0}", id3v2.Year));
                System.Console.WriteLine(string.Format("Track:     {0}", id3v2.TrackNumber));
                System.Console.WriteLine(string.Format("Genre:     {0}", id3v2.Genre));
                System.Console.WriteLine(string.Format("Pictures:  {0}", id3v2.PictureList.Count));
                System.Console.WriteLine(string.Format("Comments:  {0}", id3v2.CommentsList.Count));
                System.Console.WriteLine();
            }

            if (ID3v1Tag.DoesTagExist(fileName))
            {
                IID3v1Tag id3v1 = new ID3v1Tag(fileName);

                System.Console.WriteLine(EnumUtils.GetDescription(id3v1.TagVersion));
                System.Console.WriteLine();

                System.Console.WriteLine(string.Format("Artist:    {0}", id3v1.Artist));
                System.Console.WriteLine(string.Format("Title:     {0}", id3v1.Title));
                System.Console.WriteLine(string.Format("Album:     {0}", id3v1.Album));
                System.Console.WriteLine(string.Format("Year:      {0}", id3v1.Year));
                System.Console.WriteLine(string.Format("Comment:   {0}", id3v1.Comment));
                System.Console.WriteLine(string.Format("Track:     {0}", id3v1.TrackNumber));
                System.Console.WriteLine(string.Format("Genre:     {0}", GenreHelper.GenreByIndex[id3v1.GenreIndex]));
                System.Console.WriteLine();
            }
        }

        private static string GetFileName()
        {
            System.Console.Write("File name: ");
            string fileName = System.Console.ReadLine();

            if (!File.Exists(fileName))
            {
                string tryFileName = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(tryFileName))
                {
                    fileName = tryFileName;
                }
                else
                {
                    System.Console.WriteLine(string.Format("\"{0}\" not found.", fileName));
                    return null;
                }
            }

            return fileName;
        }
    }
}
