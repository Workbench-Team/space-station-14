using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions; // Starshine-guidebook-url-start
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Input;
using Robust.Shared.Utility;
using Content.Client.UserInterface.ControlExtensions;

namespace Content.Client.Guidebook.RichText;

[UsedImplicitly]
public sealed class TextLinkTag : IMarkupTagHandler
{
    [Dependency] private readonly IUriOpener _uri = default!; // Starshine-guidebook-url
    public static Color LinkColor => Color.CornflowerBlue;

    public string Name => "textlink";

    /// <inheritdoc/>
    public bool TryCreateControl(MarkupNode node, [NotNullWhen(true)] out Control? control)
    {
        if (!node.Value.TryGetString(out var text)
            || !node.Attributes.TryGetValue("link", out var linkParameter)
            || !linkParameter.TryGetString(out var link))
        {
            control = null;
            return false;
        }

        var label = new Label();
        label.Text = text;

        label.MouseFilter = Control.MouseFilterMode.Stop;
        label.FontColorOverride = LinkColor;
        label.DefaultCursorShape = Control.CursorShape.Hand;

        label.OnMouseEntered += _ => label.FontColorOverride = Color.LightSkyBlue;
        label.OnMouseExited += _ => label.FontColorOverride = Color.CornflowerBlue;
        label.OnKeyBindDown += args => OnKeybindDown(args, link, label);

        control = label;
        return true;
    }

    private void OnKeybindDown(GUIBoundKeyEventArgs args, string link, Control? control)
    {
        if (args.Function != EngineKeyFunctions.UIClick)
            return;

        if (control == null)
            return;

        if (control.TryGetParentHandler<ILinkClickHandler>(out var handler))
        {
            handler.HandleClick(link);
            TryOpenUrl(link); // Starshine-guidebook-url
        }
        else
            Logger.Warning("Warning! No valid ILinkClickHandler found.");
    }

    #region StarshineUrl
    private bool IsValidUrl(string url) // Starshine-guidebook-url-start
    {
        const string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        var rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return rgx.IsMatch(url);
    }

    public bool TryOpenUrl(string url)
    {
        if (!IsValidUrl(url))
            return false;
        _uri.OpenUri(url);
        return true;
    } // Starshine-guidebook-url-end
    #endregion
}

public interface ILinkClickHandler
{
    public void HandleClick(string link);
}
