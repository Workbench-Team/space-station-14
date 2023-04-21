using Content.Shared.Damage;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.FixedPoint;
using Content.Shared.Disease.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.EntityHealthBar;

/// <summary>
/// Yeah a lot of this is duplicated from doafters.
/// Not much to be done until there's a generic HUD system
/// </summary>
public sealed class EntityHealthBarOverlay : Overlay
{
    private readonly IEntityManager _entManager;
    private readonly SharedTransformSystem _transform;
    private readonly MobStateSystem _mobStateSystem;
    private readonly MobThresholdSystem _mobThresholdSystem;
    private readonly Texture _barTexture;
    private readonly Texture _lifeStateTexture;
    private readonly Texture _sickStateTexture;
    private readonly Texture _deadStateTexture;
    private readonly ShaderInstance _shader;
    public override OverlaySpace Space => OverlaySpace.WorldSpaceBelowFOV;
    public string? DamageContainer;

    public EntityHealthBarOverlay(IEntityManager entManager, IPrototypeManager protoManager)
    {
        _entManager = entManager;
        _transform = _entManager.EntitySysManager.GetEntitySystem<SharedTransformSystem>();
        _mobStateSystem = _entManager.EntitySysManager.GetEntitySystem<MobStateSystem>();
        _mobThresholdSystem = _entManager.EntitySysManager.GetEntitySystem<MobThresholdSystem>();

        var sprite = new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/health_bar.rsi"), "icon");
        _barTexture = _entManager.EntitySysManager.GetEntitySystem<SpriteSystem>().Frame0(sprite);

        var life_state_sprite = new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/health_state.rsi"), "life_state");
        _lifeStateTexture = _entManager.EntitySysManager.GetEntitySystem<SpriteSystem>().Frame0(life_state_sprite);
        var sick_state_sprite = new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/health_state.rsi"), "sick_state");
        _sickStateTexture = _entManager.EntitySysManager.GetEntitySystem<SpriteSystem>().Frame0(sick_state_sprite);
        var dead_state_sprite = new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/health_state.rsi"), "dead_state");
        _deadStateTexture = _entManager.EntitySysManager.GetEntitySystem<SpriteSystem>().Frame0(dead_state_sprite);

        _shader = protoManager.Index<ShaderPrototype>("unshaded").Instance();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.WorldHandle;
        var rotation = args.Viewport.Eye?.Rotation ?? Angle.Zero;
        var spriteQuery = _entManager.GetEntityQuery<SpriteComponent>();
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();

        const float scale = 1f;
        var scaleMatrix = Matrix3.CreateScale(new Vector2(scale, scale));
        var rotationMatrix = Matrix3.CreateRotation(-rotation);
        handle.UseShader(_shader);

        foreach (var (thresholds, mob, dmg) in _entManager.EntityQuery<MobThresholdsComponent, MobStateComponent, DamageableComponent>(true))
        {
            if (!xformQuery.TryGetComponent(mob.Owner, out var xform) ||
                xform.MapID != args.MapId)
            {
                continue;
            }

            if (DamageContainer != null && dmg.DamageContainerID != DamageContainer)
                continue;

            var worldPosition = _transform.GetWorldPosition(xform);
            var worldMatrix = Matrix3.CreateTranslation(worldPosition);

            Matrix3.Multiply(scaleMatrix, worldMatrix, out var scaledWorld);
            Matrix3.Multiply(rotationMatrix, scaledWorld, out var matty);

            handle.SetTransform(matty);

            float yOffset;
            float xIconOffset;
            float yIconOffset;
            if (spriteQuery.TryGetComponent(mob.Owner, out var sprite))
            {
                yOffset = sprite.Bounds.Height + 10f;
                yIconOffset = sprite.Bounds.Height + 11f;
                xIconOffset = sprite.Bounds.Width + 11f;
            }
            else
            {
                yOffset = 1f;
                yIconOffset = 1f;
                xIconOffset = 1f;
            }

            var position = new Vector2(-_barTexture.Width / 2f / EyeManager.PixelsPerMeter,
                yOffset / EyeManager.PixelsPerMeter);

            // Draw the underlying bar texture
            handle.DrawTexture(_barTexture, position);

            // Draw state icon
            Texture current_state;
            if (_entManager.TryGetComponent<DiseasedComponent>(mob.Owner, out var bebra) && _mobStateSystem.IsAlive(mob.Owner, mob))
                current_state = _sickStateTexture; //TryComp<DiseasedComponent>
            else if (_mobStateSystem.IsAlive(mob.Owner, mob))
                current_state = _lifeStateTexture;
            else
                current_state = _deadStateTexture;

            var icon_position = new Vector2(xIconOffset / EyeManager.PixelsPerMeter,
                yIconOffset / EyeManager.PixelsPerMeter);

            handle.DrawTexture(current_state, icon_position);

            // we are all progressing towards death every day
            (float ratio, bool inCrit) deathProgress = CalcProgress(mob.Owner, mob, dmg, thresholds);

            var color = GetProgressColor(deathProgress.ratio, deathProgress.inCrit);

            // Hardcoded width of the progress bar because it doesn't match the texture.
            const float startX = 2f;
            const float endX = 22f;

            var xProgress = (endX - startX) * deathProgress.ratio + startX;

            var box = new Box2(new Vector2(startX, 2f) / EyeManager.PixelsPerMeter, new Vector2(xProgress, 5f) / EyeManager.PixelsPerMeter);
            box = box.Translated(position);
            handle.DrawRect(box, color);
        }

        handle.UseShader(null);
        handle.SetTransform(Matrix3.Identity);
    }

    /// <summary>
    /// Returns a ratio between 0 and 1, and whether the entity is in crit.
    /// </summary>
    private (float, bool) CalcProgress(EntityUid uid, MobStateComponent component, DamageableComponent dmg, MobThresholdsComponent thresholds)
    {
        if (_mobStateSystem.IsAlive(uid, component))
        {
            if (!_mobThresholdSystem.TryGetThresholdForState(uid, MobState.Critical, out var threshold, thresholds))
                return (1, false);

            var ratio = 1 - ((FixedPoint2)(dmg.TotalDamage / threshold)).Float();
            return (ratio, false);
        }

        if (_mobStateSystem.IsCritical(uid, component))
        {
            if (!_mobThresholdSystem.TryGetThresholdForState(uid, MobState.Critical, out var critThreshold, thresholds) ||
                !_mobThresholdSystem.TryGetThresholdForState(uid, MobState.Dead, out var deadThreshold, thresholds))
            {
                return (1, true);
            }

            var ratio = 1 -
                    ((dmg.TotalDamage - critThreshold) /
                    (deadThreshold - critThreshold)).Value.Float();

            return (ratio, true);
        }

        return (0, true);
    }

    public static Color GetProgressColor(float progress, bool crit)
    {
        if (progress >= 1.0f)
        {
            return new Color(0f, 1f, 0f);
        }
        // lerp
        if (!crit)
        {
            var hue = (5f / 18f) * progress;
            return Color.FromHsv((hue, 1f, 0.75f, 1f));
        } else
        {
            return Color.Red;
        }
    }
}
