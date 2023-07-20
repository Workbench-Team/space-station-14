using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions;

/// <summary>
///     Converts frezon into miasma when the two come into contact. Does not occur at very high temperatures.
/// </summary>
[UsedImplicitly]
public sealed class MiasmicSubsumationReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem)
    {
        var initialHyperNoblium = mixture.GetMoles(Gas.HyperNoblium);
        if (initialHyperNoblium >= 5.0f && mixture.Temperature > 20f)
            return ReactionResult.NoReaction;

        var initialMiasma = mixture.GetMoles(Gas.Miasma);
        var initialFrezon = mixture.GetMoles(Gas.Frezon);

        var convert = Math.Min(Math.Min(initialFrezon, initialMiasma), Atmospherics.MiasmicSubsumationMaxConversionRate);

        mixture.AdjustMoles(Gas.Miasma, convert);
        mixture.AdjustMoles(Gas.Frezon, -convert);

        return ReactionResult.Reacting;
    }
}
