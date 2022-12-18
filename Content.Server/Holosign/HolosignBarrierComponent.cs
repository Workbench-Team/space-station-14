using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Holosign
{
    [RegisterComponent]
    public sealed class HolosignBarrierComponent : Component
    {
        /// <summary>
        /// Solution to notify Holoprojector about holobarrier remove.
        /// </summary>
        [ViewVariables]
        public EntityUid Parent = new();
    }
}
