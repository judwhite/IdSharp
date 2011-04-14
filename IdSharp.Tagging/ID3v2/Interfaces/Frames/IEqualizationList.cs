using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Equalization</para>
    /// </summary>
    public interface IEqualizationList : IFrame
    {
        /// <summary>
        /// Gets or sets the interpolation method.  Only valid in ID3v2.4.
        /// </summary>
        /// <value>The interpolation method.  Only valid in ID3v2.4.</value>
        InterpolationMethod InterpolationMethod { get; set; }

        /// <summary>
        /// Gets or sets the identification string.  Used to identify the situation and/or
        /// device where this adjustment should apply.  Only valid in ID3v2.4.
        /// </summary>
        /// <value>The identification string.  Only valid in ID3v2.4.</value>
        string Identification { get; set; }

        /*/// <summary>
        /// Gets or sets the number of bits used to represent the adjustment.  Normally 16 bits.
        /// </summary>
        /// <value>The number of bits used to represent the adjustment.  Normally 16 bits.</value>
        Int16 AdjustmentBits { get; set; }*/

        /// <summary>
        /// Gets the BindingList of <see cref="IEqualizationItem"/> items.
        /// </summary>
        /// <value>The BindingList of <see cref="IEqualizationItem"/> items.</value>
        BindingList<IEqualizationItem> Items { get; }
    }
}
