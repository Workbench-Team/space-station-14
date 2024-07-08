using Content.Shared.Actions;
using Content.Shared.Starshine.Eye.NightVision.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Starshine.Eye.NightVision.Components;

[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(NightVisionSystem), typeof(PNVSystem))]
public sealed partial class NightVisionComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public bool IsOn;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public Color Color = Color.FromHex("#5cd65c");

    [DataField]
    public bool IsToggle;

    [DataField] public EntityUid? ActionContainer;

    [Access(Other = AccessPermissions.ReadWriteExecute)]
    public bool DrawShadows = false;

    [Access(Other = AccessPermissions.ReadWriteExecute)]
    public bool GraceFrame = false;

    [DataField]
    public bool PlaySoundOn = true;

    [DataField]
    public SoundSpecifier OffSound = new SoundPathSpecifier("/Audio/Starshine/Misc/night_vision.ogg");
}

public sealed partial class NvInstantActionEvent : InstantActionEvent
{
}
