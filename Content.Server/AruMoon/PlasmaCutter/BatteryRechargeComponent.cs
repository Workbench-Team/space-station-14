namespace Content.Server.AruMoon.Plasmacutter;

[RegisterComponent]
public sealed partial class BatteryRechargeComponent : Component
{

    /// <summary>
    /// Type of material to be converted into energy
    /// </summary>
    ///
    [DataField("materialType"), ViewVariables(VVAccess.ReadWrite)]
    public List<string>? MaterialType = null;


    /// <summary>
    /// NOT (material.Amount * Multiplier)
    /// This is (material.materialComposition * Multiplier)
    /// 1 plasma sheet = 100 material units
    /// 1 plasma ore = 500 material units
    /// </summary>
    ///
    [DataField("multiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float Multiplier = 1.0f;


    /// <summary>
    /// Max material storage limit
    /// 7500 = 15 plasma ore
    /// </summary>
    [DataField("storageMaxCapacity"), ViewVariables(VVAccess.ReadWrite)]
    public int StorageMaxCapacity = 7500;
}

