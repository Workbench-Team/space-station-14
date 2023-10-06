using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server.Weapons.Melee.Chainsaw;

[RegisterComponent]
internal sealed partial class ChainsawComponent : Component
{

    public bool Hacked = false;

    public bool Activated = false;

    [DataField("isSharp")]
    public bool IsSharp = true;

    /// <summary>
    ///     Does this become hidden when deactivated
    /// </summary>
    [DataField("secret")]
    public bool Secret { get; set; } = false;


    [DataField("activateSound")]
    public SoundSpecifier ActivateSound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/chainsaw_start.ogg");

    [DataField("deActivateSound")]
    public SoundSpecifier DeActivateSound { get; set; } = new SoundPathSpecifier("/Audio/Weapons/chainsaw_stop.ogg");

    [DataField("onHitOn")]
    public SoundSpecifier OnHitOn { get; set; } = new SoundPathSpecifier("/Audio/Weapons/chainsawhit.ogg");

    [DataField("onHitOff")]
    public SoundSpecifier OnHitOff { get; set; } = new SoundPathSpecifier("/Audio/Weapons/chainsawhit.ogg");


    [DataField("litDamageBonus")]
    public DamageSpecifier LitDamageBonus = new();

    [DataField("litDisarmMalus")]
    public float LitDisarmMalus = 0.6f;
}

[ByRefEvent]
public readonly record struct ChainsawActivatedEvent();

[ByRefEvent]
public readonly record struct ChainsawDeactivatedEvent();
