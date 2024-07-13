using Content.Client.Overlays;
using Content.Shared.Starshine.Eye.NightVision.Components;
using Content.Shared.Inventory.Events;
using Robust.Client.Graphics;

namespace Content.Client.Starshine.Eye.NightVision;

public sealed class NightVisionSystem : EquipmentHudSystem<NightVisionComponent>
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly ILightManager _lightManager = default!;


    private NightVisionOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new NightVisionOverlay(Color.Green);
    }

    protected override void UpdateInternal(RefreshEquipmentHudEvent<NightVisionComponent> component)
    {
        base.UpdateInternal(component);

        foreach (var comp in component.Components)
        {
            _overlay.Color = comp.Color;
        }
        if (!_overlayMan.HasOverlay<NightVisionOverlay>())
        {
            _overlayMan.AddOverlay(_overlay);
        }
        _lightManager.DrawLighting = false;
    }

    protected override void DeactivateInternal()
    {
        base.DeactivateInternal();
        _overlayMan.RemoveOverlay(_overlay);
        _lightManager.DrawLighting = true;
    }
}
