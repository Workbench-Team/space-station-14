using Content.Shared.ActionBlocker;
using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Stunnable;

public abstract class SharedPolicebatonSystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PolicebatonComponent, ItemToggleActivateAttemptEvent>(TryTurnOn);
        SubscribeLocalEvent<PolicebatonComponent, ItemToggleDeactivateAttemptEvent>(TryTurnOff);
    }

    protected virtual void TryTurnOn(Entity<PolicebatonComponent> entity, ref ItemToggleActivateAttemptEvent args)
    {
        if (args.User == null || _actionBlocker.CanComplexInteract(args.User.Value))
            return;

        args.Cancelled = true;
    }

    protected virtual void TryTurnOff(Entity<PolicebatonComponent> entity, ref ItemToggleDeactivateAttemptEvent args)
    {
        if (args.User == null || _actionBlocker.CanComplexInteract(args.User.Value))
            return;

        args.Cancelled = true;
    }
}
