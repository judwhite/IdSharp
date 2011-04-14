using System.Collections.Generic;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class TempoDataBindingList : BindingList<ITempoData>
    {
        public TempoDataBindingList()
        {
            AllowNew = true;
        }

        public TempoDataBindingList(IList<ITempoData> tempoDataList)
            : base(tempoDataList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            ITempoData tempoData = new TempoData();
            Add(tempoData);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return tempoData;
        }

        protected override void InsertItem(int index, ITempoData item)
        {
            //item.DescriptionChanging += new EventHandler<CancelDataEventArgs<String>>(AttachedPicture_DescriptionChanging);
            //item.PictureTypeChanging += new EventHandler<CancelDataEventArgs<PictureType>>(AttachedPicture_PictureTypeChanging);

            base.InsertItem(index, item);
        }
    }
}
