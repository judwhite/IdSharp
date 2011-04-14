using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class PrivateFrameBindingList : BindingList<IPrivateFrame>
    {
        public PrivateFrameBindingList()
        {
            AllowNew = true;
        }

        public PrivateFrameBindingList(IList<IPrivateFrame> privateFrameList)
            : base(privateFrameList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IPrivateFrame privateFrame = new PrivateFrame();
            Add(privateFrame);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return privateFrame;
        }

        protected override void InsertItem(int index, IPrivateFrame item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
