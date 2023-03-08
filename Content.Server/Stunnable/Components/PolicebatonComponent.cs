using Content.Server.Stunnable.Systems;
using Content.Shared.Timing;
using Robust.Shared.Audio;

namespace Content.Server.Stunnable.Components
{
    [RegisterComponent, Access(typeof(PolicebatonSystem))]
    public sealed class PolicebatonComponent : Component
    {
        public bool Activated = false;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("telescopic")]
        public bool Telescopic { get; set; } = false;

        [DataField("stunSound")]
        public SoundSpecifier StunSound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/egloves.ogg");
        
        [DataField("toggleSound")]
        public SoundSpecifier ToggleSound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/batonextend.ogg");
    }
}
