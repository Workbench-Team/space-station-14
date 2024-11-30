using System.Numerics;
using System.Threading;
using Content.Server.DoAfter;
using Content.Server.Resist;
using Content.Server.Popups;
using Content.Server.Inventory;
using Content.Server.Starshine.Carrying.Components;
using Content.Shared.Mobs;
using Content.Shared.DoAfter;
using Content.Shared.Buckle.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Hands;
using Content.Shared.Stunnable;
using Content.Shared.Interaction.Events;
using Content.Shared.Verbs;
using Content.Shared.Climbing.Events;
using Content.Shared.Starshine.Carrying;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;
using Content.Shared.ActionBlocker;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Item;
using Content.Shared.Throwing;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Events;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Server.GameObjects;

namespace Content.Server.Starshine.Carrying
{
    public sealed class CarryingSystem : EntitySystem
    {
        [Dependency] private readonly VirtualItemSystem _virtualItemSystem = default!;
        [Dependency] private readonly CarryingSlowdownSystem _slowdown = default!;
        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly StandingStateSystem _standingState = default!;
        [Dependency] private readonly ActionBlockerSystem _actionBlockerSystem = default!;
        [Dependency] private readonly PullingSystem _pullingSystem = default!;
        [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
        [Dependency] private readonly EscapeInventorySystem _escapeInventorySystem = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly MovementSpeedModifierSystem _movementSpeed = default!;
        [Dependency] private readonly TransformSystem _transform = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<CarriableComponent, GetVerbsEvent<AlternativeVerb>>(AddCarryVerb);
            SubscribeLocalEvent<CarryingComponent, VirtualItemDeletedEvent>(OnVirtualItemDeleted);
            SubscribeLocalEvent<CarryingComponent, BeforeThrowEvent>(OnThrow);
            SubscribeLocalEvent<CarryingComponent, EntParentChangedMessage>(OnParentChanged);
            SubscribeLocalEvent<CarryingComponent, MobStateChangedEvent>(OnMobStateChanged);
            SubscribeLocalEvent<BeingCarriedComponent, InteractionAttemptEvent>(OnInteractionAttempt);
            SubscribeLocalEvent<BeingCarriedComponent, MoveInputEvent>(OnMoveInput);
            SubscribeLocalEvent<BeingCarriedComponent, UpdateCanMoveEvent>(OnMoveAttempt);
            SubscribeLocalEvent<BeingCarriedComponent, StandAttemptEvent>(OnStandAttempt);
            SubscribeLocalEvent<BeingCarriedComponent, GettingInteractedWithAttemptEvent>(OnInteractedWith);
            SubscribeLocalEvent<BeingCarriedComponent, PullAttemptEvent>(OnPullAttempt);
            SubscribeLocalEvent<BeingCarriedComponent, StartClimbEvent>(OnStartClimb);
            SubscribeLocalEvent<BeingCarriedComponent, BuckledEvent>(OnBuckleChange);
            SubscribeLocalEvent<BeingCarriedComponent, UnbuckledEvent>(OnBuckleChange);
            SubscribeLocalEvent<BeingCarriedComponent, StrappedEvent>(OnBuckleChange);
            SubscribeLocalEvent<BeingCarriedComponent, UnstrappedEvent>(OnBuckleChange);
            SubscribeLocalEvent<CarriableComponent, CarryDoAfterEvent>(OnDoAfter);
        }

