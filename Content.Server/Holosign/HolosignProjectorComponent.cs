using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Holosign
{
    [RegisterComponent]
    public sealed partial class HolosignProjectorComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("signProto", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string SignProto = "HolosignWetFloor";

        /// <summary>
        /// How much signs can be for one holoprojector.
        /// </summary>
        [DataField]
        public int MaxSigns = 12;

        [DataField]
        public float DrawRatePerHolo = 1f;

        /// <summary>
        /// Time in seconds need to set up holosign.
        /// </summary>
        [DataField]
        public float DeployTime = 0;

        [ViewVariables]
        public readonly List<EntityUid> Childs = new();
    }
}
