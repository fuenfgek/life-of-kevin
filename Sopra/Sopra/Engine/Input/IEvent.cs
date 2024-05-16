namespace Sopra.Engine.Input
{
    /// <summary>
    /// Base interface for input events.
    /// </summary>
    /// <author>Michael Fleig</author>
    public interface IEvent
    {
        
        /// <summary>
        /// Determine wheter the event was handled or not.
        /// </summary>
        bool Handled { get; set; }
    }
}