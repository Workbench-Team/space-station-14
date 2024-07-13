using Content.Shared.CCVar;
using Content.Shared.Chat.TypingIndicator;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;

namespace Content.Client.Chat.TypingIndicator;

// Client-side typing system tracks user input in chat box
public sealed class TypingIndicatorSystem : SharedTypingIndicatorSystem
{
    [Dependency] private readonly IGameTiming _time = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private readonly TimeSpan _typingTimeout = TimeSpan.FromSeconds(2);
    private TimeSpan _lastTextChange;
    private TypingIndicatorState State;
    private bool isChatBoxActive;

    public override void Initialize()
    {
        base.Initialize();

        Subs.CVar(_cfg, CCVars.ChatShowTypingIndicator, OnShowTypingChanged);
    }

    public void ClientFocusChat()
    {
        // don't update it if player don't want to show typing indicator
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        isChatBoxActive = true;

        // client typed something - show typing indicator
        ClientUpdateTyping(TypingIndicatorState.Thinking);
        _lastTextChange = _time.CurTime;
    }

    public void ClientUnFocusChat()
    {
        isChatBoxActive = false;

        ClientUpdateTyping(TypingIndicatorState.None);
    }

    public void ClientChangedChatText()
    {
        // don't update it if player don't want to show typing indicator
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client typed something - show typing indicator
        ClientUpdateTyping(TypingIndicatorState.Typing);
        _lastTextChange = _time.CurTime;
    }

    public void ClientChangedChatTextQuestion()
    {
        // don't update it if player don't want to show typing indicator
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client typed something - show typing indicator
        ClientUpdateTyping(TypingIndicatorState.TypingQuestion);
        _lastTextChange = _time.CurTime;
    }

    public void ClientChangedChatTextAction()
    {
        // don't update it if player don't want to show typing indicator
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client typed something - show typing indicator
        ClientUpdateTyping(TypingIndicatorState.TypingAction);
        _lastTextChange = _time.CurTime;
    }

    public void ClientSubmittedChatText()
    {
        // don't update it if player don't want to show typing
        if (!_cfg.GetCVar(CCVars.ChatShowTypingIndicator))
            return;

        // client submitted text - hide typing indicator
        ClientUpdateTyping(TypingIndicatorState.None);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // check if client didn't changed chat text box for a long time and still chatbox is open
        if (State != TypingIndicatorState.None)
        {
            var dif = _time.CurTime - _lastTextChange;
            if (dif > _typingTimeout)
            {
                if(isChatBoxActive)
                {
                    // client didn't typed anything for a long time - change state to thinking
                    ClientUpdateTyping(TypingIndicatorState.Thinking);
                }
            }
        }
    }

    private void ClientUpdateTyping(TypingIndicatorState state)
    {
        if (State == state)
            return;

        State = state;

        // check if player controls any entity.
        if (_playerManager.LocalEntity == null)
            return;

        RaisePredictiveEvent(new TypingChangedEvent(state));
    }

    private void OnShowTypingChanged(bool showTyping)
    {
        // hide typing indicator immediately if player don't want to show it anymore
        if (!showTyping)
        {
            ClientUpdateTyping(TypingIndicatorState.None);
        }
    }
}
