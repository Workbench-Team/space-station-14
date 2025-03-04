using Content.Server.Materials;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Materials;

namespace Content.Server.Starshine.PlasmaCutter
{
    /// <summary>
    /// Manages battery recharging with material usage and adjusts storage capacity dynamically.
    /// </summary>
    public sealed class BatteryRechargeSystem : EntitySystem
    {
        [Dependency] private readonly MaterialStorageSystem _materialStorage = default!;
        [Dependency] private readonly BatterySystem _batterySystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<BatteryRechargeComponent, MaterialEntityInsertedEvent>(OnMaterialAmountChanged);
            SubscribeLocalEvent<BatteryRechargeComponent, ChargeChangedEvent>(OnChargeChanged);
        }

        private void OnMaterialAmountChanged(
            EntityUid uid,
            BatteryRechargeComponent component,
            MaterialEntityInsertedEvent args)
        {
            if (!TryComp<MaterialStorageComponent>(uid, out var materialStorage))
                return;

            if (materialStorage.MaterialWhiteList == null)
                return;

            foreach (var fuelType in materialStorage.MaterialWhiteList)
            {
                FuelAddCharge(uid, fuelType);
            }
        }

        private void OnChargeChanged(EntityUid uid, BatteryRechargeComponent component, ChargeChangedEvent args)
        {
            AdjustStorageCapacity(uid, component);
        }

        private void AdjustStorageCapacity(EntityUid uid,
            BatteryRechargeComponent component,
            BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return;

            // Calculate new storage limit based on current charge
            var newCapacity = battery.CurrentCharge >= battery.MaxCharge
                ? 0
                : (int)(component.StorageMaxCapacity * (1 - battery.CurrentCharge / battery.MaxCharge));

            _materialStorage.TryChangeStorageLimit(uid, newCapacity);
        }

        private void FuelAddCharge(EntityUid uid, string fuelType, BatteryRechargeComponent? recharge = null)
        {
            if (!Resolve(uid, ref recharge))
                return;

            var availableMaterial = _materialStorage.GetMaterialAmount(uid, fuelType);
            var chargePerMaterial = availableMaterial * recharge.Multiplier;

            if (_materialStorage.TryChangeMaterialAmount(uid, fuelType, -availableMaterial))
            {
                _batterySystem.TryAddCharge(uid, chargePerMaterial);
            }
        }
    }
}
