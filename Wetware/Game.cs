using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Serialize;
using Friflo.Engine.ECS.Systems;
using Wetware.Flags;
using Wetware.Globals;
using Wetware.Map;
using Wetware.Systems;

namespace Wetware;

public class Game
{
    internal static Game Instance = null!;
    public static bool ClockTurn => Instance.World.GetUniqueEntity(EId.Clock).Tags.Has<TakingTurn>();

    private readonly string _name;
    public bool LoadedFromFile;

    public readonly EntityStore World;
    public readonly MapRepository MapRepository;
    private readonly SystemRoot _updateSystems;
    private readonly SystemRoot _renderSystems;

    public Game(string? name)
    {
        _name = string.IsNullOrWhiteSpace(name) ? "world" : name;

        World = InitWorld();
        MapRepository = new();

        _updateSystems = new SystemRoot(World)
        {
            new EnergySystem(),
            new HealthRegenSystem(),
            new DoNothingSystem(),
        };

        _renderSystems = new SystemRoot(World) {
          new EntityRenderSystem(),
        };

        Instance = this;
    }

    private EntityStore InitWorld()
    {
        var store = new EntityStore();
        store.EventRecorder.Enabled = true;

        if (!File.Exists($"Runs/{_name}.json")) return store;

        LoadedFromFile = true;
        return store.LoadFromFile($"Runs/{_name}.json");
    }

    private static UpdateTick GetTick() => new();

    public void Tick()
    {
        Console.WriteLine("Ticking UpdateSystems.");
        _updateSystems.Update(GetTick());

        var serializer = new EntitySerializer();
        var writeStream = new FileStream($"Runs/{_name}.json", FileMode.Create);
        serializer.WriteStore(World, writeStream);
        writeStream.Close();
    }

    public void Render()
    {
        _renderSystems.Update(GetTick());
    }
}
