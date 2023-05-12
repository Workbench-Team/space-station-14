using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.AlertLevel
{

    public sealed class AlertLevelChangedEvent : EntityEventArgs
    {
        public EntityUid Station { get; }
        public string AlertLevel { get; }

        public AlertLevelChangedEvent(EntityUid station, string alertLevel)
        {
            Station = station;
            AlertLevel = alertLevel;
        }
    }
}
