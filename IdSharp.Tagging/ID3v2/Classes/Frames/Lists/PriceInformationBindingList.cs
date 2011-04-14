using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class PriceInformationBindingList : BindingList<IPriceInformation>
    {
        public PriceInformationBindingList()
        {
            AllowNew = true;
        }

        public PriceInformationBindingList(IList<IPriceInformation> priceInformationList)
            : base(priceInformationList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IPriceInformation priceInformation = new PriceInformation();
            Add(priceInformation);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return priceInformation;
        }

        protected override void InsertItem(int index, IPriceInformation item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
