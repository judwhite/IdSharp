using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2.Frames.Items
{
    /// <summary>
    /// Involved person item.  See <see cref="IInvolvedPersonList"/>.
    /// </summary>
    public interface IInvolvedPerson : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        /// <value>The name of the person.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the person's involvement.
        /// </summary>
        /// <value>The person's involvement.</value>
        string Involvement { get; set; }
    }
}
