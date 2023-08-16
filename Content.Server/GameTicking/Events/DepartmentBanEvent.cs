using Content.Shared.Database;
using Content.Shared.Roles;

namespace Content.Shared.GameTicking;

public sealed class DepartmentBanEvent : EntityEventArgs
{
    public string Username { get; }
    public DepartmentPrototype Department { get; }
    public DateTimeOffset? Expires { get; }
    public string Reason { get; }
    public NoteSeverity Severity { get; }
    public string AdminUsername { get; }


    public DepartmentBanEvent(string username, DateTimeOffset? expires, DepartmentPrototype department, string reason, NoteSeverity severity, string adminusername)
    {
        Username = username;
        Department = department;
        Expires = expires;
        Reason = reason;
        Severity = severity;
        AdminUsername = adminusername;
    }
}
