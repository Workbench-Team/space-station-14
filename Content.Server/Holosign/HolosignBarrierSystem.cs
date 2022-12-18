using Content.Shared.Interaction.Events;
using Content.Shared.Examine;
using Content.Server.Coordinates.Helpers;
using Content.Server.Power.Components;
using Content.Server.PowerCell;
using Content.Shared.PowerCell.Components;
using Robust.Shared.Timing;
using Content.Shared.Verbs;

namespace Content.Server.Holosign
{
    public sealed class HolosignBarrierSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
//            SubscribeLocalEvent<HolosignBarrierComponent, OnDestroyed>(OnDestroyed);
        }
/*
        private void OnDestroyed(EntityUid uid, HolosignBarrierComponent component, DestructionEventArgs args)
        {
            RaiseLocalEvent(component.Parent, new HoloBarrierDestroyed(component.Parent, uid));
        }
*/
    }

/*
    internal sealed class HoloBarrierDestroyed : EntityEventArgs
    {
     	public EntityUid Parent { get; }
        public EntityUid Child { get; }

        public WoundLickingEvent(EntityUid parent, EntityUid child)
        {
            Parent = parent;
            Child = child;
        }
    }
*/
}
