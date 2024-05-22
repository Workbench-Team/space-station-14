using Content.Server.Stunnable.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Examine;
using Content.Shared.Item;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Stunnable;

namespace Content.Server.Stunnable.Systems
{
    public sealed class PolicebatonSystem : SharedPolicebatonSystem
    {
        [Dependency] private readonly SharedItemSystem _item = default!;
        [Dependency] private readonly SharedItemToggleSystem _itemToggle = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<PolicebatonComponent, ExaminedEvent>(OnExamined);
            SubscribeLocalEvent<PolicebatonComponent, StaminaDamageOnHitAttemptEvent>(OnStaminaHitAttempt);
            SubscribeLocalEvent<PolicebatonComponent, ItemToggledEvent>(ToggleDone);
        }

        private void OnStaminaHitAttempt(Entity<PolicebatonComponent> entity, ref StaminaDamageOnHitAttemptEvent args)
        {
            if (!_itemToggle.IsActivated(entity.Owner))
            {
                args.Cancelled = true;
                return;
            }
        }

        private void OnExamined(Entity<PolicebatonComponent> entity, ref ExaminedEvent args)
        {
            var onMsg = _itemToggle.IsActivated(entity.Owner)
                ? Loc.GetString("comp-policebaton-telescopic-examined-on")
                : Loc.GetString("comp-policebaton-telescopic-examined-off");
            args.PushMarkup(onMsg);
        }

        private void ToggleDone(Entity<PolicebatonComponent> entity, ref ItemToggledEvent args)
        {
            if (!TryComp<ItemComponent>(entity, out var item))
                return;

            _item.SetHeldPrefix(entity.Owner, args.Activated ? "on" : "off", component: item);
        }
    }
}
