using Content.Shared.Flash;

namespace Content.Server.Flash.Components
{
    [ComponentReference(typeof(SharedFlashableComponent))]
    [RegisterComponent, Access(typeof(FlashSystem))]
    public sealed class FlashableComponent : SharedFlashableComponent
    {
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
    }
}
