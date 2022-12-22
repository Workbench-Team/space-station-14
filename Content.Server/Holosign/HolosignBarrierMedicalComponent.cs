using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Holosign
{
    [RegisterComponent]
    public sealed class HolosignBarrierMedicalComponent : Component
    {
        /// <summary>
        /// Allow all personel pass throught barrier.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool AllAccess = false;
    }
}
