using System.Collections.Generic;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Lists
{
    internal sealed class CommentsBindingList : BindingList<IComments>
    {
        public CommentsBindingList()
        {
            AllowNew = true;
        }

        public CommentsBindingList(IList<IComments> commentList)
            : base(commentList)
        {
            AllowNew = true;
        }

        protected override object AddNewCore()
        {
            IComments comment = new Comments();
            Add(comment);

            // Not necessary to hook up event handlers, base class calls InsertItem

            return comment;
        }

        protected override void InsertItem(int index, IComments item)
        {
            foreach (IComments comment in Items)
            {
            }

            // TODO: Hook up to NotifyPropertyChanged

            base.InsertItem(index, item);
        }
    }
}
