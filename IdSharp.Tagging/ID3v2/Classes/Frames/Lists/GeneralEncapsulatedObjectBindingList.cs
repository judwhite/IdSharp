using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class GeneralEncapsulatedObjectBindingList : BindingList<IGeneralEncapsulatedObject>
    {
        public GeneralEncapsulatedObjectBindingList()
        {
            AllowNew = true;
        }

        public GeneralEncapsulatedObjectBindingList(IList<IGeneralEncapsulatedObject> generalEncapsulatedObjectList)
            : base(generalEncapsulatedObjectList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IGeneralEncapsulatedObject tmpGeneralEncapsulatedObject = new GeneralEncapsulatedObject();
            Add(tmpGeneralEncapsulatedObject);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return tmpGeneralEncapsulatedObject;
        }

        protected override void InsertItem(int index, IGeneralEncapsulatedObject item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
