using Content.Server.DoAfter;
using Content.Server.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Server.Body.Components;
using Content.Shared.IdentityManagement;
using System.Threading;

namespace Content.Server.Felinid
{
    /// <summary>
    /// "Lick your or other felinid wounds. Reduce bleeding, but unsanitary and can cause diseases."
    /// </summary>
    public sealed class WoundLickingSystem : EntitySystem
    {
        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        
        private const string WouldLickingActionPrototype = "WoundLicking";

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<WoundLickingComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<WoundLickingComponent, WoundLickingEvent>(OnWouldLick);

            SubscribeLocalEvent<WoundLickingEventCancel>(OnWouldLickCancel);
            SubscribeLocalEvent<WoundLickingTargetActionEvent>(OnActionPerform);
        }

        private void OnInit(EntityUid uid, WoundLickingComponent comp, ComponentInit args)
        {
            var action = new EntityTargetAction(_prototypeManager.Index<EntityTargetActionPrototype>(WouldLickingActionPrototype));
            _actionsSystem.AddAction(uid, action, null);
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
                _popupSystem.PopupEntity(Loc.GetString("lick-wounds-yourself-impossible"), performer, Filter.Entities(performer));
                return;
            }

            if (bloodstream.BleedAmount == 0)
            {
                if (performer == target)
                {
                    _popupSystem.PopupEntity(Loc.GetString("lick-wounds-yourself-no-wounds"), performer, Filter.Entities(performer));
                    return;
                }
                _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-no-wounds", ("target", target)), performer, Filter.Entities(performer));
                return;
            }

            var targetIdentity = Identity.Entity(target, EntityManager);
            var performerIdentity = Identity.Entity(performer, EntityManager);
            var otherFilter = Filter.Pvs(performer, entityManager: EntityManager).RemoveWhereAttachedEntity(e => e == performer || e == target);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-begin", ("target", targetIdentity)), performer, Filter.Entities(performer));
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-target-begin", ("performer", performerIdentity)), target, Filter.Entities(target));
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-other-begin", ("performer", performerIdentity), ("target", targetIdentity)), target, otherFilter);

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
            LickWound(args.Performer, args.Target);
        }

        private void OnWouldLickCancel(WoundLickingEventCancel args)
        {
            args.WoundLicking.CancelToken = null;
        }

        private void LickWound(EntityUid performer, EntityUid target, float diseaseChance = 0.5f, float maxHeal = 15f)
        {
            var chance = 0f;
            

            var targetIdentity = Identity.Entity(target, EntityManager);
            var performerIdentity = Identity.Entity(performer, EntityManager);
            var otherFilter = Filter.Pvs(performer, entityManager: EntityManager).RemoveWhereAttachedEntity(e => e == performer || e == target);

            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-success", ("target", targetIdentity)), performer, Filter.Entities(performer));
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-target-success", ("performer", performerIdentity)), target, Filter.Entities(target));
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-other-success", ("performer", performerIdentity), ("target", targetIdentity)), target, otherFilter);
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
