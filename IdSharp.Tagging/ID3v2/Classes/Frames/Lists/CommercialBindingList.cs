using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class CommercialBindingList : BindingList<ICommercial>
    {
        public CommercialBindingList()
        {
            AllowNew = true;
        }

        public CommercialBindingList(IList<ICommercial> commercialInfoList)
            : base(commercialInfoList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ICommercial commercialInfo = new Commercial();
            Add(commercialInfo);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return commercialInfo;
        }

        protected override void InsertItem(int index, ICommercial item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
