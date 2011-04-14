using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class MpegLookupTableItemBindingList : BindingList<IMpegLookupTableItem>
    {
        public MpegLookupTableItemBindingList()
        {
            AllowNew = true;
        }

        public MpegLookupTableItemBindingList(IList<IMpegLookupTableItem> mpegLookupTableItemList)
            : base(mpegLookupTableItemList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IMpegLookupTableItem mpegLookupTableItem = new MpegLookupTableItem();
            Add(mpegLookupTableItem);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return mpegLookupTableItem;
        }

        protected override void InsertItem(int index, IMpegLookupTableItem item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
