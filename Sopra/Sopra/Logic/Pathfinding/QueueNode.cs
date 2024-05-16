using Priority_Queue;

namespace Sopra.Logic.Pathfinding
{
    /// <inheritdoc />
    internal sealed class QueueNode : StablePriorityQueueNode
    {
        public int Id { get; }
        public QueueNode(int id)
        {
            Id = id;
        }
    }
}
