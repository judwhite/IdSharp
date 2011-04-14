using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Involved person list</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>Since there might be a lot of people contributing to an audio file in
    ///    various ways, such as musicians and technicians, the 'Text
    ///    information frames' are often insufficient to list everyone involved
    ///    in a project. The 'Involved people list' is a frame containing the
    ///    names of those involved, and how they were involved. The body simply
    ///    contains a terminated string with the involvement directly followed
    ///    by a terminated string with the involvee followed by a new
    ///    involvement and so on. There may only be one "IPLS" frame in each
    ///    tag.</para>
    /// 
    ///      <para>[Header for 'Involved people list', ID: "IPLS"]<br />
    ///      Text encoding          $xx<br />
    ///      People list strings    [text strings according to encoding]</para>
    /// </summary>
    public interface IInvolvedPersonList : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets the BindingList of involved persons.
        /// </summary>
        /// <value>The BindingList of involved persons.</value>
        BindingList<IInvolvedPerson> Items { get; }
    }
}
