using Content.Server.Starshine.Shuttles.Systems;
using Robust.Shared.Utility;

namespace Content.Server.Starshine.Shuttles.Components;

[RegisterComponent, Access(typeof(StationShuttleDock))]
public sealed partial class StationShuttleDockComponent : Component
{
    [DataField]
    public EntityUid Shuttle;

    [DataField(required: true)]
    public string TargetTag;

    [DataField(required: true)]
    public ResPath Path;
}
