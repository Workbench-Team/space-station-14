using Content.Shared.Roles;

namespace Content.Shared.GameTicking;

public sealed class DepartmentBanEvent : EntityEventArgs
{
    public string Username { get; }
    public DepartmentPrototype Department { get; }
    public DateTimeOffset? Expires { get; }
    public string Reason { get; }


    public DepartmentBanEvent(string username, DateTimeOffset? expires, DepartmentPrototype department, string reason)
    {
        Username = username;
        Department = department;
        Expires = expires;
        Reason = reason;
    }
}
