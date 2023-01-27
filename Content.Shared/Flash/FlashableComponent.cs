using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Flash
{
    [RegisterComponent, NetworkedComponent]
    public sealed class FlashableComponent : Component
    {
        public float Duration;
        public TimeSpan LastFlash;

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
}
