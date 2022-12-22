using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Server.Disease;
using Content.Server.Disease.Components;
using Content.Server.Body.Systems;
using Content.Server.Body.Components;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.IdentityManagement;
using Robust.Shared.Random;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using System.Threading;
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
            SubscribeLocalEvent<WoundLickingComponent, WoundLickingEvent>(OnWouldLick);

            SubscribeLocalEvent<WoundLickingEventCancel>(OnWouldLickCancel);
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

            if (!TryComp<WoundLickingComponent>(performer, out var woundLicking))
            { return; }

            // Prevents DoAfter from being called multiple times
            if (woundLicking.CancelToken != null)
            { return; }

            if (!TryComp<BloodstreamComponent>(target, out var bloodstream))
            { return; }

            // Logic
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
            
            // DoAfter
            woundLicking.CancelToken = new CancellationTokenSource();
            _doAfterSystem.DoAfter(new DoAfterEventArgs(performer, woundLicking.Delay, woundLicking.CancelToken.Token, target)
            {
                BreakOnTargetMove = true,
                BreakOnUserMove = true,
                BreakOnDamage = true,
                BreakOnStun = true,
                UserFinishedEvent = new WoundLickingEvent(performer, target, woundLicking, bloodstream),
                BroadcastCancelledEvent = new WoundLickingEventCancel(woundLicking)
            });

            ev.Handled = true;
        }

        private void OnWouldLick(EntityUid uid, WoundLickingComponent comp, WoundLickingEvent args)
        {
            comp.CancelToken = null;
            LickWound(args.Performer, args.Target, args.Bloodstream, comp.PossibleDiseases, comp.MaxHeal, comp.DiseaseChance);
        }

        private void OnWouldLickCancel(WoundLickingEventCancel args)
        {
            args.WoundLicking.CancelToken = null;
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
    }
    
    internal sealed class WoundLickingEvent : EntityEventArgs
    {
        public EntityUid Performer { get; }
        public EntityUid Target { get; }
        public WoundLickingComponent WoundLicking;
        public BloodstreamComponent Bloodstream;

        public WoundLickingEvent(EntityUid performer, EntityUid target, WoundLickingComponent woundLicking, BloodstreamComponent bloodstream)
        {
            Performer = performer;
            Target = target;
            WoundLicking = woundLicking;
            Bloodstream = bloodstream;
        }
    }

    internal sealed class WoundLickingEventCancel : EntityEventArgs
    {
        public WoundLickingComponent WoundLicking;

        public WoundLickingEventCancel(WoundLickingComponent woundLicking)
        {
            WoundLicking = woundLicking;
        }
    }

    public sealed class WoundLickingTargetActionEvent : EntityTargetActionEvent {}
}
