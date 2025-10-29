using Robust.Shared.Configuration;

namespace Content.Shared.Starshine.CCVar;

[CVarDefs]
public sealed partial class CCVars
{
    /// <summary>
    /// Send station goal on round start.
    /// </summary>
    public static readonly CVarDef<bool> StationGoal =
        CVarDef.Create("game.station_goal", true, CVar.SERVERONLY);
}
