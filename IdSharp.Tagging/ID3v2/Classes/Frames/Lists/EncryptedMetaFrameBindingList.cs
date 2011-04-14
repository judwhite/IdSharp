using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class EncryptedMetaFrameBindingList : BindingList<IEncryptedMetaFrame>
    {
        public EncryptedMetaFrameBindingList()
        {
            AllowNew = true;
        }

        public EncryptedMetaFrameBindingList(IList<IEncryptedMetaFrame> encryptedMetaFrameList)
            : base(encryptedMetaFrameList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEncryptedMetaFrame encryptedMetaFrame = new EncryptedMetaFrame();
            Add(encryptedMetaFrame);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return encryptedMetaFrame;
        }

        protected override void InsertItem(int index, IEncryptedMetaFrame item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
