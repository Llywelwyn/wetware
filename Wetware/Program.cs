using Friflo.Engine.ECS;
using Wetware;
using Wetware.Components;
using Wetware.Flags;
using Wetware.Globals;
using Wetware.Assets;
using Wetware.Screens;
using Raylib_cs;

Console.WriteLine("a name?");
var name = Console.ReadLine();

Raylib.InitWindow(1920, 1056, "wetware");
var game = new Game(name);

if (!game.LoadedFromFile) MakeTestEntities();

var tick = true;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    if (tick) Game.Instance.Tick();
    Game.Instance.Render();
    Raylib.EndDrawing();
    tick = false;
    if (Raylib.IsKeyPressed(KeyboardKey.Space)) tick = true;
}

void MakeTestEntities()
{
    game.World.CreateEntity(
        new UniqueEntity(EId.Clock),
        new Name(EId.Clock),
        new Energy(0),
        new Speed(100),
        Tags.Get<Clock>());
    game.World.CreateEntity(
        new Name("reflex machine"),
        BrainPresets.Passive(),
        new Renderable(Sprite.m),
        new Energy(0),
        new Wetware.Components.Position(9, 10),
        new Speed(100),
        Tags.Get<BlocksMovement>());
    game.World.CreateEntity(
        new Name("speed machine"),
        BrainPresets.Passive(),
        new Renderable(Sprite.m),
        new Wetware.Components.Position(10, 11),
        new Energy(0),
        new Speed(110),
        Tags.Get<BlocksMovement>());
    game.World.CreateEntity(
        new Name("slow machine"),
        BrainPresets.Passive(),
        new Renderable(Sprite.m),
        new Wetware.Components.Position(5, 16),
        new Energy(0),
        new Speed(60),
        new Health(20, 10),
        Tags.Get<BlocksMovement>());
}
