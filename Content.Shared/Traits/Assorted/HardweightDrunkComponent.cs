using Robust.Shared.GameStates;
using Content.Shared.Drunk;

namespace Content.Shared.Traits.Assorted;

/// <summary>
/// Used for the Hardweight trait. DrunkSystem will check for this component and modify the boozePower accordingly if it finds it.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedDrunkSystem))]
public sealed partial class HardweightDrunkComponent : Component
{
    [DataField("boozeHardStrengthMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float BoozeHardStrengthMultiplier = 0.5f;
}
