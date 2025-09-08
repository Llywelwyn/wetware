using Friflo.Engine.ECS;
using Wetware;
using Wetware.Components;
using Wetware.Flags;
using Wetware.Globals;
using Wetware.Assets;

Console.WriteLine("a name?");
var name = Console.ReadLine();

var game = new Game(name);

if (!game.LoadedFromFile) MakeTestEntities();

for (; ; )
{
    Game.Instance.Tick();
    Game.Instance.Render();
    Console.ReadKey(true);
}

void MakeTestEntities()
{
    game.World.CreateEntity(
        new UniqueEntity(EId.Clock),
        new Name(EId.Clock),
        new Energy(0),
        new Speed(12),
        Tags.Get<Clock>());
    game.World.CreateEntity(
        new Name("reflex machine"),
        new Renderable(Sprite.m),
        new Energy(0),
        new Wetware.Components.Position(9, 10),
        new Speed(12),
        Tags.Get<SeenByPov>());
    game.World.CreateEntity(
        new Name("speed machine"),
        new Renderable(Sprite.m),
        new Wetware.Components.Position(10, 11),
        new Energy(0),
        new Speed(16),
        Tags.Get<SeenByPov>());
    game.World.CreateEntity(
        new Name("slow machine"),
        new Renderable(Sprite.m),
        new Wetware.Components.Position(5, 19),
        new Energy(0),
        new Speed(6),
        new Health(20, 10),
        Tags.Get<SeenByPov, BlocksMovement>());
}
