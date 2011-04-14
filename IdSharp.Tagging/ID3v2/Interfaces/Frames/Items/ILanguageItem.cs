using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// A language item.  See <see cref="ILanguageFrame"/>.
    /// </summary>
    public interface ILanguageItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the ISO-639-2 language code.
        /// </summary>
        /// <value>The ISO-639-2 language code.</value>
        string LanguageCode { get; set; }

        /// <summary>
        /// Gets the language display in English.
        /// </summary>
        /// <value>The language display in English.</value>
        string LanguageDisplay { get; }
    }
}
