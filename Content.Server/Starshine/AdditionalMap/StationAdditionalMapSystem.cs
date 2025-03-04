/*
 * All right reserved to CrystallEdge.
 *
 * BUT this file is sublicensed under MIT License
 *
 * BY Ed, discord: eshhhed, github: TheShuEd.
 */

using Content.Server.Station.Components;
using Content.Server.Station.Events;
using Robust.Server.GameObjects;
using Robust.Shared.EntitySerialization;
using Robust.Shared.EntitySerialization.Systems;

namespace Content.Server.Starshine.AdditionalMap;

public sealed class StationAdditionalMapSystem : EntitySystem
{
    [Dependency] private readonly MapSystem _map = default!;
    [Dependency] private readonly MapLoaderSystem _mapLoader = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StationAdditionalMapComponent, StationPostInitEvent>(OnStationPostInit);
    }

    private void OnStationPostInit(Entity<StationAdditionalMapComponent> addMap, ref StationPostInitEvent args)
    {
        if (!HasComp<StationDataComponent>(addMap))
            return;

        foreach (var path in addMap.Comp.MapPaths)
        {
            var options = DeserializationOptions.Default with { InitializeMaps = true };

            if (_mapLoader.TryLoadMap(path, out var mapUid, out _, options))
                continue;

            Log.Error($"Failed to load map from {path}!");
            Del(mapUid);
            return;
        }
    }
}
