using Content.Shared.Access.Systems;
using Content.Shared.StationRecords;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Access.Components;

/// <summary>
///     Stores access levels necessary to "use" an entity
///     and allows checking if something or somebody is authorized with these access levels.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed class AccessStorageComponent : Component
{
    /// <summary>
    ///     The set of tags that will automatically deny an allowed check, if any of them are present.
    /// </summary>
    [DataField("denyTags", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<AccessLevelPrototype>))]
    public HashSet<string> DenyTags = new();

    /// <summary>
    ///     List of access lists to check allowed against. For an access check to pass
    ///     there has to be an access list that is a subset of the access in the checking list.
    /// </summary>
    [DataField("access")]
    public List<HashSet<string>> AccessLists = new();

    /// <summary>
    /// A list of valid stationrecordkeys
    /// </summary>
    [DataField("accessKeys")]
    public HashSet<StationRecordKey> AccessKeys = new();

    [DataField("accessLevels", customTypeSerializer: typeof(PrototypeIdListSerializer<AccessLevelPrototype>))]
    public List<string> AccessLevels = new()
    {
        "Armory",
        "Atmospherics",
        "Bar",
        "Brig",
        "Detective",
        "Captain",
        "Cargo",
        "Chapel",
        "Chemistry",
        "ChiefEngineer",
        "ChiefMedicalOfficer",
        "Command",
        "Engineering",
        "External",
        "HeadOfPersonnel",
        "HeadOfSecurity",
        "Hydroponics",
        "Janitor",
        "Kitchen",
        "Maintenance",
        "Medical",
        "Quartermaster",
        "Research",
        "ResearchDirector",
        "Salvage",
        "Security",
        "Service",
        "Theatre",
    };
}

[Serializable, NetSerializable]
public sealed class AccessStorageComponentState : ComponentState
{
    public HashSet<string> DenyTags;
    public List<HashSet<string>> AccessLists;
    public HashSet<StationRecordKey> AccessKeys;

    public AccessStorageComponentState(HashSet<string> denyTags, List<HashSet<string>> accessLists, HashSet<StationRecordKey> accessKeys)
    {
        DenyTags = denyTags;
        AccessLists = accessLists;
        AccessKeys = accessKeys;
    }
}
