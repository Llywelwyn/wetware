using Friflo.Engine.ECS.Serialize;
using Wetware.Globals;
using MessagePack;
using Wetware.Config;
using System.Text.Json;
using Friflo.Engine.ECS;
using Wetware.Serializer.Mappers;

namespace Wetware.Serializer;

public static class WetwareSerializer
{
    public static EntityStore DeserializeEntityStore(byte[] bytes)
    {
        var store = new EntityStore();
        var es = new EntitySerializer();
        using var ms = new MemoryStream(bytes);
        es.ReadIntoStore(store, ms);
        return store;
    }

    public static void SerializeState()
    {
        var path = $"Runs/{Game.Instance.Name}";
        var state = SnapshotGameState();
        byte[] bytes = MessagePackSerializer.Serialize(state);
        File.WriteAllBytes(path, bytes);

        if (Debug.IsEnabled(DebugFlag.AdditionalJsonSerialization)) SerializeStateJson(path);
    }

    public static void SerializeStateJson(string path)
    {
        var state = SnapshotGameState();
        var options = new JsonSerializerOptions { WriteIndented = true, };
        string json = JsonSerializer.Serialize(state, options);
        File.WriteAllText($"{path}.json", json);
    }

    public static SaveState SnapshotGameState()
    {
        return new SaveState
        {
            Name = Game.Instance.Name,
            World = SerializeEntityStore(Game.Instance.World),
            TurnQueue = [.. TurnQueue.GetSnapshot()],
            ClockTurn = Game.Instance.ClockTurn,
            MapRepository = MapRepositoryMapper.ToDto(Game.Instance.MapRepository),
        };
    }

    public static byte[] SerializeEntityStore(EntityStore store)
    {
        var serializer = new EntitySerializer();
        using var ms = new MemoryStream();
        serializer.WriteStore(store, ms);
        return ms.ToArray();
    }

    public static T[] Flatten<T>(T[,] array)
    {
        int width = array.GetLength(0);
        int height = array.GetLength(1);
        var flat = new T[width * height];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                flat[y * width + x] = array[x, y];

        return flat;
    }

    public static T[,] Restore<T>(T[] flat, int width, int height)
    {
        var array = new T[width, height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                array[x, y] = flat[y * width + x];
        return array;
    }
}
