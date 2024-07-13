using Content.Shared.Starshine.Eye.NightVision.Components; //creater - vladospupuos
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Client.Starshine.Eye.NightVision
{
    public sealed class NightVisionOverlay : Overlay
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly ILightManager _lightManager = default!;


        public override bool RequestScreenTexture => true;
        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        private readonly ShaderInstance _greyscaleShader;
	    public Color Color;

        private NightVisionComponent _nightVisionComponent = default!;

	    public NightVisionOverlay(Color color)
        {
            IoCManager.InjectDependencies(this);
            _greyscaleShader = _prototypeManager.Index<ShaderPrototype>("GreyscaleFullscreen").InstanceUnique();

            Color = color;
        }
        protected override bool BeforeDraw(in OverlayDrawArgs args)
        {
            if (!_entityManager.TryGetComponent(_playerManager.LocalSession?.AttachedEntity, out EyeComponent? eyeComp))
                return false;

            if (args.Viewport.Eye != eyeComp.Eye)
                return false;

            var playerEntity = _playerManager.LocalSession?.AttachedEntity;

            if (playerEntity == null)
                return false;

            if (!_entityManager.TryGetComponent<NightVisionComponent>(playerEntity, out var nightVisionComponent))
                return false;

            _nightVisionComponent = nightVisionComponent;

            var nightVision = _nightVisionComponent.IsOn;

            if (nightVision || !_nightVisionComponent.DrawShadows) // Disable our Night Vision
                return nightVision;
            _lightManager.DrawLighting = true;
            _nightVisionComponent.DrawShadows = false;
            _nightVisionComponent.GraceFrame = true;
            return true;

        }

        protected override void Draw(in OverlayDrawArgs args)
        {
            if (ScreenTexture == null)
                return;

            if (!_nightVisionComponent.GraceFrame)
            {
                _nightVisionComponent.DrawShadows = true; // Enable our Night Vision
                _lightManager.DrawLighting = false;
            }
            else
            {
                _nightVisionComponent.GraceFrame = false;
            }

            _greyscaleShader.SetParameter("SCREEN_TEXTURE", ScreenTexture);

            var worldHandle = args.WorldHandle;
            var viewport = args.WorldBounds;
            worldHandle.UseShader(_greyscaleShader);
            worldHandle.DrawRect(viewport, Color);
            worldHandle.UseShader(null);
        }
    }
}
