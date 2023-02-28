using Content.Server.Disease.Components;
using Content.Server.Disease;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Actions;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Felinid
{
    /// <summary>
    /// "Lick your or other felinid wounds. Reduce bleeding, but unsanitary and can cause diseases."
    /// </summary>
    public sealed class WoundLickingSystem : EntitySystem
    {
        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly DiseaseSystem _disease = default!;
        [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly BloodstreamSystem _bloodstreamSystem = default!;

        private const string WouldLickingActionPrototype = "WoundLicking";

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<WoundLickingComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<WoundLickingComponent, ComponentRemove>(OnRemove);
            SubscribeLocalEvent<WoundLickingComponent, DoAfterEvent<WoundLickData>>(OnDoAfter);
            SubscribeLocalEvent<WoundLickingTargetActionEvent>(OnActionPerform);
        }

        private void OnInit(EntityUid uid, WoundLickingComponent comp, ComponentInit args)
        {
            var action = new EntityTargetAction(_prototypeManager.Index<EntityTargetActionPrototype>(WouldLickingActionPrototype));
            _actionsSystem.AddAction(uid, action, null);
        }

        private void OnRemove(EntityUid uid, WoundLickingComponent comp, ComponentRemove args)
        {
            var action = new EntityTargetAction(_prototypeManager.Index<EntityTargetActionPrototype>(WouldLickingActionPrototype));
            _actionsSystem.RemoveAction(uid, action);
        }

        private void OnActionPerform(WoundLickingTargetActionEvent ev)
        {
            if (ev.Handled)
                return;

            var performer = ev.Performer;
            var target = ev.Target;

            if (
                !TryComp<WoundLickingComponent>(performer, out var woundLicking) ||
                !TryComp<BloodstreamComponent>(target, out var bloodstream) ||
                !TryComp<MobStateComponent>(target, out var mobState)
            )
            return;

            // Logic
            if (mobState.CurrentState == MobState.Dead) return;

            if (performer == target & !woundLicking.CanSelfApply)
            {
                _popupSystem.PopupEntity(Loc.GetString("lick-wounds-yourself-impossible"),
                    performer, Filter.Entities(performer), true);
                return;
            }

            if (woundLicking.ReagentWhitelist.Any() &&
                !woundLicking.ReagentWhitelist.Contains(bloodstream.BloodReagent)
            )  return;

            if (bloodstream.BleedAmount == 0)
            {
                if (performer == target)
                {
                    _popupSystem.PopupEntity(Loc.GetString("lick-wounds-yourself-no-wounds"),
                        performer, performer);
                    return;
                }
                _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-no-wounds", ("target", target)),
                    performer, performer);
                return;
            }

            // Popup
            var targetIdentity = Identity.Entity(target, EntityManager);
            var performerIdentity = Identity.Entity(performer, EntityManager);
            var otherFilter = Filter.Pvs(performer, entityManager: EntityManager)
                .RemoveWhereAttachedEntity(e => e == performer || e == target);

            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-begin", ("target", targetIdentity)),
                performer, performer);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-target-begin", ("performer", performerIdentity)),
                target, target);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-other-begin", ("performer", performerIdentity), ("target", targetIdentity)),
                performer, otherFilter, true);

            var WoundLickData = new WoundLickData(bloodstream);

            // DoAfter
            var doAfterEventArgs = new DoAfterEventArgs(performer, woundLicking.Delay, target: target)
            {
                BreakOnTargetMove = true,
                BreakOnUserMove = true,
                BreakOnDamage = true,
                BreakOnStun = true
            };

            _doAfterSystem.DoAfter(doAfterEventArgs, WoundLickData);

            ev.Handled = true;
        }

        private void OnDoAfter(EntityUid uid, WoundLickingComponent comp, DoAfterEvent<WoundLickData> args)
        {
            if (args.Cancelled || args.Handled || args.Args.Target == null)
            {
                return;
            }
            LickWound(uid, args.Args.Target.Value, args.AdditionalData.bloodstream, comp.PossibleDiseases, comp.MaxHeal, comp.DiseaseChance);
        }

        private void LickWound(EntityUid performer, EntityUid target, BloodstreamComponent bloodstream, List<String> diseases, float maxHeal = 15f, float diseaseChance = 0.25f)
        {
            // The more you heal, the more is disease chance
            // For 15 maxHeal and 50% diseaseChance
            //  Heal 15 > chance 50%
            //  Heal 7.5 > chance 25%
            //  Heal 0 > chance 0%

            var healed = bloodstream.BleedAmount;
            if (maxHeal - bloodstream.BleedAmount < 0) healed = maxHeal;
            var chance = diseaseChance*(1/maxHeal*healed);

            if(diseaseChance > 0f & diseases.Any())
            {
                if (TryComp<DiseaseCarrierComponent>(target, out var disCarrier))
                {
                    var diseaseName = diseases[_random.Next(0, diseases.Count())];
                    _disease.TryInfect(disCarrier, diseaseName, chance);
                }
            }

            _bloodstreamSystem.TryModifyBleedAmount(target, -healed, bloodstream);

            var targetIdentity = Identity.Entity(target, EntityManager);
            var performerIdentity = Identity.Entity(performer, EntityManager);
            var otherFilter = Filter.Pvs(performer, entityManager: EntityManager)
                .RemoveWhereAttachedEntity(e => e == performer || e == target);

            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-success", ("target", targetIdentity)),
                performer, performer);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-target-success", ("performer", performerIdentity)),
                target, target);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-other-success", ("performer", performerIdentity), ("target", targetIdentity)),
                performer, otherFilter, true);
        }

        private record struct WoundLickData(BloodstreamComponent bloodstream)
        {
            public readonly BloodstreamComponent bloodstream = bloodstream;
        }
    }

    public sealed class WoundLickingTargetActionEvent : EntityTargetActionEvent {}
}
