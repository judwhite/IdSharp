using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class PopularimeterBindingList : BindingList<IPopularimeter>
    {
        public PopularimeterBindingList()
        {
            AllowNew = true;
        }

        public PopularimeterBindingList(IList<IPopularimeter> popularimeterList)
            : base(popularimeterList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IPopularimeter popularimeterFrame = new Popularimeter();
            Add(popularimeterFrame);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return popularimeterFrame;
        }

        protected override void InsertItem(int index, IPopularimeter item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
