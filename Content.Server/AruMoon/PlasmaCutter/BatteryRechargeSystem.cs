using Content.Server.Popups;
using Content.Server.Materials;
using Content.Shared.Materials;
using Content.Server.Power.EntitySystems;
using Content.Server.Power.Components;
using Content.Server.Power.Generator;

namespace Content.Server.AruMoon.Plasmacutter
{

    /// <summary>
    /// This CODE FULL OF SHICODE!!!
    /// <see cref="BatteryRechargeComponent"/>
    /// </summary>
    public sealed class BatteryRechargeSystem : EntitySystem
    {
        [Dependency] private readonly PopupSystem _popup = default!;
        [Dependency] private readonly MaterialStorageSystem _materialStorage = default!;
        [Dependency] private readonly BatterySystem _batterySystem = default!;



        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<BatteryRechargeComponent, MaterialEntityInsertedEvent>(OnMaterialAmountChanged);
        }

        private void OnMaterialAmountChanged(EntityUid uid, BatteryRechargeComponent component, ref MaterialEntityInsertedEvent args)
        {
            if (component.MaterialType == null)
                return;

            foreach (var fuelType in component.MaterialType)
            {
                FuelAddCharge(uid, fuelType);
            }
        }

        private void FuelAddCharge(
            EntityUid uid,
            string fuelType,
            BatteryRechargeComponent? recharge = null)
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
