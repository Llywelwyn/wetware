using Friflo.Engine.ECS;
using Wetware;
using Wetware.Components;
using Wetware.Flags;
using Wetware.Globals;

Console.WriteLine("a name?");
var name = Console.ReadLine();

var game = new Game(name);

if (!game.LoadedFromFile) MakeTestEntities();

for (;;)
{
    Game.Instance.Tick();
    Console.ReadKey(true);
}

void MakeTestEntities()
{
    game.World.CreateEntity(new UniqueEntity(EId.Clock), new Name(EId.Clock), new Energy(0), new Speed(12), Tags.Get<Clock>());
    game.World.CreateEntity(new Name("reflex machine"), new Energy(0), new Speed(12));
    game.World.CreateEntity(new Name("speed machine"), new Energy(0), new Speed(16));
    game.World.CreateEntity(new Name("slow machine"), new Energy(0), new Speed(6), new Health(20, 10));
}