namespace orchid_backend_net.Infrastructure.Service.GmailSettings
{
    public class GmailOptions
    {
        public const string GmailOptionsKey = "GmailOptions";
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
