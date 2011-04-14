using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class SynchronizedTextBindingList : BindingList<ISynchronizedText>
    {
        public SynchronizedTextBindingList()
        {
            AllowNew = true;
        }

        public SynchronizedTextBindingList(IList<ISynchronizedText> synchronizedTextList)
            : base(synchronizedTextList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ISynchronizedText synchronizedText = new SynchronizedText();
            Add(synchronizedText);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return synchronizedText;
        }

        protected override void InsertItem(int index, ISynchronizedText item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
