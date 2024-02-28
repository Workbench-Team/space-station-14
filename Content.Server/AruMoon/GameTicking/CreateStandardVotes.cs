using Content.Server.Voting.Managers;
using Content.Shared.CCVar;
using Content.Shared.Voting;

namespace Content.Server.GameTicking;

public sealed partial class GameTicker
{
    public void CreateStandardVotes()
    {
        if (_cfg.GetCVar(CCVars.OnLobbyCreateVotesEnabled) && _playerManager.PlayerCount != 0 && LobbyEnabled)
        {
            _sawmill.Info("Creating standard votes!");
            SendServerMessage(Loc.GetString("game-ticker-standard-votes"));
            var mgr = IoCManager.Resolve<IVoteManager>();
            mgr.CreateStandardVote(null, StandardVoteType.Preset);
            mgr.CreateStandardVote(null, StandardVoteType.Map);
        }
    }
}
