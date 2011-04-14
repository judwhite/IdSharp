using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class InvolvedPersonBindingList : BindingList<IInvolvedPerson>
    {
        public InvolvedPersonBindingList()
        {
            AllowNew = true;
        }

        public InvolvedPersonBindingList(IList<IInvolvedPerson> involvedPersonList)
            : base(involvedPersonList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IInvolvedPerson involvedPerson = new InvolvedPerson();
            Add(involvedPerson);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return involvedPerson;
        }

        protected override void InsertItem(int index, IInvolvedPerson item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
