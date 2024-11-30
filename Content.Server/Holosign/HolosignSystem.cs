using Content.Shared.Examine;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Shared.Destructible;
using Content.Shared.Coordinates.Helpers;
using Content.Server.PowerCell;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.PowerCell;
using Content.Shared.Verbs;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Utility;


namespace Content.Server.Holosign
{
    public sealed class HolosignSystem : EntitySystem
    {
        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly PowerCellSystem _powerCell = default!;
        [Dependency] private readonly BatterySystem _battery = default!;
        [Dependency] private readonly SharedTransformSystem _transform = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<HolosignProjectorComponent, AfterInteractEvent>(OnAfterInteract);
            SubscribeLocalEvent<HolosignProjectorComponent, BeforeRangedInteractEvent>(OnBeforeInteract);
            SubscribeLocalEvent<HolosignProjectorComponent, UseInHandEvent>(OnUse);
            SubscribeLocalEvent<HolosignProjectorComponent, ExaminedEvent>(OnExamine);
            SubscribeLocalEvent<HolosignProjectorComponent, ComponentRemove>(OnRemove);
            SubscribeLocalEvent<HolosignProjectorComponent, GetVerbsEvent<Verb>>(AddClearVerb);
            SubscribeLocalEvent<HolosignProjectorComponent, PowerCellSlotEmptyEvent>(OnOutOfPower);
            SubscribeLocalEvent<HolosignBarrierComponent, ComponentRemove>(OnChildRemove);
            SubscribeLocalEvent<HolosignBarrierComponent, DestructionEventArgs>(OnChildDestroyed);
        }

        private void OnAfterInteract(EntityUid uid, HolosignProjectorComponent component, AfterInteractEvent args)
        {
            if (!args.CanReach)
            {
                _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-player-cannot-reach-message"), args.User, args.User);
                return;
            }

            // Not a holosign or holosign what put on map by mapper.
            if(args.Target == null || !EntityManager.TryGetComponent(args.Target.Value, out HolosignBarrierComponent? holosigncomponent))
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
            UpdateCellDrawState(uid, component, component.Childs.Count != 0);

            args.Handled = true;
        }

        private void OnUse(EntityUid uid, HolosignProjectorComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if (component.Childs.Count == 0)
            {
                _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-holosigns-none"), args.User, args.User);
                return;
            }

            ClearHolosignsVerb(uid, component);
            _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-holosigns-cleared"), args.User, args.User);

            args.Handled = true;
        }

        private void OnExamine(EntityUid uid, HolosignProjectorComponent component, ExaminedEvent args)
        {
            var childs = component.Childs.Count;
            var max = component.MaxSigns;
            args.PushMarkup(Loc.GetString("holoprojector-component-examine", ("amount", childs), ("max", max)));
        }

        private void ClearHolosignsVerb(EntityUid uid, HolosignProjectorComponent component)
        {
            UpdateCellDrawState(uid, component, false);

            // Should be cleared by OnChildRemove event?
            foreach (var child in component.Childs.ToArray())
            {
                if(!_entManager.Deleted(child))
                {
                    _entManager.DeleteEntity(child);
                }
            }
        }

        private void UpdateCellDrawState(EntityUid uid, HolosignProjectorComponent component, bool activated)
        {
            var drawComp = Comp<PowerCellDrawComponent>(uid);
            drawComp.DrawRate = component.DrawRatePerHolo * component.Childs.Count;
            _powerCell.QueueUpdate((uid, drawComp));
            _powerCell.SetDrawEnabled((uid, drawComp), activated);
        }

