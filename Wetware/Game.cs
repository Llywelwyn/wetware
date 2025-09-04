using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Flags;
using Wetware.Globals;
using Wetware.Systems;

namespace Wetware;

public class Game
{
    internal static Game Instance = null!;
    public static bool ClockTurn => Instance.World.GetUniqueEntity(EId.Clock).Tags.Has<TakingTurn>();
    
    public readonly EntityStore World;
    private readonly SystemRoot _updateSystems;
    private readonly SystemRoot _renderSystems;

    public Game()
    {
        Instance = this;
        World = NewWorld();
        _updateSystems = new SystemRoot(World)
        {
            new EnergySystem(),
            new DoNothingSystem(),
        };
        
        _renderSystems = new SystemRoot(World);
    }

    private static EntityStore NewWorld() => new() { EventRecorder = { Enabled = true } };

    private static UpdateTick GetTick() => new UpdateTick();
    
    public void Tick()
    {
        Console.WriteLine("Ticking UpdateSystems.");
        _updateSystems.Update(GetTick());
    }

    public void Render()
    {
        _renderSystems.Update(GetTick());
    }
}