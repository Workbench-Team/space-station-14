using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Chat.TypingIndicator;

/// <summary>
///     Prototype to store chat typing indicator visuals.
/// </summary>
[Prototype("typingIndicator")]
public sealed class TypingIndicatorPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField("spritePath")]
    public ResPath SpritePath = new("/Textures/Effects/speech.rsi");

    [DataField("typingState", required: true)]
    public string TypingState = default!;

    [DataField("questionState", required: true)]
    public string QuestionState = default!;

    [DataField("actionState", required: true)]
    public string ActionState = default!;

    [DataField("thinkState", required: true)]
    public string ThinkState = default!;

    [DataField("offset")]
    public Vector2 Offset = new(0.5f, 0.5f);

    [DataField("shader")]
    public string Shader = "unshaded";

}
