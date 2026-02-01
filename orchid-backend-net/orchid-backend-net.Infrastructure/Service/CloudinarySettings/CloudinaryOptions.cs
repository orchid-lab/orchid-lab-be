namespace orchid_backend_net.Infrastructure.Service.CloudinarySettings
{
    public class CloudinaryOptions
    {
        public string CloudName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ApiSecret { get; set; } = string.Empty;

        //custom options
        public string DefaultFolder { get; set; } = "OrchidReportImages";
        public bool UseFilename { get; set; }
        public bool UniqueFilename { get; set; }
        public bool Overwrite { get; set; }
    }
}
