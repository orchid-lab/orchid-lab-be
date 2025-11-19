namespace orchid_backend_net.Infrastructure.Service.GmailSettings
{
    public class GmailOptions
    {
        public const string GmailOptionsKey = "GmailOptions";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}
