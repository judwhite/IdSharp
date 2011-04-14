using System.Drawing;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Attached picture</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    /// <para>This frame contains a picture directly related to the audio file.
    /// Image format is the MIME type and subtype [MIME] for the image. In
    /// the event that the MIME media type name is omitted, "image/" will be
    /// implied. The "image/png" [PNG] or "image/jpeg" [JFIF] picture format
    /// should be used when interoperability is wanted. Description is a
    /// short description of the picture, represented as a terminated
    /// textstring. The description has a maximum length of 64 characters,
    /// but may be empty. There may be several pictures attached to one file,
    /// each in their individual "APIC" frame, but only one with the same
    /// content descriptor [editor's note: meaning, short description]. There may only be one picture with the picture
    /// type declared as picture type $01 and $02 respectively. There is the
    /// possibility to put only a link to the image file by using the 'MIME
    /// type' "-->" and having a complete URL [URL] instead of picture data.
    /// The use of linked files should however be used sparingly since there
    /// is the risk of separation of files.</para>
    /// 
    ///      <para>[Header for 'Attached picture', ID: "APIC"]<br />
    ///      Text encoding      $xx<br />
    ///      MIME type          [text string] $00<br />
    ///      Picture type       $xx<br />
    ///      Description        [text string according to encoding] $00 (00)<br />
    ///      Picture data       [binary data]</para>
    /// </summary>
    public interface IAttachedPicture : IFrame, ITextEncoding
    {
        /// <summary>
        /// <para>Gets or sets the MIME type and subtype for the image.  In
        /// the event that the MIME media type name is omitted, "image/" will be
        /// implied. The "image/png" [PNG] or "image/jpeg" [JFIF] picture format
        /// should be used when interoperability is wanted.</para><para>Additionally, there is the
        /// possibility to put only a link to the image file by using the 'MIME
        /// type' "-->" and having a complete URL [URL] instead of picture data.
        /// The use of linked files should however be used sparingly since there
        /// is the risk of separation of files.</para>
        /// </summary>
        /// <value>The MIME type and subtype, or "-->" if a URL is contained in the 
        /// <see cref="IAttachedPicture.PictureData"/> property.</value>
        string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the type of the picture.  There may only be one picture with the picture type
        /// <see cref="IdSharp.Tagging.ID3v2.PictureType.FileIcon32x32Png"/> and <see cref="IdSharp.Tagging.ID3v2.PictureType.OtherFileIcon"/>.
        /// </summary>
        /// <value>The type of the picture.</value>
        PictureType PictureType { get; set; }

        /// <summary>
        /// Gets or sets a short description of the picture.  This description must be unique
        /// within the tag.  The maximum length for this field is 
        /// 64 characters, but may be blank.  Note that if the original tag has a description longer than
        /// 64 characters it will be returned as it was originally stored in the tag, and written back with
        /// all original characters in tact (that is, possibly with more than 64 characters).
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the raw picture data, or URL.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>A copy of the raw picture data, or URL.</value>
        byte[] PictureData { get; set; }

        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        Image Picture { get; set; }

        /// <summary>
        /// Gets the picture extension.
        /// </summary>
        /// <value>The picture extension.</value>
        string PictureExtension { get; }
    }
}
