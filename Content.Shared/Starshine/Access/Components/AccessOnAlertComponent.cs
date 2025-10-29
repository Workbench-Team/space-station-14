using Content.Shared.Access;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Starshine.Access.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class AccessOnAlertComponent : Component
{
    [DataField]
    public ProtoId<AccessOnAlertSettingsPrototype> SettingsPrototype = "StationAccessOnAlert";

    [ViewVariables]
    public List<HashSet<ProtoId<AccessLevelPrototype>>> AddedAlertAccesses = [];

    [ViewVariables]
    public List<HashSet<ProtoId<AccessLevelPrototype>>> LockedAlertAccesses = [];
}
