using Content.Shared.Communications;

namespace Content.Server.Communications
{
    [RegisterComponent]
    public sealed partial class CommunicationsConsoleAnnounceComponent : SharedCommunicationsConsoleComponent
    {
        /// <summary>
        /// Starshine Announce dictionary
        /// </summary>
        public List<string> JobSpecialAnnounceList =
        [
            "Captain",
            "ChiefEngineer",
            "ChiefMedicalOfficer",
            "HeadOfPersonnel",
            "HeadOfSecurity",
            "ResearchDirector",
        ];
    }
}
