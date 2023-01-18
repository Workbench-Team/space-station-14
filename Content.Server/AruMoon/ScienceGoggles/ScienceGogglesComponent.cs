using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared.Actions.ActionTypes;
using Robust.Shared.Utility;
using System.Threading;

namespace Content.Server.ScienceGoggles
{
    [RegisterComponent]
    [Access(typeof(ScienceGogglesSystem))]
    public sealed class ScienceGogglesComponent : Component
    {
    }
}
