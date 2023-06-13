using Content.Shared.Roles;

namespace Content.Shared.GameTicking;

public sealed class JobBanEvent : EntityEventArgs
{
    public string Username { get; }
    public JobPrototype Job { get; }
    public DateTimeOffset? Expires { get; }
    public string Reason { get; }


    public JobBanEvent(string username, DateTimeOffset? expires, JobPrototype job, string reason)
    {
        Username = username;
        Job = job;
        Expires = expires;
        Reason = reason;
    }
}
