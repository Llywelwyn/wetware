using Wetware.Serializer;

namespace Wetware.Globals;

public static class TurnQueue
{
    /// <summary>
    /// Stores entity IDs and energy amounts. When dequeueing, it's compared against the entity
    /// index to validate staleness of entries. If an entity doesn't match the latest update in
    /// the entity index, it's dequeued and discarded, and the next is retrieved.
    /// </summary>
    private static readonly PriorityQueue<int, int> m_queue = new();

    /// <summary>
    /// An index of entity IDs and their last-updated energy amounts. Used for validating the
    /// staleness of entities dequeued from the turn order queue.
    /// </summary>
    private static readonly Dictionary<int, int> m_entityIndex = [];

    public static bool IsEmpty => m_queue.Count == 0;

    /// <summary>Add an entityId to the queue with its current energy.</summary>
    /// <remarks>PriorityQueue is min-first, so negate energy for descending order.</remarks>
    public static void Enqueue(int id, int energy)
    {
        if (energy < 0) return;
        m_entityIndex[id] = -energy;
        m_queue.Enqueue(id, -energy);
    }

    /// <summary>Fetch the next valid entity in the queue, or null if empty.</summary>
    public static int? Next()
    {
        PrintQueue();
        while (!IsEmpty)
        {
            m_queue.TryDequeue(out int id, out int energy);

            // If not in index, it was removed. Skip. 
            if (!m_entityIndex.TryGetValue(id, out var lastUpdate))
                continue;

            // If in index but not equal to last update, it is stale. Skip.
            if (energy != lastUpdate)
                continue;

            return id;
        }
        return null;
    }

    /// <summary>Remove an entity from the entity index, invalidating it in the PriorityQueue.</summary>
    public static void Remove(int id) => m_entityIndex.Remove(id);

    /// <summary>Clear the index and queue. Blank slate.</summary>
    public static void Clear()
    {
        m_queue.Clear();
        m_entityIndex.Clear();
    }

    public static void PrintQueue()
    {
        Console.WriteLine("TurnQueue (latest values only):");
        foreach (var kv in m_entityIndex.OrderByDescending(kv => -kv.Value))
        {
            Console.WriteLine($"id: {kv.Key}, energy: {-kv.Value}");
        }
    }

    public static IEnumerable<TurnQueueEntry> GetSnapshot()
    {
        foreach (var kv in m_entityIndex)
            yield return new TurnQueueEntry { Id = kv.Key, Priority = -kv.Value };
    }

    public static void LoadSnapshot(List<TurnQueueEntry> snapshot)
    {
        Clear();
        foreach (var entry in snapshot)
        {
            Console.WriteLine($"{entry.Id}: {entry.Priority}");
            Enqueue(entry.Id, entry.Priority);
        }
    }
}
