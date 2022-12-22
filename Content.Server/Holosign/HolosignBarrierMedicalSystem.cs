using Content.Server.Construction;
using Content.Server.Popups;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Content.Shared.Interaction;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Server.GameObjects;

namespace Content.Server.Holosign
{
    public sealed class HolosignBarrierMedicalSystem : EntitySystem
    {
        [Dependency] private readonly SharedPhysicsSystem _physics = default!;
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly SharedInteractionSystem _interactionSystem = default!;
        [Dependency] private readonly PopupSystem _popup = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<HolosignBarrierMedicalComponent, StartCollideEvent>(OnCollide);
            SubscribeLocalEvent<HolosignBarrierMedicalComponent, EndCollideEvent>(OnEndCollide);
            SubscribeLocalEvent<HolosignBarrierMedicalComponent, InteractHandEvent>(OnInteract);
        }

        private void OnCollide(EntityUid uid, HolosignBarrierMedicalComponent component, ref StartCollideEvent args)
        {
            var otherEnt = args.OtherFixture.Body.Owner;

            if (!_entManager.TryGetComponent<SpriteComponent?>(uid, out var sprite))
                return;

            sprite.LayerSetState(0, "deny");
        }

        private void OnEndCollide(EntityUid uid, HolosignBarrierMedicalComponent component, ref EndCollideEvent args)
        {
     	    var otherEnt = args.OtherFixture.Body.Owner;

            if (!_entManager.TryGetComponent<SpriteComponent?>(uid, out var sprite))
                return;

            sprite.LayerSetState(0, "icon");
        }

        private void OnInteract(EntityUid uid, HolosignBarrierMedicalComponent component, InteractHandEvent args)
        {
            if (!_interactionSystem.InRangeUnobstructed(args.User, args.Target))
                return;

            if (!_entManager.TryGetComponent(args.User, out ActorComponent? actor))
                return;
        }

    }
}
