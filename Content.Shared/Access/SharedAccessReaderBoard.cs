using Robust.Shared.Serialization;

namespace Content.Shared.Access.Systems
{
    /// <summary>
    /// Key representing which <see cref="BoundUserInterface"/> is currently open.
    /// Useful when there are multiple UI for an object. Here it's future-proofing only.
    /// </summary>
    [Serializable, NetSerializable]
    public enum AccessStorageUiKey : byte
    {
        Key,
    }

    /// <summary>
    /// Represents an <see cref="AgentIDCardComponent"/> state that can be sent to the client
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class AccessStorageBoundUserInterfaceState : BoundUserInterfaceState
    {
        public readonly string[]? AccessList;
        public readonly string[]? DenyTags;

        public AccessStorageBoundUserInterfaceState(string[]? accessList, string[]? denyTags)
        {
            AccessList = accessList;
            DenyTags = denyTags;
        }
    }

    [Serializable, NetSerializable]
    public sealed class WriteToTargetAccessStorageMessage : BoundUserInterfaceMessage
    {
        public readonly List<string> AccessList;
        public readonly List<string> DenyTags;

        public WriteToTargetAccessStorageMessage(List<string> accessList, List<string> denyTags)
        {
            AccessList = accessList;
            DenyTags = denyTags;
        }
    }
}
