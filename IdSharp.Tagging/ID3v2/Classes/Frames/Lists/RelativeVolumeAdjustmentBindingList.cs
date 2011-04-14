using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class RelativeVolumeAdjustmentBindingList : BindingList<IRelativeVolumeAdjustment>
    {
        public RelativeVolumeAdjustmentBindingList()
        {
            AllowNew = true;
        }

        public RelativeVolumeAdjustmentBindingList(IList<IRelativeVolumeAdjustment> relativeVolumeAdjustmentList)
            : base(relativeVolumeAdjustmentList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IRelativeVolumeAdjustment relativeVolumeAdjustment = new RelativeVolumeAdjustment();
            Add(relativeVolumeAdjustment);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return relativeVolumeAdjustment;
        }

        protected override void InsertItem(int index, IRelativeVolumeAdjustment item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
