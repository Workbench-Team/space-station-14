using Content.Shared.Access.Systems;
using Content.Shared.StationRecords;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Access.Components;

/// <summary>
///     Stores access levels necessary to "use" an entity
///     and allows checking if something or somebody is authorized with these access levels.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed class AccessReaderBoardComponent : Component
{
    /// <summary>
    /// Whether or not the accessreader is enabled.
    /// If not, it will always let people through.
    /// </summary>
    [DataField("enabled")]
    public bool Enabled = true;

    /// <summary>
    /// The slot the board is stored in.
    /// </summary>
    [ViewVariables]
    public Container BoardContainer = default!;

    [ViewVariables]
    public readonly string BoardContainerId = "board";
}

[Serializable, NetSerializable]
public sealed class AccessReaderBoardComponentState : ComponentState
{
    public bool Enabled;

    public AccessReaderBoardComponentState(bool enabled)
    {
        Enabled = enabled;
    }
}

