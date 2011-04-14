namespace IdSharp.Common.Events
{
    /// <summary>
    /// Allows an object to publish information about incorrectly formatted data, which can be treated as either a warning or error.
    /// </summary>
    public interface INotifyInvalidData
    {
        /// <summary>
        /// Occurs when a property in the object has been set to an incorrect state.
        /// </summary>
        event InvalidDataEventHandler InvalidData;
    }
}