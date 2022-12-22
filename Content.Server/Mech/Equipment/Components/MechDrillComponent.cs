using System.Threading;
using Robust.Shared.Audio;
using Robust.Shared.Containers;

namespace Content.Server.Mech.Equipment.Components;

/// <summary>
/// A piece of mech equipment that grabs entities and stores them
/// inside of a container so large objects can be moved.
/// </summary>
[RegisterComponent]
public sealed class MechDrillComponent : Component
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

    public IPlayingAudioStream? AudioStream;

    public CancellationTokenSource? Token;
}

