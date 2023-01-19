namespace Content.Server.Chemistry.Components.SolutionManager
{
    [RegisterComponent]
    public sealed class ExaminableSolutionComponent: Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("solution")]
        public string Solution { get; set; } = "default";

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("allowShowQuantity")]
        public bool AllowShowQuantity { get; set; } = true;
    }
}
