using System.Threading;
using Content.Server.Destructible;
using Content.Server.Gatherable.Components;
using Content.Shared.Destructible;
using Content.Shared.EntityList;
using Content.Server.Interaction;
using Content.Server.Mech.Components;
using Content.Server.Mech.Equipment.Components;
using Content.Server.Mech.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Mech.Components;
using Content.Shared.Mech.Equipment.Components;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Mech.Equipment.EntitySystems;

/// <summary>
/// Handles <see cref="MechDrillComponent"/> and all related UI logic
/// </summary>
public sealed class MechDrillSystem : EntitySystem
{

    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly MechSystem _mech = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly InteractionSystem _interaction = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly DestructibleSystem _destructible = default!;
    [Dependency] private readonly TagSystem _tagSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<MechDrillComponent, InteractNoHandEvent>(OnInteract);
        SubscribeLocalEvent<MechDrillComponent, MechDrillDoAfterEvent>(OnDoAfter);
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

        var damageRequired = _destructible.DestroyedAt(args.Target.Value);
        var damageTime = (damageRequired / gatheringTool.Damage.Total).Float();
        damageTime = Math.Max(1f, damageTime);

        var doAfter = new DoAfterArgs(args.User, damageTime, new MechDrillDoAfterEvent(), uid, target: component.Owner, used: uid)
        {
            BreakOnDamage = true,
            BreakOnTargetMove = true,
            BreakOnUserMove = true,
            MovementThreshold = 0.25f
        };

        _doAfter.TryStartDoAfter(doAfter);
    }

    private void OnDoAfter(EntityUid uid, MechDrillComponent component, DoAfterEvent args)
    {
        component.Token = null;

        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) || equipmentComponent.EquipmentOwner == null)
            return;

        if (!_mech.TryChangeEnergy(equipmentComponent.EquipmentOwner.Value, component.DrillEnergyDelta))
            return;

        //FIXME RaiseLocalEvent(args.Args.Target, new GatherableDoAfterEvent(), true);
        _mech.UpdateUserInterface(equipmentComponent.EquipmentOwner.Value);
    }
}
