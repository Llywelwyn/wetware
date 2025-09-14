using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Map;
using Wetware.Screens;
using Wetware.Serializer;

namespace Wetware;

public class Game
{
    internal static Game Instance = null!;

    public readonly string Name;
    public bool LoadedFromFile;

    public bool ClockTurn = true;
    public readonly EntityStore World;
    public readonly MapRepository MapRepository;
    private readonly SystemRoot _updateSystems;

    public readonly ScreenManager ScreenManager;

    public Game(string? name)
    {
        Instance = this;
        Name = string.IsNullOrWhiteSpace(name) ? "world" : name;
        World = CreateStore();
        MapRepository = new(World);
        ScreenManager = new();

        _updateSystems = new SystemRoot(World)
        {
            new Systems.Update.Turn.EnergySystem(),
            new Systems.Update.Turn.HealthRegenSystem(),
            new Systems.Update.Turn.FinalTurnSystem(),
            new Systems.Update.EntityTurnSystem(),
        };

        if (File.Exists($"Runs/{Name}")) WetwareSerializer.DeserializeState(this);

    }

    private static EntityStore CreateStore()
    {
        var store = new EntityStore();
        store.EventRecorder.Enabled = true;
        return store;
    }

    private static UpdateTick GetTick() => new();

    public void Tick()
    {
        Console.WriteLine("Ticking UpdateSystems.");
        _updateSystems.Update(GetTick());
        WetwareSerializer.SerializeState();
    }

    public void Render() => ScreenManager.Render();
}
