using System.IO;

namespace IdSharp.Tagging.Harness.Wpf.Model
{
    public class Track
    {
        private string _fullFileName;

        public string FullFileName
        {
            get { return _fullFileName; }
            set
            {
                _fullFileName = value;
                FileName = Path.GetFileName(_fullFileName);
            }
        }

        public string FileName { get; private set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public string Album { get; set; }

        public string Year { get; set; }

        public string Genre { get; set; }

        public Picture Picture { get; set; }
    }
}
