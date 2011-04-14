using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    internal sealed class InvolvedPerson : IInvolvedPerson
    {
        private string _name;
        private string _involvement;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                SendPropertyChanged("Name");
            }
        }

        public string Involvement
        {
            get { return _involvement; }
            set
            {
                _involvement = value;
                SendPropertyChanged("Involvement");
            }
        }

        private void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