        private void AddCarryVerb(EntityUid uid, CarriableComponent component, GetVerbsEvent<AlternativeVerb> args)
        {
            if (!args.CanInteract || !args.CanAccess || !_mobStateSystem.IsAlive(args.User)
                || !CanCarry(args.User, uid, component)
                || HasComp<CarryingComponent>(args.User)
                || HasComp<BeingCarriedComponent>(args.User) || HasComp<BeingCarriedComponent>(args.Target)
                || args.User == args.Target)
                return;

            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    StartCarryDoAfter(args.User, uid, component);
                },
                Text = Loc.GetString("carry-verb"),
                Priority = 2,
            };
            args.Verbs.Add(verb);
        }

        /// <summary>
        /// Since the carried entity is stored as 2 virtual items, when deleted we want to drop them.
        /// </summary>
        private void OnVirtualItemDeleted(EntityUid uid, CarryingComponent component, VirtualItemDeletedEvent args)
        {
            if (!HasComp<CarriableComponent>(args.BlockingEntity))
                return;

            DropCarried(uid, args.BlockingEntity);
        }

        /// <summary>
        /// Basically using virtual item passthrough to throw the carried person. A new age!
        /// Maybe other things besides throwing should use virt items like this...
        /// </summary>
        private void OnThrow(EntityUid uid, CarryingComponent component, ref BeforeThrowEvent args)
        {
            if (!TryComp<VirtualItemComponent>(args.ItemUid, out var virtItem)
                || !TryComp<PhysicsComponent>(virtItem.BlockingEntity, out var carriedPhysics)
                || !HasComp<CarriableComponent>(virtItem.BlockingEntity))
                return;

            args.ItemUid = virtItem.BlockingEntity;

            var multiplier = carriedPhysics.Mass / 71.5f;
            args.ThrowSpeed = 5f / multiplier;
        }

        private void OnParentChanged(EntityUid uid, CarryingComponent component, ref EntParentChangedMessage args)
        {
            var xform = Transform(uid);
            if (xform.MapUid != args.OldMapId || xform.ParentUid == xform.GridUid)
                return;

            DropCarried(uid, component.Carried);
        }

        private void OnMobStateChanged(EntityUid uid, CarryingComponent component, MobStateChangedEvent args)
        {
            DropCarried(uid, component.Carried);
        }

        /// <summary>
        /// Only let the person being carried interact with their carrier and things on their person.
        /// </summary>
        private void OnInteractionAttempt(EntityUid uid, BeingCarriedComponent component, InteractionAttemptEvent args)
        {
            if (args.Target == null)
                return;

            var targetParent = Transform(args.Target.Value).ParentUid;

            if (args.Target.Value != component.Carrier && targetParent != component.Carrier && targetParent != uid)
                args.Cancelled = true;
        }

        /// <summary>
        /// Try to escape via the escape inventory system.
        /// </summary>
        private void OnMoveInput(EntityUid uid, BeingCarriedComponent component, ref MoveInputEvent args)
        {
            if (!TryComp<CanEscapeInventoryComponent>(uid, out var escape)
                || !args.HasDirectionalMovement)
                return;

            // Check if the victim is in any way incapacitated, and if not make an escape attempt.
            if (!_actionBlockerSystem.CanInteract(uid, component.Carrier))
                return;

            _escapeInventorySystem.AttemptEscape(uid, component.Carrier, escape, 0.5f);
        }

        private void OnMoveAttempt(EntityUid uid, BeingCarriedComponent component, UpdateCanMoveEvent args)
        {
            args.Cancel();
        }

        private void OnStandAttempt(EntityUid uid, BeingCarriedComponent component, StandAttemptEvent args)
        {
            args.Cancel();
        }

        private void OnInteractedWith(EntityUid uid, BeingCarriedComponent component, GettingInteractedWithAttemptEvent args)
        {
            if (args.Uid != component.Carrier)
                args.Cancelled = true;
        }

        private void OnPullAttempt(EntityUid uid, BeingCarriedComponent component, PullAttemptEvent args)
        {
            args.Cancelled = true;
        }

        private void OnStartClimb(EntityUid uid, BeingCarriedComponent component, ref StartClimbEvent args)
        {
            DropCarried(component.Carrier, uid);
        }

        private void OnBuckleChange<TEvent>(EntityUid uid, BeingCarriedComponent component, TEvent args)
        {
            DropCarried(component.Carrier, uid);
        }

        private void OnDoAfter(EntityUid uid, CarriableComponent component, CarryDoAfterEvent args)
        {
            component.CancelToken = null;
            if (args.Handled || args.Cancelled
                || !CanCarry(args.Args.User, uid, component))
                return;

            Carry(args.Args.User, uid);
            args.Handled = true;
        }
        private void StartCarryDoAfter(EntityUid carrier, EntityUid carried, CarriableComponent component)
        {
            var length = TimeSpan.FromSeconds(component.PickupDuration
                        * (_standingState.IsDown(carried) ? 0.5f : 1f));

            component.CancelToken = new CancellationTokenSource();

            var ev = new CarryDoAfterEvent();
            var args = new DoAfterArgs(EntityManager, carrier, length, ev, carried, target: carried)
            {
                BreakOnMove = true,
                NeedHand = true,
            };

            _doAfterSystem.TryStartDoAfter(args);

            // Show a popup to the person getting picked up
            _popupSystem.PopupEntity(Loc.GetString("carry-started", ("carrier", carrier)), carried, carried, Shared.Popups.PopupType.MediumCaution);
        }

        private void Carry(EntityUid carrier, EntityUid carried)
        {
            if (TryComp<PullableComponent>(carried, out var pullable))
                _pullingSystem.TryStopPull(carried, pullable);

            _transform.AttachToGridOrMap(carrier);
            _transform.AttachToGridOrMap(carried);
            _transform.SetCoordinates(carried, Transform(carrier).Coordinates);
            _transform.SetParent(carried, carrier);

            _virtualItemSystem.TrySpawnVirtualItemInHand(carried, carrier);
            _virtualItemSystem.TrySpawnVirtualItemInHand(carried, carrier);
            var carryingComp = EnsureComp<CarryingComponent>(carrier);
            EnsureComp<CarryingSlowdownComponent>(carrier);
            ApplyCarrySlowdown(carrier, carried);
            var carriedComp = EnsureComp<BeingCarriedComponent>(carried);
            EnsureComp<KnockedDownComponent>(carried);

            carryingComp.Carried = carried;
            carriedComp.Carrier = carrier;

            _actionBlockerSystem.UpdateCanMove(carried);
        }

        public bool TryCarry(EntityUid carrier, EntityUid toCarry, CarriableComponent? carriedComp = null)
        {
            if (!Resolve(toCarry, ref carriedComp, false)
                || !CanCarry(carrier, toCarry, carriedComp)
                || HasComp<BeingCarriedComponent>(carrier)
                || HasComp<ItemComponent>(carrier)
                || TryComp<PhysicsComponent>(carrier, out var carrierPhysics)
                && TryComp<PhysicsComponent>(toCarry, out var toCarryPhysics)
                && carrierPhysics.Mass < toCarryPhysics.Mass * 2f)
                return false;

            Carry(carrier, toCarry);

            return true;
        }

        public void DropCarried(EntityUid carrier, EntityUid carried)
        {
            RemComp<CarryingComponent>(carrier); // get rid of this first so we don't recursively fire that event
            RemComp<CarryingSlowdownComponent>(carrier);
            RemComp<BeingCarriedComponent>(carried);
            RemComp<KnockedDownComponent>(carried);
            _actionBlockerSystem.UpdateCanMove(carried);
            _virtualItemSystem.DeleteInHandsMatching(carrier, carried);
            _transform.AttachToGridOrMap(carried);
            _standingState.Stand(carried);
            _movementSpeed.RefreshMovementSpeedModifiers(carrier);
        }

        private void ApplyCarrySlowdown(EntityUid carrier, EntityUid carried)
        {

            var slowdownComp = EnsureComp<CarryingSlowdownComponent>(carrier);
            _slowdown.SetModifier(carrier, slowdownComp.WalkModifier, slowdownComp.SprintModifier, slowdownComp);
        }

        public bool CanCarry(EntityUid carrier, EntityUid carried, CarriableComponent? carriedComp = null)
        {
            return Resolve(carried, ref carriedComp, false)
                   && carriedComp.CancelToken == null
                   && HasComp<MapGridComponent>(Transform(carrier).ParentUid)
                   && !HasComp<BeingCarriedComponent>(carrier)
                   && !HasComp<BeingCarriedComponent>(carried)
                   && TryComp<HandsComponent>(carrier, out var hands)
                   && hands.CountFreeHands() >= carriedComp.FreeHandsRequired;
        }

        public override void Update(float frameTime)
        {
            var query = EntityQueryEnumerator<BeingCarriedComponent>();
            while (query.MoveNext(out var carried, out var comp))
            {
                var carrier = comp.Carrier;
                if (carrier is not { Valid: true } || carried is not { Valid: true })
                    continue;

                // SOMETIMES - when an entity is inserted into disposals, or a cryosleep chamber - it can get re-parented without a proper reparent event
                // when this happens, it needs to be dropped because it leads to weird behavior
                if (Transform(carried).ParentUid != carrier)
                {
                    DropCarried(carrier, carried);
                    continue;
                }

                // Make sure the carried entity is always centered relative to the carrier, as gravity pulls can offset it otherwise
                var xform = Transform(carried);
                if (!xform.LocalPosition.Equals(Vector2.Zero))
                {
                    xform.LocalPosition = Vector2.Zero;
                }
            }
            query.Dispose();
        }
    }
}
