using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace Content.Server.Arumoon.BansNotifications
{

    /// <summary>
    /// Listen game events and send notifications to Discord
    /// </summary>
    public interface IBansNotificationsSystem
    {
        void RaiseLocalBanEvent(string username, DateTimeOffset? expires, string reason);

        void RaiseLocalJobBanEvent(string username, DateTimeOffset? expires, JobPrototype job, string reason);
        void RaiseLocalDepartmentBanEvent(string username, DateTimeOffset? expires, DepartmentPrototype department, string reason);
    }

    public sealed class BansNotificationsSystem : EntitySystem, IBansNotificationsSystem
    {
        [Dependency] private readonly IConfigurationManager _config = default!;
        private ISawmill _sawmill = default!;
        private readonly HttpClient _httpClient = new();
        private string _webhookUrl = String.Empty;

        public override void Initialize()
        {
            SubscribeLocalEvent<BanEvent>(OnBan);
            SubscribeLocalEvent<JobBanEvent>(OnJobBan);
            SubscribeLocalEvent<DepartmentBanEvent>(OnDepartmentBan);
            _config.OnValueChanged(CCVars.DiscordBanWebhook, value => _webhookUrl = value, true);
        }

        public void RaiseLocalBanEvent(string username, DateTimeOffset? expires, string reason)
        {
            RaiseLocalEvent(new BanEvent(username, expires, reason));
        }

        public void RaiseLocalJobBanEvent(string username, DateTimeOffset? expires, JobPrototype job, string reason)
        {
            RaiseLocalEvent(new JobBanEvent(username, expires, job, reason));
        }

        public void RaiseLocalDepartmentBanEvent(string username, DateTimeOffset? expires, DepartmentPrototype department, string reason)
        {
            RaiseLocalEvent(new DepartmentBanEvent(username, expires, department, reason));
        }

        private async void SendDiscordMessage(WebhookPayload payload)
        {
            var request = await _httpClient.PostAsync(_webhookUrl,
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            var content = await request.Content.ReadAsStringAsync();
            if (!request.IsSuccessStatusCode)
            {
                _sawmill.Log(LogLevel.Error, $"Discord returned bad status code when posting message: {request.StatusCode}\nResponse: {content}");
                return;
            }
        }

        public void OnBan(BanEvent e)
        {
            if (String.IsNullOrEmpty(_webhookUrl))
                return;

            var payload = new WebhookPayload();
            var expires = e.Expires == null ? Loc.GetString("discord-permanent") : Loc.GetString("discord-expires-at", ("date", e.Expires));
            var text = Loc.GetString("discord-ban-msg",
                ("username", e.Username),
                ("expires", expires),
                ("reason", e.Reason));

            payload.Content = text;

            SendDiscordMessage(payload);
        }

        public void OnJobBan(JobBanEvent e)
        {
            if (String.IsNullOrEmpty(_webhookUrl))
                return;

            var payload = new WebhookPayload();
            var expires = e.Expires == null ? Loc.GetString("discord-permanent") : Loc.GetString("discord-expires-at", ("date", e.Expires));
            var text = Loc.GetString("discord-jobban-msg",
                ("username", e.Username),
                ("role", e.Job.LocalizedName),
                ("expires", expires),
                ("reason", e.Reason));

            payload.Content = text;
            SendDiscordMessage(payload);
        }

        public void OnDepartmentBan(DepartmentBanEvent e)
        {
            if (String.IsNullOrEmpty(_webhookUrl))
                return;

            var payload = new WebhookPayload();
            var departamentLocName = Loc.GetString(string.Concat("department-", e.Department.ID));
            var expires = e.Expires == null ? Loc.GetString("discord-permanent") : Loc.GetString("discord-expires-at", ("date", e.Expires));
            var text = Loc.GetString("discord-departmentban-msg",
                ("username", e.Username),
                ("department", departamentLocName),
                ("expires", expires),
                ("reason", e.Reason));

            payload.Content = text;
            SendDiscordMessage(payload);
        }

        private struct WebhookPayload
        {
            [JsonPropertyName("content")]
            public string Content { get; set; } = "";

            public Dictionary<string, string[]> AllowedMentions { get; set; } =
                new()
                {
                    { "parse", Array.Empty<string>() }
                };

            public WebhookPayload() {}
        }
    }
}
