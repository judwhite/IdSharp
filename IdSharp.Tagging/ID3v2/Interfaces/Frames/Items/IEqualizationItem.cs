namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// Equalization item.  See <see cref="IEqualizationList"/>.
    /// </summary>
    public interface IEqualizationItem
    {
        /// <summary>
        /// Gets or sets the direction of the volume adjustment.
        /// </summary>
        /// <value>The direction of the volume adjustment.</value>
        VolumeAdjustmentDirection VolumeAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the frequency.  Allowed range is 0 - 32767Hz.
        /// </summary>
        /// <value>The frequency.</value>
        short Frequency { get; set; }

        /// <summary>
        /// Gets or sets the adjustment.
        /// </summary>
        /// <value>The adjustment.</value>
        int Adjustment { get; set; }
    }
}
