using Content.Server.DoAfter;
using Content.Server.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Server.Body.Components;
using Content.Shared.IdentityManagement;

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

            if (!TryComp<BloodstreamComponent>(target, out var bloodstrComponent))
            { return; }

            if (performer == target & !woundLicking.CanSelfApply)
            {
                _popupSystem.PopupEntity(Loc.GetString("lick-wounds-yourself-impossible"), performer, Filter.Entities(performer));
                return;
            }

            if (bloodstrComponent.BleedAmount == 0)
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

            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-begin", ("target", targetIdentity)), performer, Filter.Entities(performer));
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-target-begin", ("performer", performerIdentity)), target, Filter.Entities(target));

            var otherFilter = Filter.Pvs(performer, entityManager: EntityManager).RemoveWhereAttachedEntity(e => e == performer || e == target);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-other-begin", ("performer", performerIdentity), ("target", targetIdentity)), target, otherFilter);

            _doAfterSystem.DoAfter(new DoAfterEventArgs(performer, woundLicking.Delay, default, target)
            {
                BreakOnTargetMove = true,
                BreakOnUserMove = true,
                BreakOnDamage = true,
                BreakOnStun = true,
                UserFinishedEvent = new WoundLickingEvent(performer, target, bloodstrComponent)
            });

            ev.Handled = true;
        }

        private void OnWouldLick(EntityUid uid, WoundLickingComponent comp, WoundLickingEvent args)
        {
            LickWound(args.Performer, args.Target);
        }

        private void LickWound(EntityUid performer, EntityUid target, bool CanSelfApply = false)
        {
            var targetIdentity = Identity.Entity(target, EntityManager);
            var performerIdentity = Identity.Entity(performer, EntityManager);

            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-performer-success", ("target", targetIdentity)), performer, Filter.Entities(performer));
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-target-success", ("performer", performerIdentity)), target, Filter.Entities(target));

            var otherFilter = Filter.Pvs(performer, entityManager: EntityManager).RemoveWhereAttachedEntity(e => e == performer || e == target);
            _popupSystem.PopupEntity(Loc.GetString("lick-wounds-other-success", ("performer", performerIdentity), ("target", targetIdentity)), target, otherFilter);
        }
    }


    public sealed class WoundLickingActionEvent : InstantActionEvent { };

    internal sealed class WoundLickingEvent : EntityEventArgs
    {
        public EntityUid Performer { get; }
        public EntityUid Target { get; }

        public WoundLickingEvent(EntityUid performer, EntityUid target, BloodstreamComponent bloodstream)
        {
            Performer = performer;
            Target = target;
        }
    }

    public sealed class WoundLickingTargetActionEvent : EntityTargetActionEvent {}
}
