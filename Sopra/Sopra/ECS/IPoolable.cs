namespace Sopra.ECS
{
    /// <summary>
    /// Mark a class as poolable.
    /// </summary>
    /// <author>Michael Fleig</author>
    public interface IPoolable
    {
        /// <summary>
        /// Reset the state of the poolable object.
        /// </summary>
        void Reset();
    }}
