using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.AlertLevel;
using Content.Shared.Starshine.Access.Components;
using Robust.Shared.Prototypes;
using System.Linq;

namespace Content.Shared.Starshine.Access.Systems;

public sealed class AccessOnAlertSystem : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AlertLevelChangedEvent>(OnAlertLevelChanged);
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

        if (ShouldSkipAccessReader(mainAccessReader.Value.Comp, settings))
            return;

        if (alertComp.AddedAlertAccesses.Count > 0)
        {
            _accessReader.TryRemoveAccesses(mainAccessReader.Value, alertComp.AddedAlertAccesses);
            alertComp.AddedAlertAccesses.Clear();
        }

        if (settings.AlertAccessMappings.TryGetValue(alertLevel, out var lockedAccesses))
        {
            alertComp.LockedAlertAccesses = [..lockedAccesses];
        }
        else
        {
            alertComp.LockedAlertAccesses.Clear();
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
    }

    private bool ShouldSkipAccessReader(AccessReaderComponent accessReader, AccessOnAlertSettingsPrototype settings)
    {
        var accessListToCheck = accessReader.AccessListsOriginal ?? accessReader.AccessLists;
        return accessListToCheck.Count > 0 &&
               accessListToCheck[0].Any(access => settings.IgnoredAccessLevels.Contains(access));
    }
}
