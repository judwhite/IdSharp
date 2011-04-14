using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class EqualizationListBindingList : BindingList<IEqualizationList>
    {
        public EqualizationListBindingList()
        {
            AllowNew = true;
        }

        public EqualizationListBindingList(IList<IEqualizationList> equalizationListList)
            : base(equalizationListList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEqualizationList equalizationList = new EqualizationList();
            Add(equalizationList);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return equalizationList;
        }

        protected override void InsertItem(int index, IEqualizationList item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
