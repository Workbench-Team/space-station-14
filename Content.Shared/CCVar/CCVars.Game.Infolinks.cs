using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Link to Discord server to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksDiscord =
        CVarDef.Create("infolinks.discord", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to website to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksForum =
        CVarDef.Create("infolinks.forum", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Link to GitLab page to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksGitLab =
        CVarDef.Create("infolinks.gitlab", "https://git.arumoon.ru/Workbench-Team/space-station-14/-/tree/arumoon-server", CVar.SERVER | CVar.REPLICATED);
    // HARDCODE: Default value specified because DevInfoBanner window uses client side config instead of server's config

    /// <summary>
    ///     Link to website to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksWebsite =
        CVarDef.Create("infolinks.website", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to wiki to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksWiki =
        CVarDef.Create("infolinks.wiki", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to Patreon. Not shown in the launcher currently.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksPatreon =
        CVarDef.Create("infolinks.patreon", "", CVar.SERVER | CVar.REPLICATED);

    #region Starshine

    /// <summary>
    ///     Link to the bug report form.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksBugReport =
        CVarDef.Create("infolinks.bug_report", "https://git.arumoon.ru/Workbench-Team/space-station-14/-/issues/new", CVar.SERVER | CVar.REPLICATED);
    // HARDCODE: Default value specified because DevInfoBanner window uses client side config instead of server's config

    /// <summary>
    /// Link to wiki page with roles description in Rules menu.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksRoles =
        CVarDef.Create("infolinks.roles", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Link to wiki page with space laws in Rules menu.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksLaws =
        CVarDef.Create("infolinks.laws", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Donation link.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksDonate =
        CVarDef.Create("infolinks.donate", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Link to shop.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksShop =
        CVarDef.Create("infolinks.shop", "", CVar.SERVER | CVar.REPLICATED);

    #endregion

    /// <summary>
    ///     Link to site handling ban appeals. Shown in ban disconnect messages.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksAppeal =
        CVarDef.Create("infolinks.appeal", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to Telegram channel to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksTelegram =
        CVarDef.Create("infolinks.telegram", "", CVar.SERVER | CVar.REPLICATED);
}
