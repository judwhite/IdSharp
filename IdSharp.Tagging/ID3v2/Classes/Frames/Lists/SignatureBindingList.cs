using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class SignatureBindingList : BindingList<ISignature>
    {
        public SignatureBindingList()
        {
            AllowNew = true;
        }

        public SignatureBindingList(IList<ISignature> signatureList)
            : base(signatureList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ISignature signature = new Signature();
            Add(signature);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return signature;
        }

        protected override void InsertItem(int index, ISignature item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
