using Content.Shared.Access;
using Robust.Shared.Prototypes;

namespace Content.Shared.Starshine.Access
{
    [Prototype]
    public sealed class AccessOnAlertSettingsPrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField]
        public Dictionary<string, List<HashSet<ProtoId<AccessLevelPrototype>>>> AlertAccessMappings = new();
    }
}
