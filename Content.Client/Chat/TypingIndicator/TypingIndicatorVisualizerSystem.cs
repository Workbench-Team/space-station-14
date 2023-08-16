using Content.Shared.Chat.TypingIndicator;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client.Chat.TypingIndicator;

public sealed class TypingIndicatorVisualizerSystem : VisualizerSystem<TypingIndicatorComponent>
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    protected override void OnAppearanceChange(EntityUid uid, TypingIndicatorComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!_prototypeManager.TryIndex<TypingIndicatorPrototype>(component.Prototype, out var proto))
        {
            Log.Error($"Unknown typing indicator id: {component.Prototype}");
            return;
        }

        AppearanceSystem.TryGetData<TypingIndicatorState>(uid, TypingIndicatorVisuals.State, out var TypingState, args.Component);
        var layerExists = args.Sprite.LayerMapTryGet(TypingIndicatorLayers.Base, out var layer);
        if (!layerExists)
            layer = args.Sprite.LayerMapReserveBlank(TypingIndicatorLayers.Base);

        args.Sprite.LayerSetRSI(layer, proto.SpritePath);
        switch (TypingState)
        {
            case TypingIndicatorState.Typing:
            args.Sprite.LayerSetState(layer, proto.TypingState);
            break;
            case TypingIndicatorState.TypingQuestion:
            args.Sprite.LayerSetState(layer, proto.QuestionState);
            break;
            case TypingIndicatorState.TypingAction:
            args.Sprite.LayerSetState(layer, proto.ActionState);
            break;
            case TypingIndicatorState.Thinking:
            args.Sprite.LayerSetState(layer, proto.ThinkState);
            break;
        }
        args.Sprite.LayerSetShader(layer, proto.Shader);
        args.Sprite.LayerSetOffset(layer, proto.Offset);
        args.Sprite.LayerSetVisible(layer, TypingState != TypingIndicatorState.None);
    }
}
