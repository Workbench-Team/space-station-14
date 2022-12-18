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
        [Dependency] private readonly PowerCellSystem _cellSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<HolosignProjectorComponent, UseInHandEvent>(OnUse);
            SubscribeLocalEvent<HolosignProjectorComponent, ExaminedEvent>(OnExamine);
            SubscribeLocalEvent<HolosignProjectorComponent, ComponentRemove>(OnRemove);
            SubscribeLocalEvent<HolosignProjectorComponent, GetVerbsEvent<Verb>>(AddClearVerb);
//            SubscribeLocalEvent<HolosignBarrierComponent, HoloBarrierDestroyed>(OnChildDestroy);
            SubscribeLocalEvent<HolosignBarrierComponent, DestructionEventArgs>(OnChildDestroyed);
        }

        private void OnUse(EntityUid uid, HolosignProjectorComponent component, UseInHandEvent args)
        {
            if (args.Handled || !_cellSystem.TryGetBatteryFromSlot(uid, out var battery))
                return;

            if(component.Childs.Count >= component.MaxSigns)
            {
                _popupSystem.PopupEntity("I hate C#", args.User, args.User);
            } else if(!battery.TryUseCharge(component.ChargeUse))
            {
                // TODO: Too tired to deal
                var holo = EntityManager.SpawnEntity(component.SignProto, Transform(args.User).Coordinates.SnapToGrid(EntityManager));

//                Transform(holo).Anchored = true;
//                HolosignBarrierComponent(holo).Parent = uid;
                component.Childs.Add(holo);
            }

            args.Handled = true;
        }

        private void OnExamine(EntityUid uid, HolosignProjectorComponent component, ExaminedEvent args)
        {
            // TODO: This should probably be using an itemstatus
            // TODO: I'm too lazy to do this rn but it's literally copy-paste from emag.
            _cellSystem.TryGetBatteryFromSlot(uid, out var battery);
            var charges = UsesRemaining(component, battery);
            var maxCharges = MaxUses(component, battery);
            var childs = component.Childs.Count;

            args.PushMarkup(Loc.GetString("emag-charges-remaining", ("charges", charges)));

            if (charges > 0 && charges == maxCharges)
            {
                args.PushMarkup(Loc.GetString("emag-max-charges"));
                return;
            }

            args.PushMarkup("Childs: {childs}");
        }

        private void RemoveChilds(EntityUid uid, HolosignProjectorComponent component)
        {
            foreach (var child in component.Childs)
            {
                if(_entManager.EntityExists(child))
                {
                    _entManager.DeleteEntity(child);
                }
            }
            component.Childs.Clear();
        }

        private void OnRemove(EntityUid uid, HolosignProjectorComponent component, ComponentRemove args)
        {
            RemoveChilds(uid, component);
        }

        private void AddClearVerb(EntityUid uid, HolosignProjectorComponent component, GetVerbsEvent<Verb> args)
        {
            if (!args.CanAccess || !args.CanInteract)
                return;

            if(component.Childs.Count > 0)
            {
                Verb clear = new ()
                {
             	    Act = () => RemoveChilds(uid, component),
                    Text = "Clear",
                    IconTexture =  "/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png",
                    Priority = -1,
                    CloseMenu = true, // allow for easy double rotations.
                };
                args.Verbs.Add(clear);
            }
        }

        private void OnChildDestroyed(EntityUid uid, HolosignBarrierComponent component, DestructionEventArgs args)
        {
            if(!_entManager.EntityExists(component.Parent))
                return;

            // Holosign without Holoprojector component. BRUH
            if(EntityManager.TryGetComponent(component.Parent, out HolosignProjectorComponent? holosign))
            {
                holosign.Childs.Remove(uid);
            }
        }

        private int UsesRemaining(HolosignProjectorComponent component, BatteryComponent? battery = null)
        {
            if (battery == null ||
                component.ChargeUse == 0f) return 0;

            return (int) (battery.CurrentCharge / component.ChargeUse);
        }

        private int MaxUses(HolosignProjectorComponent component, BatteryComponent? battery = null)
        {
            if (battery == null ||
                component.ChargeUse == 0f) return 0;

            return (int) (battery.MaxCharge / component.ChargeUse);
        }
    }
}
