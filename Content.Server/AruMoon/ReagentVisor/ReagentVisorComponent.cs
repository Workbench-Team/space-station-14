namespace Content.Server.ReagentVisor
{
    [RegisterComponent]
    [Access(typeof(ReagentVisorSystem))]
    public sealed class ReagentVisorComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("enabled")]
        public bool Enabled { get; set; } = true;
    }
}
