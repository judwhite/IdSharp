using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class UniqueFileIdentifierBindingList : BindingList<IUniqueFileIdentifier>
    {
        public UniqueFileIdentifierBindingList()
        {
            AllowNew = true;
        }

        public UniqueFileIdentifierBindingList(IList<IUniqueFileIdentifier> uniqueFileIdentifierList)
            : base(uniqueFileIdentifierList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IUniqueFileIdentifier uniqueFileIdentifier = new UniqueFileIdentifier();
            Add(uniqueFileIdentifier);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return uniqueFileIdentifier;
        }

        protected override void InsertItem(int index, IUniqueFileIdentifier item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
