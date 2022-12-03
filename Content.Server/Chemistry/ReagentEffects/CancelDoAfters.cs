using System.Text.Json.Serialization;
using Content.Server.DoAfter;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using JetBrains.Annotations;

namespace Content.Server.Chemistry.ReagentEffects
{
    /// <summary>
    /// Cancel any actions in progress
    /// </summary>
    [UsedImplicitly]
    public sealed class CancelDoAfters : ReagentEffect
    {
        /// <summary>
        /// Should DoAfter be cancelled if it breaks after damage
        /// </summary>
        [DataField("damageLike")]
        [JsonPropertyName("damageLike")]
        public bool DamageLike = true;
        
        /// <summary>
        /// Should DoAfter be cancelled if it breaks after user movement
        /// </summary>
        [DataField("movementLike")]
        [JsonPropertyName("movementLike")]
        public bool MovementLike = true;

        /// <summary>
        /// Should DoAfter be cancelled if it breaks after stun
        /// </summary>
        [DataField("stunLike")]
        [JsonPropertyName("stunLike")]
        public bool StunLike = true;

        /// <summary>
        /// How much damage it deals for DoAfter
        /// </summary>
        [DataField("damage")]
        public float Damage = 5f;

        /// <summary>
        /// How much damage it moves for DoAfter
        /// </summary>
        [DataField("movement")]
        public float Movement = 1f;

        public override void Effect(ReagentEffectArgs args)
        {
            if (!args.EntityManager.TryGetComponent(args.SolutionEntity, out DoAfterComponent? doAfterComp))
            { return; }

            foreach (var (doAfter, _) in doAfterComp.DoAfters)
            {
                if (
                    (DamageLike && doAfter.EventArgs.BreakOnDamage && Damage > doAfter.EventArgs.DamageThreshold) ||
                    (MovementLike && doAfter.EventArgs.BreakOnUserMove && Movement > doAfter.EventArgs.MovementThreshold) ||
                    (StunLike && doAfter.EventArgs.BreakOnStun)
                )
                {
                    doAfter.Cancel();
                }
            }
        }
    }
}
