using Friflo.Engine.ECS;
using Wetware;
using Wetware.Components;
using Wetware.Flags;
using Wetware.Globals;

var game = new Game();

game.World.CreateEntity(new UniqueEntity(EId.Clock), new Name(EId.Clock), new Energy(0), new Speed(12), Tags.Get<Clock>());
game.World.CreateEntity(new Name("reflex machine"), new Energy(0), new Speed(12));
game.World.CreateEntity(new Name("speed machine"), new Energy(0), new Speed(16));
game.World.CreateEntity(new Name("slow machine"), new Energy(0), new Speed(6));

for (;;)
{
    Game.Instance.Tick();
    Console.ReadKey(true);
}