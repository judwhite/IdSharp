namespace IdSharp.Tagging.Harness.Wpf.Model
{
    public class Track
    {
        public Track(string artist, string title, string album, string year, string genre, string fileName)
        {
            Artist = artist;
            Title = title;
            Album = album;
            Year = year;
            Genre = genre;
            FileName = fileName;
        }

        public string FileName { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public string Album { get; set; }

        public string Year { get; set; }

        public string Genre { get; set; }
    }
}
