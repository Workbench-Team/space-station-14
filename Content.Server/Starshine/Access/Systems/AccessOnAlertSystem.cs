using Content.Server.AlertLevel;
using Content.Shared.Access.Systems;
using Content.Shared.AlertLevel;
using Content.Shared.Starshine.Access.Components;
using Robust.Shared.Prototypes;
using System.Linq;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Station;

namespace Content.Server.Starshine.Access.Systems;

public sealed class AccessOnAlertSystem : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly SharedStationSystem _station = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AccessOnAlertComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<AccessOnAlertComponent, ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<AccessOnAlertComponent, AccessOverriderValidateModifyEvent>(OnValidateModify);
        SubscribeLocalEvent<AlertLevelChangedEvent>(OnAlertLevelChanged);
    }

    private void OnMapInit(EntityUid uid, AccessOnAlertComponent component, MapInitEvent args)
    {
        var stationUid = _station.GetOwningStation(uid);

        if (!TryComp<AlertLevelComponent>(stationUid, out var alertLevel))
            return;

        UpdateAccessForAlertLevel(uid, component, alertLevel.CurrentLevel);
    }

    private void OnComponentShutdown(EntityUid uid, AccessOnAlertComponent component, ComponentShutdown args)
    {
        TryRemoveAlertAccesses(uid, component);
    }

    private void OnValidateModify(EntityUid uid, AccessOnAlertComponent component, AccessOverriderValidateModifyEvent args)
    {
        if (args.Cancelled || args.TargetReader != uid)
            return;

        foreach (var lockedSet in component.LockedAlertAccesses)
        {
            if (lockedSet.All(access => args.ProposedAccess.Contains(access)))
                continue;

            args.CancelReason = Loc.GetString("access-on-alert-overrider-cancel-reason");
            args.Cancel();
            break;
        }
    }

    private void OnAlertLevelChanged(AlertLevelChangedEvent args)
    {
        var query = EntityQueryEnumerator<AccessOnAlertComponent>();

        while (query.MoveNext(out var uid, out var alertComp))
        {
            UpdateAccessForAlertLevel(uid, alertComp, args.AlertLevel);
        }
    }

    private void UpdateAccessForAlertLevel(EntityUid uid, AccessOnAlertComponent alertComp, string alertLevel)
    {
        if (!_prototype.TryIndex(alertComp.SettingsPrototype, out var settings))
            return;

        if (!_accessReader.GetMainAccessReader(uid, out var mainAccessReader))
            return;

        TryRemoveAlertAccesses(uid, alertComp, mainAccessReader.Value);
        var ev = new AlertAccessUpdatedEvent(uid);

        if (settings.AlertAccessMappings.TryGetValue(alertLevel, out var lockedAccesses))
        {
            alertComp.LockedAlertAccesses = [..lockedAccesses];
        }
        else
        {
            alertComp.LockedAlertAccesses.Clear();
            RaiseLocalEvent(ev);
            return;
        }

        foreach (var access in lockedAccesses)
        {
            var alreadyExists = mainAccessReader.Value.Comp.AccessLists.Any(existing =>
                existing.SetEquals(access));

            if (alreadyExists)
                continue;

            _accessReader.TryAddAccess(mainAccessReader.Value, access);
            alertComp.AddedAlertAccesses.Add(access);
        }

        RaiseLocalEvent(ev);
    }

    public bool TryRemoveAlertAccesses(EntityUid uid, AccessOnAlertComponent alertComp)
    {
        if (alertComp.AddedAlertAccesses.Count == 0)
            return false;

        return _accessReader.GetMainAccessReader(uid, out var mainAccessReader) &&
               TryRemoveAlertAccesses(uid, alertComp, mainAccessReader.Value);
    }

    public bool TryRemoveAlertAccesses(EntityUid uid, AccessOnAlertComponent alertComp, Entity<AccessReaderComponent> mainAccessReader)
    {
        if (alertComp.AddedAlertAccesses.Count == 0)
            return false;

        _accessReader.TryRemoveAccesses(mainAccessReader, alertComp.AddedAlertAccesses);
        alertComp.AddedAlertAccesses.Clear();

        var ev = new AlertAccessUpdatedEvent(uid);
        RaiseLocalEvent(ev);

        return true;
    }
}

public sealed class AlertAccessUpdatedEvent : EntityEventArgs
{
    public EntityUid AccessReaderUid;

    public AlertAccessUpdatedEvent(EntityUid accessReaderUid)
    {
        AccessReaderUid = accessReaderUid;
    }
}
