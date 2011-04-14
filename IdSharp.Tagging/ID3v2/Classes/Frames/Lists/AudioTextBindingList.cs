using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class AudioTextBindingList : BindingList<IAudioText>
    {
        public AudioTextBindingList()
        {
            AllowNew = true;
        }

        public AudioTextBindingList(IList<IAudioText> audioTextList)
            : base(audioTextList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IAudioText audioText = new AudioText();
            Add(audioText);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return audioText;
        }

        protected override void InsertItem(int index, IAudioText item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
