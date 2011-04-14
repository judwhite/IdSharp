using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class LanguageItemBindingList : BindingList<ILanguageItem>
    {
        public LanguageItemBindingList()
        {
            AllowNew = true;
        }

        public LanguageItemBindingList(IList<ILanguageItem> languageItemList)
            : base(languageItemList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ILanguageItem languageItem = new LanguageItem();
            Add(languageItem);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return languageItem;
        }

        protected override void InsertItem(int index, ILanguageItem item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
