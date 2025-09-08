using FluentAssertions;
using Friflo.Engine.ECS;
using Position = Wetware.Components.Position;
using Wetware.Flags;

namespace Wetware.Tests.GameSystem;

public class MapTests
{
    private Wetware.Map.Map? _map;
    private EntityStore? _store;

    [SetUp]
    public void Setup()
    {
        _store = new EntityStore();
        _map = new(20, 20, _store);
    }

    [Test]
    public void Tiles_StartWithNoFlags()
    {
        foreach (var flag in _map!._tiles)
        {
            flag.Should().Be(Map.TileFlag.None);
        }
    }

    [Test]
    public void Tiles_AddingBlocksMovementEntity_SetsFlag()
    {
        _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksMovement>());

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeTrue();

        _map!.Has(10, 10, Map.TileFlag.BlocksItems)
          .Should()
          .BeFalse();

        _map!.Has(10, 10, Map.TileFlag.BlocksVision)
          .Should()
          .BeFalse();
    }

    [Test]
    public void Tiles_AddingFlaggedEntity_OnlySetsCorrectTile()
    {
        _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksMovement>());

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeTrue();

        _map!.Has(9, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeFalse();
    }

    [Test]
    public void Tiles_DeletingEntity_ClearsFlags()
    {
        var e = _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksMovement>());

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeTrue();

        e.DeleteEntity();

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeFalse();
    }

    [Test]
    public void Tiles_RemovingPosition_ClearsFlags()
    {
        var e = _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksMovement>());

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeTrue();

        e.RemoveComponent<Position>();

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeFalse();
    }

    [Test]
    public void Tiles_ChangingPosition_ClearsFlags()
    {
        var e = _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksMovement>());

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeTrue();

        e.AddComponent<Position>(new Position(11, 11));

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement)
          .Should()
          .BeFalse();

        _map!.Has(11, 11, Map.TileFlag.BlocksMovement)
          .Should()
          .BeTrue();
    }

    [Test]
    public void Tiles_RemovingOneEntity_RetainsOtherFlags()
    {
        var e = _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksMovement>());
        var sightBlocker = _store.CreateEntity(new Position(10, 10), Tags.Get<BlocksVision>());

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement).Should().BeTrue();
        _map!.Has(10, 10, Map.TileFlag.BlocksVision).Should().BeTrue();

        e.DeleteEntity();

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement).Should().BeFalse();
        _map!.Has(10, 10, Map.TileFlag.BlocksVision).Should().BeTrue();
    }

    [Test]
    public void Has_MultipleFlags_Handles()
    {
        var e = _store!.CreateEntity();
        e.AddTag<BlocksMovement>();
        e.AddTag<BlocksVision>();
        e.AddComponent<Position>(new Position(10, 10));

        _map!.Has(10, 10, Map.TileFlag.BlocksMovement).Should().BeTrue();
        _map!.Has(10, 10, Map.TileFlag.BlocksVision).Should().BeTrue();
        _map!.Has(10, 10, Map.TileFlag.BlocksMovement | Map.TileFlag.BlocksVision).Should().BeTrue();
        _map!.Has(10, 10, Map.TileFlag.BlocksMovement & Map.TileFlag.BlocksItems).Should().BeFalse();
    }
}
