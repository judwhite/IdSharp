using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class TermsOfUseBindingList : BindingList<ITermsOfUse>
    {
        public TermsOfUseBindingList()
        {
            AllowNew = true;
        }

        public TermsOfUseBindingList(IList<ITermsOfUse> urlList)
            : base(urlList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ITermsOfUse termsOfUse = new TermsOfUse();
            Add(termsOfUse);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return termsOfUse;
        }

        protected override void InsertItem(int index, ITermsOfUse item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
