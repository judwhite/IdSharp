using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Common.Events;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class AttachedPictureBindingList : BindingList<IAttachedPicture>
    {
        public AttachedPictureBindingList()
        {
            AllowNew = true;
        }

        public AttachedPictureBindingList(IList<IAttachedPicture> attachedPictureList)
            : base(attachedPictureList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IAttachedPicture attachedPicture = new AttachedPicture();
            Add(attachedPicture);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return attachedPicture;
        }

        private void AttachedPicture_PictureTypeChanging(object sender, CancelDataEventArgs<PictureType> e)
        {
            foreach (IAttachedPicture attachedPicture in Items)
            {
                if (e.Data == PictureType.OtherFileIcon && attachedPicture.PictureType == PictureType.OtherFileIcon)
                {
                    // TODO: Make this a warning
                    //e.Cancel = true;
                    //e.CancelReason = "There may only be one picture of this picture type in the tag";
                    return;
                }
                if (e.Data == PictureType.FileIcon32x32Png && attachedPicture.PictureType == PictureType.FileIcon32x32Png)
                {
                    // TODO: Make this a warning
                    //e.Cancel = true;
                    //e.CancelReason = "There may only be one picture of this picture type in the tag";
                    return;
                }
            }
        }

        private void AttachedPicture_DescriptionChanging(object sender, CancelDataEventArgs<string> e)
        {
            if (string.IsNullOrEmpty(e.Data)) 
                return;

            foreach (IAttachedPicture attachedPicture in Items)
            {
                if (attachedPicture != sender)
                {
                    if (string.IsNullOrEmpty(attachedPicture.Description) == false)
                    {
                        if (string.Compare(attachedPicture.Description, e.Data, false) == 0)
                        {
                            // TODO: Make this a warning
                            //e.Cancel = true;
                            //e.CancelReason = "A picture with this description already exists in the tag";
                            return;
                        }
                    }
                }
            }
        }

        protected override void InsertItem(int index, IAttachedPicture item)
        {
            /*foreach (IAttachedPicture tmpAttachedPicture in base.Items)
            {
                if (String.IsNullOrEmpty(item.Description) == false && String.IsNullOrEmpty(tmpAttachedPicture.Description) == false)
                {
                    if (String.Compare(item.Description, tmpAttachedPicture.Description, false) == 0)
                    {
                        // TODO: Make this a warning
                        //("A picture with this description already exists in the tag");
                    }
                }
                if (item.PictureType == PictureType.OtherFileIcon && tmpAttachedPicture.PictureType == PictureType.OtherFileIcon)
                {
                    // TODO: Make this a warning
                    //("There may only be one picture of this picture type in the tag");
                }
                if (item.PictureType == PictureType.FileIcon32x32PNG && tmpAttachedPicture.PictureType == PictureType.FileIcon32x32PNG)
                {
                    // TODO: Make this a warning
                    //("There may only be one picture of this picture type in the tag");
                }
            }*/

            // TODO: How to handle this with INotifyPropertyChanged?
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
