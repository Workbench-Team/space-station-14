using System.Text.RegularExpressions;
using Content.Shared.Speech.Components;
using Robust.Shared.Random; // Corvax-Localization

namespace Content.Shared.Speech.EntitySystems;

public sealed partial class LizardAccentSystem : RelayAccentSystem<LizardAccentComponent>
{
    private static readonly Regex RegexLowerS = new("s+");
    private static readonly Regex RegexUpperS = new("S+");
    private static readonly Regex RegexInternalX = new(@"(\w)x");
    private static readonly Regex RegexLowerEndX = new(@"\bx([\-|r|R]|\b)");
    private static readonly Regex RegexUpperEndX = new(@"\bX([\-|r|R]|\b)");

    // Corvax-Localization-Start
    private static readonly Regex RegexLowerC = new ("с+");
    private static readonly Regex RegexUpperC = new ("С+");
    private static readonly Regex RegexLowerZ = new ("з+");
    private static readonly Regex RegexUpperZ = new ("З+");
    private static readonly Regex RegexLowerSh = new ("ш+");
    private static readonly Regex RegexUpperSh = new ("Ш+");
    private static readonly Regex RegexLowerCh = new ("ч+");
    private static readonly Regex RegexUpperCh = new ("Ч+");
    private static readonly List<string> ReplacementsSs = new List<string> { "сс", "ссс" };
    private static readonly List<string> ReplacementsSsUpper = new List<string> { "СС", "ССС" };
    private static readonly List<string> ReplacementsSh = new List<string> { "шш", "шшш" };
    private static readonly List<string> ReplacementsShUpper = new List<string> { "ШШ", "ШШШ" };
    private static readonly List<string> ReplacementsCh = new List<string> { "щщ", "щщщ" };
    private static readonly List<string> ReplacementsChUpper = new List<string> { "ЩЩ", "ЩЩЩ" };
    // Corvax-Localization-End

    [Dependency] private readonly IRobustRandom _random = default!; // Corvax-Localization

    public override string Accentuate(string message, Entity<LizardAccentComponent>? ent = null)
    {
        // hissss
        message = RegexLowerS.Replace(message, "sss");
        // hiSSS
        message = RegexUpperS.Replace(message, "SSS");
        // ekssit
        message = RegexInternalX.Replace(message, "$1kss");
        // ecks
        message = RegexLowerEndX.Replace(message, "ecks$1");
        // eckS
        message = RegexUpperEndX.Replace(message, "ECKS$1");

        // Corvax-Localization-Start
        message = RegexLowerC.Replace(message, _random.Pick(ReplacementsSs));
        message = RegexUpperC.Replace(message, _random.Pick(ReplacementsSsUpper));
        message = RegexLowerZ.Replace(message, _random.Pick(ReplacementsSs));       // для "з+" используются те же замены, что и для "с+"
        message = RegexUpperZ.Replace(message, _random.Pick(ReplacementsSsUpper)); // для "З+" используются те же замены, что и для "С+"
        message = RegexLowerSh.Replace(message, _random.Pick(ReplacementsSh));
        message = RegexUpperSh.Replace(message, _random.Pick(ReplacementsShUpper));
        message = RegexLowerCh.Replace(message, _random.Pick(ReplacementsCh));
        message = RegexUpperCh.Replace(message, _random.Pick(ReplacementsChUpper));
        // Corvax-Localization-End

        return message;
    }
}
