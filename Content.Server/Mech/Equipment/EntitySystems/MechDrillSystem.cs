using System.Threading;
using Content.Server.DoAfter;
using Content.Server.Gatherable.Components;
using Content.Server.Interaction;
using Content.Server.Mech.Components;
using Content.Server.Mech.Equipment.Components;
using Content.Server.Mech.Systems;
using Content.Shared.Interaction;
using Content.Shared.Mech.Equipment.Components;
using static Content.Server.Gatherable.GatherableSystem;

namespace Content.Server.Mech.Equipment.EntitySystems;

/// <summary>
/// Handles <see cref="MechDrillComponent"/> and all related UI logic
/// </summary>
public sealed class MechDrillSystem : EntitySystem
{
    [Dependency] private readonly MechSystem _mech = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly InteractionSystem _interaction = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<MechDrillComponent, InteractNoHandEvent>(OnInteract);
        SubscribeLocalEvent<MechDrillComponent, DrillDoafterSuccess>(OnDrillFinished);
        SubscribeLocalEvent<MechDrillComponent, DrillDoafterCancel>(OnDrillCancelled);
    }

    /// <summary>
    /// When mecha driver uses the tool
    /// </summary>
    private void OnInteract(EntityUid uid, MechDrillComponent component, InteractNoHandEvent args)
    {
        if (args.Handled || args.Target == null)
            return;

        if (!TryComp<MechComponent>(args.User, out var mech))
            return;

        if (!TryComp<GatheringToolComponent>(uid, out var gatheringTool))
            return;

        if (mech.Energy + component.DrillEnergyDelta < 0)
            return;


        if (!_interaction.InRangeUnobstructed(args.User, args.Target.Value))
            return;
        args.Handled = true;
        component.Token = new();

        var cancelToken = new CancellationTokenSource();
        var doAfter = new DoAfterEventArgs(args.User, gatheringTool.GatheringTime, cancelToken.Token, component.Owner)
        {
            BreakOnDamage = true,
            BreakOnStun = true,
            BreakOnTargetMove = true,
            BreakOnUserMove = true,
            MovementThreshold = 0.25f,
            BroadcastCancelledEvent = new DrillDoafterCancel { Tool = uid, Resource = args.Target.Value },
            TargetFinishedEvent = new DrillDoafterSuccess { Tool = uid, Resource = args.Target.Value, Player = args.User }
        };

        _doAfter.DoAfter(doAfter);
    }

    /// <summary>
    /// When drilling is complete
    /// </summary>
    private void OnDrillFinished(EntityUid uid, MechDrillComponent component, DrillDoafterSuccess args)
    {
        component.Token = null;

        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) || equipmentComponent.EquipmentOwner == null)
            return;
        if (!_mech.TryChangeEnergy(equipmentComponent.EquipmentOwner.Value, component.DrillEnergyDelta))
            return;

        RaiseLocalEvent(args.Resource, new GatheringDoafterSuccess { Tool = uid, Resource = args.Resource, Player = args.Player }, true);
        _mech.UpdateUserInterface(equipmentComponent.EquipmentOwner.Value);
    }

    /// <summary>
    /// When drilling is cancelled
    /// </summary>
    private void OnDrillCancelled(EntityUid uid, MechDrillComponent component, DrillDoafterCancel args)
    {
        component.AudioStream?.Stop();
        component.Token = null;
        RaiseLocalEvent(args.Resource, new GatheringDoafterCancel { Tool = uid, Resource = args.Tool }, true);
    }

    public sealed class DrillDoafterCancel : EntityEventArgs
    {
        public EntityUid Tool;
        public EntityUid Resource;
    }

    private sealed class DrillDoafterSuccess : EntityEventArgs
    {
        public EntityUid Tool;
        public EntityUid Resource;
        public EntityUid Player;
    }
}
