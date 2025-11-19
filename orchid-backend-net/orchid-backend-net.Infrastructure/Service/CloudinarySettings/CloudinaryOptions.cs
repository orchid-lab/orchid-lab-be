namespace orchid_backend_net.Infrastructure.Service.CloudinarySettings
{
    public class CloudinaryOptions
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        //custom options
        public string DefaultFolder { get; set; } = "OrchidReportImages";
        public bool UseFilename { get; set; }
        public bool UniqueFilename { get; set; }
        public bool Overwrite { get; set; }
    }
}
