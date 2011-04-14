using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// Musician credits list.
    /// </summary>
    public interface IMusicianCreditsList : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets the BindingList of <see cref="IMusicianCreditsItem"/> items.
        /// </summary>
        /// <value>The BindingList of <see cref="IMusicianCreditsItem"/> items.</value>
        BindingList<IMusicianCreditsItem> Items { get; }
    }
}
