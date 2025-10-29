using System.Linq;
using Content.Server.Popups;
using Content.Server.Starshine.Access.Systems; // Starshine-AccessOnAlert
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Starshine.Access.Components;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using static Content.Shared.Access.Components.AccessOverriderComponent;

namespace Content.Server.Access.Systems;

[UsedImplicitly]
public sealed class AccessOverriderSystem : SharedAccessOverriderSystem
{
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!;
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedInteractionSystem _interactionSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AccessOverriderComponent, ComponentStartup>(UpdateUserInterface);
        SubscribeLocalEvent<AccessOverriderComponent, EntInsertedIntoContainerMessage>(UpdateUserInterface);
        SubscribeLocalEvent<AccessOverriderComponent, EntRemovedFromContainerMessage>(UpdateUserInterface);
        SubscribeLocalEvent<AccessOverriderComponent, AfterInteractEvent>(AfterInteractOn);
        SubscribeLocalEvent<AccessOverriderComponent, AccessOverriderDoAfterEvent>(OnDoAfter);
        SubscribeLocalEvent<AlertAccessUpdatedEvent>(OnAlertAccessUpdated); // Starshine-AccessOnAlert

        Subs.BuiEvents<AccessOverriderComponent>(AccessOverriderUiKey.Key, subs =>
        {
            subs.Event<BoundUIOpenedEvent>(UpdateUserInterface);
            subs.Event<BoundUIClosedEvent>(OnClose);
            subs.Event<WriteToTargetAccessReaderIdMessage>(OnWriteToTargetAccessReaderIdMessage);
            subs.Event<AlertAccessToggledMessage>(OnAlertAccessToggled); // Starshine-AccessOnAlert
        });
    }

    private void AfterInteractOn(EntityUid uid, AccessOverriderComponent component, AfterInteractEvent args)
    {
        if (args.Target == null || !TryComp(args.Target, out AccessReaderComponent? accessReader))
            return;

        if (!_interactionSystem.InRangeUnobstructed(args.User, (EntityUid) args.Target))
            return;

        var doAfterEventArgs = new DoAfterArgs(EntityManager, args.User, component.DoAfter, new AccessOverriderDoAfterEvent(), uid, target: args.Target, used: uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true,
        };

        _doAfterSystem.TryStartDoAfter(doAfterEventArgs);
    }

    private void OnDoAfter(EntityUid uid, AccessOverriderComponent component, AccessOverriderDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (args.Args.Target != null)
        {
            component.TargetAccessReaderId = args.Args.Target.Value;
            _userInterface.OpenUi(uid, AccessOverriderUiKey.Key, args.User);
            UpdateUserInterface(uid, component, args);
        }

        args.Handled = true;
    }

    private void OnClose(EntityUid uid, AccessOverriderComponent component, BoundUIClosedEvent args)
    {
        if (args.UiKey.Equals(AccessOverriderUiKey.Key))
        {
            component.TargetAccessReaderId = new();
        }
    }

    private void OnWriteToTargetAccessReaderIdMessage(EntityUid uid, AccessOverriderComponent component, WriteToTargetAccessReaderIdMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        TryWriteToTargetAccessReaderId(uid, args.AccessList, player, component);

        UpdateUserInterface(uid, component, args);
    }

    private void UpdateUserInterface(EntityUid uid, AccessOverriderComponent component, EntityEventArgs args)
    {
        if (!component.Initialized)
            return;

        var privilegedIdName = string.Empty;
        var targetLabel = string.Empty; // Starshine-modified
        var targetLabelColor = Color.Red;
        var hasAlertAccess = false; // Starshine-AccessOnAlert

        ProtoId<AccessLevelPrototype>[]? possibleAccess = null;
        ProtoId<AccessLevelPrototype>[]? currentAccess = null;
        ProtoId<AccessLevelPrototype>[]? missingAccess = null;
        ProtoId<AccessLevelPrototype>[]? addedAlertAccess = null; // Starshine-AccessOnAlert
        ProtoId<AccessLevelPrototype>[]? lockedAlertAccess = null; // Starshine-AccessOnAlert

        if (component.TargetAccessReaderId is { Valid: true } accessReader)
        {
            targetLabel = Loc.GetString("access-overrider-window-target-label") + " " + Comp<MetaDataComponent>(component.TargetAccessReaderId).EntityName;
            targetLabelColor = Color.White;

            if (TryComp<AccessOnAlertComponent>(accessReader, out var alertComp)) // Starshine-AccessOnAlert
            {
                hasAlertAccess = true;
                addedAlertAccess = ConvertAccessHashSetsToList(alertComp.AddedAlertAccesses).ToArray();
                lockedAlertAccess = ConvertAccessHashSetsToList(alertComp.LockedAlertAccesses).ToArray();
            }

            if (!_accessReader.GetMainAccessReader(accessReader, out var accessReaderEnt))
                return;

            var currentAccessHashsets = accessReaderEnt.Value.Comp.AccessLists;
            currentAccess = ConvertAccessHashSetsToList(currentAccessHashsets).ToArray();
        }

        if (component.PrivilegedIdSlot.Item is { Valid: true } idCard)
        {
            privilegedIdName = Comp<MetaDataComponent>(idCard).EntityName;

            if (component.TargetAccessReaderId is { Valid: true })
            {
                possibleAccess = _accessReader.FindAccessTags(idCard).ToArray();
            }

            if (currentAccess != null && possibleAccess != null)
            {
                missingAccess = currentAccess.Except(possibleAccess).ToArray();
            }
        }

        AccessOverriderBoundUserInterfaceState newState;

        newState = new AccessOverriderBoundUserInterfaceState(
            component.PrivilegedIdSlot.HasItem,
            PrivilegedIdIsAuthorized(uid, component),
            hasAlertAccess, // Starshine-AccessOnAlert
            HasRequiredAccessForAlertAccess(component, false), // Starshine-AccessOnAlert
            component.AlertAccessRequired.ToArray(), // Starshine-AccessOnAlert
            addedAlertAccess, // Starshine-AccessOnAlert
            lockedAlertAccess, // Starshine-AccessOnAlert
            currentAccess,
            possibleAccess,
            missingAccess,
            privilegedIdName,
            targetLabel,
            targetLabelColor);

        _userInterface.SetUiState(uid, AccessOverriderUiKey.Key, newState);
    }

    private List<ProtoId<AccessLevelPrototype>> ConvertAccessHashSetsToList(List<HashSet<ProtoId<AccessLevelPrototype>>> accessHashsets)
    {
        var accessList = new List<ProtoId<AccessLevelPrototype>>();

        if (accessHashsets.Count <= 0)
            return accessList;

        foreach (var hashSet in accessHashsets)
        {
            accessList.AddRange(hashSet);
        }

        return accessList;
    }

    /// <summary>
    /// Called whenever an access button is pressed, adding or removing that access requirement from the target access reader.
    /// </summary>
    private void TryWriteToTargetAccessReaderId(EntityUid uid,
        List<ProtoId<AccessLevelPrototype>> newAccessList,
        EntityUid player,
        AccessOverriderComponent? component = null)
    {
        if (!Resolve(uid, ref component) || component.TargetAccessReaderId is not { Valid: true })
            return;

        if (!PrivilegedIdIsAuthorized(uid, component))
            return;

        if (IsOutOfRangePopup(player, player, component.TargetAccessReaderId)) // Starshine-modified
            return;

        var validateEv = new AccessOverriderValidateModifyEvent(component.TargetAccessReaderId, newAccessList);
        RaiseLocalEvent(component.TargetAccessReaderId, validateEv);

        if (validateEv.Cancelled)
        {
            _popupSystem.PopupCursor(Loc.GetString("access-overrider-validation-cancelled", ("reason", validateEv.CancelReason ?? "access-overrider-validation-cancelled-reason-unknown")), player);
            _audioSystem.PlayPvs(component.DenialSound, uid);
            return;
        }

        if (newAccessList.Count > 0 && !newAccessList.TrueForAll(x => component.AccessLevels.Contains(x)))
        {
            _sawmill.Warning($"User {ToPrettyString(uid)} tried to write unknown access tag.");
            return;
        }

        if (!_accessReader.GetMainAccessReader(component.TargetAccessReaderId, out var accessReaderEnt))
            return;

        var oldTags = ConvertAccessHashSetsToList(accessReaderEnt.Value.Comp.AccessLists);
        var privilegedId = component.PrivilegedIdSlot.Item;

        if (oldTags.SequenceEqual(newAccessList))
            return;

        var difference = newAccessList.Union(oldTags).Except(newAccessList.Intersect(oldTags)).ToHashSet();
        var privilegedPerms = _accessReader.FindAccessTags(privilegedId!.Value).ToHashSet();

        if (!difference.IsSubsetOf(privilegedPerms))
        {
            _sawmill.Warning($"User {ToPrettyString(uid)} tried to modify permissions they could not give/take!");

            return;
        }

        if (!oldTags.ToHashSet().IsSubsetOf(privilegedPerms))
        {
            _sawmill.Warning($"User {ToPrettyString(uid)} tried to modify permissions when they do not have sufficient access!");
            _popupSystem.PopupEntity(Loc.GetString("access-overrider-cannot-modify-access"), player, player);
            _audioSystem.PlayPvs(component.DenialSound, uid);

            return;
        }

        var addedTags = newAccessList.Except(oldTags).Select(tag => "+" + tag).ToList();
        var removedTags = oldTags.Except(newAccessList).Select(tag => "-" + tag).ToList();

        _adminLogger.Add(LogType.Action, LogImpact.High,
            $"{ToPrettyString(player):player} has modified {ToPrettyString(accessReaderEnt.Value):entity} with the following allowed access level holders: [{string.Join(", ", addedTags.Union(removedTags))}] [{string.Join(", ", newAccessList)}]");

        _accessReader.TrySetAccesses(accessReaderEnt.Value, newAccessList);

        var ev = new OnAccessOverriderAccessUpdatedEvent(player);
        RaiseLocalEvent(component.TargetAccessReaderId, ref ev);
    }

    /// <summary>
    /// Returns true if there is an ID in <see cref="AccessOverriderComponent.PrivilegedIdSlot"/> and said ID satisfies the requirements of <see cref="AccessReaderComponent"/>.
    /// </summary>
    /// <remarks>
    /// Other code relies on the fact this returns false if privileged Id is null. Don't break that invariant.
    /// </remarks>
    private bool PrivilegedIdIsAuthorized(EntityUid uid, AccessOverriderComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return true;

        if (_accessReader.GetMainAccessReader(uid, out var accessReader))
            return true;

        var privilegedId = component.PrivilegedIdSlot.Item;
        return privilegedId != null && _accessReader.IsAllowed(privilegedId.Value, uid, accessReader);
    }

    #region Starshine-AccessOnAlert

    private void OnAlertAccessUpdated(AlertAccessUpdatedEvent args)
    {
        var query = EntityQueryEnumerator<AccessOverriderComponent>();

        while (query.MoveNext(out var overriderUid, out var overriderComp))
        {
            if (overriderComp.TargetAccessReaderId == args.AccessReaderUid)
            {
                UpdateUserInterface(overriderUid, overriderComp, args);
            }
        }
    }

    private void OnAlertAccessToggled(EntityUid uid, AccessOverriderComponent component, AlertAccessToggledMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        TryToggleAlertAccess(uid, args.AlertAccessEnabled, player, component);
        UpdateUserInterface(uid, component, args);
    }

    /// <summary>
    /// Toggles AccessOnAlertComponent on the target access reader based on user privileges
    /// </summary>
    private void TryToggleAlertAccess(EntityUid uid,
        bool alertAccessEnabled,
        EntityUid player,
        AccessOverriderComponent? component = null)
    {
        if (!Resolve(uid, ref component) || component.TargetAccessReaderId is not { Valid: true } targetReader)
            return;

        if (!PrivilegedIdIsAuthorized(uid, component))
            return;

        if (IsOutOfRangePopup(player, player, targetReader))
            return;

        if (!HasRequiredAccessForAlertAccess(component))
        {
            _sawmill.Warning($"User {ToPrettyString(player)} tried to modify Alert Access status on {ToPrettyString(targetReader):entity} when they do not have sufficient access!");
            _popupSystem.PopupCursor(Loc.GetString("access-overrider-insufficient-access-for-alert"), player);
            _audioSystem.PlayPvs(component.DenialSound, uid);
            return;
        }

        if (alertAccessEnabled)
            EnsureComp<AccessOnAlertComponent>(targetReader);
        else
            RemComp<AccessOnAlertComponent>(targetReader);

        _adminLogger.Add(
            LogType.Action,
            LogImpact.Medium,
            $"{ToPrettyString(player):player} has {(alertAccessEnabled ? "enabled" : "disabled")} Alert Access on {ToPrettyString(targetReader):entity}");

        var locString = Loc.GetString("access-overrider-alert-access-modified",
            ("user", player),
            ("target", targetReader));

        _popupSystem.PopupPredicted(locString, player, null);


        _audioSystem.PlayPvs(new SoundPathSpecifier("/Audio/Machines/quickbeep.ogg"), uid);
    }

    /// <summary>
    /// Checks if the user has required access for Alert Access functionality
    /// </summary>
    private bool HasRequiredAccessForAlertAccess(AccessOverriderComponent component, bool requirePrivileges = true)
    {
        var privilegedId = component.PrivilegedIdSlot.Item;
        if (privilegedId == null)
            return false;

        if (!_accessReader.GetMainAccessReader(component.TargetAccessReaderId, out var accessReaderEnt))
            return false;

        var privilegedPerms = _accessReader.FindAccessTags(privilegedId.Value).ToHashSet();

        if (!requirePrivileges)
            return component.AlertAccessRequired.Any(requiredAccess => privilegedPerms.Contains(requiredAccess));
        {
            var oldTags = ConvertAccessHashSetsToList(accessReaderEnt.Value.Comp.AccessLists);
            return oldTags.ToHashSet().IsSubsetOf(privilegedPerms) &&
                   component.AlertAccessRequired.Any(requiredAccess => privilegedPerms.Contains(requiredAccess));
        }
    }

    private bool IsOutOfRangePopup(EntityUid uid, EntityUid recipient, EntityUid targetReader)
    {
        if (_interactionSystem.InRangeUnobstructed(uid, targetReader))
            return false;

        _popupSystem.PopupEntity(Loc.GetString("access-overrider-out-of-range"), uid, recipient);
        return true;
    }

    #endregion

}
