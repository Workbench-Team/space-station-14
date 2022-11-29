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

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<WoundLickingComponent, WoundLickingActionEvent>(OnActionPerform);
            SubscribeLocalEvent<WoundLickingComponent, ComponentInit>(OnInit);
        }

        private void OnInit(EntityUid uid, WoundLickingComponent comp, ComponentInit args)
        {
            _actionsSystem.AddAction(uid, comp.Action, null);
        }

        private void OnActionPerform(EntityUid uid, WoundLickingComponent comp, WoundLickingActionEvent args)
        {
            if (args.Handled)
            return;

            //  i have no idea how i must do it
            _popupSystem.PopupEntity("WIP", uid, Filter.Entities(uid));

            args.Handled = true;
        }
    }

    public sealed class WoundLickingActionEvent : InstantActionEvent { };
}
