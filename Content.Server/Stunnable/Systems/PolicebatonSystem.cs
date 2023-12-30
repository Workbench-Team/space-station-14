using Content.Server.Power.Components;
using Content.Server.Power.Events;
using Content.Server.Stunnable.Components;
using Content.Shared.Audio;
using Content.Shared.Damage.Events;
using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Popups;
using Content.Shared.Stunnable;
using Content.Shared.Toggleable;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Audio.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Player;

namespace Content.Server.Stunnable.Systems
{
    public sealed class PolicebatonSystem : EntitySystem
    {
        [Dependency] private readonly SharedItemSystem _item = default!;
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<PolicebatonComponent, UseInHandEvent>(OnUseInHand);
            SubscribeLocalEvent<PolicebatonComponent, ExaminedEvent>(OnExamined);
            SubscribeLocalEvent<PolicebatonComponent, StaminaDamageOnHitAttemptEvent>(OnStaminaHitAttempt);
            SubscribeLocalEvent<PolicebatonComponent, MeleeHitEvent>(OnMeleeHit);
        }

        private void OnMeleeHit(EntityUid uid, PolicebatonComponent component, MeleeHitEvent args)
        {
            if (!component.Activated && component.Telescopic) return;
        }

        private void OnStaminaHitAttempt(EntityUid uid, PolicebatonComponent component, ref StaminaDamageOnHitAttemptEvent args)
        {
            if (!component.Activated && component.Telescopic)
            {
                args.Cancelled = true;
                return;
            }
        }

        private void OnUseInHand(EntityUid uid, PolicebatonComponent comp, UseInHandEvent args)
        {
            if(!comp.Telescopic) return;

            if (comp.Activated)
            {
               TurnOff(uid, comp);
            }
            else
            {
               TurnOn(uid, comp, args.User);
            }
        }

        private void OnExamined(EntityUid uid, PolicebatonComponent comp, ExaminedEvent args)
        {
        	if(comp.Telescopic)
        	{
        	   var msg = comp.Activated ? Loc.GetString("comp-policebaton-telescopic-examined-on") : Loc.GetString("comp-policebaton-telescopic-examined-off");
        	   args.PushMarkup(msg);
        	}
        }

        private void TurnOff(EntityUid uid, PolicebatonComponent comp)
        {
            if (!comp.Activated || !comp.Telescopic)
                return;

            if (TryComp<AppearanceComponent>(uid, out var appearance) &&
                TryComp<ItemComponent>(uid, out var item))
            {
                _item.SetHeldPrefix(uid, "off", item);
                _appearance.SetData(uid, ToggleVisuals.Toggled, false, appearance);
            }

            _audio.PlayPvs(comp.ToggleSound, uid, AudioHelpers.WithVariation(0.25f));

            comp.Activated = false;
            Dirty(uid, comp);
        }

        private void TurnOn(EntityUid uid, PolicebatonComponent comp, EntityUid user)
        {
            if (comp.Activated || !comp.Telescopic)
                return;

            if (EntityManager.TryGetComponent<AppearanceComponent>(uid, out var appearance) &&
                EntityManager.TryGetComponent<ItemComponent>(uid, out var item))
            {
                _item.SetHeldPrefix(uid, "on", item);
                _appearance.SetData(uid, ToggleVisuals.Toggled, true, appearance);
            }

            _audio.PlayPvs(comp.ToggleSound, uid, AudioHelpers.WithVariation(0.25f));
            comp.Activated = true;
            Dirty(uid, comp);
        }
    }
}
