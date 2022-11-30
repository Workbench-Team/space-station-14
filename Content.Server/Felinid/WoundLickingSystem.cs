using Content.Server.DoAfter;
using Content.Server.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;

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

            _doAfterSystem.DoAfter(new DoAfterEventArgs(performer, 5, default, target)
            {
                BreakOnTargetMove = true,
                BreakOnUserMove = true,
                BreakOnDamage = true,
                BreakOnStun = true,
                UserFinishedEvent = new WoundLickingEvent(performer, target)
            });

            ev.Handled = true;
        }

        private void OnWouldLick(EntityUid uid, WoundLickingComponent comp, WoundLickingEvent args)
        {
            LickWound(args.Performer, args.Target, comp);
        }

        private void LickWound(EntityUid performer, EntityUid target, WoundLickingComponent comp)
        {
            _popupSystem.PopupEntity($"{performer} > {target}", performer, Filter.Entities(performer));
        }
    }


    public sealed class WoundLickingActionEvent : InstantActionEvent { };

    internal sealed class WoundLickingEvent : EntityEventArgs
    {
        public EntityUid Performer { get; }
        public EntityUid Target { get; }

        public WoundLickingEvent(EntityUid performer, EntityUid target)
        {
            Performer = performer;
            Target = target;
        }
    }

    public sealed class WoundLickingTargetActionEvent : EntityTargetActionEvent {}
}
