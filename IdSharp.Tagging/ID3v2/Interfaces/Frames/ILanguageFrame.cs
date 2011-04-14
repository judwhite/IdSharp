using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Language frame</para>
    /// 
    /// <para>The 'Language' frame should contain the languages of the text or
    /// lyrics spoken or sung in the audio. The language is represented with
    /// three characters according to ISO-639-2 [ISO-639-2]. If more than one
    /// language is used in the text their language codes should follow
    /// according to the amount of their usage, e.g. "eng" $00 "sve" $00.</para>
    /// </summary>
    public interface ILanguageFrame : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets the BindingList of language items.
        /// </summary>
        /// <value>The BindingList of language items.</value>
        BindingList<ILanguageItem> Items { get; }
    }
}
