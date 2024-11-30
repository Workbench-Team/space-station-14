using System.Threading;

namespace Content.Server.Starshine.Carrying.Components
{
    [RegisterComponent]
    public sealed partial class CarriableComponent : Component
    {
        public CancellationTokenSource? CancelToken;
        /// <summary>
        ///     Number of free hands required
        ///     to carry the entity
        /// </summary>
        [DataField]
        public int FreeHandsRequired = 2;

        /// <summary>
        ///     The base duration (In Seconds) of how long it should take to pick up this entity
        ///     before Contests are considered.
        /// </summary>
        [DataField]
        public float PickupDuration = 4;
    }
}
