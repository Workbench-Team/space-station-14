using Content.Shared.Stunnable;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Server.Stunnable.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedPolicebatonSystem))]
public sealed partial class PolicebatonComponent : Component
{
    [DataField("telescopic"), ViewVariables(VVAccess.ReadWrite)]
    public bool Telescopic { get; set; } = false;

    [DataField("toggleSound")]
    public SoundSpecifier ToggleSound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/batonextend.ogg");
}
