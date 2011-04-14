using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class LinkedInformationBindingList : BindingList<ILinkedInformation>
    {
        public LinkedInformationBindingList()
        {
            AllowNew = true;
        }

        public LinkedInformationBindingList(IList<ILinkedInformation> linkedInformationList)
            : base(linkedInformationList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ILinkedInformation linkedInformation = new LinkedInformation();
            Add(linkedInformation);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return linkedInformation;
        }

        protected override void InsertItem(int index, ILinkedInformation item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
