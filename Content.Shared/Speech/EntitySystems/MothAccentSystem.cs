using System.Text.RegularExpressions;
using Content.Shared.Speech.Components;

namespace Content.Shared.Speech.EntitySystems;

public sealed partial class MothAccentSystem : RelayAccentSystem<MothAccentComponent>
{
    private static readonly Regex RegexLowerBuzz = new("z{1,3}");
    private static readonly Regex RegexUpperBuzz = new("Z{1,3}");

    // Starshine-Localization-Start
    private static readonly Regex RegexLowerRuBuzz = new("з{1,3}");
    private static readonly Regex RegexUpperRuBuzz = new("З{1,3}");
    // Starshine-Localization-End

    public override string Accentuate(string message, Entity<MothAccentComponent>? ent = null)
    {
        // buzzz
        message = RegexLowerBuzz.Replace(message, "zzz");
        // buZZZ
        message = RegexUpperBuzz.Replace(message, "ZZZ");

        // Starshine-Localization-Start
        message = RegexLowerRuBuzz.Replace(message, "ззз");
        message = RegexUpperRuBuzz.Replace(message, "ЗЗЗ");
        // Starshine-Localization-End

        return message;
    }
}
