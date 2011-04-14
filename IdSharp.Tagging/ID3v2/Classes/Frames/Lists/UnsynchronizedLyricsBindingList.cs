using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class UnsynchronizedLyricsBindingList : BindingList<IUnsynchronizedText>
    {
        public UnsynchronizedLyricsBindingList()
        {
            AllowNew = true;
        }

        public UnsynchronizedLyricsBindingList(IList<IUnsynchronizedText> urlList)
            : base(urlList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IUnsynchronizedText unsynchronizedText = new UnsynchronizedText();
            Add(unsynchronizedText);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return unsynchronizedText;
        }

        protected override void InsertItem(int index, IUnsynchronizedText item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
