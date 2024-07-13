using Content.Shared.Starshine.Eye.NightVision.Components;
using Content.Shared.Inventory;
using Content.Shared.Actions;
using JetBrains.Annotations;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Shared.Starshine.Eye.NightVision.Systems;

public sealed class NightVisionSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        base.Initialize();

        if(_net.IsServer)
            SubscribeLocalEvent<NightVisionComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<NightVisionComponent, NvInstantActionEvent>(OnActionToggle);
    }

    [ValidatePrototypeId<EntityPrototype>]
    private const string SwitchNightVisionAction = "SwitchNightVision";

    private void OnComponentStartup(EntityUid uid, NightVisionComponent component, ComponentStartup args)
    {
        if (component.IsToggle)
            _actionsSystem.AddAction(uid, ref component.ActionContainer, SwitchNightVisionAction);
        _actionsSystem.SetCooldown(component.ActionContainer, TimeSpan.FromSeconds(1)); // GCD?
    }

    private void OnActionToggle(EntityUid uid, NightVisionComponent component, NvInstantActionEvent args)
    {
        component.IsOn = !component.IsOn;
        var changeEv = new NightVisionChangedEvent(component.IsOn);
        RaiseLocalEvent(uid, ref changeEv);
        Dirty(uid, component);
        if (component is not { IsOn: true, PlaySoundOn: true })
            return;
        if(_net.IsServer)
            _audioSystem.PlayPvs(component.OffSound, uid);
    }

    [PublicAPI]
    public void UpdateIsNightVision(EntityUid uid, NightVisionComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        var old = component.IsOn;


        var ev = new CanVisionAttemptEvent();
        RaiseLocalEvent(uid, ev);
        component.IsOn = ev.NightVision;

        if (old == component.IsOn)
            return;

        var changeEv = new NightVisionChangedEvent(component.IsOn);
        RaiseLocalEvent(uid, ref changeEv);
        Dirty(uid, component);
    }
}

[ByRefEvent]
public record struct NightVisionChangedEvent(bool IsOn);


public sealed class CanVisionAttemptEvent : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public bool NightVision => Cancelled;
    public SlotFlags TargetSlots => SlotFlags.EYES | SlotFlags.MASK | SlotFlags.HEAD;
}
