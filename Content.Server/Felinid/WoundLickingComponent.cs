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
        /// How long it requires to lick wounds
        /// </summary>
        [DataField("delay")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float Delay { get; set; } = 5f;

        /// <summary>
        /// If true, then wound-licking can be applied only on other entities
        /// </summary>
        [DataField("canSelfApply")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool CanSelfApply { get; set; } = false;


        /// <summary>
        /// Which diseases can be caused because of wound-licking
        /// </summary>
        [DataField("possibleDiseases")]
        public List<String> PossibleDiseases { get; set; } = new(){
            "Plague"
        };
    }
}
