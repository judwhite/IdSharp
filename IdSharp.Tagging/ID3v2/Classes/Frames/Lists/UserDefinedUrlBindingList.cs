using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class UserDefinedUrlBindingList : BindingList<IWXXXFrame>
    {
        public UserDefinedUrlBindingList()
        {
            AllowNew = true;
        }

        public UserDefinedUrlBindingList(IList<IWXXXFrame> userDefineUrlList)
            : base(userDefineUrlList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IWXXXFrame tmpUserDefinedUrl = new WXXXFrame();
            Add(tmpUserDefinedUrl);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return tmpUserDefinedUrl;
        }

        protected override void InsertItem(int index, IWXXXFrame item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
