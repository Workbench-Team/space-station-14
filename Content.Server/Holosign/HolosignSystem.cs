using Content.Shared.Interaction.Events;
using Content.Shared.Examine;
using Content.Server.Coordinates.Helpers;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Shared.Destructible;
using Content.Server.Power.Components;
using Content.Server.PowerCell;
using Content.Shared.PowerCell.Components;
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
            SubscribeLocalEvent<HolosignProjectorComponent, UseInHandEvent>(OnUse);
            SubscribeLocalEvent<HolosignProjectorComponent, ExaminedEvent>(OnExamine);
            SubscribeLocalEvent<HolosignProjectorComponent, ComponentRemove>(OnRemove);
            SubscribeLocalEvent<HolosignProjectorComponent, GetVerbsEvent<Verb>>(AddClearVerb);
            SubscribeLocalEvent<HolosignBarrierComponent, ComponentRemove>(OnChildRemove);
            SubscribeLocalEvent<HolosignBarrierComponent, DestructionEventArgs>(OnChildDestroyed);
        }

        private void OnUse(EntityUid uid, HolosignProjectorComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if(component.Childs.Count >= component.MaxSigns)
            {
                _popupSystem.PopupEntity(Loc.GetString("holoprojector-limit"), args.User, args.User);
            }
            else
            {
                var holo = EntityManager.SpawnEntity(component.SignProto, Transform(args.User).Coordinates.SnapToGrid(EntityManager));
                var holosigncomp = _entManager.AddComponent<HolosignBarrierComponent>(holo);
                holosigncomp.Parent = uid;
                component.Childs.Add(holo);
            }

            args.Handled = true;
        }

        private void OnExamine(EntityUid uid, HolosignProjectorComponent component, ExaminedEvent args)
        {
            var childs = component.Childs.Count;
            args.PushMarkup(Loc.GetString("holoprojector-barriers-active", ("amount", childs)));
        }

        private void ClearHolosignsVerb(EntityUid uid, HolosignProjectorComponent component, EntityUid player)
        {
            _popupSystem.PopupEntity(Loc.GetString("holoprojector-cleared"), player, player);

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
                    Text = Loc.GetString("holoprojector-verb-clear"),
                    IconTexture =  "/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png",
                    Priority = -1,
                    CloseMenu = true, // allow for easy double rotations.
                };
                args.Verbs.Add(clear);
            }
        }

        private void OnChildRemove(EntityUid uid, HolosignBarrierComponent component, ComponentRemove args)
        {
            if(!_entManager.EntityExists(component.Parent))
                return;

            // Holoprojector without Holoprojector component. BRUH
            if(EntityManager.TryGetComponent(component.Parent, out HolosignProjectorComponent? holoprojector))
            {
                holoprojector.Childs.Remove(uid);
            }
        }

        private void OnChildDestroyed(EntityUid uid, HolosignBarrierComponent component, DestructionEventArgs args)
        {
            if(!_entManager.EntityExists(component.Parent))
                return;

            // Holoprojector without Holoprojector component. BRUH
            if(EntityManager.TryGetComponent(component.Parent, out HolosignProjectorComponent? holoprojector))
            {
                holoprojector.Childs.Remove(uid);
            }
        }
    }
}
