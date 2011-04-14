using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class UserDefinedTextBindingList : BindingList<ITXXXFrame>
    {
        public UserDefinedTextBindingList()
        {
            AllowNew = true;
        }

        public UserDefinedTextBindingList(IList<ITXXXFrame> userDefineTextList)
            : base(userDefineTextList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ITXXXFrame userDefinedText = new TXXXFrame();
            Add(userDefinedText);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return userDefinedText;
        }

        protected override void InsertItem(int index, ITXXXFrame item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
