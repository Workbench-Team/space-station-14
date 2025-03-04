using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Systems;
using Content.Server.Starshine.Shuttles.Components;
using Content.Server.Station.Components;
using Content.Server.Station.Events;
using Content.Server.Station.Systems;
using Robust.Shared.EntitySerialization.Systems;
using TimedDespawnComponent = Robust.Shared.Spawners.TimedDespawnComponent;

namespace Content.Server.Starshine.Shuttles.Systems;

/// <summary>
/// If enabled spawns players on a separate arrivals station before they can transfer to the main station.
/// </summary>
public sealed class StationShuttleDock : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly MapLoaderSystem _loader = default!;
    [Dependency] private readonly ShuttleSystem _shuttles = default!;
    [Dependency] private readonly StationSystem _station = default!;

    /// <summary>
    ///     The first arrival is a little early, to save everyone 10s
    /// </summary>
    private const float RoundStartFTLDuration = 10f;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StationShuttleDockComponent, StationPostInitEvent>(OnStationPostInit);
    }

    private bool TryGetStation(out EntityUid uid)
    {
        var stationsQuery = EntityQueryEnumerator<BecomesStationComponent>();

        while (stationsQuery.MoveNext(out uid, out _))
        {
            return true;
        }

        return false;
    }

    private void OnStationPostInit(EntityUid uid, StationShuttleDockComponent component, ref StationPostInitEvent args)
    {
        SetupShuttle(uid, component);
    }

    private void SetupShuttle(EntityUid uid, StationShuttleDockComponent component)
    {
        if (!Deleted(component.Shuttle))
            return;

        // Spawn shuttle on a dummy map then dock it to the source.
        var dummpMapEntity = _mapSystem.CreateMap(out var dummyMapId);

        if (TryGetStation(out var station) &&
            _loader.TryLoadGrid(dummyMapId, component.Path, out var shuttle))
        {
            component.Shuttle = shuttle.Value;
            var shuttleComp = Comp<ShuttleComponent>(component.Shuttle);
            _shuttles.FTLToDock(component.Shuttle, shuttleComp, station, hyperspaceTime: RoundStartFTLDuration, priorityTag: component.TargetTag);
            _station.AddGridToStation(uid, component.Shuttle);
        }

        // Don't start the arrivals shuttle immediately docked so power has a time to stabilise?
        var timer = AddComp<TimedDespawnComponent>(dummpMapEntity);
        timer.Lifetime = 15f;
    }
}
