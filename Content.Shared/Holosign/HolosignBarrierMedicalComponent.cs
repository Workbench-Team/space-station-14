using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.GameStates;

namespace Content.Shared.Holosign
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class HolosignBarrierMedicalComponent : Component
    {
        /// <summary>
        /// Allow all personel pass throught barrier.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool AllAccess = false;
    }
}
