using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Holosign
{
    [RegisterComponent]
    public sealed class HolosignProjectorComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("signProto", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string SignProto = "HolosignWetFloor";

        /// <summary>
        /// How much signs can be for one holoprojector.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("maxSigns")]
        public int MaxSigns = 10;

        /// <summary>
        /// Time in seconds need to set up hoosign.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("deployTime")]
        public float DeployTime = 0;

        [ViewVariables]
        public readonly List<EntityUid> Childs = new();
    }
}
