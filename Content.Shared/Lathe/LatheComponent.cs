using Content.Shared.Construction.Prototypes;
using Content.Shared.Research.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared.Construction.Prototypes;

namespace Content.Shared.Lathe
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class LatheComponent : Component
    {
        /// <summary>
        /// All of the recipes that the lathe has by default
        /// </summary>
        [DataField]
        public List<ProtoId<LatheRecipePrototype>> StaticRecipes = new();

        /// <summary>
        /// All of the recipes that the lathe is capable of researching
        /// </summary>
        [DataField]
        public List<ProtoId<LatheRecipePrototype>> DynamicRecipes = new();

        /// <summary>
        /// The lathe's construction queue
        /// </summary>
        [DataField]
        public List<LatheRecipePrototype> Queue = new();

        /// <summary>
        /// The sound that plays when the lathe is producing an item, if any
        /// </summary>
        [DataField]
        public SoundSpecifier? ProducingSound;

        /// <summary>
        /// The sound that plays when a player trying to produce an item without access to the lathe
        /// </summary>
        [DataField("soundError")]
        public SoundSpecifier? ErrorSound;

        #region Visualizer info
        [DataField]
        public string? IdleState;

        [DataField]
        public string? RunningState;
        #endregion

        /// <summary>
        /// The recipe the lathe is currently producing
        /// </summary>
        [ViewVariables]
        public LatheRecipePrototype? CurrentRecipe;

        #region MachineUpgrading
        /// <summary>
        /// A modifier that changes how long it takes to print a recipe
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float TimeMultiplier = 1;

        /// <summary>
        /// A modifier that changes how much of a material is needed to print a recipe
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float MaterialUseMultiplier = 1;

        public const float DefaultPartRatingMaterialUseMultiplier = 0.85f;
        #endregion

        [DataField("machinePartMinTemp", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string MachinePartMinTemp = "Capacitor";
    }

    public sealed class LatheGetRecipesEvent : EntityEventArgs
    {
        public readonly EntityUid Lathe;

        public bool getUnavailable;

        public List<ProtoId<LatheRecipePrototype>> Recipes = new();

        public LatheGetRecipesEvent(EntityUid lathe, bool forced)
        {
            Lathe = lathe;
            getUnavailable = forced;
        }
    }

    /// <summary>
    /// Event raised on a lathe when it starts producing a recipe.
    /// </summary>
    [ByRefEvent]
    public readonly record struct LatheStartPrintingEvent(LatheRecipePrototype Recipe);
}
