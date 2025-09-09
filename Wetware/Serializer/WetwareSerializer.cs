using Friflo.Engine.ECS.Serialize;
using Wetware.Globals;
using MessagePack;
using Wetware.Config;
using System.Text.Json;

namespace Wetware.Serializer;

public static class WetwareSerializer
{
    public static void DeserializeState(Game game)
    {
        byte[] bytes = File.ReadAllBytes($"Runs/{game.Name}");
        var state = MessagePackSerializer.Deserialize<SaveState>(bytes);

        // Deserialize World
        var es = new EntitySerializer();
        using var ms = new MemoryStream(state.World);
        es.ReadIntoStore(game.World, ms);

        // Deserialize TurnQueue
        TurnQueue.LoadSnapshot(state.TurnQueue);

        // Deserialize ClockTurn
        game.ClockTurn = state.ClockTurn;

        game.LoadedFromFile = true;
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
            World = SerializeEntityStore(),
            TurnQueue = TurnQueue.GetSnapshot().ToList(),
            ClockTurn = Game.Instance.ClockTurn,
        };
    }

    public static byte[] SerializeEntityStore()
    {
        var snapshot = Game.Instance.World;
        var serializer = new EntitySerializer();
        using var ms = new MemoryStream();
        serializer.WriteStore(snapshot, ms);
        return ms.ToArray();
    }
}
