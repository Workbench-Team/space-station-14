using System.Linq;
using Content.Server.DoAfter;
using Content.Server.Interaction;
using Content.Server.Mech.Components;
using Content.Server.Mech.Equipment.Components;
using Content.Server.Mech.Systems;
using Content.Shared.Interaction;
using Content.Shared.Mech;
using Content.Shared.Mech.Equipment.Components;
using Content.Shared.Wall;
using Robust.Shared.Containers;
using Robust.Shared.Map;

namespace Content.Server.Mech.Equipment.EntitySystems;

/// <summary>
/// Handles <see cref="MechDrillComponent"/> and all related UI logic
/// </summary>
public sealed class MechDrillSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly MechSystem _mech = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly InteractionSystem _interaction = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<MechDrillComponent, MechEquipmentRemovedEvent>(OnEquipmentRemoved);

        SubscribeLocalEvent<MechDrillComponent, InteractNoHandEvent>(OnInteract);
        // SubscribeLocalEvent<MechDrillComponent, MechDrillDrillbFinishedEvent>(OnDrillFinished);
        // SubscribeLocalEvent<MechDrillComponent, MechDrillDrillCancelledEvent>(OnDrillCancelled);
    }

    private void OnEquipmentRemoved(EntityUid uid, MechDrillComponent component, ref MechEquipmentRemovedEvent args)
    {
        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) ||
            equipmentComponent.EquipmentOwner == null)
            return;
    }

    private void OnInteract(EntityUid uid, MechDrillComponent component, InteractNoHandEvent args)
    {
        if (args.Handled || args.Target == null)
            return;

        if (!TryComp<MechComponent>(args.User, out var mech))
            return;

        if (mech.Energy + component.DrillEnergyDelta < 0)
            return;

        if (component.Token != null)
            return;

        if (!_interaction.InRangeUnobstructed(args.User, args.Target.Value))
            return;

        args.Handled = true;
        component.Token = new();
        // component.AudioStream = _audio.PlayPvs(component.GrabSound, uid);
        if (!TryComp<MechDrillSystem>(args.Used, out var tool) ||
            component.ToolWhitelist?.IsValid(args.Used) == false ||
            tool.GatheringEntities.TryGetValue(uid, out var cancelToken))
            return;

        // Can't gather too many entities at once.
        if (tool.MaxGatheringEntities < tool.GatheringEntities.Count + 1)
            return;

        // cancelToken = new CancellationTokenSource();
        tool.GatheringEntities[uid] = cancelToken;

        var doAfter = new DoAfterEventArgs(args.User, tool.GatheringTime, cancelToken.Token, uid)
        {
            BreakOnDamage = true,
            BreakOnStun = true,
            BreakOnTargetMove = true,
            BreakOnUserMove = true,
            MovementThreshold = 0.25f,
            // BroadcastCancelledEvent = new GatheringDoafterCancel { Tool = args.Used, Resource = uid },
            // TargetFinishedEvent = new GatheringDoafterSuccess { Tool = args.Used, Resource = uid, Player = args.User }
        };

        _doAfter.DoAfter(doAfter);
    }

    private void OnDrillFinished(EntityUid uid, MechDrillComponent component, MechGrabberGrabFinishedEvent args)
    {
        component.Token = null;

        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) || equipmentComponent.EquipmentOwner == null)
            return;
        if (!_mech.TryChangeEnergy(equipmentComponent.EquipmentOwner.Value, component.DrillEnergyDelta))
            return;

        _mech.UpdateUserInterface(equipmentComponent.EquipmentOwner.Value);
    }

    private void OnDrillCancelled(EntityUid uid, MechDrillComponent component, MechGrabberGrabCancelledEvent args)
    {
        component.AudioStream?.Stop();
        component.Token = null;
    }
}
