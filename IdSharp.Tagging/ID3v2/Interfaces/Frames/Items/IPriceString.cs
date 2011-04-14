using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// Price information.  Used by <see cref="ICommercial"/>.
    /// </summary>
    public interface IPriceInformation : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the ISO-4217 currency code.
        /// </summary>
        /// <value>The ISO-4217 currency code.</value>
        string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the price in the currency indicated in the CurrencyCode property.
        /// </summary>
        /// <value>The price in the currency indicated in the CurrencyCode property.</value>
        decimal Price { get; set; }
    }
}
