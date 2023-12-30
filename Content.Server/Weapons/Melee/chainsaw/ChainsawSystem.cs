using Content.Server.Audio;
using Content.Server.CombatMode.Disarm;
using Content.Server.Kitchen.Components;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Temperature;
using Content.Shared.Toggleable;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Server.Weapons.Melee.Chainsaw;

public sealed class ChainsawSystem : EntitySystem
{
    [Dependency] private readonly SharedItemSystem _item = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly AmbientSoundSystem _ambientSound = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChainsawComponent, GetMeleeDamageEvent>(OnGetMeleeDamage);
        SubscribeLocalEvent<ChainsawComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<ChainsawComponent, IsHotEvent>(OnIsHotEvent);
        SubscribeLocalEvent<ChainsawComponent, ChainsawOffEvent>(TurnOff);
        SubscribeLocalEvent<ChainsawComponent, ChainsawOnEvent>(TurnOn);

        SubscribeLocalEvent<ChainsawComponent, ItemUnwieldedEvent>(TurnOffonUnwielded);
        SubscribeLocalEvent<ChainsawComponent, ItemWieldedEvent>(TurnOnonWielded);

    }
    private void OnGetMeleeDamage(EntityUid uid, ChainsawComponent comp, ref GetMeleeDamageEvent args)
    {
        if (!comp.On)
            return;

        args.Damage = comp.LitDamageBonus;
    }

    private void OnUseInHand(EntityUid uid, ChainsawComponent comp, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (TryComp<WieldableComponent>(uid, out var wieldableComp))
            return;

        if (comp.On)
        {
            var ev = new ChainsawOffEvent();
            RaiseLocalEvent(uid, ref ev);
        }
        else
        {
            var ev = new ChainsawOnEvent();
            RaiseLocalEvent(uid, ref ev);
        }

        UpdateState(uid, comp);
    }

    private void TurnOffonUnwielded(EntityUid uid, ChainsawComponent comp, ItemUnwieldedEvent args)
    {
        var ev = new ChainsawOffEvent();
        RaiseLocalEvent(uid, ref ev);
        UpdateState(uid, comp);
    }

    private void TurnOnonWielded(EntityUid uid, ChainsawComponent comp, ref ItemWieldedEvent args)
    {
        var ev = new ChainsawOnEvent();
        RaiseLocalEvent(uid, ref ev);
        UpdateState(uid, comp);
    }

    private void TurnOff(EntityUid uid, ChainsawComponent comp, ref ChainsawOffEvent args)
    {
        if (TryComp(uid, out ItemComponent? item))
        {
            _item.SetSize(uid, "Normal", item);
        }

        if (TryComp<DisarmMalusComponent>(uid, out var malus))
        {
            malus.Malus -= comp.LitDisarmMalus;
        }

        if (TryComp<MeleeWeaponComponent>(uid, out var weaponComp))
        {
            weaponComp.HitSound = comp.OnHitOff;
            if (comp.Secret)
                weaponComp.Hidden = true;
        }

        if (comp.IsSharp)
            RemComp<SharpComponent>(uid);

        _audio.PlayEntity(comp.DeActivateSound, Filter.Pvs(uid, entityManager: EntityManager), uid, true, comp.DeActivateSound.Params);

        comp.On = false;
    }

    private void TurnOn(EntityUid uid, ChainsawComponent comp, ref ChainsawOnEvent args)
    {
        if (TryComp(uid, out ItemComponent? item))
        {
            _item.SetSize(uid, "Ginormous", item);
        }

        if (comp.IsSharp)
            EnsureComp<SharpComponent>(uid);

        if (TryComp<MeleeWeaponComponent>(uid, out var weaponComp))
        {
            weaponComp.HitSound = comp.OnHitOn;
            if (comp.Secret)
                weaponComp.Hidden = false;
        }

        if (TryComp<DisarmMalusComponent>(uid, out var malus))
        {
            malus.Malus += comp.LitDisarmMalus;
        }

        _audio.PlayEntity(comp.ActivateSound, Filter.Pvs(uid, entityManager: EntityManager), uid, true, comp.ActivateSound.Params);

        comp.On = true;
    }

    private void UpdateState(EntityUid uid, ChainsawComponent component)
    {
        if (!TryComp(uid, out AppearanceComponent? appearanceComponent))
            return;

        _appearance.SetData(uid, ToggleableLightVisuals.Enabled, component.On, appearanceComponent);
        _ambientSound.SetAmbience(uid, component.On);
    }

    private void OnIsHotEvent(EntityUid uid, ChainsawComponent chainsaw, IsHotEvent args)
    {
        args.IsHot = chainsaw.On;
    }
}
