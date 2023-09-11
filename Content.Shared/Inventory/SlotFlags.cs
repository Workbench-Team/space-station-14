using Robust.Shared.Serialization;

namespace Content.Shared.Inventory;

/// <summary>
///     Defines what slot types an item can fit into.
/// </summary>
[Serializable, NetSerializable]
[Flags]
public enum SlotFlags
{
    NONE = 0,
    PREVENTEQUIP = 1 << 0,
    SOCKS = 1 << 1,
    HEAD = 1 << 2,
    EYES = 1 << 3,
    EARS = 1 << 4,
    MASK = 1 << 5,
    OUTERCLOTHING = 1 << 6,
    INNERCLOTHING = 1 << 7,
    NECK = 1 << 8,
    BACK = 1 << 9,
    BELT = 1 << 10,
    GLOVES = 1 << 11,
    IDCARD = 1 << 12,
    POCKET = 1 << 13,
    LEGS = 1 << 14,
    FEET = 1 << 15,
    SUITSTORAGE = 1 << 16,
    All = ~NONE,
}
