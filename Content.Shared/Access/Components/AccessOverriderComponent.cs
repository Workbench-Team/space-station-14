using Content.Shared.Access.Systems;
using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Access.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedAccessOverriderSystem))]
public sealed partial class AccessOverriderComponent : Component
{
    public static string PrivilegedIdCardSlotId = "AccessOverrider-privilegedId";

    [DataField]
    public ItemSlot PrivilegedIdSlot = new();

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public SoundSpecifier? DenialSound;

    public EntityUid TargetAccessReaderId = new();

    [Serializable, NetSerializable]
    public sealed class WriteToTargetAccessReaderIdMessage : BoundUserInterfaceMessage
    {
        public readonly List<ProtoId<AccessLevelPrototype>> AccessList;

        public WriteToTargetAccessReaderIdMessage(List<ProtoId<AccessLevelPrototype>> accessList)
        {
            AccessList = accessList;
        }
    }

    #region Starshine-AccessOnAlert

    [Serializable, NetSerializable]
    public sealed class AlertAccessToggledMessage : BoundUserInterfaceMessage
    {
        public readonly bool AlertAccessEnabled;

        public AlertAccessToggledMessage(bool alertAccessEnabled)
        {
            AlertAccessEnabled = alertAccessEnabled;
        }
    }

    [DataField]
    public List<ProtoId<AccessLevelPrototype>> AlertAccessRequired = [];

    #endregion

    [DataField, AutoNetworkedField]
    public List<ProtoId<AccessLevelPrototype>> AccessLevels = new();

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float DoAfter;

    [Serializable, NetSerializable]
    public sealed class AccessOverriderBoundUserInterfaceState : BoundUserInterfaceState
    {
        public readonly string TargetLabel;
        public readonly Color TargetLabelColor;
        public readonly string PrivilegedIdName;
        public readonly bool IsPrivilegedIdPresent;
        public readonly bool IsPrivilegedIdAuthorized;
        public readonly bool HasAlertAccess; // Starshine-AccessOnAlert
        public readonly bool HasRequiredAlertAccess; // Starshine-AccessOnAlert
        public readonly ProtoId<AccessLevelPrototype>[]? AlertAccessRequired; // Starshine-AccessOnAlert
        public readonly ProtoId<AccessLevelPrototype>[]? AddedAlertAccess; // Starshine-AccessOnAlert
        public readonly ProtoId<AccessLevelPrototype>[]? LockedAlertAccess; // Starshine-AccessOnAlert
        public readonly ProtoId<AccessLevelPrototype>[]? TargetAccessReaderIdAccessList;
        public readonly ProtoId<AccessLevelPrototype>[]? AllowedModifyAccessList;
        public readonly ProtoId<AccessLevelPrototype>[]? MissingPrivilegesList;

        public AccessOverriderBoundUserInterfaceState(bool isPrivilegedIdPresent,
            bool isPrivilegedIdAuthorized,
            bool hasAlertAccess, // Starshine-AccessOnAlert
            bool hasRequiredAlertAccess, // Starshine-AccessOnAlert
            ProtoId<AccessLevelPrototype>[]? alertAccessRequired, // Starshine-AccessOnAlert
            ProtoId<AccessLevelPrototype>[]? addedAlertAccess, // Starshine-AccessOnAlert
            ProtoId<AccessLevelPrototype>[]? lockedAlertAccess, // Starshine-AccessOnAlert
            ProtoId<AccessLevelPrototype>[]? targetAccessReaderIdAccessList,
            ProtoId<AccessLevelPrototype>[]? allowedModifyAccessList,
            ProtoId<AccessLevelPrototype>[]? missingPrivilegesList,
            string privilegedIdName,
            string targetLabel,
            Color targetLabelColor)
        {
            IsPrivilegedIdPresent = isPrivilegedIdPresent;
            IsPrivilegedIdAuthorized = isPrivilegedIdAuthorized;
            HasAlertAccess = hasAlertAccess; // Starshine-AccessOnAlert
            HasRequiredAlertAccess = hasRequiredAlertAccess; // Starshine-AccessOnAlert
            AlertAccessRequired = alertAccessRequired; // Starshine-AccessOnAlert
            AddedAlertAccess = addedAlertAccess; // Starshine-AccessOnAlert
            LockedAlertAccess = lockedAlertAccess; // Starshine-AccessOnAlert
            TargetAccessReaderIdAccessList = targetAccessReaderIdAccessList;
            AllowedModifyAccessList = allowedModifyAccessList;
            MissingPrivilegesList = missingPrivilegesList;
            PrivilegedIdName = privilegedIdName;
            TargetLabel = targetLabel;
            TargetLabelColor = targetLabelColor;
        }
    }

    [Serializable, NetSerializable]
    public enum AccessOverriderUiKey : byte
    {
        Key,
    }
}

#region Starshine-AccessOnAlert
public sealed class AccessOverriderValidateModifyEvent : CancellableEntityEventArgs
{
    public EntityUid TargetReader { get; }
    public List<ProtoId<AccessLevelPrototype>> ProposedAccess { get; }

    public string? CancelReason { get; set; }

    public AccessOverriderValidateModifyEvent(EntityUid targetReader, List<ProtoId<AccessLevelPrototype>> proposedAccess)
    {
        TargetReader = targetReader;
        ProposedAccess = proposedAccess;
    }
}
#endregion
