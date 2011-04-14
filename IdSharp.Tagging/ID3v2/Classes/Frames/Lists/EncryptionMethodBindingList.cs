using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class EncryptionMethodBindingList : BindingList<IEncryptionMethod>
    {
        public EncryptionMethodBindingList()
        {
            AllowNew = true;
        }

        public EncryptionMethodBindingList(IList<IEncryptionMethod> encryptionMethodList)
            : base(encryptionMethodList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IEncryptionMethod encryptionMethod = new EncryptionMethod();
            Add(encryptionMethod);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return encryptionMethod;
        }

        protected override void InsertItem(int index, IEncryptionMethod item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
