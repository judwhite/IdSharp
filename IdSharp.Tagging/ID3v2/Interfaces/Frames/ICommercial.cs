using System;
using System.ComponentModel;
using IdSharp.Tagging.ID3v2.Frames.Items;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Commercial frame</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>This frame enables several competing offers in the same tag by
    ///    bundling all needed information. That makes this frame rather complex
    ///    but it's an easier solution than if one tries to achieve the same
    ///    result with several frames. The frame begins, after the frame ID,
    ///    size and encoding fields, with a price string field. A price is
    ///    constructed by one three character currency code, encoded according
    ///    to ISO-4217 alphabetic currency code, followed by a
    ///    numerical value where "." is used as decimal separator. In the price
    ///    string several prices may be concatenated, separated by a "/"
    ///    character, but there may only be one currency of each type.</para>
    ///
    ///    <para>The price string is followed by an 8 character date string in the
    ///    format YYYYMMDD, describing for how long the price is valid. After
    ///    that is a contact URL, with which the user can contact the seller,
    ///    followed by a one byte 'received as' field. It describes how the
    ///    audio is delivered when bought according to the following list:</para>
    ///
    ///         <para>$00  Other<br />
    ///         $01  Standard CD album with other songs<br />
    ///         $02  Compressed audio on CD<br />
    ///         $03  File over the Internet<br />
    ///         $04  Stream over the Internet<br />
    ///         $05  As note sheets<br />
    ///         $06  As note sheets in a book with other sheets<br />
    ///         $07  Music on other media<br />
    ///         $08  Non-musical merchandise</para>
    ///
    ///    <para>Next follows a terminated string with the name of the seller followed
    ///    by a terminated string with a short description of the product. The
    ///    last thing is the ability to include a company logotype. The first of
    ///    them is the 'Picture MIME type' field containing information about
    ///    which picture format is used. In the event that the MIME media type
    ///    name is omitted, "image/" will be implied. Currently only "image/png"
    ///    and "image/jpeg" are allowed. This format string is followed by the
    ///    binary picture data. This two last fields may be omitted if no
    ///    picture is attached. There may be more than one 'commercial frame' in
    ///    a tag, but no two may be identical.</para>
    /// 
    ///      <para>[Header for 'Commercial frame', ID: "COMR"]<br />
    ///      Text encoding      $xx<br />
    ///      Price string       [text string] $00<br />
    ///      Valid until        [text string]<br />
    ///      Contact URL        [text string] $00<br />
    ///      Received as        $xx<br />
    ///      Name of seller     [text string according to encoding] $00 (00)<br />
    ///      Description        [text string according to encoding] $00 (00)<br />
    ///      Picture MIME type  [string] $00<br />
    ///      Seller logo        [binary data]</para>
    /// </summary>
    public interface ICommercial : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets the BindingList of price information.
        /// </summary>
        /// <value>The BindingList of price information.</value>
        BindingList<IPriceInformation> PriceList { get; }

        /// <summary>
        /// Gets or sets the date which the price is valid until.
        /// </summary>
        /// <value>The date which the price is valid until.</value>
        DateTime ValidUntil { get; set; }

        /// <summary>
        /// Gets or sets the contact URL for the seller.
        /// </summary>
        /// <value>The contact URL for the seller.</value>
        string ContactUrl { get; set; }

        /// <summary>
        /// Gets or sets how the audio is delivered when bought.
        /// </summary>
        /// <value>How the audio is delivered when bought.</value>
        ReceivedAs ReceivedAs { get; set; }

        /// <summary>
        /// Gets or sets the name of the seller.
        /// </summary>
        /// <value>The name of the seller.</value>
        string NameOfSeller { get; set; }

        /// <summary>
        /// Gets or sets a short description of the product.
        /// </summary>
        /// <value>The short description of the product.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the seller logo.
        /// </summary>
        /// <value>The MIME type of the seller logo.</value>
        string SellerLogoMimeType { get; set; }

        /// <summary>
        /// Gets or sets the seller logo.  Returns a copy of the raw data, therefore the byte array cannot 
        /// be modified directly.  Use the set property to update the raw data.
        /// </summary>
        /// <value>The seller logo.</value>
        byte[] SellerLogo { get; set; }
    }
}
