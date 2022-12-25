namespace Content.Server.Drone.Components
{
    [RegisterComponent]
    public sealed class DroneComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite), DataField("interactionBlockRange")]
        public float InteractionBlockRange { get; set; } = 2.15f;
    }
}
