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
using Robust.Shared.Player;

namespace Content.Server.Weapons.Melee.Chainsaw;

public sealed class ChainsawSystem : EntitySystem
{
    [Dependency] private readonly SharedItemSystem _item = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChainsawComponent, GetMeleeDamageEvent>(OnGetMeleeDamage);
        SubscribeLocalEvent<ChainsawComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<ChainsawComponent, IsHotEvent>(OnIsHotEvent);
        SubscribeLocalEvent<ChainsawComponent, ChainsawDeactivatedEvent>(TurnOff);
        SubscribeLocalEvent<ChainsawComponent, ChainsawActivatedEvent>(TurnOn);

        SubscribeLocalEvent<ChainsawComponent, ItemUnwieldedEvent>(TurnOffonUnwielded);
        SubscribeLocalEvent<ChainsawComponent, ItemWieldedEvent>(TurnOnonWielded);
    }
    private void OnGetMeleeDamage(EntityUid uid, ChainsawComponent comp, ref GetMeleeDamageEvent args)
    {
        if (!comp.Activated)
            return;

        args.Damage += comp.LitDamageBonus;
    }

    private void OnUseInHand(EntityUid uid, ChainsawComponent comp, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (TryComp<WieldableComponent>(uid, out var wieldableComp))
            return;

        if (comp.Activated)
        {
            var ev = new ChainsawDeactivatedEvent();
            RaiseLocalEvent(uid, ref ev);
        }
        else
        {
            var ev = new ChainsawActivatedEvent();
            RaiseLocalEvent(uid, ref ev);
        }

        UpdateAppearance(uid, comp);
    }

    private void TurnOffonUnwielded(EntityUid uid, ChainsawComponent comp, ItemUnwieldedEvent args)
    {
        var ev = new ChainsawDeactivatedEvent();
        RaiseLocalEvent(uid, ref ev);
        UpdateAppearance(uid, comp);
    }

    private void TurnOnonWielded(EntityUid uid, ChainsawComponent comp, ref ItemWieldedEvent args)
    {
        var ev = new ChainsawActivatedEvent();
        RaiseLocalEvent(uid, ref ev);
        UpdateAppearance(uid, comp);
    }

    private void TurnOff(EntityUid uid, ChainsawComponent comp, ref ChainsawDeactivatedEvent args)
    {
        if (TryComp(uid, out ItemComponent? item))
        {
            _item.SetSize(uid, 30, item);
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

        _audio.Play(comp.DeActivateSound, Filter.Pvs(uid, entityManager: EntityManager), uid, true, comp.DeActivateSound.Params);

        comp.Activated = false;
    }

    private void TurnOn(EntityUid uid, ChainsawComponent comp, ref ChainsawActivatedEvent args)
    {
        if (TryComp(uid, out ItemComponent? item))
        {
            _item.SetSize(uid, 9999, item);
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

        _audio.Play(comp.ActivateSound, Filter.Pvs(uid, entityManager: EntityManager), uid, true, comp.ActivateSound.Params);

        comp.Activated = true;
    }

    private void UpdateAppearance(EntityUid uid, ChainsawComponent component)
    {
        if (!TryComp(uid, out AppearanceComponent? appearanceComponent))
            return;

    }

    private void OnIsHotEvent(EntityUid uid, ChainsawComponent Chainsaw, IsHotEvent args)
    {
        args.IsHot = Chainsaw.Activated;
    }
}
