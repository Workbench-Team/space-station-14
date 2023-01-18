using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Server.Disease;
using Content.Server.Disease.Components;
using Content.Server.Body.Systems;
using Content.Server.Body.Components;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.IdentityManagement;
using Robust.Shared.Random;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using System.Threading;
using System.Linq;

namespace Content.Server.ScienceGoggles
{
    /// <summary>
    /// ""
    /// </summary>
    public sealed class ScienceGogglesSystem : EntitySystem
    {
        [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        public override void Initialize()
        {
            base.Initialize();
        }

        private void OnSolutionExamine()
        {

        }

        public sealed class SolutionExamineEvent : EntityEventArgs
        {
            public SolutionExamineEvent()
            {
            }
        }
    }
}