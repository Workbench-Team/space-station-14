using Content.Shared.Database;

namespace Content.Shared.GameTicking;

public sealed class BanEvent : EntityEventArgs
{
    public string Username { get; }
    public DateTimeOffset? Expires { get; }
    public string Reason { get; }
    public NoteSeverity Severity { get; }
    public string AdminUsername { get; }

    public BanEvent(string username, DateTimeOffset? expires, string reason, NoteSeverity severity, string adminusername)
    {
        Username = username;
        Expires = expires;
        Reason = reason;
        Severity = severity;
        AdminUsername = adminusername;
    }
}
