using Content.Shared.Flash;

namespace Content.Server.Flash.Components
{
    [ComponentReference(typeof(SharedFlashableComponent))]
    [RegisterComponent, Access(typeof(FlashSystem))]
    public sealed class FlashableComponent : SharedFlashableComponent
    {
        [DataField("durationMultiplier")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float DurationMultiplier { get; set; } = 1;

        [DataField("durationBangMultiplier")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float DurationBangMultiplier { get; set; } = 1;
    }
}
