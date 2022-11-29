using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared.Actions.ActionTypes;
using Robust.Shared.Utility;

namespace Content.Server.Felinid
{
    [RegisterComponent]
    [Access(typeof(WoundLickingSystem))]
    public sealed class WoundLickingComponent : Component
    {
        /// <summary>
        /// How frequent wound-licking will cause diseases? Scales with amount of reduced bleeding
        /// </summary>
        [DataField("diseaseChance")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float DiseaseChance { get; set; } = 0.1f;

        /// <summary>
        /// If true, then wound-licking can be applied only on entities with this component (Felinids in e.x.)
        /// </summary>
        [DataField("onlyForWoundLickers")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool OnlyForWoundLickers { get; set; } = false;


        /// <summary>
        /// Which diseases can be caused because of wound-licking
        /// </summary>
        [DataField("possibleDiseases")]
        public List<String> PossibleDiseases { get; set; } = new(){
            "Plague"
        };

        /// <summary>
        /// Action that will be used to activate wound-licking
        /// </summary>
        [DataField("action")]
        public InstantAction Action = new()
        {
            Icon = new SpriteSpecifier.Texture(new ResourcePath("Mobs/Species/Human/organs.rsi/tongue.png")),
            DisplayName = "action-name-lick-wounds",
            Description = "action-desc-lick-wounds",
            Priority = -1,
            Event = new WoundLickingActionEvent(),
        };
    }
}
