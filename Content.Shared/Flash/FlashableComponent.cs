using Content.Shared.Physics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Flash
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class FlashableComponent : Component
    {
        public float Duration;
        public TimeSpan LastFlash;

        [DataField]
        public CollisionGroup CollisionGroup = CollisionGroup.Opaque;

        public override bool SendOnlyToOwner => true;

        /// <summary>
        /// Coefficent for flash duration
        /// </summary>
        [DataField("durationMultiplier")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float DurationMultiplier { get; set; } = 1;

        /// <summary>
        /// Additional duration coefficent if entity was flashed with flashbang
        /// </summary>
        [DataField("bangAddMultiplier")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float BangAddMultiplier { get; set; } = 0;

        /// <summary>
        /// Additional duration coefficent if entity was flashed with flashbang
        /// </summary>
        [DataField("bangFlash")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool BangFlash { get; set; } = true;
    }

    [Serializable, NetSerializable]
    public sealed class FlashableComponentState : ComponentState
    {
        public float Duration { get; }
        public TimeSpan Time { get; }

        public FlashableComponentState(float duration, TimeSpan time)
        {
            Duration = duration;
            Time = time;
        }
    }

    [Serializable, NetSerializable]
    public enum FlashVisuals : byte
    {
        BaseLayer,
        LightLayer,
        Burnt,
        Flashing,
    }
}
