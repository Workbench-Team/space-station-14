using Content.Shared.Interaction.Events;
using Content.Shared.Examine;
using Content.Server.Coordinates.Helpers;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Shared.Destructible;
using Content.Server.Power.Components;
using Content.Server.PowerCell;
using Content.Shared.PowerCell.Components;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Robust.Shared.Timing;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;


namespace Content.Server.Holosign
{
    public sealed class HolosignSystem : EntitySystem
    {
        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly IEntityManager _entManager = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<HolosignProjectorComponent, AfterInteractEvent>(OnAfterInteract);
            SubscribeLocalEvent<HolosignProjectorComponent, UseInHandEvent>(OnUse);
            SubscribeLocalEvent<HolosignProjectorComponent, ExaminedEvent>(OnExamine);
            SubscribeLocalEvent<HolosignProjectorComponent, ComponentRemove>(OnRemove);
            SubscribeLocalEvent<HolosignProjectorComponent, GetVerbsEvent<Verb>>(AddClearVerb);
            SubscribeLocalEvent<HolosignBarrierComponent, ComponentRemove>(OnChildRemove);
            SubscribeLocalEvent<HolosignBarrierComponent, DestructionEventArgs>(OnChildDestroyed);
        }

        private void OnAfterInteract(EntityUid uid, HolosignProjectorComponent component, AfterInteractEvent args)
        {
            if (!args.CanReach)
            {
                _popupSystem.PopupEntity(Loc.GetString("gas-analyzer-component-player-cannot-reach-message"), args.User, args.User);
                return;
            }

            // Not a holosign or holosign what put on map by mapper.
            if((args.Target == null) || !EntityManager.TryGetComponent(args.Target.Value, out HolosignBarrierComponent? holosigncomponent))
            {
                return;
            }

            if(!component.Childs.Contains(args.Target.Value))
            {
                _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-player-not-a-child"), args.User, args.User);
                return;
            }

            // Would be fun see removing holoprojection without holosign component.
            _entManager.DeleteEntity(args.Target.Value);
            _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-holosign-removed"), args.User, args.User);

            args.Handled = true;
        }

        private void OnUse(EntityUid uid, HolosignProjectorComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if(component.Childs.Count >= component.MaxSigns)
            {
                _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-holosigns-limit"), args.User, args.User);
            }
            else
            {
                var holo = EntityManager.SpawnEntity(component.SignProto, Transform(args.User).Coordinates.SnapToGrid(EntityManager));
                var holosigncomp = _entManager.AddComponent<HolosignBarrierComponent>(holo);
                holosigncomp.Holoprojector = uid;
                component.Childs.Add(holo);
            }

            args.Handled = true;
        }

        private void OnExamine(EntityUid uid, HolosignProjectorComponent component, ExaminedEvent args)
        {
            var childs = component.Childs.Count;
            var max = component.MaxSigns;
            args.PushMarkup(Loc.GetString("holoprojector-component-examine", ("amount", childs), ("max", max)));
        }

        private void ClearHolosignsVerb(EntityUid uid, HolosignProjectorComponent component, EntityUid player)
        {
            _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-holosigns-cleared"), player, player);

            // Should be cleared by OnChildRemove event?
            foreach (var child in component.Childs.ToArray())
            {
                if(_entManager.EntityExists(child))
                {
                    _entManager.DeleteEntity(child);
                }
            }
        }

        private void OnRemove(EntityUid uid, HolosignProjectorComponent component, ComponentRemove args)
        {
            foreach (var child in component.Childs.ToArray())
            {
                if(_entManager.EntityExists(child))
                {
                    _entManager.DeleteEntity(child);
                }
            }
        }

        private void AddClearVerb(EntityUid uid, HolosignProjectorComponent component, GetVerbsEvent<Verb> args)
        {
            if (!args.CanAccess || !args.CanInteract)
                return;

            if(component.Childs.Count > 0)
            {
                Verb clear = new ()
                {
             	    Act = () => ClearHolosignsVerb(uid, component, args.User),
                    Text = Loc.GetString("holoprojector-component-verb-clear"),
                    IconTexture =  "/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png",
                    Priority = 1,
                    CloseMenu = true, // allow for easy double rotations.
                };
                args.Verbs.Add(clear);
            }
        }

        private void OnChildRemove(EntityUid uid, HolosignBarrierComponent component, ComponentRemove args)
        {
            if(!_entManager.EntityExists(component.Holoprojector))
                return;

            // Holoprojector without Holoprojector component. BRUH
            if(EntityManager.TryGetComponent(component.Holoprojector, out HolosignProjectorComponent? holoprojector))
            {
                holoprojector.Childs.Remove(uid);
            }
        }

        private void OnChildDestroyed(EntityUid uid, HolosignBarrierComponent component, DestructionEventArgs args)
        {
            if(!_entManager.EntityExists(component.Holoprojector))
                return;

            // Holoprojector without Holoprojector component. BRUH
            if(EntityManager.TryGetComponent(component.Holoprojector, out HolosignProjectorComponent? holoprojector))
            {
                holoprojector.Childs.Remove(uid);
            }
        }
    }
}
