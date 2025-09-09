using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Map;
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
    private readonly SystemRoot _renderSystems;

    public Game(string? name)
    {
        Name = string.IsNullOrWhiteSpace(name) ? "world" : name;
        World = CreateStore();
        MapRepository = new(World);

        _updateSystems = new SystemRoot(World)
        {
            new Systems.Update.Turn.EnergySystem(),
            new Systems.Update.Turn.HealthRegenSystem(),
            new Systems.Update.Turn.FinalTurnSystem(),
            new Systems.Update.EntityTurnSystem(),
        };

        _renderSystems = new SystemRoot(World)
        {
            //new EntityRenderSystem(),
        };

        if (File.Exists($"Runs/{Name}")) WetwareSerializer.DeserializeState(this);

        Instance = this;
    }

    private EntityStore CreateStore()
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

    public void Render()
    {
        _renderSystems.Update(GetTick());
    }
}
