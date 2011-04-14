using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class AudioEncryptionBindingList : BindingList<IAudioEncryption>
    {
        public AudioEncryptionBindingList()
        {
            AllowNew = true;
        }

        public AudioEncryptionBindingList(IList<IAudioEncryption> audioEncryptionList)
            : base(audioEncryptionList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IAudioEncryption audioEncryption = new AudioEncryption();
            Add(audioEncryption);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return audioEncryption;
        }

        protected override void InsertItem(int index, IAudioEncryption item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
