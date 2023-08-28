namespace Content.Server.Drone.Components
{
    [RegisterComponent]
    public sealed partial class DroneComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite), DataField("interactionBlockRange")]
        public float InteractionBlockRange { get; set; } = 2.15f;
    }
}
