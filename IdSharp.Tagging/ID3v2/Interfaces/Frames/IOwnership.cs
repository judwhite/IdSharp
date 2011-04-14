using System;

namespace IdSharp.Tagging.ID3v2.Frames
{
    /// <summary>
    /// <para>Ownership frame</para>
    /// 
    /// <para>From the ID3v2 specification:</para>
    /// 
    ///    <para>The ownership frame might be used as a reminder of a made transaction
    ///    or, if signed, as proof. Note that the "USER" and "TOWN" frames are
    ///    good to use in conjunction with this one. The frame begins, after the
    ///    frame ID, size and encoding fields, with a 'price payed' field. The
    ///    first three characters of this field contains the currency used for
    ///    the transaction, encoded according to ISO-4217 alphabetic
    ///    currency code. Concatenated to this is the actual price payed, as a
    ///    numerical string using "." as the decimal separator. Next is an 8
    ///    character date string (YYYYMMDD) followed by a string with the name
    ///    of the seller as the last field in the frame. There may only be one
    ///    "OWNE" frame in a tag.</para>
    /// 
    ///      <para>[Header for 'Ownership frame', ID: "OWNE"]<br />
    ///      Text encoding     $xx<br />
    ///      Price payed       [text string] $00<br />
    ///      Date of purch.    [text string]<br />
    ///      Seller            [text string according to encoding]</para>
    /// </summary>
    public interface IOwnership : IFrame, ITextEncoding
    {
        /// <summary>
        /// Gets or sets the price paid in the currency indicated in the CurrencyCode property.
        /// </summary>
        /// <value>The price paid in the currency indicated in the CurrencyCode property.</value>
        decimal PricePaid { get; set; }

        /// <summary>
        /// Gets or sets the ISO-4217 currency code.
        /// </summary>
        /// <value>The ISO-4217 currency code.</value>
        string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the date of purchase.
        /// </summary>
        /// <value>The date of purchase.</value>
        DateTime DateOfPurchase { get; set; }

        /// <summary>
        /// Gets or sets the name of the seller.
        /// </summary>
        /// <value>The name of the seller.</value>
        string Seller { get; set; }
    }
}
