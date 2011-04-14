using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class EventTimingItemBindingList : BindingList<IEventTimingItem>
    {
        public EventTimingItemBindingList()
        {
            AllowNew = true;
        }

        public EventTimingItemBindingList(IList<IEventTimingItem> eventTimingItemList)
            : base(eventTimingItemList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEventTimingItem eventTimingItem = new EventTimingItem();
            Add(eventTimingItem);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return eventTimingItem;
        }

        protected override void InsertItem(int index, IEventTimingItem item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
