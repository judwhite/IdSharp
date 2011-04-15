using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdSharp.Tagging.Harness.Wpf.Events;
using IdSharp.Tagging.Harness.Wpf.ViewModel.Interfaces;
using IdSharp.Tagging;

namespace IdSharp.Tagging.Harness.Wpf.ViewModel
{
    public class ID3v2ViewModel : ViewModelBase, IID3v2ViewModel
    {
        public ID3v2ViewModel()
        {
            EventDispatcher.Subscribe<string>(EventType.LoadFile, OnLoadFile);
            EventDispatcher.Subscribe<string>(EventType.SaveFile, OnSaveFile);
        }

        private void OnLoadFile(string fileName)
        {
            ID3v2.ID3v2 id3v2 = new ID3v2.ID3v2(fileName);

            // TODO
        }

        private void OnSaveFile(string fileName)
        {
            ID3v2.ID3v2 id3v2 = new ID3v2.ID3v2(fileName);

            // TODO
        }
    }
}
