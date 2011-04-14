namespace IdSharp.Common.Events
{
    /// <summary>
    /// Represents the method that will handle the <see cref="INotifyInvalidData.InvalidData"/> event.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">InvalidDataEventArgs</param>
    public delegate void InvalidDataEventHandler(object sender, InvalidDataEventArgs e);
}