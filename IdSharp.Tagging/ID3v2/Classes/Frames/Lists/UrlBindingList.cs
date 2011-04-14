using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class UrlBindingList : BindingList<IUrlFrame>
    {
        private readonly string _id3v24FrameID;
        private readonly string _id3v23FrameID;
        private readonly string _id3v22FrameID;

        public UrlBindingList(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID)
        {
            AllowNew = true;
            _id3v24FrameID = id3v24FrameID;
            _id3v23FrameID = id3v23FrameID;
            _id3v22FrameID = id3v22FrameID;
        }

        public UrlBindingList(string id3v24FrameID, string id3v23FrameID, string id3v22FrameID, IList<IUrlFrame> urlList)
            : base(urlList)
        {
            AllowNew = true;
            _id3v24FrameID = id3v24FrameID;
            _id3v23FrameID = id3v23FrameID;
            _id3v22FrameID = id3v22FrameID;
        }

        public UrlBindingList()
        {
            throw new NotSupportedException("Use constructor with ID3v2 FrameID's");
        }

        public UrlBindingList(IList<IUrlFrame> urlList)
            : base(urlList)
        {
            throw new NotSupportedException("Use constructor with ID3v2 FrameID's");
        }

        protected override object AddNewCore()
        {
            IUrlFrame url = new UrlFrame(_id3v24FrameID, _id3v23FrameID, _id3v22FrameID);
            Add(url);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return url;
        }

        protected override void InsertItem(int index, IUrlFrame item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
