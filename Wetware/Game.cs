using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using MessagePack;
using Wetware.Globals;
using Wetware.Maps;
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
    private readonly SystemRoot m_updateSystems;

    public readonly ScreenManager ScreenManager;

    public static Game Load(string? name)
    {
        name ??= "world";
        string path = $"Runs/{name}";

        if (!File.Exists(path))
            return new Game(name);

        var state = MessagePackSerializer.Deserialize<SaveState>(File.ReadAllBytes(path));
        return new Game(state);
    }

    public Game(SaveState state)
    {
        Instance = this;
        Name = state.Name;
        World = WetwareSerializer.DeserializeEntityStore(state.World);
        m_updateSystems = CreateSystemRoot(World);
        MapRepository = Serializer.Mappers.MapRepositoryMapper.FromDto(state.MapRepository);
        MapRepository.OnMapChanged += OnMapChanged;
        ScreenManager = new(MapRepository);
        MapRepository.Initialise();

        TurnQueue.LoadSnapshot(state.TurnQueue);
        ClockTurn = state.ClockTurn;
        LoadedFromFile = true;
    }

    public Game(string? name)
    {
        Instance = this;
        Name = string.IsNullOrWhiteSpace(name) ? "world" : name;
        World = CreateStore();
        m_updateSystems = CreateSystemRoot(World);
        MapRepository = new();
        MapRepository.OnMapChanged += OnMapChanged;
        ScreenManager = new(MapRepository);
        MapRepository.Initialise();
    }

    private static SystemRoot CreateSystemRoot(EntityStore store)
    {
        return new SystemRoot(store)
        {
            new Systems.Update.Turn.EnergySystem(),
            new Systems.Update.Turn.HealthRegenSystem(),
            new Systems.Update.Turn.FinalTurnSystem(),
            new Systems.Update.EntityTurnSystem(),
        };
    }

    private static EntityStore CreateStore()
    {
        var store = new EntityStore();
        store.EventRecorder.Enabled = true;
        return store;
    }

    private void OnMapChanged(Map? oldMap, Map newMap)
    {
        if (oldMap is not null) m_updateSystems.RemoveStore(oldMap.Entities);
        m_updateSystems.AddStore(newMap.Entities);
    }

    private static UpdateTick GetTick() => new();

    public void Tick()
    {
        Console.WriteLine("Ticking UpdateSystems.");
        m_updateSystems.Update(GetTick());
        WetwareSerializer.SerializeState();
    }

    public void Render() => ScreenManager.Render();
}
