namespace Wetware.Globals;

public static class TurnQueue
{
    /// <summary>
    /// Stores entity IDs and energy amounts. When dequeueing, it's compared against the entity
    /// index to validate staleness of entries. If an entity doesn't match the latest update in
    /// the entity index, it's dequeued and discarded, and the next is retrieved.
    /// </summary>
    private static readonly PriorityQueue<int, int> _queue = new();

    /// <summary>
    /// An index of entity IDs and their last-updated energy amounts. Used for validating the
    /// staleness of entities dequeued from the turn order queue.
    /// </summary>
    private static readonly Dictionary<int, int> _entityIndex = new();

    public static bool IsEmpty => _queue.Count == 0;

    /// <summary>Add an entityId to the queue with its current energy.</summary>
    /// <remarks>PriorityQueue is min-first, so negate energy for descending order.</remarks>
    public static void Enqueue(int id, int energy)
    {
        if (energy < 0) return;
        _entityIndex[id] = energy;
        _queue.Enqueue(id, -energy);
    }

    /// <summary>Fetch the next valid entity in the queue, or null if empty.</summary>
    public static int? Next()
    {
        while (!IsEmpty)
        {
            _queue.TryDequeue(out int id, out int energy);

            // If not in index, it was removed. Skip. 
            if (!_entityIndex.TryGetValue(id, out var lastUpdate))
                continue;

            // If in index but not equal to last update, it is stale. Skip.
            if (energy != lastUpdate)
                continue;

            return id;
        }
        return null;
    }

    /// <summary>Remove an entity from the entity index, invalidating it in the PriorityQueue.</summary>
    public static void Remove(int id) => _entityIndex.Remove(id);

    /// <summary>Clear the index and queue. Blank slate.</summary>
    public static void Clear()
    {
        _queue.Clear();
        _entityIndex.Clear();
    }
}
