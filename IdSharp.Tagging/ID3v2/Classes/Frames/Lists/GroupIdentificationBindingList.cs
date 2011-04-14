using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class GroupIdentificationBindingList : BindingList<IGroupIdentification>
    {
        public GroupIdentificationBindingList()
        {
            AllowNew = true;
        }

        public GroupIdentificationBindingList(IList<IGroupIdentification> groupIdentificationList)
            : base(groupIdentificationList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IGroupIdentification groupIdentification = new GroupIdentification();
            Add(groupIdentification);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return groupIdentification;
        }

        protected override void InsertItem(int index, IGroupIdentification item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