        private void OnOutOfPower(Entity<HolosignProjectorComponent> entity, ref PowerCellSlotEmptyEvent args)
        {
            if (entity.Comp.Childs.Count == 0)
                return;

            if (_powerCell.TryGetBatteryFromSlot(entity.Owner, out var batteryEnt, out var battery))
            {
                var drawComp = Comp<PowerCellDrawComponent>(entity.Owner);
                if (!_powerCell.TryUseCharge(entity.Owner, drawComp.DrawRate))
                    _battery.SetCharge(batteryEnt.Value, 0, battery);
            }

            ClearHolosignsVerb(entity.Owner, entity.Comp);
            _audio.PlayPvs(_audio.GetSound(new SoundPathSpecifier("/Audio/Machines/buzz-two.ogg")), entity.Owner);
            _popupSystem.PopupPredicted(Loc.GetString("holoprojector-component-oop"),
                entity.Owner,
                null,
                PopupType.SmallCaution);
        }

        private void OnRemove(EntityUid uid, HolosignProjectorComponent component, ComponentRemove args)
        {
            foreach (var child in component.Childs.ToArray())
            {
                if(!_entManager.Deleted(child))
                {
                    _entManager.DeleteEntity(child);
                }
            }
        }

        private void AddClearVerb(EntityUid uid, HolosignProjectorComponent component, GetVerbsEvent<Verb> args)
        {
            if (!args.CanAccess || !args.CanInteract)
                return;

            if (component.Childs.Count == 0)
                return;

            Verb clear = new ()
            {
                Act = () => ClearHolosignsVerb(uid, component),
                Text = Loc.GetString("holoprojector-component-verb-clear"),
                Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png")),
                Priority = 1,
                CloseMenu = true, // allow for easy double rotations.
            };
            args.Verbs.Add(clear);
        }

        private void OnChildRemove(EntityUid uid, HolosignBarrierComponent component, ComponentRemove args)
        {
            if(_entManager.Deleted(component.Holoprojector))
                return;

            // Holoprojector without Holoprojector component. BRUH
            if (!EntityManager.TryGetComponent(component.Holoprojector, out HolosignProjectorComponent? holoprojector))
                return;

            holoprojector.Childs.Remove(uid);
            UpdateCellDrawState(component.Holoprojector, holoprojector, holoprojector.Childs.Count != 0);
        }

        private void OnChildDestroyed(EntityUid uid, HolosignBarrierComponent component, DestructionEventArgs args)
        {
            if(_entManager.Deleted(component.Holoprojector))
                return;

            // Holoprojector without Holoprojector component. BRUH
            if (!EntityManager.TryGetComponent(component.Holoprojector, out HolosignProjectorComponent? holoprojector))
                return;

            holoprojector.Childs.Remove(uid);
            UpdateCellDrawState(component.Holoprojector, holoprojector, holoprojector.Childs.Count != 0);
        }
        private void OnBeforeInteract(EntityUid uid, HolosignProjectorComponent component, BeforeRangedInteractEvent args)
        {

            if (args.Handled
                || !args.CanReach // prevent placing out of range
                || HasComp<StorageComponent>(args.Target) // if it's a storage component like a bag, we ignore usage so it can be stored
                || HasComp<HolosignBarrierComponent>(args.Target)
                || !_powerCell.TryUseCharge(uid, component.DrawRatePerHolo, user: args.User)
               )
                return;

            if(component.Childs.Count >= component.MaxSigns)
            {
                _popupSystem.PopupEntity(Loc.GetString("holoprojector-component-holosigns-limit"), args.User, args.User);
            }
            else
            {
                // places the holographic sign at the click location, snapped to grid.
                // overlapping of the same holo on one tile remains allowed to allow holofan refreshes
                var holoUid =
                    EntityManager.SpawnEntity(component.SignProto, args.ClickLocation.SnapToGrid(EntityManager));
                var xform = Transform(holoUid);
                if (!xform.Anchored)
                {
                    _transform.AnchorEntity(holoUid, xform); // anchor to prevent any tempering with (don't know what could even interact with it)
                }

                var holoComp = _entManager.AddComponent<HolosignBarrierComponent>(holoUid);
                holoComp.Holoprojector = uid;
                component.Childs.Add(holoUid);
                UpdateCellDrawState(uid, component, component.Childs.Count != 0);

                args.Handled = true;
            }
        }
    }
}
