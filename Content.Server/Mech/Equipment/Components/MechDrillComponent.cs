using System.Threading;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Containers;

namespace Content.Server.Mech.Equipment.Components;

/// <summary>
/// A piece of mech equipment that grabs entities and stores them
/// inside of a container so large objects can be moved.
/// </summary>
[RegisterComponent]
public sealed partial class MechDrillComponent : Component
{
    /// <summary>
    /// The change in energy after each drill.
    /// </summary>
    [DataField("drillEnergyDelta")]
    public float DrillEnergyDelta = -10;

    /// <summary>
    /// How long does it take to grab something?
    /// </summary>
    [DataField("drillDelay")]
    public float DrillDelay = 2.5f;

    /// <summary>
    /// The sound played when a mech is drilling something
    /// </summary>
    [DataField("drillSound")]
    public SoundSpecifier DrillSound = new SoundPathSpecifier("/Audio/Mecha/sound_mecha_hydraulic.ogg");

    public AudioComponent? AudioStream;

    public CancellationTokenSource? Token;
}

