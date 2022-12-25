namespace Content.Server.Drone.Components
{
    [RegisterComponent]
    public sealed class DroneComponent : Component
    {
        public const float DefaultInteractionBlockRange = 2.15f;

        [ViewVariables(VVAccess.ReadWrite), DataField("interactionBlockRange")]
        public float InteractionBlockRange { get; set; } = DefaultInteractionBlockRange;
    }
}
